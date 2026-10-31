#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuZhangOlRuntime
{
    internal const string FengweiDrawReason = "skill-program.liu-zhang.fengwei.draw";
    // Registration only; the query still belongs to legacy GameEngine.Fengwei.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 90)]
    internal void ProjectFengweiCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        // Read between Dongxin and Jigu, but append after every ordinary label.
        projection.AppendLast(HasFengweiCapability && _coreProgramEventHistoryOperations.ProgramEventHistory<FengweiAcquiredEvent>().Any(grant => grant.CardId == cardId && grant.Round == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber && _cardZonesOperations._cardZones.Contains(CardLocation.Hand(grant.Source.OwnerSeat), cardId)) ? "丰蔚" : null);
    }

    internal bool HasFengweiCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveFengweiRoundStartDraw);

    internal static CardConversionSource FengweiSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_liu_zhang_ol_Fengwei.FengweiSource(f);
    internal bool FengweiLive(CharacterState p) => p.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.Any(i => i.SkillId == "ol:fengwei" && i.Program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveFengweiRoundStartDraw)));
    internal int? FengweiMarkedHand(int seat) => _coreProgramEventHistoryOperations.ProgramEventHistory<FengweiAcquiredEvent>().Where(e => e.Source.OwnerSeat == seat && e.Round == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber && _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.Hand(seat)).Select(e => (int? )e.CardId).FirstOrDefault();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 27510, SkillProgramEffectOp.ResolveFengweiRoundStartDraw, SkillProgramEffectOp.ResolveFengweiActualDrawSkip)]
    internal bool QualifyFengwei(ProgramTriggerLegalityContext c)
    {
        if (c.SkillId != "ol:fengwei" || c.WindowContext is not { } w || !c.Owner.IsAlive)
            return false;
        if (c.Trigger.Id == "draw-skip")
            return w.Window == SkillProgramTriggerWindow.DrawPhaseStarting && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat && FengweiMarkedHand(c.OwnerSeat)is not null;
        return c.Trigger.Id == "round-draw" && w.Window == SkillProgramTriggerWindow.RoundStarting && _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Count == 1 && _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra && _coreProgramEventHistoryOperations.ProgramEventHistory<RoundStartedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat) && !_coreProgramEventHistoryOperations.ProgramEventHistory<FengweiStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Round == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber);
    }

    internal SkillProgramStepOutcome ResolveFengwei(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Fengwei is not null || f.InstructionIndex != 1 || f.SkillId != "ol:fengwei" || f.WindowContext is not { } c || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame w || w.Id != c.ParentFrameId || w.Window != c.Window)
            throw new InvalidOperationException("Fengwei requires its actual round/draw lifecycle parent.");
        var source = FengweiSource(f);
        if (op == SkillProgramEffectOp.ResolveFengweiActualDrawSkip)
        {
            if (c.Window != SkillProgramTriggerWindow.DrawPhaseStarting || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._currentSeat || FengweiMarkedHand(f.OwnerSeat)is not { } id)
                throw new InvalidOperationException("Fengwei skip requires a currently held original acquisition.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiDrawSkippedEvent(f.Id, source, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, w.Id, id));
            _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { ActualPhaseSubstitution = new(w.Id, f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, SkillProgramTurnPhase.Draw) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(f.Id, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.OwnerSeat, _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber, c.Window, SkillProgramTurnPhase.Draw));
            return _coreProgramTurnPhaseOperations.SkipProgramTurnPhases(f, [SkillProgramTurnPhase.Draw]);
        }

        if (c.Window != SkillProgramTriggerWindow.RoundStarting || _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Count != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<FengweiStartedEvent>().Any(e => e.Source.OwnerSeat == f.OwnerSeat && e.Round == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber))
            throw new InvalidOperationException("Fengwei may start once per genuine round.");
        var r = new FengweiReceipt(source, f.GameplayHash, w.Id, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, FengweiStage.Choice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fengwei = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiStartedEvent(f.Id, source, f.GameplayHash, w.Id, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber));
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        PublishFengwei(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> FengweiChoices(ProgramSkillFrame f) => Enumerable.Range(1, 5).Select(n => new PromptChoice(new($"fengwei.{(f.Id)}.{(n)}"), $"摸{(n)}张牌", [], [], new Dictionary<string, string> { { "program-action", "liu-zhang-fengwei" }, { "count", n.ToString(CultureInfo.InvariantCulture) } })).ToArray();
    internal void PublishFengwei(ProgramSkillFrame f)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishFengwei(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = FengweiChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "丰蔚：选择摸一至五张牌。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("liu-zhang-fengwei")]
    internal void AnswerFengwei(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Fengwei: { Stage: FengweiStage.Choice } r } f || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(FengweiChoices(f).Where(c => c.Id == choice.Id).ToArray(), [choice]))
            throw new InvalidOperationException("Fengwei requires its actual positive round draw selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var count = int.Parse(choice.Parameters["count"], CultureInfo.InvariantCulture);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, Fengwei = r with { Stage = FengweiStage.DrawChildren, Requested = count, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], count, true, new(FengweiDrawReason), r.Source);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fengwei = f.Fengwei!with { Actual = drawn.Count, After = after } });
        foreach (var id in drawn)
        {
            var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.Sequence <= after && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == FengweiDrawReason);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiAcquiredEvent(f.Id, r.Source, r.Round, id, m.Sequence));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FengweiDrawCompletedEvent(f.Id, r.Source, r.Round, count, drawn.Count, before, after));
        ContinueFengwei(f.Id);
    }

    internal void ContinueFengwei(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Fengwei: { } r } f || f.Id != id)
            return;
        if (r.Stage == FengweiStage.Choice)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishFengwei(f);
            return;
        }

        if (DrainZonghu(f))
            return;
        FinishFengwei(f);
    }

    internal void FinishFengwei(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Fengwei = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27510)]
    internal bool ResumeFengwei(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Fengwei: not null } f || f.Id != id)
            return false;
        ContinueFengwei(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27510)]
    internal bool ReturnFengweiMovement(ProgramSkillFrame f)
    {
        if (f.Fengwei is null)
            return false;
        ContinueFengwei(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27510)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27510)]
    internal bool ContinueIssuedFengwei(ProgramSkillFrame f)
    {
        if (f.Fengwei is null)
            return false;
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveFengweiRoundStartDraw)]
    internal PromptChoice AiFengwei(PendingDecision p, ProgramSkillFrame f) => p.Choices.Last();

    internal bool IsFengweiMovement(ProgramSkillFrame f, CardMovementRecord m) => f.Fengwei is { Stage: FengweiStage.DrawChildren } r && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == FengweiDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    internal bool FengweiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Fengwei is not { Stage: FengweiStage.DrawChildren })
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsFengweiMovement(f, m));
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 190)]
    internal bool IsLiuZhangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Zonghu is { Stage: ZonghuStage.CostChildren, Paid: { } p } && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && p.Gifts.Any(g => g.Kind == CardKind.SilverLion && g.From == CardLocation.Equipment(f.OwnerSeat)) && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == ZonghuGiftReason;
}
