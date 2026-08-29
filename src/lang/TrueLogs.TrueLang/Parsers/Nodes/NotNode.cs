using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes;

public class NotNode : Node
{
    public required Node Operand { get; init; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static NotNode New(Node operand) => new()
    {
        Operand = operand
    };
}
