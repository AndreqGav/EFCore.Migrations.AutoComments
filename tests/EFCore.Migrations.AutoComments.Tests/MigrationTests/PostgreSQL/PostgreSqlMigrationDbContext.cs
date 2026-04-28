using EFCore.Migrations.AutoComments.Tests.Helpers;
using EFCore.Migrations.AutoComments.Tests.Models;
using EFCore.Migrations.AutoComments.Tests.Models.Inheritance;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Migrations.AutoComments.Tests.MigrationTests.PostgreSQL;

public class PostgreSqlMigrationDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public DbSet<BlogView> BlogViews { get; set; }

    public DbSet<Order> Orders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(PostgreSqlDatabase.ConnectionString)
            .UseAutoComments(options => options.AddEnumDescriptions());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>();

        modelBuilder.Entity<Order>();

        ConfigureTphInheritance(modelBuilder);
        ConfigureTptInheritance(modelBuilder);
        ConfigureTpcInheritance(modelBuilder);
    }

    private static void ConfigureTphInheritance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostBase>(builder =>
        {
            builder.HasKey(entity => entity.Id);
            builder.UseTphMappingStrategy();
        });

        modelBuilder.Entity<PostA>(builder => builder.HasBaseType<PostBase>());
        modelBuilder.Entity<PostB>(builder => builder.HasBaseType<PostBase>());
    }

    private static void ConfigureTptInheritance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArticleBase>(builder =>
        {
            builder.HasKey(entity => entity.Id);
            builder.UseTptMappingStrategy();
        });

        modelBuilder.Entity<ArticleA>();
        modelBuilder.Entity<ArticleB>();
    }

    private static void ConfigureTpcInheritance(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogBase>(builder =>
        {
            builder.HasKey(entity => entity.Id);
            builder.UseTpcMappingStrategy();
        });

        modelBuilder.Entity<BlogA>();
        modelBuilder.Entity<BlogB>();
    }
}