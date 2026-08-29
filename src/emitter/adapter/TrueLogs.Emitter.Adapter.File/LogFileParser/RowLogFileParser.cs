using TrueLogs.Contract.Emitter.Dto;
using TrueLogs.Contract.Emitter.Enums;
using TrueLogs.Emitter.Adapter.File.LogFileConfig;

namespace TrueLogs.Emitter.Adapter.File.LogFileParser;

public class RowLogFileParser : ILogFileParser
{
    private readonly ILogFileConfigRedaer _logFileConfig;

    public RowLogFileParser(ILogFileConfigRedaer logFileConfig)
    {
        _logFileConfig = logFileConfig;
    }

    /// <summary>
    /// Код сгенерирован дип сик, нужно переписать
    /// </summary>
    public Task<LogEmitterContract> Parse(string rowLog, CancellationToken token)
    {
        var result = new LogEmitterContract();

        // Парсим временную метку: "2026-05-04 21:27:03.607 +03:00 [WRN]"
        var timestampEndIndex = rowLog.IndexOf('[') - 1;
        var timestampPart = rowLog.Substring(0, timestampEndIndex).Trim();
        var levelStartIndex = rowLog.IndexOf('[');
        var levelEndIndex = rowLog.IndexOf(']');
        var levelCode = rowLog.Substring(levelStartIndex + 1, levelEndIndex - levelStartIndex - 1);

        // Разбираем временную метку
        var timestampParts = timestampPart.Split(' ');
        var dateTimePart = $"{timestampParts[0]} {timestampParts[1]}";

        result.Timestamp = DateTime.ParseExact(dateTimePart, "yyyy-MM-dd HH:mm:ss.fff", null);
        result.Level = ParseLevel(levelCode);

        // Получаем текст сообщения (всё после уровня логирования)
        var messagePart = rowLog.Substring(levelEndIndex + 1).Trim();

        // В простых логах шаблон сообщения совпадает с отображённым сообщением
        result.RenderedMessage = messagePart;
        result.MessageTemplate = messagePart;

        // Извлекаем свойства, если они есть (например, в фигурных скобках)
        result.Properties = new Dictionary<string, object?>();

        // Проверяем наличие структурированных данных вида {action = "GetAll", controller = "LogProvider"}
        if (messagePart.Contains("{"))
        {
            ExtractPropertiesFromMessage(messagePart, result.Properties);
        }

        // Устанавливаем источник из контекста логирования
        result.Source = _logFileConfig.GetConfig().Key;

        // Эти поля отсутствуют в вашем формате, оставляем пустыми
        result.Exception = null;
        result.Environment = null;
        result.TraceId = null;
        result.SpanId = null;

        return Task.FromResult(result);
    }

    private LevelEmitterContract ParseLevel(string levelCode)
    {
        return levelCode switch
        {
            "WRN" => LevelEmitterContract.Warning,
            "INF" => LevelEmitterContract.Information,
            "DBG" => LevelEmitterContract.Debug,
            "ERR" => LevelEmitterContract.Error,
            "FAT" => LevelEmitterContract.Fatal,
            "TRC" => LevelEmitterContract.Trace,
            _ => LevelEmitterContract.Information
        };
    }

    private void ExtractPropertiesFromMessage(string message, Dictionary<string, object?> properties)
    {
        // Ищем блок в фигурных скобках
        var startIndex = message.IndexOf('{');
        var endIndex = message.LastIndexOf('}');

        if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
        {
            var propertiesPart = message.Substring(startIndex + 1, endIndex - startIndex - 1);
            var propertyPairs = propertiesPart.Split(',');

            foreach (var pair in propertyPairs)
            {
                var keyValue = pair.Split('=', StringSplitOptions.TrimEntries);
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim().Trim('"');
                    properties[key] = value;
                }
            }
        }
    }
}
