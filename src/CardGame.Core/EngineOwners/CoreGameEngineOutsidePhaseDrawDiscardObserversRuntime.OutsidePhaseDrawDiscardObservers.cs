#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardObserversRuntime
{
    internal bool ValidOutsidePhaseDrawDiscardReceipt(ProgramSkillFrame f)
    {
        if (f.OutsidePhaseDrawDiscard is not { } r || f.WindowContext is not { MovementBatch: { } batch } context || !CoreGameEngineOutsidePhaseDrawDiscardRuntime.IsOutsidePhaseOperation(r.Op) || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Source != new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || context.ParentFrameId != r.WindowFrameId || batch.Id != r.OriginalBatchId || !Enum.IsDefined(r.Stage) || r.OriginalMovementIndex < 0 || r.OriginalMovementIndex >= batch.Movements.Count)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowFrameId);
        if (parent is null || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Batch.Id != batch.Id || !parent.Batch.Movements.SequenceEqual(batch.Movements) || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(f);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (trigger.Effects is not [{ Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always } effect] || effect.Op != r.Op || plan.Instructions.Count != 1 || plan.Instructions[0].Op != r.Op || trigger.MovementOccurrence != SkillProgramMovementOccurrence.PerBatch || trigger.Optional)
            return false;
        var draw = r.Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw;
        var candidate = parent.Candidates[parent.CandidateIndex];
        if (draw)
        {
            var indexes = _coreOutsidePhaseDrawDiscardOperations.MatchingOutsideDrawIndexes(batch, candidate);
            if (context.Window != SkillProgramTriggerWindow.CardsGained || r.OriginalDraw is not { } proof || batch.NativeDrawInvocation is not { } native || proof.InvocationId != native.InvocationId || !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof) || !proof.Materials.SequenceEqual(native.Materials) || proof.LastBatchId != r.OriginalBatchId || proof.Phase != r.OriginalPhase || indexes is not [var index] || index != r.OriginalMovementIndex || r.OriginalDiscards.Count != 0 || r.EligibleMaterials.Count != 0 || r.EligibleTargets.Count == 0 || r.EligibleTargets.Distinct().Count() != r.EligibleTargets.Count || r.EligibleTargets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)))
                return false;
        }
        else
        {
            var indexes = _coreOutsidePhaseDrawDiscardOperations.MatchingOutsideDiscardIndexes(batch, candidate);
            var expected = indexes.Select(i => new DiscardRecoveryEntity(batch.Movements[i].CardId, batch.Movements[i].Sequence, _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(batch.Movements[i])!.Value));
            if (context.Window != SkillProgramTriggerWindow.DiscardPileReceived || context.MovementIndex != r.OriginalMovementIndex || context.SourceSeat != f.OwnerSeat || context.TargetSeat != f.OwnerSeat || r.OriginalDraw is not null || batch.DiscardRecoveryPhase != r.OriginalPhase || indexes.Length == 0 || indexes[0] != r.OriginalMovementIndex || !r.OriginalDiscards.SequenceEqual(expected) || r.EligibleTargets.Count != 0 || r.EligibleMaterials.Count == 0 || r.EligibleMaterials.Select(m => m.CardId).Distinct().Count() != r.EligibleMaterials.Count || r.EligibleMaterials.Any(m => m.CardId <= 0 || !Enum.IsDefined(m.PrintedKind) || m.Slot < 0 || m.From.OwnerSeat is not { } seat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment)))
                return false;
        }

        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<OutsidePhaseDrawDiscardStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start.Source != r.Source || start.GameplayHash != r.GameplayHash || start.Op != r.Op || start.WindowFrameId != r.WindowFrameId || start.OriginalBatchId != r.OriginalBatchId || start.NativeDrawInvocationId != r.OriginalDraw?.InvocationId)
            return false;
        var issues = history.OfType<OutsidePhaseDrawDiscardMovementIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var completions = history.OfType<OutsidePhaseDrawDiscardCompletedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == OutsidePhaseDrawDiscardStage.Choosing)
            return !r.MovementIssued && r.ParticipantSeat is null && r.PaidMaterial is null && r.ActualCount == 0 && r.SequenceBefore == 0 && r.SequenceAfter == 0 && r.BatchId is null && f.PendingMovementContinuation is null && issues.Length == 0 && completions.Length == 0;
        if (!r.MovementIssued || r.ParticipantSeat is not { } participant || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(participant) || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || issues is not [var issue] || issue.Op != r.Op || issue.ParticipantSeat != participant || issue.ActualCount != r.ActualCount || issue.SequenceBefore != r.SequenceBefore || issue.SequenceAfter != r.SequenceAfter || issue.BatchId != r.BatchId || issue.PaidMaterial != r.PaidMaterial)
            return false;
        var movements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        if (draw)
        {
            if (r.PaidMaterial is not null || r.BatchId is not null || !r.EligibleTargets.Contains(participant) || r.ActualCount is < 0 or > 1 || movements.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(participant) && m.Reason.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, true)) != r.ActualCount || movements.Any(m => !(m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(participant) && m.Reason.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, true) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)))
                return false;
            var native = history.OfType<NativeDrawInvocationRecordedEvent>().Where(e => e.ParentFrameId == f.Id && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter && e.OwnerSeat == participant && e.Reason.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, true) && e.DirectProducer == r.Source).ToArray();
            if (r.ActualCount == 1 ? native is not [{ ActualCount: 1, Requested: 1 }] : native.Length != 0)
                return false;
        }
        else
        {
            if (participant == f.OwnerSeat || r.PaidMaterial is not { } material || !r.EligibleMaterials.Contains(material) || material.From.OwnerSeat != participant || r.ActualCount != 1 || r.BatchId is null || movements is not [var paid] || paid.CardId != material.CardId || paid.CardKind != material.PrintedKind || paid.From != material.From || paid.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.From, (material.IsGeneralWeapon && material.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile)) && !_coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(paid, material.From, new(CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, false))) || paid.Reason.Value != CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, false))
                return false;
        }

        return r.Stage == OutsidePhaseDrawDiscardStage.Complete ? f.PendingMovementContinuation is null && completions is [var complete] && complete.ActualCount == r.ActualCount : completions.Length == 0 && f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == participant;
    }

    internal bool OutsidePhaseDrawDiscardFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidOutsidePhaseDrawDiscardReceipt(f) || f.OutsidePhaseDrawDiscard is not { MovementIssued: true, Stage: OutsidePhaseDrawDiscardStage.MovementChildren, ParticipantSeat: { } participant } r)
            return false;
        var draw = r.Op == SkillProgramEffectOp.DrawAfterActualOutsideDraw;
        bool PaidEquipment(CardKind kind) => !draw && r.PaidMaterial is { From.Zone: CardZoneKind.Equipment } material && material.PrintedKind == kind && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == material.CardId && m.From == material.From && m.To == CardLocation.DiscardPile && m.Reason.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, false));
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && (draw ? m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(participant) && m.Reason.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, true) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle : moved.Batch.Id == r.BatchId && r.PaidMaterial is { } material && m.CardId == material.CardId && m.CardKind == material.PrintedKind && m.From == material.From && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(material.CardId), material.From, (material.IsGeneralWeapon && material.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile)) && m.Reason.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, false)) || !draw && PaidEquipment(CardKind.WoodenOx) && moved.Batch.ParentBatchId == r.BatchId && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.From == CardLocation.WoodenOxGrain(participant) && m.To == CardLocation.DiscardPile));
        if (!draw && r.PaidMaterial?.From.Zone == CardZoneKind.Equipment && child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        if (!PaidEquipment(CardKind.SilverLion))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == participant && hp.Change.TargetSeat == participant && hp.Change.Amount == 1;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == participant && recovery.Attempt.TargetSeat == participant && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason?.Value == CoreGameEngineOutsidePhaseDrawDiscardRuntime.OutsidePhaseMovementReason(f, false);
    }

    internal bool OutsidePhaseDrawDiscardStructuralEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (parent is ProgramSkillFrame f && OutsidePhaseDrawDiscardFirstChild(f, child))
            return true;
        return parent is CardsMovedTriggerWindowFrame window && child is ProgramSkillFrame { OutsidePhaseDrawDiscard: { } r } observer && r.WindowFrameId == window.Id && observer.WindowContext?.ParentFrameId == window.Id && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(observer, window.Candidates[window.CandidateIndex]) && ValidOutsidePhaseDrawDiscardReceipt(observer);
    }

    internal ProgramSkillFrame? OutsidePhaseDrawDiscardObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.OutsidePhaseDrawDiscardFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 80)]
    internal bool IsOutsidePhaseDrawDiscardDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && OutsidePhaseDrawDiscardObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 90)]
    internal bool TryAdvanceOutsidePhaseDrawDiscardSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || OutsidePhaseDrawDiscardObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
