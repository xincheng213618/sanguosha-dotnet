#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaXiuMaTieOlRuntime
{
    internal bool IsKenshangTargetsReplacedFact(CardUseFrame use, SlashTargetsReplacedEvent fact)
    {
        if (use.Kenshang is not { RewriteFrameId: { } frame, RewrittenTargets: { } targets } r || !KenshangUseProof(use) || fact.FrameId != frame || fact.CardUseFrameId != use.Id || fact.SkillId != r.Source.SkillId || fact.OwnerSeat != r.Source.OwnerSeat || targets.Count != r.MaterialCount || targets.Distinct().Count() != targets.Count || !fact.Targets.SequenceEqual(targets) || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangTargetsCommittedEvent>().Count(e => e.ProgramFrameId == frame && e.UseId == use.Id && e.ActionId == r.ActionId && e.Count == targets.Count) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == frame && e.SkillId == fact.SkillId && e.OwnerSeat == fact.OwnerSeat && e.SkillInstanceId == r.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseCommitted) != 1)
            return false;
        var selected = _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangTargetSelectedEvent>().Where(e => e.ProgramFrameId == frame && e.UseId == use.Id).OrderBy(e => e.Index).ToArray();
        return selected.Length == targets.Count && selected.Select(e => e.Index).SequenceEqual(Enumerable.Range(0, targets.Count)) && selected.Select(e => e.TargetSeat).SequenceEqual(targets) && _contentRegistryOperations._contentRegistry.GetSkill(fact.SkillId).Program!.Triggers.Any(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveKenshangTargetRewrite }]);
    }

    internal bool HasKenshangTargetTail(CardUseFrame use) => use.Action is { } a && HasKenshangTargetTail(use, a);
    internal bool HasKenshangTargetTail(CardUseFrame use, CardActionContext action)
    {
        if (use.Kenshang is not { RewrittenTargets: { } targets } r || use.Action?.ActionId != action.ActionId || !KenshangUseProof(use) || !action.TargetSeats.SequenceEqual(targets) || _coreProgramEventHistoryOperations.ProgramEventHistory<SlashTargetsReplacedEvent>().Where(e => e.CardUseFrameId == use.Id && e.FrameId == r.RewriteFrameId).ToArray()is not [var changed] || !IsKenshangTargetsReplacedFact(use, changed))
            return false;
        var actual = targets.ToArray();
        foreach (var d in _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRedirectedEvent>().Where(e => e.UseId == use.Id))
        {
            if (d.ActionId != action.ActionId || d.TargetIndex < 0 || d.TargetIndex >= actual.Length || actual[d.TargetIndex] != d.PreviousSeat || d.Source.OwnerSeat != d.PreviousSeat || _contentRegistryOperations._contentRegistry.GetSkill(d.Source.SkillId).Program is not { } p || p.GameplayHash != d.GameplayHash || ProgramInstructionResolver.Default.Find(p, ProgramInstructionSourceKind.Trigger, d.Source.BindingId)is not { } plan || plan.Trigger?.Window != SkillProgramTriggerWindow.SlashTargetRedirecting || d.InstructionIndex < 1 || d.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(d.InstructionIndex).Effect.Op != SkillProgramEffectOp.RedirectCurrentAttack || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == d.ProgramFrameId && e.OwnerSeat == d.Source.OwnerSeat && e.SkillId == d.Source.SkillId && e.BindingId == d.Source.BindingId && e.SkillInstanceId == d.Source.SkillInstanceId) != 1)
                return false;
            actual[d.TargetIndex] = d.TargetSeat;
        }

        return use.TargetSeats.SequenceEqual(actual);
    }

    internal void RecordKenshangRedirect(ProgramSkillFrame f, long id, int previous, int next)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Kenshang: { RewrittenTargets: not null } r, Action: { } a } use)
            return;
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } card } c || c.TargetSeat != previous || card.ParentCardUseFrameId != id || card.CardActionId != a.ActionId || f.OwnerSeat != previous || f.InstructionIndex < 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { Op: SkillProgramEffectOp.RedirectCurrentAttack } e || next != global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(f, e.Target) || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.TargetSeats[use.TargetIndex] != next || _coreProgramEventHistoryOperations.ProgramEventHistory<KenshangRedirectedEvent>().Any(d => d.UseId == id && d.ProgramFrameId == f.Id && d.InstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("Kenshang target redirect lost its actual native producer or current target cursor.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new KenshangRedirectedEvent(f.Id, id, a.ActionId, new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, f.InstructionIndex, use.TargetIndex, previous, next));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27110)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27110)]
    internal bool ContinuePaidKenshang(ProgramSkillFrame f)
    {
        if (f.Kenshang is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is { } id && f.ActivationId == id && f.WindowContext is { } c && CanRunIssuedKenshangCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 560)]
    internal bool IsKenshangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.Kenshang is not { Stage: KenshangProgramStage.DrawChildren } || e?.Op != SkillProgramEffectOp.DrawForKenshangShortDamage || p != f.PendingMovementContinuation)
            return false;
        return true;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 830)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 770)]
    internal bool IsKenshangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => false;
    internal bool KenshangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Kenshang is not { Stage: KenshangProgramStage.DrawChildren } r)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame s && s.ResumeProgramFrameId == f.Id)
            return s.Window == SkillProgramTriggerWindow.SkillsChanged && s.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return f.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame m && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, m) && m.Batch.OriginOwnerSeat == f.OwnerSeat && m.Batch.OriginSkillId == f.SkillId && m.Batch.OriginSkillInstanceId == f.SkillInstanceId && m.Batch.Movements.All(x => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(x) && x.Sequence > r.Before && x.Sequence <= r.After);
    }

    internal ProgramSkillFrame? KenshangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterMaXiuMaTieOlOperations.KenshangFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 670)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 780)]
    internal bool HasKenshangDamageObserver(long id) => KenshangObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 810)]
    internal bool HasKenshangBeforeDamageObserver(long id) => KenshangObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27110)]
    internal bool AdvanceKenshangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KenshangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
