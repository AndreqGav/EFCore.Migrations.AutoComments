namespace EFCore.Migrations.AutoComments.Tests.Models.TableSplitting;

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

    public ProductDetails Details { get; set; }
}

/// <summary>
/// Product details.
/// </summary>
public class ProductDetails
{
    /// <summary>
    /// Product identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Product description.
    /// </summary>
    public string Description { get; set; }
}