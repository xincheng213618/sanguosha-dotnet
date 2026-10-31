#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDingShangWanOlRuntime
{
    Winner global::CardGame.Core.IDingShangWanNativeFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    long global::CardGame.Core.IDingShangWanNativeFacts.MovementSequence => _coreCardMovementJournalMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IDingShangWanNativeFacts.CurrentDamageHasSource => _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false };

    DyingFrame? global::CardGame.Core.IDingShangWanNativeFacts.ActiveDying => _coreDyingCursorRuntimeOperations.ActiveDying;

    bool global::CardGame.Core.IDingShangWanNativeFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.IDingShangWanNativeFacts.HasSkillInstance(int seat, string skill, string instance) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skill, instance);
    SkillProgramTrigger global::CardGame.Core.IDingShangWanNativeFacts.Trigger(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.GetProgramTrigger(frame);
    bool global::CardGame.Core.IDingShangWanNativeFacts.IsActualAfterTurnEndedParent(DeferredTurnEndFrame frame) => _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(frame);
    ProgramTriggerCandidate global::CardGame.Core.IDingShangWanNativeFacts.AfterTurnEndedCandidate(DeferredTurnEndFrame frame) => _coreAfterTurnEndedProgramsOperations.AfterTurnEndedCandidate(frame);
    bool global::CardGame.Core.IDingShangWanNativeFacts.ValidResponseCompletionWindow(ProgramCardTriggerWindowFrame frame) => _coreResponseCompletionOperations.ValidResponseCompletionWindow(frame);
    bool global::CardGame.Core.IDingShangWanNativeFacts.RecoveryReplacementFrameRidesOn(ResolutionFrame child, ResolutionFrame parent) => _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, parent);
    bool global::CardGame.Core.IDingShangWanNativeFacts.MovementObserverEdge(int child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child);
    bool global::CardGame.Core.IDingShangWanNativeFacts.DyingExitEdge(int child) => _characterYanPuOlOperations.IsYanPuDyingExitEdge(child);
    bool global::CardGame.Core.IDingShangWanNativeFacts.SuspendedOrPaidRescueSuffix(int child, DyingFrame dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying);
    void global::CardGame.Core.IDingShangWanNativeOperations.Give(long frameId, int giver, int recipient, IReadOnlyList<int> cards, CardMoveReason reason) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards, CardLocation.Hand(recipient), reason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frameId, giver, recipient, cards, _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
}
