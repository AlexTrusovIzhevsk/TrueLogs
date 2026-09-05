using Fluxor;
using TrueLogs.Api.Mappers;
using TrueLogs.Contract.Web.Logs;
using TrueLogs.Web.Store.Exceptions;
using TrueLogs.Web.Store.LogsFeature.Actions;
using TrueLogs.Web.Store.LogsFeature.States;

namespace TrueLogs.Web.Store.LogsFeature.Effects;

public class Effect(ILogProviderClient _client)
{
    [EffectMethod]
    public async Task HandleFetchUserInfosAction(LoadLogsAction action, IDispatcher dispatcher)
    {
        try
        {
            var response = await _client.Get(
                action.Skip, 
                action.Take,
                action.Query,
                CancellationToken.None
            );

            var resultAction = new LoadedLogsAction(
                Logs: response!.Logs.Map().ToList(),
                TotalCount: response.TotalCount,
                Error: null,
                Skip: action.Skip,
                Take: action.Take
            );

            dispatcher.Dispatch(resultAction);
        }
        catch (Exception exception)
        {
            var errorAction = new LoadedLogsAction(
                Logs: null,
                TotalCount: action.TotalCount,
                Error: exception.Message,
                Skip: action.Skip,
                Take: action.Take
            );

            dispatcher.Dispatch(errorAction);
        }
    }
}
