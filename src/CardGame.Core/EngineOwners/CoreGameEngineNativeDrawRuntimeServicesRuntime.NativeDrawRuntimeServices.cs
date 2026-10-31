#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawRuntimeServicesRuntime
{
    int INativeDrawProducerQueries.ResolveParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    int INativeDrawProducerQueries.BoundCardCount(ProgramSkillFrame frame, string bind) => _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind).CardIds.Count;
    int INativeDrawProducerQueries.PhaseSkillUsage(ProgramSkillFrame frame, string usageId) => _coreProgramCategoryTargetLedgerOperations.GetProgramPhaseSkillUsage(frame, usageId);
    bool INativeDrawProducerQueries.HasAttemptCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability;

    bool INativeDrawProducerQueries.IsLiushiClearDraw(ProgramSkillFrame frame) => _coreLiushiEffectsOperations.Value.IsLiushiClearDraw(frame);
    bool INativeDrawProducerQueries.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    bool INativeDrawProducerQueries.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool INativeDrawProducerQueries.HasSkillInstance(int seat, string skillId, string skillInstanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, skillInstanceId);
    bool INativeDrawProducerQueries.CanInvokeProgramSkill(ProgramSkillFrame frame) => _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame);
    bool INativeDrawProducerQueries.CanApplyPrepDiscardImmediateDraw(ProgramSkillFrame frame) => _corePrepDiscardImmediateDrawOperations.Value.CanApplyPrepDiscardImmediateDraw(frame);
    Winner INativeDrawProducerQueries.Winner => _coreMatchOutcomeWinnerAccess._winner;

    long INativeDrawProducerQueries.PairBenefitSequence => _corePairObtainShownGiftsOperations.PairBenefitSequence;

    long INativeDrawProducerQueries.MovementSequence => _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;

    ProgramExecutionPlan INativeDrawProducerQueries.ResolvePlan(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
    bool INativeDrawProducerQueries.HasOperationReturn(SkillProgramEffectOp operation) => CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
    int? INativeDrawProducerOperations.ResolveDrawCount(ProgramSkillFrame frame, int targetSeat, int amount, SkillProgramNumberExpression? expression) => _coreProgramLifecycleOperations.ResolveProgramDrawCount(frame, targetSeat, amount, expression);
    void INativeDrawProducerOperations.DrawSelectedTargets(long id, int count, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramSelectedTargets(id, count, reason);
    void INativeDrawProducerOperations.DrawProgramCards(long id, int seat, int count, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason) => _coreProgramLifecycleOperations.DrawProgramCards(id, seat, count, null, resultBind, visibility, reason);
    IReadOnlyList<int> INativeDrawProducerOperations.DrawCards(int seat, int count, bool log, CardMoveReason reason) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], count, log, reason);
    void INativeDrawProducerOperations.ReturnOperation(SkillProgramEffectOp operation, ProgramSkillFrame frame, NativeDrawOutcome outcome) => _nativeDrawReturnsOperations.ReturnOperation(operation, frame, outcome);
    void INativeDrawProducerOperations.CaptureLiushiPayment(ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced) => _coreLiushiEffectsOperations.Value.CaptureLiushiClearDrawPayment(frame, requested, before, after, actual, attemptId, replaced);
    void INativeDrawProducerOperations.CompleteDrawCards(long id, int seat, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason, long fixedRecipientSequenceBefore, IReadOnlyList<int> drawn) => _coreProgramLifecycleOperations.CompleteProgramDrawCards(id, seat, resultBind, visibility, reason, fixedRecipientSequenceBefore, drawn);
    void INativeDrawProducerOperations.CancelBinding(ProgramSkillFrame frame, string reason) => _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, reason);
    bool INativeDrawProducerOperations.BeginRecoveryReplacement(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    bool INativeDrawProducerOperations.BeginCharacterStateWindow(long id) => _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    bool INativeDrawProducerOperations.BeginHpChangedWindow(long id) => _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    bool INativeDrawProducerOperations.BeginCardsMovedWindow(long id) => _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id);
    bool INativeDrawProducerOperations.BeginSkillsChangedWindow(long id) => _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    bool INativeDrawAttemptQueries.HasAttemptCapability => _coreNativeDrawAttemptsOperations.HasNativeDrawAttemptCapability;

    bool INativeDrawAttemptQueries.SetupComplete => _coreMatchSetupSetupCompleteAccess._setupComplete;

    bool INativeDrawAttemptQueries.IsAlive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool INativeDrawAttemptQueries.IsValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    Winner INativeDrawAttemptQueries.Winner => _coreMatchOutcomeWinnerAccess._winner;

    int INativeDrawAttemptQueries.TurnNumber => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int INativeDrawAttemptQueries.TurnOwnerSeat => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    ActualDiscardRecoveryPhaseKey? INativeDrawAttemptQueries.ActualPhase => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase();

    long INativeDrawAttemptQueries.MovementSequence => _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;

    SkillProgramTriggerFacts INativeDrawAttemptQueries.CaptureTriggerFacts(int seat) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[seat]);
    IReadOnlyList<ProgramTriggerCandidate> INativeDrawAttemptQueries.CollectCandidates(int seat, SkillProgramTriggerFacts facts) => _programPipelineHooksOperations._programPipelineHooks.IncludeNativeDrawCandidates(seat, _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramTriggerWindow.NativeDrawStarting, facts));
    IEnumerable<CardMovementRecord> INativeDrawAttemptQueries.Movements()
    {
        foreach (var movement in _coreCardMovementJournalCardMovementsAccess._cardMovements)
            yield return movement;
    }

    bool INativeDrawAttemptQueries.IsRedirectedGain(CardMovementRecord movement, CardLocation expectedTo) => _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, expectedTo);
    NativeDrawPileSlashProof? INativeDrawAttemptQueries.DrawPileSlashReplacementProof(ProgramSkillFrame frame, NativeDrawAttemptContext context) => _characterZhangFeiOlMagicOperations.Value.NativeDrawPileSlashReplacementProof(frame, context);
    bool INativeDrawAttemptQueries.IsIssuedDrawPileSlashReplacement(NativeDrawAttemptContext context, NativeDrawReplacementResult replacement) => _characterZhangFeiOlMagicOperations.Value.IsIssuedNativeDrawPileSlashReplacement(context, replacement);
    void INativeDrawAttemptOperations.CompleteLingkongReplacement(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement) => _characterZhouFeiLeOperations.CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
}
