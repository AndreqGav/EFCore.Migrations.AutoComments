using System;
using EFCore.Migrations.AutoComments.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;

namespace EFCore.Migrations.AutoComments.Conventions;

/// <summary>
/// Adds annotations for properties that require enum value descriptions in comments.
/// </summary>
internal class AutoCommentEnumDescriptionConvention : IModelFinalizingConvention
{
    private readonly bool _allEnumsHasAutoCommentDescription;

    private readonly ILogger<AutoCommentEnumDescriptionConvention> _logger;

    public const string Name = "AutoCommentEnumDescription";

    public AutoCommentEnumDescriptionConvention(bool allEnumsHasAutoCommentDescription,
        ILogger<AutoCommentEnumDescriptionConvention> logger)
    {
        _allEnumsHasAutoCommentDescription = allEnumsHasAutoCommentDescription;
        _logger = logger;
    }

    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        _logger.LogDebug("AutoCommentEnumDescription: starting model processing");

        var entityTypes = modelBuilder.Metadata.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            _logger.LogDebug("AutoCommentEnumDescription: processing entity {Entity}", entityType.ClrType.Name);

            foreach (var property in entityType.GetProperties())
            {
                TrySetAutoCommentEnumDescriptionAnnotation(property);
            }
        }

        _logger.LogDebug("AutoCommentEnumDescription: model processing complete");
    }

    private void TrySetAutoCommentEnumDescriptionAnnotation(IConventionProperty property)
    {
        var memberInfo = property.PropertyInfo;
        if (memberInfo == null)
        {
            return;
        }

        if (_allEnumsHasAutoCommentDescription)
        {
            var propType = property.PropertyInfo?.PropertyType;

            if (propType?.IsEnum == true)
            {
                var ignoreAutoEnumComment = Attribute.GetCustomAttribute(memberInfo, typeof(IgnoreAutoCommentEnumDescriptionAttribute));

                if (ignoreAutoEnumComment is null)
                {
                    _logger.LogDebug("AutoCommentEnumDescription: annotating {Entity}.{Property} — enum description enabled",
                        property.DeclaringType.ClrType.Name, property.Name);

                    property.Builder.AddEnumDescriptionComment();
                }
                else
                {
                    _logger.LogDebug(
                        "AutoCommentEnumDescription: skipping {Entity}.{Property} — IgnoreAutoCommentEnumDescription attribute",
                        property.DeclaringType.ClrType.Name, property.Name);
                }
            }
        }
        else
        {
            var autoEnumComment = Attribute.GetCustomAttribute(memberInfo, typeof(AutoCommentEnumDescriptionAttribute));

            if (autoEnumComment is not null)
            {
                _logger.LogDebug("AutoCommentEnumDescription: annotating {Entity}.{Property} — AutoCommentEnumDescription attribute",
                    property.DeclaringType.ClrType.Name, property.Name);

                property.Builder.AddEnumDescriptionComment();
            }
        }
    }
}