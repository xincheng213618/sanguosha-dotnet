#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineInspectedHandSlashesRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 70, SkillProgramEffectOp.PayHpInspectHandThenDiscardOrSlash)]
    internal bool AllowsPayHpInspectHandThenDiscardOrSlashActivation(ProgramActivationLegalityContext gate)
    {
        var owner = gate.Owner;
        var features = gate.Features;
        return !(features.HasOperation(SkillProgramEffectOp.PayHpInspectHandThenDiscardOrSlash) && owner.Hp < 1);
    }

    internal const string InspectedHandDiscardReason = "program.hp-hand-inspection.discard";
    internal CardConversionSource InspectedHandSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome BeginInspectedHandSlash(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.InspectedHandSlash is not null || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedTargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == owner.Seat || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || owner.Hp < 1 || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Count == 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("HP hand inspection requires its exact unpaid living activation.");
        var d = new InspectedHandSlashDraft(f.InstructionIndex, InspectedHandSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, target, owner.Hp, owner.Hp - 1, InspectedHandSlashStage.HpChildren, []);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { InspectedHandSlash = d });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new InspectedHandHpPaidEvent(f.Id, d.Source, d.GameplayHash, d.TurnNumber, d.TurnOwnerSeat, target, d.HpBefore, d.HpAfter));
        var outcome = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1);
        if (outcome != SkillProgramStepOutcome.AwaitChild)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 680)]
    internal bool ResumeInspectedHandSlash(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.InspectedHandSlash is not { } d)
            return false;
        if (!_coreInspectedHandSlashReturnsOperations.Value.ValidInspectedHandSlash(f))
            throw new InvalidOperationException("HP hand inspection lost its original once-paid activation.");
        if (d.Stage == InspectedHandSlashStage.SlashIssued)
            throw new InvalidOperationException("An issued hand-inspection Slash requires its exact typed return.");
        if (d.Stage == InspectedHandSlashStage.Viewing)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishInspectedHandChoice(f);
            return true;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
        // Source qualification was consumed by the actual cost; later loss cannot repeat or retract that payment.
        if (d.Stage == InspectedHandSlashStage.DiscardChildren || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.TargetSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat]).Count == 0)
        {
            FinishInspectedHandSlash(f, false);
            return true;
        }

        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat]);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { InspectedHandSlash = d with { Stage = InspectedHandSlashStage.Viewing, ViewedCardIds = hand.Select(c => c.Id).ToArray(), ContainsPrintedDodge = hand.Any(c => c.Kind == CardKind.Dodge) } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new InspectedHandViewedEvent(f.Id, f.OwnerSeat, d.TargetSeat, hand.Count));
        PublishInspectedHandChoice(f);
        return true;
    }

    internal Dictionary<string, string> InspectedHandParameters(ProgramSkillFrame f, string option) => new()
    {
        ["program-action"] = "inspected-hand-slash",
        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
        ["option"] = option
    };
    internal IReadOnlyList<PromptChoice> InspectedHandChoices(ProgramSkillFrame f)
    {
        var d = f.InspectedHandSlash!;
        if (d.ContainsPrintedDodge)
            return[new(new($"inspected-hand.{(f.Id)}.slash"), "已观看手牌，结算视为使用杀。", [], [], InspectedHandParameters(f, "slash"))];
        return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, d.TargetSeat, [CardZoneKind.Hand], OwnedCardMoveIntent.Discard, canSelect: (_, card) => d.ViewedCardIds.Contains(card.Id)).Select(choice =>
        {
            var args = choice.Parameters.ToDictionary(p => p.Key, p => p.Value, StringComparer.Ordinal);
            foreach (var pair in InspectedHandParameters(f, "discard"))
                args[pair.Key] = pair.Value;
            var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
            var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat])[slot];
            return new PromptChoice(choice.Id, $"弃置【{(card.DisplayName)}】（{(card.Suit)} {(card.RankText)}）", [card.Id], [], args);
        }).ToArray());
    }

    internal void PublishInspectedHandChoice(ProgramSkillFrame f)
    {
        var choices = InspectedHandChoices(f);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.InspectedHandSlash!.TargetSeat].IsAlive || choices.Count == 0)
        {
            FinishInspectedHandSlash(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "观看其当前全部手牌，随后按牌面结算。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.InspectedHandSlash.TargetSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name + " · 观看手牌", skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal IEnumerable<CardSnapshot> InspectedHandPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.InspectedHandSlash is { Stage: InspectedHandSlashStage.Viewing }).SelectMany(f => f.InspectedHandSlash!.ViewedCardIds.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.InspectedHandSlash.TargetSeat)).Select(_coreCardAttackStateOperations.GetAttackCard)).Select(_coreSnapshotValueProjectionOperations.ToSnapshot);
    [ProgramAnswer("inspected-hand-slash")]
    internal void ResolveInspectedHandChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.InspectedHandSlash is not { Stage: InspectedHandSlashStage.Viewing } d || !_coreInspectedHandSlashReturnsOperations.Value.ValidInspectedHandSlash(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != f.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], InspectedHandChoices(f).Where(c => c.Id == choice.Id).ToArray()) || !_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[d.TargetSeat]).Select(c => c.Id).SequenceEqual(d.ViewedCardIds))
            throw new InvalidOperationException("The inspected hand choice must retain its private viewer, complete current hand and exact payment.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[d.TargetSeat].IsAlive)
        {
            FinishInspectedHandSlash(f, false);
            return;
        }

        if (d.ContainsPrintedDodge)
        {
            _coreInspectedHandSlashReturnsOperations.Value.IssueInspectedHandSlash(f);
            return;
        }

        if (choice.Cards is not [var cardId] || choice.Parameters.GetValueOrDefault("option") != "discard" || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(d.TargetSeat) || _coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, _coreCardAttackStateOperations.GetAttackCard(cardId), CardLocation.Hand(d.TargetSeat), OwnedCardMoveIntent.Discard))
            throw new InvalidOperationException("An inspected card cannot bypass actual discard eligibility.");
        var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { InspectedHandSlash = d with { Stage = InspectedHandSlashStage.DiscardChildren, DiscardedCardId = cardId, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, _coreCardAttackStateOperations.GetAttackCard(cardId), CardLocation.Hand(d.TargetSeat), new(InspectedHandDiscardReason), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, f.OwnerSeat, f.Id));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { InspectedHandSlash = f.InspectedHandSlash!with { SequenceAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new InspectedHandDiscardPaidEvent(f.Id, d.TargetSeat, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 380)]
    internal bool ReturnInspectedHandMovement(ProgramSkillFrame f)
    {
        if (f.InspectedHandSlash is not { Stage: InspectedHandSlashStage.HpChildren or InspectedHandSlashStage.DiscardChildren })
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.PayHpInspectHandThenDiscardOrSlash)]
    internal PromptChoice SelectAiInspectedHand(PendingDecision decision, ProgramSkillFrame f) => f.InspectedHandSlash!.ContainsPrintedDodge ? decision.Choices.Single() : decision.Choices.OrderByDescending(c => CardCatalog.Get(_coreCardAttackStateOperations.GetAttackCard(c.Cards.Single()).Kind).HandKeepValue).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First(); // Only the already-authorized viewer's revealed choices.
    internal void FinishInspectedHandSlash(ProgramSkillFrame f, bool issued)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new InspectedHandFinishedEvent(f.Id, issued));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { InspectedHandSlash = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome IInspectedHandSlashProgramHost.PayHpInspectHandThenDiscardOrSlash(ProgramSkillFrame f) => BeginInspectedHandSlash(f);
}
