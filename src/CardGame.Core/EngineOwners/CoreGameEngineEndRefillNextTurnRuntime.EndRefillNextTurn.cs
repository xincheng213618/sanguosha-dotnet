#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEndRefillNextTurnRuntime
{
    SkillProgramStepOutcome IEndRefillNextTurnProgramHost.RefillAndScheduleNextTurnSelfOnly(ProgramSkillFrame f) => BeginEndRefillNextTurn(f);
    internal static string EndRefillReason(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_Core_EndRefillNextTurn.EndRefillReason(f);
    internal bool IsExactOwnAfterTurnEndedProgram(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.AfterTurnEnded } context && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is { } parent && parent.OwnerSeat == f.OwnerSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && parent.AfterTurnEnded?.CurrentChild?.FrameId == f.Id && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, f, context);
    internal SkillProgramStepOutcome BeginEndRefillNextTurn(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!IsExactOwnAfterTurnEndedProgram(f) || f.InstructionIndex != 1 || f.EndRefillNextTurn is not null)
            throw new InvalidOperationException("End refill requires its actual completed own turn and one unissued instruction.");
        var r = new EndRefillNextTurnReceipt
        {
            WindowFrameId = f.WindowContext!.ParentFrameId,
            Source = CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f),
            GameplayHash = f.GameplayHash,
            ActualTurnNumber = _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
            ActualTurnOwnerSeat = _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat,
            Stage = EndRefillNextTurnStage.Target,
            Targets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { EndRefillNextTurn = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndRefillNextTurnOfferedEvent(f.Id, r));
        PublishEndRefillNextTurnChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> EndRefillNextTurnChoices(ProgramSkillFrame f)
    {
        var r = f.EndRefillNextTurn!;
        var parameters = new Dictionary<string, string>
        {
            ["program-action"] = "end-refill-next-turn",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        return r.Targets.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Select(s => new PromptChoice(new($"end-refill-next-turn.{(f.Id)}.{(s)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[s].Name)} 摸至 {(Math.Min(5, _coreMatchParticipantsPlayersAccess._players[s].MaxHp))} 张手牌，并于其下回合开始检查目标限制", [], [s], parameters)).Append(new(new($"end-refill-next-turn.{(f.Id)}.decline"), "不执行", [], [], parameters)).ToArray();
    }

    internal void PublishEndRefillNextTurnChoice(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var choices = EndRefillNextTurnChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse, EndRefillNextTurn = f.EndRefillNextTurn!with { Choices = choices } });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】选择一名角色。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = f.EndRefillNextTurn!.Choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("end-refill-next-turn")]
    internal void ResolveEndRefillNextTurnChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("End refill lost its frame.");
        RequireEndRefillNextTurnReceipt(f);
        if (f.EndRefillNextTurn!.Stage != EndRefillNextTurnStage.Target || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], f.EndRefillNextTurn.Choices.Where(c => c.Id == choice.Id).ToArray()) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], EndRefillNextTurnChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("End refill selected a stale participant or an already issued draw.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Targets.Count == 0)
        {
            CompleteEndRefillNextTurn(f);
            return;
        }

        var target = _coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count;
        var requested = Math.Max(0, Math.Min(target.MaxHp, 5) - hand);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var r = f.EndRefillNextTurn with
        {
            Stage = EndRefillNextTurnStage.MovementChildren,
            TargetSeat = target.Seat,
            FrozenHandCount = hand,
            FrozenMaxHp = target.MaxHp,
            DrawRequested = requested,
            MovementBefore = before,
            MovementAfter = before,
            Choices = []
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, EndRefillNextTurn = r, PendingMovementContinuation = new(target.Seat, 0, null) });
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(target, requested, true, new(EndRefillReason(f))).Count;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { EndRefillNextTurn = r with { DrawActual = actual, MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndRefillNextTurnIssuedEvent(f.Id, f.EndRefillNextTurn!));
        if (!DrainEndRefillNextTurnChildren(f))
            ReturnEndRefillNextTurnMovement(f);
    }

    internal bool DrainEndRefillNextTurnChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    internal void CompleteEndRefillNextTurn(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndRefillNextTurnCompletedEvent(f.Id, f.EndRefillNextTurn!.TargetSeat));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { EndRefillNextTurn = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 20560)]
    internal bool ReturnEndRefillNextTurnMovement(ProgramSkillFrame f)
    {
        if (f.EndRefillNextTurn is null)
            return false;
        RequireEndRefillNextTurnReceipt(f);
        if (f.EndRefillNextTurn.Stage != EndRefillNextTurnStage.MovementChildren || f.PendingMovementContinuation is null)
            throw new InvalidOperationException("A refill movement returned without its frozen issued receipt.");
        if (DrainEndRefillNextTurnChildren(f))
            return true;
        CompleteEndRefillNextTurn(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 20560)]
    internal bool ResumeEndRefillNextTurn(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { EndRefillNextTurn: not null } f || f.Id != id)
            return false;
        RequireEndRefillNextTurnReceipt(f);
        if (f.PendingMovementContinuation is not null && !DrainEndRefillNextTurnChildren(f))
            ReturnEndRefillNextTurnMovement(f);
        return true;
    }

    internal void RequireEndRefillNextTurnReceipt(ProgramSkillFrame f)
    {
        var r = f.EndRefillNextTurn ?? throw new InvalidOperationException("End refill lost its receipt.");
        if (!IsExactOwnAfterTurnEndedProgram(f) || f.InstructionIndex != 1 || f.TriggerId is null || r.WindowFrameId != f.WindowContext!.ParentFrameId || r.Source != CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(f) || r.GameplayHash != f.GameplayHash || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.RefillAndScheduleNextTurnSelfOnly || r.Targets.Count != r.Targets.Distinct().Count() || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EndRefillNextTurnOfferedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Receipt.WindowFrameId == r.WindowFrameId && e.Receipt.Source == r.Source && e.Receipt.GameplayHash == r.GameplayHash && e.Receipt.ActualTurnNumber == r.ActualTurnNumber && e.Receipt.Targets.SequenceEqual(r.Targets)) != 1)
            throw new InvalidOperationException("End refill lost its actual parent, source, candidate or instruction.");
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EndRefillNextTurnIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (r.Stage == EndRefillNextTurnStage.Target)
        {
            if (r.TargetSeat != -1 || issued.Length != 0 || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("An unissued refill retained a paid movement.");
            return;
        }

        if (r.Stage != EndRefillNextTurnStage.MovementChildren || issued is not [var fact] || !SameEndRefillReceipt(fact.Receipt, r) || !r.Targets.Contains(r.TargetSeat) || r.FrozenHandCount < 0 || r.FrozenMaxHp < 0 || r.DrawRequested != Math.Max(0, Math.Min(r.FrozenMaxHp, 5) - r.FrozenHandCount) || r.DrawActual < 0 || r.DrawActual > r.DrawRequested || r.MovementBefore < 0 || r.MovementAfter < r.MovementBefore || r.MovementAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != r.TargetSeat)
            throw new InvalidOperationException("A paid refill lost its exact single movement producer.");
        var movements = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value == EndRefillReason(f)).ToArray();
        if (movements.Length != r.DrawActual || movements.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(r.TargetSeat)))
            throw new InvalidOperationException("A paid refill does not match the actual private card movements.");
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 450)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 440)]
    internal bool CanContinueEndRefillNextTurn(ProgramSkillFrame f)
    {
        if (f.EndRefillNextTurn is not { Stage: EndRefillNextTurnStage.MovementChildren })
            return false;
        RequireEndRefillNextTurnReceipt(f);
        return true;
    }

    internal static bool SameEndRefillReceipt(EndRefillNextTurnReceipt a, EndRefillNextTurnReceipt b) => RuntimeSkills.RuntimeRules_Core_EndRefillNextTurn.SameEndRefillReceipt(a, b);
    internal void ConsumeEndRefillNextTurn(CharacterState current)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RefillAndScheduleNextTurnSelfOnly))
            return;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var consumed = history.OfType<EndRefillNextTurnConsumedEvent>().Select(e => e.ProgramFrameId).ToHashSet();
        var actual = history.OfType<TurnStartedEvent>().LastOrDefault();
        if (actual is null || actual.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || actual.ActorSeat != current.Seat)
            throw new InvalidOperationException("Refill due consumption requires the actual next turn-start fact.");
        foreach (var e in history.OfType<EndRefillNextTurnIssuedEvent>().Where(e => e.Receipt.TargetSeat == current.Seat && e.Receipt.ActualTurnNumber < _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && !consumed.Contains(e.ProgramFrameId)))
        {
            var r = e.Receipt;
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(current).Count;
            var greatest = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count).DefaultIfEmpty(0).Max();
            var restricted = current.IsAlive && hand >= greatest;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndRefillNextTurnConsumedEvent(e.ProgramFrameId, current.Seat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, hand, greatest, restricted));
            if (!restricted)
                continue;
            var granted = _turnCardUseEffectsOperations._turnCardUseEffects.GrantTargetRestriction(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, current.Seat, e.ProgramFrameId, 0, r.Source, SkillProgramCardTargetRestriction.SelfOnly, null, current.Seat == r.Source.OwnerSeat ? null : current.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardTargetRestrictionGrantedEvent(granted));
        }
    }

    internal double EndRefillPublicChoiceScore(int owner, int target)
    {
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(owner);
        var p = snapshot.Players[target];
        if (!p.IsAlive)
            return double.NegativeInfinity;
        var draw = Math.Max(0, Math.Min(p.MaxHp, 5) - p.HandCount);
        var cancelDraw = _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreCardEffectIntervention(snapshot, CardKind.DrawTwo, target);
        var likelyMost = p.HandCount + draw >= snapshot.Players.Where(q => q.IsAlive && q.Seat != target).Select(q => q.HandCount).DefaultIfEmpty(0).Max();
        return -cancelDraw * draw / 2d + (likelyMost ? cancelDraw * .5d : 0d);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RefillAndScheduleNextTurnSelfOnly)]
    internal PromptChoice SelectAiEndRefillNextTurn(PendingDecision prompt, ProgramSkillFrame f) => prompt.Choices.OrderByDescending(c => c.Targets.Count == 0 ? 0d : EndRefillPublicChoiceScore(f.OwnerSeat, c.Targets[0])).First();
    internal double EndRefillNextTurnPublicScore(int owner) => _corePlayerViewProjectionOperations.CreateSnapshot(owner).Players.Where(p => p.IsAlive).Select(p => EndRefillPublicChoiceScore(owner, p.Seat)).Append(0d).Max();
}
