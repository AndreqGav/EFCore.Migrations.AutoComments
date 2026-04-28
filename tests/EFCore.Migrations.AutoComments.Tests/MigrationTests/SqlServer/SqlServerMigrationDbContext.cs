using EFCore.Migrations.AutoComments.Tests.Helpers;
using EFCore.Migrations.AutoComments.Tests.Models;
using EFCore.Migrations.AutoComments.Tests.Models.Inheritance;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Migrations.AutoComments.Tests.MigrationTests.SqlServer;

public class SqlServerMigrationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public DbSet<BlogView> BlogViews { get; set; }

    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer(SqlServerTestDatabase.ConnectionString)
            .UseAutoComments();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("blog_view");
        });

        modelBuilder.Entity<Blog>();

        modelBuilder.Entity<Order>();

        ConfigureTphInheritance(modelBuilder);
    }

    private static void ConfigureTphInheritance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostBase>(builder =>
        {
            builder.HasKey(entity => entity.Id);
        });

        modelBuilder.Entity<PostA>(builder => builder.HasBaseType<PostBase>());
        modelBuilder.Entity<PostB>(builder => builder.HasBaseType<PostBase>());
    }
}