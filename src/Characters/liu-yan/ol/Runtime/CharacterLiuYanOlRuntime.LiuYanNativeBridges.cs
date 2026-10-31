#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuYanOlRuntime
{
    internal bool MatchesLiuYanLimu(ProgramSkillFrame f, LiuYanLimuUseOrigin o) => f.LiuYan is { Stage: LiuYanStage.LimuNative, Use: { } expected } r && o == expected && o.ProgramFrameId == f.Id && o.Source == r.Source && o.GameplayHash == r.GameplayHash && ValidLiuYan(f);
    internal void BeginLiuYanLimu(ProgramSkillFrame f, LiuYanLimuUseOrigin o, Card card)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !MatchesLiuYanLimu(f, o) || o.CardUseFrameId != _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1 || o.ActionId != _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence + 1)
            throw new InvalidOperationException("Limu lost its reserved physical native use.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, o.ActorSeat, [o.ActorSeat], CardKind.Indulgence, conversionSource: o.Source, liuYanLimu: o);
        if (id != o.CardUseFrameId || _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.Action?.ActionId != o.ActionId)
            throw new InvalidOperationException("Limu changed its native use or action identity.");
        _corePhysicalMovementFacadesOperations.MoveCard(card, o.From, CardLocation.Processing, CardMoveReasons.Use, m =>
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
            if (parent.LiuYan!.PaymentSequence is not null || m.CardId != o.CardId || m.CardKind != o.PrintedKind || m.From != o.From || m.To != CardLocation.Processing || m.Reason != CardMoveReasons.Use)
                throw new InvalidOperationException("Limu needs one exact original payment.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { LiuYan = parent.LiuYan with { PaymentSequence = m.Sequence, After = m.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLimuPaidEvent(o, m.Sequence));
        });
        var current = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(current.Id, current with { LiuYan = current.LiuYan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (!IsLiuYanLimuUse(id))
        {
            var actual = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
            var a = actual.Action!;
            var p = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
            throw new InvalidOperationException($"Limu issue proof failed: receipt={(ValidLiuYan(p))},origin={(actual.LiuYanLimu == o)},kind={(actual.CardKind)}/{(a.EffectiveKind)},suit={(a.EffectiveSuit)},rank={(a.EffectiveRank)}/{(o.Rank)},red={(a.EffectiveIsRed)},provider={(a.ProviderSeat)}/{(o.ActorSeat)},declared={(_coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == id))},actor={(_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(actual, o.ActorSeat, o.ActorSeat))},cost={(a.PhysicalCards.Count)},conversion={(a.ConversionChain.SequenceEqual([o.Source]))},from={((a.PhysicalCards.Count == 1 ? a.PhysicalCards[0].From.ToString() : "none"))},expected={(o.From)}");
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, o.ActorSeat, [o.ActorSeat], LegalActionKind.Indulgence, playedCardKind: CardKind.Indulgence);
    }

    internal bool HasPaidLiuYanInitialSelection(ProgramSkillFrame f) => f.LiuYan is { Operation: SkillProgramEffectOp.UseLimuSelfIndulgenceAndRecover, PaymentSequence: not null, Use: { } o } && f.SelectedCardIds.SequenceEqual([o.CardId]) && ValidLiuYan(f);
    internal bool IsLiuYanLimuUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { LiuYanLimu: { } o, CardKind: CardKind.Indulgence, Action: { Type: CardActionType.Use } a } use || use.CardId != o.CardId || id != o.CardUseFrameId || a.ActionId != o.ActionId || a.EffectiveKind != CardKind.Indulgence || use.PhysicalCardIds is not [var paid] || paid != o.CardId || a.PhysicalCards is not [var cost] || cost.CardId != o.CardId || cost.CardKind != o.PrintedKind || cost.From != o.From || a.EffectiveSuit != Suit.Diamond || a.EffectiveRank != o.Rank || a.EffectiveIsRed != true || !a.ConversionChain.SequenceEqual([o.Source]) || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || a.ResponderSeat is not null || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat) || !a.TargetSeats.SequenceEqual(use.TargetSeats))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id);
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MatchesLiuYanLimu(f, o);
    }

    internal bool IsLiuYanLimuSelfDelayedUse(NullificationWindowFrame p) => p.EffectCardKind == CardKind.Indulgence && p.ActionKind == LegalActionKind.Indulgence && IsLiuYanLimuUse(p.ParentFrameId) && _coreCardUseLifecycleOperations.LifecycleCardUse(p.ParentFrameId)is { LiuYanLimu: { } o } && p.EffectCardId == o.CardId && global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(p) == o.ActorSeat;
    internal bool TrySkipUnavailableLiuYanLimuEffect(NullificationWindowFrame p)
    {
        if (!IsLiuYanLimuSelfDelayedUse(p))
            return false;
        var o = _coreCardUseLifecycleOperations.LifecycleCardUse(p.ParentFrameId)!.LiuYanLimu!;
        var target = _coreMatchParticipantsPlayersAccess._players[o.ActorSeat];
        if (target.IsAlive && !target.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(target, CardKind.Indulgence) && _cardZonesOperations._cardZones.GetLocation(o.CardId) == CardLocation.Processing)
            return false;
        if (_cardZonesOperations._cardZones.GetLocation(o.CardId) != CardLocation.Processing && !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(p.ParentFrameId, o.CardId))
            throw new InvalidOperationException("Limu's paid physical material moved without a native entity claim.");
        _coreNativeTrickEffectsOperations.SkipUnavailableTargetCardEffect(p, target, CardEffectSkipReason.SkillNullified);
        return true;
    }

    [NativeUseValidation(27231)]
    internal void ValidateLiuYanLimu(CardUseFrame use)
    {
        if (use.LiuYanLimu is not null && !IsLiuYanLimuUse(use.Id))
            throw new InvalidOperationException("Limu changed its original diamond material, self-use origin, legal actor chain or exact parent.");
    }

    [NativeUseReturn(27231)]
    internal void ReturnLiuYanLimu(CardUseFrame use)
    {
        if (use.LiuYanLimu is not { } o)
            return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !MatchesLiuYanLimu(f, o) || f.LiuYan!.PaymentSequence is null || _coreProgramEventHistoryOperations.ProgramEventHistory<LiuYanLimuReturnedEvent>().Any(e => e.Origin == o))
            throw new InvalidOperationException("Limu must return exactly once after its physical card use finishes.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuYanLimuReturnedEvent(o));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuYan = f.LiuYan!with { Stage = LiuYanStage.LimuReturnedChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueLiuYan(f.Id);
    }
}
