namespace TrueLogs.TrueLang.Lexers;

public enum TokenType
{
    None = 0,
    EOF = 1,
    Identifier = 2,
    Equals = 3,
    Greater = 4,
    GreaterOrEqual = 5,
    Less = 6,
    LessOrEqual = 7,
    Tilde = 8,
    LeftParen = 9,
    RightParen = 10,
    StringLiteral = 11,
    NumberLiteral = 12,
    DateLiteral = 13,
    LeftBracket = 14,
    RightBracket = 15,
    Comma = 16,
}