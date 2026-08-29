using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes;

public abstract class Node 
{
    public abstract string Accept(NodeVisitor visitor, TranslationContext context);
}
