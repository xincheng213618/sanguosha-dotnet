#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreActualPhaseEndRuntimePortsRuntime
{
    Winner global::CardGame.Core.IActualPhaseEndFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    bool global::CardGame.Core.IActualPhaseEndFacts.HasLishiCapability => _characterZhongHuiAnniversaryGodOperations.HasLishiCapability;

    bool global::CardGame.Core.IActualPhaseEndFacts.HasMouSimaShi => _characterSimaShiMouOperations.HasMouSimaShi;

    bool global::CardGame.Core.IActualPhaseEndFacts.HasMouHuCheEr => _characterHuCheErMouOperations.HasMouHuCheEr;

    bool global::CardGame.Core.IActualPhaseEndFacts.ModeTracksActualPhaseEnds => _modeRulesOperations._modeRules.RuntimeRequirements.TracksActualPhaseEnds;

    bool global::CardGame.Core.IActualPhaseEndFacts.HasOlLuShi => _characterLuShiOlOperations.HasOlLuShi;

    IReadOnlyList<ActualPhaseEndItem> global::CardGame.Core.IActualPhaseEndIssuedItems.Qingya(ActualDiscardRecoveryPhaseKey phase) => _characterYaDanOlOperations.CollectQingyaPhaseEndCandidates(phase);
    IReadOnlyList<ActualPhaseEndItem> global::CardGame.Core.IActualPhaseEndIssuedItems.MouSimaShi(ActualDiscardRecoveryPhaseKey phase) => _characterSimaShiMouOperations.CollectMouSimaShiPhaseEndCandidates(phase);
    IReadOnlyList<ActualPhaseEndItem> global::CardGame.Core.IActualPhaseEndIssuedItems.ShiHuangZu(ActualDiscardRecoveryPhaseKey phase) => _characterHuangZuMobileShiOperations.CollectShiHzPhaseEndCandidates(phase);
    IReadOnlyList<ActualPhaseEndItem> global::CardGame.Core.IActualPhaseEndIssuedItems.ShiLuYuSheng(ActualDiscardRecoveryPhaseKey phase) => _characterLuYuShengMobileShiOperations.CollectShiLysPhaseEndCandidates(phase);
    void global::CardGame.Core.IActualPhaseEndNativeReturns.BeginTurnStartAfterProgramLifecycle(int actorSeat) => _coreActualTurnsOperations.BeginTurnStartAfterProgramLifecycle(_coreMatchParticipantsPlayersAccess._players[actorSeat]);
    void global::CardGame.Core.IActualPhaseEndNativeReturns.CompleteTurnStart(int actorSeat, int delayedEffects) => _coreActualTurnsOperations.CompleteTurnStart(_coreMatchParticipantsPlayersAccess._players[actorSeat], (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects);
    void global::CardGame.Core.IActualPhaseEndNativeReturns.CompleteTurnStartAfterDraw(int actorSeat, int delayedEffects, bool afterNormalDrawProgramsCompleted, bool drawPhaseEndedProgramsCompleted, bool actualDrawCompletionCompleted) => _coreActualTurnsOperations.CompleteTurnStartAfterDraw(_coreMatchParticipantsPlayersAccess._players[actorSeat], (global::CardGame.Core.GameEngine.DelayedTurnEffects)delayedEffects, afterNormalDrawProgramsCompleted, drawPhaseEndedProgramsCompleted, actualDrawCompletionCompleted);
    void global::CardGame.Core.IActualPhaseEndNativeReturns.CompletePlayPhaseAfterProgramWindow() => _coreProgramLifecycleOperations.CompletePlayPhaseAfterProgramWindow();
    void global::CardGame.Core.IActualPhaseEndNativeReturns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void global::CardGame.Core.IActualPhaseEndNativeReturns.FinalizeEndTurn(int actorSeat) => _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[actorSeat]);
    void global::CardGame.Core.IActualPhaseEndNativeReturns.AdvanceRulesAndPublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IActualPhaseEndNativeReturns.ResumeScheduledProgramPhase() => _programPhaseSchedulingOperations.Get().ResumeScheduledProgramPhase();
}
