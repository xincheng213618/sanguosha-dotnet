#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZuMobileShiRuntime
{
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 47900)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 47900)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 47900)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 47900)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 47900)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPaidWeaponSlash, 47900)]
    [ProgramDistanceRule(ProgramDistanceStage.SpecificSlashExemption, 47900)]
    internal bool ShiHzSlashDistance(ProgramSlashDistanceQuery q) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(q.Kind) && _coreProgramCardPoliciesOperations.CardPolicies(q.Actor, SkillProgramCardPolicyKind.IgnoreUseDistance, q.Kind).Any(p => p.Source.SkillId == ShiHzChizhang);
    internal bool IsShiHzOriginalUseChoice(CharacterState owner, Card card, CardKind kind)
    {
        if (!owner.IsAlive || card.Kind != kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHuangZu: { Stage: ShiHzStage.IssuingUse } r, WindowContext: { Window: SkillProgramTriggerWindow.ActualPhaseEnded, ActualPhaseEnd: { } due } } f || f.OwnerSeat != owner.Seat || f.SkillId != ShiHzDuanyang || f.GameplayHash != r.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || ShiHzGrant(r.GrantId)is not { } g)
            return false;
        return r.CardId == card.Id && g.CardId == card.Id && r.Source == g.Source && r.GameplayHash == g.GameplayHash && due.EntitlementId == g.GrantId && due.Phase == g.Phase && g.Phase.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && g.Source.OwnerSeat == owner.Seat && _cardZonesOperations._cardZones.GetLocation(card.Id) == g.Pile.Location && _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.GetValueOrDefault((g.Pile.OwnerSeat, g.Pile.SkillId, g.Pile.SkillInstanceId)) == g.Pile && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzGrantConsumedEvent>().Count(e => e.GrantId == g.GrantId && e.FrameId == f.Id) == 1;
    }

    internal bool IsShiHzOriginalUseParent(CardUseFrame use)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost], ConversionChain.Count: 0 } action || action.ProviderSeat != action.ActorSeat || n < 1 || _resolutionStackOperations._resolutionStack[n - 1] is not ProgramSkillFrame { ShiHuangZu: { Stage: ShiHzStage.IssuingUse } r } f || ShiHzGrant(r.GrantId)is not { } g)
            return false;
        return action.ActorSeat == f.OwnerSeat && use.SourceSeat == f.OwnerSeat && use.CardId == r.CardId && cost.CardId == r.CardId && cost.From == g.Pile.Location && cost.CardKind == _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind && use.CardKind == cost.CardKind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && use.TargetSeats.SequenceEqual([r.Target]) && use.PhysicalCardIds is [var id] && id == r.CardId && r.Source == g.Source && r.GameplayHash == g.GameplayHash && r.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzGrantConsumedEvent>().Count(e => e.GrantId == g.GrantId && e.FrameId == f.Id) == 1;
    }

    internal void IssueShiHzNativeUse(ProgramSkillFrame f)
    {
        var r = f.ShiHuangZu!;
        if (r.Stage != ShiHzStage.IssuingUse || !ShiHzTargets(f).Contains(r.Target) || ShiHzGrant(r.GrantId)is not { } g)
            throw new InvalidOperationException("Duanyang cannot replace its stored original Slash or chosen native target.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.Target];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId);
        _coreCardConversionsOperations.SelectUseConversion(new(LegalActionKind.Slash, card.Id, target.Seat, "使用断鞅储存的原牌", card.Kind));
        var id = _coreNativeCardUseOperations.BeginCardUse(card, owner.Seat, [target.Seat], card.Kind, physicalCardIds: [card.Id]);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (!IsShiHzOriginalUseParent(use))
            throw new InvalidOperationException("Duanyang lost its original public-pile material on native declaration.");
        var origin = new ShiHzNativeUse(r.GrantId, f.Id, r.Source, r.GameplayHash, r.ActualTurn, card.Id, card.Kind, g.Pile.Location, target.Seat, id, use.Action!.ActionId);
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = parent with { ShiHuangZu = r with { Stage = ShiHzStage.NativeUse, UseId = id, ActionId = origin.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { ShiHuangZuUse = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHzNativeIssuedEvent(g.GrantId, f.Id, id, origin.ActionId, card.Id, target.Seat));
        _corePhysicalMovementFacadesOperations.MoveCard(card, g.Pile.Location, CardLocation.Processing, CardMoveReasons.Use);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, owner.Seat, target.Seat, card, damageAmount: 1 + owner.AlcoholDamageBonus, playedCardKind: card.Kind, physicalCards: [card], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(owner, target, card.Kind), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(owner.Seat, card.Kind);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, owner);
        owner.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, card.Kind, owner.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(owner, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [target.Seat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool ShiHzUseRidesOn(CardUseFrame use, ProgramSkillFrame f) => use.ShiHuangZuUse is { } n && f.ShiHuangZu is { } r && n.ParentId == f.Id && n.Source == r.Source && n.GameplayHash == f.GameplayHash && n.GrantId == r.GrantId && n.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && n.UseId == use.Id && n.UseId == r.UseId && n.ActionId == use.Action?.ActionId && n.ActionId == r.ActionId && n.CardId == r.CardId && n.Target == r.Target && use.CardKind == n.Kind && use.SourceSeat == f.OwnerSeat && use.CardId == n.CardId && use.PhysicalCardIds is [var id] && id == n.CardId && use.Action is { Type: CardActionType.Use, PhysicalCards: [var cost], ConversionChain.Count: 0 } a && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, f.OwnerSeat, f.OwnerSeat) && cost.CardId == n.CardId && cost.From == n.From && cost.CardKind == n.Kind && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzNativeIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == use.Id && e.ActionId == a.ActionId && e.CardId == id) == 1;
    internal bool IsShiHzOriginalActionCost(CardActionContext action, CardActionCost cost)
    {
        if (!HasShiHz || action is not { Type: CardActionType.Use, PhysicalCards: [var only], ConversionChain.Count: 0 } || only != cost || action.ActorSeat != action.ProviderSeat || action.RequesterSeat is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzNativeIssuedEvent>().SingleOrDefault(e => e.ActionId == action.ActionId && e.CardId == cost.CardId)is not { } issued || ShiHzGrant(issued.GrantId)is not { } g || g.Source.OwnerSeat != action.ActorSeat || cost.From != g.Pile.Location || action.EffectiveKind != cost.CardKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(cost.CardKind))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzGrantConsumedEvent>().Count(e => e.GrantId == g.GrantId && e.FrameId == issued.FrameId) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == cost.CardId && m.From == cost.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use && m.Sequence > g.DiscardSequence && m.TurnNumber == g.Phase.TurnNumber) == 1;
    }

    [NativeUseValidation(47900)]
    internal void ValidateShiHzNativeUse(CardUseFrame use)
    {
        if (use.ShiHuangZuUse is { } n && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.ParentId)is not { } f || !ShiHzUseRidesOn(use, f)))
            throw new InvalidOperationException("Duanyang cannot retire a different or incomplete original Slash Use.");
    }

    [NativeUseReturn(47900)]
    internal bool ReturnShiHzNativeUse(CardUseFrame use)
    {
        if (use.ShiHuangZuUse is not { } n)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.ParentId);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || !ShiHzUseRidesOn(use, f) || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzNativeReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Duanyang requires one exact whole native use return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiHzNativeReturnedEvent(n.GrantId, use.Id));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShiHuangZu = f.ShiHuangZu!with { Stage = ShiHzStage.ReturnedUse } });
        if (use.CardAttack is not { Active: true })
            ContinueShiHz(f.Id);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 47900)]
    internal bool ResumeShiHzAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiHuangZu: { Stage: ShiHzStage.ReturnedUse } } f)
            return false;
        ContinueShiHz(f.Id);
        return true;
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MatchBatch, 47900)]
    internal bool ShiHzNativeMaterialBatch(CardUseFrame use, CardMovementBatchContext batch) => use.ShiHuangZuUse is { } n && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.ParentId)is { } f && ShiHzUseRidesOn(use, f) && batch.ParentFrameId == use.Id && batch.Movements.Any(m => m.CardId == n.CardId && m.From == n.From && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 47900)]
    internal bool ShiHzExemptDebit(long useId, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { ShiHuangZuUse: { } n } use && n.Source.OwnerSeat == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.ParentId)is { } f && ShiHzUseRidesOn(use, f);
    internal bool ShiHzResponseColorAllowed(int responder, bool? effectiveIsRed)
    {
        if (effectiveIsRed is not { } red || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()is not { ShiHuangZuColors: { } ban, Action: { } action } use || responder == action.ActorSeat)
            return true;
        if (ban.UseId != use.Id || ban.ActionId != action.ActionId || ban.OwnerSeat != action.ActorSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzColorsBoundEvent>().Count(e => e == ban) != 1)
            throw new InvalidOperationException("Chizhang response colors lost the exact original Use and paid instruction.");
        return red ? !ban.Red : !ban.Black;
    }

    internal bool? ShiHzResponseColor(CharacterState owner, IReadOnlyList<int> cards, CardConversionSource? source)
    {
        if (cards.Count == 0)
            return null;
        var material = cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        if (material.Length == 1)
        {
            var appeared = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, material[0], source);
            return global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, appeared));
        }

        return _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, material);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.NativeResponseBan, 47900)]
    internal bool ShiHzNativeResponseBan(ProgramNativeResponseQuery q) => !q.DyingUse && !ShiHzResponseColorAllowed(q.Provider.Seat, ShiHzResponseColor(q.Provider, q.CardIds, q.Source));
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 47900)]
    internal bool ShiHzViewAsResponseBan(ProgramViewAsLegalityQuery q) => q.ForResponse && !q.DyingUse && !ShiHzResponseColorAllowed(q.Owner.Seat, ShiHzResponseColor(q.Owner, q.CardIds, q.Source));
    [ProgramCardLegality(ProgramCardLegalityStage.SubmittedUseValidation, 47900)]
    internal void ShiHzSubmittedResponse(ProgramSubmittedUseQuery q)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind is not (DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.Nullification))
            return;
        if (!ShiHzResponseColorAllowed(q.ActorSeat, ShiHzResponseColor(_coreMatchParticipantsPlayersAccess._players[q.ActorSeat], q.CardIds, q.ConversionChain.LastOrDefault())))
            throw new InvalidOperationException("This response color is prohibited by the exact incoming Chizhang Use.");
    }
}
