<#
.SYNOPSIS
    PropertyMapper benchmark runner.

.DESCRIPTION
    Wrapper script for BenchmarkDotNet. Builds the project in Release mode,
    runs the selected benchmark (or all of them) and optionally opens the HTML report.

.PARAMETER Benchmark
    Benchmark to run. Shows an interactive menu when omitted.
    Allowed values: All | Simple | Wide | Nested | Record | Struct | Collection | Batch | Clone | FirstCall | Async | Dictionary | Warmup | FieldMask | Context | Statistics

.PARAMETER Quick
    Quick mode (ShortRun) -- results are not fully representative,
    useful for fast functional verification.

.PARAMETER List
    Prints all available benchmark methods and exits without measuring.

.PARAMETER OutputDir
    Target directory for BDN artefacts (HTML / CSV / JSON reports).
    Default: .\results\<yyyy-MM-dd_HH-mm-ss>_<Benchmark>

.PARAMETER NoOpen
    Do not open the HTML report automatically after the run.

.PARAMETER NoBuild
    Skip the "dotnet build -c Release" step (project must already be built).

.EXAMPLE
    .\Run-Benchmarks.ps1
    # Interactive menu

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark All
    # Run all benchmarks (may take over an hour)

.EXAMPLE
    .\Run-Benchmarks.ps1 Simple
    # Run SimpleObjectBenchmark (positional parameter)

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Collection -Quick
    # Quick verification of CollectionBenchmark

.EXAMPLE
    .\Run-Benchmarks.ps1 -List
    # Print available methods without running

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Wide -OutputDir C:\bench\results -NoOpen
    # Save results to a custom directory without opening the report

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Async -Quick
    # Quick verification of async vs parallel mapping (N = 10 / 100 / 1000)

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Dictionary
    # Run MapDictionary vs manual benchmark

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Warmup
    # Cold-path IL-compile cost: Warmup<TIn,TOut> vs WarmupBatch (2 and 4 pairs)

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark FieldMask -Quick
    # Quick hot-path comparison: plain Map vs MapWithMask (1 field / 3 fields)

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Context
    # MapWithContext overhead: guard + lookup, 1 setter, 3 setters vs plain Map

.EXAMPLE
    .\Run-Benchmarks.ps1 -Benchmark Statistics -Quick
    # GetStatistics lock-acquire cost vs lock-free Map hot path
#>

[CmdletBinding()]
param (
    [Parameter(Position = 0)]
    [ValidateSet('All', 'Simple', 'Wide', 'Nested', 'Record', 'Struct', 'Collection', 'Batch', 'Clone', 'FirstCall', 'Async', 'Dictionary', 'Warmup', 'FieldMask', 'Context', 'Statistics', IgnoreCase = $true)]
    [string] $Benchmark,

    [switch] $Quick,
    [switch] $List,
    [string] $OutputDir,
    [switch] $NoOpen,
    [switch] $NoBuild
)

# Running under Windows PowerShell 5.x -- auto-relaunch via pwsh.exe (PowerShell 7+)
if ($PSVersionTable.PSVersion.Major -lt 7) {
    $pwshCmd = Get-Command pwsh -ErrorAction SilentlyContinue
    if (-not $pwshCmd) {
        Write-Warning "This script requires PowerShell 7+. Current version: $($PSVersionTable.PSVersion). Install: https://aka.ms/powershell"
        exit 1
    }
    $relaunchArgs = @('-NoProfile', '-File', $PSCommandPath)
    if ($PSBoundParameters.ContainsKey('Benchmark'))  { $relaunchArgs += '-Benchmark',  $Benchmark }
    if ($PSBoundParameters.ContainsKey('Quick'))      { $relaunchArgs += '-Quick' }
    if ($PSBoundParameters.ContainsKey('List'))       { $relaunchArgs += '-List' }
    if ($PSBoundParameters.ContainsKey('OutputDir'))  { $relaunchArgs += '-OutputDir',  $OutputDir }
    if ($PSBoundParameters.ContainsKey('NoOpen'))     { $relaunchArgs += '-NoOpen' }
    if ($PSBoundParameters.ContainsKey('NoBuild'))    { $relaunchArgs += '-NoBuild' }
    & pwsh @relaunchArgs
    exit $LASTEXITCODE
}

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# ------------------------------------------------------------------------------
# Paths
# ------------------------------------------------------------------------------

$ProjectDir  = Resolve-Path (Join-Path $PSScriptRoot '..')
$ProjectFile = Join-Path $ProjectDir 'PropertyMapper.Benchmarks.csproj'
$ResultsRoot = Join-Path $ProjectDir 'results'

# ------------------------------------------------------------------------------
# Output helpers
# ------------------------------------------------------------------------------

function Write-Banner {
    Write-Host ''
    Write-Host '  +=========================================================+' -ForegroundColor Cyan
    Write-Host '  |          PropertyMapper  --  Benchmark Runner           |' -ForegroundColor Cyan
    Write-Host '  +=========================================================+' -ForegroundColor Cyan
    Write-Host ''
}

function Write-Section ([string]$text) {
    Write-Host "  $text" -ForegroundColor Cyan
    Write-Host "  $('-' * $text.Length)" -ForegroundColor DarkCyan
}

function Write-Step ([string]$text) { Write-Host "  >> $text" -ForegroundColor White }
function Write-Ok   ([string]$text) { Write-Host "  OK $text" -ForegroundColor Green }
function Write-Warn ([string]$text) { Write-Host "  !! $text" -ForegroundColor Yellow }
function Write-Err  ([string]$text) { Write-Host "  XX $text" -ForegroundColor Red }
function Write-Info ([string]$text) { Write-Host "   . $text" -ForegroundColor DarkGray }

# ------------------------------------------------------------------------------
# Benchmark registry
# ------------------------------------------------------------------------------

$Registry = [ordered]@{
    'Simple'     = @{ Filter = '*SimpleObject*';  Desc = 'Flat class, 4 properties        - hot-path baseline' }
    'Wide'       = @{ Filter = '*WideObject*';    Desc = 'Flat class, 12 properties       - property scaling'  }
    'Nested'     = @{ Filter = '*Nested*';        Desc = 'Nested objects                  - PersonSource -> PersonTarget' }
    'Record'     = @{ Filter = '*Record*';        Desc = 'C# records                      - OrderSource -> OrderTarget'   }
    'Struct'     = @{ Filter = '*Struct*';        Desc = 'Value types / structs            - MapValueType<>'              }
    'Collection' = @{ Filter = '*Collection*';    Desc = 'List<T> mapping                 - params: 10 / 100 / 1000'     }
    'Batch'      = @{ Filter = '*Batch*';         Desc = 'MapBatch vs MapBatchInPlace      - params: 10 / 100 / 1000'      }
    'Clone'      = @{ Filter = '*Clone*';         Desc = 'Clone<T> self-copy              - FlatSource round-trip'         }
    'FirstCall'  = @{ Filter = '*FirstCall*';     Desc = 'Cold-start                      - mapper creation + first Map()'}
    'Async'      = @{ Filter = '*Async*';         Desc = 'Async vs parallel mapping        - params: 10 / 100 / 1000'       }
    'Dictionary' = @{ Filter = '*Dictionary*';    Desc = 'MapDictionary vs manual           - params: 10 / 100 / 1000'       }
    'Warmup'     = @{ Filter = '*Warmup*';        Desc = 'Cold-path compile cost           - Warmup vs WarmupBatch'           }
    'FieldMask'  = @{ Filter = '*FieldMask*';     Desc = 'MapWithMask overhead             - Map vs mask (1 / 3 fields)'     }
    'Context'    = @{ Filter = '*Context*';      Desc = 'MapWithContext overhead          - guard + lookup + 1 / 3 setters'  }
    'Statistics' = @{ Filter = '*Statistics*';   Desc = 'GetStatistics lock-acquire cost  - lock vs lock-free Map hot path'   }
}

# ------------------------------------------------------------------------------
# Prerequisites
# ------------------------------------------------------------------------------

function Assert-Prerequisites {
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        Write-Err '.NET SDK not found in PATH.'
        exit 1
    }
    if (-not (Test-Path $ProjectFile)) {
        Write-Err 'Project file not found:'
        Write-Err "    $ProjectFile"
        exit 1
    }
    Write-Info ".NET SDK  : $(dotnet --version)"
    Write-Info "Project   : $ProjectFile"
    Write-Host ''
}

# ------------------------------------------------------------------------------
# Release build
# ------------------------------------------------------------------------------

function Invoke-ReleaseBuild {
    Write-Section 'Building in Release configuration'
    Write-Step "dotnet build -c Release $ProjectFile"
    Write-Host ''

    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    dotnet build $ProjectFile -c Release --nologo -v q
    $sw.Stop()

    if ($LASTEXITCODE -ne 0) {
        Write-Host ''
        Write-Err 'Build failed. Benchmarks will not run.'
        exit 1
    }

    Write-Host ''
    Write-Ok ("Build completed in {0:0.0} s." -f $sw.Elapsed.TotalSeconds)
    Write-Host ''
}

# ------------------------------------------------------------------------------
# Interactive menu
# ------------------------------------------------------------------------------

function Invoke-Menu {
    Write-Section 'Available benchmarks'
    Write-Host ''

    $keys = @($Registry.Keys)
    for ($i = 0; $i -lt $keys.Count; $i++) {
        $key  = $keys[$i]
        $desc = $Registry[$key].Desc
        Write-Host ("    [{0}] {1,-12}  {2}" -f ($i + 1), $key, $desc) -ForegroundColor White
    }
    Write-Host ''
    Write-Host ("    [0] {0,-12}  {1}" -f 'All', 'Run all benchmarks  !!  may take over an hour') -ForegroundColor Yellow
    Write-Host ''

    $maxIdx = $keys.Count
    do {
        Write-Host "  Select [0-$maxIdx]: " -ForegroundColor Cyan -NoNewline
        $raw   = Read-Host
        $num   = 0
        $valid = [int]::TryParse($raw.Trim(), [ref]$num) -and $num -ge 0 -and $num -le $maxIdx
        if (-not $valid) { Write-Warn "Invalid choice -- enter a number between 0 and $maxIdx." }
    } while (-not $valid)

    Write-Host ''
    if ($num -eq 0) { return 'All' }
    return $keys[$num - 1]
}

# ------------------------------------------------------------------------------
# List mode  (--list flat)
# ------------------------------------------------------------------------------

function Invoke-List {
    Write-Section 'Available benchmark methods'
    Write-Info 'dotnet run -c Release -- --list flat'
    Write-Host ''
    dotnet run --project $ProjectFile -c Release --no-build -- --list flat
    Write-Host ''
}

# ------------------------------------------------------------------------------
# Run benchmarks
# ------------------------------------------------------------------------------

function Invoke-Benchmark ([string]$name, [string]$artifactsDir) {
    $filter = if ($name -eq 'All') { '*' } else { $Registry[$name].Filter }

    $dotnetArgs = [System.Collections.Generic.List[string]]@(
        'run', '--project', $ProjectFile,
        '-c', 'Release',
        '--no-build',
        '--',
        '--filter',    $filter,
        '--artifacts', $artifactsDir,
        '--exporters', 'html', 'csv', 'json'
    )

    if ($Quick) {
        $dotnetArgs.AddRange([string[]]@('--job', 'short'))
    }

    Write-Section "Running benchmark: $name"
    Write-Info "Filter    : $filter"
    Write-Info "Results   : $artifactsDir"
    if ($Quick) { Write-Warn 'Quick mode (ShortRun) -- results are not fully representative.' }
    Write-Host ''

    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    & dotnet @dotnetArgs
    $exitCode = $LASTEXITCODE
    $sw.Stop()

    Write-Host ''
    if ($exitCode -eq 0) {
        Write-Ok ("Benchmarks completed in {0:hh\:mm\:ss}." -f $sw.Elapsed)
    } else {
        Write-Err "BenchmarkDotNet exited with code $exitCode."
        exit $exitCode
    }
}

# ------------------------------------------------------------------------------
# Open HTML report
# ------------------------------------------------------------------------------

function Open-HtmlReport ([string]$artifactsDir) {
    $report = Get-ChildItem -Path $artifactsDir -Filter '*.html' -Recurse -ErrorAction SilentlyContinue |
              Sort-Object LastWriteTime -Descending |
              Select-Object -First 1

    if ($report) {
        Write-Ok 'Opening HTML report:'
        Write-Info "    $($report.FullName)"
        Start-Process $report.FullName
    } else {
        Write-Warn 'HTML report not found in the artefacts directory.'
    }
}

# ------------------------------------------------------------------------------
# Results summary from CSV
# ------------------------------------------------------------------------------

function Show-ResultsSummary ([string]$artifactsDir) {
    $csv = Get-ChildItem -Path $artifactsDir -Filter '*-report-full.csv' -Recurse -ErrorAction SilentlyContinue |
           Sort-Object LastWriteTime -Descending |
           Select-Object -First 1

    if (-not $csv) { return }

    Write-Host ''
    Write-Section 'Quick results summary'

    try {
        $rows = Import-Csv -Path $csv.FullName |
                Select-Object Method,
                              @{ N = 'Mean (ns)';  E = { [double]$_.Mean } },
                              @{ N = 'Alloc (B)';  E = { $_.('Allocated (B)') } },
                              Ratio |
                Sort-Object 'Mean (ns)'

        $rows | Format-Table -AutoSize | Out-String | ForEach-Object {
            Write-Host "  $_" -ForegroundColor White
        }
    } catch {
        Write-Warn 'CSV report could not be parsed for summary (this is not a benchmark error).'
    }
}

# ------------------------------------------------------------------------------
# Entry point
# ------------------------------------------------------------------------------

Write-Banner
Assert-Prerequisites

if (-not $NoBuild) {
    Invoke-ReleaseBuild
}

if ($List) {
    Invoke-List
    exit 0
}

if (-not $Benchmark) {
    $Benchmark = Invoke-Menu
}

if (-not $OutputDir) {
    $ts        = Get-Date -Format 'yyyy-MM-dd_HH-mm-ss'
    $OutputDir = Join-Path $ResultsRoot "${ts}_${Benchmark}"
}
$OutputDir = [IO.Path]::GetFullPath($OutputDir)
New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

Invoke-Benchmark -name $Benchmark -artifactsDir $OutputDir

Show-ResultsSummary -artifactsDir $OutputDir

if (-not $NoOpen) {
    Write-Host ''
    Open-HtmlReport -artifactsDir $OutputDir
}

Write-Host ''
Write-Ok 'Done.'
Write-Host ''
