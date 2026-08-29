using System.Text.Json;
using LiteDB;

namespace TrueLogs.Storage.LiteDB.Logs.Mappers;

internal static class LogBsonDictionaryMapper
{
    internal static void RegisterMapping()
    {
        BsonMapper.Global.RegisterType(Serialize, Deserialize);
    }

    private static BsonValue Serialize(Dictionary<string, object?> dictionary)
    {
        var document = new BsonDocument();
        foreach (var kvp in dictionary)
        {
            document[kvp.Key] = ConvertToBsonValue(kvp.Value);
        }
        return document;
    }

    private static BsonValue ConvertToBsonValue(object? value)
    {
        return value switch
        {
            null => BsonValue.Null,
            string s => new BsonValue(s),
            int i => new BsonValue(i),
            long l => new BsonValue(l),
            bool b => new BsonValue(b),
            DateTime dt => new BsonValue(dt),
            double d => new BsonValue(d),
            decimal dec => new BsonValue((double)dec),
            Dictionary<string, object?> dict => new BsonDocument(dict.ToDictionary(
                kvp => kvp.Key,
                kvp => ConvertToBsonValue(kvp.Value))),
            IEnumerable<object> list => new BsonArray(list.Select(ConvertToBsonValue)),
            JsonElement jsonElement => ConvertJsonElementToBsonValue(jsonElement),
            _ => throw new NotSupportedException(value.GetType().FullName)
        };
    }

    private static BsonValue ConvertJsonElementToBsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Null => BsonValue.Null,
            JsonValueKind.String => new BsonValue(element.GetString()),
            JsonValueKind.Number => element.TryGetInt32(out int intValue)
                ? new BsonValue(intValue)
                : element.TryGetInt64(out long longValue)
                    ? new BsonValue(longValue)
                    : new BsonValue(element.GetDouble()),
            JsonValueKind.True => new BsonValue(true),
            JsonValueKind.False => new BsonValue(false),
            JsonValueKind.Object => new BsonDocument(
                element.EnumerateObject().ToDictionary(
                    prop => prop.Name,
                    prop => ConvertJsonElementToBsonValue(prop.Value))),
            JsonValueKind.Array => new BsonArray(
                element.EnumerateArray().Select(ConvertJsonElementToBsonValue)),
            _ => throw new NotSupportedException($"JsonValueKind {element.ValueKind} не поддерживается")
        };
    }

    private static Dictionary<string, object?> Deserialize(BsonValue bsonValue)
    {
        var dictionary = new Dictionary<string, object?>();
        var document = bsonValue.AsDocument;
        foreach (var key in document.Keys)
        {
            dictionary[key] = ConvertFromBsonValue(document[key]);
        }
        return dictionary;
    }

    private static object? ConvertFromBsonValue(BsonValue bson)
    {
        if (bson.IsNull) return null;
        if (bson.IsString) return bson.AsString;
        if (bson.IsInt32) return bson.AsInt32;
        if (bson.IsInt64) return bson.AsInt64;
        if (bson.IsBoolean) return bson.AsBoolean;
        if (bson.IsDateTime) return bson.AsDateTime;
        if (bson.IsDouble) return bson.AsDouble;
        if (bson.IsDocument)
        {
            var doc = bson.AsDocument;
            return doc.Keys.ToDictionary(key => key, key => ConvertFromBsonValue(doc[key]));
        }
        if (bson.IsArray)
        {
            return bson.AsArray.Select(ConvertFromBsonValue).ToList();
        }
        throw new NotSupportedException(bson.Type.ToString());
    }
}
