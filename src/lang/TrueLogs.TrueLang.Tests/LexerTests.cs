using FluentAssertions;
using TrueLogs.TrueLang.Lexers;
using TrueLogs.TrueLang.Parsers;

namespace TrueLogs.TrueLang.Tests;

public class LexerTests
{
    [Fact]
    public void Lexer_ShouldTokenizeEmpty()
    {
        // Arrange
        var input = "";
        var lexer = new Lexer(input);

        // Act
        var tokent = lexer.NextToken();

        // Assert
        tokent.Type.Should().Be(TokenType.EOF);
        tokent.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldReturnIdentifierTokenForAtPrefix()
    {
        // Arrange
        var input = "@level";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.Identifier);
        first.Value.Should().Be("level");
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeIdentifierWithUnderscoreAndDigits()
    {
        // Arrange
        var input = "@user_id_123";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("user_id_123");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }


    [Fact]
    public void Lexer_ShouldTokenizeEqualsOperator()
    {
        // Arrange
        var input = "=";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.Equals);
        first.Value.Should().Be("=");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeGreaterOperator()
    {
        // Arrange
        var input = ">";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.Greater);
        first.Value.Should().Be(">");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeGreaterOrEqualOperator()
    {
        // Arrange
        var input = ">=";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.GreaterOrEqual);
        first.Value.Should().Be(">=");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeLessOperator()
    {
        // Arrange
        var input = "<";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.Less);
        first.Value.Should().Be("<");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeLessOrEqualOperator()
    {
        // Arrange
        var input = "<=";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.LessOrEqual);
        first.Value.Should().Be("<=");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeTildeOperator()
    {
        // Arrange
        var input = "~";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.Tilde);
        first.Value.Should().Be("~");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeLeftParenthesis()
    {
        // Arrange
        var input = "(";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.LeftParen);
        first.Value.Should().Be("(");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeRightParenthesis()
    {
        // Arrange
        var input = ")";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.RightParen);
        first.Value.Should().Be(")");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.EOF);
        second.Value.Should().BeNull();
    }

    [Fact]
    public void Lexer_ShouldTokenizeStringLiteral()
    {
        // Arrange
        var input = "`Hello world`";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.StringLiteral);
        token.Value.Should().Be("Hello world");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeEmptyStringLiteral()
    {
        // Arrange
        var input = "``";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.StringLiteral);
        token.Value.Should().Be("");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeIntegerLiteral()
    {
        // Arrange
        var input = "123";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.NumberLiteral);
        token.Value.Should().Be("123");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeFloatLiteral()
    {
        // Arrange
        var input = "3.14";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.NumberLiteral);
        token.Value.Should().Be("3.14");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldThrowOnUnknownCharacter()
    {
        // Arrange
        var input = "$";
        var lexer = new Lexer(input);

        // Act & Assert
        var exception = Record.Exception(() => lexer.NextToken());
        exception.Should().NotBeNull();
        exception.Message.Should().Contain("Unexpected character");
    }

    [Fact]
    public void Lexer_ShouldThrowOnUnknownCharacterAfterCorrect()
    {
        // Arrange
        var input = "@level $";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();

        // Act & Assert
        first.Type.Should().Be(TokenType.Identifier);
        first.Value.Should().Be("level");
        var exception = Record.Exception(() => lexer.NextToken());
        exception.Should().NotBeNull();
        exception.Message.Should().Contain("Unexpected character");
    }

    [Fact]
    public void Lexer_ShouldThrowOnUnterminatedString()
    {
        // Arrange
        var input = "`Error";
        var lexer = new Lexer(input);

        // Act & Assert
        var exception = Record.Exception(() => lexer.NextToken());
        exception.Should().NotBeNull();
        exception.Message.Should().Contain("Unterminated literal");
    }

    [Fact]
    public void Lexer_ShouldThrowOnEmptyIdentifier()
    {
        // Arrange
        var input = "@";
        var lexer = new Lexer(input);

        // Act & Assert
        var exception = Record.Exception(() => lexer.NextToken());
        exception.Should().NotBeNull();
        exception.Message.Should().Contain("Expected identifier after '@'");
    }

    [Fact]
    public void Lexer_ShouldTokenizeAndKeywordAsIdentifier()
    {
        // Arrange
        var input = "and";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("and");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeOrKeywordAsIdentifier()
    {
        // Arrange
        var input = "or";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("or");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeFullExpressionWithSpaces()
    {
        // Arrange
        var input = "@level = `Error`";
        var lexer = new Lexer(input);

        // Act
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        // Assert
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.EOF
        );
        tokens[0].Value.Should().Be("level");
        tokens[1].Value.Should().Be("=");
        tokens[2].Value.Should().Be("Error");
    }

    [Fact]
    public void Lexer_ShouldStopIdentifierBeforeEquals()
    {
        // Arrange
        var input = "@level=";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.Identifier);
        first.Value.Should().Be("level");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.Equals);
        second.Value.Should().Be("=");
        second.Position.Should().Be(6); // позиция '=' (длина "@level")
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeNumberFollowedByOperator()
    {
        // Arrange
        var input = "1.2 =";
        var lexer = new Lexer(input);

        // Act
        var first = lexer.NextToken();
        var second = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        first.Type.Should().Be(TokenType.NumberLiteral);
        first.Value.Should().Be("1.2");
        first.Position.Should().Be(0);
        second.Type.Should().Be(TokenType.Equals);
        second.Value.Should().Be("=");
        second.Position.Should().Be(4); // позиция '=' после числа и пробела
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeAndKeywordWithSpaces()
    {
        var input = "and ";
        var lexer = new Lexer(input);
        var token = lexer.NextToken();
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("and");
        var eof = lexer.NextToken();
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeAndBeforeEquals()
    {
        var input = "and=";
        var lexer = new Lexer(input);
        var token = lexer.NextToken();
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("and");
        var op = lexer.NextToken();
        op.Type.Should().Be(TokenType.Equals);
    }

    [Fact]
    public void Lexer_ShouldTokenizeExpressionWithAnd()
    {
        // Arrange
        var input = "@level = `Error` and @level = `Warning`";
        var lexer = new Lexer(input);

        // Act
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        // Assert
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.Identifier, // and
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.EOF
        );
        tokens[0].Value.Should().Be("level");
        tokens[1].Value.Should().Be("=");
        tokens[2].Value.Should().Be("Error");
        tokens[3].Value.Should().Be("and");
        tokens[4].Value.Should().Be("level");
        tokens[5].Value.Should().Be("=");
        tokens[6].Value.Should().Be("Warning");
    }

    [Fact]
    public void Lexer_ShouldTokenizeExpressionWithParentheses()
    {
        // Arrange
        var input = "(@level = `Error` or @level = `Warning`) and @message ~ `timeout`";
        var lexer = new Lexer(input);

        // Act
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        // Assert
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.LeftParen,
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.Identifier, // or
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.RightParen,
            TokenType.Identifier, // and
            TokenType.Identifier,
            TokenType.Tilde,
            TokenType.StringLiteral,
            TokenType.EOF
        );
    }

    [Fact]
    public void Lexer_ShouldIgnoreLeadingAndTrailingSpaces()
    {
        // Arrange
        var input = "  @level  ";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("level");
        token.Position.Should().Be(2); // позиция с учётом ведущих пробелов
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeExpressionWithOr()
    {
        // Arrange
        var input = "@level = `Error` or @level = `Warning`";
        var lexer = new Lexer(input);

        // Act
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        // Assert
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.Identifier, // or
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.EOF
        );
        tokens[3].Value.Should().Be("or");
    }

    [Fact]
    public void Lexer_ShouldTokenizeExpressionWithTildeAndAnd()
    {
        // Arrange
        var input = "@message ~ `error` and @level = `Error`";
        var lexer = new Lexer(input);

        // Act
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        // Assert
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.Identifier,
            TokenType.Tilde,
            TokenType.StringLiteral,
            TokenType.Identifier, // and
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.EOF
        );
    }

    [Fact]
    public void Lexer_ShouldTokenizeNotKeywordAsIdentifier()
    {
        // Arrange
        var input = "not";

        // Act
        var lexer = new Lexer(input);
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("not");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeExpressionWithNot()
    {
        // Arrange
        var input = "@a = `1` and not @b = `2`";

        // Act
        var lexer = new Lexer(input);
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        // Assert
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.Identifier, // and
            TokenType.Identifier, // not
            TokenType.Identifier,
            TokenType.Equals,
            TokenType.StringLiteral,
            TokenType.EOF
        );
        tokens[4].Value.Should().Be("not");
    }

    [Fact]
    public void Lexer_ShouldTokenizeDateLiteralWithPipes()
    {
        // Arrange
        var input = "|2026-08-15T10:00:00Z|";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.DateLiteral);
        token.Value.Should().Be("2026-08-15T10:00:00Z");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldThrowOnUnclosedDateLiteral()
    {
        // Arrange
        var input = "|2026-08-15";
        var lexer = new Lexer(input);

        // Act & Assert
        var exception = Record.Exception(() => lexer.NextToken());
        exception.Should().NotBeNull();
        exception.Message.Should().Contain("Unterminated literal");
    }

    [Fact]
    public void Lexer_ShouldTokenizeLeftBracket()
    {
        // Arrange
        var input = "[";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.LeftBracket);
        token.Value.Should().Be("[");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeRightBracket()
    {
        // Arrange
        var input = "]";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.RightBracket);
        token.Value.Should().Be("]");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeComma()
    {
        // Arrange
        var input = ",";
        var lexer = new Lexer(input);

        // Act
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        // Assert
        token.Type.Should().Be(TokenType.Comma);
        token.Value.Should().Be(",");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeInKeywordAsIdentifier()
    {
        var input = "in";
        var lexer = new Lexer(input);
        var token = lexer.NextToken();
        var eof = lexer.NextToken();

        token.Type.Should().Be(TokenType.Identifier);
        token.Value.Should().Be("in");
        token.Position.Should().Be(0);
        eof.Type.Should().Be(TokenType.EOF);
    }

    [Fact]
    public void Lexer_ShouldTokenizeInExpression()
    {
        var input = "@level in [`Error`, `Warning`]";
        var lexer = new Lexer(input);
        var tokens = new List<Token>();
        Token token;
        do
        {
            token = lexer.NextToken();
            tokens.Add(token);
        } while (token.Type != TokenType.EOF);

        tokens.Select(t => t.Type).Should().Equal(
            TokenType.Identifier,
            TokenType.Identifier, // in
            TokenType.LeftBracket,
            TokenType.StringLiteral,
            TokenType.Comma,
            TokenType.StringLiteral,
            TokenType.RightBracket,
            TokenType.EOF
        );
        tokens[0].Value.Should().Be("level");
        tokens[1].Value.Should().Be("in");
        tokens[3].Value.Should().Be("Error");
        tokens[5].Value.Should().Be("Warning");
    }
}