namespace EFCore.Migrations.AutoComments.Tests.Models.TableSplitting;

/// <summary>
/// Contract.
/// </summary>
public class Contract
{
    /// <summary>
    /// Contract identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Contract number.
    /// </summary>
    public string Number { get; set; }

    public ContractDetails Details { get; set; }
}

/// <summary>
/// Contract.
/// </summary>
public class ContractDetails
{
    /// <summary>
    /// Contract identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Contract notes.
    /// </summary>
    public string Notes { get; set; }
}