namespace PropertyMapper.Tests;

#region Simple Types

public class SimpleSource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SimpleTarget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

#endregion

#region Nullable Types

public class NullableSource
{
    public int? NullableInt { get; set; }
    public double? NullableDouble { get; set; }
    public DateTime? NullableDate { get; set; }
    public int RequiredInt { get; set; }
}

public class NullableTarget
{
    public int? NullableInt { get; set; }
    public double? NullableDouble { get; set; }
    public DateTime? NullableDate { get; set; }
    public int? RequiredInt { get; set; }
}

public class ValueTarget
{
    public int NullableInt { get; set; }
    public double NullableDouble { get; set; }
    public DateTime NullableDate { get; set; }
}

#endregion

#region Nested Types

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

public class AddressDto
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

public class PersonWithAddress
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public Address Address { get; set; } = new();
}

public class PersonWithAddressDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public AddressDto Address { get; set; } = new();
}

#endregion

#region Structs

public struct PointStruct
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Label { get; set; }
}

public struct Point3DStruct
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }
}

public record struct VectorStruct(double X, double Y, double Z);
public record struct VectorDto(double X, double Y, double Z);

#endregion

#region Records

public record PersonRecord
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
}

public record PersonDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
}

public record OrderRecord
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public record OrderDto
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

#endregion

#region Operator Conversions

public readonly struct Meters
{
    public double Value { get; init; }
    
    public Meters(double value) => Value = value;
    
    public static explicit operator Feet(Meters m) => new(m.Value * 3.28084);
}

public readonly struct Feet
{
    public double Value { get; init; }
    
    public Feet(double value) => Value = value;
    
    public static explicit operator Meters(Feet f) => new(f.Value / 3.28084);
}

public class Measurement
{
    public string Name { get; set; } = string.Empty;
    public Meters Distance { get; set; }
}

public class MeasurementDto
{
    public string Name { get; set; } = string.Empty;
    public Feet Distance { get; set; }
}

#endregion

#region Complex Nested

public class Company
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    public List<Employee> Employees { get; set; } = new();
}

public class Employee
{
    public string Name { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public Address HomeAddress { get; set; } = new();
}

public class CompanyDto
{
    public string Name { get; set; } = string.Empty;
    public AddressDto Address { get; set; } = new();
    public List<EmployeeDto> Employees { get; set; } = new();
}

public class EmployeeDto
{
    public string Name { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public AddressDto HomeAddress { get; set; } = new();
}

#endregion

#region In-Place / Merge Mapping Models

/// <summary>Source with a subset of <see cref="SimpleTarget"/> properties — used to verify unmatched target fields are preserved.</summary>
public class PartialSource
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>First merge source — contributes <c>Id</c> and <c>Name</c>.</summary>
public class MergeSourceA
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>Second (override) merge source — contributes <c>Name</c> (overrides A) and <c>IsActive</c>.</summary>
public class MergeSourceB
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

/// <summary>Merge target — receives properties from both <see cref="MergeSourceA"/> and <see cref="MergeSourceB"/>.</summary>
public class MergeTarget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

#endregion

#region Mismatched Properties

public class SourceWithExtra
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ExtraProperty { get; set; } = string.Empty;
}

public class TargetWithLess
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SourceWithLess
{
    public int Id { get; set; }
}

public class TargetWithExtra
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

#endregion

#region Edge Cases

public class ReadOnlySource
{
    private readonly int _id = 42;
    public int Id => _id;
    public string Name { get; set; } = "Test";
}

public class WriteOnlyTarget
{
    private int _id;
    public int Id { set => _id = value; }
    public string Name { get; set; } = string.Empty;
}

public class InitOnlyRecord
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

#endregion
