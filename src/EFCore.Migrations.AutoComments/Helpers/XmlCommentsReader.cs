using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace EFCore.Migrations.AutoComments.Helpers;

internal class XmlCommentsReader
{
    private const string NewLinePlaceholder = "\n";

    private static readonly string[] LineSeparators =
    {
        "\r\n", "\r", "\n"
    };

    private readonly ConcurrentDictionary<string, string> _comments = new(StringComparer.Ordinal);

    private readonly bool _autoLoadAssemblies;

    private readonly HashSet<Assembly> _loadedAssemblies = new();

    private readonly object _syncRoot = new();

    public static XmlCommentsReader Create(IReadOnlyCollection<string> xmlFiles)
    {
        return xmlFiles.Count > 0 ? new XmlCommentsReader(xmlFiles) : new XmlCommentsReader();
    }

    private XmlCommentsReader(IReadOnlyCollection<string> xmlFiles)
    {
        _autoLoadAssemblies = false;

        LoadXmlFiles(xmlFiles);
    }

    private XmlCommentsReader()
    {
        _autoLoadAssemblies = true;
    }

    public string GetTypeComment(Type type)
    {
        EnsureAssemblyLoaded(type);

        foreach (var t in TypeHelper.GetParentTypes(type))
        {
            if (_comments.TryGetValue("T:" + GetFullName(t), out var comment))
                return comment;
        }

        return null;
    }

    public string GetPropertyComment(Type declaringType, string propertyName)
    {
        EnsureAssemblyLoaded(declaringType);

        foreach (var t in TypeHelper.GetParentTypes(declaringType))
        {
            if (_comments.TryGetValue($"P:{GetFullName(t)}.{propertyName}", out var comment))
                return comment;
        }

        return null;
    }

    public string GetEnumFieldComment(Type enumType, string fieldName)
    {
        EnsureAssemblyLoaded(enumType);

        return _comments.TryGetValue($"F:{GetFullName(enumType)}.{fieldName}", out var comment) ? comment : null;
    }

    private static string GetFullName(Type type) => type?.FullName?.Replace("+", ".") ?? string.Empty;

    private static string NormalizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        return string.Join(NewLinePlaceholder, text.Trim()
            .Split(LineSeparators, StringSplitOptions.None)
            .Select(line => line.Trim()));
    }

    private void LoadXmlFiles(IEnumerable<string> xmlFiles)
    {
        foreach (var xmlFile in xmlFiles)
        {
            var fileInfo = new FileInfo(xmlFile);

            if (fileInfo.Length == 0) continue;

            var doc = new XmlDocument();
            using (var stream = File.Open(xmlFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                doc.Load(stream);
            }

            foreach (XmlElement member in doc.GetElementsByTagName("member"))
            {
                var name = member.GetAttribute("name");

                if (string.IsNullOrEmpty(name)) continue;

                var summary = member["summary"];

                if (summary == null) continue;

                var normalized = NormalizeText(summary.InnerText);

                if (normalized != null)
                    _comments.TryAdd(name, normalized);
            }
        }
    }

    private void EnsureAssemblyLoaded(Type type)
    {
        if (!_autoLoadAssemblies || type == null) return;

        var assembly = type.Assembly;

        lock (_syncRoot)
        {
            if (_loadedAssemblies.Contains(assembly) || assembly.IsDynamic) return;

            _loadedAssemblies.Add(assembly);

            var xmlPath = GetXmlDocsPath(assembly);

            if (xmlPath != null)
            {
                LoadXmlFiles(new[]
                {
                    xmlPath
                });
            }
        }
    }

    private static string GetXmlDocsPath(Assembly assembly)
    {
        var assemblyPath = assembly.Location;

        if (!string.IsNullOrEmpty(assemblyPath))
        {
            var xmlPath = Path.ChangeExtension(assemblyPath, ".xml");

            if (File.Exists(xmlPath)) return xmlPath;
        }

        var baseDirectory = AppContext.BaseDirectory;
        if (!string.IsNullOrEmpty(baseDirectory))
        {
            var nameSource = string.IsNullOrEmpty(assemblyPath)
                ? assembly.ManifestModule.Name
                : assemblyPath;

            var assemblyName = Path.GetFileNameWithoutExtension(nameSource);
            var xmlPath = Path.Combine(baseDirectory, assemblyName + ".xml");

            if (File.Exists(xmlPath)) return xmlPath;
        }

        return null;
    }
}