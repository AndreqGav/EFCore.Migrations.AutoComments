namespace EFCore.Migrations.AutoComments.Tests.Models.Owned;

/// <summary>
/// Warehouse.
/// </summary>
public class Warehouse
{
    /// <summary>
    /// Warehouse identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Warehouse name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Address.
    /// </summary>
    public WarehouseAddress Address { get; set; }
}

/// <summary>
/// Warehouse address.
/// </summary>
public class WarehouseAddress
{
    /// <summary>
    /// Street.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// City.
    /// </summary>
    public string City { get; set; }
}

/// <summary>
/// Shipment.
/// </summary>
public class Shipment
{
    /// <summary>
    /// Shipment identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Tracking number.
    /// </summary>
    public string TrackNumber { get; set; }

    /// <summary>
    /// Address.
    /// </summary>
    public ShipmentAddress Address { get; set; }
}

/// <summary>
/// Shipment address.
/// </summary>
public class ShipmentAddress
{
    /// <summary>
    /// Shipping street.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Shipping city.
    /// </summary>
    public string City { get; set; }
}

/// <summary>
/// Employee.
/// </summary>
public class Employee
{
    /// <summary>
    /// Employee identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Employee name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Department.
    /// </summary>
    public Department Department { get; set; }
}

/// <summary>
/// Department.
/// </summary>
public class Department
{
    /// <summary>
    /// Department name.
    /// </summary>
    public string Name { get; set; }
}
