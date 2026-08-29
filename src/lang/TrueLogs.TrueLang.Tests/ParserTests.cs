using FluentAssertions;
using System.Globalization;
using TrueLogs.TrueLang.Lexers;
using TrueLogs.TrueLang.Parsers;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

namespace TrueLogs.TrueLang.Tests;

public class ParserTests
{
    [Fact]
    public void Parser_ShouldParseSimpleComparison()
    {
        // Arrange
        var input = "@level = `Error`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comparison = (ComparisonNode)ast;
        comparison.Field.Should().Be("level");
        comparison.Operator.Should().Be(ComparisonType.Equals);

        var comparisonNodeValue = comparison.Value;
        comparisonNodeValue.Should().BeOfType<StringLiteralNode>();
        var stringLiteral = (StringLiteralNode)comparisonNodeValue;
        stringLiteral.Value.Should().Be("Error");
    }


    [Fact]
    public void Parser_ShouldReturnNullForEmptyInput()
    {
        // Arrange
        var input = "";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeNull();
    }

    [Fact]
    public void Parser_ShouldReturnNullForWhitespaceInput()
    {
        // Arrange
        var input = "   ";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeNull();
    }

    [Fact]
    public void Parser_ShouldParseAndExpression()
    {
        // Arrange
        var input = "@level = `Error` and @level = `Warning`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.And);

        bin.Left.Should().BeOfType<ComparisonNode>();
        var left = (ComparisonNode)bin.Left;
        left.Field.Should().Be("level");
        left.Operator.Should().Be(ComparisonType.Equals);
        left.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Error");

        bin.Right.Should().BeOfType<ComparisonNode>();
        var right = (ComparisonNode)bin.Right;
        right.Field.Should().Be("level");
        right.Operator.Should().Be(ComparisonType.Equals);
        right.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Warning");
    }

    [Fact]
    public void Parser_ShouldParseOrExpression()
    {
        // Arrange
        var input = "@level = `Error` or @level = `Warning`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.Or);
        // проверяем левую и правую части как сравнения
    }

    [Fact]
    public void Parser_ShouldParseParenthesizedExpression()
    {
        // Arrange
        var input = "(@level = `Error`) and (@level = `Warning`)";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.And);
        // левая часть должна быть GroupNode, внутри которой ComparisonNode
        // правая часть тоже GroupNode
    }

    [Fact]
    public void Parser_ShouldThrowOnIncompleteAnd()
    {
        // Arrange
        var input = "@level = `Error` and";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldParseContainsOperator()
    {
        // Arrange
        var input = "@message ~ `error`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comparison = (ComparisonNode)ast;
        comparison.Field.Should().Be("message");
        comparison.Operator.Should().Be(ComparisonType.Contains);
        comparison.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("error");
    }

    [Fact]
    public void Parser_ShouldParseComplexExpressionWithParentheses()
    {
        // Arrange
        var input = "(@level = `Error` or @level = `Warning`) and @message ~ `timeout`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.And);

        bin.Left.Should().BeOfType<GroupNode>();
        var group = (GroupNode)bin.Left;
        group.Inner.Should().BeOfType<BinaryOperationNode>();
        var innerBin = (BinaryOperationNode)group.Inner;
        innerBin.Operator.Should().Be(BinaryOperationType.Or);
        innerBin.Left.Should().BeOfType<ComparisonNode>();
        innerBin.Right.Should().BeOfType<ComparisonNode>();

        bin.Right.Should().BeOfType<ComparisonNode>();
        var rightComp = (ComparisonNode)bin.Right;
        rightComp.Field.Should().Be("message");
        rightComp.Operator.Should().Be(ComparisonType.Contains);
        rightComp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("timeout");
    }

    [Fact]
    public void Parser_ShouldThrowOnUnclosedParenthesis()
    {
        // Arrange
        var input = "(@level = `Error`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldParseChainOfThreeAnds()
    {
        var input = "@a = `1` and @b = `2` and @c = `3`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);
        var ast = parser.Parse();

        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.And);
        // Левая часть должна быть (a and b)
        bin.Left.Should().BeOfType<BinaryOperationNode>();
        // Правая часть должна быть сравнением c = '3'
        bin.Right.Should().BeOfType<ComparisonNode>();
    }

    [Fact]
    public void Parser_ShouldParseNestedParentheses()
    {
        // Arrange
        var input = "((@a = `1`))";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<GroupNode>();
        var outerGroup = (GroupNode)ast;
        outerGroup.Inner.Should().BeOfType<GroupNode>();
        var innerGroup = (GroupNode)outerGroup.Inner;
        innerGroup.Inner.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)innerGroup.Inner;
        comp.Field.Should().Be("a");
        comp.Operator.Should().Be(ComparisonType.Equals);
        comp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("1");
    }

    [Fact]
    public void Parser_ShouldParseNumberComparison()
    {
        // Arrange
        var input = "@count > 10";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)ast;
        comp.Field.Should().Be("count");
        comp.Operator.Should().Be(ComparisonType.Greater);
        comp.Value.Should().BeOfType<NumberLiteralNode>();
        var num = (NumberLiteralNode)comp.Value;
        num.Value.Should().Be(10.0);
    }

    [Fact]
    public void Parser_ShouldThrowWhenMissingOperator()
    {
        // Arrange
        var input = "@a = `1` @b = `2`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldThrowWhenTwoOperatorsInRow()
    {
        // Arrange
        var input = "@a = `1` and and @b = `2`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldThrowOnEmptyParentheses()
    {
        // Arrange
        var input = "()";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldParseMixedAndOrWithoutParentheses()
    {
        // Arrange
        var input = "@a = `1` or @b = `2` and @c = `3`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        // Не проверяем структуру, просто что парсер справился
    }

    [Fact]
    public void Parser_ShouldParseLessThanComparison()
    {
        // Arrange
        var input = "@count < 10";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)ast;
        comp.Field.Should().Be("count");
        comp.Operator.Should().Be(ComparisonType.Less);
        comp.Value.Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(10.0);
    }

    [Fact]
    public void Parser_ShouldParseGreaterOrEqualComparison()
    {
        // Arrange
        var input = "@count >= 10";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)ast;
        comp.Field.Should().Be("count");
        comp.Operator.Should().Be(ComparisonType.GreaterOrEqual);
        comp.Value.Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(10.0);
    }

    [Fact]
    public void Parser_ShouldParseLessOrEqualComparison()
    {
        // Arrange
        var input = "@count <= 10";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)ast;
        comp.Field.Should().Be("count");
        comp.Operator.Should().Be(ComparisonType.LessOrEqual);
        comp.Value.Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(10.0);
    }

    [Fact]
    public void Parser_ShouldParseNotExpression()
    {
        var input = "not @level = `Error`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);
        var ast = parser.Parse();

        ast.Should().BeOfType<NotNode>();
        var not = (NotNode)ast;
        not.Operand.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)not.Operand;
        comp.Field.Should().Be("level");
        comp.Operator.Should().Be(ComparisonType.Equals);
        comp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Error");
    }

    [Fact]
    public void Parser_ShouldParseNotWithParentheses()
    {
        // Arrange
        var input = "not (@level = `Error`)";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<NotNode>();
        var not = (NotNode)ast;
        not.Operand.Should().BeOfType<GroupNode>();
        var group = (GroupNode)not.Operand;
        group.Inner.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)group.Inner;
        comp.Field.Should().Be("level");
        comp.Operator.Should().Be(ComparisonType.Equals);
        comp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Error");
    }

    [Fact]
    public void Parser_ShouldParseNotWithAnd()
    {
        // Arrange
        var input = "not @a = `1` and @b = `2`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.And);

        bin.Left.Should().BeOfType<NotNode>();
        var not = (NotNode)bin.Left;
        not.Operand.Should().BeOfType<ComparisonNode>();
        var leftComp = (ComparisonNode)not.Operand;
        leftComp.Field.Should().Be("a");
        leftComp.Operator.Should().Be(ComparisonType.Equals);
        leftComp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("1");

        bin.Right.Should().BeOfType<ComparisonNode>();
        var rightComp = (ComparisonNode)bin.Right;
        rightComp.Field.Should().Be("b");
        rightComp.Operator.Should().Be(ComparisonType.Equals);
        rightComp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("2");
    }

    [Fact]
    public void Parser_ShouldParseNotWithOr()
    {
        // Arrange
        var input = "@a = `1` or not @b = `2`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.Or);

        bin.Left.Should().BeOfType<ComparisonNode>();
        var leftComp = (ComparisonNode)bin.Left;
        leftComp.Field.Should().Be("a");
        leftComp.Operator.Should().Be(ComparisonType.Equals);
        leftComp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("1");

        bin.Right.Should().BeOfType<NotNode>();
        var not = (NotNode)bin.Right;
        not.Operand.Should().BeOfType<ComparisonNode>();
        var rightComp = (ComparisonNode)not.Operand;
        rightComp.Field.Should().Be("b");
        rightComp.Operator.Should().Be(ComparisonType.Equals);
        rightComp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("2");
    }

    [Fact]
    public void Parser_ShouldParseDoubleNot()
    {
        // Arrange
        var input = "not not @a = `1`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<NotNode>();
        var outerNot = (NotNode)ast;
        outerNot.Operand.Should().BeOfType<NotNode>();
        var innerNot = (NotNode)outerNot.Operand;
        innerNot.Operand.Should().BeOfType<ComparisonNode>();
        var comp = (ComparisonNode)innerNot.Operand;
        comp.Field.Should().Be("a");
        comp.Operator.Should().Be(ComparisonType.Equals);
        comp.Value.Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("1");
    }

    [Fact]
    public void Parser_ShouldThrowOnNotWithoutOperand()
    {
        // Arrange
        var input = "not";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldThrowOnNotAnd()
    {
        // Arrange
        var input = "not and @a = '1'";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldParseDateLiteralWithPipes()
    {
        // Arrange
        var input = "@timestamp > |2026-08-15T10:00:00.000Z|";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<ComparisonNode>();
        var comparison = (ComparisonNode)ast;
        comparison.Field.Should().Be("timestamp");
        comparison.Operator.Should().Be(ComparisonType.Greater);
        comparison.Value.Should().BeOfType<DateLiteralNode>();
        var dateLiteral = (DateLiteralNode)comparison.Value;
        dateLiteral.Value.Should().Be(DateTime.Parse("2026-08-15T10:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
    }

    [Fact]
    public void Parser_ShouldParseInExpression()
    {
        // Arrange
        var input = "@level in [`Error`, `Warning`]";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<InNode>();
        var inNode = (InNode)ast;
        inNode.Field.Should().Be("level");
        inNode.Values.Should().HaveCount(2);
        inNode.Values[0].Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Error");
        inNode.Values[1].Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Warning");
    }

    [Fact]
    public void Parser_ShouldParseInWithNumbers()
    {
        // Arrange
        var input = "@status in [200, 404, 500]";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<InNode>();
        var inNode = (InNode)ast;
        inNode.Field.Should().Be("status");
        inNode.Values.Should().HaveCount(3);
        inNode.Values[0].Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(200);
        inNode.Values[1].Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(404);
        inNode.Values[2].Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(500);
    }

    [Fact]
    public void Parser_ShouldThrowOnEmptyInList()
    {
        // Arrange
        var input = "@level in []";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldParseInWithSingleValue()
    {
        // Arrange
        var input = "@level in [`Error`]";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<InNode>();
        var inNode = (InNode)ast;
        inNode.Field.Should().Be("level");
        inNode.Values.Should().HaveCount(1);
        inNode.Values[0].Should().BeOfType<StringLiteralNode>().Which.Value.Should().Be("Error");
    }

    [Fact]
    public void Parser_ShouldParseInWithDates()
    {
        // Arrange
        var input = "@timestamp in [|2026-08-15T10:00:00.000Z|, |2026-08-16T10:00:00.000Z|]";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<InNode>();
        var inNode = (InNode)ast;
        inNode.Field.Should().Be("timestamp");
        inNode.Values.Should().HaveCount(2);
        inNode.Values[0].Should().BeOfType<DateLiteralNode>().Which.Value.Should().Be(DateTime.Parse("2026-08-15T10:00:00.000Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
        inNode.Values[1].Should().BeOfType<DateLiteralNode>().Which.Value.Should().Be(DateTime.Parse("2026-08-16T10:00:00.000Z", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind));
    }

    [Fact]
    public void Parser_ShouldParseInWithAnd()
    {
        // Arrange
        var input = "@level in [`Error`, `Warning`] and @status = 200";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        var ast = parser.Parse();

        // Assert
        ast.Should().BeOfType<BinaryOperationNode>();
        var bin = (BinaryOperationNode)ast;
        bin.Operator.Should().Be(BinaryOperationType.And);
        bin.Left.Should().BeOfType<InNode>();
        bin.Right.Should().BeOfType<ComparisonNode>();
        var inNode = (InNode)bin.Left;
        inNode.Field.Should().Be("level");
        inNode.Values.Should().HaveCount(2);
        var comp = (ComparisonNode)bin.Right;
        comp.Field.Should().Be("status");
        comp.Operator.Should().Be(ComparisonType.Equals);
        comp.Value.Should().BeOfType<NumberLiteralNode>().Which.Value.Should().Be(200);
    }

    [Fact]
    public void Parser_ShouldThrowOnUnclosedInBracket()
    {
        // Arrange
        var input = "@level in [`Error`";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }

    [Fact]
    public void Parser_ShouldThrowOnTrailingCommaInIn()
    {
        // Arrange
        var input = "@level in [`Error`,]";
        var lexer = new Lexer(input);
        var parser = new Parser(lexer);

        // Act
        Action act = () => parser.Parse();

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*unexpected type*");
    }
}