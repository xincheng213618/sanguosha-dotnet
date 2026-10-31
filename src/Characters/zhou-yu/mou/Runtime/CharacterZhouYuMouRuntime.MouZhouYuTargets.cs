#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouRuntime
{
    internal bool HasMouZyFireTargets(CharacterState actor) => MouZyAnniversary.Budget(actor.Seat) > 0;
    internal bool IsMouZyTargetAction(CharacterState actor, LegalAction action) => action.ProgramSkillId == MouZhouYuRuntime.Ronghuo && action.ProgramActivationId == MouZhouYuRuntime.Extra && HasMouZyFireTargets(actor) && (action.PlayedCardKind ?? (action.CardId is { } id ? _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(actor, _coreAdvancedLifecycleOperations.GetAdvancedCard(id)) : CardKind.Dodge))is CardKind.FireSlash or CardKind.FireAttack;
    internal int MouZyTargetLimit(CharacterState actor) => MouZyAnniversary.Budget(actor.Seat);
    [ProgramCardAction(ProgramCardActionPhase.AfterProgram, 36000)]
    internal void AddMouZyFireTargets(List<LegalAction> actions, CharacterState actor)
    {
        var budget = MouZyAnniversary.Budget(actor.Seat);
        if (budget <= 0)
            return;
        var keys = actions.Select(a => (a.CardId, a.Kind, a.PlayedCardKind, a.ConversionSource, a.TargetCardId, Targets: string.Join(",", a.TargetSeats))).ToHashSet();
        foreach (var a in actions.ToArray().Where(a => a.CardId is not null && a.ProgramActivationId is null && a.Kind is LegalActionKind.Slash or LegalActionKind.FireAttack))
        {
            var(kind, suit) = _coreNextActualUseTargetAdjustmentsOperations.TargetAdjustmentAppearance(actor, a);
            if (kind is not (CardKind.FireSlash or CardKind.FireAttack))
                continue;
            var normal = a.TargetSeats;
            var extra = _coreMatchParticipantsPlayersAccess._players.Where(p => !normal.Contains(p.Seat) && _coreNextCardTargetAdjustmentsOperations.CanBeExtraNextCardTarget(actor, p, a, kind, suit) && (kind != CardKind.FireSlash || _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, p, _coreAdvancedLifecycleOperations.GetAdvancedCard(a.CardId!.Value), a.ConversionSource, kind)) && (kind != CardKind.FireAttack || _corePhysicalOwnedCardQueriesOperations.GetHand(p).Any(c => p.Seat != actor.Seat || c.Id != a.CardId))).Select(p => p.Seat).ToArray();
            foreach (var group in MouZyExtraGroups(extra, budget).Where(g => g.Count > 0))
            {
                var targets = Array.AsReadOnly(normal.Concat(group).ToArray());
                if (!keys.Add((a.CardId, a.Kind, a.PlayedCardKind, a.ConversionSource, a.TargetCardId, string.Join(",", targets))))
                    continue;
                actions.Add(a with { TargetSeat = targets[0], TargetSeats = targets, ProgramSkillId = MouZhouYuRuntime.Ronghuo, ProgramActivationId = MouZhouYuRuntime.Extra, Description = a.Description + "（融火额外目标）" });
            }
        }
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 36000)]
    internal bool MouZyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouZhouYuAnniversary is { } r && effect?.Op == r.Operation && MouZyAnniversary.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouZyFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouZyAnniversary.Valid(f) || f.MouZhouYuAnniversary is not { } r)
            return false;
        if (child is CardUseFrame u)
            return MouZyNativeRidesOn(u, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouZyMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouZhouYuStage.Drawing;
        if (r.Stage == MouZhouYuStage.DamageChildren && f.AttackAttempt is { } a && f.AttackReturn is not null && a.TargetSeat == r.Target && a.SourceSeat == f.OwnerSeat && !a.SourceLess)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle.Window == SkillProgramTriggerWindow.SkillsChanged && lifecycle.Continuation == ProgramLifecycleContinuation.ResumeParentProgram);
    }

    internal bool MouZyEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouZyFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouZySuffix(int i) => _resolutionStackOperations._resolutionStack[i + 1] is DyingFrame d && i + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterZhouYuMouOperations.MouZyEdge(n), (n, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouZyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhouYuMouOperations.MouZyFirstChild(f, c), (n) => _characterZhouYuMouOperations.MouZySuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 36000)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 36000)]
    internal bool MouZyPaidDamage(long id) => MouZyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 36000)]
    internal bool MouZyBeforeDamage(long id) => MouZyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 36000)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 36000)]
    internal bool MouZyDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouZyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 36000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 36000)]
    internal bool MouZyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouZyFirstChild(f, recovery))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && n > i + 1 && MouZyFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouZySuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[n - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 36000)]
    internal bool MouZyAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
