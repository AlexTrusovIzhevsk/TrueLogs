using Fluxor;
using TrueLogs.TrueLang.Lexers;
using TrueLogs.TrueLang.Parsers;
using TrueLogs.TrueLang.Parsers.Nodes;
using TrueLogs.TrueLang.Parsers.Nodes.ComparisonNodes;
using TrueLogs.TrueLang.Rewriters;
using TrueLogs.TrueLang.Translator.TrueLang;
using TrueLogs.Web.Store.Exceptions;
using TrueLogs.Web.Store.LogsFeature.Actions;
using TrueLogs.Web.Store.LogsFeature.States;

namespace TrueLogs.Web.Store.LogsFeature.Effects;

public class Reducer
{
    [ReducerMethod]
    public static LogsFeatureState ReduceFilters(LogsFeatureState state, AddFilterAction action)
    {
        var query = state.Query;

        var lexer = new Lexer(query);
        var parser = new Parser(lexer);
        var ast = parser.Parse();

        var rewriter = new Rewriter(ast);
        var comparisonType = Map(action.Property.OperationType);

        if (action.Property.FilterType == FilterType.Timestamp)
        {
            var date = DateTime.Parse(action.Value);
            rewriter.AddComparison(action.Key, comparisonType, date);
        }
        else
        {
            rewriter.AddComparison(action.Key, comparisonType, action.Value);
        }
        

        ast = rewriter.Result;

        if  (ast is not null)
        {
            var translator = new TrueLangTranslator();
            query = translator.Translate(ast);
        }

        return state with
        {
            Query = query
        };
    }

    private static ComparisonType Map(OperationType operationType) => operationType switch
    {
        OperationType.None => ComparisonType.None,
        OperationType.Equal => ComparisonType.Equals,
        OperationType.Less => ComparisonType.Less,
        OperationType.Greater => ComparisonType.Greater,
        OperationType.Contains => ComparisonType.Contains,
        _ => throw LogicException.New($"Failed maping '{operationType}'"),
    };

    //private static Func<string, ComparisonType, string, Node> Map(FilterType filterType) => filterType switch
    //{
    //    FilterType.None => () => ,
    //    _ => throw LogicException.New($"Failed maping '{filterType}'"),
    //};

    [ReducerMethod]
    public static LogsFeatureState ReduceLoadLogsAction(LogsFeatureState state, LoadLogsAction action) 
        => state with 
        { 
            IsLoading = true,
            Query = action.Query,
        };

    [ReducerMethod]
    public static LogsFeatureState ReduceLoadedLogsAction(LogsFeatureState state, LoadedLogsAction action)
    {
        var logs = action.Logs;

        var isNotFirstPage = action.Skip != 0;
        if (isNotFirstPage)
        {
            logs = state.Logs
                .Concat(logs ?? [])
                .ToList();
        }

        return state with
        {
            IsLoading = false,
            Logs = logs,
            Error = action.Error,
            Skip = action.Skip,
            Take = action.Take,
        };
    }
}
