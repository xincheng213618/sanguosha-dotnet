#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeDrawProducerServices(Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime,Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime,Func<RuntimeEntries.CoreGameEngineProgramCategoryTargetLedgerEntries> getCoreGameEngineProgramCategoryTargetLedgerRuntime,Func<RuntimeEntries.CoreGameEngineNativeDrawAttemptsEntries> getCoreGameEngineNativeDrawAttemptsRuntime,Func<RuntimeEntries.CoreGameEngineLiushiEffectsEntries> getCoreGameEngineLiushiEffectsRuntime,Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime,EngineStates.CoreMatchParticipantsState matchParticipantsState,Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime,Func<RuntimeEntries.CoreGameEnginePrepDiscardImmediateDrawEntries> getCoreGameEnginePrepDiscardImmediateDrawRuntime,EngineStates.CoreMatchOutcomeState matchOutcomeState,Func<RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime,Func<RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardEntries> getCoreGameEngineOutsidePhaseDrawDiscardRuntime,Func<ContentRegistry> getContentRegistry,Func<RuntimeEntries.CorePhysicalDrawFacadesEntries> getCorePhysicalDrawFacadesRuntime,GetNativeReturnOperationTarget getReturnOperationTarget,Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime,Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime,Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime,Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime,Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime) : INativeDrawProducerQueries, INativeDrawProducerOperations
{
    public int ResolveParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) => getCoreGameEngineProgramCardPaymentsRuntime().ResolveProgramParticipant(frame, reference);
    public int BoundCardCount(ProgramSkillFrame frame, string bind) => getCoreGameEngineProgramLifecycleRuntime().GetProgramCardSet(frame, bind).CardIds.Count;
    public int PhaseSkillUsage(ProgramSkillFrame frame, string usageId) => getCoreGameEngineProgramCategoryTargetLedgerRuntime().GetProgramPhaseSkillUsage(frame, usageId);
    public bool HasAttemptCapability => getCoreGameEngineNativeDrawAttemptsRuntime().HasNativeDrawAttemptCapability;

    public bool IsLiushiClearDraw(ProgramSkillFrame frame) => getCoreGameEngineLiushiEffectsRuntime().IsLiushiClearDraw(frame);
    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public bool HasSkillInstance(int seat, string skillId, string skillInstanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, skillInstanceId);
    public bool CanApplyPrepDiscardImmediateDraw(ProgramSkillFrame frame) => getCoreGameEnginePrepDiscardImmediateDrawRuntime().CanApplyPrepDiscardImmediateDraw(frame);
    public Winner Winner => matchOutcomeState.ReferenceToWinner;
    public long PairBenefitSequence => getCoreGameEnginePairObtainShownGiftsRuntime().PairBenefitSequence;
    public long MovementSequence => getCoreGameEngineOutsidePhaseDrawDiscardRuntime().OutsidePhaseMovementSequence;

    public ProgramExecutionPlan ResolvePlan(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, getContentRegistry().GetSkill(frame.SkillId).Program!);
    public bool HasOperationReturn(SkillProgramEffectOp operation) => EngineOwners.CoreProgramNativeDrawReturnsDefinitions.ProgramNativeDrawReturns.Value.ContainsKey(operation);
    public int? ResolveDrawCount(ProgramSkillFrame frame, int targetSeat, int amount, SkillProgramNumberExpression? expression) => getCoreGameEngineProgramLifecycleRuntime().ResolveProgramDrawCount(frame, targetSeat, amount, expression);
    public void DrawSelectedTargets(long id, int count, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramSelectedTargets(id, count, reason);
    public void DrawProgramCards(long id, int seat, int count, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason) => getCoreGameEngineProgramLifecycleRuntime().DrawProgramCards(id, seat, count, null, resultBind, visibility, reason);
    public IReadOnlyList<int> DrawCards(int seat, int count, bool log, CardMoveReason reason) => getCorePhysicalDrawFacadesRuntime().DrawCards(matchParticipantsState.ReferenceToPlayers[seat], count, log, reason);
    public void ReturnOperation(SkillProgramEffectOp operation, ProgramSkillFrame frame, NativeDrawOutcome outcome) => getReturnOperationTarget(operation)(frame, outcome);
    public void CaptureLiushiPayment(ProgramSkillFrame frame, int requested, long before, long after, int actual, long? attemptId, bool replaced) => getCoreGameEngineLiushiEffectsRuntime().CaptureLiushiClearDrawPayment(frame, requested, before, after, actual, attemptId, replaced);
    public void CompleteDrawCards(long id, int seat, string? resultBind, SkillProgramCardSetVisibility visibility, CardMoveReason reason, long fixedRecipientSequenceBefore, IReadOnlyList<int> drawn) => getCoreGameEngineProgramLifecycleRuntime().CompleteProgramDrawCards(id, seat, resultBind, visibility, reason, fixedRecipientSequenceBefore, drawn);
    public void CancelBinding(ProgramSkillFrame frame, string reason) => getCoreGameEngineProgramLifecycleRuntime().CancelProgramBindingAndCleanup(frame, reason);
    public bool BeginRecoveryReplacement(long id) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program);
    public bool BeginCharacterStateWindow(long id) => getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program);
    public bool BeginHpChangedWindow(long id) => getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program);
    public bool BeginCardsMovedWindow(long id) => getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(id);
    public bool BeginSkillsChangedWindow(long id) => getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(id);
}
