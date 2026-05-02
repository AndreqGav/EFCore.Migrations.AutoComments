namespace EFCore.Migrations.AutoComments.Tests.Models.Schema;

/// <summary>
/// Invoice in the domain schema.
/// </summary>
public class DomainInvoice
{
    /// <summary>
    /// Invoice identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Invoice number.
    /// </summary>
    public string Number { get; set; }
}

/// <summary>
/// Invoice in the billing schema.
/// </summary>
public class BillingInvoice
{
    /// <summary>
    /// Invoice identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Invoice amount.
    /// </summary>
    public decimal Amount { get; set; }
}
