using TrueLogs.TrueLang.Parsers.Nodes;

namespace TrueLogs.TrueLang.Translators;

public abstract class NodeTranslator
{
    public string Translate(Node node)
    {
        var visitor = CreateVisitor();
        var context = TranslationContext.New(node);
        var result = node.Accept(visitor, context);
        return result;
    }

    protected abstract NodeVisitor CreateVisitor();
}