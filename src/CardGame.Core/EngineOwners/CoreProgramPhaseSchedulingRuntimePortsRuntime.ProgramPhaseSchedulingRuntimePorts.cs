#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPhaseSchedulingRuntimePortsRuntime
{
    TurnPhase global::CardGame.Core.IProgramPhaseQueries.Phase => _coreActualTurnProgressPhaseAccess._phase;

    Winner global::CardGame.Core.IProgramPhaseSchedulingFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.IProgramPhaseSchedulingFacts.MatchesAfterTurnEndedChild(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context) => _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, child, context);
    bool global::CardGame.Core.IProgramPhaseSchedulingFacts.HasRuntimeSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    bool global::CardGame.Core.IProgramPhaseSchedulingFacts.IsReturningGrantedEntityPhase(ProgramSkillFrame frame) => _coreGrantedEntityPhasesOperations.IsReturningGrantedEntityPhase(frame);
    ProgramPhaseSchedule? global::CardGame.Core.IProgramPhaseScheduleState.Current { get => _programPhaseScheduleOperations._programPhaseSchedule; set => _programPhaseScheduleOperations._programPhaseSchedule = value; }

    void global::CardGame.Core.IProgramPhaseScheduleState.RestorePhase(TurnPhase phase) => _coreActualTurnProgressPhaseAccess._phase = phase;
    void global::CardGame.Core.IProgramPhaseNativeEntries.EnterPlayPhase(int seat) => _coreActualTurnsOperations.EnterPlayPhase(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IProgramPhaseNativeEntries.TryBeginDrawPhaseProgramWindow(int seat) => _coreProgramLifecycleOperations.TryBeginDrawPhaseProgramWindow(_coreMatchParticipantsPlayersAccess._players[seat], false);
    void global::CardGame.Core.IProgramPhaseNativeEntries.ConsumeDuFuRenDrawPhase(int seat) => _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    void global::CardGame.Core.IProgramPhaseNativeEntries.ConsumeChengGongYingDraw(int seat) => _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(seat);
    void global::CardGame.Core.IProgramPhaseNativeEntries.DrawNormalCards(int seat) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[seat]), true);
    void global::CardGame.Core.IProgramPhaseNativeEntries.CompleteTurnStartAfterDraw(int seat) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[seat], global::CardGame.Core.GameEngine.DelayedTurnEffects.None);
    bool global::CardGame.Core.IProgramPhaseNativeReturns.TryResumeZhangShiPingHongjiPhase() => _characterZhangShiPingOlOperations.TryResumeZhangShiPingHongjiPhase();
    bool global::CardGame.Core.IProgramPhaseNativeReturns.TryCancelZhangShiPingHongjiPhase() => _characterZhangShiPingOlOperations.TryCancelZhangShiPingHongjiPhase();
    ProgramSkillFrame global::CardGame.Core.IProgramPhaseNativeReturns.FreezeGrantedEntityPhaseEnd(ProgramSkillFrame frame) => _coreGrantedEntityPhasesOperations.FreezeGrantedEntityPhaseEnd(frame);
    void global::CardGame.Core.IProgramPhaseNativeReturns.ResumeZhangYiDianjunExtraPlay(long frameId) => _characterZhangYiOlOperations.ResumeZhangYiDianjunExtraPlay(frameId);
    void global::CardGame.Core.IProgramPhaseNativeReturns.CompleteDetachedProgramBinding(ProgramSkillFrame frame, bool completed) => _coreProgramLifecycleOperations.CompleteDetachedProgramBinding(frame, completed);
    void global::CardGame.Core.IProgramPhaseNativeReturns.ExpireActualTurnEndEffects(int seat) => _coreActualTurnsOperations.ExpireActualTurnEndEffects(_coreMatchParticipantsPlayersAccess._players[seat]);
    void global::CardGame.Core.IProgramPhaseNativeReturns.ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber) => _corePlayPhaseSkillGrantsOperations.ExpirePlayPhaseSkillGrantsForEndedTurn(turnNumber);
    void global::CardGame.Core.IProgramPhaseNativeReturns.ExpireActualTurnSkillGrants(int turnNumber, int seat) => _coreActualTurnSkillGrantsOperations.ExpireActualTurnSkillGrants(turnNumber, seat);
    bool global::CardGame.Core.IProgramPhasePipelineHooks.ResumeScheduledPhase() => _programPipelineHooksOperations._programPipelineHooks.ResumeScheduledPhase();
    bool global::CardGame.Core.IProgramPhasePipelineHooks.CancelScheduledPhase() => _programPipelineHooksOperations._programPipelineHooks.CancelScheduledPhase();
    bool global::CardGame.Core.IProgramPhasePipelineHooks.ContinueDetachedProgramBinding(ProgramSkillFrame frame) => _programPipelineHooksOperations._programPipelineHooks.ContinueDetachedProgramBinding(frame);
}
