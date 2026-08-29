using System.Xml.Linq;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.BinaryOperationNodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

namespace TrueLogs.TrueLang.Rewriters;

public class Rewriter
{
    private Node? _ast;

    public Node? Result => _ast;

    public Rewriter(Node? ast)
    {
        _ast = ast;
    }

    public void AddComparison(string field, ComparisonType comparisonType, string value)
    {
        var literalNode = StringLiteralNode.New(value);
        AddComparison(field, comparisonType, literalNode);
    }

    public void AddComparison(string field, ComparisonType comparisonType, DateTime value)
    {
        var literalNode = DateLiteralNode.New(value);
        AddComparison(field, comparisonType, literalNode);
    }

    public void AddComparison(string field, ComparisonType comparisonType, double value)
    {
        var literalNode = NumberLiteralNode.New(value);
        AddComparison(field, comparisonType, literalNode);
    }

    private void AddComparison(string field, ComparisonType comparisonType, LiteralNode node)
    {
        //if (TryAddIn(field, comparisonType, node))
        //{
        //    return;
        //}

        var comparisonNode = ComparisonNode.New(field, comparisonType, node);
        AddAnd(comparisonNode);
    }

    //private bool TryAddIn(string field, ComparisonType comparisonType, LiteralNode node)
    //{
    //    if (_ast is not null && comparisonType == ComparisonType.Equals)
    //    {
    //        var addInOk = Bypass(_ast, (node) => _ast = node, field, node);
    //        return addInOk;
    //    }

    //    return false;
    //}

    //private bool Bypass(Node current, Action<Node> setNode, string key, LiteralNode literalNode)
    //{
    //    if (current is InNode inNode && inNode.Field == key)
    //    {
    //        var list = inNode.Values.ToList();
    //        list.Add(literalNode);
    //        inNode = InNode.New(key, list.ToArray());
    //        setNode(inNode);

    //        return true;
    //    }

    //    if (current is ComparisonNode comparison && comparison.Field == key)
    //    {
    //        var oldNode = comparison.Value;
    //        var newNode = InNode.New(key, [oldNode, literalNode]);
    //        setNode(newNode);

    //        return true;
    //    }

    //    if (current is BinaryOperationNode binaryOperation)
    //    {
    //        var left = Bypass(binaryOperation.Left, (node) => binaryOperation.Left = node, key, literalNode);
    //        var right = Bypass(binaryOperation.Right, (node) => binaryOperation.Right = node, key, literalNode);
    //        return left || right;
    //    }

    //    return false;
    //}

    private void AddAnd(ComparisonNode comparisonNode)
    {
        if (_ast is null)
        {
            _ast = comparisonNode;
        }
        else
        {
            var andNode = BinaryOperationNode.New(_ast, BinaryOperationType.And, comparisonNode);
            _ast = andNode;
        }
    }
}
