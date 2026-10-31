#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangTongAnniversaryGodRuntime
{
    internal bool ShenPtPaidRemoval(long frame, long death, int card, long before, long after, long batch) => card > 0 && batch > 0 && after > before && after <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreAdvancedLifecycleOperations.GetAdvancedCard(card).Kind is CardKind.FireSlash or CardKind.FireAttack && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenPangTongRemovalPaidEvent>().Count(e => e == new ShenPangTongRemovalPaidEvent(frame, death, batch, before, after)) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == card && m.Sequence > before && m.Sequence == after && m.From == CardLocation.DrawPile && m.To == CardLocation.OutsideGame && m.Reason.Value == ShenPangTongRemovalReason) == 1;
    internal void RequireShenPtRevival(DeathFrame death, ShenPangTongRevivalReceipt r)
    {
        if (death.VictimSeat != r.Source.OwnerSeat || r.Source.SkillId != ShenPangTongComposition.Kunyu || r.Source.BindingId != "death-rebirth" || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || !ShenPtPaidRemoval(r.ProgramFrameId, death.Id, r.RemovedCardId, r.Before, r.After, r.BatchId) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenPangTongStartedEvent>().Count(e => e.FrameId == r.ProgramFrameId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Operation == SkillProgramEffectOp.ResolveShenPangTongKunyu && e.DeathId == death.Id && e.ParentId == r.WindowId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenPangTongRevivalGrantedEvent>().Count(e => e == new ShenPangTongRevivalGrantedEvent(r.ProgramFrameId, death.Id, death.VictimSeat)) != 1)
            throw new InvalidOperationException("Kunyu lost its original death, paid fire removal or typed revival entitlement.");
    }

    internal bool ShenPtActualDamageTarget(ProgramSkillFrame f, ShenPangTongReceipt r, AttackAttemptState a)
    {
        var transferred = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramDamageTransferredEvent>().Where(e => e.ResolutionId == f.Id && e.SourceSeat == f.OwnerSeat && e.Nature == DamageNature.Fire).ToArray();
        var initial = r.TargetSeat;
        if (a.DamageRedirected && transferred.FirstOrDefault(e => e.OwnerSeat == initial)is { } redirect)
            initial = redirect.TargetSeat;
        if (!a.IsChainPropagation)
            return a.TargetSeat == initial;
        var targets = a.ChainedTargetSeats ?? [];
        var cursor = a.ChainedTargetIndex;
        if (targets.Count == 0 || targets.Distinct().Count() != targets.Count || targets.Contains(initial) || cursor < 1 || cursor > targets.Count || targets[cursor - 1] != a.TargetSeat)
            return false;
        var steps = _coreProgramEventHistoryOperations.ProgramEventHistory<ChainedDamagePropagatedEvent>().Where(e => e.ResolutionId == f.Id).ToArray();
        var previous = initial;
        var previousIndex = -1;
        foreach (var step in steps)
        {
            var index = Array.IndexOf(targets.ToArray(), step.TargetSeat);
            if (step.SourceSeat != f.OwnerSeat || step.FromSeat != previous || step.Nature != DamageNature.Fire || step.Amount <= 0 || index <= previousIndex || index >= cursor)
                return false;
            previous = step.TargetSeat;
            previousIndex = index;
        }

        return previousIndex == cursor - 1 && previous == a.TargetSeat;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 39200)]
    internal bool ShenPtMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p)
    {
        if (f.ShenPangTong is not { Operation: SkillProgramEffectOp.ResolveShenPangTongKunyu, Stage: ShenPangTongStage.RemovalChildren } r || effect?.Op != r.Operation || p != f.PendingMovementContinuation || p.SubjectSeat != f.OwnerSeat || p.BeforeCount != 0 || p.CoverageResultBind is not null)
            return false;
        return ShenPtPaidRemoval(f.Id, r.DeathId, r.RemovedCardId, r.Before, r.After, r.BatchId);
    }

    internal bool ShenPtFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ShenPangTong is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.Batch.ParentFrameId != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId)
                return false;
            if (r.Stage == ShenPangTongStage.RemovalChildren)
                return f.PendingMovementContinuation is { } p && ShenPtMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Id == r.BatchId && moved.Batch.Movements is [var m] && m.CardId == r.RemovedCardId && m.Sequence == r.After;
            return r.Stage == ShenPangTongStage.DrawChildren && moved.ResumeProgramFrameId == f.Id && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason.Value == "skill-program.anniversary:fengliao.draw");
        }

        return r.Stage == ShenPangTongStage.DamageChildren && f.AttackAttempt is { Nature: DamageNature.Fire, SourceLess: false } a && a.SourceSeat == f.OwnerSeat && ShenPtActualDamageTarget(f, r, a) && f.AttackReturn is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? ShenPtObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterPangTongAnniversaryGodOperations.ShenPtFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 39200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 39200)]
    internal bool ShenPtDamageWindow(long id) => ShenPtObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 39200)]
    internal bool ShenPtBeforeDamage(long id) => ShenPtObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 39200)]
    internal bool ShenPtDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ShenPtObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 39200)]
    internal bool AdvanceShenPtChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShenPtObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
