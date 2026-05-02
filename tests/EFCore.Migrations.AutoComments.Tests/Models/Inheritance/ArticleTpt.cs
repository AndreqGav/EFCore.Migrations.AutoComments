namespace EFCore.Migrations.AutoComments.Tests.Models.Inheritance;

/// <summary>
/// Base type in TPT inheritance.
/// </summary>
public class ArticleBase
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }
}

/// <summary>
/// TPT derived type A.
/// </summary>
public class ArticleA : ArticleBase
{
    /// <summary>
    /// Type A specific content.
    /// </summary>
    public string ContentA { get; set; }
}

/// <summary>
/// TPT derived type B.
/// </summary>
public class ArticleB : ArticleBase
{
    /// <summary>
    /// Type B specific content.
    /// </summary>
    public string ContentB { get; set; }
}