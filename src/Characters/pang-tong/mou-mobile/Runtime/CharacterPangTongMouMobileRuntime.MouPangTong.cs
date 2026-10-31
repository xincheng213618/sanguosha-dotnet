#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongMouMobileRuntime
{
    internal bool HasMouPangTongCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouPangTongLianhuanTargets);

    internal static CardConversionSource MouPtSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_pang_tong_mou_mobile_MouPangTong.MouPtSource(f);
    internal bool MouPtUpgraded(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongUpgradedEvent>().Any(e => e.Source.OwnerSeat == owner);
    internal bool HasMouPangTongExpandedTargets(CharacterState owner) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, MouPangTongComposition.Lianhuan) && MouPtUpgraded(owner.Seat);
    internal CardUseFrame? MouPtUse(ProgramSkillWindowContext c) => c.CardUse is { } card ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == card.CardActionId) : null;
    internal bool MouPtPaidFor(CardUseFrame use, int owner) => use.Action is { Type: CardActionType.Use, EffectiveKind: CardKind.IronChain } a && a.ActorSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongLianhuanPaidEvent>().Any(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.Source.OwnerSeat == owner);
    internal bool MouPtPaidBySource(CardUseFrame use, CardConversionSource source, string gameplayHash) => MouPtPaidFor(use, source.OwnerSeat) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongLianhuanPaidEvent>().Any(e => e.UseId == use.Id && e.ActionId == use.Action!.ActionId && e.GameplayHash == gameplayHash && e.Source == (source with { BindingId = "pay-for-chain" }));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 30500)]
    internal void CaptureMouPtConversion(CardUseDeclaredEvent e)
    {
        if (!HasMouPangTongCapability || _coreCardUseLifecycleOperations.LifecycleCardUse(e.ResolutionId)is not { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.IronChain } a } || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != a.ActorSeat || a.ConversionChain.FirstOrDefault(s => s.SkillId == MouPangTongComposition.Lianhuan && s.BindingId == "club-hand-chain" && s.OwnerSeat == a.ActorSeat)is not { } source || _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongConversionUsedEvent>().Any(f => f.UseId == e.ResolutionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongConversionUsedEvent(source, phase, e.ResolutionId, a.ActionId));
    }

    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 30500)]
    internal void FilterMouPtConversion(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        if (!HasMouPangTongCapability || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase)
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongConversionUsedEvent>().Any(e => e.Source.OwnerSeat == actor.Seat && e.Phase == phase))
            actions.RemoveAll(a => a.Kind == LegalActionKind.IronChain && a.ConversionSource?.SkillId == MouPangTongComposition.Lianhuan);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 30500, SkillProgramEffectOp.ArmMouPangTongLianhuan, SkillProgramEffectOp.ResolveMouPangTongLianhuanTargets)]
    internal bool CanOfferMouPt(ProgramTriggerLegalityContext gate)
    {
        if (MouPtUse(gate.Context)is not { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.IronChain } a } use || a.ActorSeat != gate.OwnerSeat)
            return false;
        if (gate.Features.HasOperation(SkillProgramEffectOp.ArmMouPangTongLianhuan))
            return !MouPtUpgraded(gate.OwnerSeat) && !MouPtPaidFor(use, gate.OwnerSeat);
        var source = new CardConversionSource(gate.SkillId, gate.Trigger.Id, gate.OwnerSeat, gate.Candidate.SkillInstanceId);
        return (MouPtUpgraded(gate.OwnerSeat) || MouPtPaidBySource(use, source, gate.Candidate.GameplayHash)) && (gate.Context.MouPangTongLateTargetSeat is not null ? MouPtLateNotification(gate.Context, use, source, gate.Candidate.GameplayHash) : !_coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongTargetsStartedEvent>().Any(e => e.UseId == use.Id && e.Source.OwnerSeat == gate.OwnerSeat));
    }

    SkillProgramStepOutcome global::CardGame.Core.IMouPangTongHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => ResolveMouPt(f, op);
    internal SkillProgramStepOutcome ResolveMouPt(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (op == SkillProgramEffectOp.UpgradeMouPangTongLianhuan)
        {
            if (f.SkillId != MouPangTongComposition.Niepan || f.TriggerId != "rebirth" || f.InstructionIndex != 6)
                throw new InvalidOperationException("Lianhuan must upgrade after the complete native rebirth sequence.");
            if (!MouPtUpgraded(f.OwnerSeat))
                _coreProgramSkillChangesOperations.CommitProgramSkillChangeFact(f, new MouPangTongUpgradedEvent(f.Id, MouPtSource(f), f.GameplayHash));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.WindowContext is not { } c || MouPtUse(c)is not { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.IronChain } a } use || a.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Lianhuan requires its exact native actor's IronChain Use.");
        if (op == SkillProgramEffectOp.ArmMouPangTongLianhuan)
        {
            if (f.TriggerId != "pay-for-chain" || f.InstructionIndex != 2)
                throw new InvalidOperationException("Lianhuan's paid Use must follow its original HP cost.");
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1) && !MouPtPaidFor(use, f.OwnerSeat))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongLianhuanPaidEvent(f.Id, MouPtSource(f), f.GameplayHash, use.Id, a.ActionId));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.MouPangTongTargets is not null || f.InstructionIndex != 1 || c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || !(MouPtUpgraded(f.OwnerSeat) || MouPtPaidBySource(use, MouPtSource(f), f.GameplayHash)))
            throw new InvalidOperationException("Lianhuan cannot invent an unpaid final-target discard queue.");
        if (c.MouPangTongLateTargetSeat is not null && !MouPtLateNotification(c, use, MouPtSource(f), f.GameplayHash))
            throw new InvalidOperationException("Lianhuan's additional target lost its original native notification and source.");
        var receipt = new MouPangTongTargetsReceipt(MouPtSource(f), f.GameplayHash, c.ParentFrameId, use.Id, a.ActionId)
        {
            Targets = (c.MouPangTongLateTargetSeat is { } lateTarget ? [lateTarget] : use.TargetSeats.Distinct().Where(t => !MouPtVisited(use.Id, f.OwnerSeat, t)).ToArray()),
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouPangTongTargets = receipt, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongTargetsStartedEvent(f.Id, receipt.Source, receipt.GameplayHash, receipt.WindowId, use.Id, a.ActionId, receipt.Targets.Count));
        ContinueMouPtTargets(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ValidMouPtTargets(ProgramSkillFrame f) => f.MouPangTongTargets is { } r && r.Source == MouPtSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && f.SkillId == MouPangTongComposition.Lianhuan && f.TriggerId == "discard-unchained-targets" && f.WindowContext is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized } c && c.ParentFrameId == r.WindowId && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is { Action: { } a } && a.ActionId == r.ActionId && a.ActorSeat == f.OwnerSeat && a.EffectiveKind == CardKind.IronChain && r.Cursor >= 0 && r.Cursor <= r.Targets.Count && r.Targets.Distinct().Count() == r.Targets.Count && r.Targets.All(_coreTableSeatQueriesOperations.IsValidPlayerSeat) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongTargetsStartedEvent>().Count(e => e == new MouPangTongTargetsStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.UseId, r.ActionId, r.Targets.Count)) == 1;
    internal void ContinueMouPtTargets(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouPangTongTargets: { } r } f || f.Id != id)
            return;
        if (!ValidMouPtTargets(f))
            throw new InvalidOperationException("Lianhuan lost its actual target queue.");
        if (DrainMouPt(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouPangTongTargets!;
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            for (var cursor = r.Cursor; cursor < r.Targets.Count; cursor++)
            {
                var target = _coreMatchParticipantsPlayersAccess._players[r.Targets[cursor]];
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target);
                r = r with
                {
                    Cursor = cursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouPangTongTargets = r });
                if (MouPtVisited(r.UseId, f.OwnerSeat, target.Seat))
                    continue;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongTargetVisitedEvent(id, f.OwnerSeat, r.UseId, r.ActionId, target.Seat));
                if (!target.IsAlive || target.IsChained || hand.Count == 0)
                    continue;
                var card = hand[_randomOperations._random.Next(hand.Count)];
                var from = CardLocation.Hand(target.Seat);
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouPangTongTargets = r with { Cursor = cursor + 1, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                var logical = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [card.Id], new("skill-program.mou-mobile:lianhuan.discard"), f.Id);
                _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.DiscardPile, new("skill-program.mou-mobile:lianhuan.discard"), move => _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongTargetDiscardedEvent(id, cursor, target.Seat, card.Id, from, move.To, move.Sequence)));
                _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logical);
                var latest = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                _coreRuntimeOperations.ReplaceRuntimeTop(latest with { MouPangTongTargets = latest.MouPangTongTargets!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
                foreach (var move in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongMovementSignedEvent(id, move.CardId, move.CardKind, move.From, move.To, move.Reason, move.Sequence));
                ContinueMouPtTargets(id);
                return;
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouPangTongTargetsCompletedEvent(id, f.OwnerSeat, r.UseId, r.Targets.Count));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouPangTongTargets = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 11)]
    internal void CollectPaidMouPtTargets(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (!HasMouPangTongCapability || window != SkillProgramTriggerWindow.CardUseTargetsFinalized || action.Type != CardActionType.Use || action.EffectiveKind != CardKind.IronChain || !_coreMatchParticipantsPlayersAccess._players[action.ActorSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { } use)
            return;
        foreach (var paid in _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongLianhuanPaidEvent>().Where(e => e.UseId == use.Id && e.ActionId == action.ActionId && e.Source.OwnerSeat == action.ActorSeat))
        {
            if (result.Any(c => c.OwnerSeat == paid.Source.OwnerSeat && c.SkillId == paid.Source.SkillId && c.SkillInstanceId == paid.Source.SkillInstanceId && c.TriggerId == "discard-unchained-targets"))
                continue;
            var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, action.ActorSeat, action.TargetSeats.Count == 1 ? action.TargetSeats[0] : -1, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], action));
            result.Add(new(action.ActorSeat, action.TargetSeats.Count == 1 ? action.TargetSeats[0] : -1, paid.Source.SkillId, "discard-unchained-targets", paid.GameplayHash, paid.Source.SkillInstanceId, 0, context));
        }
    }

    [ProgramIssuedTrigger(90)]
    internal bool IsIssuedMouPtTargets(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (candidate.SkillId != MouPangTongComposition.Lianhuan || candidate.BindingId != "discard-unchained-targets" || c.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || c.OwnerSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(candidate, c, window) || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { Action: { Type: CardActionType.Use, EffectiveKind: CardKind.IronChain } a } use || a.ActorSeat != candidate.OwnerSeat || c.CardUse?.CardActionId != a.ActionId || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is not { } p || p.GameplayHash != candidate.GameplayHash)
            return false;
        return (c.MouPangTongLateTargetSeat is not null ? MouPtLateNotification(c, use, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash) : !_coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongTargetsStartedEvent>().Any(e => e.UseId == use.Id && e.Source.OwnerSeat == candidate.OwnerSeat)) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouPangTongLianhuanPaidEvent>().Any(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.GameplayHash == candidate.GameplayHash && e.Source == new CardConversionSource(candidate.SkillId, "pay-for-chain", candidate.OwnerSeat, candidate.SkillInstanceId));
    }
}
