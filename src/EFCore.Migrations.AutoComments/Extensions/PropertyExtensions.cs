using System.Diagnostics.CodeAnalysis;
using EFCore.Migrations.AutoComments.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Migrations.AutoComments.Extensions;

public static class PropertyExtensions
{
    /// <summary>
    /// Marks a property to include enum value descriptions in its comment.
    /// </summary>
    public static void AddEnumDescriptionComment([NotNull] this IConventionPropertyBuilder propertyBuilder)
    {
        var property = propertyBuilder.Metadata;
        property.Builder.HasAnnotation(AutoCommentEnumDescriptionConvention.Name, string.Empty);
    }

    /// <summary>
    /// Checks whether a property is marked for enum description comments.
    /// </summary>
    public static bool HasEnumDescriptionComment([NotNull] this IConventionProperty property)
    {
        return property.FindAnnotation(AutoCommentEnumDescriptionConvention.Name) is not null;
    }
}