#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordFacts(EngineStates.CoreCardMovementJournalState cardMovementJournalState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<ModeRuleSet> getModeRuleSet) : IMovementRecordFacts
{
    public int Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public bool PublishesDisclosure => getModeRuleSet().PublishesCardMovementDisclosure;
    public bool Started => actualTurnProgressState.ReferenceToStarted;
    public CardMoveReason SimultaneousTopReservationReason => EngineOwners.CoreGameEngineSimultaneousPindianSelectionsDefinitions.SimultaneousTopReservationReason;
    public string PrivatePindianReserveReason => EngineOwners.CharacterLuKangWumiaoRuntime.WuLuKangReserveReason;
}
