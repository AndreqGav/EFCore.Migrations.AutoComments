namespace EFCore.Migrations.AutoComments.Tests.Models;

/// <summary>
/// Base entity.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }
}

/// <summary>
/// Document.
/// </summary>
public class Document : EntityBase
{
    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }
}