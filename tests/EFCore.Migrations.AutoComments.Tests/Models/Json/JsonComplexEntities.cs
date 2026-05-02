namespace EFCore.Migrations.AutoComments.Tests.Models.Json;

/// <summary>
/// Report.
/// </summary>
public class Report
{
    /// <summary>
    /// Report identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Metadata.
    /// </summary>
    public ReportMetadata Metadata { get; set; }
}

/// <summary>
/// Report metadata.
/// </summary>
public class ReportMetadata
{
    /// <summary>
    /// Report title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Report author.
    /// </summary>
    public string Author { get; set; }
}

/// <summary>
/// Ticket.
/// </summary>
public class Ticket
{
    /// <summary>
    /// Ticket identifier.
    /// </summary>
    public int Id { get; set; }

    public SeatInfo Seat { get; set; }
}

/// <summary>
/// Seat information.
/// </summary>
public class SeatInfo
{
    /// <summary>
    /// Row number.
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Seat number.
    /// </summary>
    public int Number { get; set; }
}
