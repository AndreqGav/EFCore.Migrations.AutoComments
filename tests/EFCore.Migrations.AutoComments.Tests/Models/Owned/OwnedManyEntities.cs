using System.Collections.Generic;

namespace EFCore.Migrations.AutoComments.Tests.Models.Owned;

/// <summary>
/// Shopping cart.
/// </summary>
public class ShoppingCart
{
    /// <summary>
    /// Identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Cart items.
    /// </summary>
    public ICollection<CartItem> Items { get; set; }
}

/// <summary>
/// Cart item.
/// </summary>
public class CartItem
{
    /// <summary>
    /// Product name.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Quantity.
    /// </summary>
    public int Quantity { get; set; }
}
