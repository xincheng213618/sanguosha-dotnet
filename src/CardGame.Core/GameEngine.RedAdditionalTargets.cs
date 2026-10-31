namespace CardGame.Core;
public sealed record RedAdditionalTargetsGrantedEvent(int OwnerSeat, CardConversionSource Source, int Maximum) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public bool? PersistAfterSkillLoss { get; init; }
}

public sealed record RedAdditionalTargetsConsumedEvent(int OwnerSeat, CardConversionSource Source, IReadOnlyList<int> Targets) : IGameEvent;
