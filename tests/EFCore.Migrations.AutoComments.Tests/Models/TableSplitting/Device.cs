namespace EFCore.Migrations.AutoComments.Tests.Models.TableSplitting;

/// <summary>
/// Entity for device monitoring.
/// </summary>
public class DeviceMain
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Current device readiness status.
    /// </summary>
    public string StateInfo { get; set; }

    /// <summary>
    /// Details.
    /// </summary>
    public DeviceDetail Detail { get; set; }
}

/// <summary>
/// Entity for deep diagnostics.
/// </summary>
public class DeviceDetail
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full log of the latest state change.
    /// </summary>
    public string StateInfo { get; set; }
}