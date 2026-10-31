#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramPhaseSchedulingRuntimePortsRuntime
{
    TurnPhase IProgramPhaseQueries.Phase => _coreActualTurnProgressPhaseAccess._phase;

    Winner IProgramPhaseSchedulingFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool IProgramPhaseSchedulingFacts.MatchesAfterTurnEndedChild(DeferredTurnEndFrame parent, ProgramSkillFrame child, ProgramSkillWindowContext context) => _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, child, context);
    bool IProgramPhaseSchedulingFacts.HasRuntimeSkillInstance(int seat, string skillId, string instanceId) => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], skillId, instanceId);
    bool IProgramPhaseSchedulingFacts.IsReturningGrantedEntityPhase(ProgramSkillFrame frame) => _coreGrantedEntityPhasesOperations.IsReturningGrantedEntityPhase(frame);
    ProgramPhaseSchedule? IProgramPhaseScheduleState.Current { get => _programPhaseScheduleOperations._programPhaseSchedule; set => _programPhaseScheduleOperations._programPhaseSchedule = value; }

    void IProgramPhaseScheduleState.RestorePhase(TurnPhase phase) => _coreActualTurnProgressPhaseAccess._phase = phase;
    void IProgramPhaseNativeEntries.EnterPlayPhase(int seat) => _coreActualTurnsOperations.EnterPlayPhase(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IProgramPhaseNativeEntries.TryBeginDrawPhaseProgramWindow(int seat) => _coreProgramLifecycleOperations.TryBeginDrawPhaseProgramWindow(_coreMatchParticipantsPlayersAccess._players[seat], false);
    void IProgramPhaseNativeEntries.ConsumeDuFuRenDrawPhase(int seat) => _characterDuFuRenOlOperations.ConsumeDuFuRenPhase(ActualDiscardRecoveryPhaseKind.Draw, seat);
    void IProgramPhaseNativeEntries.ConsumeChengGongYingDraw(int seat) => _characterChengGongYingOlOperations.ConsumeChengGongYingDraw(seat);
    void IProgramPhaseNativeEntries.DrawNormalCards(int seat) => _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[seat], _characterCaoXiOlOperations.FreezeAndCaptureCaoXiNativeDrawBase(_coreMatchParticipantsPlayersAccess._players[seat]), true);
    void IProgramPhaseNativeEntries.CompleteTurnStartAfterDraw(int seat) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[seat], GameEngine.DelayedTurnEffects.None);
    bool IProgramPhaseNativeReturns.TryResumeZhangShiPingHongjiPhase() => _characterZhangShiPingOlOperations.TryResumeZhangShiPingHongjiPhase();
    bool IProgramPhaseNativeReturns.TryCancelZhangShiPingHongjiPhase() => _characterZhangShiPingOlOperations.TryCancelZhangShiPingHongjiPhase();
    ProgramSkillFrame IProgramPhaseNativeReturns.FreezeGrantedEntityPhaseEnd(ProgramSkillFrame frame) => _coreGrantedEntityPhasesOperations.FreezeGrantedEntityPhaseEnd(frame);
    void IProgramPhaseNativeReturns.ResumeZhangYiDianjunExtraPlay(long frameId) => _characterZhangYiOlOperations.ResumeZhangYiDianjunExtraPlay(frameId);
    void IProgramPhaseNativeReturns.CompleteDetachedProgramBinding(ProgramSkillFrame frame, bool completed) => _coreProgramLifecycleOperations.CompleteDetachedProgramBinding(frame, completed);
    void IProgramPhaseNativeReturns.ExpireActualTurnEndEffects(int seat) => _coreActualTurnsOperations.ExpireActualTurnEndEffects(_coreMatchParticipantsPlayersAccess._players[seat]);
    void IProgramPhaseNativeReturns.ExpirePlayPhaseSkillGrantsForEndedTurn(int turnNumber) => _corePlayPhaseSkillGrantsOperations.ExpirePlayPhaseSkillGrantsForEndedTurn(turnNumber);
    void IProgramPhaseNativeReturns.ExpireActualTurnSkillGrants(int turnNumber, int seat) => _coreActualTurnSkillGrantsOperations.ExpireActualTurnSkillGrants(turnNumber, seat);
    bool IProgramPhasePipelineHooks.ResumeScheduledPhase() => _programPipelineHooksOperations._programPipelineHooks.ResumeScheduledPhase();
    bool IProgramPhasePipelineHooks.CancelScheduledPhase() => _programPipelineHooksOperations._programPipelineHooks.CancelScheduledPhase();
    bool IProgramPhasePipelineHooks.ContinueDetachedProgramBinding(ProgramSkillFrame frame) => _programPipelineHooksOperations._programPipelineHooks.ContinueDetachedProgramBinding(frame);
}
