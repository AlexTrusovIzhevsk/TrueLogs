using FluentAssertions;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

namespace TrueLogs.TrueLang.Translator.TrueLang.Tests;

public class TrueLangTranslatorTests
{
    private readonly TrueLangTranslator _translator = new();

    [Fact]
    public void Translate_EqualityComparison_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Error"));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@level = `Error`");
    }

    [Fact]
    public void Translate_GreaterComparison_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("count", ComparisonType.Greater, NumberLiteralNode.New(10));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@count > 10");
    }

    [Fact]
    public void Translate_LessComparison_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("count", ComparisonType.Less, NumberLiteralNode.New(10));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@count < 10");
    }

    [Fact]
    public void Translate_GreaterOrEqualComparison_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("count", ComparisonType.GreaterOrEqual, NumberLiteralNode.New(10));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@count >= 10");
    }

    [Fact]
    public void Translate_LessOrEqualComparison_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("count", ComparisonType.LessOrEqual, NumberLiteralNode.New(10));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@count <= 10");
    }

    [Fact]
    public void Translate_ContainsComparison_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("message", ComparisonType.Contains, StringLiteralNode.New("exception"));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@message ~ `exception`");
    }

    [Fact]
    public void Translate_NumberLiteral_Integer_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("value", ComparisonType.Equals, NumberLiteralNode.New(42));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@value = 42");
    }

    [Fact]
    public void Translate_NumberLiteral_Float_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("pi", ComparisonType.Equals, NumberLiteralNode.New(3.14));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@pi = 3.14");
    }

    [Fact]
    public void Translate_NumberLiteral_Negative_ReturnsTrueLang()
    {
        // Arrange
        var node = ComparisonNode.New("temp", ComparisonType.Equals, NumberLiteralNode.New(-5.5));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@temp = -5.5");
    }

    [Fact]
    public void Translate_DateLiteral_ReturnsTrueLang()
    {
        // Arrange
        var date = new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc);
        var node = ComparisonNode.New("timestamp", ComparisonType.Greater, DateLiteralNode.New(date));

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be($"@timestamp > |{date:yyyy-MM-ddTHH:mm:ss.fffZ}|");
    }

    [Fact]
    public void Translate_AndExpression_ReturnsTrueLangWithParentheses()
    {
        // Arrange
        var left = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Error"));
        var right = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Warning"));
        var node = BinaryOperationNode.New(left, BinaryOperationType.And, right);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@level = `Error` and @level = `Warning`");
    }

    [Fact]
    public void Translate_OrExpression_ReturnsTrueLangWithParentheses()
    {
        // Arrange
        var left = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Error"));
        var right = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Warning"));
        var node = BinaryOperationNode.New(left, BinaryOperationType.Or, right);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@level = `Error` or @level = `Warning`");
    }

    [Fact]
    public void Translate_NotExpression_SimpleComparison_ReturnsTrueLang()
    {
        // Arrange
        var operand = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Info"));
        var node = NotNode.New(operand);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("not @level = `Info`");
    }

    [Fact]
    public void Translate_NotExpression_WithBinaryOperation_ReturnsTrueLangWithParentheses()
    {
        // Arrange
        var left = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Error"));
        var right = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Warning"));
        var binary = BinaryOperationNode.New(left, BinaryOperationType.Or, right);
        var node = NotNode.New(binary);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("not (@level = `Error` or @level = `Warning`)");
    }

    [Fact]
    public void Translate_GroupExpression_ReturnsTrueLangWithParentheses()
    {
        // Arrange
        var inner = ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Error"));
        var node = GroupNode.New(inner);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("(@level = `Error`)");
    }

    [Fact]
    public void Translate_InWithStrings_ReturnsTrueLang()
    {
        // Arrange
        var values = new LiteralNode[]
        {
            StringLiteralNode.New("Error"),
            StringLiteralNode.New("Warning")
        };
        var node = InNode.New("level", values);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@level in [`Error`, `Warning`]");
    }

    [Fact]
    public void Translate_InWithNumbers_ReturnsTrueLang()
    {
        // Arrange
        var values = new LiteralNode[]
        {
            NumberLiteralNode.New(200),
            NumberLiteralNode.New(404),
            NumberLiteralNode.New(500)
        };
        var node = InNode.New("status", values);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@status in [200, 404, 500]");
    }

    [Fact]
    public void Translate_InWithDates_ReturnsTrueLang()
    {
        // Arrange
        var date1 = new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc);
        var date2 = new DateTime(2026, 8, 16, 10, 0, 0, DateTimeKind.Utc);
        var values = new LiteralNode[]
        {
            DateLiteralNode.New(date1),
            DateLiteralNode.New(date2)
        };
        var node = InNode.New("timestamp", values);

        // Act
        var result = _translator.Translate(node);

        // Assert
        var expected = $"@timestamp in [|{date1:yyyy-MM-ddTHH:mm:ss.fffZ}|, |{date2:yyyy-MM-ddTHH:mm:ss.fffZ}|]";
        result.Should().Be(expected);
    }

    [Fact]
    public void Translate_InWithMixedTypes_ReturnsTrueLang()
    {
        // Arrange
        var date = new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc);
        var values = new LiteralNode[]
        {
            StringLiteralNode.New("text"),
            NumberLiteralNode.New(100),
            DateLiteralNode.New(date)
        };
        var node = InNode.New("value", values);

        // Act
        var result = _translator.Translate(node);

        // Assert
        var expected = $"@value in [`text`, 100, |{date:yyyy-MM-ddTHH:mm:ss.fffZ}|]";
        result.Should().Be(expected);
    }

    [Fact]
    public void Translate_InWithSingleValue_ReturnsTrueLang()
    {
        // Arrange
        var values = new LiteralNode[] { StringLiteralNode.New("Error") };
        var node = InNode.New("level", values);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("@level in [`Error`]");
    }

    [Fact]
    public void Translate_NotIn_ReturnsTrueLang()
    {
        // Arrange
        var inNode = InNode.New("level", new LiteralNode[] { StringLiteralNode.New("Error"), StringLiteralNode.New("Warning") });
        var node = NotNode.New(inNode);

        // Act
        var result = _translator.Translate(node);

        // Assert
        result.Should().Be("not @level in [`Error`, `Warning`]");
    }

    [Fact]
    public void Translate_ComplexExpression_ReturnsTrueLang()
    {
        // Arrange
        // not (@a = `1` or @b = `2`) and @c = `3`
        var orLeft = ComparisonNode.New("a", ComparisonType.Equals, StringLiteralNode.New("1"));
        var orRight = ComparisonNode.New("b", ComparisonType.Equals, StringLiteralNode.New("2"));
        var orNode = BinaryOperationNode.New(orLeft, BinaryOperationType.Or, orRight);
        var groupOr = GroupNode.New(orNode);
        var notNode = NotNode.New(groupOr);
        var cNode = ComparisonNode.New("c", ComparisonType.Equals, StringLiteralNode.New("3"));
        var root = BinaryOperationNode.New(notNode, BinaryOperationType.And, cNode);

        // Act
        var result = _translator.Translate(root);

        // Assert
        result.Should().Be("not (@a = `1` or @b = `2`) and @c = `3`");
    }

    [Fact]
    public void Translate_ComplexWithMixedPrecedence_ReturnsTrueLang()
    {
        // Arrange
        // (@level = `Error` or @level = `Warning`) and @message ~ `timeout`
        var leftOr = BinaryOperationNode.New(
            ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Error")),
            BinaryOperationType.Or,
            ComparisonNode.New("level", ComparisonType.Equals, StringLiteralNode.New("Warning")));
        var leftGroup = GroupNode.New(leftOr);
        var right = ComparisonNode.New("message", ComparisonType.Contains, StringLiteralNode.New("timeout"));
        var root = BinaryOperationNode.New(leftGroup, BinaryOperationType.And, right);

        // Act
        var result = _translator.Translate(root);

        // Assert
        result.Should().Be("(@level = `Error` or @level = `Warning`) and @message ~ `timeout`");
    }
}