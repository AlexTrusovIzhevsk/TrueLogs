using FluentAssertions;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

namespace TrueLogs.TrueLang.Translator.LiteDB.Tests;

public class LiteDBTranslatorTests
{
    [Fact]
    public void Translator_ShouldTranslateEqualityComparison()
    {
       // Arrange

       var node = new ComparisonNode
       {
           Field = "level",
           Operator = ComparisonType.Equals,
           Value = StringLiteralNode.New("Error")
       };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("level = 'Error'");
    }

    [Fact]
    public void Translator_ShouldTranslateGreaterComparison()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "count",
            Operator = ComparisonType.Greater,
            Value = NumberLiteralNode.New(10)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("count > 10");
    }

    [Fact]
    public void Translator_ShouldTranslateLessComparison()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "count",
            Operator = ComparisonType.Less,
            Value = NumberLiteralNode.New(10)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("count < 10");
    }

    [Fact]
    public void Translator_ShouldTranslateGreaterOrEqualComparison()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "count",
            Operator = ComparisonType.GreaterOrEqual,
            Value = NumberLiteralNode.New(10)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("count >= 10");
    }

    [Fact]
    public void Translator_ShouldTranslateLessOrEqualComparison()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "count",
            Operator = ComparisonType.LessOrEqual,
            Value = NumberLiteralNode.New(10)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("count <= 10");
    }

    [Fact]
    public void Translator_ShouldTranslateContainsComparison()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "message",
            Operator = ComparisonType.Contains,
            Value = StringLiteralNode.New("exception")
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("message LIKE '%exception%'");
    }

    [Fact]
    public void Translator_ShouldTranslateAndExpression()
    {
        // Arrange
        var left = new ComparisonNode
        {
            Field = "level",
            Operator = ComparisonType.Equals,
            Value = StringLiteralNode.New("Error")
        };
        var right = new ComparisonNode
        {
            Field = "level",
            Operator = ComparisonType.Equals,
            Value = StringLiteralNode.New("Warning")
        };
        var node = new BinaryOperationNode
        {
            Left = left,
            Operator = BinaryOperationType.And,
            Right = right
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("(level = 'Error' AND level = 'Warning')");
    }

    [Fact]
    public void Translator_ShouldTranslateOrExpression()
    {
        // Arrange
        var left = new ComparisonNode
        {
            Field = "level",
            Operator = ComparisonType.Equals,
            Value = StringLiteralNode.New("Error")
        };
        var right = new ComparisonNode
        {
            Field = "level",
            Operator = ComparisonType.Equals,
            Value = StringLiteralNode.New("Warning")
        };
        var node = new BinaryOperationNode
        {
            Left = left,
            Operator = BinaryOperationType.Or,
            Right = right
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("(level = 'Error' OR level = 'Warning')");
    }

    [Fact]
    public void Translator_ShouldTranslateNotExpression()
    {
        // Arrange
        var operand = new ComparisonNode
        {
            Field = "level",
            Operator = ComparisonType.Equals,
            Value = StringLiteralNode.New("Error")
        };
        var node = new NotNode
        {
            Operand = operand
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("NOT (level = 'Error')");
    }

    [Fact]
    public void Translator_ShouldTranslateGroupNode()
    {
        // Arrange
        var inner = new ComparisonNode
        {
            Field = "level",
            Operator = ComparisonType.Equals,
            Value = StringLiteralNode.New("Error")
        };
        var node = new GroupNode { Inner = inner };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("level = 'Error'");
    }
    [Fact]
    public void Translator_ShouldTranslateDateTimeLiteral()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "timestamp",
            Operator = ComparisonType.Greater,
            Value = DateLiteralNode.New(new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc))
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("timestamp > '2026-08-15T10:00:00.000Z'");
    }

    [Fact]
    public void Translator_ShouldTranslateComplexExpression()
    {
        // not (@a = '1' or @b = '2') and @c = '3'
        var orLeft = new ComparisonNode { Field = "a", Operator = ComparisonType.Equals, Value = StringLiteralNode.New("1") };
        var orRight = new ComparisonNode { Field = "b", Operator = ComparisonType.Equals, Value = StringLiteralNode.New("2") };
        var or = new BinaryOperationNode { Left = orLeft, Operator = BinaryOperationType.Or, Right = orRight };
        var not = new NotNode { Operand = new GroupNode { Inner = or } };
        var c = new ComparisonNode { Field = "c", Operator = ComparisonType.Equals, Value = StringLiteralNode.New("3") };
        var root = new BinaryOperationNode { Left = not, Operator = BinaryOperationType.And, Right = c };

        var translator = new LiteDBTranslator();
        var result = translator.Translate(root);

        result.Should().Be("(NOT ((a = '1' OR b = '2')) AND c = '3')");
    }

    [Fact]
    public void Translator_ShouldTranslateNegativeNumber()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "count",
            Operator = ComparisonType.Greater,
            Value = NumberLiteralNode.New(-5.5)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("count > -5.5");
    }

    [Fact]
    public void Translator_ShouldTranslateFloatWithManyDigits()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "value",
            Operator = ComparisonType.Equals,
            Value = NumberLiteralNode.New(123.456789)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("value = 123.456789");
    }

    [Fact]
    public void Translator_ShouldTranslateIntegerWithManyDigits()
    {
        // Arrange
        var node = new ComparisonNode
        {
            Field = "count",
            Operator = ComparisonType.Equals,
            Value = NumberLiteralNode.New(123456789)
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("count = 123456789");
    }

    [Fact]
    public void Translator_ShouldTranslateInWithStrings()
    {
        // Arrange
        var node = new InNode
        {
            Field = "level",
            Values = [
                StringLiteralNode.New("Error"),
                StringLiteralNode.New("Warning")
            ]
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("level IN ['Error', 'Warning']");
    }

    [Fact]
    public void Translator_ShouldTranslateInWithNumbers()
    {
        // Arrange
        var node = new InNode
        {
            Field = "status",
            Values = [
                NumberLiteralNode.New(200),
                NumberLiteralNode.New(404),
                NumberLiteralNode.New(500)
            ]
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("status IN [200, 404, 500]");
    }

    [Fact]
    public void Translator_ShouldTranslateInWithDates()
    {
        // Arrange
        var node = new InNode
        {
            Field = "timestamp",
            Values = [
                DateLiteralNode.New(new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc)),
                DateLiteralNode.New(new DateTime(2026, 8, 16, 10, 0, 0, DateTimeKind.Utc))
            ]
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("timestamp IN ['2026-08-15T10:00:00.000Z', '2026-08-16T10:00:00.000Z']");
    }

    [Fact]
    public void Translator_ShouldTranslateInWithMixedTypes()
    {
        // Arrange
        var node = new InNode
        {
            Field = "value",
            Values = [
                StringLiteralNode.New("text"),
                NumberLiteralNode.New(100),
                DateLiteralNode.New(new DateTime(2026, 8, 15, 10, 0, 0, DateTimeKind.Utc))
            ]
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("value IN ['text', 100, '2026-08-15T10:00:00.000Z']");
    }

    [Fact]
    public void Translator_ShouldTranslateInWithSingleValue()
    {
        // Arrange
        var node = new InNode
        {
            Field = "level",
            Values = [ StringLiteralNode.New("Error") ]
        };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(node);

        // Assert
        result.Should().Be("level IN ['Error']");
    }

    [Fact]
    public void Translator_ShouldTranslateNotIn()
    {
        // Arrange
        var inNode = new InNode
        {
            Field = "level",
            Values = [
                StringLiteralNode.New("Error"),
                StringLiteralNode.New("Warning")
            ]
        };
        var notNode = new NotNode { Operand = inNode };
        var translator = new LiteDBTranslator();

        // Act
        var result = translator.Translate(notNode);

        // Assert
        result.Should().Be("NOT (level IN ['Error', 'Warning'])");
    }
}