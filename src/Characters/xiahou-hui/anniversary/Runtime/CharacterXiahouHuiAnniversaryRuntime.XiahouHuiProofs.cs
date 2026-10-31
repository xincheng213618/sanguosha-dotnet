#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouHuiAnniversaryRuntime
{
    internal bool ValidXiahouHui(ProgramSkillFrame f)
    {
        if (f.XiahouHui is not { } r || f.TriggerId is null || f.ActivationId != f.TriggerId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } c || c.ParentFrameId != r.ParentId || c.OwnerSeat != f.OwnerSeat || r.Source != XiahouHuiSource(f) || r.Hash != f.GameplayHash || !_contentRegistryOperations._contentRegistry.Skills.TryGetValue(f.SkillId, out var definition) || definition.Program?.GameplayHash != r.Hash || r.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Drawn.Count > 2 || r.Drawn.Distinct().Count() != r.Drawn.Count || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.BindingId == f.TriggerId && e.SkillInstanceId == f.SkillInstanceId && e.Window == c.Window) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new XiahouHuiStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.Operation, r.ThresholdSequence, r.Drawn.Count))
            return false;
        var gift = r.Operation == SkillProgramEffectOp.GiveAnniversaryXiahouHuiDujunDrawnCards;
        if (gift)
        {
            if (f.InstructionIndex != 2 || r.ThresholdSequence != 0 || !XiahouHuiDamageCandidate(XiahouHuiCandidate(f), c, true) || f.CardSetBindings.SingleOrDefault(b => b.Name == XiahouHuiComposition.Drawn)is not { } bound || !bound.CardIds.SequenceEqual(r.Drawn) || bound.SourceLocations.Any(l => l != CardLocation.Hand(f.OwnerSeat)))
                return false;
            if (r.Drawn.Count > 0 || r.DrawInvocationId != 0)
            {
                if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(e => e.InvocationId == r.DrawInvocationId).ToArray()is not [var draw] || draw.ParentFrameId != f.Id || draw.OwnerSeat != f.OwnerSeat || draw.Requested != 2 || draw.ActualCount != r.Drawn.Count || draw.Reason != new CardMoveReason($"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.Draw)}") || r.Drawn.Any(id => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == id && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason == draw.Reason)))
                    return false;
            }
        }
        else if (r.Operation != SkillProgramEffectOp.ResolveAnniversaryXiahouHuiJikun || f.InstructionIndex != 1 || r.Drawn.Count != 0 || r.DrawInvocationId != 0 || XiahouHuiLossCandidate(XiahouHuiCandidate(f), c)?.Sequence != r.ThresholdSequence)
            return false;
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var paid = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiahouHuiMovedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == XiahouHuiStage.Recipient)
            return r.Recipient == -1 && r.Donor == -1 && r.Before == 0 && r.After == 0 && r.BatchId == 0 && r.Candidates.Count == 0 && r.Moves.Count == 0 && issued.Length == 0 && paid.Length == 0 && f.PendingMovementContinuation is null;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Recipient) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Donor) || gift && r.Recipient == r.Donor || !gift && r.Recipient == f.OwnerSeat || gift && r.Donor != f.OwnerSeat || r.BatchId <= 0 || r.Candidates.Count == 0 || r.Candidates.Select(x => x.CardId).Distinct().Count() != r.Candidates.Count || r.Candidates.Any(x => x.From.OwnerSeat != r.Donor || x.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || x.From == CardLocation.Hand(r.Recipient) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == x.CardId && m.Sequence <= r.Before)?.To != x.From) || issued is not [var commitment] || commitment != new XiahouHuiCommittedEvent(f.Id, r.Recipient, r.Donor, gift ? r.Candidates.Count : 1, r.Candidates.Count, r.Before) || paid is not [var invoice] || invoice != new XiahouHuiMovedEvent(f.Id, r.Recipient, r.Donor, r.Moves.Count, r.BatchId, r.Before, r.After) || r.Moves.Select(m => m.CardId).Distinct().Count() != r.Moves.Count || r.Moves.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After || !r.Candidates.Contains(new(m.CardId, m.From)) || m.Reason != (gift ? XiahouHuiGiveReason : XiahouHuiObtainReason) || m.To != CardLocation.Hand(r.Recipient) && !_coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(r.Recipient))))
            return false;
        if (gift)
            return r.RandomBefore == r.RandomAfter && r.Candidates.All(x => r.Drawn.Contains(x.CardId)) && r.Moves.Count == r.Candidates.Count;
        if (r.Moves is not [var moved] || r.RandomBefore == 0)
            return false;
        var random = new DeterministicRandom(unchecked((int)r.RandomBefore));
        var selected = r.Candidates[random.Next(r.Candidates.Count)];
        return random.State == r.RandomAfter && selected.CardId == moved.CardId && selected.From == moved.From;
    }
    internal bool XiahouHuiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.XiahouHui is not { Stage: XiahouHuiStage.MovementChildren } r || !ValidXiahouHui(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => r.Moves.Contains(m));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (!r.Moves.Any(m => m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Donor)))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.AwaitedProgramMovement, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == XiahouHuiObtainReason && recovery.Attempt.SourceSeat == r.Donor && recovery.Attempt.TargetSeat == r.Donor && recovery.Attempt.Amount == 1;
        return child is HpChangedTriggerWindowFrame hp && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.Donor && hp.Change.TargetSeat == r.Donor && hp.Change.Amount == 1;
    }

    internal ProgramSkillFrame? XiahouHuiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.XiahouHuiFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 46200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 46200)]
    internal bool XiahouHuiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => XiahouHuiFirstChild(f, recovery);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46200)]
    internal bool AdvanceXiahouHuiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XiahouHuiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
