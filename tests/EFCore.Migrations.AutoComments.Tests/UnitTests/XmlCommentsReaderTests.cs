using System;
using System.IO;
using System.Text;
using EFCore.Migrations.AutoComments.Helpers;
using Xunit;

namespace EFCore.Migrations.AutoComments.Tests.UnitTests;

/// <summary>
/// Unit tests for <see cref="XmlCommentsReader"/>.
/// </summary>
public class XmlCommentsReaderTests
{
    private class SampleModel
    {
        public string Name { get; set; }

        public int Count { get; set; }
    }

    private class BaseModel
    {
        public string BaseProperty { get; set; }
    }

    private class DerivedModel : BaseModel
    {
        public string DerivedProperty { get; set; }
    }

    private enum SampleEnum
    {
        Value1,

        Value2
    }

    /// <summary>
    /// Class comment.
    /// </summary>
    private class AssemblySampleModel
    {
        /// <summary>
        /// Comment for Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Comment for Count.
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// Enum comment.
    /// </summary>
    private enum AssemblySampleEnum
    {
        /// <summary>
        /// Comment for Value1.
        /// </summary>
        Value1,

        /// <summary>
        /// Comment for Value2.
        /// </summary>
        Value2
    }

    private static string TypeMember(Type type) => $"T:{type.FullName!.Replace("+", ".")}";

    private static string PropertyMember(Type type, string property) => $"P:{type.FullName!.Replace("+", ".")}.{property}";

    private static string EnumFieldMember(Type type, string field) => $"F:{type.FullName!.Replace("+", ".")}.{field}";

    private static string BuildXml(params (string memberName, string summary)[] members)
    {
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\"?><doc><assembly><name>Test</name></assembly><members>");

        foreach (var (memberName, summary) in members)
        {
            sb.Append($"<member name=\"{memberName}\"><summary>{summary}</summary></member>");
        }

        sb.Append("</members></doc>");

        return sb.ToString();
    }

    private static XmlCommentsReader CreateReader(string xmlContent)
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, xmlContent, Encoding.UTF8);

        return XmlCommentsReader.Create(new[]
        {
            path
        });
    }

    private static XmlCommentsReader CreateReader(string xml1, string xml2)
    {
        var path1 = Path.GetTempFileName();
        var path2 = Path.GetTempFileName();
        File.WriteAllText(path1, xml1, Encoding.UTF8);
        File.WriteAllText(path2, xml2, Encoding.UTF8);

        return (XmlCommentsReader.Create(new[]
        {
            path1, path2
        }));
    }

    private static XmlCommentsReader CreateReader()
    {
        return XmlCommentsReader.Create(Array.Empty<string>());
    }

    [Fact]
    public void GetTypeComment_Should_ReturnComment_FromXmlFile_WhenTypeExistsInXml()
    {
        // Arrange
        var xml = BuildXml((TypeMember(typeof(SampleModel)), "Test model."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(SampleModel));

        // Assert
        Assert.Equal("Test model.", comment);
    }

    [Fact]
    public void GetTypeComment_Should_ReturnNull_FromXmlFile_WhenTypeNotInXml()
    {
        // Arrange
        var xml = BuildXml((TypeMember(typeof(SampleModel)), "Test model."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(DerivedModel));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetTypeComment_Should_ReturnComment_FromAssembly_WhenClassCommentExists()
    {
        // Arrange
        var reader = CreateReader();

        // Act
        var comment = reader.GetTypeComment(typeof(AssemblySampleModel));

        // Assert
        Assert.Equal("Class comment.", comment);
    }

    [Fact]
    public void GetTypeComment_Should_ReturnNull_FromAssembly_WhenClassCommentNotExists()
    {
        // Arrange
        var reader = CreateReader();

        // Act
        var comment = reader.GetTypeComment(typeof(SampleModel));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetTypeComment_Should_TrimLeadingAndTrailingWhitespace()
    {
        // Arrange
        var xml = BuildXml((TypeMember(typeof(SampleModel)), "  Test model.  "));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(SampleModel));

        // Assert
        Assert.Equal("Test model.", comment);
    }

    [Fact]
    public void GetTypeComment_Should_NormalizeMultilineText()
    {
        // Arrange
        var xml = BuildXml((TypeMember(typeof(SampleModel)), "\n  First line.  \n  Second line.  \n"));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(SampleModel));

        // Assert
        Assert.Equal("First line.\nSecond line.", comment);
    }

    [Fact]
    public void GetTypeComment_Should_ReturnNull_WhenSummaryIsWhitespaceOnly()
    {
        // Arrange
        var xml = BuildXml((TypeMember(typeof(SampleModel)), "   "));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(SampleModel));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetTypeComment_Should_ReturnBaseClassComment_WhenDerivedClassNotInXml()
    {
        // Arrange
        var xml = BuildXml((TypeMember(typeof(BaseModel)), "Base model."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(DerivedModel));

        // Assert
        Assert.Equal("Base model.", comment);
    }

    [Fact]
    public void GetTypeComment_Should_PreferDerivedClassComment_OverBaseClass()
    {
        // Arrange
        var xml = BuildXml(
            (TypeMember(typeof(DerivedModel)), "Derived model."),
            (TypeMember(typeof(BaseModel)), "Base model."));

        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetTypeComment(typeof(DerivedModel));

        // Assert
        Assert.Equal("Derived model.", comment);
    }

    [Fact]
    public void GetPropertyComment_Should_ReturnComment_FromXmlFile_WhenPropertyExistsInXml()
    {
        // Arrange
        var xml = BuildXml((PropertyMember(typeof(SampleModel), nameof(SampleModel.Name)), "Name."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetPropertyComment(typeof(SampleModel), nameof(SampleModel.Name));

        // Assert
        Assert.Equal("Name.", comment);
    }

    [Fact]
    public void GetPropertyComment_Should_ReturnNull_FromXmlFile_WhenPropertyNotInXml()
    {
        // Arrange
        var xml = BuildXml((PropertyMember(typeof(SampleModel), nameof(SampleModel.Name)), "Name."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetPropertyComment(typeof(SampleModel), nameof(SampleModel.Count));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetPropertyComment_Should_ReturnComment_FromAssembly_WhenPropertyCommentExists()
    {
        // Arrange
        var reader = CreateReader();

        // Act
        var comment = reader.GetPropertyComment(typeof(AssemblySampleModel), nameof(AssemblySampleModel.Name));

        // Assert
        Assert.Equal("Comment for Name.", comment);
    }

    [Fact]
    public void GetPropertyComment_Should_ReturnNull_FromAssembly_WhenPropertyCommentNotExists()
    {
        // Arrange
        var reader = CreateReader();

        // Act
        var comment = reader.GetPropertyComment(typeof(SampleModel), nameof(SampleModel.Count));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetPropertyComment_Should_FindPropertyComment_InBaseClass()
    {
        // Arrange
        var xml = BuildXml((PropertyMember(typeof(BaseModel), nameof(BaseModel.BaseProperty)), "Base property."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetPropertyComment(typeof(DerivedModel), nameof(BaseModel.BaseProperty));

        // Assert
        Assert.Equal("Base property.", comment);
    }

    [Fact]
    public void GetPropertyComment_Should_ReturnNull_FromXmlFile_WhenTypeNotInXmlAtAll()
    {
        // Arrange
        var xml = BuildXml();
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetPropertyComment(typeof(SampleModel), nameof(SampleModel.Name));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetEnumFieldComment_Should_ReturnComment_FromXmlFile_WhenFieldExistsInXml()
    {
        // Arrange
        var xml = BuildXml((EnumFieldMember(typeof(SampleEnum), nameof(SampleEnum.Value1)), "First value."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetEnumFieldComment(typeof(SampleEnum), nameof(SampleEnum.Value1));

        // Assert
        Assert.Equal("First value.", comment);
    }

    [Fact]
    public void GetEnumFieldComment_Should_ReturnNull_FromXmlFile_WhenFieldNotInXml()
    {
        // Arrange
        var xml = BuildXml((EnumFieldMember(typeof(SampleEnum), nameof(SampleEnum.Value1)), "First value."));
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetEnumFieldComment(typeof(SampleEnum), nameof(SampleEnum.Value2));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetEnumFieldComment_Should_ReturnComment_FromAssembly_WhenFieldCommentExistsInXml()
    {
        // Arrange
        var reader = CreateReader();

        // Act
        var comment = reader.GetEnumFieldComment(typeof(AssemblySampleEnum), nameof(AssemblySampleEnum.Value1));

        // Assert
        Assert.Equal("Comment for Value1.", comment);
    }

    [Fact]
    public void GetEnumFieldComment_Should_ReturnNull_FromAssembly_WhenFieldCommentNotExists()
    {
        // Arrange
        var reader = CreateReader();

        // Act
        var comment = reader.GetEnumFieldComment(typeof(SampleEnum), nameof(SampleEnum.Value2));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void GetEnumFieldComment_Should_ReturnNull_FromXmlFile_WhenXmlIsEmpty()
    {
        // Arrange
        var xml = BuildXml();
        var reader = CreateReader(xml);

        // Act
        var comment = reader.GetEnumFieldComment(typeof(SampleEnum), nameof(SampleEnum.Value1));

        // Assert
        Assert.Null(comment);
    }

    [Fact]
    public void MultipleFiles_Should_FindTypesFromBothFiles()
    {
        // Arrange
        var xml1 = BuildXml((TypeMember(typeof(SampleModel)), "Model from the first file."));
        var xml2 = BuildXml((TypeMember(typeof(BaseModel)), "Model from the second file."));
        var reader = CreateReader(xml1, xml2);

        // Act
        var commentFromFile1 = reader.GetTypeComment(typeof(SampleModel));
        var commentFromFile2 = reader.GetTypeComment(typeof(BaseModel));

        // Assert
        Assert.Equal("Model from the first file.", commentFromFile1);
        Assert.Equal("Model from the second file.", commentFromFile2);
    }
}