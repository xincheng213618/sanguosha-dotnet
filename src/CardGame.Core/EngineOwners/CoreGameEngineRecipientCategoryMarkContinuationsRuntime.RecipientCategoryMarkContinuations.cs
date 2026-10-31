#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRecipientCategoryMarkContinuationsRuntime
{
    internal bool ExactRecipientCategoryDyingCursor(ProgramSkillFrame root, DyingFrame dying, ProgramLifecycleTriggerWindowFrame entry)
    {
        if (root.RecipientCategoryMark?.OriginalDyingCursor is not { } old || dying.Id != old.DyingFrameId || dying.ParentFrameId != old.ParentFrameId || dying.VictimSeat != root.OwnerSeat || dying.VictimSeat != old.VictimSeat || dying.KillerSeat != old.KillerSeat || dying.Continuation != old.Continuation || dying.Step != old.Step || dying.ResponderIndex != old.ResponderIndex || !dying.ResponderSeats.SequenceEqual(old.ResponderSeats) || !dying.AttemptedSelfDyingBindings.SequenceEqual(old.AttemptedSelfDyingBindings) || dying.PendingRecoveryAttempts is { Count: > 0 } || dying.PaidFactionRequestCostRecovery is not null || entry.Id != old.EntryFrameId || entry.CandidateIndex != old.CandidateIndex || entry.Step != old.EntryStep || entry.Candidates[entry.CandidateIndex] != old.Candidate || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == old.ParentFrameId)is not { } parent || parent.Kind != old.ParentKind || parent.Step != old.ParentStep || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == parent.Id) + 1 != _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) || global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime.DyingSuitsCursorHash<ResolutionFrame>(parent) != old.ParentHash || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == dying.VictimSeat && e.KillerSeat == dying.KillerSeat) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RecipientCategoryMarkDyingCursorIssuedEvent>().Count(e => e.FrameId == root.Id && e.DyingFrameId == dying.Id && e.CursorHash == global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime.DyingSuitsCursorHash(old)) != 1)
            return false;
        if (dying.Continuation == DyingContinuationKind.Damage)
            return parent is DamageFrame damage && damage.TargetSeat == dying.VictimSeat && damage.ParentFrameId == old.AttackOwnerFrameId && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == old.AttackOwnerFrameId)is { } owner && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == owner.Id) + 1 == _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == damage.Id) && global::CardGame.Core.EngineOwners.CoreGameEngineDyingSuitsAndHistoricalUseObserversRuntime.DyingSuitsCursorHash<ResolutionFrame>(owner) == old.AttackOwnerHash;
        return old.AttackOwnerFrameId is null && old.AttackOwnerHash is null && (dying.Continuation == DyingContinuationKind.ProgramSkill && parent is ProgramSkillFrame || _coreWenhePoisonOperations.IsWenhePoisonDyingChild(parent, dying) || dying.Continuation == DyingContinuationKind.AttackHpLoss && parent is ProgramSkillFrame or CardUseFrame or JudgmentFrame);
    }

    internal bool RecipientCategoryObserverSuffix(int rootIndex) => _coreObserverSubtreesOperations.HasObserverSuffix(rootIndex, (child) => _coreRecipientCategoryMarkContinuationsOperations.IsRecipientCategoryObserverEdge(child), completedDyingSuffix: null);
    internal bool IsRecipientCategoryObserverEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        return global::CardGame.Core.EngineOwners.CoreGameEngineObserverSubtreesRuntime.IsObserverSkillsChangedEdge(parent, current) || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkStructuralEdge(parent, current) || _coreOriginalHandEntityObserversOperations.OriginalHandEntityStructuralEdge(parent, current) || _coreOutsidePhaseDrawDiscardObserversOperations.OutsidePhaseDrawDiscardStructuralEdge(parent, current) || _coreRoundGainedEquipmentObserversOperations.RoundGainedEquipmentDrawStructuralEdge(parent, current) || _coreJoinedTrickDamageObserversOperations.JoinedTrickDamageRewardStructuralEdge(parent, current) || _corePairedColorResponseObserversOperations.PairedColorDispositionStructuralEdge(parent, current) || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandStructuralEdge(parent, current) || _coreCompletedUndamagedTargetRevealObserversOperations.CompletedUndamagedTargetRevealStructuralEdge(parent, current) || _coreResponseCompletionOperations.ResponseCompletionStructuralEdge(parent, current) || _coreCardSupplyCompletionOperations.CardSupplyCompletionStructuralEdge(parent, current) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child);
    }

    internal bool RecipientCategoryStructuralPrefix(ProgramSkillFrame f)
    {
        if (!_coreRecipientCategoryMarkObserversOperations.ValidRecipientCategoryMarkReceipt(f))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return i >= 1 && (i == _resolutionStackOperations._resolutionStack.Count - 1 || _coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && RecipientCategoryObserverSuffix(i));
    }

    internal bool RecipientCategoryPureDyingPrefix(ProgramSkillFrame f)
    {
        if (!_coreRecipientCategoryMarkObserversOperations.ValidRecipientCategoryMarkReceipt(f))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (i < 2)
            return false;
        if (i == _resolutionStackOperations._resolutionStack.Count - 1)
            return true;
        if (!_coreRecipientCategoryMarkObserversOperations.RecipientCategoryMarkFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]))
            return false;
        // DyingSuitsStructuralEdge is a native-edge predicate which never queries
        // ActiveDying. Do not call observer-root predicates from this filter.
        for (var child = i + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            var parent = _resolutionStackOperations._resolutionStack[child - 1];
            var current = _resolutionStackOperations._resolutionStack[child];
            if (current is ProgramLifecycleTriggerWindowFrame skills && parent is ProgramSkillFrame acquired && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == acquired.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count)
                continue;
            if (!_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(parent, current) && !_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossStructuralEdge(parent, current) && !_coreOverflowTargetCancellationObserversOperations.OverflowTargetCancellationStructuralEdge(parent, current) && !_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(parent, current))
                return false;
        }

        return true;
    }

    internal bool IsOriginalDyingSuspendedByRecipientCategoryMark(DyingFrame dying) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.RecipientCategoryMark is { Consumed: true, RecoveryIssued: true, Stage: not RecipientCategoryMarkStage.Complete } r && r.DyingFrameId == dying.Id && RecipientCategoryPureDyingPrefix(f));
    internal ProgramSkillFrame? RecipientCategoryMarkObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreRecipientCategoryMarkObserversOperations2.RecipientCategoryMarkFirstChild(root, child), (index) => _coreRecipientCategoryMarkContinuationsOperations.RecipientCategoryObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 530, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 610)]
    internal bool IsRecipientCategoryMarkProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && RecipientCategoryMarkObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1840)]
    internal bool HasRecipientCategoryMarkDamageObserver(long windowId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == windowId && f is DamageTriggerWindowFrame);
        if (index < 0 || RecipientCategoryMarkObserverRoot()is not { } root)
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        if (index > rootIndex)
            return true; // The validated suffix contains this exact native descendant.
        // An enclosing damage observer is accepted only if each edge from its
        // actual candidate through the original Dying entry to this root agrees.
        for (var child = index + 1; child <= rootIndex; child++)
            if (!_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]))
                return false;
        return index < rootIndex;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 10)]
    internal bool TryAdvanceRecipientCategoryMarkSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || RecipientCategoryMarkObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
