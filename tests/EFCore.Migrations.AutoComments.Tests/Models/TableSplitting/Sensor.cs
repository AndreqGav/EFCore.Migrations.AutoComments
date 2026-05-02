namespace EFCore.Migrations.AutoComments.Tests.Models.TableSplitting;

/// <summary>
/// Basic sensor information.
/// </summary>
public class SensorMain
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Data in JSON format.
    /// </summary>
    public string Data { get; set; }
}

/// <summary>
/// Technical sensor details.
/// </summary>
public class SensorDetail
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Data in JSON format.
    /// </summary>
    public string Data { get; set; }
}