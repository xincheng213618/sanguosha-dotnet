#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoTuOlRuntime
{

    internal bool HasWeijieSpecializedPayment(ProgramSkillFrame f, CardUseFrame? use = null)
    {
        if (_characterLiuFengBoundaryOperations.HasBoundaryLiuFengProgramPayment(f, use))
            return true;
        if (f.WeijieReturn is not { } r || r.Origin is not { } p || !ValidWeijiePayment(p) || p.EffectiveName != global::CardGame.Core.EngineOwners.CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(p.OutputKind) || p.Need.OwnerFrameId != f.Id || p.Need.RequestFrameId != f.Id || p.Need.InstructionIndex != f.InstructionIndex || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) != 1)
            return false;
        if (use is not null && (use.WeijieOrigin != p || r.UseId != use.Id || r.ActionId != use.Action?.ActionId || !IsIssuedWeijieUse(use)))
            return false;
        if (_characterSunYuanOlOperations.TryGetSayingSlashRequest(f, out _, out _))
            return _characterSunYuanOlOperations.HasSayingWeijiePayment(f, use);
        if (f.Chanshuang is { } cs)
            return _characterLiYiOlOperations.TryGetChanshuangSlashRequest(f, out var actor, out var targets) && actor == p.Need.ActorSeat && targets.SequenceEqual([p.TargetSeat]) && cs.Stage is ChanshuangStage.SlashIssuing or ChanshuangStage.NativeUse or ChanshuangStage.NativeChildren && cs.Selection is { Materials.Count: 0, Sources: [var csSource] } selection && csSource == p.Source && selection.ActorSeat == p.Need.ActorSeat && selection.ProviderSeat == p.Need.ProviderSeat && selection.TargetSeat == p.TargetSeat && selection.Kind == p.OutputKind && (cs.Stage != ChanshuangStage.NativeChildren || r.Returned && cs.NativeReturned && cs.Native is { } csNative && csNative.UseId == r.UseId && csNative.ActionId == r.ActionId && csNative.Selection == selection && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieUseReturnedEvent>().Count(e => e.AttemptId == p.AttemptId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ChanshuangSlashReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == cs.Cursor && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1);
        if (f.WuJing is { } wu)
            return wu.Source == _characterWuJingOlOperations.WuJingSource(f) && wu.GameplayHash == f.GameplayHash && wu.Stage is WuJingStage.IssuingUse or WuJingStage.NativeUse or WuJingStage.NativeChildren && wu.TargetSeat == p.TargetSeat && f.OwnerSeat == p.Need.ActorSeat && wu.Selection is { CardIds.Count: 0, Conversions: [var source] } && source == p.Source && wu.Selection.Kind == p.OutputKind && (wu.Stage != WuJingStage.NativeChildren || r.Returned && wu.NativeCompleted && wu.NativeOrigin is { } native && native.UseId == r.UseId && native.ActionId == r.ActionId && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieUseReturnedEvent>().Count(e => e.AttemptId == p.AttemptId && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuJingHejiReturnedEvent>().Count(e => e.ProgramFrameId == f.Id && e.UseId == r.UseId) == 1);
        if (f.LvKuangLvXiang is { } qi)
            return qi.Source == global::CardGame.Core.EngineOwners.CharacterLvKuangLvXiangOlRuntime.LvKuangLvXiangSource(f) && qi.GameplayHash == f.GameplayHash && qi.Stage is "issuing" or "native" && qi.Actor == p.Need.ActorSeat && qi.Target == p.TargetSeat && qi.IssuingProvider == p.Need.ProviderSeat && qi.IssuingIds.Count == 0 && qi.IssuingKind == p.OutputKind;
        return f.CaoChun is { Stage: CaoChunStage.NativeSlash, Slash: { } n } cc && cc.Source == global::CardGame.Core.EngineOwners.CharacterCaoChunOlRuntime.CaoChunSource(f) && cc.GameplayHash == f.GameplayHash && n.ParentFrameId == f.Id && !n.Ending && n.Materials.Count == 0 && n.Kind == p.OutputKind && n.TargetSeat == p.TargetSeat && f.OwnerSeat == p.Need.ActorSeat && n.ProviderSeat == p.Need.ProviderSeat;
    }

    internal bool HasAcceptedWeijieKeAction(ProgramSkillFrame f, LegalAction a)
    {
        if (f.KeBiNengPingduan is not { } r || f.WeijieKeAcceptedOrigin is not { } p || !ValidWeijiePayment(p) || p.Need.Intent != WeijieIntent.GenericBasic || p.Need.OwnerFrameId != f.Id || p.Need.InstructionIndex != f.InstructionIndex || p.Need.ActorSeat != r.ActorSeat || p.Need.ProviderSeat != r.ActorSeat || a.CardId != 0 || a.SelectableCardIds.Count != 0 || a.ConversionSource != p.Source || a.PlayedCardKind != p.OutputKind || a.Kind != (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(p.OutputKind) ? LegalActionKind.Slash : p.OutputKind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol) || !a.TargetSeats.SequenceEqual(p.OutputKind == CardKind.Alcohol ? [] : new[] { p.TargetSeat }))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieKeActionAcceptedEvent>().Count(e => e == new WeijieKeActionAcceptedEvent(f.Id, f.InstructionIndex, r.Source, f.GameplayHash, p.AttemptId, p.OutputKind, p.TargetSeat)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieAttemptResolvedEvent>().Count(e => e.FrameId == p.AttemptId && e.Matched && e.NativeLegal) == 1;
    }

    internal bool IsWeijieOrdinaryAlcoholQuery(CharacterState actor) => ActiveWeijieDraft()is { Weijie: { } r } f && f.OwnerSeat == actor.Seat && r.Need.Intent == WeijieIntent.GenericBasic && r.Need.ActorSeat == actor.Seat && r.Need.ProviderSeat == actor.Seat && WeijieParent(f) && _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == r.Need.OwnerFrameId)is ProgramSkillFrame { KeBiNengPingduan: { Stage: KeBiNengPingduanStage.BasicChoice, ActorSeat: var seat } } && seat == actor.Seat;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27463)]
    internal void ExpireWeijieAlcohol(ActualTurnCursorReturnedEvent returned)
    {
        if (!HasWeijieCapability)
            return;
        foreach (var issued in _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieUseIssuedEvent>().Where(e => e.OutputKind == CardKind.Alcohol))
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<WeijieCostPaidEvent>().SingleOrDefault(e => e.FrameId == issued.AttemptId && e.ActualTurn == returned.ActualTurn && e.ActualTurnOwner == returned.OwnerSeat);
            if (paid is null || issued.ActorSeat == returned.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[issued.ActorSeat].HasAlcoholEffect || !_coreProgramEventHistoryOperations.ProgramEventHistory<AlcoholAppliedEvent>().Any(e => e.ResolutionId == issued.UseId && e.SourceSeat == issued.ActorSeat) || !_coreProgramEventHistoryOperations.ProgramEventHistory<JianjiOrdinaryAlcoholPaidEvent>().Any(e => e.NativeUseId == issued.UseId && e.ActualTurn == returned.ActualTurn))
                continue;
            _coreMatchParticipantsPlayersAccess._players[issued.ActorSeat].HasAlcoholEffect = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(issued.ActorSeat));
        }
    }
}
