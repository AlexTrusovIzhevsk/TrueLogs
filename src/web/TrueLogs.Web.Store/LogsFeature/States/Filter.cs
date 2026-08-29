using TrueLogs.Web.Store.LogsFeature.Actions;

namespace TrueLogs.Web.Store.LogsFeature.States;

public record Filter(
    string Key,
    string Value,
    Property Property
);
