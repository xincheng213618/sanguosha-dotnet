#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMiHengLeRuntime
{
    internal bool JiguRootMatches(ProgramSkillFrame f)
    {
        if (f.Jigu is not { Paid: true } r || f.SkillId != "le:jigu" || f.TriggerId != "damage-equipment-draw" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != JiguSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, OccurrenceIndex: 0 } c || c.OwnerSeat != f.OwnerSeat || c.ParentFrameId != r.DamageWindowId || c.DamageFrameId != r.DamageFrameId || c.Amount <= 0 || c.SourceSeat != f.OwnerSeat && c.TargetSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.DamageWindowId)is not DamageTriggerWindowFrame window || window.ParentFrameId != r.DamageFrameId || window.TriggerWindow != c.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.DamageFrameId)is not DamageFrame damage || damage.TargetSeat != c.TargetSeat || damage.Amount != c.Amount)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == r.DamageWindowId && _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid == JiguPayment(f.Id, r) && _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.DamageFrameId == r.DamageFrameId) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28211)]
    internal void AssertJigu(ProgramSkillFrame f)
    {
        if (f.Jigu is not { } r)
            return;
        if (!JiguRootMatches(f) || !Enum.IsDefined(r.Stage) || r.RoundNumber < 0 || r.ActualTurn < 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TurnOwner) || r.MarkedHandCount < 0 || r.MarkedHandCount != r.EquipmentCount || r.EmptySlotCount < 0 || r.StartedSeatCount < 0 || r.StartedSeatCount > _coreMatchParticipantsPlayersAccess._players.Count || r.UseNumber < 1 || r.DisabledForRound != (r.UseNumber >= r.StartedSeatCount) || _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Where(e => e.Source.OwnerSeat == f.OwnerSeat && e.RoundNumber == r.RoundNumber).Count(e => e.UseNumber < r.UseNumber) != r.UseNumber - 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<JiguActivationPaidEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.RoundNumber == r.RoundNumber && e.UseNumber < r.UseNumber && e.DisabledForRound))
            throw new InvalidOperationException("Jigu lost its exact damage mother, paid equipment sample or round latch.");
        var returned = _coreProgramEventHistoryOperations.ProgramEventHistory<JiguDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == JiguStage.Drawing)
        {
            if (r.DrawOutcome is not null || returned.Length != 0 || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Jigu cannot reuse an earlier native draw as a new request.");
            return;
        }

        if (r.DrawOutcome is not { } o || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.EmptySlotCount || o.SequenceBefore < 0 || o.SequenceAfter < o.SequenceBefore || o.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || returned is not [var actual] || actual != new JiguDrawReturnedEvent(f.Id, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId) || f.NativeDraw is not null || f.PendingMovementContinuation is not { } p || !IsLeMiHengMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            throw new InvalidOperationException("Jigu lost its native actual/replacement outcome or unfinished movement children.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28211)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28211)]
    internal bool PermitPaidJigu(ProgramSkillFrame f)
    {
        if (f.Jigu is not { Paid: true })
            return false;
        AssertJigu(f);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 200)]
    internal bool IsLeMiHengMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.Jigu is { Paid: true, Stage: JiguStage.DrawChildren, DrawOutcome: not null } && effect?.Op == SkillProgramEffectOp.ResolveJiguDamageEquipmentDraw && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool JiguDrawMovement(ProgramSkillFrame f, CardMovementRecord movement)
    {
        if (f.Jigu is not { Paid: true, Stage: JiguStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement) || movement.Sequence <= o.SequenceBefore || movement.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(movement.CardId) && movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(f.OwnerSeat) && movement.Reason.Value == JiguDrawReason || o.ReplacementGainedIds.Contains(movement.CardId) && movement.To == CardLocation.Hand(f.OwnerSeat) || movement.From == CardLocation.DiscardPile && movement.To == CardLocation.DrawPile && movement.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool JiguFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Jigu is not { Paid: true } r)
            return false;
        AssertJigu(f);
        if (r.Stage == JiguStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => JiguDrawMovement(f, m));
    }

    internal bool JiguObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: { Paid: true } } replacement && _characterSunHongOlOperations.ZenrunFirstChild(replacement, _resolutionStackOperations2._resolutionStack()[n]) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? JiguObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterMiHengLeOperations.JiguFirstChild(f, child), (index) => _characterMiHengLeOperations.JiguObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1130)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1240)]
    internal bool HasLeMiHengDamageObserver(long id)
    {
        if (JiguObserverRoot()is not { Jigu: { Paid: true } r } root || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id)is not DamageTriggerWindowFrame window)
            return false;
        if (_resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id))
            return true;
        // A paid Jigu draw also suspends the original damage window directly
        // below it. Its whole native draw/replacement/observer suffix must be
        // proved before that exact mother can retain its outstanding cursor.
        return r.DamageWindowId == window.Id && r.DamageFrameId == window.ParentFrameId && root.WindowContext?.ParentFrameId == window.Id && window.TriggerWindow == SkillProgramTriggerWindow.AfterDamageApplied && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, window);
    }

    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1270)]
    internal bool HasLeMiHengBeforeDamageObserver(long id) => JiguObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1100)]
    internal bool IsLeMiHengDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && JiguObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 360)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 350)]
    internal bool IsLeMiHengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Jigu is not { Paid: true } || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && JiguFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && JiguObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28211)]
    internal bool AdvanceJiguChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && JiguObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
