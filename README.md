# EFCore.Migrations.AutoComments

[![NuGet](https://img.shields.io/nuget/v/EFCore.Migrations.AutoComments)](https://www.nuget.org/packages/EFCore.Migrations.AutoComments) [![Downloads](https://img.shields.io/nuget/dt/EFCore.Migrations.AutoComments)](https://www.nuget.org/packages/EFCore.Migrations.AutoComments) [![License](https://img.shields.io/github/license/AndreqGav/EFCore.Migrations.AutoComments)](LICENSE)

Automatically applies database comments to tables and columns based on XML documentation. Comments flow from `<summary>` tags directly into migrations.

## Installation

```
dotnet add package EFCore.Migrations.AutoComments
```

## Prerequisites

Enable XML documentation generation in your project:

```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

## Registration

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options
        .UseNpgsql(...)
        .UseAutoComments());
```

## XML file path resolution

If no file is specified, XML docs are auto-discovered by assembly name.

```csharp
// Auto-discover (default)
options.UseAutoComments();

// Single file
options.UseAutoComments("MyApp.xml");

// Multiple files - merges docs from several assemblies
options.UseAutoComments("MyApp.xml", "SharedLibrary.xml");
```

## Example

```csharp
/// <summary>
/// Represents an animal in the shelter.
/// </summary>
public class Animal
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Animal name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Type of animal.
    /// </summary>
    public AnimalType Type { get; set; }
}

/// <summary>
/// Animal type.
/// </summary>
public enum AnimalType
{
    /// <summary>Dog.</summary>
    Dog,

    /// <summary>Cat.</summary>
    Cat,

    /// <summary>Fish.</summary>
    Fish
}
```

Generated migration comments:

```sql
COMMENT ON TABLE "Animals" IS 'Represents an animal in the shelter.';
COMMENT ON COLUMN "Animals"."Id" IS 'Unique identifier.';
COMMENT ON COLUMN "Animals"."Name" IS 'Animal name.';
COMMENT ON COLUMN "Animals"."Type" IS 'Type of animal.';
```

## Enum descriptions

With `options.UseAutoComments(o => o.AddEnumDescriptions())`, the `Type` column comment becomes:

```
Type of animal.

0 - Dog.
1 - Cat.
2 - Fish.
```

For string-backed enums, the enum member name is used instead of the numeric value:

```
Status.

Active - Active account.
Suspended - Temporarily suspended.
Closed - Account closed.
```

Use `[AutoCommentEnumDescription]` on a property to enable enum descriptions for that property only, or `[IgnoreAutoCommentEnumDescription]` to exclude it when global mode is active.

---

## License

MIT © Андрей Гаврилов 2026

---

> Extracted from [AndreqGav/EFCore.Migrations.Toolkit](https://github.com/AndreqGav/EFCore.Migrations.Toolkit).
