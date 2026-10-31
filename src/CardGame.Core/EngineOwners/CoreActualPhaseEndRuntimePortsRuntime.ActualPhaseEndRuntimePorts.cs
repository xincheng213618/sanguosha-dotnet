#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualPhaseEndRuntimePortsRuntime
{
    Winner IActualPhaseEndFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool IActualPhaseEndFacts.HasLishiCapability => _characterZhongHuiAnniversaryGodOperations.HasLishiCapability;

    bool IActualPhaseEndFacts.HasMouSimaShi => _characterSimaShiMouOperations.HasMouSimaShi;

    bool IActualPhaseEndFacts.HasMouHuCheEr => _characterHuCheErMouOperations.HasMouHuCheEr;

    bool IActualPhaseEndFacts.ModeTracksActualPhaseEnds => _modeRulesOperations._modeRules.RuntimeRequirements.TracksActualPhaseEnds;

    bool IActualPhaseEndFacts.HasOlLuShi => _characterLuShiOlOperations.Value.HasOlLuShi;

    IReadOnlyList<ActualPhaseEndItem> IActualPhaseEndIssuedItems.Qingya(ActualDiscardRecoveryPhaseKey phase) => _characterYaDanOlOperations.CollectQingyaPhaseEndCandidates(phase);
    IReadOnlyList<ActualPhaseEndItem> IActualPhaseEndIssuedItems.MouSimaShi(ActualDiscardRecoveryPhaseKey phase) => _characterSimaShiMouOperations.CollectMouSimaShiPhaseEndCandidates(phase);
    IReadOnlyList<ActualPhaseEndItem> IActualPhaseEndIssuedItems.ShiHuangZu(ActualDiscardRecoveryPhaseKey phase) => _characterHuangZuMobileShiOperations.CollectShiHzPhaseEndCandidates(phase);
    IReadOnlyList<ActualPhaseEndItem> IActualPhaseEndIssuedItems.ShiLuYuSheng(ActualDiscardRecoveryPhaseKey phase) => _characterLuYuShengMobileShiOperations.Value.CollectShiLysPhaseEndCandidates(phase);
    void IActualPhaseEndNativeReturns.BeginTurnStartAfterProgramLifecycle(int actorSeat) => _coreActualTurnsOperations.BeginTurnStartAfterProgramLifecycle(_coreMatchParticipantsPlayersAccess._players[actorSeat]);
    void IActualPhaseEndNativeReturns.CompleteTurnStart(int actorSeat, int delayedEffects) => _coreActualTurnsOperations.CompleteTurnStart(_coreMatchParticipantsPlayersAccess._players[actorSeat], (GameEngine.DelayedTurnEffects)delayedEffects);
    void IActualPhaseEndNativeReturns.CompleteTurnStartAfterDraw(int actorSeat, int delayedEffects, bool afterNormalDrawProgramsCompleted, bool drawPhaseEndedProgramsCompleted, bool actualDrawCompletionCompleted) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[actorSeat], (GameEngine.DelayedTurnEffects)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    void IActualPhaseEndNativeReturns.CompletePlayPhaseAfterProgramWindow() => _coreProgramLifecycleOperations.CompletePlayPhaseAfterProgramWindow();
    void IActualPhaseEndNativeReturns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void IActualPhaseEndNativeReturns.FinalizeEndTurn(int actorSeat) => _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[actorSeat]);
    void IActualPhaseEndNativeReturns.AdvanceRulesAndPublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool IActualPhaseEndNativeReturns.ResumeScheduledProgramPhase() => _programPhaseSchedulingOperations.Get().ResumeScheduledProgramPhase();
}
