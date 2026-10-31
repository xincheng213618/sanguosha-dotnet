#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYunOlRuntime
{
    internal static bool WangYunOriginsEqual(WangYunNativeUseOrigin a, WangYunNativeUseOrigin b) => global::CardGame.Core.RuntimeSkills.RuntimeRules_wang_yun_ol_WangYunObservers.WangYunOriginsEqual(a, b);
    internal void BindWangYunNativeUse(long useId)
    {
        if (!HasWangYunPlanCapability && !HasWangYunTrickCapability)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == useId);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index] is not CardUseFrame { Action: { } action } use || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame f)
            return;
        string kind;
        int actor;
        IReadOnlyList<int> expected;
        CardKind output;
        if (f.WangYunPlan is { Stage: "issuing-weapon" or "issuing-slash" } plan)
        {
            kind = plan.Stage == "issuing-weapon" ? "weapon" : "slash";
            actor = plan.Actor;
            expected = plan.IssuingIds;
            output = plan.IssuingKind!.Value;
        }
        else if (f.WangYunTrick is { Stage: "issuing-trick" } trick)
        {
            kind = "trick";
            actor = f.OwnerSeat;
            expected = [trick.CardId];
            output = trick.SelectedKind!.Value;
        }
        else
            return;
        var roundPayment = f.WangYunPlan?.RoundOncePayment;
        if (expected.Count == 0 && (kind != "slash" || roundPayment is null || !ValidWangYunRoundOnceHeader(roundPayment, action, actor)))
            throw new InvalidOperationException("Lianji zero native issuance lacks its strict independently paid source header.");
        if (use.WangYunNativeOrigin is not null || action.Type != CardActionType.Use || action.ActorSeat != actor || action.ProviderSeat != actor || action.RequesterSeat is not null || action.EffectiveKind != output || !action.PhysicalCards.Select(c => c.CardId).SequenceEqual(expected) || use.PhysicalCardIds is null || !use.PhysicalCardIds.SequenceEqual(expected) || action.PhysicalCards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != c.From || (kind == "weapon" ? c.From != CardLocation.DrawPile : c.From.OwnerSeat != actor)))
            throw new InvalidOperationException("Wang Yun native issuance lost its actual actor, provider or accepted original materials.");
        var origin = new WangYunNativeUseOrigin
        {
            ProgramFrameId = f.Id,
            InstructionIndex = f.InstructionIndex,
            Source = WangYunSource(f),
            GameplayHash = f.GameplayHash,
            Turn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            PhaseInstance = _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId,
            Kind = kind,
            UseId = use.Id,
            ActionId = action.ActionId,
            Actor = actor,
            OutputKind = output,
            Materials = action.PhysicalCards,
            Targets = action.TargetSeats,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            RoundOncePayment = roundPayment
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f.WangYunPlan is { } p ? f with { WangYunPlan = p with { Stage = kind + "-issued", NativeUse = origin } } : f with { WangYunTrick = f.WangYunTrick!with { Stage = "trick-issued", NativeUse = origin } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, current => current with { WangYunNativeOrigin = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangYunNativeUseIssuedEvent(origin));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashPaymentCompleted, 10)]
    internal void CompleteWangYunNativePayment(long useId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.WangYunNativeOrigin is not { } old || old.PaymentCompleted)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == old.ProgramFrameId);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (old.Materials.Count == 0 && (old.RoundOncePayment is null || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { } zero || zero.Action is not { } action || zero.RoundOnceCardlessBasicUse is not { } receipt || receipt.Payment != old.RoundOncePayment || !_coreRoundOnceCardlessBasicOperations.IsIssuedRoundOnceCardlessBasicUse(receipt, zero, action) || !ValidWangYunRoundOnceHeader(old.RoundOncePayment, action, old.Actor)))
            throw new InvalidOperationException("Lianji zero native payment requires the actual independent Use receipt, never an empty material predicate.");
        if (old.Materials.Any(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => IsWangYunNativeMaterialPayment(old, c, m, after)) != 1))
            throw new InvalidOperationException("Wang Yun native use did not pay every accepted physical source entity exactly once.");
        var paid = old with
        {
            After = after,
            PaymentCompleted = true
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f.WangYunPlan is { } p ? f with { WangYunPlan = p with { NativeUse = paid } } : f with { WangYunTrick = f.WangYunTrick!with { NativeUse = paid } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, use => use with { WangYunNativeOrigin = paid });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangYunNativePaymentCompletedEvent(paid));
    }

    internal bool ValidWangYunNativeUse(CardUseFrame use)
    {
        if (use.WangYunNativeOrigin is not { } r || use.Id != r.UseId || use.CardKind != r.OutputKind || use.PhysicalCardIds is null || !use.PhysicalCardIds.SequenceEqual(r.Materials.Select(c => c.CardId)) || use.Action is not { } action || action.ActionId != r.ActionId || action.Type != CardActionType.Use || action.ProviderSeat != r.Actor || action.RequesterSeat is not null || !action.PhysicalCards.SequenceEqual(r.Materials) || action.EffectiveKind != r.OutputKind || r.Before > r.After || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == r.ProgramFrameId)is not { } parent || r.Source != WangYunSource(parent) || r.GameplayHash != parent.GameplayHash || r.InstructionIndex != parent.InstructionIndex || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            return false;
        var receipt = parent.WangYunPlan?.NativeUse ?? parent.WangYunTrick?.NativeUse;
        if (receipt is null || !WangYunOriginsEqual(r, receipt))
            return false;
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangYunNativeUseIssuedEvent>().Where(e => e.Origin.UseId == r.UseId).ToArray();
        if (issued is not [var e] || !WangYunOriginsEqual(e.Origin, r with { After = r.Before, PaymentCompleted = false }))
            return false;
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangYunNativePaymentCompletedEvent>().Where(e => e.Origin.UseId == r.UseId).ToArray();
        if (r.Materials.Count == 0 && (r.RoundOncePayment is null || parent.WangYunPlan?.RoundOncePayment != r.RoundOncePayment || !ValidWangYunRoundOnceStoredUse(use, r)))
            return false;
        if (!r.PaymentCompleted)
            return paid.Length == 0;
        if (r.Materials.Count == 0 && (use.RoundOnceCardlessBasicUse is not { } zeroReceipt || zeroReceipt.Payment != r.RoundOncePayment || !ValidWangYunRoundOnceStoredUse(use, r)))
            return false;
        return paid is [var invoice] && WangYunOriginsEqual(invoice.Origin, r) && _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, r.Actor) && r.Materials.All(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => IsWangYunNativeMaterialPayment(r, c, m, r.After)) == 1);
    }

    internal bool IsWangYunNativeMaterialPayment(WangYunNativeUseOrigin origin, CardActionCost material, CardMovementRecord movement, long after)
    {
        if (movement.Sequence <= origin.Before || movement.Sequence > after || movement.CardId != material.CardId || movement.From != material.From)
            return false;
        if (movement.To == CardLocation.Processing)
            return true;
        // A purchased equipment entity is destroyed by its normal Use movement.
        // Credit only this accepted source and owning native Use, never an
        // arbitrary outside-game destination or an earlier destruction.
        return material.From == CardLocation.Equipment(origin.Actor) && movement.CardKind == material.CardKind && movement.Reason == CardMoveReasons.Use && _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentCreatedEvent>().Where(e => e.CardId == material.CardId).ToArray()is [var created] && created.Seat == origin.Actor && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Where(e => e.CardId == material.CardId).ToArray()is [var destroyed] && destroyed.MovementSequence == movement.Sequence && destroyed.ParentFrameId == origin.UseId;
    }

    internal bool ValidWangYunRoundOnceStoredUse(CardUseFrame use, WangYunNativeUseOrigin origin)
    {
        if (origin.RoundOncePayment is not { } payment || use.Action is not { } current)
            return false;
        var original = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).FirstOrDefault(a => a.ActionId == origin.ActionId && a.Type == CardActionType.Use && a.ActorSeat == origin.Actor && a.ProviderSeat == origin.Actor && a.EffectiveKind == CardKind.Slash && a.PhysicalCards.Count == 0 && a.ConversionChain is [var source] && source == payment.Source);
        if (original is null)
            return ValidWangYunRoundOnceHeader(payment, current, origin.Actor) && (!origin.PaymentCompleted || use.RoundOnceCardlessBasicUse is { } initialReceipt && initialReceipt.Payment == payment && _coreRoundOnceCardlessBasicOperations.IsIssuedRoundOnceCardlessBasicUse(initialReceipt, use, current));
        if (!ValidWangYunRoundOnceHeader(payment, original, origin.Actor))
            return false;
        if (origin.PaymentCompleted && (use.RoundOnceCardlessBasicUse is not { } receipt || receipt.Payment != payment || !_coreRoundOnceCardlessBasicOperations.IsIssuedRoundOnceCardlessBasicUse(receipt, use, original)))
            return false;
        return current.ActorSeat == origin.Actor || _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, current.ActorSeat, payment.Source.OwnerSeat);
    }

    internal bool ValidWangYunRoundOnceHeader(RoundOnceCardlessBasicPaymentReceipt payment, CardActionContext action, int actor) => payment.Source.OwnerSeat == actor && payment.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && payment.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && payment.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber && payment.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && action.Type == CardActionType.Use && action.ActorSeat == actor && action.ProviderSeat == actor && action.RequesterSeat is null && action.EffectiveKind == CardKind.Slash && action.PhysicalCards.Count == 0 && action.ConversionChain is [var source] && source == payment.Source && _corePlayCardHistoryOperations.ViewAsRule(source)is { OutputKind: CardKind.Slash, ForPlay: true, ForResponse: false, RoundOnceCardlessBasic: { } policy } && policy.UsageGroup == payment.UsageGroup && _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.GameplayHash == payment.GameplayHash && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundOnceCardlessBasicQuotaPaidEvent>().Count(e => e.Receipt == payment) == 1;
    internal bool WangYunCardUseRidesOn(CardUseFrame use, ProgramSkillFrame parent) => use.WangYunNativeOrigin is { } r && r.ProgramFrameId == parent.Id && ValidWangYunNativeUse(use) && ValidWangYun(parent);
    internal void ResumeWangYunCompletedNativeUse(CardUseFrame use)
    {
        if (use.WangYunNativeOrigin!.Kind != "slash")
            _coreRuntimeOperations.AdvanceRuntimeProgram(use.WangYunNativeOrigin.ProgramFrameId);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.BeforeParentResume, 10)]
    internal void ValidateWangYunNativeAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame frame && completion.ProgramFrameId is { } frameId && frame.WangYunPlan is { NativeUse: { Kind: "slash" } origin } plan && (!plan.NativeReturned || origin.ProgramFrameId != frameId || origin.UseId != completion.ResolutionId || !CanContinueWangYun(frame)))
            throw new InvalidOperationException("A Wang Yun Slash completion lost its exact once-returned native use.");
    }

    [NativeUsePreparation(20, 80, "ResumeWangYunCompletedNativeUse")]
    internal bool ReturnWangYunNativeUse(CardUseFrame use)
    {
        if (use.WangYunNativeOrigin is not { } r)
            return false;
        CompleteWangYunNativePayment(use.Id);
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        r = use.WangYunNativeOrigin!;
        if (!ValidWangYunNativeUse(use) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangYunNativeUseReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Wang Yun native child returned a changed or already consumed invoice.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == r.ProgramFrameId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent.WangYunPlan is { } p ? parent with { WangYunPlan = p with { NativeReturned = true } } : parent with { WangYunTrick = parent.WangYunTrick!with { NativeReturned = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangYunNativeUseReturnedEvent(parent.Id, r.UseId, r.ActionId, r.Kind));
        return true;
    }

    internal bool ValidWangYun(ProgramSkillFrame f)
    {
        if (f.WangYunPlan is { } p)
        {
            if (!WangYunActivationMatches(f, SkillProgramEffectOp.ResolveLianjiPaidWeaponAndSlash) || p.Source != WangYunSource(f) || p.GameplayHash != f.GameplayHash || p.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || p.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || p.InstructionIndex != f.InstructionIndex || f.SelectedCardIds is not [var cost] || cost != p.CostCardId || f.SelectedTargetSeats is not [var actor] || actor != p.Actor || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) || actor == f.OwnerSeat)
                return false;
            var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            if (events.OfType<WangYunPlanStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new WangYunPlanStartedEvent(f.Id, p.Source, p.GameplayHash, p.Turn, p.PhaseInstance, actor, cost))
                return false;
            var moves = events.OfType<WangYunPlanMovementPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (moves.Count(e => e.Next == "weapon") != (p.CostPaid ? 1 : 0) || moves.Length > 2 || moves.Any(e => e.Before > e.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > e.Before && m.Sequence <= e.After && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == WangYunPlanMoveReason) != 1) || p.CostPaid && !moves.Any(e => e.Next == "weapon" && e.CardId == cost && e.From == CardLocation.Hand(f.OwnerSeat) && e.To is var to && (to == CardLocation.DiscardPile || to == CardLocation.OutsideGame)))
                return false;
            if (p.Stage == "movement" && moves.Count(e => e.Before == p.Before && e.After == p.After && e.Next == p.ReturnStage) != 1)
                return false;
            if (p.Stage is not ("cost" or "movement" or "weapon" or "issuing-weapon" or "weapon-issued" or "target" or "branch" or "transfer" or "complete" or "issuing-slash" or "slash-issued"))
                return false;
            if (p.SlashTarget is { } target && (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == p.Actor))
                return false;
            return ValidWangYunStoredOrigin(f, p.NativeUse, p.NativeReturned, events);
        }

        if (f.WangYunTrick is { } t)
        {
            if (!WangYunActivationMatches(f, SkillProgramEffectOp.UseThreeRandomTricksFromSelectedMaterial) || t.Source != WangYunSource(f) || t.GameplayHash != f.GameplayHash || t.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || t.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || t.InstructionIndex != f.InstructionIndex || !f.SelectedCardIds.SequenceEqual([t.CardId]) || f.SelectedTargetSeats.Count != 0 || t.From.OwnerSeat != f.OwnerSeat || t.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || t.Samples.Count != 3 || t.Samples.Distinct().Count() != 3 || t.Samples.Any(k => !WangYunRandomTrickPool().Contains(k)) || t.Stage is not ("choice" or "issuing-trick" or "trick-issued"))
                return false;
            var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            return events.OfType<WangYunRandomTricksStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var started] && started == new WangYunRandomTricksStartedEvent(f.Id, t.Source, t.GameplayHash, t.Turn, t.PhaseInstance, t.CardId, t.PrintedKind, t.From, WangYunSamplesFingerprint(t.Samples)) && (t.SelectedKind is null || t.Samples.Contains(t.SelectedKind.Value)) && ValidWangYunStoredOrigin(f, t.NativeUse, t.NativeReturned, events);
        }

        if (f.WangYunAwakening is { } a)
            return a.Source == WangYunSource(f) && a.GameplayHash == f.GameplayHash && a.ParentFrameId == f.WindowContext?.ParentFrameId && a.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && a.InstructionIndex == f.InstructionIndex && f.WindowContext?.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangYunAwakenedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.LostSkillId == "ol:lianji" && e.AcquiredSkillId == "ol:jingong") == 1;
        return false;
    }

    internal bool ValidWangYunStoredOrigin(ProgramSkillFrame f, WangYunNativeUseOrigin? origin, bool returned, IReadOnlyList<IGameEvent> events)
    {
        if (origin is null)
            return !returned;
        if (origin.ProgramFrameId != f.Id || origin.Source != WangYunSource(f) || origin.GameplayHash != f.GameplayHash || origin.InstructionIndex != f.InstructionIndex || origin.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || origin.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId)
            return false;
        var issued = events.OfType<WangYunNativeUseIssuedEvent>().Where(e => e.Origin.UseId == origin.UseId).ToArray();
        if (issued is not [var first] || !WangYunOriginsEqual(first.Origin, origin with { After = origin.Before, PaymentCompleted = false }))
            return false;
        var payments = events.OfType<WangYunNativePaymentCompletedEvent>().Where(e => e.Origin.UseId == origin.UseId).ToArray();
        if (origin.PaymentCompleted ? payments is not [var paid] || !WangYunOriginsEqual(paid.Origin, origin) : payments.Length != 0)
            return false;
        return events.OfType<WangYunNativeUseReturnedEvent>().Count(e => e.ProgramFrameId == f.Id && e.UseId == origin.UseId && e.ActionId == origin.ActionId && e.Kind == origin.Kind) == (returned ? 1 : 0);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 150)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 150)]
    internal bool CanContinueWangYun(ProgramSkillFrame f) => ValidWangYun(f) && (f.WangYunPlan is { CostPaid: true } || f.WangYunTrick?.NativeUse is not null || f.WangYunAwakening is not null);
    internal bool WangYunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidWangYun(f))
            return false;
        if (child is CardUseFrame use)
            return WangYunCardUseRidesOn(use, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.WangYunPlan is { Stage: "movement" } p)
            return moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > p.Before && m.Sequence <= p.After);
        if (child is ProgramLifecycleTriggerWindowFrame skills)
            return skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.Program;
        return false;
    }

    internal ProgramSkillFrame? WangYunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _characterWangYunOlOperations.WangYunFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1540)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1650)]
    internal bool HasWangYunDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && WangYunObserverRoot()is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1650)]
    internal bool HasWangYunBeforeDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && WangYunObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 190)]
    internal bool TryAdvanceWangYunSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WangYunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
