#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceXiyingEndingRuntime
{
    internal bool ValidSourceXiyingPaid(ShanheSourceXiyingPaidEvent paid)
    {
        if (!_coreShanheSourceXiyingOperations.SourceXiyingMode || paid.Source.SkillId != ShanheSourceXiyingContract.Skill || paid.Source.BindingId != ShanheSourceXiyingContract.Start || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(paid.Source.OwnerSeat) || paid.ActualTurn < 1 || paid.PhaseInstance < 1 || paid.CardId <= 0 || paid.Before < 0 || paid.After <= paid.Before || paid.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || paid.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(paid.Source.SkillId).Program!.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingPaidEvent>().Count(e => e.FrameId == paid.FrameId) != 1)
            return false;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > paid.Before && m.Sequence <= paid.After && m.Reason.Value == "skill-program.source-xiying-1180.discard").ToArray();
        return moves is [var m] && m.CardId == paid.CardId && m.From == CardLocation.Hand(paid.Source.OwnerSeat) && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame) && CardCatalog.Get(m.CardKind).CategoryName != "基本牌";
    }

    internal bool SourceXiyingTurnHasOwnPlayDamage(ShanheSourceXiyingPaidEvent paid)
    {
        ActualDiscardRecoveryPhaseKey? phase = null;
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (fact is ActualDiscardRecoveryPhaseStartedEvent started)
                phase = started.Phase;
            else if (fact is ActualDiscardRecoveryPhaseRestoredEvent restored)
                phase = restored.Phase;
            else if (fact is ActualDiscardRecoveryPhaseClearedEvent)
                phase = null;
            else if (fact is DamageAppliedEvent { Amount: > 0, SourceLess: false } damage && phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } && phase.TurnNumber == paid.ActualTurn && phase.ActualTurnOwnerSeat == paid.Source.OwnerSeat && phase.ActorSeat == paid.Source.OwnerSeat && damage.SourceSeat == paid.Source.OwnerSeat)
                return true;
        }

        return false;
    }

    internal bool SourceXiyingEarned(ShanheSourceXiyingPaidEvent paid) => ValidSourceXiyingPaid(paid) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingResolvedEvent>().Any(e => e.FrameId == paid.FrameId && e.SkillId == paid.Source.SkillId && e.BindingId == ShanheSourceXiyingContract.Start && e.SkillInstanceId == paid.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.PlayPhaseStarting && e.Completed) && SourceXiyingTurnHasOwnPlayDamage(paid);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.Collection, 62063)]
    internal bool CollectSourceXiying(SkillProgramTrigger t) => t.Id != ShanheSourceXiyingContract.Ending || !t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceXiying);
    [ProgramPipelineHook(ProgramPipelinePhase.AppendTurnEndingItems, 62063)]
    internal void AppendSourceXiyingEnding(List<TurnEndingBoundaryItem> items)
    {
        if (!_coreShanheSourceXiyingOperations.SourceXiyingMode)
            return;
        foreach (var paid in _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingPaidEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Source.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingRewardEvent>().Any(r => r.PaidFrameId == e.FrameId)).ToArray())
        {
            if (!_coreMatchParticipantsPlayersAccess._players[paid.Source.OwnerSeat].IsAlive || !SourceXiyingEarned(paid))
                continue;
            var t = _contentRegistryOperations._contentRegistry.GetSkill(paid.Source.SkillId).Program!.Triggers.Single(t => t.Id == ShanheSourceXiyingContract.Ending);
            var candidate = new ProgramTriggerCandidate(paid.Source.OwnerSeat, paid.Source.SkillId, t.Id, paid.Source.SkillInstanceId, paid.GameplayHash, t.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, t.Priority, "source-xiying-ending:" + paid.FrameId, candidate, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])));
        }
    }

    internal ShanheSourceXiyingPaidEvent? ExactSourceXiyingEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c)
    {
        if (!_coreShanheSourceXiyingOperations.SourceXiyingMode || candidate.SkillId != ShanheSourceXiyingContract.Skill || candidate.BindingId != ShanheSourceXiyingContract.Ending || c.Window != SkillProgramTriggerWindow.TurnEnding || c.OwnerSeat != candidate.OwnerSeat || c.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || c.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || candidate.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is not { } parent || parent.OwnerSeat != candidate.OwnerSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate != candidate)
            return null;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingPaidEvent>().SingleOrDefault(e => parent.Items[parent.ItemIndex].StableIdentity == "source-xiying-ending:" + e.FrameId);
        return paid is not null && paid.Source.OwnerSeat == candidate.OwnerSeat && paid.Source.SkillInstanceId == candidate.SkillInstanceId && paid.GameplayHash == candidate.GameplayHash && paid.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && SourceXiyingEarned(paid) ? paid : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 62063)]
    internal bool IsIssuedSourceXiyingEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => ExactSourceXiyingEnding(candidate, c)is { } paid && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingRewardEvent>().Any(e => e.PaidFrameId == paid.FrameId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62063, SkillProgramEffectOp.ResolveShanheSourceXiying)]
    internal bool CanOfferSourceXiying(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId != ShanheSourceXiyingContract.Skill)
            return false;
        if (gate.Candidate.BindingId == ShanheSourceXiyingContract.Ending)
            return IsIssuedSourceXiyingEnding(gate.Candidate, gate.Context);
        return gate.Owner.IsAlive && _coreShanheSourceXiyingOperations.SourceXiyingPlayParent(gate.OwnerSeat, gate.Candidate, gate.Context) && _corePhysicalOwnedCardQueriesOperations.GetHand(gate.Owner).Any(card => CardCatalog.Get(card.Kind).CategoryName != "基本牌" && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(gate.OwnerSeat, card, CardLocation.Hand(gate.OwnerSeat), OwnedCardMoveIntent.Discard));
    }

    internal SkillProgramStepOutcome BeginSourceXiyingReward(ProgramSkillFrame f)
    {
        var c = f.WindowContext!;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        var paid = ExactSourceXiyingEnding(candidate, c) ?? throw new InvalidOperationException("Source1180 reward lost its exact paid Play and actual Ending obligation.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingRewardEvent>().Any(e => e.PaidFrameId == paid.FrameId))
            throw new InvalidOperationException("Source1180 cannot pay the same earned reward twice.");
        var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) || global::CardGame.Core.EngineOwners.CoreGameEngineShanheSchoolTrickRuntime.IsShanheSchoolDamageTrick(card.Kind)).ToArray();
        var selected = pool.Length == 0 ? null : pool[_randomOperations._random.Next(pool.Length)];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (selected is null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceXiyingRewardEvent(f.Id, paid.FrameId, f.OwnerSeat, null, before, before));
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceXiying = new(c.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, paid.PhaseInstance, ShanheSourceXiyingStage.RewardChildren, TargetBefore: before, TargetAfter: before, PaidFrameId: paid.FrameId, RewardCardId: selected.Id) });
        _corePhysicalMovementFacadesOperations.MoveCard(selected, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new("skill-program.source-xiying-1180.reward"), move =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShanheSourceXiying = active.ShanheSourceXiying!with { TargetAfter = move.Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceXiyingRewardEvent(f.Id, paid.FrameId, f.OwnerSeat, selected.Id, before, move.Sequence));
        });
        _coreShanheSourceXiyingOperations.ContinueSourceXiying(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62063)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 62063)]
    internal bool KeepPaidSourceXiying(ProgramSkillFrame f) => f.SkillId == ShanheSourceXiyingContract.Skill && (f.ShanheSourceXiying is { CostCardId: > 0 } && _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingPaidEvent>().Any(e => e.FrameId == f.Id && ValidSourceXiyingPaid(e)) || f.TriggerId == ShanheSourceXiyingContract.Ending && f.WindowContext is { } c && ExactSourceXiyingEnding(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c)is not null);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 62063)]
    internal void AssertSourceXiyingFrame(ProgramSkillFrame f)
    {
        if (f.ShanheSourceXiying is not { } r)
            return;
        if (f.SkillId != ShanheSourceXiyingContract.Skill || f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { } c || r.ParentId != c.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || r.PlayPhaseInstance < 1 || r.Seats.Distinct().Count() != r.Seats.Count || r.Seats.Contains(f.OwnerSeat) || r.Index < 0 || r.Index > r.Seats.Count)
            throw new InvalidOperationException("Source1180 lost its immutable owner, native frame or actual-Play cursor.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (r.Stage == ShanheSourceXiyingStage.RewardChildren)
        {
            var paid = ExactSourceXiyingEnding(candidate, c);
            if (paid is null || r.PaidFrameId != paid.FrameId || r.RewardCardId is null || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingRewardEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var reward] || reward.PaidFrameId != paid.FrameId || reward.CardId != r.RewardCardId || reward.Before != r.TargetBefore || reward.After != r.TargetAfter || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == reward.After && m.CardId == reward.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.source-xiying-1180.reward"))
                throw new InvalidOperationException("Source1180 reward lost its one original deck entity or original paid Ending invoice.");
            return;
        }

        if (!_coreShanheSourceXiyingOperations.SourceXiyingPlayParent(f.OwnerSeat, candidate, c) || r.PlayPhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || r.Stage != ShanheSourceXiyingStage.Cost && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingPaidEvent>().Any(e => e.FrameId == f.Id && e.Source == global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceXiyingRuntime.SourceXiyingSource(f) && e.CardId == r.CostCardId && e.Before == r.CostBefore && e.After == r.CostAfter && ValidSourceXiyingPaid(e)))
            throw new InvalidOperationException("Source1180 lost its exact once-paid non-basic hand cost or Play parent.");
        if (r.Stage == ShanheSourceXiyingStage.TargetChildren && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceXiyingTargetPaidEvent>().Any(e => e.FrameId == f.Id && e.TargetSeat == r.TargetSeat && e.CardId == r.TargetCardId && e.Before == r.TargetBefore && e.After == r.TargetAfter))
            throw new InvalidOperationException("Source1180 lost an actual other-character discard invoice.");
    }
}
