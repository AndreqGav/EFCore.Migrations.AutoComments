namespace EFCore.Migrations.AutoComments.Tests.Models.Json;

/// <summary>
/// Contact information.
/// </summary>
public class ContactInfo
{
    /// <summary>
    /// Phone.
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    public string Email { get; set; }
}

/// <summary>
/// Postal address.
/// </summary>
public class PostalAddress
{
    /// <summary>
    /// Street.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Contact.
    /// </summary>
    public ContactInfo Contact { get; set; }
}

/// <summary>
/// Employee.
/// </summary>
public class Employee
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Contact details.
    /// </summary>
    public ContactInfo Contact { get; set; }
}

/// <summary>
/// Contractor.
/// </summary>
public class Contractor
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Contact details.
    /// </summary>
    public ContactInfo Contact { get; set; }
}

/// <summary>
/// Staff.
/// </summary>
public class Staff
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Home contact details.
    /// </summary>
    public ContactInfo HomeContact { get; set; }

    /// <summary>
    /// Work contact details.
    /// </summary>
    public ContactInfo WorkContact { get; set; }
}

/// <summary>
/// Person.
/// </summary>
public class Person
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Home address.
    /// </summary>
    public PostalAddress HomeAddress { get; set; }
}

/// <summary>
/// Product.
/// </summary>
public class Product
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Specification.
    /// </summary>
    public ProductSpec Spec { get; set; }
}

/// <summary>
/// Product specification.
/// </summary>
public class ProductSpec
{
    /// <summary>
    /// Specification name.
    /// </summary>
    public string Name { get; set; }
}