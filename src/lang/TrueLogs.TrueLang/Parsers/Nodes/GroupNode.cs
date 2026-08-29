using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes;

public class GroupNode : Node
{
    public required Node Inner { get; init; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static GroupNode New(Node inner) => new()
    {
        Inner = inner
    };
}
