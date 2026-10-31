#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengDaOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27240)]
    internal void AssertMengDa(ProgramSkillFrame f)
    {
        if (f.Goude is not { } r)
            return;
        if (f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveGoudeActualTurnEnd || r.Source != MengDaSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.HistoryCutoff < 0 || r.HistoryCutoff > _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count() || r.ExecutedMask is <= 0 or >= 15 || r.ExecutedMask != GoudeExecutedMask(r.ActualTurn, r.FactionSeats, r.HistoryCutoff) || r.FactionSeats.Distinct().Count() != r.FactionSeats.Count || !r.FactionSeats.Contains(f.OwnerSeat) || r.FactionSeats.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || string.IsNullOrWhiteSpace(r.FactionId) || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == r.WindowId)is not { } parent || !_coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, f, context) || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new GoudeStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, r.WindowId, r.HistoryCutoff, r.FactionId, r.ExecutedMask))
            throw new InvalidOperationException("Goude lost its exact actual-ended parent, current-faction frozen seats or whole-invocation history mask.");
        var factions = _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeFactionSeatFrozenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (factions.Length != r.FactionSeats.Count || factions.Where((e, i) => e.Index != i || e.Seat != r.FactionSeats[i]).Any())
            throw new InvalidOperationException("Goude's faction set differs from its original scalar invoices.");
        var chosen = _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeOptionChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == GoudeStage.Option ? r.Option is not null || chosen.Length != 0 : r.Option is not { } option || !Enum.IsDefined(option) || (r.ExecutedMask & (int)option) != 0 || chosen is not [var selected] || selected.Option != option)
            throw new InvalidOperationException("Goude cannot choose an already executed option, invent or repeat its branch.");
        var targets = _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeTargetChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.TargetSeat is { } target)
        {
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || targets is not [var selectedTarget] || selectedTarget.Option != r.Option || selectedTarget.TargetSeat != target || r.Option is not (GoudeOption.Discard or GoudeOption.Slash))
                throw new InvalidOperationException("Goude lost its exact original branch target.");
        }
        else if (targets.Length != 0 || r.Materials.Count != 0 || r.PaidMaterial is not null)
            throw new InvalidOperationException("Goude contains Hand material before a real target selection.");
        var materials = _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeHandMaterialFrozenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (materials.Length != r.Materials.Count || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || materials.Where((e, i) => e.Index != i || e.Material != r.Materials[i] || e.Material.HolderSeat != r.TargetSeat || e.Material.HandEntry < 0 || e.Material.Slot < 0).Any())
            throw new InvalidOperationException("Goude lost its original private Hand candidates.");
        if (r.Stage == GoudeStage.DiscardCard && (!r.Materials.SequenceEqual(GoudeDiscardMaterials(f.OwnerSeat, r.TargetSeat!.Value)) || r.PaidMaterial is not null))
            throw new InvalidOperationException("Goude's unpaid opaque Hand slots changed.");
        if (r.NativeOrigin is { } origin)
        {
            if (r.Option != GoudeOption.Slash || r.Stage is not (GoudeStage.NativeSlash or GoudeStage.NativeChildren) || origin.ProgramFrameId != f.Id || origin.Source != r.Source || origin.GameplayHash != r.GameplayHash || origin.ActualTurn != r.ActualTurn || origin.TurnOwner != r.TurnOwner || origin.OriginalTarget != r.TargetSeat || origin.OriginalActor != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeSlashIssuedEvent>().Count(e => e.Origin == origin) != 1 || (r.NativeCompleted ? _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == origin.UseId) != 1 : !IsGoudeSlashUse(origin.UseId)))
                throw new InvalidOperationException("Goude lost its exact issued native zero-entity Slash and typed completion.");
        }
        else if (r.Stage is GoudeStage.NativeSlash or GoudeStage.NativeChildren || r.NativeCompleted)
            throw new InvalidOperationException("Goude's native child lost its immutable use invoice.");
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeMovementPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == GoudeStage.MovementChildren)
        {
            if (paid is not [var payment] || payment.Option != r.Option || payment.Before != r.Before || payment.After != r.After || payment.ActualCount != r.ActualCount || payment.Material != r.PaidMaterial || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualCount is < 0 or > 1)
                throw new InvalidOperationException("Goude lost its once-paid actual movement interval.");
            var reason = r.Option == GoudeOption.Draw ? GoudeDrawReason : GoudeDiscardReason;
            if (r.Option is GoudeOption.Draw or GoudeOption.Discard)
            {
                var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == reason).ToArray();
                if (moves.Length != r.ActualCount || r.Option == GoudeOption.Draw && moves.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(f.OwnerSeat)) || r.Option == GoudeOption.Discard && (r.PaidMaterial is not { } m || !r.Materials.Contains(m) || r.ActualCount != 1 || moves is not [var move] || move.CardId != m.CardId || move.CardKind != m.Kind || move.From != CardLocation.Hand(m.HolderSeat) || move.To != CardLocation.DiscardPile || move.Sequence != payment.Sequence))
                    throw new InvalidOperationException("Goude cannot replace or repay its original actual draw or Hand discard.");
            }
            else if (r.Option != GoudeOption.Faction || r.NewFaction is null || r.ActualCount != 0 || r.Before != r.After || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeFactionChosenEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var changed] || changed.Current != r.NewFaction || changed.Previous == changed.Current || !_coreProgramEventHistoryOperations.ProgramEventHistory<NativeEffectiveFactionChangedEvent>().Any(e => e.ParentFrameId == f.Id && e.TargetSeat == f.OwnerSeat && e.Source == r.Source && e.PreviousFactionId == changed.Previous && e.CurrentFactionId == changed.Current && e.Cause == "program"))
                throw new InvalidOperationException("Goude lost its genuine effective-faction change.");
        }
        else if (paid.Length != 0 || r.PaidMaterial is not null || r.NewFaction is not null || r.Before != 0 || r.After != 0 || r.ActualCount != 0)
            throw new InvalidOperationException("Goude cannot contain a payment outside its original paid branch.");
        var pending = r.Stage is GoudeStage.MovementChildren or GoudeStage.NativeChildren;
        if (pending ? f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat : f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Goude lost its owning native movement continuation.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27240)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27240)]
    internal bool ContinuePaidMengDa(ProgramSkillFrame f)
    {
        if (f.Goude is not { Stage: GoudeStage.MovementChildren or GoudeStage.NativeSlash or GoudeStage.NativeChildren })
            return false;
        AssertMengDa(f);
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 280)]
    internal bool IsMengDaMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.Goude is not { Stage: GoudeStage.MovementChildren or GoudeStage.NativeChildren } || e?.Op != SkillProgramEffectOp.ResolveGoudeActualTurnEnd || p != f.PendingMovementContinuation)
            return false;
        AssertMengDa(f);
        return true;
    }

    internal bool MengDaFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Goude is not { } r)
            return false;
        AssertMengDa(f);
        if (child is CardUseFrame use)
            return r.Stage == GoudeStage.NativeSlash && use.GoudeSlash == r.NativeOrigin && IsGoudeSlashUse(use.Id);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == GoudeStage.MovementChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (r.Option == GoudeOption.Draw ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == GoudeDrawReason || m.Reason == CardMoveReasons.Reshuffle : r.PaidMaterial is { } material && m.CardId == material.CardId && m.From == CardLocation.Hand(material.HolderSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == GoudeDiscardReason));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? MengDaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterMengDaOlOperations.MengDaFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 730)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 840)]
    internal bool HasMengDaDamageObserver(long id) => MengDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 870)]
    internal bool HasMengDaBeforeDamageObserver(long id) => MengDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 700)]
    internal bool IsMengDaDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MengDaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27240)]
    internal bool AdvanceMengDaSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MengDaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
