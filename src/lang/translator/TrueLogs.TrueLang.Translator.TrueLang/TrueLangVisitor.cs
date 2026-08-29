using System.Globalization;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;
using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Translator.TrueLang;

internal class TrueLangVisitor : NodeVisitor
{
    public override string Visit(GroupNode node, TranslationContext context)
    {
        var operand = node.Inner.Accept(this, context);

        if (node.Inner is GroupNode)
        {
            return operand;
        }

        return $"({operand})";
    }

    public override string Visit(NotNode node, TranslationContext context)
    {
        var operand = node.Operand.Accept(this, context);

        if (node.Operand is BinaryOperationNode)
        {
            return $"not ({operand})";
        }

        return $"not {operand}";
    }

    public override string Visit(InNode node, TranslationContext context)
    {
        var values = new List<string>();
        foreach (var value in node.Values)
        {
            var valueText = value.Accept(this, context);
            values.Add(valueText);
        }
        var valuesText = string.Join(", ", values);

        return $"@{node.Field} in [{valuesText}]";
    }

    public override string Visit(ComparisonNode node, TranslationContext context)
    {
        var comparisonOperator = Translate(node.Operator);
        var value = node.Value.Accept(this, TranslationContext.New(node));

        return $"@{node.Field} {comparisonOperator} {value}";
    }

    public override string Visit(BinaryOperationNode node, TranslationContext context)
    {
        var left = node.Left.Accept(this, context);
        var operatorType = Translate(node.Operator);
        var right = node.Right.Accept(this, context);

        return $"{left} {operatorType} {right}";
    }

    public override string Visit(StringLiteralNode node, TranslationContext context)
    {
        return $"`{node.Value}`";
    }

    public override string Visit(NumberLiteralNode node, TranslationContext context)
    {
        return node.Value.ToString(CultureInfo.InvariantCulture);
    }

    public override string Visit(DateLiteralNode node, TranslationContext context)
    {
        return $"|{node.Value:yyyy-MM-ddTHH:mm:ss.fffZ}|";
    }

    private string Translate(ComparisonType comparisonType) => comparisonType switch
    {
        ComparisonType.Equals => "=",
        ComparisonType.Greater => ">",
        ComparisonType.GreaterOrEqual => ">=",
        ComparisonType.Less => "<",
        ComparisonType.LessOrEqual => "<=",
        ComparisonType.Contains => "~",
        _ => throw UnexpectedComparisonException(comparisonType),
    };

    private string Translate(BinaryOperationType binaryOperationType) => binaryOperationType switch
    {
        BinaryOperationType.And => "and",
        BinaryOperationType.Or => "or",
        _ => throw UnexpectedBinaryOperationException(binaryOperationType),
    };
}
