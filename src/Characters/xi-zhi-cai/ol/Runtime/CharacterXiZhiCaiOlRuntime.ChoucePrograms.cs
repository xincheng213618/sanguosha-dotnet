#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiZhiCaiOlRuntime
{
    internal SkillProgramStepOutcome StartChouceDamagePointJudgment(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (f.Chouce is not null || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame window || window.TriggerWindow != SkillProgramTriggerWindow.AfterDamageApplied || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } c || c.ParentFrameId != window.Id || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate()))
            throw new InvalidOperationException("Chouce needs its exact actual after-damage point candidate.");
        var attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
        if (!attack.DamageWasApplied || attack.TargetSeat != f.OwnerSeat || c.OccurrenceIndex < 0 || c.OccurrenceIndex >= attack.DamageAmount)
            throw new InvalidOperationException("Chouce lost its actual positive damage point.");
        var receipt = new ChoucePointReceipt(XiZhiCaiSource(f), f.GameplayHash, window.Id, window.ParentFrameId, attack.ResolutionId, c.OccurrenceIndex, attack.DamageAmount, attack.IsSourceLess || attack.IsDelayedJudgmentDamage ? null : attack.SourceSeat, attack.TargetSeat, f.InstructionIndex, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, effect.JudgmentReason!, effect.ResultBind!);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = new(receipt, ChouceStage.Judgment) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChouceJudgmentIssuedEvent(f.Id, receipt));
        return _coreProgramLifecycleOperations.StartProgramJudgment(f, f.OwnerSeat, receipt.Reason, receipt.ResultBind, SkillProgramCardSetVisibility.Public, f.OwnerSeat);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 150)]
    internal void CaptureChouceFrozenJudgment(IGameEvent payload)
    {
        if (payload is not JudgmentResolvedEvent { Suit: { } suit } e)
            return;
        ProgramSkillFrame? root = null;
        for (var index = _resolutionStackOperations._resolutionStack.Count - 1; index >= 0; index--)
            if (_resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame { Chouce: { Stage: ChouceStage.Judgment } pending } candidate && pending.Receipt.JudgmentFrameId == e.ResolutionId && candidate.Id == e.ParentResolutionId)
            {
                root = candidate;
                break;
            }

        if (root is null)
            return;
        var receipt = root.Chouce!.Receipt;
        if (e.TargetSeat != root.OwnerSeat || e.Reason != receipt.Reason || root.Chouce.FrozenSuit is not null)
            throw new InvalidOperationException("Chouce judgment lost its owning point or was frozen twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(root.Id, root with { Chouce = root.Chouce with { FrozenSuit = suit } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChouceJudgmentFrozenEvent(root.Id, e.ResolutionId, suit, e.CardId));
    }

    internal SkillProgramStepOutcome ResolveChouceFrozenJudgment(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Chouce is not { Stage: ChouceStage.Judgment, FrozenSuit: { } suit } p || p.Receipt.ResultBind != effect.SourceBind)
            throw new InvalidOperationException("Chouce requires its frozen real final judgment.");
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishChouce(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = p with { Stage = ChouceStage.ChooseTarget } });
        if (ChouceChoices(f).Count == 0)
        {
            FinishChouce(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishChouceDecision(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool ChouceIsRed(ProgramSkillFrame f) => f.Chouce!.FrozenSuit is Suit.Heart or Suit.Diamond;
    internal IReadOnlyList<PromptChoice> ChouceCardOptions(ProgramSkillFrame f, int target) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, target, [CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment], OwnedCardMoveIntent.Discard, canSelect: (zone, card) => target != f.OwnerSeat || !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(target, card, new(zone, target), OwnedCardMoveIntent.Discard));
    internal IReadOnlyList<PromptChoice> ChouceChoices(ProgramSkillFrame f)
    {
        if (f.Chouce!.Stage == ChouceStage.ChooseCard)
            return Array.AsReadOnly(ChouceCardOptions(f, f.Chouce.TargetSeat!.Value).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "chouce", ["chouce-action"] = "discard" } }).ToArray());
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (ChouceIsRed(f) || ChouceCardOptions(f, p.Seat).Count > 0)).Select(p => new PromptChoice(new($"chouce.{(f.Id)}.target-{(p.Seat)}"), p.Name, [], [p.Seat], new Dictionary<string, string> { { "program-action", "chouce" }, { "chouce-action", "target" } })).ToArray());
    }

    internal void PublishChouceDecision(ProgramSkillFrame f)
    {
        var choices = ChouceChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, ChouceIsRed(f) ? "请选择筹策摸牌角色。" : "请选择筹策弃牌。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.Chouce!.TargetSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveChouceFrozenJudgment)]
    internal PromptChoice ChouceAiChoice(ProgramSkillFrame f) => ChouceChoices(f).First();
    [ProgramAnswer("chouce")]
    internal void ResolveChouceChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var p = f.Chouce!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], ChouceChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Chouce lost its actual chooser authority.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishChouce(f);
            return;
        }

        if (p.Stage == ChouceStage.ChooseTarget)
        {
            var target = choice.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = p = p with { TargetSeat = target } });
            if (!ChouceIsRed(f))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = p with { Stage = ChouceStage.ChooseCard } });
                PublishChouceDecision(f);
                return;
            }

            var count = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].XianfuRelations.Any(g => g.TargetSeat == target) ? 2 : 1;
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = p with { Stage = ChouceStage.NativeChildren, Before = before, After = before, Count = 0 }, PendingMovementContinuation = new(target, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
            var actual = _coreMatchParticipantsPlayersAccess._players[target].IsAlive ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[target], count, true, new("program.chouce.red")).Count : 0;
            var fresh = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = fresh with { Chouce = fresh.Chouce!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Count = actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChouceOutcomeIssuedEvent(f.Id, p.Receipt.JudgmentFrameId, target, true, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, actual, null));
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var owner = p.TargetSeat!.Value;
        var location = new CardLocation(zone, owner);
        var slot = int.Parse(choice.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture);
        var card = _cardZonesOperations._cardZones.CardsAt(location)[slot];
        if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, location, OwnedCardMoveIntent.Discard) || owner == f.OwnerSeat && _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, location, OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("Chouce actual discard ceased to be legal.");
        var first = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = p with { Stage = ChouceStage.NativeChildren, CardId = card.Id, CardLocation = location, Before = first, After = first, Count = 1 }, PendingMovementContinuation = new(owner, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, location, new("program.chouce.black"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id));
        var freshBlack = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = freshBlack with { Chouce = freshBlack.Chouce!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChouceOutcomeIssuedEvent(f.Id, p.Receipt.JudgmentFrameId, owner, false, first, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, 1, card.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 23103)]
    internal bool ResumeChouce(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Chouce: { } pending } f || f.Id != id)
            return false;
        if (pending.Stage == ChouceStage.Judgment && !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && pending.FrozenSuit is not null && f.InstructionIndex == pending.Receipt.StartInstructionIndex && f.CardSetBindings.Any(binding => binding.Name == pending.Receipt.ResultBind) && !_resolutionStackOperations._resolutionStack.Any(child => child.Id == pending.Receipt.JudgmentFrameId))
        {
            FinishChouce(f);
            return true;
        }

        if (pending.Stage != ChouceStage.NativeChildren)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        FinishChouce(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 23103)]
    internal bool ReturnChouceMovement(ProgramSkillFrame f) => f.Chouce is not null && ResumeChouce(f.Id);
    internal void FinishChouce(ProgramSkillFrame f)
    {
        var r = f.Chouce!.Receipt;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChouceCompletedEvent(f.Id, r.JudgmentFrameId, r.PointIndex));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Chouce = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }
}
