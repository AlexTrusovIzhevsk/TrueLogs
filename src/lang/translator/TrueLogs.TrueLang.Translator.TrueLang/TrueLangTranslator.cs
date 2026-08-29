using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Translator.TrueLang;

public class TrueLangTranslator : NodeTranslator
{
    protected override NodeVisitor CreateVisitor()
        => new TrueLangVisitor();
}
