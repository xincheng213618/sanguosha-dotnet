namespace CardGame.Core;
public sealed record RedAdditionalTargetsGrantedEvent(int OwnerSeat, CardConversionSource Source, int Maximum) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public bool? PersistAfterSkillLoss { get; init; }
}

public sealed record RedAdditionalTargetsConsumedEvent(int OwnerSeat, CardConversionSource Source, IReadOnlyList<int> Targets) : IGameEvent;
public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<int, (global::CardGame.Core.CardConversionSource Source, int Maximum, bool? PersistAfterSkillLoss)> _redAdditionalTargetGrants => ref _redAdditionalTargets.ReferenceToRedAdditionalTargetGrants;
    private ref global::CardGame.Core.CardConversionSource? _selectedRedAdditionalTargetSource => ref _redAdditionalTargets.ReferenceToSelectedRedAdditionalTargetSource;
}
