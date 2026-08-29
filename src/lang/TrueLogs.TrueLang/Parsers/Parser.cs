using TrueLogs.TrueLang.Lexers;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

namespace TrueLogs.TrueLang.Parsers;

public class Parser(Lexer lexer)
{
    public Node? Parse()
    {
        if (_current.Type == TokenType.EOF)
        {
            return null;
        }

        var result = ParseExpression();

        if (_current.Type != TokenType.EOF)
        {
            throw UnexpectedTypeException(_current);
        }

        return result;
    }

    private Node ParseExpression()
    {
        var result = ParseCondition();

        while (_current.Type == TokenType.Identifier)
        {
            if (BinaryOperations.TryGetValue(_current.Value, out var operation))
            {
                Next();

                var right = ParseCondition();

                result = BinaryOperationNode.New(result, operation, right);
            }
            else
            {
                break;
            }
        }

        return result;
    }

    private Node ParseCondition()
    {
        if (_current.Type == TokenType.Identifier)
        {
            if (_current.Value == NotOperation)
            {
                return ParseNot();
            }
            else
            {
                return ParseComparison();
            }
        }
        else if (_current.Type == TokenType.LeftParen)
        {
            Next();

            var inner = ParseExpression();
            if (_current.Type != TokenType.RightParen)
            {
                throw UnexpectedTypeException(_current);
            }

            Next();

            return GroupNode.New(inner);
        }

        throw UnexpectedTypeException(_current);
    }

    private Node ParseNot()
    {
        Next();

        var condition = ParseCondition();
        return NotNode.New(condition);
    }

    private Node ParseComparison()
    {
        if (_current.Type != TokenType.Identifier)
        {
            throw UnexpectedTypeException(_current);
        }
        var field = _current.Value;

        Next();

        if (ComparisonTypes.TryGetValue(_current.Type, out var comparisonOperatorType))
        {
            return ParseOperatorComparison(field);
        }
        else if (_current.Value == InOperation)
        {
            return ParseIn(field);
        }

        throw UnexpectedTypeException(_current);
    }

    private Node ParseOperatorComparison(string field)
    {
        if (!ComparisonTypes.TryGetValue(_current.Type, out var comparisonOperatorType))
        {
            throw UnexpectedTypeException(_current);
        }

        Next();
        var value = ParseLiteral();
        Next();

        var node = ComparisonNode.New(field, comparisonOperatorType, value);
        return node;
    }

    private InNode ParseIn(string field)
    {
        var values = new List<LiteralNode>();

        Next();

        if (_current.Type != TokenType.LeftBracket)
        {
            throw UnexpectedTypeException(_current);
        }

        do
        {
            Next();
            var value = ParseLiteral();
            values.Add(value);

            Next();
            if (_current.Type != TokenType.Comma && _current.Type != TokenType.RightBracket)
            {
                throw UnexpectedTypeException(_current);
            }
        } while (_current.Type != TokenType.RightBracket);

        Next();

        var node = InNode.New(field, values.ToArray());
        return node;
    }

    private LiteralNode ParseLiteral() => _current.Type switch 
    {
        TokenType.StringLiteral => StringLiteralNode.New(_current.Value),
        TokenType.NumberLiteral => NumberLiteralNode.New(_current.Value),
        TokenType.DateLiteral => DateLiteralNode.New(_current.Value),
        _ => throw UnexpectedTypeException(_current),
    };


private void Next() => _current = lexer.NextToken();

    private Token _current = lexer.NextToken();

    private static Exception UnexpectedTypeException(Token token)
        => Exception($"unexpected type {token.Type}", token);

    private static Exception Exception(string text, Token token) 
        => new ArgumentException($"{text}; type: {token.Type}; value: {token.Value}; position: {token.Position}");

    private static Dictionary<TokenType, ComparisonType> ComparisonTypes = new Dictionary<TokenType, ComparisonType>() {
        [TokenType.Equals] = ComparisonType.Equals,
        [TokenType.Greater] = ComparisonType.Greater,
        [TokenType.GreaterOrEqual] = ComparisonType.GreaterOrEqual,
        [TokenType.Less] = ComparisonType.Less,
        [TokenType.LessOrEqual] = ComparisonType.LessOrEqual,
        [TokenType.Tilde] = ComparisonType.Contains,
    };

    private static Dictionary<string, BinaryOperationType> BinaryOperations = new Dictionary<string, BinaryOperationType>()
    {
        ["and"] = BinaryOperationType.And,
        ["or"] = BinaryOperationType.Or,
    };

    private const string NotOperation = "not";

    private const string InOperation = "in";
}
