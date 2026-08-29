using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

public class StringLiteralNode : LiteralNode
{
    public string Value { get; set; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static StringLiteralNode New(string value) => new()
    {
        Value = value
    };
}