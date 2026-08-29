using TrueLogs.TrueLang.Parsers.Nodes;

namespace TrueLogs.TrueLang.Translators;

public class TranslationContext
{
    public required Node Parent { get; init; }

    public static TranslationContext New(Node parrent) =>
        new TranslationContext { Parent = parrent };
}
