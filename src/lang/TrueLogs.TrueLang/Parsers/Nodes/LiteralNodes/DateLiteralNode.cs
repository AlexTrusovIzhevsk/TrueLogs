using System.Globalization;
using TrueLogs.TrueLang.Translators;

namespace TrueLogs.TrueLang.Parsers.Nodes.LiteralNodes;

public class DateLiteralNode : LiteralNode
{
    public DateTime Value { get; set; }

    public override string Accept(NodeVisitor visitor, TranslationContext context)
        => visitor.Visit(this, context);

    public static DateLiteralNode New(string value) => new()
    {
        Value = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)
    };

    public static DateLiteralNode New(DateTime value) => new()
    {
        Value = value
    };
}