#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuHuanBoundaryMobileRuntime
{
    internal const string BoundaryPingkouAction = "boundary-zhu-huan-pingkou", BoundaryPingkouGain = "skill.boundary-zhu-huan.pingkou.equipment";
    internal bool TracksBoundaryZhuHuanPhaseSkips => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileZhuHuanPingkou);

    internal static CardConversionSource BoundaryZhuHuanSource(ProgramSkillFrame frame) => new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    SkillProgramStepOutcome IBoundaryZhuHuanHost.Resolve(ProgramSkillFrame frame) => BeginBoundaryPingkou(frame);
    void IBoundaryZhuHuanHost.SkipDraw(ProgramSkillFrame frame) => SkipBoundaryZhuHuanDraw(frame);
    internal void SkipBoundaryZhuHuanDraw(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryZhuHuanComposition.Fenli || frame.TriggerId != "skip-draw-most-hand" || frame.InstructionIndex != 1 || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^1].Id != frame.Id || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.OwnerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Fenli Draw substitution requires its exact original owner starting window.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, frame.Id, frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, SkillProgramTurnPhase.Draw) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(frame.Id, frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.SkillInstanceId, frame.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, context.Window, SkillProgramTurnPhase.Draw));
    }

    internal void RecordBoundaryZhuHuanSkippedPhase(int owner, SkillProgramTurnPhase phase, long extraDrawFrameId = 0)
    {
        if (!TracksBoundaryZhuHuanPhaseSkips)
            return;
        if (owner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || extraDrawFrameId < 0 || extraDrawFrameId != 0 && phase != SkillProgramTurnPhase.Draw)
            throw new InvalidOperationException("Pingkou phase history requires a genuine current actual phase.");
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanPhaseSkippedEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Phase == phase && e.ExtraDrawFrameId == extraDrawFrameId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanPhaseSkippedEvent(owner, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, phase, extraDrawFrameId));
    }

    // Observation precedes any hook that opens a child; re-entry deduplicates the same real occurrence.
    [ProgramPipelineHook(ProgramPipelinePhase.ActualPhaseSkipped, 538)]
    internal bool ObserveBoundaryZhuHuanSkippedPhase(ProgramActualPhaseSkipContext context)
    {
        if (TracksBoundaryZhuHuanPhaseSkips)
        {
            if (context.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
                throw new InvalidOperationException("Pingkou cannot count another actual turn's phase.");
            RecordBoundaryZhuHuanSkippedPhase(context.OwnerSeat, context.Phase);
        }

        return false;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 53800)]
    internal void ObserveBoundaryZhuHuanSkippedDraw(IGameEvent payload)
    {
        if (!TracksBoundaryZhuHuanPhaseSkips || payload is not DrawPhaseSkippedEvent e)
            return;
        if (e.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || e.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<DrawPhaseObligationFrame>().SingleOrDefault(f => f.Id == e.FrameId)is not { Skipped: true } native || native.IsExtra != e.IsExtra || native.OwnerSeat != e.OwnerSeat)
            throw new InvalidOperationException("Pingkou requires the original skipped native Draw occurrence.");
        RecordBoundaryZhuHuanSkippedPhase(e.OwnerSeat, SkillProgramTurnPhase.Draw, e.IsExtra ? e.FrameId : 0);
    }

    internal int BoundaryPingkouMaximum(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanPhaseSkippedEvent>().Count(e => e.OwnerSeat == owner && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 53800)]
    internal bool CanOfferBoundaryZhuHuan(ProgramTriggerLegalityContext gate)
    {
        var owner = gate.Owner;
        if (gate.SkillId == BoundaryZhuHuanComposition.Fenli)
            return owner.Seat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && (gate.Candidate.BindingId switch
            {
                "skip-draw-most-hand" => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count),
                "skip-play-most-hp" => owner.HasHpValue && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.HasHpValue).All(p => p.Hp <= owner.Hp),
                "skip-discard-most-equipment" => _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).All(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count),
                _ => false
            });
        return gate.SkillId != BoundaryZhuHuanComposition.Pingkou || owner.Seat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && BoundaryPingkouMaximum(owner.Seat) > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != owner.Seat);
    }

    internal SkillProgramStepOutcome BeginBoundaryPingkou(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.BoundaryZhuHuan is not null || frame.SkillId != BoundaryZhuHuanComposition.Pingkou || frame.InstructionIndex != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().Any(p => p.Id == context.ParentFrameId && p.OwnerSeat == frame.OwnerSeat && p.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) || BoundaryPingkouMaximum(frame.OwnerSeat) < 1)
            throw new InvalidOperationException("Pingkou requires its original actually ended turn and real skipped phases.");
        var receipt = new BoundaryZhuHuanReceipt(BoundaryZhuHuanSource(frame), frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, context.ParentFrameId, BoundaryPingkouMaximum(frame.OwnerSeat), BoundaryZhuHuanStage.Targets);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanStartedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.ActualTurn, receipt.EndedFrame, receipt.Maximum));
        PublishBoundaryPingkou(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> BoundaryPingkouChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhuHuan!;
        var choices = new List<PromptChoice>();
        if (receipt.Stage != BoundaryZhuHuanStage.Targets)
            return[];
        if (receipt.Targets.Count < receipt.Maximum)
            choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat && !receipt.Targets.Contains(p.Seat)).Select(p => new PromptChoice(new($"boundary-pingkou.{(frame.Id)}.target.{(p.Seat)}"), "选择 " + p.Name, [], [p.Seat], new Dictionary<string, string> { { "program-action", BoundaryPingkouAction }, { "branch", "target" } })));
        choices.Add(new(new($"boundary-pingkou.{(frame.Id)}.finish"), $"依次造成伤害（已选{(receipt.Targets.Count)}/{(receipt.Maximum)}）", [], [], new Dictionary<string, string> { { "program-action", BoundaryPingkouAction }, { "branch", "finish" } }));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishBoundaryPingkou(ProgramSkillFrame frame)
    {
        var choices = BoundaryPingkouChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"平寇：本回合实际跳过{(frame.BoundaryZhuHuan!.Maximum)}个阶段，选择至多该数量的其他角色。", [], Array.AsReadOnly(choices.SelectMany(c => c.Targets).Distinct().ToArray()), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryPingkouAction)]
    internal void AnswerBoundaryPingkou(PromptChoice answer)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhuHuan: { Stage: BoundaryZhuHuanStage.Targets } receipt } frame || !ValidBoundaryPingkou(frame) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([answer], BoundaryPingkouChoices(frame).Where(c => c.Id == answer.Id).ToArray()))
            throw new InvalidOperationException("Pingkou requires the same owner and fresh distinct living targets.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        if (answer.Parameters["branch"] == "target")
        {
            var target = answer.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = receipt = receipt with { Targets = receipt.Targets.Append(target).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanTargetEvent(frame.Id, receipt.Targets.Count - 1, target));
            PublishBoundaryPingkou(frame);
            return;
        }

        if (receipt.Targets.Count == 0)
        {
            CompleteBoundaryPingkou(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = receipt with { Targets = receipt.Targets.OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, s)).ToArray(), Sealed = true, Stage = BoundaryZhuHuanStage.Damage } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanSealedEvent(frame.Id, receipt.Targets.Count));
        IssueBoundaryPingkouDamage(frame);
    }

    internal void IssueBoundaryPingkouDamage(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhuHuan!;
        while (receipt.Index < receipt.Targets.Count && !_coreMatchParticipantsPlayersAccess._players[receipt.Targets[receipt.Index]].IsAlive)
            receipt = receipt with
            {
                Index = receipt.Index + 1
            };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = receipt });
        if (receipt.Index >= receipt.Targets.Count)
        {
            ObtainBoundaryPingkouEquipment(frame);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanDamageIssuedEvent(frame.Id, receipt.Index, receipt.Targets[receipt.Index]));
        _coreSkillProgramsOperations.BeginProgramSkillDamage(frame, receipt.Targets[receipt.Index], 1);
    }

    internal void ObtainBoundaryPingkouEquipment(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhuHuan!;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = receipt with { Stage = BoundaryZhuHuanStage.Equipment } });
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_corePhysicalDrawFacadesOperations.EnsureDrawPile())
        {
            CompleteBoundaryPingkou(frame);
            return;
        }

        var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind)).OrderBy(c => c.Id).ToArray();
        if (candidates.Length == 0)
        {
            CompleteBoundaryPingkou(frame);
            return;
        }

        var card = candidates[_randomOperations._random.Next(candidates.Length)];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = frame.BoundaryZhuHuan!with { Stage = BoundaryZhuHuanStage.Movement, EquipmentCard = card.Id, Before = before, After = before }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([card.Id], CardLocation.Hand(frame.OwnerSeat), new(BoundaryPingkouGain), (batch, moves) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { BoundaryZhuHuan = active.BoundaryZhuHuan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Batch = batch } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanEquipmentObtainedEvent(frame.Id, frame.OwnerSeat, card.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, batch));
        });
        ContinueBoundaryPingkou(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 53800)]
    internal bool ContinueBoundaryPingkou(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhuHuan: { } receipt } frame || frame.Id != id)
            return false;
        if (!ValidBoundaryPingkou(frame))
            throw new InvalidOperationException("Pingkou lost its exact actual-turn targets or issued payment.");
        if (frame.AttackAttempt is not null)
            return true;
        if (receipt.Stage == BoundaryZhuHuanStage.Targets)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishBoundaryPingkou(frame);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        receipt = frame.BoundaryZhuHuan!;
        if (receipt.Stage == BoundaryZhuHuanStage.Movement)
        {
            CompleteBoundaryPingkou(frame);
            return true;
        }

        if (receipt.Stage != BoundaryZhuHuanStage.Damage || receipt.Index >= receipt.Targets.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanDamageIssuedEvent>().Count(e => e.FrameId == id && e.Index == receipt.Index && e.Target == receipt.Targets[receipt.Index]) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanDamageReturnedEvent>().Any(e => e.FrameId == id && e.Index == receipt.Index))
            throw new InvalidOperationException("Pingkou damage may return only once to its exact issued target.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanDamageReturnedEvent(id, receipt.Index, receipt.Targets[receipt.Index]));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhuHuan = receipt with { Index = receipt.Index + 1 } });
        IssueBoundaryPingkouDamage(frame);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 53800)]
    internal bool ReturnBoundaryPingkouMovement(ProgramSkillFrame frame) => frame.BoundaryZhuHuan is not null && ContinueBoundaryPingkou(frame.Id);
    internal void CompleteBoundaryPingkou(ProgramSkillFrame frame)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhuHuanCompletedEvent(frame.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { BoundaryZhuHuan = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool ValidBoundaryPingkou(ProgramSkillFrame frame) => frame.BoundaryZhuHuan is { } receipt && frame.InstructionIndex == 1 && frame.SkillId == BoundaryZhuHuanComposition.Pingkou && receipt.Source == BoundaryZhuHuanSource(frame) && receipt.GameplayHash == frame.GameplayHash && receipt.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && receipt.EndedFrame == frame.WindowContext?.ParentFrameId && frame.WindowContext?.Window == SkillProgramTriggerWindow.AfterTurnEnded && receipt.Maximum > 0 && receipt.Maximum == BoundaryPingkouMaximum(frame.OwnerSeat) && receipt.Targets.Count <= receipt.Maximum && receipt.Targets.Distinct().Count() == receipt.Targets.Count && receipt.Targets.All(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && s != frame.OwnerSeat) && receipt.Index >= 0 && receipt.Index <= receipt.Targets.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanStartedEvent>().Count(e => e.FrameId == frame.Id && e.Source == receipt.Source && e.GameplayHash == receipt.GameplayHash && e.ActualTurn == receipt.ActualTurn && e.EndedFrame == receipt.EndedFrame && e.Maximum == receipt.Maximum) == 1 && (!receipt.Sealed || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanSealedEvent>().Count(e => e.FrameId == frame.Id && e.Count == receipt.Targets.Count) == 1 && receipt.Targets.SequenceEqual(_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryZhuHuanTargetEvent>().Where(e => e.FrameId == frame.Id).OrderBy(e => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, e.Target)).Select(e => e.Target)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 53800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 53800)]
    internal bool KeepIssuedBoundaryPingkou(ProgramSkillFrame frame) => frame.BoundaryZhuHuan is { Sealed: true } && ValidBoundaryPingkou(frame);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryZhuHuanPingkou)]
    internal PromptChoice BoundaryPingkouAi(PendingDecision pending, ProgramSkillFrame frame) => pending.Choices.FirstOrDefault(c => c.Targets is [var target] && !CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target])) ?? pending.Choices.Single(c => c.Parameters["branch"] == "finish");
}
