using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;

public class BinaryOperationNode : Node
{
    public required Node Left { get; set; }
    public required BinaryOperationType Operator { get; init; }
    public required Node Right { get; set; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static BinaryOperationNode New(Node left, BinaryOperationType Operator, Node Right) => new()
    {
        Left = left,
        Operator = Operator,
        Right = Right,
    };
}