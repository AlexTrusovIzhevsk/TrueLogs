namespace TrueLogs.Web.Store.LogsFeature.Actions;

public record AddFilterAction(
    string Key,
    string Value,
    Property Property
);

public enum FilterType
{
    None = 0,
    Properties = 1,
    Level = 2,
    Timestamp = 3,
    Message = 4,
}

public enum OperationType
{
    None = 0,
    Equal = 1,
    Greater = 2,
    Less = 3,
    Contains = 4,
}

public record Property(
    OperationType OperationType,
    FilterType FilterType
);
