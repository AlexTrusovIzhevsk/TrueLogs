using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;
using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes;

public class InNode : Node
{
    public required string Field { get; init; }
    public required LiteralNode[] Values { get; init; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static InNode New(string field, LiteralNode[] values) => new()
    {
        Field = field,
        Values = values
    };
}
