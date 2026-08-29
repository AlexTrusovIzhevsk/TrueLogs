namespace TrueLogs.TrueLang.Lexers;

public class Lexer(string input)
{
    public Token NextToken()
    {
        SkipWhiteSpace();

        if (_position == input.Length)
        {
            return Token.EOF(_position);
        }

        var currentChar = input[_position];
        var currenPosition = _position;

        if (currentChar == IdentifierPrefix)
        {
            return ReadIdentifier();
        }

        if (IsStringLiteralBorder(currentChar))
        {
            return ReadStringLiteral();
        }

        if (IsDateLiteralBorder(currentChar))
        {
            return ReadDateLiteral();
        }

        if (IsDigit(currentChar))
        {
            return ReadNumberLiteral();
        }

        if (IsLetter(currentChar))
        {
            return ReadKeywordOrError();
        }

        return ReadOperator(currentChar);

        throw new ArgumentException($"Unexpected character '{currentChar}' at position {_position}");
    }

    private Token ReadOperator(char currentChar) => currentChar switch
    {
        EqualsOperator => Token.Operator(_position++, TokenType.Equals, EqualsOperatorText),
        TildeOperator => Token.Operator(_position++, TokenType.Tilde, TildeOperatorText),
        CommaOperator => Token.Operator(_position++, TokenType.Comma, CommaOperatorText),
        LeftParenOperator => Token.Operator(_position++, TokenType.LeftParen, LeftParenOperatorText),
        RightParenOperator => Token.Operator(_position++, TokenType.RightParen, RightParenOperatorText),
        LeftBracketOperator => Token.Operator(_position++, TokenType.LeftBracket, LeftBracketOperatorText),
        RightBracketOperator => Token.Operator(_position++, TokenType.RightBracket, RightBracketOperatorText),
        GreaterOperator => ReadOperator(TokenType.Greater, TokenType.GreaterOrEqual, GreaterOperatorText, GreaterOrEqualsOperatorText),
        LessOperator => ReadOperator(TokenType.Less, TokenType.LessOrEqual, LessOperatorText, LessOrEqualsOperatorText),
        _ => throw new ArgumentException($"Unexpected character '{currentChar}' at position {_position}"),
    };

    private void SkipWhiteSpace()
    {
        while (_position < input.Length && char.IsWhiteSpace(input[_position]))
        {
            _position++;
        }
    }
    private Token ReadKeywordOrError()
    {
        var start = _position;
        while (_position < input.Length && char.IsLetter(input[_position]))
        {
            _position++;
        }

        var word = input.Substring(start, _position - start);
        if (Keywords.Contains(word))
        {
            return Token.Identifier(start, word);
        }
        throw new ArgumentException($"Unexpected keyword '{word}'");
    }

    private Token ReadIdentifier()
    {
        var startPosition = _position;
        _position++;
        ReadWord(IsNotIdentifierChar);

        var name = input.Substring(startPosition + 1, _position - startPosition - 1);

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Expected identifier after '@'");
        }

        return Token.Identifier(startPosition, name);
    }

    private Token ReadStringLiteral()
    {
        var startPosition = _position;
        var value = ReadLiteral(StringLiteralBorder, IsStringLiteralBorder);
        return Token.StringLiteral(startPosition, value);
    }

    private Token ReadDateLiteral()
    {
        var startPosition = _position;
        var value = ReadLiteral(DateLiteralBorder, IsDateLiteralBorder);
        return Token.DateLiteral(startPosition, value);
    }

    private string ReadLiteral(char literalBorder, Func<char, bool>isLiteralBorder)
    {
        var startPosition = _position;
        _position++;
        ReadWord(isLiteralBorder);

        var value = input.Substring(startPosition + 1, _position - startPosition - 1);

        if (_position >= input.Length || input[_position] != literalBorder)
        {
            throw new ArgumentException("Unterminated literal");
        }

        _position++;
        return value;
    }

    private Token ReadNumberLiteral()
    {
        var startPosition = _position;
        ReadWord(c => !IsDigit(c) && c != '.');

        var value = input.Substring(startPosition, _position - startPosition);
        return Token.NumberLiteral(startPosition, value);
    }

    private void ReadWord(Func<char,bool> isEnd)
    {
        while (_position < input.Length && !isEnd(input[_position]))
        {
            _position++;
        }
    }

    private Token ReadOperator(
        TokenType tokenType, 
        TokenType tokenTypeWithEqual, 
        string operatorText, 
        string operatorWithEqualText)
    {
        var currenPosition = _position;
        var nextCharPosition = _position + 1;
        if (input.Length > nextCharPosition)
        {
            var nextChar = input[_position + 1];
            if (nextChar == EqualsOperator)
            {
                _position += 2;
                return Token.Operator(currenPosition, tokenTypeWithEqual, operatorWithEqualText);
            }
        }
        return Token.Operator(_position++, tokenType, operatorText);
    }


    private int _position = 0;

    private const char IdentifierPrefix = '@';
    private const char EqualsOperator = '=';
    private const char GreaterOperator = '>';
    private const char LessOperator = '<';
    private const char TildeOperator = '~';
    private const char LeftParenOperator = '(';
    private const char RightParenOperator = ')';
    private const char LeftBracketOperator = '[';
    private const char RightBracketOperator = ']';
    private const char StringLiteralBorder = '`';
    private const char DateLiteralBorder = '|';
    private const char CommaOperator = ',';
    private const string AndOperator = "and";
    private const string OrOperator = "or";
    private const string NotOperator = "not";
    private const string InOperator = "in";

    private static HashSet<string> Keywords = new() { AndOperator, OrOperator, NotOperator, InOperator };

    private static string EqualsOperatorText = EqualsOperator.ToString();
    private static string GreaterOperatorText = GreaterOperator.ToString();
    private static string GreaterOrEqualsOperatorText = GreaterOperator + EqualsOperatorText;
    private static string LessOperatorText = LessOperator.ToString();
    private static string LessOrEqualsOperatorText = LessOperator + EqualsOperatorText;
    private static string TildeOperatorText = TildeOperator.ToString();
    private static string LeftParenOperatorText = LeftParenOperator.ToString();
    private static string RightParenOperatorText = RightParenOperator.ToString();
    private static string LeftBracketOperatorText = LeftBracketOperator.ToString();
    private static string RightBracketOperatorText = RightBracketOperator.ToString();
    private static string CommaOperatorText = CommaOperator.ToString();

    private static Func<char, bool> IsDigit = ch => char.IsDigit(ch);
    private static Func<char, bool> IsLetter = ch => char.IsLetter(ch);
    private static Func<char, bool> IsStringLiteralBorder = ch => ch == StringLiteralBorder;
    private static Func<char, bool> IsDateLiteralBorder = ch => ch == DateLiteralBorder;
    private static Func<char, bool> IsNotIdentifierChar = ch => !(char.IsLetterOrDigit(ch) || ch == '_');
}
