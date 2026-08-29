using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

public class NumberLiteralNode : LiteralNode
{
    public double Value { get; set; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static NumberLiteralNode New(string value) => new()
    {
        Value = double.Parse(value)
    };

    public static NumberLiteralNode New(int value) => new()
    {
        Value = value
    };

    public static NumberLiteralNode New(double value) => new()
    {
        Value = value
    };
}