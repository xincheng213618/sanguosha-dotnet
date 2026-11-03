using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramInsufficientCardBehavior { CancelBinding, BindEmpty }

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramExpression? CountExpression { get; internal init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramInsufficientCardBehavior? OnInsufficient { get; internal init; }
}

internal static class ProgramOwnedCardRequests
{
    internal static bool IsExact(SkillProgramEffect effect) => effect.CountExpression is not null ||
        effect.Amount != 1 || effect.ExceptBind is not null ||
        effect.OnInsufficient is not null;
}
