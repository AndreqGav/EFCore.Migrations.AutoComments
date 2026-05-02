namespace EFCore.Migrations.AutoComments.Tests.Models.Inheritance;

/// <summary>
/// Abstract base type in TPC inheritance.
/// </summary>
public abstract class BlogBase
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }
}

/// <summary>
/// TPC derived type A.
/// </summary>
public class BlogA : BlogBase
{
    /// <summary>
    /// Name A.
    /// </summary>
    public string Name { get; set; }
}

/// <summary>
/// TPC derived type B.
/// </summary>
public class BlogB : BlogBase
{
    /// <summary>
    /// Name B.
    /// </summary>
    public string Name { get; set; }
}