using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Translator.PostgreSQL;

public class PostgreSQLTranslator : NodeTranslator
{
    protected override NodeVisitor CreateVisitor() =>
        new PostgreSQLVisitor();
}
