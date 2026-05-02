using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFCore.Migrations.AutoComments;

public static class DependencyInjection
{
    /// <summary>
    /// Enables auto comments and configures XML documentation files.
    /// If no files are provided, XML documentation is auto-discovered by assembly name.
    /// </summary>
    public static TBuilder UseAutoComments<TBuilder>([NotNull] this TBuilder optionsBuilder, params string[] xmlFiles)
        where TBuilder : DbContextOptionsBuilder
    {
        return optionsBuilder.UseAutoComments(o => o.FromXmlFiles(xmlFiles));
    }

    /// <summary>
    /// Enables auto comments and applies custom options.
    /// </summary>
    public static TBuilder UseAutoComments<TBuilder>([NotNull] this TBuilder optionsBuilder, Action<AutoCommentOptionsBuilder> configure)
        where TBuilder : DbContextOptionsBuilder
    {
        var extension = optionsBuilder.Options.FindExtension<AutoCommentsExtension>() ??
                        new AutoCommentsExtension(new AutoCommentOptions());

        var builder = new AutoCommentOptionsBuilder(extension.Options);

        configure?.Invoke(builder);

        extension = new AutoCommentsExtension(builder.Options);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}

public record AutoCommentOptions
{
    internal IReadOnlyList<string> XmlFiles { get; init; } = Array.Empty<string>();

    internal bool AutoCommentEnumDescriptions { get; init; }

    internal bool CombineInheritanceComments { get; init; }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var item in XmlFiles)
        {
            hash.Add(item);
        }

        hash.Add(AutoCommentEnumDescriptions);
        hash.Add(CombineInheritanceComments);

        return hash.ToHashCode();
    }
}

public class AutoCommentOptionsBuilder
{
    public AutoCommentOptions Options { get; private set; }

    public AutoCommentOptionsBuilder(AutoCommentOptions initialOptions)
    {
        Options = initialOptions;
    }

    /// <summary>
    /// Sets XML documentation files used as the comments source.
    /// </summary>
    public AutoCommentOptionsBuilder FromXmlFiles(params string[] xmlFiles)
    {
        Options = Options with
        {
            XmlFiles = GetXmlFiles(xmlFiles).ToList()
        };

        return this;
    }

    /// <summary>
    /// Enables global enum value descriptions in comments.
    /// </summary>
    public AutoCommentOptionsBuilder AddEnumDescriptions()
    {
        Options = Options with
        {
            AutoCommentEnumDescriptions = true
        };

        return this;
    }

    /// <summary>
    /// Enables merged comments across inheritance hierarchies (experimental).
    /// </summary>
    public AutoCommentOptionsBuilder AddInheritanceComments()
    {
        Options = Options with
        {
            CombineInheritanceComments = true
        };

        return this;
    }

    private static IEnumerable<string> GetXmlFiles(string[] xmlFiles)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        foreach (var xmlFile in xmlFiles ?? Array.Empty<string>())
        {
            var found = false;
            var paths = new List<string>
            {
                xmlFile,
                Path.Combine(assemblyLocation, xmlFile)
            };

            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    yield return path;
                    found = true;

                    break;
                }
            }

            if (!found)
            {
                var searchedPaths = string.Join(Environment.NewLine, paths.Select(p => $"- {p}"));

                throw new InvalidOperationException(
                    $"XML file not found. Searched locations:{Environment.NewLine}{searchedPaths}");
            }
        }
    }
}