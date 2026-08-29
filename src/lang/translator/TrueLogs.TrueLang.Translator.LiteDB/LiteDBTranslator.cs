using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Translator.LiteDB;

public class LiteDBTranslator : NodeTranslator
{
    protected override NodeVisitor CreateVisitor() =>
        new LiteDBVisitor();
}
