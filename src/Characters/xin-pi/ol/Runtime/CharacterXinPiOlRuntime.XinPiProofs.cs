#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 30)]
    internal bool XinPiFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => HasXinPiCapability;
    internal bool HasXinPiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.InvalidateRemainingActualDamageCardEffects) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ClaimCompletedUndamagedIncomingCardEntities) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PreventYinjuDamageAndRecover) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawBothAfterYinjuActualTargetUse) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ArmYinjuActualTurnRelation);

    internal static CardConversionSource XinPiSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_xin_pi_ol_XinPiProofs.XinPiSource(f);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 540)]
    internal void ObserveXinPiEvent(IGameEvent payload)
    {
        if (!HasXinPiCapability)
            return;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { Type: CardActionType.Use } a } u)
        {
            if (u.XinPiOrigin is not null)
                throw new InvalidOperationException("XinPi use origin cannot issue twice.");
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(u.Id, f => f with { XinPiOrigin = new(u.Id, a, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
        }

        if (payload is CardActionAcceptedEvent { Action: { Type: CardActionType.Use } accepted } && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == accepted.ActionId)is { XinPiOrigin: { } origin } native)
        {
            var invoices = origin.Materials.ToList();
            foreach (var cost in accepted.PhysicalCards.Where(c => c.From == CardLocation.Processing))
                if (!invoices.Any(m => m.CardId == cost.CardId) && XinPiExactTunanPayment(native, cost.CardId)is { } payment)
                    invoices.Add(new(cost.CardId, payment, null));
            if (invoices.Count != origin.Materials.Count)
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(native.Id, f => f with { XinPiOrigin = origin with { Materials = invoices } });
        }

        if (payload is TurnEndedEvent)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            {
                p.YinjuRelation = null;
                p.ChijieUsedActualTurn = null;
            }
        }
    }

    internal static IReadOnlyList<int> XinPiMainTargets(CardUseFrame u) => global::CardGame.Core.RuntimeSkills.RuntimeRules_xin_pi_ol_XinPiProofs.XinPiMainTargets(u);
    internal CardUseFrame? ExactXinPiNativeWindow(ProgramSkillWindowContext w, bool completed)
    {
        if (w.CardUse is not { } identity || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.CompletedResponseReturn is not null || window.ParentFrameId != identity.ParentCardUseFrameId || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != (completed ? SkillProgramTriggerWindow.CardUseCompleted : SkillProgramTriggerWindow.CardUseTargetsFinalized) || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use } a } u || a.ActionId != identity.CardActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(a, window.Action) || !_coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(u, a))
            return null;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != u.Id)
            return null;
        if (completed && (u.Step != ResolutionFrameStep.Completed || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == u.Id && e.CardId == u.CardId && e.CardKind == u.CardKind) != 1))
            return null;
        return u;
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 40)]
    internal void CaptureXinPiUseMovementBatch(CardMovementBatchContext batch)
    {
        if (!HasXinPiCapability || batch.ParentBatchId is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { XinPiOrigin: { } origin, Action: { } action } use || batch.ParentFrameId != use.Id || origin.UseId != use.Id || origin.Action.ActionId != action.ActionId)
            return;
        var materials = origin.Materials.ToList();
        foreach (var m in batch.Movements)
        {
            if (!action.PhysicalCards.Any(c => c.CardId == m.CardId && c.CardKind == m.CardKind) || m.Sequence <= origin.Before || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
                continue;
            var cost = action.PhysicalCards.Single(c => c.CardId == m.CardId);
            var existing = materials.FindIndex(c => c.CardId == m.CardId);
            if (m.From == cost.From && m.To == CardLocation.Processing && existing < 0)
                materials.Add(new(m.CardId, m, batch.Id));
            else if (existing < 0 && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && XinPiExactTunanPayment(use, m.CardId)is { } tunan)
                materials.Add(new(m.CardId, tunan, null, m, batch.Id));
            else if (existing >= 0 && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && materials[existing].Finished is null && m.Sequence > materials[existing].Payment.Sequence)
                materials[existing] = materials[existing] with
                {
                    Finished = m,
                    FinishedBatchId = batch.Id
                };
        }

        if (materials.Count != origin.Materials.Count || !materials.SequenceEqual(origin.Materials))
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { XinPiOrigin = origin with { Materials = materials } });
    }

    internal CardMovementRecord? XinPiExactTunanPayment(CardUseFrame use, int id)
    {
        if (use.XinPiOrigin is not { } origin || use.Action is not { } a || use.CardAttack is null || !_coreCompletedUndamagedTargetRevealOperations.HasExactCompletedUndamagedTunanVirtualAttack(use, a, _kernelCoreMainOperations.CreateCardAttackHandle(use.Id)))
            return null;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent)
            return null;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id && m.Sequence <= origin.Before && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == $"skill-program.{(parent.SkillId)}.{(global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(parent))}.reveal");
    }

    internal bool XinPiMaterialStillOriginal(CardUseFrame use, XinPiMaterialInvoice invoice)
    {
        if (use.XinPiOrigin is not { } origin || use.Action is not { } a || origin.UseId != use.Id || origin.Action.ActionId != a.ActionId || invoice.PaymentBatchId is not > 0 && XinPiExactTunanPayment(use, invoice.CardId) != invoice.Payment || !a.PhysicalCards.Any(c => c.CardId == invoice.CardId && c.CardKind == invoice.Payment.CardKind && (c.From == invoice.Payment.From || c.From == CardLocation.Processing && XinPiExactTunanPayment(use, invoice.CardId) == invoice.Payment)) || invoice.Payment.CardId != invoice.CardId || invoice.Payment.To != CardLocation.Processing || invoice.Payment.Sequence <= origin.Before && XinPiExactTunanPayment(use, invoice.CardId) != invoice.Payment || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(invoice.Payment))
            return false;
        var tail = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.CardId == invoice.CardId && m.Sequence > invoice.Payment.Sequence).ToArray();
        return invoice.Finished is { } finished ? invoice.FinishedBatchId is> 0 && finished.CardId == invoice.CardId && finished.From == CardLocation.Processing && finished.To == CardLocation.DiscardPile && tail is [var actual] && actual == finished && _cardZonesOperations._cardZones.GetLocation(invoice.CardId) == CardLocation.DiscardPile : tail.Length == 0 && _cardZonesOperations._cardZones.GetLocation(invoice.CardId) == CardLocation.Processing;
    }

    internal bool IsXinPiUseIneffective(CardUseFrame use)
    {
        if (use.ChijieInvalidation is not { } p)
            return false;
        if (p.UseId != use.Id || use.Action is not { } a || p.ActionId != a.ActionId || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.SkillId == p.Source.SkillId && e.BindingId == p.Source.BindingId && e.SkillInstanceId == p.Source.SkillInstanceId && e.OwnerSeat == p.Source.OwnerSeat && e.Window == SkillProgramTriggerWindow.AfterDamageApplied) != 1 || _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash != p.GameplayHash || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChijieUseInvalidatedEvent>().Count(e => e.Proof == p) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChijieUsedEvent>().Count(e => e.FrameId == p.ProgramFrameId && e.Source == p.Source && e.Branch == "invalidate" && e.UseId == use.Id && e.ActionId == a.ActionId && e.ActualTurn == p.ActualTurn) != 1 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CompletedUndamagedUseDamageRecordedEvent>().Any(e => e.CardUseFrameId == use.Id && e.CardActionId == a.ActionId && e.DamageFrameId == p.DamageFrameId && e.SourceSeat == p.SourceSeat && e.TargetSeat == p.TargetSeat && e.Amount > 0 && !e.SourceLess))
            throw new InvalidOperationException("Chijie invalidation lost its exact already-applied native card damage proof.");
        return true;
    }

    internal bool IsXinPiRemainingCardDamageIneffective(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack is global::CardGame.Core.NativeOwners.CardAttackHandle c && !c.IsProgramSkillDamage && !c.IsProgramJudgmentDamage && !c.IsDelayedJudgmentDamage && _coreCardUseLifecycleOperations.LifecycleCardUse(c.ResolutionId)is { } use && use.CardKind == c.EffectiveCardKind && _coreCompletedUndamagedTargetRevealOperations.CompletedUndamagedNativeUseActorMatches(use, c) && IsXinPiUseIneffective(use);
    internal bool IsXinPiPhysicalClaim(long useId, int cardId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { XinPiClaim: { } p, Action: { } a, XinPiOrigin: { } origin } || p.UseId != useId || p.ActionId != a.ActionId || !p.Entities.Contains(cardId) || p.After is not { } after || p.BatchId is not { } batch || after <= p.Before || batch <= 0 || _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program?.GameplayHash != p.GameplayHash || !origin.Materials.Any(m => m.CardId == cardId) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == p.FrameId && e.OwnerSeat == p.Source.OwnerSeat && e.SkillId == p.Source.SkillId && e.BindingId == p.Source.BindingId && e.SkillInstanceId == p.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseCompleted) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChijieClaimIssuedEvent>().Count(e => e.FrameId == p.FrameId && e.UseId == useId && e.ActionId == a.ActionId && e.Source == p.Source && e.GameplayHash == p.GameplayHash && e.Before == p.Before && e.Count == p.Entities.Count) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ChijieClaimPaidEvent>().Count(e => e == new ChijieClaimPaidEvent(p.FrameId, p.Source, useId, a.ActionId, p.Before, after, batch, p.Entities.Count)) != 1)
            return false;
        return p.Entities.All(id => origin.Materials.Any(m => m.CardId == id) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.Sequence > p.Before && m.Sequence <= after && m.From.Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile && m.To == CardLocation.Hand(p.Source.OwnerSeat) && m.Reason.Value == ChijieClaimReason) == 1);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 550)]
    internal bool CanOfferXinPi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        var features = ProgramInstructionResolver.Default.Features(t);
        if (features.HasOperation(SkillProgramEffectOp.InvalidateRemainingActualDamageCardEffects))
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].ChijieUsedActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ChijieDamageUse(c.OwnerSeat, w, out _)is { } use && !IsXinPiUseIneffective(use);
        if (features.HasOperation(SkillProgramEffectOp.ClaimCompletedUndamagedIncomingCardEntities))
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].ChijieUsedActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ChijieUndamagedIncoming(c.OwnerSeat, w, out _);
        if (features.HasOperation(SkillProgramEffectOp.PreventYinjuDamageAndRecover))
            return ExactYinjuBeforeCandidate(c, w, out _, out _);
        if (features.HasOperation(SkillProgramEffectOp.DrawBothAfterYinjuActualTargetUse))
            return ExactYinjuUseCandidate(c, w, out _, out _);
        return true;
    }
}
