#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordFacts(global::CardGame.Core.EngineStates.CoreCardMovementJournalState cardMovementJournalState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.ModeRuleSet> getModeRuleSet) : IMovementRecordFacts
{
    public int Sequence => cardMovementJournalState.ReferenceToMovementSequence;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public bool PublishesDisclosure => getModeRuleSet().PublishesCardMovementDisclosure;
    public bool Started => actualTurnProgressState.ReferenceToStarted;
    public CardMoveReason SimultaneousTopReservationReason => global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsDefinitions.SimultaneousTopReservationReason;
    public string PrivatePindianReserveReason => global::CardGame.Core.EngineOwners.CharacterLuKangWumiaoRuntime.WuLuKangReserveReason;
}
