namespace PropertyMapper.Benchmarks.Models;

// ---------------------------------------------------------------------------
// Flat class — 4 properties
// ---------------------------------------------------------------------------

/// <summary>Simple flat source object with 4 scalar properties.</summary>
public class FlatSource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>Simple flat target object mirroring <see cref="FlatSource"/>.</summary>
public class FlatTarget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

// ---------------------------------------------------------------------------
// Wide class — 12 properties
// ---------------------------------------------------------------------------

/// <summary>Wide source object with 12 heterogeneous properties.</summary>
public class WideSource
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Department { get; set; } = string.Empty;
    public int ManagerId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

/// <summary>Wide target object mirroring <see cref="WideSource"/>.</summary>
public class WideTarget
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Department { get; set; } = string.Empty;
    public int ManagerId { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

// ---------------------------------------------------------------------------
// Nested class
// ---------------------------------------------------------------------------

/// <summary>Embedded address used in nested mapping scenarios.</summary>
public class AddressSource
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

/// <summary>Target address mirroring <see cref="AddressSource"/>.</summary>
public class AddressTarget
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

/// <summary>Person with a nested address property.</summary>
public class PersonSource
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public AddressSource Address { get; set; } = new();
}

/// <summary>Target person mirroring <see cref="PersonSource"/>.</summary>
public class PersonTarget
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public AddressTarget Address { get; set; } = new();
}

// ---------------------------------------------------------------------------
// Records
// ---------------------------------------------------------------------------

/// <summary>Immutable order record with 4 init-only properties.</summary>
public record OrderSource
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}

/// <summary>Target order record mirroring <see cref="OrderSource"/>.</summary>
public record OrderTarget
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}

// ---------------------------------------------------------------------------
// Structs
// ---------------------------------------------------------------------------

/// <summary>3-D point value type — 3 float properties.</summary>
public struct PointSource
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

/// <summary>Target point value type mirroring <see cref="PointSource"/>.</summary>
public struct PointTarget
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
