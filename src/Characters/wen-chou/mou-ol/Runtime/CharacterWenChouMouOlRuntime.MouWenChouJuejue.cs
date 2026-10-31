#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWenChouMouOlRuntime
{
    internal int MouWenChouTargetCount(int owner, int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouTargetDeclaredEvent>().Count(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActorSeat == owner && e.TargetSeat == target);
    internal void RecordMouWenChouTargets(CardActionContext action)
    {
        if (!_coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn || !_coreProgramEventHistoryOperations.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.CardActionId == action.ActionId && e.ActorSeat == action.ActorSeat))
            return;
        foreach (var target in action.TargetSeats.Distinct())
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouTargetDeclaredEvent>().Any(e => e.ActionId == action.ActionId && e.TargetSeat == target))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouTargetDeclaredEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, action.ActorSeat, action.ActionId, target));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 33101)]
    internal void ObserveMouWenChouTargets(IGameEvent fact)
    {
        if (!HasMouWenChouOl)
            return;
        if (fact is CardActionAcceptedEvent accepted)
            RecordMouWenChouTargets(accepted.Action);
        if (fact is ProgramCardUseTargetAddedEvent added && _coreCardUseLifecycleOperations.LifecycleCardUse(added.CardUseFrameId)?.Action is { } enlarged)
            RecordMouWenChouTargets(enlarged);
        // Every target still present at a later native boundary has also been
        // designated by this Use, including a genuine native added target.
        if (fact is CardUseFinishedEvent finished && _coreCardUseLifecycleOperations.LifecycleCardUse(finished.ResolutionId)?.Action is { } action)
            RecordMouWenChouTargets(action);
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 33102)]
    internal void CaptureMouWenChouAllHandPayment(CardMovementBatchContext batch)
    {
        if (!HasMouWenChouOl || batch.ParentFrameId is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { Type: CardActionType.Use } a } use || a.ActorSeat != a.ProviderSeat || a.PhysicalCards.Count == 0 || a.PhysicalCards.Any(c => c.From != CardLocation.Hand(a.ActorSeat)))
            return;
        var count = batch.SourceCounts.SingleOrDefault(s => s.Location == CardLocation.Hand(a.ActorSeat));
        var ids = a.PhysicalCards.Select(c => c.CardId).Order().ToArray();
        if (count is null || count.CountBefore != ids.Length || !batch.Movements.Where(m => m.From == CardLocation.Hand(a.ActorSeat) && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use).Select(m => m.CardId).Order().SequenceEqual(ids))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { MouWenChouAllHand = true });
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouAllHandPaidEvent>().Any(e => e.UseId == use.Id && e.ActionId == a.ActionId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouAllHandPaidEvent(use.Id, a.ActionId, a.ActorSeat, ids.Length));
    }

    internal bool MouWenChouAllHandUse(CardUseFrame use) => use.MouWenChouAllHand == true && use.Action is { Type: CardActionType.Use, PhysicalCards.Count: > 0 } a && a.ProviderSeat == a.ActorSeat && a.PhysicalCards.All(c => c.From == CardLocation.Hand(a.ActorSeat)) && a.PhysicalCards.All(c => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.CardId && m.From == c.From && m.Reason == CardMoveReasons.Use && m.To == CardLocation.Processing));
    internal CardUseFrame? MouWenChouContextUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || _coreCardUseLifecycleOperations.LifecycleCardUse(info.ParentCardUseFrameId)is not { Action: { } a } use || a.ActionId != info.CardActionId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame parent || parent.ParentFrameId != use.Id || parent.Action.ActionId != a.ActionId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        return use;
    }

    [ProgramIssuedTrigger(610)]
    internal bool CanRunIssuedMouWenChou(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (MouWenChouContextUse(c, w)is not { Action: { } a } use || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive)
            return false;
        if (c.SkillId == MouWenChouOlComposition.Lunzhan)
            return c.BindingId == "reward" && w.Window == SkillProgramTriggerWindow.CardUseCompleted && use.MouWenChouLunzhan is { } o && o.Source.OwnerSeat == c.OwnerSeat && o.Source.SkillInstanceId == c.SkillInstanceId && o.GameplayHash == c.GameplayHash && IsMouWenChouDuel(use.Id) && use.TargetSeats is [var target] && target != c.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouUseDamageEvent>().Any(d => d.UseId == use.Id && d.SourceSeat == c.OwnerSeat && d.TargetSeat == target && d.Amount > 0) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouStartedEvent>().Any(e => e.Operation == SkillProgramEffectOp.RewardMouWenChouLunzhan && e.UseId == use.Id);
        return c.SkillId == MouWenChouOlComposition.Juejue && c.BindingId == "all-hand" && w.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _coreWholeActualTurnUsesOperations.HasOngoingWholeActualUseTurn && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == c.OwnerSeat && a.ActorSeat == c.OwnerSeat && MouWenChouAllHandUse(use) && use.TargetSeats is [var sole] && sole != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[sole].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouJuejueQualifiedEvent>().Any(e => e.Entry.Source.OwnerSeat == c.OwnerSeat && e.Entry.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 33100, SkillProgramEffectOp.RewardMouWenChouLunzhan, SkillProgramEffectOp.ResolveMouWenChouJuejue)]
    internal bool QualifyMouWenChouUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => CanRunIssuedMouWenChou(c, w);
    internal SkillProgramStepOutcome BeginMouWenChouTrigger(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.MouWenChouOl is not null || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || !CanRunIssuedMouWenChou(c, w))
            throw new InvalidOperationException("Mou Wen Chou requires its original paid native action and exact qualifying card window.");
        var use = MouWenChouContextUse(c, w)!;
        var target = use.TargetSeats.Single();
        var amount = MouWenChouTargetCount(f.OwnerSeat, target);
        var r = new MouWenChouOlReceipt(MouWenChouSource(f), f.GameplayHash, op, op == SkillProgramEffectOp.RewardMouWenChouLunzhan ? MouWenChouOlStage.Reward : MouWenChouOlStage.Discard)
        {
            ActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber,
            UseId = use.Id,
            ActionId = use.Action!.ActionId,
            TargetSeat = target,
            Amount = amount
        };
        if (op == SkillProgramEffectOp.ResolveMouWenChouJuejue)
        {
            var entry = new MouWenChouJuejueEntry(r.Source, r.GameplayHash, use.Id, r.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, target, amount);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouWenChouJuejue = entry });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouJuejueQualifiedEvent(entry));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouStartedEvent(f.Id, r.Source, r.GameplayHash, op, use.Id));
        PublishMouWenChouOl(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.RewardMouWenChouLunzhan)]
    internal void ReturnMouWenChouDraw(ProgramSkillFrame f, NativeDrawOutcome draw)
    {
        if (!ValidMouWenChouOl(f) || f.MouWenChouOl is not { Stage: MouWenChouOlStage.Drawing, Draw: null } r || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != r.Amount)
            throw new InvalidOperationException("Lunzhan must return its frozen native reward draw exactly once.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouWenChouOl = r with { Draw = draw, Before = draw.SequenceBefore, After = draw.SequenceAfter, Stage = MouWenChouOlStage.Children }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouDrawReturnedEvent(f.Id, r.Amount, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count));
        ContinueMouWenChouOl(f.Id);
    }

    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 33100)]
    internal bool CaptureMouWenChouDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageId)
    {
        if (HasMouWenChouOl && attack.DamageWasApplied && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { MouWenChouLunzhan: not null } use && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == damageId)is DamageFrame { Amount: > 0 } damage && damage.ParentFrameId == use.Id && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouWenChouUseDamageEvent>().Any(e => e.DamageId == damageId))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouWenChouUseDamageEvent(use.Id, damageId, damage.SourceSeat, damage.TargetSeat, damage.Amount));
        return false;
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 33100)]
    internal bool ObserveMouWenChouAllUses(ProgramTargetCardWindowQuery q) => HasMouWenChouOl;
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 33100)]
    internal bool NormalizeMouWenChouDrawTwo(ProgramTargetCardWindowQuery q) => HasMouWenChouOl;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 33100)]
    internal void CollectIssuedMouWenChou(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { MouWenChouLunzhan: { } o } use || !IsMouWenChouDuel(use.Id) || !_coreMatchParticipantsPlayersAccess._players[o.Source.OwnerSeat].IsAlive)
            return;
        if (result.Any(c => c.SkillId == o.Source.SkillId && c.SkillInstanceId == o.Source.SkillInstanceId && c.OwnerSeat == o.Source.OwnerSeat && c.TriggerId == "reward"))
            return;
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program!.Triggers.Single(t => t.Id == "reward");
        var target = use.TargetSeats.FirstOrDefault(-1);
        if (target < 0)
            return;
        result.Add(new(o.Source.OwnerSeat, target, o.Source.SkillId, "reward", o.GameplayHash, o.Source.SkillInstanceId, trigger.Priority, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, o.Source.OwnerSeat, target, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[o.Source.OwnerSeat], action))));
    }
}
