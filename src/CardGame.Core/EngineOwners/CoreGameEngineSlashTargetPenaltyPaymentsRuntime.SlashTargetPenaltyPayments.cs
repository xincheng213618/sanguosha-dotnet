#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSlashTargetPenaltyPaymentsRuntime
{
    internal string SlashTargetPenaltyCostReason(ProgramSkillFrame f, bool recast) => recast ? CardMoveReasons.RecastDiscard.Value : $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.RequireTargetDiscardOrEquipmentRecast)}";
    internal long SlashTargetPenaltySequence => _coreCardMovementJournalCardMovementsAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;

    internal bool TargetPenaltyStillMayIssue(ProgramSkillFrame f) => _coreSlashTargetPenaltyWindowsOperations.SlashTargetPenaltyProgramMatches(f) && f.WindowContext!.SlashTargetPenalty is { } r && _coreSlashTargetPenaltyWindowsOperations.CanRunSlashTargetPenalty(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext.OccurrenceIndex), f.WindowContext);
    internal SkillProgramStepOutcome RequireTargetDiscardOrEquipmentRecast(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreSlashTargetPenaltyWindowsOperations.SlashTargetPenaltyProgramMatches(f) || f.InstructionIndex != 1 || f.WindowContext!.SlashTargetPenalty is not { } r)
            throw new InvalidOperationException("The target penalty lost its issued instruction and original target.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SlashTargetPenaltyDraft = f.SlashTargetPenaltyDraft ?? new(r, SlashTargetPenaltyStage.Offered), ReexecuteParticipantInstruction = true });
        if (!TargetPenaltyStillMayIssue(f))
        {
            FinishTargetPenalty(f, false);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var choices = TargetPenaltyChoices(f);
        if (choices.Count == 0)
        {
            FinishTargetPenalty(f, true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, r.TargetSeat, "追击：弃置一张手牌或装备牌，或重铸装备区里的所有牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.TargetSeat,
            SkillPrompt = new(f.SkillId, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Name, "追击 · 指定目标", _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> TargetPenaltyChoices(ProgramSkillFrame f)
    {
        if (f.SlashTargetPenaltyDraft is not { Stage: SlashTargetPenaltyStage.Offered } d || !TargetPenaltyStillMayIssue(f))
            return[];
        var target = d.Identity.TargetSeat;
        var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, target, target, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c =>
        {
            var p = c.Parameters.ToDictionary(x => x.Key, x => x.Value, StringComparer.Ordinal);
            p["program-action"] = "slash-target-penalty";
            p["penalty-option"] = "discard";
            return new PromptChoice(c.Id, c.Description, c.Cards, c.Targets, p);
        }).ToList();
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).ToArray();
        // General-generated weapons are destroyed by MoveCard rather than recast.
        // Do not advertise a partially payable 'all equipment' branch.
        if (equipment.Length > 0 && equipment.All(c => !c.IsGeneralWeapon))
            choices.Add(new(new($"target-penalty.{(f.Id)}.recast"), $"重铸全部{(equipment.Length)}张装备牌", [], [], new Dictionary<string, string> { ["program-action"] = "slash-target-penalty", ["penalty-option"] = "recast", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) }));
        return Array.AsReadOnly(choices.ToArray());
    }

    [ProgramAnswer("slash-target-penalty")]
    internal void ResolveTargetPenaltyChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.SlashTargetPenaltyDraft is not { Stage: SlashTargetPenaltyStage.Offered } d || !_coreSlashTargetPenaltyWindowsOperations.SlashTargetPenaltyProgramMatches(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != d.Identity.TargetSeat || selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !TargetPenaltyChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("A target penalty requires its current published target-owned choice.");
        var recast = selected.Parameters.GetValueOrDefault("penalty-option") == "recast";
        IReadOnlyList<Card> cards;
        IReadOnlyList<CardLocation> from;
        if (recast)
        {
            cards = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[d.Identity.TargetSeat]).ToArray();
            from = cards.Select(_ => CardLocation.Equipment(d.Identity.TargetSeat)).ToArray();
        }
        else
        {
            if (!Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone) || zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), out var slot))
                throw new InvalidOperationException("The target-owned discard lost its exact current slot.");
            var location = new CardLocation(zone, d.Identity.TargetSeat);
            var pool = _cardZonesOperations._cardZones.CardsAt(location);
            if (slot < 0 || slot >= pool.Count)
                throw new InvalidOperationException("The selected target card is no longer in its published slot.");
            cards = [pool[slot]];
            from = [location];
        }

        if (cards.Count == 0 || recast && cards.Any(c => c.IsGeneralWeapon))
            throw new InvalidOperationException("The selected equipment recast is not fully payable.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var before = SlashTargetPenaltySequence;
        if (recast)
            f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, d.Identity.TargetSeat, cards.Select(c => c.Id).ToArray());
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SlashTargetPenaltyDraft = d with { Stage = SlashTargetPenaltyStage.CostChildren, Recast = recast, SequenceBefore = before, PaidCardIds = cards.Select(c => c.Id).ToArray(), PaidFrom = from }, PendingMovementContinuation = new(d.Identity.TargetSeat, 0, null) });
        if (recast)
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.Equipment(d.Identity.TargetSeat), CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        else
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, d.Identity.TargetSeat, cards[0], from[0], new(SlashTargetPenaltyCostReason(f, false)));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var after = SlashTargetPenaltySequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SlashTargetPenaltyDraft = f.SlashTargetPenaltyDraft!with { SequenceAfter = after } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetPenaltyPaidEvent(f.Id, d.Identity.CardUseFrameId, d.Identity.ActorSeat, d.Identity.TargetSeat, recast, Array.AsReadOnly(cards.Select(c => c.Id).ToArray()), before, after));
        ContinueTargetPenaltyChildren(f);
    }

    internal bool ValidTargetPenaltyPayment(ProgramSkillFrame f)
    {
        if (!_coreSlashTargetPenaltyWindowsOperations.SlashTargetPenaltyProgramMatches(f) || f.InstructionIndex != 1 || f.SlashTargetPenaltyDraft is not { } d || d.Identity != f.WindowContext!.SlashTargetPenalty || d.Stage == SlashTargetPenaltyStage.Offered || d.PaidCardIds.Count == 0 || d.PaidCardIds.Count != d.PaidFrom.Count || d.PaidCardIds.Distinct().Count() != d.PaidCardIds.Count || !d.Recast && d.PaidCardIds.Count != 1 || d.SequenceAfter <= d.SequenceBefore)
            return false;
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter).ToArray();
        var reason = SlashTargetPenaltyCostReason(f, d.Recast);
        if (moves.Length != d.PaidCardIds.Count)
            return false;
        for (var i = 0; i < moves.Length; i++)
            if (moves[i].CardId != d.PaidCardIds[i] || moves[i].From != d.PaidFrom[i] || moves[i].From.OwnerSeat != d.Identity.TargetSeat || moves[i].From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || moves[i].To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreCardAttackStateOperations.GetAttackCard(moves[i].CardId), moves[i].From, (moves[i].From.Zone == CardZoneKind.Equipment && _coreCardAttackStateOperations.GetAttackCard(moves[i].CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile)) || moves[i].Reason.Value != reason || d.Recast && (!_coreEquipmentContentOperations.EquipmentRules.IsEquipment(moves[i].CardKind) || moves[i].From.Zone != CardZoneKind.Equipment))
                return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SlashTargetPenaltyPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.CardUseFrameId == d.Identity.CardUseFrameId && e.ActorSeat == d.Identity.ActorSeat && e.TargetSeat == d.Identity.TargetSeat && e.Recast == d.Recast && e.CardIds.SequenceEqual(d.PaidCardIds) && e.SequenceBefore == d.SequenceBefore && e.SequenceAfter == d.SequenceAfter) == 1;
    }

    internal void ContinueTargetPenaltyChildren(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!ValidTargetPenaltyPayment(f))
            throw new InvalidOperationException("The penalty lost its exact completed cost ledger.");
        if (f.PendingMovementContinuation is not null && (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id)))
            return;
        if (f.PendingMovementContinuation is not null)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 600)]
    internal bool ResumeTargetPenalty(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.SlashTargetPenaltyDraft is not { } d)
            return false;
        if (!_coreSlashTargetPenaltyWindowsOperations.SlashTargetPenaltyProgramMatches(f) || f.InstructionIndex != 1 || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash || ProgramInstructionResolver.Default.Resolve(f, p).Instructions is not [{ Op: SkillProgramEffectOp.RequireTargetDiscardOrEquipmentRecast }])
            throw new InvalidOperationException("An issued target penalty cannot bypass unrelated source ownership.");
        if (d.Stage == SlashTargetPenaltyStage.Offered)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return true;
            RequireTargetDiscardOrEquipmentRecast(f);
            return true;
        }

        if (!ValidTargetPenaltyPayment(f))
            throw new InvalidOperationException("The resumed target penalty lost its paid receipt.");
        if (f.PendingMovementContinuation is not null)
        {
            ContinueTargetPenaltyChildren(f);
            return true;
        }

        if (d.Stage == SlashTargetPenaltyStage.CostChildren)
        {
            if (!d.Recast)
            {
                FinishTargetPenalty(f, true);
                return true;
            }

            // Paying all equipment issues the target's recast action. Its reward
            // no longer depends on the attacker's subsequently suppressed source.
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[d.Identity.TargetSeat].IsAlive)
            {
                PublishTargetPenaltyRecast(f, 0);
                FinishTargetPenalty(f, false);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SlashTargetPenaltyDraft = d with { Stage = SlashTargetPenaltyStage.RewardChildren, DrawAttempted = true }, PendingMovementContinuation = new(d.Identity.TargetSeat, 0, null) });
            var rewardBefore = SlashTargetPenaltySequence;
            var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[d.Identity.TargetSeat], d.PaidCardIds.Count, true, CardMoveReasons.RecastDraw);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var rewardAfter = SlashTargetPenaltySequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SlashTargetPenaltyDraft = f.SlashTargetPenaltyDraft!with { ActualDrawCount = drawn.Count, RewardBefore = rewardBefore, RewardAfter = rewardAfter } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetPenaltyDrawIssuedEvent(f.Id, d.Identity.TargetSeat, d.PaidCardIds.Count, drawn.Count, rewardBefore, rewardAfter));
            PublishTargetPenaltyRecast(f, drawn.Count);
            ContinueTargetPenaltyChildren(f);
            return true;
        }

        if (d.Stage == SlashTargetPenaltyStage.RewardChildren)
        {
            FinishTargetPenalty(f, true);
            return true;
        }

        throw new InvalidOperationException("A completed target penalty retained a live draft.");
    }

    internal void PublishTargetPenaltyRecast(ProgramSkillFrame f, int actual)
    {
        var d = f.SlashTargetPenaltyDraft!;
        for (var i = 0; i < d.PaidCardIds.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(d.Identity.TargetSeat, d.PaidCardIds[i], _coreCardAttackStateOperations.GetAttackCard(d.PaidCardIds[i]).Kind, i < actual ? 1 : 0));
        _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), actual);
    }

    internal void FinishTargetPenalty(ProgramSkillFrame supplied, bool completed)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var d = f.SlashTargetPenaltyDraft!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SlashTargetPenaltyFinishedEvent(f.Id, d.Identity.CardUseFrameId, d.Identity.TargetSeat, d.Recast, d.ActualDrawCount, completed));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SlashTargetPenaltyDraft = null, PendingMovementContinuation = null, ReexecuteParticipantInstruction = false });
        _coreSkillProgramsOperations.FinishProgramSkill(f, completed);
    }

    [ProgramContinuation(ProgramContinuationKind.MovementTail, 30)]
    internal bool ReturnTargetPenaltyMovement(ProgramSkillFrame f)
    {
        if (f.SlashTargetPenaltyDraft is not { Stage: SlashTargetPenaltyStage.CostChildren or SlashTargetPenaltyStage.RewardChildren })
            return false;
        if (f.PendingMovementContinuation is null || !ValidTargetPenaltyPayment(f))
            throw new InvalidOperationException("The target penalty movement lost its original paid parent.");
        ContinueTargetPenaltyChildren(f);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.RequireTargetDiscardOrEquipmentRecast)]
    internal PromptChoice SelectAiTargetPenalty(PendingDecision decision, ProgramSkillFrame f)
    {
        // Choices expose only the chooser's own cards and public equipment.
        var target = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).ToArray();
        var recast = decision.Choices.SingleOrDefault(c => c.Parameters.GetValueOrDefault("penalty-option") == "recast");
        var discard = decision.Choices.Where(c => c.Parameters.GetValueOrDefault("penalty-option") == "discard").OrderBy(c => c.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(c.Cards[0]), target) : 10).ThenBy(c => c.Id.Value, StringComparer.Ordinal).FirstOrDefault();
        var recastCost = equipment.Sum(c => CoreNativeAiDecisionsRuntime.GetKeepValue(c, target)) - equipment.Length * 3 - (target.Hp < target.MaxHp && equipment.Any(c => c.Kind == CardKind.SilverLion) ? 4 : 0);
        return recast is not null && (discard is null || recastCost < (discard.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreCardAttackStateOperations.GetAttackCard(discard.Cards[0]), target) : 10)) ? recast : discard ?? recast ?? throw new InvalidOperationException("A target penalty has no published legal choice.");
    }

    SkillProgramStepOutcome ISlashTargetPenaltyProgramHost.RequireTargetDiscardOrEquipmentRecast(ProgramSkillFrame f) => RequireTargetDiscardOrEquipmentRecast(f);
}
