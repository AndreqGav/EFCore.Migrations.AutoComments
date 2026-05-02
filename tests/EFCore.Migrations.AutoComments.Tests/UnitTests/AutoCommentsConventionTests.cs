using EFCore.Migrations.AutoComments.Tests.Helpers;
using EFCore.Migrations.AutoComments.Tests.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EFCore.Migrations.AutoComments.Tests.UnitTests;

/// <summary>
/// Tests that the auto-comments convention sets table and column comments based on XML documentation.
/// </summary>
public class AutoCommentsConventionTests
{
    private static DbContextOptions<AutoCommentsContext> BuildOptions(bool globalEnumDescriptions = false)
    {
        var builder = new DbContextOptionsBuilder<AutoCommentsContext>()
            .UseSqlite("Data Source=unit_tests.db")
            .UseAutoComments(options =>
            {
                if (globalEnumDescriptions)
                {
                    options.AddEnumDescriptions();
                }
            });

        return builder.Options;
    }

    private static string GetTableComment<TEntity>(DbContext context)
        => ModelAccessor.GetModel(context).FindEntityType(typeof(TEntity))!.GetComment();

    private static string GetColumnComment<TEntity>(DbContext context, string propertyName)
        => ModelAccessor.GetModel(context)
            .FindEntityType(typeof(TEntity))!
            .FindProperty(propertyName)!
            .GetComment();

    [Fact]
    public void AutoComments_Should_SetTableComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetTableComment<Order>(context);

        // Assert
        Assert.Equal("Customer order.", comment);
    }

    [Fact]
    public void AutoComments_Should_SetColumnComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var idComment = GetColumnComment<Order>(context, nameof(Order.Id));
        var numberComment = GetColumnComment<Order>(context, nameof(Order.Number));

        // Assert
        Assert.Equal("Order identifier.", idComment);
        Assert.Equal("Order number.", numberComment);
    }

    [Fact]
    public void AutoComments_Should_SetTrimmedColumnComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetColumnComment<Order>(context, nameof(Order.TotalAmount));

        // Assert
        Assert.Equal("Total order amount.", comment);
    }

    [Fact]
    public void AutoComments_Should_AppendEnumValues_ToIntColumn_WhenHasEnumDescriptionsAttribute()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetColumnComment<Order>(context, nameof(Order.Status));

        // Assert
        Assert.StartsWith("Order status.\n", comment);
        Assert.Contains("0 - Active, awaiting fulfillment.", comment);
        Assert.Contains("1 - Completed, delivered to the customer.", comment);
        Assert.Contains("2 - Cancelled, refund issued.", comment);
    }

    [Fact]
    public void AutoComments_ShouldNot_AppendEnumValues_WhenHasNoEnumDescriptionsAttribute()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetColumnComment<Order>(context, nameof(Order.Category));

        // Assert
        Assert.Equal("Order category.", comment);
    }

    [Fact]
    public void AutoComments_Should_AppendEnumValues_ToStringColumn_WhenGlobalEnableEnumDescriptions()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions(globalEnumDescriptions: true));

        // Act
        var comment = GetColumnComment<Order>(context, nameof(Order.Category));

        // Assert
        Assert.StartsWith("Order category.\n", comment);
        Assert.Contains("Clothing - Clothing.", comment);
        Assert.Contains("Books - Books.", comment);
        Assert.Contains("Toys - Toys.", comment);
    }

    [Fact]
    public void AutoComments_ShouldNot_AppendEnumValues_WhenIgnoreAutoCommentEnumDescription()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions(globalEnumDescriptions: true));

        // Act
        var comment = GetColumnComment<Order>(context, nameof(Order.DeliveryMethod));

        // Assert
        Assert.Equal("Delivery method.", comment);
    }

    [Fact]
    public void AutoComments_InheritedProperty_Should_SetColumnComment_FromBaseClassXml()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act + Assert
        Assert.Equal("Identifier.", GetColumnComment<Document>(context, nameof(EntityBase.Id)));
    }

    [Fact]
    public void AutoComments_Should_NotOverwrite_ManualTableComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetTableComment<Blog>(context);

        // Assert
        Assert.Equal("Blog (manual comment)", comment);
    }

    [Fact]
    public void AutoComments_Should_NotOverwrite_ManualPropertyComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetColumnComment<Blog>(context, nameof(Blog.Url));

        // Assert
        Assert.Equal("URL (manual comment)", comment);
    }

    [Fact]
    public void AutoComments_Should_SkipView_AndNotSetComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetTableComment<OrderCatalogView>(context);

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void AutoComments_Should_SkipViewColumns_AndNotSetComments()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetColumnComment<OrderCatalogView>(context, nameof(OrderCatalogView.Number));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void AutoComments_Should_SkipSqlQuery_AndNotSetComment()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetTableComment<OrderCatalogSqlQuery>(context);

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void AutoComments_Should_SkipSqlQueryColumns_AndNotSetComments()
    {
        // Arrange
        using var context = new AutoCommentsContext(BuildOptions());

        // Act
        var comment = GetColumnComment<OrderCatalogSqlQuery>(context, nameof(OrderCatalogSqlQuery.TotalAmount));

        // Assert
        Assert.Null(comment);
    }
}

internal sealed class AutoCommentsContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<Blog> Blogs { get; set; }

    public DbSet<OrderCatalogView> OrderCatalogViews { get; set; }

    public AutoCommentsContext(DbContextOptions<AutoCommentsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // View - the auto-comments convention must skip it.
        modelBuilder.Entity<OrderCatalogView>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("OrderCatalog");
        });

        modelBuilder.Entity<OrderCatalogSqlQuery>(builder =>
        {
            builder.HasNoKey();
            builder.ToSqlQuery("SELECT * FROM \"Orders\"");
        });

        modelBuilder.Entity<Order>(builder => builder.Property(e => e.Category).HasConversion<string>());

        // Manual comment - the convention must not overwrite it.
        modelBuilder.Entity<Blog>(builder =>
        {
            builder.ToTable("Blogs", t => t.HasComment("Blog (manual comment)"));
            builder.Property(b => b.Url).HasComment("URL (manual comment)");
        });

        modelBuilder.Entity<Document>(builder => { builder.HasKey(e => e.Id); });
    }
}