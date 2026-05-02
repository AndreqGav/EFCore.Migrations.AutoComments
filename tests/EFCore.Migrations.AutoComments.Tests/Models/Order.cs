namespace EFCore.Migrations.AutoComments.Tests.Models;

/// <summary>
/// Customer order.
/// </summary>
public class Order
{
    /// <summary>
    /// Order identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Order number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    ///      Total order amount.       
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Order confirmation status.
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    /// Order status.
    /// </summary>
    [AutoCommentEnumDescription]
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Order category.
    /// </summary>
    public OrderCategory Category { get; set; }

    /// <summary>
    /// Delivery method.
    /// </summary>
    [IgnoreAutoCommentEnumDescription]
    public DeliveryMethod DeliveryMethod { get; set; }
}

/// <summary>
/// Order catalog view.
/// </summary>
public class OrderCatalogView
{
    /// <summary>
    /// Order number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Total order amount.
    /// </summary>
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Order catalog SQL query.
/// </summary>
public class OrderCatalogSqlQuery
{
    /// <summary>
    /// Order number.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Total order amount.
    /// </summary>
    public decimal TotalAmount { get; set; }
}

/// <summary>
/// Order status.
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Active, awaiting fulfillment.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Completed, delivered to the customer.
    /// </summary>
    Completed = 1,

    /// <summary>
    /// Cancelled, refund issued.
    /// </summary>
    Cancelled = 2,
}

/// <summary>
/// Order category.
/// </summary>
public enum OrderCategory
{
    /// <summary>
    /// Clothing.
    /// </summary>
    Clothing,

    /// <summary>
    /// Books.
    /// </summary>
    Books,

    /// <summary>
    /// Toys.
    /// </summary>
    Toys,
}

/// <summary>
/// Order delivery method.
/// </summary>
public enum DeliveryMethod
{
    /// <summary>
    /// Pickup from a pickup point.
    /// </summary>
    Pickup,

    /// <summary>
    /// Courier delivery to the door.
    /// </summary>
    Courier,

    /// <summary>
    /// Postal service.
    /// </summary>
    Post,
}