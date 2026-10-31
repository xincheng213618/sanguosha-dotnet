#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawRuntimeServicesRuntime
{
    int global::CardGame.Core.INativeDrawProducerQueries.ResolveParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    int global::CardGame.Core.INativeDrawProducerQueries.BoundCardCount(ProgramSkillFrame frame, string bind) => _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind).CardIds.Count;
    int global::CardGame.Core.INativeDrawProducerQueries.PhaseSkillUsage(ProgramSkillFrame frame, string usageId) => _coreProgramCategoryTargetLedgerOperations.GetProgramPhaseSkillUsage(frame, usageId);
    bool global::CardGame.Core.INativeDrawProducerQueries.HasAttemptCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability;

    bool global::CardGame.Core.INativeDrawProducerQueries.IsLiushiClearDraw(ProgramSkillFrame frame) => _coreLiushiEffectsOperations.IsLiushiClearDraw(frame);
    bool global::CardGame.Core.INativeDrawProducerQueries.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool global::CardGame.Core.INativeDrawProducerQueries.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.INativeDrawProducerQueries.HasSkillInstance(int seat, string skillId, string skillInstanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, skillInstanceId);
    bool global::CardGame.Core.INativeDrawProducerQueries.CanApplyPrepDiscardImmediateDraw(ProgramSkillFrame frame) => _corePrepDiscardImmediateDrawOperations.CanApplyPrepDiscardImmediateDraw(frame);
    Winner global::CardGame.Core.INativeDrawProducerQueries.Winner => _coreMatchOutcomeWinnerAccess._winner;

    long global::CardGame.Core.INativeDrawProducerQueries.PairBenefitSequence => _corePairObtainShownGiftsOperations.PairBenefitSequence;

    long global::CardGame.Core.INativeDrawProducerQueries.MovementSequence => _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;

    ProgramExecutionPlan global::CardGame.Core.INativeDrawProducerQueries.ResolvePlan(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
    bool global::CardGame.Core.INativeDrawProducerQueries.HasOperationReturn(SkillProgramEffectOp operation) => global::CardGame.Core.EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
    IEnumerable<NativeDrawAttemptDecidedEvent> global::CardGame.Core.INativeDrawProducerQueries.DecidedEvents() => _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>();
    int? global::CardGame.Core.INativeDrawProducerOperations.ResolveDrawCount(ProgramSkillFrame frame, int targetSeat, int amount, SkillProgramNumberExpression? expression) => _coreProgramLifecycleOperations.ResolveProgramDrawCount(frame, targetSeat, amount, expression);
    void global::CardGame.Core.INativeDrawProducerOperations.DrawSelectedTargets(long id, int count, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramSelectedTargets(id, count, reason);
    void global::CardGame.Core.INativeDrawProducerOperations.DrawProgramCards(long id, int seat, int count, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramCards(id, seat, count, null, resultBind, visibility, reason);
    IReadOnlyList<int> global::CardGame.Core.INativeDrawProducerOperations.DrawCards(int seat, int count, bool log, CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], count, log, reason);
    void global::CardGame.Core.INativeDrawProducerOperations.ReturnOperation(SkillProgramEffectOp operation, ProgramSkillFrame frame, NativeDrawOutcome outcome) => _nativeDrawReturnsOperations.ReturnOperation(operation, frame, outcome);
    void global::CardGame.Core.INativeDrawProducerOperations.CaptureLiushiPayment(ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced) => _coreLiushiEffectsOperations.CaptureLiushiClearDrawPayment(frame, requested, before, after, actual, attemptId, replaced);
    void global::CardGame.Core.INativeDrawProducerOperations.CompleteDrawCards(long id, int seat, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason, long fixedRecipientSequenceBefore, IReadOnlyList<int> drawn) => _coreProgramLifecycleOperations.CompleteProgramDrawCards(id, seat, resultBind, visibility, reason, fixedRecipientSequenceBefore, drawn);
    void global::CardGame.Core.INativeDrawProducerOperations.CancelBinding(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    bool global::CardGame.Core.INativeDrawProducerOperations.BeginRecoveryReplacement(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    bool global::CardGame.Core.INativeDrawProducerOperations.BeginCharacterStateWindow(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    bool global::CardGame.Core.INativeDrawProducerOperations.BeginHpChangedWindow(long id) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    bool global::CardGame.Core.INativeDrawProducerOperations.BeginCardsMovedWindow(long id) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    bool global::CardGame.Core.INativeDrawProducerOperations.BeginSkillsChangedWindow(long id) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    bool global::CardGame.Core.INativeDrawAttemptQueries.HasAttemptCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability;

    bool global::CardGame.Core.INativeDrawAttemptQueries.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    bool global::CardGame.Core.INativeDrawAttemptQueries.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.INativeDrawAttemptQueries.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    Winner global::CardGame.Core.INativeDrawAttemptQueries.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int global::CardGame.Core.INativeDrawAttemptQueries.TurnNumber => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.INativeDrawAttemptQueries.TurnOwnerSeat => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    ActualDiscardRecoveryPhaseKey? global::CardGame.Core.INativeDrawAttemptQueries.ActualPhase => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();

    long global::CardGame.Core.INativeDrawAttemptQueries.MovementSequence => _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;

    SkillProgramTriggerFacts global::CardGame.Core.INativeDrawAttemptQueries.CaptureTriggerFacts(int seat) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.INativeDrawAttemptQueries.CollectCandidates(int seat, SkillProgramTriggerFacts facts) => _programPipelineHooksOperations._programPipelineHooks.IncludeNativeDrawCandidates(seat, _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramTriggerWindow.NativeDrawStarting, facts));
    IEnumerable<CardMovementRecord> global::CardGame.Core.INativeDrawAttemptQueries.Movements()
    {
        foreach (var movement in _coreCardMovementJournalCardMovementsAccess._cardMovements)
            yield return movement;
    }

    bool global::CardGame.Core.INativeDrawAttemptQueries.IsRedirectedGain(CardMovementRecord movement, CardLocation expectedTo) => _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, expectedTo);
    IEnumerable<NativeDrawAttemptStartedEvent> global::CardGame.Core.INativeDrawAttemptQueries.StartedEvents() => _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptStartedEvent>();
    IEnumerable<NativeDrawAttemptDecidedEvent> global::CardGame.Core.INativeDrawAttemptQueries.DecidedEvents() => _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>();
    NativeDrawPileSlashProof? global::CardGame.Core.INativeDrawAttemptQueries.DrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => _characterZhangFeiOlMagicOperations.NativeDrawPileSlashReplacementProof(frame, context);
    bool global::CardGame.Core.INativeDrawAttemptQueries.IsIssuedDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => _characterZhangFeiOlMagicOperations.IsIssuedNativeDrawPileSlashReplacement(context, replacement);
    void global::CardGame.Core.INativeDrawAttemptOperations.CompleteLingkongReplacement(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement) => _characterZhouFeiLeOperations.CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
}
