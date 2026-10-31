#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanTanYuanShangOlRuntime
{
    internal const string NeifaDrawReason = "skill-program.neifa.draw", NeifaFieldReason = "skill-program.neifa.field", NeifaDiscardReason = "skill-program.neifa.discard", NeifaEquipmentReason = "skill-program.neifa.equipment-draw";
    internal static CardConversionSource NeifaSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_yuan_tan_yuan_shang_ol_YuanTanYuanShangPrograms.NeifaSource(f);
    internal bool NeifaSourceCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(f) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 520)]
    internal bool CanOfferNeifa(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        var features = ProgramInstructionResolver.Default.Features(t);
        if (features.HasOperation(SkillProgramEffectOp.GainThenDiscardAndArmNeifaActualTurn))
            return c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0;
        if (features.HasOperation(SkillProgramEffectOp.AdjustNeifaOrdinaryTrickTargets))
            return ExactNeifaCandidate(c, w, out var use) && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind);
        if (features.HasOperation(SkillProgramEffectOp.DrawForFirstTwoPaidNeifaEquipmentUses))
            return ExactNeifaCandidate(c, w, out var eq) && eq.NeifaEquipment is { } q && ValidNeifaEquipment(q, eq) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NeifaSegmentPaidEvent>().Any(e => e.WindowId == w.ParentFrameId && e.Segment == "equipment-draw");
        return true;
    }

    internal SkillProgramStepOutcome BeginNeifa(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Neifa is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w)
            throw new InvalidOperationException("Neifa requires its new exact native trigger.");
        var r = new NeifaReceipt(NeifaSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, w.ParentFrameId, NeifaStage.Branch);
        if (effect.Op == SkillProgramEffectOp.GainThenDiscardAndArmNeifaActualTurn)
        {
            if (!NeifaSourceCurrent(f) || w.Window != SkillProgramTriggerWindow.PlayPhaseStarting || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play)
                throw new InvalidOperationException("Neifa activation lost actual own Play start.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Neifa = r });
            PublishNeifa(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (f.TriggerId is not { } binding || !ExactNeifaCandidate(new(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, 0), w, out var use))
            throw new InvalidOperationException("Neifa issued right lost its exact actor/native window.");
        if (effect.Op == SkillProgramEffectOp.AdjustNeifaOrdinaryTrickTargets)
        {
            if (!CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(use.CardKind))
                throw new InvalidOperationException("Neifa target adjustment requires ordinary trick.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Neifa = r with { Stage = NeifaStage.Targets, UseId = use.Id, ActionId = use.Action!.ActionId, OriginalTargets = NeifaPrimaryShape(use) } });
            PublishNeifa(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (use.NeifaEquipment is not { } q || !ValidNeifaEquipment(q, use))
            throw new InvalidOperationException("Neifa draw requires its actual paid equipment use.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Neifa = r with { Stage = NeifaStage.EquipmentDrawChildren, Equipment = q, UseId = use.Id, ActionId = q.Action.ActionId } });
        PayNeifaDraw(f, q.RequestedDraw, true);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<Card> NeifaFieldCards(int actor) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => new[] { CardLocation.Equipment(p.Seat), CardLocation.Judgment(p.Seat) }).SelectMany(_cardZonesOperations._cardZones.CardsAt).Where(c => !c.IsGeneralWeapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Obtain)).ToArray();
    internal PromptChoice NeifaChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"neifa.{(f.Id)}.{(f.Neifa!.Stage)}.{(branch)}." + string.Join('-', cards ?? []) + "." + string.Join('-', targets ?? [])), label, cards ?? [], targets ?? [], new Dictionary<string, string> { { "program-action", "neifa" }, { "branch", branch } });
    internal IReadOnlyList<PromptChoice> NeifaChoices(ProgramSkillFrame f)
    {
        var r = f.Neifa!;
        var list = new List<PromptChoice>();
        if (r.Stage == NeifaStage.Branch)
        {
            list.Add(NeifaChoice(f, "draw", "摸两张牌"));
            if (NeifaFieldCards(f.OwnerSeat).Count > 0)
                list.Add(NeifaChoice(f, "field", "获得场上一张牌"));
        }

        if (r.Stage == NeifaStage.Field)
            list.AddRange(NeifaFieldCards(f.OwnerSeat).Select(c => NeifaChoice(f, "take", c.DisplayName, [c.Id])));
        if (r.Stage == NeifaStage.Discard)
            list.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !c.IsGeneralWeapon).Select(c => NeifaChoice(f, "discard", c.Label, c.Cards)));
        if (r.Stage == NeifaStage.Targets)
            return NeifaTargetChoices(f);
        return Array.AsReadOnly(list.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void PublishNeifa(ProgramSkillFrame f)
    {
        var choices = NeifaChoices(f);
        if (choices.Count == 0)
        {
            FinishNeifa(f);
            return;
        }

        var privateChoice = f.Neifa!.Stage != NeifaStage.Targets;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "请选择内伐结算。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = privateChoice,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.GainThenDiscardAndArmNeifaActualTurn, SkillProgramEffectOp.AdjustNeifaOrdinaryTrickTargets, SkillProgramEffectOp.DrawForFirstTwoPaidNeifaEquipmentUses)]
    internal PromptChoice NeifaAi(ProgramSkillFrame f) => NeifaChoices(f).Last();
    [ProgramAnswer("neifa")]
    internal void ResolveNeifaChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.Neifa!;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], NeifaChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Neifa changed its published exact payer/entity/target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Segments.Count == 0 && r.Stage != NeifaStage.Targets && !NeifaSourceCurrent(f))
        {
            FinishNeifa(f);
            return;
        }

        var branch = choice.Parameters["branch"];
        if (r.Stage == NeifaStage.Targets)
        {
            ResolveNeifaTargets(f, choice);
            return;
        }

        if (r.Stage == NeifaStage.Branch)
        {
            if (branch == "draw")
            {
                PayNeifaDraw(f, 2, false);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Neifa = r with { Stage = NeifaStage.Field } });
            PublishNeifa(f);
            return;
        }

        var id = choice.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        var discard = r.Stage == NeifaStage.Discard;
        if (discard ? (from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) : !NeifaFieldCards(f.OwnerSeat).Any(c => c.Id == id))
            throw new InvalidOperationException("Neifa cannot replace its exact legal field/payment entity.");
        var category = discard ? CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind) : (SkillProgramCardCategory? )null;
        var name = discard ? "discard" : "field";
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Neifa = r with { Stage = discard ? NeifaStage.DiscardChildren : NeifaStage.GainChildren, DiscardCategory = category }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var logicalDiscard = discard ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, f.OwnerSeat, [id], new(NeifaDiscardReason), f.Id) : null;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], discard ? CardLocation.DiscardPile : CardLocation.Hand(f.OwnerSeat), new(discard ? NeifaDiscardReason : NeifaFieldReason), (batch, moves) => RecordNeifaSegment(f.Id, new(name, before, moves[^1].Sequence, 1, 1, batch, id, from)));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void PayNeifaDraw(ProgramSkillFrame f, int requested, bool equipment)
    {
        var r = f.Neifa!;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Neifa = r with { Stage = equipment ? NeifaStage.EquipmentDrawChildren : NeifaStage.GainChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        var actual = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], requested, true, new(equipment ? NeifaEquipmentReason : NeifaDrawReason)).Count;
        RecordNeifaSegment(f.Id, new(equipment ? "equipment-draw" : "draw", before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, requested, actual, null));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void RecordNeifaSegment(long frameId, NeifaPaidSegment s)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var r = f.Neifa!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Neifa = r with { Segments = r.Segments.Append(s).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NeifaSegmentPaidEvent(frameId, r.Source, r.GameplayHash, r.ActualTurn, r.WindowId, s.Name, s.Before, s.After, s.Requested, s.Actual, s.BatchId));
    }

    internal bool DrainNeifa(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void FinishNeifa(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Neifa = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24800)]
    internal bool ResumeNeifa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Neifa: { Stage: NeifaStage.GainChildren or NeifaStage.DiscardChildren or NeifaStage.EquipmentDrawChildren } r } f || f.Id != id)
            return false;
        if (DrainNeifa(f))
            return true;
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Stage == NeifaStage.EquipmentDrawChildren)
        {
            FinishNeifa(f);
            return true;
        }

        if (r.Stage == NeifaStage.GainChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Neifa = r with { Stage = NeifaStage.Discard }, PendingMovementContinuation = null });
            PublishNeifa(f);
            return true;
        }

        var basic = r.DiscardCategory == SkillProgramCardCategory.Basic;
        var prior = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].NeifaTurnState;
        var same = prior?.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
        var state = new NeifaTurnState(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, basic || same && prior!.BasicBranch, !basic || same && prior!.NonBasicBranch, same ? prior!.EquipmentUses : 0, r.Source, r.GameplayHash, f.Id);
        _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].NeifaTurnState = state;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NeifaTurnArmedEvent(f.Id, r.Source, r.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, r.DiscardCategory!.Value, state.BasicBranch, state.NonBasicBranch));
        FinishNeifa(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 24800)]
    internal bool ReturnNeifaMovement(ProgramSkillFrame f) => f.Neifa is not null && ResumeNeifa(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 300)]
    internal bool CanContinueNeifa(ProgramSkillFrame f)
    {
        if (f.Neifa is null)
            return f.InstructionIndex == 0 && f.TriggerId is { } trigger && f.ActivationId == trigger && f.WindowContext is { } context && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && HasIssuedNeifaCandidate(new(f.OwnerSeat, f.SkillId, trigger, f.SkillInstanceId, f.GameplayHash, 0), context);
        var r = f.Neifa;
        if (r.Segments.Count == 0 && r.Stage != NeifaStage.Targets)
            return false;
        return true;
    }

    SkillProgramStepOutcome INeifaProgramHost.BeginNeifa(ProgramSkillFrame f, SkillProgramEffect e) => BeginNeifa(f, e);
}
