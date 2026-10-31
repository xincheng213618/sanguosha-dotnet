#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYaDanOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27260)]
    internal void AssertYaDan(ProgramSkillFrame f)
    {
        if (f.YaDan is not { } r)
            return;
        if (f.InstructionIndex != 1 || !IsYaDanOperation(r.Op) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || r.Source.OwnerSeat != f.OwnerSeat || r.Source.SkillId != f.SkillId || r.Source.SkillInstanceId != f.SkillInstanceId || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext?.ParentFrameId != r.ParentId || r.IssuedHistoryCutoff < 0 || r.IssuedHistoryCutoff > _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count() || _coreProgramEventHistoryOperations.ProgramEventHistory<YaDanStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new YaDanStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActualTurn, r.ParentId, r.OriginalUseId, r.OriginalActionId, r.OriginalTarget, r.IssuedPhase, r.IssuedHistoryCutoff, r.GrantId, r.DuePhase))
            throw new InvalidOperationException("Qingya lost its original issuing source, native window, actual turn or frozen scalar receipt.");
        if (r.Op == SkillProgramEffectOp.ResolveQingyaBetweenHandDiscards)
        {
            if (r.Source != YaDanSource(f) || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } context } || context.ParentCardUseFrameId != r.OriginalUseId || context.CardActionId != r.OriginalActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is not { Action: { } a } use || a.ActionId != r.OriginalActionId || a.ProviderSeat != r.OriginalProvider || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.ParentId)is not { } window || window.ParentFrameId != use.Id || window.Action.ActionId != a.ActionId || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window) != SkillProgramTriggerWindow.CardUseTargetsFinalized || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || r.OriginalTarget is not { } target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == f.OwnerSeat || r.GrantId is not null || r.DuePhase is not null || r.SelectedAction is not null || r.NativeOrigin is not null || r.LivingSeats.Distinct().Count() != r.LivingSeats.Count || !r.LivingSeats.Contains(target) || !r.LivingSeats.Contains(f.OwnerSeat))
                throw new InvalidOperationException("Qingya's initial sweep lost its exact genuine Slash and original live-seat geometry.");
            var living = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaLivingSeatEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (living.Length != r.LivingSeats.Count || living.Where((e, i) => e.Index != i || e.Seat != r.LivingSeats[i]).Any())
                throw new InvalidOperationException("Qingya changed its frozen original living roster.");
            var expected = QingyaArcs(f.OwnerSeat, target, r.LivingSeats);
            if (r.Arcs.Count != expected.Count || r.Arcs.Where((arc, i) => arc.Direction != expected[i].Direction || !arc.Seats.SequenceEqual(expected[i].Seats)).Any())
                throw new InvalidOperationException("Qingya cannot turn a tied side into a union or recompute dead-seat geometry after payment.");
            var arcFacts = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaArcFrozenEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (arcFacts.Length != r.Arcs.Count || r.Arcs.Any(arc => arcFacts.Count(e => e.Direction == arc.Direction && e.Count == arc.Seats.Count) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<QingyaArcSeatEvent>().Where(e => e.FrameId == f.Id && e.Direction == arc.Direction).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(arc.Seats)))
                throw new InvalidOperationException("Qingya's original side seats differ from their scalar invoices.");
            var side = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaSideChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Stage == QingyaStage.SideChoice ? r.Direction is not null || r.Targets.Count != 0 || side.Length != 0 : r.Direction is not { } direction || side is not [var chosen] || chosen.Direction != direction || !r.Arcs.Any(arc => arc.Direction == direction && arc.Seats.SequenceEqual(r.Targets)))
                throw new InvalidOperationException("Qingya cannot pay both equal sides or substitute its chosen side.");
            if (r.Cursor < 0 || r.Cursor > r.Targets.Count || r.Stage is not (QingyaStage.SideChoice or QingyaStage.HandChoice or QingyaStage.DiscardChildren))
                throw new InvalidOperationException("Qingya lost its native discard cursor.");
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaHandDiscardedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Cursor).ToArray();
            if (!r.Entities.SequenceEqual(paid.Select(e => new QingyaDiscardEntity(e.Material.CardId, e.Material.Kind, e.Material.HolderSeat, e.Sequence))) || paid.Select(e => e.Cursor).Distinct().Count() != paid.Length || paid.Any(e => e.Cursor < 0 || e.Cursor >= r.Targets.Count || e.Material.HolderSeat != r.Targets[e.Cursor] || !_coreProgramEventHistoryOperations.ProgramEventHistory<QingyaHandCandidateEvent>().Any(c => c.FrameId == f.Id && c.Cursor == e.Cursor && c.Material == e.Material) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.Sequence > e.Before && m.Sequence <= e.After && m.CardId == e.Material.CardId && m.CardKind == e.Material.Kind && m.From == CardLocation.Hand(e.Material.HolderSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == QingyaDiscardReason)))
                throw new InvalidOperationException("Qingya lost a genuine original opaque Hand payment or counted cleanup as a discard.");
            var skipped = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaParticipantSkippedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (skipped.Any(e => e.Cursor < 0 || e.Cursor >= r.Cursor || e.Seat != r.Targets[e.Cursor]) || Enumerable.Range(0, r.Cursor).Any(i => paid.Count(e => e.Cursor == i) + skipped.Count(e => e.Cursor == i) != 1) || paid.Any(e => e.Cursor > r.Cursor || e.Cursor == r.Cursor && r.Stage != QingyaStage.DiscardChildren))
                throw new InvalidOperationException("Qingya skipped or repeated an already processed original participant.");
            var materials = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaHandCandidateEvent>().Where(e => e.FrameId == f.Id && e.Cursor == r.Cursor).ToArray();
            if (materials.Length != r.Materials.Count || materials.Where((e, i) => e.Index != i || e.Material != r.Materials[i]).Any() || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || r.Stage == QingyaStage.HandChoice && r.Cursor < r.Targets.Count && !r.Materials.SequenceEqual(QingyaHandMaterials(f.OwnerSeat, r.Targets[r.Cursor])))
                throw new InvalidOperationException("Qingya changed its exact current private Hand candidates.");
            if (r.Stage == QingyaStage.DiscardChildren && (paid.LastOrDefault()is not { } last || last.Cursor != r.Cursor || r.Before != last.Before || r.After != last.After))
                throw new InvalidOperationException("Qingya lost the exact once-paid movement interval.");
        }
        else
        {
            if (f.WindowContext is not { Window: SkillProgramTriggerWindow.ActualPhaseEnded, ActualPhaseEnd: { } due } context || r.GrantId != due.EntitlementId || r.DuePhase != due.Phase || !YaDanOwnsActualPhaseEnd(f, due) || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaGrantIssuedEvent>().SingleOrDefault(g => g.GrantId == r.GrantId)is not { } grant || !ValidQingyaGrant(grant) || grant.Source != r.Source || grant.GameplayHash != r.GameplayHash || !r.Entities.SequenceEqual(QingyaGrantedEntities(grant)) || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaGrantConsumedEvent>().Count(e => e.GrantId == grant.GrantId && e.FrameId == f.Id && e.Phase == due.Phase) != 1 || r.Stage is not (QingyaStage.NativeChoice or QingyaStage.NativeUse or QingyaStage.NativeChildren) || r.Materials.Count != 0 || r.Arcs.Count != 0 || r.LivingSeats.Count != 0 || r.Targets.Count != 0 || r.Direction is not null || r.Cursor != 0 || r.OriginalUseId is not null || r.OriginalActionId is not null || r.OriginalTarget is not null)
                throw new InvalidOperationException("Qingya lost its exact original next-executed-phase entitlement.");
            if (r.SelectedAction is { } selected)
            {
                var choices = _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaNativeSelectedEvent>().Where(e => e.FrameId == f.Id).ToArray();
                if (choices is not [var chosen] || chosen.CardId != selected.CardId || chosen.Kind != selected.Kind || chosen.PlayedKind != selected.PlayedKind || chosen.TargetCardId != selected.TargetCardId || chosen.TargetCount != selected.Targets.Count || !_coreProgramEventHistoryOperations.ProgramEventHistory<QingyaNativeTargetEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(selected.Targets) || r.NativeOrigin is not { } origin || origin.ProgramFrameId != f.Id || origin.GrantId != r.GrantId || origin.Source != r.Source || origin.GameplayHash != r.GameplayHash || origin.ActualTurn != r.ActualTurn || origin.CardId != selected.CardId || !r.Entities.Any(e => e.CardId == selected.CardId) || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaNativeIssuedEvent>().Count(e => e.Origin == origin) != 1 || (r.NativeCompleted ? _coreCardUseLifecycleOperations.LifecycleCardUse(origin.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == origin.UseId) != 1 : !IsIssuedQingyaUse(origin.UseId)))
                    throw new InvalidOperationException("Qingya lost its one selected original entity, actual native issuance or typed return.");
            }
            else if (r.Stage != QingyaStage.NativeChoice || r.NativeOrigin is not null || r.NativeCompleted || r.ReservedUseId != 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<QingyaNativeSelectedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Qingya contains an unselected native payment.");
        }

        var pending = r.Stage is QingyaStage.DiscardChildren or QingyaStage.NativeChildren;
        if (pending ? f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat : f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Qingya lost its owning native movement continuation.");
    }

    internal bool YaDanOwnsActualPhaseEnd(ProgramSkillFrame f, ActualPhaseEndContext due)
    {
        var wi = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.WindowContext?.ParentFrameId);
        if (wi < 1 || _resolutionStackOperations._resolutionStack[wi] is not ProgramLifecycleTriggerWindowFrame window || window.Window != SkillProgramTriggerWindow.ActualPhaseEnded || window.ActualPhaseEnd != due || window.Continuation != ProgramLifecycleContinuation.ResumeActualPhaseEnd || window.CandidateIndex != 0 || window.Candidates is not [var candidate] || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) || _resolutionStackOperations._resolutionStack[wi - 1] is not ActualPhaseEndFrame mother || mother.Id != due.BoundaryFrameId || mother.ActiveWindowFrameId != window.Id || mother.Phase != due.Phase || !mother.ItemsCaptured || mother.ItemIndex < 0 || mother.ItemIndex >= mother.Items.Count || mother.Items[mother.ItemIndex] != new ActualPhaseEndItem(candidate, due.EntitlementId))
            return false;
        var own = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == f.Id);
        return own == wi + 1 && due.Phase.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && due.Phase.ActualTurnOwnerSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27260)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27260)]
    internal bool ContinuePaidYaDan(ProgramSkillFrame f)
    {
        if (f.YaDan is not { } r || r.Entities.Count == 0 && r.NativeOrigin is null)
            return false;
        AssertYaDan(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27261)]
    internal bool ContinueIssuedQingya(ProgramSkillFrame f)
    {
        if (f.YaDan is not null || f.InstructionIndex is not (0 or 1) || f.TriggerId != "use-next-actual-phase" || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { Window: SkillProgramTriggerWindow.ActualPhaseEnded } context || _coreProgramLifecycleOperations.GetProgramTrigger(f).Effects is not [{ Op: SkillProgramEffectOp.ResolveQingyaNextActualPhaseUse }])
            return false;
        return IsIssuedQingyaCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 290)]
    internal bool IsYaDanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.YaDan is not { Stage: QingyaStage.DiscardChildren or QingyaStage.NativeChildren } || !IsYaDanOperation(effect?.Op ?? default) || pending != f.PendingMovementContinuation)
            return false;
        AssertYaDan(f);
        return true;
    }

    internal bool YaDanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YaDan is not { } r)
            return false;
        AssertYaDan(f);
        if (child is CardUseFrame use)
            return r.Stage == QingyaStage.NativeUse && use.QingyaOrigin == r.NativeOrigin && IsIssuedQingyaUse(use.Id);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == QingyaStage.DiscardChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == QingyaDiscardReason && m.From.Zone == CardZoneKind.Hand && m.To == CardLocation.DiscardPile);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? YaDanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterYaDanOlOperations.YaDanFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 750)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 860)]
    internal bool HasYaDanDamageObserver(long id) => YaDanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 890)]
    internal bool HasYaDanBeforeDamageObserver(long id) => YaDanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 720)]
    internal bool IsYaDanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && YaDanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27260)]
    internal bool AdvanceYaDanObserver() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YaDanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
