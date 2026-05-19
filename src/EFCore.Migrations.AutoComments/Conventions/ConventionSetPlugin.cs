using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.Logging;

namespace EFCore.Migrations.AutoComments.Conventions;

internal class ConventionSetPlugin : IConventionSetPlugin
{
    private readonly AutoCommentsExtension _extension;

    private readonly ILoggerFactory _loggerFactory;

    public ConventionSetPlugin([NotNull] IDbContextOptions options, [NotNull] ILoggerFactory loggerFactory)
    {
        _extension = options.FindExtension<AutoCommentsExtension>()!;
        _loggerFactory = loggerFactory;
    }

    public ConventionSet ModifyConventions(ConventionSet conventionSet)
    {
        var enumLogger = _loggerFactory.CreateLogger<AutoCommentEnumDescriptionConvention>();
        var enumAnnotationConvention = new AutoCommentEnumDescriptionConvention(_extension.Options.AutoCommentEnumDescriptions, enumLogger);
        conventionSet.ModelFinalizingConventions.Add(enumAnnotationConvention);

        var logger = _loggerFactory.CreateLogger<AutoCommentsConvention>();
        var autoCommentsConvention = new AutoCommentsConvention(_extension.Options, logger);
        conventionSet.ModelFinalizingConventions.Add(autoCommentsConvention);

        return conventionSet;
    }
}