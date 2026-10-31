#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseNamePredictionRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 1090)]
    internal bool AllowsSettlePhaseNamePredictionTrigger(ProgramTriggerLegalityContext gate)
    {
        var candidate = gate.Candidate;
        var trigger = gate.Trigger;
        var context = gate.Context;
        return !(trigger.Effects is [{ Op: SkillProgramEffectOp.SettlePhaseNamePrediction }] && !HasIssuedPhaseNamePrediction(candidate, context));
    }

    internal bool TracksPhaseNamePredictions => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.BeginPhaseNamePrediction);
    internal int CurrentPredictionPhaseInstanceId => _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().LastOrDefault(f => f.Window == SkillProgramTriggerWindow.PlayEnding && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && f.PhaseNamePredictionPhase is { } key && key.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && key.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)?.PhaseNamePredictionPhase?.PhaseInstanceId ?? _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    internal const string PhasePredictionCostReason = "program.phase-name-prediction.cost";
    internal const string PhasePredictionClaimReason = "program.phase-name-prediction.claim";
    internal static CardKind PredictionName(CardKind kind) => RuntimeSkills.RuntimeRules_Core_PhaseNamePrediction.PredictionName(kind);
    internal bool PredictionConsumed(PhaseNamePredictionPolicy p) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Any(e => e is PhaseNamePredictionSettledEvent s && s.OriginalFrameId == p.Origin.FrameId || e is PhaseNamePredictionExpiredEvent x && x.OriginalFrameId == p.Origin.FrameId);
    internal static bool IsPhaseNamePredictionResolver(ProgramTriggerCandidate c, SkillProgram p) => RuntimeSkills.RuntimeRules_Core_PhaseNamePrediction.IsPhaseNamePredictionResolver(c, p);
    internal PhaseNamePredictionMaterial[] PredictionMaterials(long frameId, int seat) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frameId, seat, seat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Where(c => c.Cards.Count == 1).Select(c => c.Cards[0]).Distinct().Select(id => new PhaseNamePredictionMaterial(id, _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
    internal SkillProgramStepOutcome BeginPhaseNamePrediction(ProgramSkillFrame supplied, string stateId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.PhaseNamePrediction is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } target } c || target != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || target == f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not PlayPhaseStartingBoundaryFrame b || b.Id != c.ParentFrameId || b.ItemIndex < 0 || b.ItemIndex >= b.Items.Count || b.Items[b.ItemIndex].Candidate is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate))
            throw new InvalidOperationException("Phase prediction lost its exact foreign Play-start candidate.");
        var source = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var count = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseNamePredictionStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.StateId == stateId && e.ActualRoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
        var origin = new PhaseNamePredictionStartedEvent(f.Id, source, f.GameplayHash, stateId, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, target, count);
        var materials = PredictionMaterials(f.Id, f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseNamePrediction = new() { Origin = origin, Stage = PhaseNamePredictionStage.ChoosingCost, RequiredCount = Math.Min(count, materials.Length), Eligible = materials } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(origin);
        if (f.PhaseNamePrediction.RequiredCount == 0)
            ContinuePredictionAfterCost(f);
        else
            PublishPredictionChoice(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> PredictionChoices(ProgramSkillFrame f)
    {
        var r = f.PhaseNamePrediction!;
        if (r.Stage == PhaseNamePredictionStage.ChoosingCost)
            return Array.AsReadOnly(r.Eligible.Where(m => !r.Selected.Contains(m.CardId)).Select(m => PromptChoiceProjection.Freeze(new(new($"phase-prediction.{(f.Id)}.cost.{(m.CardId)}"), $"弃置 {(_coreCardAttackStateOperations.GetAttackCard(m.CardId).DisplayName)}", [m.CardId], [], new Dictionary<string, string> { ["program-action"] = "phase-name-cost", ["card-id"] = m.CardId.ToString(CultureInfo.InvariantCulture) }))).ToArray());
        if (r.Stage == PhaseNamePredictionStage.Revealing)
            return Array.AsReadOnly(r.TargetHand.Select((id, slot) => PromptChoiceProjection.Freeze(new(new($"phase-prediction.{(f.Id)}.slot.{(slot)}"), $"展示第 {(slot + 1)} 张手牌", [], [], new Dictionary<string, string> { ["program-action"] = "phase-name-reveal", ["slot-index"] = slot.ToString(CultureInfo.InvariantCulture) }))).ToArray());
        if (r.Stage == PhaseNamePredictionStage.Guessing)
            return Array.AsReadOnly(new[] { true, false }.Select(guess => PromptChoiceProjection.Freeze(new(new($"phase-prediction.{(f.Id)}.guess.{((guess ? "yes" : "no"))}"), guess ? "会使用同名牌" : "不会使用同名牌", [], [], new Dictionary<string, string> { ["program-action"] = "phase-name-guess", ["guess"] = guess ? "yes" : "no" }))).ToArray());
        return[];
    }

    internal void PublishPredictionChoice(ProgramSkillFrame f)
    {
        var r = f.PhaseNamePrediction!;
        var choices = PredictionChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        if (choices.Count == 0)
            throw new InvalidOperationException("An empty prediction choice cannot be published.");
        var text = r.Stage switch
        {
            PhaseNamePredictionStage.ChoosingCost => $"弃置 {(r.RequiredCount)} 张牌（已选 {(r.Selected.Count)}）。",
            PhaseNamePredictionStage.Revealing => "选择并展示对方的一张手牌。",
            _ => "秘密猜测其此阶段是否使用同名牌。"
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, text, choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("phase-name-cost", "phase-name-reveal", "phase-name-guess")]
    internal void ResolvePhaseNamePredictionChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Prediction has no owning frame.");
        if (f.PhaseNamePrediction is not { } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], PredictionChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Prediction choice changed its exact private draft.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == PhaseNamePredictionStage.ChoosingCost)
        {
            var id = choice.Cards.Single();
            var selected = r.Selected.Append(id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseNamePrediction = r = r with { Selected = selected } });
            if (selected.Length < r.RequiredCount)
            {
                PublishPredictionChoice(f);
                return;
            }

            if (r.Eligible.Where(m => selected.Contains(m.CardId)).Any(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) != m.From))
                throw new InvalidOperationException("A prediction cost moved before its one physical batch.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseNamePrediction = r with { Stage = PhaseNamePredictionStage.CostChildren, SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, selected, new(PhasePredictionCostReason), (_, records) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { PhaseNamePrediction = active.PhaseNamePrediction!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            });
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            r = f.PhaseNamePrediction!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseNamePredictionCostPaidEvent(f.Id, r.Origin.RequiredDiscardCount, selected.Length, r.SequenceBefore, r.SequenceAfter));
            if (!DrainPredictionChildren(f))
                ReturnPhaseNamePredictionMovement(f);
            return;
        }

        if (r.Stage == PhaseNamePredictionStage.Revealing)
        {
            var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
            var id = r.TargetHand[slot];
            if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(r.Origin.TargetSeat))
                throw new InvalidOperationException("The blind reveal lost its frozen target-hand entity.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseNamePrediction = r with { Stage = PhaseNamePredictionStage.Guessing, RevealedCardId = id } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(_coreCardAttackStateOperations.GetAttackCard(id)) })));
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(f.Id, "phase-name-hand"))
                return;
            PublishPredictionChoice(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (r.Stage != PhaseNamePredictionStage.Guessing || r.RevealedCardId is not { } shown)
            throw new InvalidOperationException("Prediction has no exact shown entity.");
        var ending = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.SettlePhaseNamePrediction, StateId: var state }] && state == r.Origin.StateId);
        var policy = new PhaseNamePredictionPolicy(r.Origin, shown, PredictionName(_coreCardAttackStateOperations.GetAttackCard(shown).Kind), choice.Parameters["guess"] == "yes", ending.Id);
        _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Add(policy);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseNamePredictionArmedEvent(f.Id, shown, PredictionName(_coreCardAttackStateOperations.GetAttackCard(shown).Kind)));
        foreach (var use in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseNamePredictionActualUseEvent>().ToArray())
            ObservePredictionUse(policy, use);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseNamePrediction = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinuePredictionAfterCost(ProgramSkillFrame f)
    {
        var r = f.PhaseNamePrediction!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Origin.TargetSeat].IsAlive || _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(r.Origin.TargetSeat)).Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseNamePrediction = null, PendingMovementContinuation = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null, PhaseNamePrediction = r with { Stage = PhaseNamePredictionStage.Revealing, TargetHand = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(r.Origin.TargetSeat)).Select(c => c.Id).ToArray() } });
        PublishPredictionChoice(f);
    }

    internal void ObservePhaseNamePredictionUse(CardActionContext action, bool qualifiedActualUse = false)
    {
        if (action.Type != CardActionType.Use && !qualifiedActualUse)
            return;
        RecordPredictionActualUse(action.ActionId, null, action.ActorSeat, action.EffectiveKind);
    }

    internal void RecordPredictionNativeCardUse(long frameId, int actorSeat, CardKind kind) => RecordPredictionActualUse(null, frameId, actorSeat, kind);
    internal void RecordPredictionActualUse(long? actionId, long? nativeFrameId, int actorSeat, CardKind kind)
    {
        if (!TracksPhaseNamePredictions || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || actorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return;
        var use = new PhaseNamePredictionActualUseEvent(actionId, nativeFrameId, actorSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, CurrentPredictionPhaseInstanceId, PredictionName(kind));
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseNamePredictionActualUseEvent>().Any(e => e == use))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(use);
        foreach (var p in _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Where(p => !PredictionConsumed(p) && p.Origin.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.Origin.PhaseInstanceId == use.PhaseInstanceId && p.Origin.TargetSeat == actorSeat).ToArray())
            ObservePredictionUse(p, use);
    }

    internal void ObservePredictionUse(PhaseNamePredictionPolicy p, PhaseNamePredictionActualUseEvent use)
    {
        if (use.ActualTurnNumber != p.Origin.ActualTurnNumber || use.PhaseInstanceId != p.Origin.PhaseInstanceId || use.ActorSeat != p.Origin.TargetSeat || use.NameKind != p.NameKind || PredictionConsumed(p) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseNamePredictionUseObservedEvent>().Any(e => e.OriginalFrameId == p.Origin.FrameId && e.ActionId == use.ActionId && e.NativeCardUseFrameId == use.NativeCardUseFrameId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseNamePredictionUseObservedEvent(p.Origin.FrameId, use.ActionId, use.ActorSeat, use.ActualTurnNumber, use.PhaseInstanceId, use.NameKind, use.NativeCardUseFrameId));
    }

    internal void AppendPhaseNamePredictionCandidates(List<ProgramTriggerCandidate> candidates)
    {
        foreach (var p in _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Where(p => !PredictionConsumed(p) && p.Origin.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && p.Origin.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && p.Origin.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId).ToArray())
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[p.Origin.Source.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[p.Origin.TargetSeat].IsAlive)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseNamePredictionExpiredEvent(p.Origin.FrameId));
                continue;
            }

            var t = _contentRegistryOperations._contentRegistry.GetSkill(p.Origin.Source.SkillId).Program!.Triggers.Single(t => t.Id == p.EndingBindingId);
            candidates.Add(new(p.Origin.Source.OwnerSeat, p.Origin.Source.SkillId, t.Id, p.Origin.Source.SkillInstanceId, p.Origin.GameplayHash, t.Priority, checked((int)p.Origin.FrameId)));
        }
    }

    internal ProgramSkillWindowContext AttachPhaseNamePredictionContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.PlayEnding)
            return context;
        var p = _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.SingleOrDefault(p => p.Origin.FrameId == c.OccurrenceIndex && p.EndingBindingId == c.BindingId && p.Origin.Source.SkillId == c.SkillId && p.Origin.Source.SkillInstanceId == c.SkillInstanceId && p.Origin.Source.OwnerSeat == c.OwnerSeat);
        return p is null ? context : context with
        {
            PhaseNamePrediction = p
        };
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 20)]
    internal bool HasIssuedPhaseNamePrediction(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.PhaseNamePrediction is not { } p || !_coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Contains(p) || PredictionConsumed(p) || context.Window != SkillProgramTriggerWindow.PlayEnding || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || p.Origin.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || p.Origin.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || p.Origin.PhaseInstanceId != CurrentPredictionPhaseInstanceId || p.Origin.FrameId != c.OccurrenceIndex || p.Origin.Source.OwnerSeat != c.OwnerSeat || p.Origin.Source.SkillId != c.SkillId || p.Origin.Source.SkillInstanceId != c.SkillInstanceId || p.EndingBindingId != c.BindingId || p.Origin.GameplayHash != c.GameplayHash || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame b || b.Window != context.Window || b.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || b.PhaseNamePredictionPhase != new PhaseNamePredictionPhaseKey(p.Origin.ActualTurnNumber, p.Origin.ActualTurnOwnerSeat, p.Origin.PhaseInstanceId) || b.CandidateIndex < 0 || b.CandidateIndex >= b.Candidates.Count || b.Candidates[b.CandidateIndex] != c)
            return false;
        return true;
    }

    internal SkillProgramStepOutcome SettlePhaseNamePrediction(ProgramSkillFrame supplied, string stateId)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = f.WindowContext!;
        var candidate = ((ProgramLifecycleTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Single(b => b.Id == c.ParentFrameId)).Candidates.Single(x => CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, x));
        if (f.InstructionIndex != 1 || !HasIssuedPhaseNamePrediction(candidate, c) || c.PhaseNamePrediction is not { } p || p.Origin.StateId != stateId)
            throw new InvalidOperationException("Prediction lost its exact issued phase-end item.");
        // Earlier frozen ending candidates may have killed the original target.
        // Retire this issued promise before creating any damage or claim receipt.
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[p.Origin.TargetSeat].IsAlive)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseNamePredictionExpiredEvent(p.Origin.FrameId));
            return SkillProgramStepOutcome.Continue;
        }

        var actual = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseNamePredictionUseObservedEvent>().Any(e => e.OriginalFrameId == p.Origin.FrameId);
        var correct = p.Guess == actual;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseNamePrediction = new() { Origin = p.Origin, IssuedPolicy = p, Stage = PhaseNamePredictionStage.DamageIssued } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseNamePredictionSettledEvent(f.Id, p.Origin.FrameId, p.Guess, actual, correct, p.CardId, p.Origin.TargetSeat));
        if (correct)
            return _coreSkillProgramsOperations.BeginProgramSkillDamage(f, p.Origin.TargetSeat, 1);
        var from = _cardZonesOperations._cardZones.GetLocation(p.CardId);
        if (from != CardLocation.DiscardPile && from != CardLocation.Processing && from != CardLocation.Hand(p.Origin.TargetSeat) && from != CardLocation.Equipment(p.Origin.TargetSeat))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseNamePrediction = null });
            return SkillProgramStepOutcome.Continue;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PhaseNamePrediction = f.PhaseNamePrediction!with { Stage = PhaseNamePredictionStage.ClaimChildren, SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Selected = [p.CardId], Eligible = [new(p.CardId, from)] }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([p.CardId], CardLocation.Hand(f.OwnerSeat), new(PhasePredictionClaimReason), (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { PhaseNamePrediction = active.PhaseNamePrediction!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        });
        if (!DrainPredictionChildren(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
            ReturnPhaseNamePredictionMovement(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainPredictionChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    [ProgramContinuation(ProgramContinuationKind.Movement, 60)]
    internal bool ReturnPhaseNamePredictionMovement(ProgramSkillFrame f)
    {
        if (f.PhaseNamePrediction is not { Stage: PhaseNamePredictionStage.CostChildren or PhaseNamePredictionStage.ClaimChildren } r)
            return false;
        if (f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Prediction lost its typed movement return.");
        if (DrainPredictionChildren(f))
            return true;
        if (r.Stage == PhaseNamePredictionStage.CostChildren)
            ContinuePredictionAfterCost(f);
        else
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseNamePrediction = null, PendingMovementContinuation = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        }

        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 80)]
    internal bool ResumePhaseNamePrediction(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PhaseNamePrediction: { } r } f || f.Id != id)
            return false;
        if (r.Stage is PhaseNamePredictionStage.CostChildren or PhaseNamePredictionStage.ClaimChildren)
            return ReturnPhaseNamePredictionMovement(f);
        if (r.Stage == PhaseNamePredictionStage.DamageIssued)
        {
            if (f.AttackAttempt is not null)
                return false;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseNamePrediction = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PhaseNamePrediction = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishPredictionChoice(f);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 630)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 650)]
    internal bool CanContinuePhaseNamePrediction(ProgramSkillFrame f) => f.PhaseNamePrediction is not null || f.WindowContext?.PhaseNamePrediction is { } p && _coreGameEnginePhaseNamePredictionPhaseNamePredictionsAccess._phaseNamePredictions.Contains(p);

    SkillProgramStepOutcome IPhaseNamePredictionHost.BeginPhaseNamePrediction(ProgramSkillFrame f, string state) => BeginPhaseNamePrediction(f, state);
    SkillProgramStepOutcome IPhaseNamePredictionHost.SettlePhaseNamePrediction(ProgramSkillFrame f, string state) => SettlePhaseNamePrediction(f, state);
}
