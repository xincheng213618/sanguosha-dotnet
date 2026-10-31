#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramLifecycleWindowRuntimePortsRuntime
{
    int? IProgramLifecycleFacts.DyingKillerSeat => _coreDyingCursorRuntimeOperations.Value.ActiveDying?.KillerSeat;

    bool IProgramLifecycleFacts.HasAttributedEventOperations => _coreAttributedMarkerEventsOperations.HasAttributedEventOperations();

    IReadOnlyDictionary<PlayerMarkerKind, int> IProgramLifecycleFacts.MarkerCounts(int seat) => new Dictionary<PlayerMarkerKind, int>(_coreMatchParticipantsPlayersAccess._players[seat].Markers);
    bool IProgramLifecycleFacts.IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame) => _coreHellActivitiesOperations.IsExactHellAppearanceWindow(frame);
    bool IProgramLifecycleContexts.IsIssuedDiciChainTrigger(SkillProgramTrigger trigger) => _characterZhaoZhongOlOperations.IsIssuedDiciChainTrigger(trigger);
    ProgramSkillWindowContext IProgramLifecycleContexts.IssuedDiciChain(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterZhaoZhongOlOperations.CreateIssuedDiciChainContext(frame, candidate, context);
    ProgramSkillWindowContext IProgramLifecycleContexts.IdentityAppearance(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _coreIdentityConcealmentOperations.CreateIdentityAppearanceContext(frame, candidate);
    ProgramSkillWindowContext IProgramLifecycleContexts.QingzhongEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterLuZhiWeiOlOperations.Value.CreateQingzhongEndingContext(frame, candidate, context);
    ProgramSkillWindowContext IProgramLifecycleContexts.WeiluEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterLvQianOlOperations.CreateWeiluEndingContext(frame, candidate, context);
    ProgramSkillWindowContext IProgramLifecycleContexts.XuRongDying(ProgramSkillWindowContext context) => _characterXuRongOlOperations.CreateXuRongDyingContext(context);
    ProgramSkillWindowContext IProgramLifecycleContexts.ZhuijiEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreZhuijiProgramsOperations.Value.CreateZhuijiEndingContext(frame, candidate, context);
    ProgramSkillWindowContext IProgramLifecycleContexts.PhaseNamePrediction(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePhaseNamePredictionOperations.AttachPhaseNamePredictionContext(candidate, context);
    ProgramSkillWindowContext IProgramLifecycleContexts.NativeDraw(ProgramLifecycleTriggerWindowFrame frame, ProgramSkillWindowContext context) => _coreNativeDrawAttemptsOperations.AttachNativeDrawContext(frame, context);
    void IProgramLifecycleReturns.DrawPhaseObligation(ProgramLifecycleTriggerWindowFrame frame) => _coreDrawPhaseObligationsOperations.Value.ReturnDrawPhaseObligationWindow(frame);
    bool IProgramLifecycleReturns.PipelineCompleted(ProgramLifecycleTriggerWindowFrame frame) => _programPipelineHooksOperations._programPipelineHooks.LifecycleWindowCompleted(frame);
    void IProgramLifecycleReturns.NativeDrawAttempt(ProgramLifecycleTriggerWindowFrame frame) => _coreNativeDrawAttemptsOperations.ReturnNativeDrawAttemptWindow(frame);
    void IProgramLifecycleReturns.RoundEnding(ProgramLifecycleTriggerWindowFrame frame) => _coreRoundEndingOperations.Value.ReturnRoundEndingLifecycle(frame);
    void IProgramLifecycleReturns.YanPuDyingExit(ProgramLifecycleTriggerWindowFrame frame) => _characterYanPuOlOperations.ReturnYanPuDyingExit(frame);
    void IProgramLifecycleReturns.AfterForeignActualContests(int seat) => _coreActualTurnsOperations.ContinueTurnAfterForeignActualContests(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IProgramLifecycleReturns.DyingEntry(ProgramLifecycleTriggerWindowFrame frame) => _coreDyingEntryProgramsOperations.CompleteDyingEntryProgramWindow(frame);
    void IProgramLifecycleReturns.AfterHengye(int seat) => _coreActualTurnsOperations.ContinueTurnAfterHengye(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IProgramLifecycleReturns.ZhouQunPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _characterZhouQunOlOperations.Value.TryReturnZhouQunPaidPreparation(frame);
    bool IProgramLifecycleReturns.DuFuRenPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _characterDuFuRenOlOperations.TryReturnDuFuRenPaidPreparation(frame);
    bool IProgramLifecycleReturns.ChengGongYingPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _characterChengGongYingOlOperations.TryReturnChengGongYingPaidPreparation(frame);
    void IProgramLifecycleReturns.NormalTurnStart(int seat) => _coreProgramLifecycleOperations.BeginNormalTurnStartAfterProgramBindings(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IProgramLifecycleReturns.IdentityAppearance(ProgramLifecycleTriggerWindowFrame frame) => _coreIdentityConcealmentOperations.ResumeIdentityAppearanceWindow(frame);
    void IProgramLifecycleReturns.BeiMiHuPrelude(ProgramLifecycleTriggerWindowFrame frame) => _characterBeiMiHuOlOperations.ReturnBeiMiHuTurnPreludeWindow(frame);
    void IProgramLifecycleReturns.AfterRoundPrograms(int seat) => _coreActualTurnsOperations.ContinueTurnAfterRoundPrograms(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IProgramLifecycleReturns.DrawPhase(ProgramLifecycleTriggerWindowFrame frame, bool actualPhaseSubstituted) => _coreActualTurnsOperations.CompleteDrawPhaseAfterProgramWindow(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkipPlayPhaseAfterDraw, frame.NormalDrawReplaced, frame.NormalDrawAdjustment, frame.FrozenBaseDrawCount, actualPhaseSubstituted);
    void IProgramLifecycleReturns.PlayPhase() => _coreProgramLifecycleOperations.CompletePlayPhaseAfterProgramWindow();
    void IProgramLifecycleReturns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void IProgramLifecycleReturns.JudgmentStarting(ProgramLifecycleTriggerWindowFrame frame) => _coreAlternativePhaseCostProgramsOperations.Value.CompleteActualJudgmentStarting(frame);
    void IProgramLifecycleReturns.CharacterStateChange(ProgramLifecycleTriggerWindowFrame frame) => _coreCharacterStateProgramsOperations.ResumeCharacterStateChange(frame);
    void IProgramLifecycleReturns.DrawPhaseEnded(int seat, int delayedEffects) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[seat], (GameEngine.DelayedTurnEffects)delayedEffects, drawPhaseEndedProgramsCompleted: true);
    void IProgramLifecycleReturns.AfterNormalDraw(int seat) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[seat], _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects, afterNormalDrawProgramsCompleted: true, drawPhaseEndedProgramsCompleted: true);
    void IProgramLifecycleReturns.DiscardPhase(int seat) => _coreActualDiscardPhaseOperations.CompleteDiscardPhaseAfterProgramWindow(_coreMatchParticipantsPlayersAccess._players[seat]);
}
