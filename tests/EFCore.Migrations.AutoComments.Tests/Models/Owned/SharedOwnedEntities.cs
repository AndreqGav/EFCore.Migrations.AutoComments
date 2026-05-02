namespace EFCore.Migrations.AutoComments.Tests.Models.Owned;

/// <summary>
/// Address.
/// </summary>
public class Address
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
/// Customer.
/// </summary>
public class Customer
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Customer address.
    /// </summary>
    public Address Address { get; set; }
}

/// <summary>
/// Supplier.
/// </summary>
public class Supplier
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Supplier address.
    /// </summary>
    public Address Address { get; set; }
}

/// <summary>
/// Order.
/// </summary>
public class CustomerOrder
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Shipping address.
    /// </summary>
    public Address ShippingAddress { get; set; }

    /// <summary>
    /// Billing address.
    /// </summary>
    public Address BillingAddress { get; set; }
}
