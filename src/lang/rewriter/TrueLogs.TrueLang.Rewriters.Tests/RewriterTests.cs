using FluentAssertions;
using TrueLogs.TrueLang.Lexers;
using TrueLogs.TrueLang.Parsers;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Translator.TrueLang;

namespace TrueLogs.TrueLang.Rewriters.Tests;

public class RewriterTests
{
    private readonly TrueLangTranslator _translator = new();

    private static readonly DateTime TestDate = new(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc);
    private const string ExpectedDateLiteral = "|2026-08-15T10:00:00.000Z|";

    [Fact]
    public void AddEqualityString_WhenAstIsNull_ShouldCreateEquality()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("level", ComparisonType.Equals, "Error");

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@level = `Error`");
    }

    [Fact]
    public void AddContainsString_WhenAstIsNull_ShouldCreateContains()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("message", ComparisonType.Contains, "timeout");

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@message ~ `timeout`");
    }

    [Fact]
    public void AddEqualityInt_WhenAstIsNull_ShouldCreateEquality()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("count", ComparisonType.Equals, 42);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@count = 42");
    }

    [Fact]
    public void AddGreaterInt_WhenAstIsNull_ShouldCreateGreater()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("count", ComparisonType.Greater, 42);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@count > 42");
    }

    [Fact]
    public void AddLessInt_WhenAstIsNull_ShouldCreateLess()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("count", ComparisonType.Less, 42);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@count < 42");
    }

    [Fact]
    public void AddGreaterOrEqualInt_WhenAstIsNull_ShouldCreateGreaterOrEqual()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("count", ComparisonType.GreaterOrEqual, 42);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@count >= 42");
    }

    [Fact]
    public void AddLessOrEqualInt_WhenAstIsNull_ShouldCreateLessOrEqual()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("count", ComparisonType.LessOrEqual, 42);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@count <= 42");
    }

    [Fact]
    public void AddEqualityDate_WhenAstIsNull_ShouldCreateEquality()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("timestamp", ComparisonType.Equals, TestDate);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be($"@timestamp = {ExpectedDateLiteral}");
    }

    [Fact]
    public void AddGreaterDate_WhenAstIsNull_ShouldCreateGreater()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("timestamp", ComparisonType.Greater, TestDate);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be($"@timestamp > {ExpectedDateLiteral}");
    }

    [Fact]
    public void AddLessDate_WhenAstIsNull_ShouldCreateLess()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("timestamp", ComparisonType.Less, TestDate);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be($"@timestamp < {ExpectedDateLiteral}");
    }

    [Fact]
    public void AddGreaterOrEqualDate_WhenAstIsNull_ShouldCreateGreaterOrEqual()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("timestamp", ComparisonType.GreaterOrEqual, TestDate);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be($"@timestamp >= {ExpectedDateLiteral}");
    }

    [Fact]
    public void AddLessOrEqualDate_WhenAstIsNull_ShouldCreateLessOrEqual()
    {
        // Arrange
        var rewriter = new Rewriter(null);

        // Act
        rewriter.AddComparison("timestamp", ComparisonType.LessOrEqual, TestDate);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be($"@timestamp <= {ExpectedDateLiteral}");
    }

    [Fact]
    public void AddComparison_WhenAstHasSingleCondition_ShouldCreateAndNode()
    {
        // Arrange
        var rewriter = new Rewriter(null);
        rewriter.AddComparison("level", ComparisonType.Equals, "Error");

        // Act
        rewriter.AddComparison("message", ComparisonType.Contains, "timeout");

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@level = `Error` and @message ~ `timeout`");
    }

    [Fact]
    public void AddComparison_WhenAstHasAndNode_ShouldCreateNestedAndNode()
    {
        // Arrange
        var rewriter = new Rewriter(null);
        rewriter.AddComparison("level", ComparisonType.Equals, "Error");
        rewriter.AddComparison("message", ComparisonType.Contains, "timeout");

        // Act
        rewriter.AddComparison("count", ComparisonType.Greater, 5);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@level = `Error` and @message ~ `timeout` and @count > 5");
    }

    [Fact]
    public void AddComparison_SameFieldDifferentOperators_ShouldCreateAndNode()
    {
        var rewriter = new Rewriter(null);
        rewriter.AddComparison("count", ComparisonType.Equals, 42);
        rewriter.AddComparison("count", ComparisonType.Greater, 10);

        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@count = 42 and @count > 10");
    }

    [Fact]
    public void AddComparison_WhenAstHasOrNode_ShouldCreateAndNodeWithOrAsLeft()
    {
        // Arrange
        var lexer = new Lexer("@level = `Error` or @level = `Warning`");
        var parser = new Parser(lexer);
        var ast = parser.Parse();
        var rewriter = new Rewriter(ast);

        // Act
        rewriter.AddComparison("message", ComparisonType.Contains, "timeout");

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@level = `Error` or @level = `Warning` and @message ~ `timeout`");
    }

    [Fact]
    public void AddComparison_WithDifferentTypes_ShouldCombineCorrectly()
    {
        // Arrange
        var rewriter = new Rewriter(null);
        rewriter.AddComparison("count", ComparisonType.Greater, 10);

        // Act
        rewriter.AddComparison("timestamp", ComparisonType.Less, TestDate);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be($"@count > 10 and @timestamp < {ExpectedDateLiteral}");
    }

    [Fact]
    public void AddComparison_WithMultipleCallsAndMixedOperators_ShouldPreserveStructure()
    {
        // Arrange
        var lexer = new Lexer("@level = `Error` or @level = `Warning`");
        var parser = new Parser(lexer);
        var ast = parser.Parse();
        var rewriter = new Rewriter(ast);

        // Act
        rewriter.AddComparison("message", ComparisonType.Contains, "timeout");
        rewriter.AddComparison("count", ComparisonType.GreaterOrEqual, 42);

        // Assert
        var result = _translator.Translate(rewriter.Result);
        result.Should().Be("@level = `Error` or @level = `Warning` and @message ~ `timeout` and @count >= 42");
    }

    //[Fact]
    //public void AddComparison_SameFieldAndEquals_ShouldCreateInNode()
    //{
    //    // Arrange
    //    var rewriter = new Rewriter(null);
    //    rewriter.AddComparison("level", ComparisonType.Equals, "Error");
    //    rewriter.AddComparison("level", ComparisonType.Equals, "Warning");

    //    // Act
    //    var result = _translator.Translate(rewriter.Result);

    //    // Assert
    //    result.Should().Be("@level in [`Error`, `Warning`]");
    //}

    //[Fact]
    //public void AddComparison_SameFieldAndEqualsMultipleValues_ShouldCreateInNodeWithAll()
    //{
    //    // Arrange
    //    var rewriter = new Rewriter(null);
    //    rewriter.AddComparison("level", ComparisonType.Equals, "Error");
    //    rewriter.AddComparison("level", ComparisonType.Equals, "Warning");
    //    rewriter.AddComparison("level", ComparisonType.Equals, "Critical");

    //    // Act
    //    var result = _translator.Translate(rewriter.Result);

    //    // Assert
    //    result.Should().Be("@level in [`Error`, `Warning`, `Critical`]");
    //}

    //[Fact]
    //public void AddComparison_MixedWithAndAndDifferentFields_ShouldGroupOnlyEquals()
    //{
    //    // Arrange
    //    var lexer = new Lexer("@level = `Error` and @count > 5");
    //    var parser = new Parser(lexer);
    //    var ast = parser.Parse();
    //    var rewriter = new Rewriter(ast);
    //    rewriter.AddComparison("level", ComparisonType.Equals, "Warning");

    //    // Act
    //    var result = _translator.Translate(rewriter.Result);

    //    // Assert
    //    result.Should().Be("@level in [`Error`, `Warning`] and @count > 5");
    //}

}