using TrueLogs.TrueLang.Lexers;

public class Token
{
    public required TokenType Type { get; init; }
    public required string? Value { get; init; }
    public required int Position { get; init; }

    public static Token EOF(int position) => new()
    {
        Position = position,
        Type = TokenType.EOF,
        Value = null,
    };

    public static Token Identifier(int position, string name) => new()
    {
        Position = position,
        Type = TokenType.Identifier,
        Value = name,
    };

    public static Token Operator(int position, TokenType type, string value) => new()
    {
        Position = position,
        Type = type,
        Value = value,
    };

    public static Token StringLiteral(int position, string value) => new()
    {
        Position = position,
        Type = TokenType.StringLiteral,
        Value = value,
    };

    public static Token NumberLiteral(int position, string value) => new()
    {
        Position = position,
        Type = TokenType.NumberLiteral,
        Value = value,
    };

    public static Token DateLiteral(int position, string value) => new()
    {
        Position = position,
        Type = TokenType.DateLiteral,
        Value = value,
    };
}