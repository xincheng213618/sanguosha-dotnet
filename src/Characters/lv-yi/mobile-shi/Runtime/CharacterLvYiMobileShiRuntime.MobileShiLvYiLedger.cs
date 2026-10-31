#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvYiMobileShiRuntime
{
    internal bool HasShiLyCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiLyHuilv);

    internal bool IsShiLySongyanDrawTwo(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0, CardKind: CardKind.DrawTwo, PhysicalCardIds: { Count: 0 }, ShiLySongyanOrigin: { UseId: var use }, } && use == id;
    internal bool ShiLyShishiCannotRespond(int sourceSeat, int responderSeat)
    {
        if (!HasShiLyCapability || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || sourceSeat == responderSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(responderSeat) || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[sourceSeat], ShiLyShishi))
            return false;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyShishiRecordedEvent>().Any(entry => entry.OwnerSeat == sourceSeat && entry.SourceSeat == responderSeat))
            return false;
        return ShiLyShishiUseUnanswered(sourceSeat);
    }

    internal bool ShiLyShishiUseUnanswered(int ownerSeat)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        for (var i = history.Length - 1; i >= 0; i--)
        {
            if (history[i] is CardRespondedEvent { SourceSeat: var responder } && responder == ownerSeat)
                return false;
            if (history[i] is CardUseDeclaredEvent { SourceSeat: var user } && user == ownerSeat)
                return true;
        }

        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 68621)]
    internal void ObserveShiLyShishiDamage(DamageAppliedEvent damage)
    {
        if (!HasShiLyCapability || damage.Amount <= 0 || damage.SourceLess || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(damage.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(damage.TargetSeat) || damage.SourceSeat == damage.TargetSeat || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[damage.TargetSeat], ShiLyShishi) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyShishiRecordedEvent>().Any(entry => entry.OwnerSeat == damage.TargetSeat && entry.SourceSeat == damage.SourceSeat))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLyShishiRecordedEvent(damage.TargetSeat, damage.SourceSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber));
    }

    // ---- trigger qualification gates --------------------------------------------
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68600, SkillProgramEffectOp.ResolveShiLyHuilv)]
    internal bool CanOfferShiLyHuilv(ProgramTriggerLegalityContext c) => c.SkillId == ShiLvYiComposition.HuilvSkill && c.Owner.IsAlive && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && c.Context is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, TargetSeat: { } actor } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && actor != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLyHuilvGrantedEvent>().Count(entry => entry.OwnerSeat == c.OwnerSeat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber) < 2;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68601, SkillProgramEffectOp.ResolveShiLyHuilvSettle)]
    internal bool CanOfferShiLyHuilvSettle(ProgramTriggerLegalityContext c) => c.SkillId == ShiLvYiComposition.HuilvSkill && c.Owner.IsAlive && _coreActualRoundProgressRoundNumberAccess._roundNumber > 0 && c.Context is { Window: SkillProgramTriggerWindow.PlayEnding, TargetSeat: { } actor } && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && ShiLyOpenGrant(c.OwnerSeat, actor)is not null;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 68602, SkillProgramEffectOp.ResolveShiLySongyan)]
    internal bool CanOfferShiLySongyan(ProgramTriggerLegalityContext c) => c.SkillId == ShiLvYiComposition.SongyanSkill && c.Owner.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && c.Context is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } or { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 };
}
