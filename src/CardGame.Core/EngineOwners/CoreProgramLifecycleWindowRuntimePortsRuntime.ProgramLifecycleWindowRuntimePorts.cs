#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramLifecycleWindowRuntimePortsRuntime
{
    int? global::CardGame.Core.IProgramLifecycleFacts.DyingKillerSeat => _coreDyingCursorRuntimeOperations.ActiveDying?.KillerSeat;

    bool global::CardGame.Core.IProgramLifecycleFacts.HasAttributedEventOperations => _coreAttributedMarkerEventsOperations.HasAttributedEventOperations();

    IReadOnlyDictionary<PlayerMarkerKind, int> global::CardGame.Core.IProgramLifecycleFacts.MarkerCounts(int seat) => new Dictionary<PlayerMarkerKind, int>(_coreMatchParticipantsPlayersAccess._players[seat].Markers);
    bool global::CardGame.Core.IProgramLifecycleFacts.IsExactHellAppearanceWindow(ProgramLifecycleTriggerWindowFrame frame) => _coreHellActivitiesOperations.IsExactHellAppearanceWindow(frame);
    bool global::CardGame.Core.IProgramLifecycleContexts.IsIssuedDiciChainTrigger(SkillProgramTrigger trigger) => _characterZhaoZhongOlOperations.IsIssuedDiciChainTrigger(trigger);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.IssuedDiciChain(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterZhaoZhongOlOperations.CreateIssuedDiciChainContext(frame, candidate, context);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.IdentityAppearance(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _coreIdentityConcealmentOperations.CreateIdentityAppearanceContext(frame, candidate);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.QingzhongEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterLuZhiWeiOlOperations.CreateQingzhongEndingContext(frame, candidate, context);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.WeiluEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _characterLvQianOlOperations.CreateWeiluEndingContext(frame, candidate, context);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.XuRongDying(ProgramSkillWindowContext context) => _characterXuRongOlOperations.CreateXuRongDyingContext(context);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.ZhuijiEnding(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _coreZhuijiProgramsOperations.CreateZhuijiEndingContext(frame, candidate, context);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.PhaseNamePrediction(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _corePhaseNamePredictionOperations.AttachPhaseNamePredictionContext(candidate, context);
    ProgramSkillWindowContext global::CardGame.Core.IProgramLifecycleContexts.NativeDraw(ProgramLifecycleTriggerWindowFrame frame, ProgramSkillWindowContext context) => _coreNativeDrawAttemptsOperations.AttachNativeDrawContext(frame, context);
    void global::CardGame.Core.IProgramLifecycleReturns.DrawPhaseObligation(ProgramLifecycleTriggerWindowFrame frame) => _coreDrawPhaseObligationsOperations.ReturnDrawPhaseObligationWindow(frame);
    bool global::CardGame.Core.IProgramLifecycleReturns.PipelineCompleted(ProgramLifecycleTriggerWindowFrame frame) => _programPipelineHooksOperations._programPipelineHooks.LifecycleWindowCompleted(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.NativeDrawAttempt(ProgramLifecycleTriggerWindowFrame frame) => _coreNativeDrawAttemptsOperations.ReturnNativeDrawAttemptWindow(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.RoundEnding(ProgramLifecycleTriggerWindowFrame frame) => _coreRoundEndingOperations.ReturnRoundEndingLifecycle(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.YanPuDyingExit(ProgramLifecycleTriggerWindowFrame frame) => _characterYanPuOlOperations.ReturnYanPuDyingExit(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.AfterForeignActualContests(int seat) => _coreActualTurnsOperations.ContinueTurnAfterForeignActualContests(_coreMatchParticipantsPlayersAccess._players[seat]);
    void global::CardGame.Core.IProgramLifecycleReturns.DyingEntry(ProgramLifecycleTriggerWindowFrame frame) => _coreDyingEntryProgramsOperations.CompleteDyingEntryProgramWindow(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.AfterHengye(int seat) => _coreActualTurnsOperations.ContinueTurnAfterHengye(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IProgramLifecycleReturns.ZhouQunPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _characterZhouQunOlOperations.TryReturnZhouQunPaidPreparation(frame);
    bool global::CardGame.Core.IProgramLifecycleReturns.DuFuRenPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _characterDuFuRenOlOperations.TryReturnDuFuRenPaidPreparation(frame);
    bool global::CardGame.Core.IProgramLifecycleReturns.ChengGongYingPaidPreparation(ProgramLifecycleTriggerWindowFrame frame) => _characterChengGongYingOlOperations.TryReturnChengGongYingPaidPreparation(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.NormalTurnStart(int seat) => _coreProgramLifecycleOperations.BeginNormalTurnStartAfterProgramBindings(_coreMatchParticipantsPlayersAccess._players[seat]);
    void global::CardGame.Core.IProgramLifecycleReturns.IdentityAppearance(ProgramLifecycleTriggerWindowFrame frame) => _coreIdentityConcealmentOperations.ResumeIdentityAppearanceWindow(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.BeiMiHuPrelude(ProgramLifecycleTriggerWindowFrame frame) => _characterBeiMiHuOlOperations.ReturnBeiMiHuTurnPreludeWindow(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.AfterRoundPrograms(int seat) => _coreActualTurnsOperations.ContinueTurnAfterRoundPrograms(_coreMatchParticipantsPlayersAccess._players[seat]);
    void global::CardGame.Core.IProgramLifecycleReturns.DrawPhase(ProgramLifecycleTriggerWindowFrame frame, bool actualPhaseSubstituted) => _coreActualTurnsOperations.CompleteDrawPhaseAfterProgramWindow(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkipPlayPhaseAfterDraw, frame.NormalDrawReplaced, frame.NormalDrawAdjustment, frame.FrozenBaseDrawCount, actualPhaseSubstituted);
    void global::CardGame.Core.IProgramLifecycleReturns.PlayPhase() => _coreProgramLifecycleOperations.CompletePlayPhaseAfterProgramWindow();
    void global::CardGame.Core.IProgramLifecycleReturns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void global::CardGame.Core.IProgramLifecycleReturns.JudgmentStarting(ProgramLifecycleTriggerWindowFrame frame) => _coreAlternativePhaseCostProgramsOperations.CompleteActualJudgmentStarting(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.CharacterStateChange(ProgramLifecycleTriggerWindowFrame frame) => _coreCharacterStateProgramsOperations.ResumeCharacterStateChange(frame);
    void global::CardGame.Core.IProgramLifecycleReturns.DrawPhaseEnded(int seat, int delayedEffects) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[seat], (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects, drawPhaseEndedProgramsCompleted: true);
    void global::CardGame.Core.IProgramLifecycleReturns.AfterNormalDraw(int seat) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[seat], _coreDelayedTurnEffectsPendingTurnDelayedEffectsAccess._pendingTurnDelayedEffects, afterNormalDrawProgramsCompleted: true, drawPhaseEndedProgramsCompleted: true);
    void global::CardGame.Core.IProgramLifecycleReturns.DiscardPhase(int seat) => _coreActualDiscardPhaseOperations.CompleteDiscardPhaseAfterProgramWindow(_coreMatchParticipantsPlayersAccess._players[seat]);
}
