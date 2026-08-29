namespace TrueLogs.Web.Store.Exceptions;

public class LogicException : Exception
{
    private LogicException(string text) : base(text) { }

    public static LogicException New(string text)
    {
        return new LogicException(text);
    }
}
