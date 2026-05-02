namespace EFCore.Migrations.AutoComments.Tests.Models.Inheritance;

internal abstract class EntityBase
{
    public int Id { get; set; }
}

/// <summary>
/// Base payment type.
/// </summary>
internal abstract class PaymentBase : EntityBase
{
    /// <summary>
    /// Amount.
    /// </summary>
    public decimal Amount { get; set; }
}

/// <summary>
/// Payment A.
/// </summary>
internal class PaymentA : PaymentBase
{
    /// <summary>
    /// Extra details A.
    /// </summary>
    public string Extra1 { get; set; }
    
    /// <summary>
    /// Shared details.
    /// </summary>
    public string ExtraShared { get; set; }
}

/// <summary>
/// Payment B.
/// </summary>
internal class PaymentB : PaymentBase
{
    /// <summary>
    /// Extra details B.
    /// </summary>
    public string Extra2 { get; set; }
    
    /// <summary>
    /// Shared details.
    /// </summary>
    public string ExtraShared { get; set; }
}