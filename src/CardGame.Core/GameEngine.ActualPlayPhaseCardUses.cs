namespace CardGame.Core;
/// <summary>Immutable content opt-in, independent of any general identity.</summary>
internal interface IActualPlayPhaseUseLedgerOperation
{
}

public sealed record ActualPlayPhaseCardUseState(int ActorSeat, int TurnNumber, int PhaseInstanceId, int UseCount);
public sealed record ActualPlayPhaseCardUseRecordedEvent(ActualPlayPhaseCardUseState State, long CardActionId) : IGameEvent
{
    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public CardKind? EffectiveCardKind { get; init; }
}

public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.HashSet<long> _actualPlayPhaseUseActions => ref _actualPlayPhaseCardUses.ReferenceToActualPlayPhaseUseActions;
}
