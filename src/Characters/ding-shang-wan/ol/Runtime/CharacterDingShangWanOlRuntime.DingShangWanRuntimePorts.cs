#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDingShangWanOlRuntime
{
    Winner IDingShangWanNativeFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    long IDingShangWanNativeFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    bool IDingShangWanNativeFacts.CurrentDamageHasSource => _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false };

    DyingFrame? IDingShangWanNativeFacts.ActiveDying => _coreDyingCursorRuntimeOperations.Value.ActiveDying;

    bool IDingShangWanNativeFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool IDingShangWanNativeFacts.HasSkillInstance(int seat, string skill, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skill, instance);
    SkillProgramTrigger IDingShangWanNativeFacts.Trigger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame);
    bool IDingShangWanNativeFacts.IsActualAfterTurnEndedParent(DeferredTurnEndFrame frame) => _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(frame);
    ProgramTriggerCandidate IDingShangWanNativeFacts.AfterTurnEndedCandidate(DeferredTurnEndFrame frame) => _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(frame);
    bool IDingShangWanNativeFacts.ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame frame) => _coreResponseCompletionOperations.ValidResponseCompletionWindow(frame);
    bool IDingShangWanNativeFacts.RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool IDingShangWanNativeFacts.MovementObserverEdge(int child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child);
    bool IDingShangWanNativeFacts.DyingExitEdge(int child) => _characterYanPuOlOperations.IsYanPuDyingExitEdge(child);
    bool IDingShangWanNativeFacts.SuspendedOrPaidRescueSuffix(int child, DyingFrame dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    void IDingShangWanNativeOperations.Give(long frameId, int giver, int recipient, IReadOnlyList<int> cards, CardMoveReason reason) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(recipient), reason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frameId, giver, recipient, cards, _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
}
