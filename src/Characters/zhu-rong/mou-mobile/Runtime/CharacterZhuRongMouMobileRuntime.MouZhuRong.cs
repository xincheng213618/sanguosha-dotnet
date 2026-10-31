#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRongMouMobileRuntime
{
    internal bool HasMouZhuRong => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiftMouZhuRongOutsideAssault);
    internal int MouZhuRongTurnOrdinal => _coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Count();

    internal static CardConversionSource MouZhuRongSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhu_rong_mou_mobile_MouZhuRong.MouZhuRongSource(f);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30400, SkillProgramEffectOp.RecordMouZhuRongLieren, SkillProgramEffectOp.SelectMouZhuRongLierenTarget, SkillProgramEffectOp.GiftMouZhuRongOutsideAssault)]
    internal bool CanOfferMouZhuRong(ProgramTriggerCandidate candidate, SkillProgramTrigger t, ProgramSkillWindowContext c)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.GiftMouZhuRongOutsideAssault))
            return c.Window == SkillProgramTriggerWindow.TurnEnding && c.OwnerSeat == candidate.OwnerSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongAssaultUsedEvent>().Any(e => e.TurnOrdinal == MouZhuRongTurnOrdinal && e.ActorSeat == candidate.OwnerSeat);
        if (c.CardUse is not { } a || a.ActorSeat != candidate.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind))
            return false;
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.RecordMouZhuRongLieren))
            return c.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _coreCardUseLifecycleOperations.LifecycleCardUse(a.ParentCardUseFrameId)is { Action: { Type: CardActionType.Use } action } use && use.TargetSeats is [var target] && target != candidate.OwnerSeat && c.TargetSeat == target && action.ActionId == a.CardActionId && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenWonEvent>().Any(e => e.UseId == use.Id && e.Source.OwnerSeat == candidate.OwnerSeat);
        return c.Window == SkillProgramTriggerWindow.CardUseCompleted && MouZhuRongWin(candidate.OwnerSeat, c)is { } win && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongLierenDamageChosenEvent>().Any(e => e.UseId == win.UseId && e.OwnerSeat == candidate.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != candidate.OwnerSeat && p.Seat != win.OpponentSeat);
    }

    void global::CardGame.Core.IMouZhuRongHost.RecordLieren(ProgramSkillFrame f) => RecordMouZhuRongLieren(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouZhuRongHost.SelectLierenTarget(ProgramSkillFrame f) => SelectMouZhuRongLierenTarget(f);
    SkillProgramStepOutcome global::CardGame.Core.IMouZhuRongHost.GiftAssault(ProgramSkillFrame f) => GiftMouZhuRongOutsideAssault(f);
    IReadOnlyList<int> global::CardGame.Core.IMouZhuRongLierenTargets.OtherTargets(int owner) => MouZhuRongLierenOtherTargets(owner);
}
