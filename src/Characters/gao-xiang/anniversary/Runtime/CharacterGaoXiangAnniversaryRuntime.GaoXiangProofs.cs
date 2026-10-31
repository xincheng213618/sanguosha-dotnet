#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGaoXiangAnniversaryRuntime
{
    internal bool ValidGaoXiang(ProgramSkillFrame f)
    {
        if (f.GaoXiangChiying is not { } r || f.SkillId != GaoXiangComposition.Skill || f.ActivationId != GaoXiangComposition.Activation || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != GaoXiangSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.TurnOwner != f.OwnerSeat || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Index < 0 || r.Index > r.Victims.Count || r.Victims.Distinct().Count() != r.Victims.Count || r.Victims.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == r.Actor) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GaoXiangChiyingStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new GaoXiangChiyingStartedEvent(f.Id, r.Source, r.Hash, r.Turn, r.TurnOwner))
            return false;
        var actors = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GaoXiangChiyingActorEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var plans = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GaoXiangChiyingCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var victims = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GaoXiangChiyingVictimEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Ordinal).ToArray();
        var moves = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GaoXiangChiyingMovedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage is GaoXiangChiyingStage.HealthQueries or GaoXiangChiyingStage.Actor)
            return r.Actor == -1 && !r.Committed && r.Index == 0 && r.Victims.Count == 0 && r.Discards.Count == 0 && r.Gains.Count == 0 && actors.Length == 0 && plans.Length == 0 && victims.Length == 0 && moves.Length == 0;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Actor) || r.ActorHp > r.OwnerHp || actors is not [var actor] || actor != new GaoXiangChiyingActorEvent(f.Id, r.Actor, r.OwnerHp, r.ActorHp))
            return false;
        if (!r.Committed)
            return r.Stage == GaoXiangChiyingStage.Victims && r.Index == 0 && r.Discards.Count == 0 && r.Gains.Count == 0 && plans.Length == 0 && victims.Length == 0 && moves.Length == 0;
        if (plans is not [var plan] || plan != new GaoXiangChiyingCommittedEvent(f.Id, r.Actor, r.Victims.Count) || victims.Length != r.Victims.Count || victims.Where((e, index) => e != new GaoXiangChiyingVictimEvent(f.Id, index, r.Actor, r.Victims[index])).Any() || r.Discards.Select(m => m.Ordinal).Distinct().Count() != r.Discards.Count || r.Discards.Select(m => m.CardId).Distinct().Count() != r.Discards.Count || r.Gains.Select(m => m.CardId).Distinct().Count() != r.Gains.Count || moves.Length != r.Discards.Count + r.Gains.Count || r.Discards.Any(m => m.Ordinal < 0 || m.Ordinal >= r.Victims.Count || m.Ordinal > r.Index || m.Victim != r.Victims[m.Ordinal] || m.From.OwnerSeat != m.Victim || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !GaoXiangMoveExists(f, m, false)) || r.Gains.Any(m => !r.Discards.Any(d => d.Ordinal == m.Ordinal && d.Victim == m.Victim && d.CardId == m.CardId && d.PrintedKind == m.PrintedKind && d.To == CardLocation.DiscardPile) || r.Actor == f.OwnerSeat || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(m.PrintedKind) != SkillProgramCardCategory.Basic || m.From != CardLocation.DiscardPile || !GaoXiangMoveExists(f, m, true)))
            return false;
        if (r.Stage == GaoXiangChiyingStage.Card)
            return r.Gains.Count == 0 && r.Discards.All(m => m.Ordinal < r.Index);
        if (r.Stage == GaoXiangChiyingStage.DiscardChildren)
            return r.Index < r.Victims.Count && r.Gains.Count == 0 && r.Discards.Count(m => m.Ordinal == r.Index && m.Sequence > r.Before && m.Sequence <= r.After) == 1;
        return r.Stage == GaoXiangChiyingStage.GainChildren && r.Index == r.Victims.Count && r.Gains.Count > 0 && r.Gains.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal bool GaoXiangMoveExists(ProgramSkillFrame f, GaoXiangChiyingMove receipt, bool gain)
    {
        var r = f.GaoXiangChiying!;
        if (receipt.Sequence <= 0 || receipt.BatchId <= 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GaoXiangChiyingMovedEvent>().Count(e => e == GaoXiangMovedEvent(f.Id, gain, receipt)) != 1)
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == receipt.Sequence && m.CardId == receipt.CardId && m.CardKind == receipt.PrintedKind && m.From == receipt.From && m.To == receipt.To && m.Reason == (gain ? GaoXiangGainReason : GaoXiangDiscardReason) && (gain ? m.To == CardLocation.Hand(r.Actor) || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(r.Actor)) : m.To == CardLocation.DiscardPile || _coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(m, receipt.From, GaoXiangDiscardReason) || m.To == CardLocation.OutsideGame && (_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial2(m.CardId, m.From, m.To) || _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).IsGeneralWeapon))) == 1;
    }

    internal void RequireGaoXiang(ProgramSkillFrame f)
    {
        if (!ValidGaoXiang(f))
            throw new InvalidOperationException("Chiying lost its original skill/frame, frozen actor/participant queue or actual once-paid discard/gain receipt.");
    }
    internal bool GaoXiangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.GaoXiangChiying is not { Stage: GaoXiangChiyingStage.DiscardChildren or GaoXiangChiyingStage.GainChildren } r || !ValidGaoXiang(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (r.Stage != GaoXiangChiyingStage.DiscardChildren || !r.Discards.Any(m => m.Ordinal == r.Index && m.PrintedKind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment))
            return false;
        var target = r.Victims[r.Index];
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.AwaitedProgramMovement, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == GaoXiangDiscardReason && recovery.Attempt.TargetSeat == target && recovery.Attempt.SourceSeat == target && recovery.Attempt.Amount == 1;
        return child is HpChangedTriggerWindowFrame hp && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == target && hp.Change.TargetSeat == target && hp.Change.Amount == 1;
    }

    internal ProgramSkillFrame? GaoXiangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.GaoXiangFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 44900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 44900)]
    internal bool GaoXiangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => GaoXiangFirstChild(f, recovery);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44900)]
    internal bool AdvanceGaoXiangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && GaoXiangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
