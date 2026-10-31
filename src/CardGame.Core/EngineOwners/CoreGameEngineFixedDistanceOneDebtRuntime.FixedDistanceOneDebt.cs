#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtRuntime
{
    internal const string FixedDistanceDebtReason = "program.fixed-distance-one.ending-discard";
    internal IEnumerable<FixedDistanceOneTurnGrant> FixedDistanceOneGrants() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FixedDistanceOneTurnGrantedEvent>().Select(e => e.Grant);
    internal bool FixedDistanceDebtConsumed(FixedDistanceOneTurnGrant g) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<FixedDistanceOneDebtConsumedEvent>().Any(e => e.Grant == g);
    internal bool ValidFixedDistanceOneGrant(FixedDistanceOneTurnGrant g) => g.ProgramFrameId > 0 && g.InstructionIndex == 1 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.TargetSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(g.ActualTurnOwnerSeat) && g.Source.OwnerSeat == g.ActualTurnOwnerSeat && g.TargetSeat != g.Source.OwnerSeat && FixedDistanceOneGrants().Count(other => other.ProgramFrameId == g.ProgramFrameId) == 1 && _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program is { } program && program.GameplayHash == g.GameplayHash && program.Activations.Any(a => a.Id == g.Source.BindingId && a.Effects is [{ Op: SkillProgramEffectOp.GrantFixedDistanceOneTurnPolicy }]) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == g.ProgramFrameId && e.OwnerSeat == g.Source.OwnerSeat && e.SkillId == g.Source.SkillId && e.ActivationId == g.Source.BindingId) == 1 && program.Triggers.Any(t => t.Id == g.EndingBindingId && t.TurnOwnerScope == SkillProgramTurnOwnerScope.IssuedFixedDistanceEnding && t.Effects is [{ Op: SkillProgramEffectOp.SettleFixedDistanceOneEndingDebt }]) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnStartedEvent>().Any(e => e.TurnNumber == g.ActualTurnNumber && e.ActorSeat == g.ActualTurnOwnerSeat);
    internal void GrantFixedDistanceOneTurnPolicy(ProgramSkillFrame supplied, int target)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var activation = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Activations.Single(a => a.Id == f.ActivationId);
        if (f.InstructionIndex != 1 || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == f.OwnerSeat || f.SelectedTargetSeats is not [var selected] || selected != target || f.SelectedCardIds.Count != 0 || activation.Effects is not [{ Op: SkillProgramEffectOp.GrantFixedDistanceOneTurnPolicy }] || FixedDistanceOneGrants().Any(g => g.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Fixed distance requires its exact zero-cost actual Play activation.");
        var ending = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.SettleFixedDistanceOneEndingDebt }]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FixedDistanceOneTurnGrantedEvent(new(f.Id, f.InstructionIndex, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, target, ending.Id)));
    }

    [ProgramDistanceRule(ProgramDistanceStage.DistanceProjection, 40)]
    internal RuleQueryEvaluation ApplyFixedDistanceOne(CharacterState source, CharacterState target, RuleQueryEvaluation original)
    {
        if (source.Seat == target.Seat || !_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantFixedDistanceOneTurnPolicy) || _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat))
            return original;
        var grants = FixedDistanceOneGrants().Where(g => g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && g.Source.OwnerSeat == source.Seat && g.TargetSeat == target.Seat && ValidFixedDistanceOneGrant(g)).ToArray();
        if (grants.Length == 0)
            return original;
        return RuleQueryService.Evaluate(SkillRuleQuery.OutgoingDistance, new(1, int.MaxValue), [new RuleQueryBaseTerm("distance:before-fixed-one", CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(original))], grants.Select(g => (RuleQueryContribution)new FiniteRuleQueryContribution($"fixed-distance-one:{(g.ProgramFrameId)}", SkillRuleOperation.Set, 1)).ToArray());
    }

    internal void ConsumeFixedDistanceDebt(FixedDistanceOneTurnGrant g, bool required)
    {
        if (!FixedDistanceDebtConsumed(g))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FixedDistanceOneDebtConsumedEvent(g, required));
    }

    internal void ExpireFixedDistanceOneGrants(int turn, int seat)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantFixedDistanceOneTurnPolicy))
            return;
        foreach (var g in FixedDistanceOneGrants().Where(g => g.ActualTurnNumber == turn && g.ActualTurnOwnerSeat == seat && !FixedDistanceDebtConsumed(g)).ToArray())
            ConsumeFixedDistanceDebt(g, false);
    }

    internal bool FixedDistanceTargetDamagedThisTurn(FixedDistanceOneTurnGrant g) => _corePlayCardHistoryOperations.EventsSinceLastBoundary(e => e is TurnStartedEvent).OfType<DamageAppliedEvent>().Any(e => e.Amount > 0 && !e.SourceLess && e.SourceSeat == g.Source.OwnerSeat && e.TargetSeat == g.TargetSeat);
    internal void AppendFixedDistanceOneEndingItems(List<TurnEndingBoundaryItem> items)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.GrantFixedDistanceOneTurnPolicy))
            return;
        foreach (var g in FixedDistanceOneGrants().Where(g => g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && !FixedDistanceDebtConsumed(g)).ToArray())
        {
            // Freeze the entire current real turn's applied damage at entry to Ending, including damage before issuance.
            if (!ValidFixedDistanceOneGrant(g) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat].IsAlive || FixedDistanceTargetDamagedThisTurn(g))
            {
                ConsumeFixedDistanceDebt(g, false);
                continue;
            }

            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(g.Source.SkillId).Program!.Triggers.Single(t => t.Id == g.EndingBindingId);
            var candidate = new ProgramTriggerCandidate(g.Source.OwnerSeat, g.Source.SkillId, trigger.Id, g.Source.SkillInstanceId, g.GameplayHash, trigger.Priority, items.Count);
            items.Add(new(TurnEndingBoundaryItemKind.Program, candidate.Priority, $"fixed-distance-debt:{(g.ProgramFrameId)}", candidate, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat])) { FixedDistanceDebt = g });
        }
    }

    internal bool MatchesFixedDistanceDebt(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool begun)
    {
        if (context.FixedDistanceDebt is not { } g || !ValidFixedDistanceOneGrant(g) || context.Window != SkillProgramTriggerWindow.TurnEnding || context.SourceSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || g.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || g.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || c.OwnerSeat != g.Source.OwnerSeat || c.SkillId != g.Source.SkillId || c.SkillInstanceId != g.Source.SkillInstanceId || c.BindingId != g.EndingBindingId || c.GameplayHash != g.GameplayHash || FixedDistanceDebtConsumed(g) != begun || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } boundary || boundary.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || boundary.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || boundary.ItemIndex < 0 || boundary.ItemIndex >= boundary.Items.Count || boundary.Items[boundary.ItemIndex].FixedDistanceDebt != g || boundary.Items[boundary.ItemIndex].Candidate is not { } original || original != c || context.OccurrenceIndex != c.OccurrenceIndex)
            return false;
        return !begun || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedDistanceOneDebtConsumedEvent>().Count(e => e.Grant == g && e.RequiredDiscard) == 1;
    }

    internal bool MatchesFixedDistanceDebt(ProgramSkillFrame f) => f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } boundary && boundary.ItemIndex >= 0 && boundary.ItemIndex < boundary.Items.Count && boundary.Items[boundary.ItemIndex].Candidate is { } c && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) && MatchesFixedDistanceDebt(c, context, true);
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 30)]
    internal bool HasIssuedFixedDistanceDebtCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => MatchesFixedDistanceDebt(c, context, false);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 690)]
    internal bool CanContinueIssuedFixedDistanceEnding(ProgramSkillFrame f) => MatchesFixedDistanceDebt(f);
    internal void ConsumeFixedDistanceDebtAtBegin(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.FixedDistanceDebt is not { } g)
            return;
        if (!MatchesFixedDistanceDebt(c, context, false))
            throw new InvalidOperationException("The exact issued distance debt changed before beginning.");
        ConsumeFixedDistanceDebt(g, true);
    }

    internal void ConsumeSkippedFixedDistanceDebt(TurnEndingBoundaryFrame f)
    {
        if (f.Items[f.ItemIndex].FixedDistanceDebt is { } g)
            ConsumeFixedDistanceDebt(g, false);
    }

    internal IReadOnlyList<PromptChoice> FixedDistanceDebtChoices(ProgramSkillFrame f) => Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c => PromptChoiceProjection.Freeze(c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "fixed-distance-ending-debt" } })).ToArray());
    internal SkillProgramStepOutcome SettleFixedDistanceOneEndingDebt(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MatchesFixedDistanceDebt(f) || f.InstructionIndex != 1)
            throw new InvalidOperationException("Distance debt lost its exact issued Ending item.");
        if (f.FixedDistanceDebtPayment is null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FixedDistanceDebtPayment = new(f.WindowContext!.FixedDistanceDebt!, f.InstructionIndex) });
        if (f.FixedDistanceDebtPayment.CardId is not null)
            throw new InvalidOperationException("An issued physical discard cannot be requested twice.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || FixedDistanceDebtChoices(f).Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { FixedDistanceDebtPayment = null });
            return SkillProgramStepOutcome.Continue;
        }

        PublishFixedDistanceDebt(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishFixedDistanceDebt(ProgramSkillFrame f)
    {
        var choices = FixedDistanceDebtChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "奋迅：本回合未对原目标造成伤害，弃置一张牌。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
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

    [ProgramAnswer("fixed-distance-ending-debt")]
    internal void ResolveFixedDistanceDebtChoice(PromptChoice choice)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Distance debt has no owning program.");
        // CardLocation is a non-nullable struct, so the owned-selection check runs
        // against a lifted nullable instead of an impossible negated pattern.
        var debtCardId = choice.Cards is [var selected] ? selected : default(int? );
        var debtSelection = debtCardId is { } paid ? _cardZonesOperations._cardZones.GetLocation(paid) : (CardLocation? )null;
        if (f.FixedDistanceDebtPayment is not { CardId: null } r || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], FixedDistanceDebtChoices(f).Where(c => c.Id == choice.Id).ToArray()) || debtCardId is not { } id || debtSelection is not { OwnerSeat: { } owner } from || owner != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("The debt lost its exact owned physical selection.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { FixedDistanceDebtPayment = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FixedDistanceDebtPayment = r with { CardId = id, From = from, SequenceBefore = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [id], new(FixedDistanceDebtReason), (_, records) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { FixedDistanceDebtPayment = current.FixedDistanceDebtPayment!with { SequenceAfter = records.Single().Sequence } });
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FixedDistanceDebtPayment = f.FixedDistanceDebtPayment!with { SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        r = f.FixedDistanceDebtPayment!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FixedDistanceOneDebtPaidEvent(f.Id, r.Grant, id, from, r.SequenceBefore, r.SequenceAfter));
        if (!DrainFixedDistanceDebtChildren(f))
            ReturnFixedDistanceOneDebtMovement(f);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 310)]
    internal bool ResumeFixedDistanceOneDebt(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FixedDistanceDebtPayment: not null } f || f.Id != id)
            return false;
        if (f.PendingMovementContinuation is not null)
        {
            if (!DrainFixedDistanceDebtChildren(f))
                ReturnFixedDistanceOneDebtMovement(f);
            return true;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || FixedDistanceDebtChoices(f).Count == 0)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { FixedDistanceDebtPayment = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return true;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishFixedDistanceDebt(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 300)]
    internal bool ReturnFixedDistanceOneDebtMovement(ProgramSkillFrame f)
    {
        if (f.FixedDistanceDebtPayment is null)
            return false;
        if (f.PendingMovementContinuation is not { } pending || !IsFixedDistanceDebtMovement(f, ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect, pending))
            throw new InvalidOperationException("The debt cannot consume another physical payment's return.");
        if (DrainFixedDistanceDebtChildren(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null, FixedDistanceDebtPayment = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool ValidFixedDistanceDebtPayment(ProgramSkillFrame f)
    {
        if (f.FixedDistanceDebtPayment is not { } r || !MatchesFixedDistanceDebt(f) || r.Grant != f.WindowContext!.FixedDistanceDebt || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1)
            return false;
        if (r.CardId is null)
            return r.From is null && r.SequenceBefore == 0 && r.SequenceAfter == 0 && f.PendingMovementContinuation is null && (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && p.PlayerSeat == f.OwnerSeat && p.TargetSeat == f.OwnerSeat && p.SkillPrompt?.SkillId == f.SkillId && CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, FixedDistanceDebtChoices(f)));
        if (r.From is not { OwnerSeat: var owner } from || owner != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.SequenceBefore < 0 || r.SequenceAfter <= r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat)
            return false;
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FixedDistanceOneDebtPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        return paid is [var fact] && fact.Grant == r.Grant && fact.CardId == r.CardId && fact.From == from && fact.SequenceBefore == r.SequenceBefore && fact.SequenceAfter == r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == r.CardId && m.From == from && (m.To == CardLocation.DiscardPile || m.To == CardLocation.OutsideGame && (_coreCardAttackStateOperations.GetAttackCard(m.CardId).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(m))) && m.Reason.Value == FixedDistanceDebtReason) == 1;
    }
    internal bool IsFixedDistanceDebtMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => e?.Op == SkillProgramEffectOp.SettleFixedDistanceOneEndingDebt && pending == f.PendingMovementContinuation && ValidFixedDistanceDebtPayment(f);
    internal bool DrainFixedDistanceDebtChildren(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);

    [ProgramAiChoice(SkillProgramEffectOp.SettleFixedDistanceOneEndingDebt)]
    internal PromptChoice SelectAiFixedDistanceDebt(PendingDecision d)
    {
        var snapshot = _corePlayerViewProjectionOperations.CreateSnapshot(d.PlayerSeat);
        // The exposed own-card choice is sufficient; no hidden zones or live actor state are scored.
        return d.Choices.OrderBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
