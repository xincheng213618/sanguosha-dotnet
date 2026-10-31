#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class MouZhangHeNative(KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreGameEngineSkillProgramsEntries> getCoreGameEngineSkillProgramsRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CoreGameEngineCharacterStateProgramsEntries> getCoreGameEngineCharacterStateProgramsRuntime, Func<RuntimeEntries.CoreGameEngineSkillOwnershipWindowsEntries> getCoreGameEngineSkillOwnershipWindowsRuntime, Func<RuntimeEntries.CoreGameEngineHpChangeProgramsEntries> getCoreGameEngineHpChangeProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCardMovementProgramsEntries> getCoreGameEngineCardMovementProgramsRuntime, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CharacterZhangHeMouMobileEntries> getCharacterZhangHeMouMobileRuntime, EngineStates.CoreCardMovementJournalState cardMovementJournalState, Func<RuntimeEntries.CoreGameEngineRuntimeEntries> getCoreGameEngineRuntimeRuntime, KernelAdvanceEventRulesAndQueueFactCall advanceEventRulesAndQueueFact, Func<RuntimeEntries.CoreGameEngineNativeLogicalDiscardsEntries> getCoreGameEngineNativeLogicalDiscardsRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, Func<CardZoneStore> getCardZoneStore, MouZhangHeFactsFactory createMouZhangHeFacts, Func<RuntimeEntries.CoreGameEngineProgramCardPaymentsEntries> getCoreGameEngineProgramCardPaymentsRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineProgramEventHistoryEntries> getCoreGameEngineProgramEventHistoryRuntime, EngineStates.CoreDelayedTurnEffectsState delayedTurnEffectsState, Func<RuntimeEntries.CorePendingDecisionPolicyPipelineEntries> getCorePendingDecisionPolicyPipelineRuntime, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<RuntimeEntries.CoreResolutionIdentityBoundaryEntries> getCoreResolutionIdentityBoundaryRuntime, Func<RuntimeEntries.CoreGameEngineProgramLifecycleEntries> getCoreGameEngineProgramLifecycleRuntime, Func<RuntimeEntries.CoreActualDiscardPhaseEntries> getCoreActualDiscardPhaseRuntime, Func<RuntimeEntries.CoreActualTurnsEntries> getCoreActualTurnsRuntime) : IMouZhangHeNative
{
    public void PublishState() => advanceRulesAndPublishState();
    public void Finish(ProgramSkillFrame f) => getCoreGameEngineSkillProgramsRuntime().FinishProgramSkill(f, true);
    public bool Drain(ProgramSkillFrame f) => getCoreRecoveryReplacementRuntime().TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCharacterStateProgramsRuntime().TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || getCoreGameEngineSkillOwnershipWindowsRuntime().TryBeginAdvancedSkillsChanged(f.Id) || getCoreGameEngineHpChangeProgramsRuntime().TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || getCoreGameEngineCardMovementProgramsRuntime().TryBeginCardsMovedProgramWindow(f.Id);
    public bool AdvanceObserverChildren() => getCoreGameEngineObserverSubtreesRuntime().TryAdvanceObserverSubtree(true);
    private void Seal(ProgramSkillFrame f, CardMovementRecord m)
    {
        var parent = getFrameStore().OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
        if (parent?.MouZhangHe is not { } r || !getCharacterZhangHeMouMobileRuntime().MouQiaobian.Valid(parent) || m.Sequence <= r.Before || !cardMovementJournalState.ReferenceToCardMovements.Contains(m))
            throw new InvalidOperationException("Qiaobian's native movement lost its owning ancestor and exact payment interval.");
        getCoreGameEngineRuntimeRuntime().ReplaceRuntimeFrame(f.Id, parent = parent with { MouZhangHe = r = r with { After = m.Sequence, PaidCount = r.PaidCount + (r.Stage == MouZhangHeStage.CostChildren && m.CardId == r.CardId && m.From == r.From && m.Reason.Value == MouQiaobianRuntime.CostReason ? 1 : 0) } });
        if (r.Stage == MouZhangHeStage.CostChildren && m.CardId == r.CardId && m.From == r.From && m.Reason.Value == MouQiaobianRuntime.CostReason)
            advanceEventRulesAndQueueFact(new MouZhangHeCostPaidEvent(f.Id, m.CardId, m.From, m.Sequence));
        if (r.Stage == MouZhangHeStage.ObtainChildren && m.CardId == r.CardId && m.From == r.From && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouQiaobianRuntime.ObtainReason)
            advanceEventRulesAndQueueFact(new MouZhangHeObtainedEvent(f.Id, f.OwnerSeat, m.Sequence));
        if (r.Stage == MouZhangHeStage.FieldChildren && m.CardId == r.CardId && m.From == r.From && m.To.OwnerSeat == r.TargetSeat && m.Reason.Value == MouQiaobianRuntime.FieldReason)
            advanceEventRulesAndQueueFact(new MouZhangHeFieldMovedEvent(f.Id, m.CardId, m.From.OwnerSeat!.Value, r.TargetSeat, m.Sequence));
    }

    public void Discard(ProgramSkillFrame f, MouZhangHeCard card) => getCoreGameEngineNativeLogicalDiscardsRuntime().MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(card.CardId), card.From, new(MouQiaobianRuntime.CostReason), m => Seal(f, m), f.Id);
    public void Obtain(ProgramSkillFrame f, int target, int id) => getCorePhysicalMovementFacadesRuntime().MoveCard(getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id), CardLocation.Hand(target), CardLocation.Hand(f.OwnerSeat), new(MouQiaobianRuntime.ObtainReason), m => Seal(f, m), disclosure: CardMovementDisclosure.FaceDown);
    public void MoveField(ProgramSkillFrame f, int id, int target)
    {
        var card = getCoreGameEngineAdvancedLifecycleRuntime().GetAdvancedCard(id);
        var from = getCardZoneStore().GetLocation(id);
        if (f.MouZhangHe?.From != from || !createMouZhangHeFacts().CanMoveField(id, target))
            throw new InvalidOperationException("Qiaobian's corresponding-zone move is no longer native legal.");
        var destination = getCoreGameEngineProgramCardPaymentsRuntime().ProgramCorrespondingZoneLocation(card, from.Zone, target);
        if (destination.Zone == CardZoneKind.Equipment)
        {
            var slot = getEquipmentContentRegistry().Get(card.Kind).Slot;
            var occupants = getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[target]).Where(c => getEquipmentContentRegistry().Get(c.Kind).Slot == slot).ToArray();
            if (occupants.Length >= matchParticipantsState.ReferenceToPlayers[target].EquipmentSlotCapacity(slot))
                getCorePhysicalMovementFacadesRuntime().MoveCard(occupants[0], CardLocation.Equipment(target), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace, m => Seal(f, m), disclosure: CardMovementDisclosure.FaceUp);
        }

        getCorePhysicalMovementFacadesRuntime().MoveCard(card, from, destination, new(MouQiaobianRuntime.FieldReason), m => Seal(f, m), disclosure: CardMovementDisclosure.FaceUp);
    }

    public void ApplySkips(ProgramSkillFrame f, IReadOnlyList<SkillProgramTurnPhase> phases)
    {
        if (!getCharacterZhangHeMouMobileRuntime().MouQiaobian.Valid(f) || f.MouZhangHe is not { Operation: SkillProgramEffectOp.PrepareMouZhangHeQiaobian, Accepted: true } r || r.PaidCount != phases.Count || !phases.SequenceEqual(r.Phases.Take(r.PaidCount)) || getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouZhangHeCostPaidEvent>().Count(x => x.FrameId == f.Id) != r.PaidCount)
            throw new InvalidOperationException("Only Qiaobian's actually paid distinct-area costs may issue phase skips.");
        foreach (var phase in phases)
            delayedTurnEffectsState.ReferenceToPendingTurnDelayedEffects |= phase switch
            {
                SkillProgramTurnPhase.Judgment => GameEngine.DelayedTurnEffects.SkipJudgmentPhase,
                SkillProgramTurnPhase.Draw => GameEngine.DelayedTurnEffects.SkipDrawPhase,
                SkillProgramTurnPhase.Play => GameEngine.DelayedTurnEffects.SkipPlayPhase,
                SkillProgramTurnPhase.Discard => GameEngine.DelayedTurnEffects.SkipDiscardPhase,
                _ => throw new InvalidOperationException("Qiaobian cannot skip Preparation or Ending.")};
    }

    public void OpenSkipWindow(MouZhangHeSkipEntitlement q, ProgramActualPhaseSkipContext c)
    {
        if (getFrameStore().Count != 0 || getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is not null || c.OwnerSeat != actualTurnProgressState.ReferenceToCurrentSeat || c.ActualTurn != actualTurnProgressState.ReferenceToTurnNumber || q.ActualTurn != c.ActualTurn || q.Source.OwnerSeat != c.OwnerSeat || q.Phase != c.Phase || !getCoreGameEngineProgramEventHistoryRuntime().ProgramEventHistory<MouZhangHeSkipIssuedEvent>().Any(x => x.Entitlement == q))
            throw new InvalidOperationException("Qiaobian can suspend only its real clean skipped-phase continuation.");
        var candidate = new ProgramTriggerCandidate(c.OwnerSeat, q.Source.SkillId, "actual-skipped-phase", q.Source.SkillInstanceId!, q.GameplayHash, 0);
        var window = new ProgramLifecycleTriggerWindowFrame(++getCoreResolutionIdentityBoundaryRuntime()._resolutionSequence, c.OwnerSeat, SkillProgramTriggerWindow.ActualPhaseSkipped, Array.AsReadOnly(new[] { candidate }), ProgramLifecycleContinuation.CompleteActualSkippedPhase, getCoreGameEngineProgramLifecycleRuntime().CaptureProgramTriggerFacts(matchParticipantsState.ReferenceToPlayers[c.OwnerSeat]))
        {
            MouZhangHeSkip = new(q, c)
        };
        getCoreGameEngineRuntimeRuntime().PushRuntimeFrame(window);
        advanceEventRulesAndQueueFact(new MouZhangHeSkipObservedEvent(window.Id, q, c));
        getCoreGameEngineRuntimeRuntime().AdvanceRuntimeFrame(window.Id);
    }

    public void ResumeSkip(ProgramLifecycleTriggerWindowFrame w)
    {
        var c = w.MouZhangHeSkip!.NativeReturn;
        if (getFrameStore().Count != 0 || getCorePendingDecisionPolicyPipelineRuntime()._pendingDecision is not null || w.OwnerSeat != actualTurnProgressState.ReferenceToCurrentSeat || c.ActualTurn != actualTurnProgressState.ReferenceToTurnNumber)
            throw new InvalidOperationException("Qiaobian's skipped-phase reward lost its native return cursor.");
        if (c.Phase == SkillProgramTurnPhase.Discard)
            getCoreActualDiscardPhaseRuntime().CompleteDiscardPhaseAfterProgramWindow(matchParticipantsState.ReferenceToPlayers[c.OwnerSeat]);
        else
            getCoreActualTurnsRuntime().CompleteTurnStartAfterDraw(matchParticipantsState.ReferenceToPlayers[c.OwnerSeat], (GameEngine.DelayedTurnEffects)c.DelayedEffects, c.AfterNormalDrawProgramsCompleted, c.DrawPhaseEndedProgramsCompleted, c.ActualDrawCompletionCompleted);
    }

    public void AdvanceLifecycleCursor(ProgramLifecycleTriggerWindowFrame w) => getCoreGameEngineProgramLifecycleRuntime().AdvanceProgramLifecycleCursor(w);
    public bool BeginExtraPlay(ProgramSkillFrame f) => getCharacterZhangHeMouMobileRuntime().BeginMouQiaobianExtraPlay(f);
    public void ResumeExtraPlay(ProgramPhaseSchedule s, bool cancel) => getCharacterZhangHeMouMobileRuntime().ReturnMouQiaobianExtraPlay(s, cancel);
}
