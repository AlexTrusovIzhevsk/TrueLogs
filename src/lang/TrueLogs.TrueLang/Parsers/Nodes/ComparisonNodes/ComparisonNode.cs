using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;
using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;

public class ComparisonNode : Node
{
    public required string Field { get; init; }
    public required ComparisonType Operator { get; init; }
    public required LiteralNode Value { get; init; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static ComparisonNode New(string field, ComparisonType comparisonType, LiteralNode value) => new()
    {
        Field = field,
        Operator = comparisonType,
        Value = value,
    };
}