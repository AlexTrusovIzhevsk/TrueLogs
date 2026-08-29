using System.Text;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

namespace TrueLogs.TrueLang.Translators;

public abstract class NodeVisitor
{
    public abstract string Visit(GroupNode node, TranslationContext context);

    public abstract string Visit(NotNode node, TranslationContext context);

    public abstract string Visit(InNode node, TranslationContext context);

    public abstract string Visit(ComparisonNode node, TranslationContext context);

    public abstract string Visit(BinaryOperationNode node, TranslationContext context);

    public abstract string Visit(StringLiteralNode node, TranslationContext context);

    public abstract string Visit(NumberLiteralNode node, TranslationContext context);

    public abstract string Visit(DateLiteralNode node, TranslationContext context);

    protected static Exception UnexpectedNodeException(Node node)
        => new ArgumentException($"unexpected node {node.GetType().Name}");

    protected static Exception UnexpectedComparisonException(ComparisonType comparisonType)
        => new ArgumentException($"unexpected comparison {comparisonType}");

    protected static Exception UnexpectedBinaryOperationException(BinaryOperationType binaryOperationType)
        => new ArgumentException($"unexpected binary operation {binaryOperationType}");
}
