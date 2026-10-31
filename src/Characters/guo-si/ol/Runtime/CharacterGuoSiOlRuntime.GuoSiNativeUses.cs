#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    internal void IssueSidao(ProgramSkillFrame f, LegalAction action)
    {
        var r = f.GuoSi!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Single(c => c.Id == action.CardId);
        var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, card);
        var option = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, CardKind.Snatch, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(suit), hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: false).Single(o => o.ActionKind == action.Kind && o.TargetCardId == action.TargetCardId && o.TargetSeats.SequenceEqual(action.TargetSeats));
        var id = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, action.TargetSeats, CardKind.Snatch, physicalCardIds: [card.Id], conversionSource: r.Source);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var native = new GuoSiNativeReceipt(f.Id, r.Source, r.GameplayHash, r.Turn, r.Phase, r.Instruction, id, use.Action!.ActionId, use.Action.ParentActionId ?? throw new InvalidOperationException("Sidao lost its original action parent."), card.Id, action.TargetSeats.Single(), action.Kind, action.TargetCardId, option.RequiredCardKind);
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
        if (parent.GuoSi != r || parent.GameplayHash != f.GameplayHash || parent.SkillInstanceId != f.SkillInstanceId || _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == parent.Id) + 1 != _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == id))
            throw new InvalidOperationException("Sidao native child lost its direct original parent.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { GuoSi = r with { Stage = GuoSiStage.NativeUse, Native = native } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { GuoSiNative = native, GuoSiCostBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, GuoSiCostAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, GuoSiCostDrained = false });
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, CardMoveReasons.Use);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { GuoSiCostAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuoSiNativeIssuedEvent(native));
        ContinueGuoSiNativePayment(id);
    }

    internal bool IsGuoSiNativeUse(CardUseFrame use) => use.GuoSiNative is not null;
    internal bool GuoSiNativeRidesOn(ProgramSkillFrame f, CardUseFrame use) => f.GuoSi is { Stage: GuoSiStage.NativeUse, Native: { } r } && r.ProgramFrameId == f.Id && use.GuoSiNative == r && r.Source == GuoSiSource(f) && r.GameplayHash == f.GameplayHash;
    [NativeUseValidation(60)]
    internal void ValidateGuoSiNativeUseReturn(CardUseFrame use)
    {
        if (IsGuoSiNativeUse(use))
            AssertGuoSiNativeUse(use);
    }

    internal void AssertGuoSiNativeUse(CardUseFrame use)
    {
        if (use.GuoSiNative is not { } r || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == r.ProgramFrameId)is not { } parent || !GuoSiNativeRidesOn(parent, use) || use.Id != r.NativeUseId || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != r.Source.OwnerSeat || a.ProviderSeat != a.ActorSeat || a.ActionId != r.ActionId || a.ParentActionId != r.ParentActionId || parent.WindowContext?.CardUse?.CardActionId != r.ParentActionId || a.EffectiveKind != CardKind.Snatch || use.CardKind != CardKind.Snatch || use.CardId != r.Material || !a.PhysicalCards.Select(c => c.CardId).SequenceEqual([r.Material]) || a.PhysicalCards.Single().From != CardLocation.Hand(a.ActorSeat) || !a.ConversionChain.SequenceEqual([r.Source]) || !use.TargetSeats.SequenceEqual([r.Target]) || use.GuoSiCostBefore is not { } before || use.GuoSiCostAfter is not { } after || before > after || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.CardId == r.Material && m.From == CardLocation.Hand(a.ActorSeat) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuoSiNativeIssuedEvent>().Count(e => e.Receipt == r) != 1)
            throw new InvalidOperationException("Sidao native use lost its exact pair, single hand material, source, accepted action or payment invoice.");
    }

    [NativeFrameRoute(typeof(CardUseFrame), 140, typeof(NativeFrameRouteConditions), "GuoSiPayment")]
    internal void ContinueGuoSiNativePayment(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        AssertGuoSiNativeUse(use);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.GuoSiCardUse) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.GuoSiCardUse) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { GuoSiCostDrained = true });
        var r = use.GuoSiNative!;
        var card = _coreCardAttackStateOperations.GetAttackCard(r.Material);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, CardKind.Snatch);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, use.SourceSeat, use.TargetSeats, r.ActionKind, r.TargetCardId, r.RequiredKind, CardKind.Snatch);
    }

    [NativeUseReturn(40)]
    internal bool CompleteGuoSiUseReturn(CardUseFrame use)
    {
        if (!IsGuoSiNativeUse(use))
            return false;
        ReturnGuoSiNativeUse(use);
        return true;
    }

    internal void ReturnGuoSiNativeUse(CardUseFrame use)
    {
        if (use.GuoSiNative is not { } r)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(r.ProgramFrameId);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !GuoSiNativeRidesOn(f, use) || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuoSiNativeReturnedEvent>().Any(e => e.Receipt == r))
            throw new InvalidOperationException("Sidao must return exactly once after its complete native Snatch.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuoSiNativeReturnedEvent(r));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { GuoSi = f.GuoSi!with { Stage = GuoSiStage.Completed } });
    }

    internal bool GuoSiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.GuoSi is not { } r)
            return false;
        if (child is CardUseFrame use)
            return GuoSiNativeRidesOn(root, use) && CanContinueIssuedGuoSi(root);
        if (r.Stage != GuoSiStage.MovementChildren || child is not CardsMovedTriggerWindowFrame moved || !CanContinueIssuedGuoSi(root))
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == r.SelectedId && m.From == r.SelectedFrom && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == TanbeiGainReason);
    }

    internal bool GuoSiObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterGuoSiOlOperations.GuoSiNativePaymentEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i, d) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i, d)));
    internal bool GuoSiNativePaymentEdge(int index)
    {
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not CardUseFrame { GuoSiNative: { } r, GuoSiCostDrained: false } use)
            return false;
        AssertGuoSiNativeUse(use);
        var child = _resolutionStackOperations._resolutionStack[index];
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeGuoSiUseFrameId == use.Id && moved.Batch.ParentFrameId == use.Id && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > use.GuoSiCostBefore && m.Sequence <= use.GuoSiCostAfter && m.CardId == r.Material && m.From == CardLocation.Hand(r.Source.OwnerSeat) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == use.Id && hp.Change.ParentFrameId == use.Id && hp.Continuation == PostEventContinuation.GuoSiCardUse;
        return child is RecoveryReplacementFrame recovery && recovery.ParentFrameId == use.Id && recovery.Return.ResumeFrameId == use.Id && recovery.Return.Continuation == PostEventContinuation.GuoSiCardUse;
    }

    internal ProgramSkillFrame? GuoSiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterGuoSiOlOperations.GuoSiFirstChild(r, c), (i) => _characterGuoSiOlOperations.GuoSiObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 210)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 320)]
    internal bool HasGuoSiDamageObserver(long id) => GuoSiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 410)]
    internal bool HasGuoSiBeforeDamageObserver(long id) => GuoSiObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 320)]
    internal bool TryAdvanceGuoSiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && GuoSiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
