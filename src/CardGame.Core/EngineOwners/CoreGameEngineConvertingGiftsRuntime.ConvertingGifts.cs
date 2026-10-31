#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConvertingGiftsRuntime
{
    SkillProgramStepOutcome IConvertingGiftProgramHost.GiveSelectedOwnedCardAndDamage(ProgramSkillFrame f, int limit) => BeginConvertingGiftDamage(f, limit);
    SkillProgramStepOutcome IConvertingGiftProgramHost.ObserveDamageSourceHandAndGive(ProgramSkillFrame f, int limit) => BeginDamageSourceObservationGift(f, limit);
    SkillProgramStepOutcome IConvertingGiftProgramHost.DrawToHandCount(ProgramSkillFrame f, int seat, int limit, SkillProgramNumberExpression? expression, bool capAtTargetMaxHp) => DrawProgramToHandCount(f, seat, expression == SkillProgramNumberExpression.LivingPlayerCount ? _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive) : limit, capAtTargetMaxHp);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 110)]
    internal bool CanActivateConvertingGift(CharacterState owner, string skillId, ProgramInstructionFeatures features) => !features.HasOperation(SkillProgramEffectOp.GiveSelectedOwnedCardAndDamage) || _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(owner.Seat, skillId) == SkillPolarity.Yang;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeAdvancedPrerequisites, 20)]
    internal bool CanOfferConvertingGift(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramInstructionFeatures features) => !features.HasOperation(SkillProgramEffectOp.ObserveDamageSourceHandAndGive) || _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(candidate.OwnerSeat, candidate.SkillId) == SkillPolarity.Yin && context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == candidate.OwnerSeat && _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger?.Id == context.ParentFrameId && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsSourceLess: false, DamageAmount: > 0 } && context.SourceSeat is { } source && source != candidate.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0;
    internal SkillProgramStepOutcome DrawProgramToHandCount(ProgramSkillFrame f, int seat, int limit, bool capAtTargetMaxHp = false)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (capAtTargetMaxHp)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[seat].HasHpValue)
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "体力值未定义，无法计算此效果。");
                return SkillProgramStepOutcome.Continue;
            }

            limit = Math.Min(limit, _coreMatchParticipantsPlayersAccess._players[seat].MaxHp);
        }

        var count = Math.Max(0, limit - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count);
        return count == 0 ? SkillProgramStepOutcome.Continue : _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, seat, count, CoreGameEngineConvertingGiftsDefinitions.HandCountDrawReason);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.DrawToHandCount)]
    internal void ReturnHandCountDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var instruction = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions[frame.InstructionIndex - 1];
        var recipient = instruction.Target == SkillProgramEffectTarget.Owner ? frame.OwnerSeat : frame.SelectedTargetSeats.Count == 1 ? frame.SelectedTargetSeats[0] : -1;
        if (frame.NativeDraw is not null || instruction.Op != SkillProgramEffectOp.DrawToHandCount || recipient != outcome.RecipientSeat || outcome.RequestedCount is < 1 or > 16 || outcome.SequenceBefore > outcome.SequenceAfter || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || outcome.ActualDrawIds.Count > outcome.RequestedCount || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > outcome.SequenceBefore && move.Sequence <= outcome.SequenceAfter && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(recipient) && move.Reason == CoreGameEngineConvertingGiftsDefinitions.HandCountDrawReason).OrderBy(move => move.Sequence).Select(move => move.CardId).SequenceEqual(outcome.ActualDrawIds))
            throw new InvalidOperationException("A hand-count refill lost its original recipient or native draw-material return.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(frame.Id, recipient, null, SkillProgramCardSetVisibility.Private, CoreGameEngineConvertingGiftsDefinitions.HandCountDrawReason, 0, outcome.ActualDrawIds);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal SkillProgramStepOutcome BeginConvertingGiftDamage(ProgramSkillFrame f, int limit)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (active.ConvertingGift is not null || active.TriggerId is not null || active.SelectedCardIds is not [var cardId] || active.SelectedTargetSeats is not [var target] || !_coreProgramLifecycleOperations.GetProgramTargetSeats(active.OwnerSeat, SkillProgramTargetKind.OtherLivingHighestHand).Contains(target) || !OwnedGiftCard(active.OwnerSeat, cardId) || _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(active.OwnerSeat, active.SkillId) != SkillPolarity.Yang)
            throw new InvalidOperationException("Gift damage lost its accepted legal activation.");
        _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(active);
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        active = active with
        {
            ConvertingGift = new(false, "gift-movement", target, limit, [], cardId)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        MoveConvertingGiftCard(active, cardId);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(active.Id, active.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool OwnedGiftCard(int owner, int cardId)
    {
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        return location.OwnerSeat == owner && location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment;
    }

    internal void MoveConvertingGiftCard(ProgramSkillFrame f, int cardId)
    {
        var draft = f.ConvertingGift!;
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == cardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(draft.RecipientSeat), new("skill-program.converting-gift.give"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, from.OwnerSeat!.Value, draft.RecipientSeat, [cardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        var ordinal = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cardId && m.From == from && m.To == CardLocation.Hand(draft.RecipientSeat) && m.Reason.Value == "skill-program.converting-gift.give")?.Sequence;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = f.ConvertingGift!with { GiftCardId = cardId, GiftOrdinal = ordinal } });
        if (draft.Observe && ordinal is { } moved)
        {
            var obligation = new GiftHandRetentionObligation(++_coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionSequence, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, draft.RecipientSeat, cardId, moved, draft.HandLimit);
            _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Add(obligation);
            AppendGiftRetentionToActiveEnding(obligation);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GiftHandRetentionScheduledEvent(obligation));
        }
    }

    internal SkillProgramStepOutcome BeginDamageSourceObservationGift(ProgramSkillFrame f, int limit)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (f.ConvertingGift is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source } || source == f.OwnerSeat || f.WindowContext.TargetSeat != f.OwnerSeat || _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger?.Id != f.WindowContext.ParentFrameId || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false, DamageAmount: > 0 } || !_coreMatchParticipantsPlayersAccess._players[source].IsAlive || _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(f.OwnerSeat, f.SkillId) != SkillPolarity.Yin)
            throw new InvalidOperationException("Private observation lost its exact other damage source.");
        var ids = Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[source]).Select(c => c.Id).ToArray());
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = new(true, "observe", source, limit, ids) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPrivateHandObservedEvent(f.Id, f.OwnerSeat, source, ids.Count));
        PublishConvertingGift(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal CardSnapshot[] GetConvertingGiftPrivatelyViewedCards(int viewerSeat) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewerSeat && f.ConvertingGift is { Observe: true, Stage: "observe" }).SelectMany(f => f.ConvertingGift!.ObservedHandIds.Select(id => _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(f.ConvertingGift.RecipientSeat)).SingleOrDefault(c => c.Id == id))).Where(c => c is not null).Select(c => _coreSnapshotValueProjectionOperations.ToSnapshot(c!)).ToArray();
    internal IReadOnlyList<PromptChoice> ConvertingGiftChoices(ProgramSkillFrame f)
    {
        var d = f.ConvertingGift!;
        Dictionary<string, string> P(string action) => new()
        {
            ["program-action"] = "converting-gift",
            ["gift-action"] = action,
            ["frame-id"] = f.Id.ToString()
        };
        if (d.Stage == "observe")
            return[new(new($"converting-gift.{(f.Id)}.observed"), "已观看手牌，选择赠牌", [], [], P("observed"))];
        if (d.Stage == "give-choice")
            return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Select(c => new PromptChoice(new($"converting-gift.{(f.Id)}.give.{(c.Id)}"), $"交给 {(_coreMatchParticipantsPlayersAccess._players[d.RecipientSeat].Name)}【{(c.DisplayName)}】", [c.Id], [d.RecipientSeat], P("give"))).ToArray();
        if (d.Stage == "reward-choice")
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => new PromptChoice(new($"converting-gift.{(f.Id)}.reward.{(p.Seat)}"), $"令 {(p.Name)} 将手牌摸至{(d.HandLimit)}", [], [p.Seat], P("reward"))).Append(new(new($"converting-gift.{(f.Id)}.reward.skip"), "不发动摸牌奖励", [], [], P("reward-skip"))).ToArray();
        throw new InvalidOperationException("Gift stage has no player choices.");
    }

    internal void PublishConvertingGift(ProgramSkillFrame f)
    {
        var choices = ConvertingGiftChoices(f);
        if (choices.Count == 0)
        {
            FinishConvertingGift(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ConvertingGift!.Stage == "observe" ? $"{(skill.Name)}：观看伤害来源手牌。" : $"{(skill.Name)}：选择结算。", choices.SelectMany(c => c.Cards).ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.ObserveDamageSourceHandAndGive, SkillProgramEffectOp.GiveSelectedOwnedCardAndDamage)]
    internal PromptChoice SelectAiConvertingGift(PendingDecision decision, ProgramSkillFrame frame)
    {
        var draft = frame.ConvertingGift ?? throw new InvalidOperationException("The gift AI lost its native draft.");
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(frame.OwnerSeat);
        var self = view.Players.Single(player => player.Seat == frame.OwnerSeat);
        if (draft.Stage == "observe")
            return decision.Choices.Single();
        if (draft.Stage == "give-choice")
        {
            var owned = self.Hand.Concat(self.Equipment).ToDictionary(card => card.Id);
            return decision.Choices.Where(choice => choice.Cards is [var id] && owned.ContainsKey(id)).OrderBy(choice => CardCatalog.Get(owned[choice.Cards[0]].Kind).HandKeepValue + (self.Equipment.Any(card => card.Id == choice.Cards[0]) ? 8 : 0)).ThenBy(choice => choice.Cards[0]).First();
        }

        if (draft.Stage == "reward-choice")
        {
            return decision.Choices.OrderByDescending(choice => choice.Targets is [var seat] ? _coreAiBrainsAiBrainsAccess._aiBrains[frame.OwnerSeat].ScoreProgramTarget(view, seat, new(0, 0, 0, Math.Max(0, draft.HandLimit - view.Players.Single(player => player.Seat == seat).HandCount), 0, 0, false, false)) : 0).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
        }

        throw new InvalidOperationException("The gift AI has no published choice at this native cursor.");
    }

    [ProgramAnswer("converting-gift")]
    internal void ResolveConvertingGiftChoice(PromptChoice choice)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var d = f.ConvertingGift ?? throw new InvalidOperationException("Gift choice lost its owner draft.");
        if (choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString() || !ConvertingGiftChoices(f).Any(c => c.Id == choice.Id))
            throw new InvalidOperationException("Gift choice is stale.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            FinishConvertingGift(f);
            return;
        }

        if (d.Stage == "observe")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = d with { Stage = "give-choice", ObservedHandIds = [] } });
            PublishConvertingGift(f);
            return;
        }

        if (d.Stage == "give-choice")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[d.RecipientSeat].IsAlive || choice.Cards is not [var id] || !OwnedGiftCard(f.OwnerSeat, id))
            {
                FinishConvertingGift(f);
                return;
            }

            _coreProgramConversionPolarityOperations.CommitProgramConversionPolarity(f);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = f.ConvertingGift!with { Stage = "gift-movement", ObservedHandIds = [], GiftCardId = id } });
            MoveConvertingGiftCard(f, id);
            if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (d.Stage == "reward-choice")
        {
            if (choice.Parameters.GetValueOrDefault("gift-action") == "reward-skip")
            {
                FinishConvertingGift(f);
                return;
            }

            if (choice.Targets is not [var recipient] || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
                throw new InvalidOperationException("Gift death reward target is invalid.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = d with { Stage = "reward-movement" } });
            DrawProgramToHandCount(f, recipient, d.HandLimit);
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id)
                _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        throw new InvalidOperationException("Invalid gift response stage.");
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 930)]
    internal bool TryResumeConvertingGift(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != frameId || f.ConvertingGift is not { } d)
            return false;
        if (f.PendingMovementContinuation is not null)
            return false;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        d = f.ConvertingGift!;
        if (d.Stage is "observe" or "give-choice" or "reward-choice")
            return true;
        if (f.AttackAttempt is not null)
            return false;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
        {
            FinishConvertingGift(f);
            return true;
        }

        if (d.Stage == "gift-movement")
        {
            if (d.Observe || d.GiftOrdinal is null || !_coreMatchParticipantsPlayersAccess._players[d.RecipientSeat].IsAlive)
            {
                FinishConvertingGift(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = d with { Stage = "damage" } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, d.RecipientSeat, 1);
            return true;
        }

        if (d.Stage == "damage")
        {
            if (!d.ExactDamageDeath)
            {
                FinishConvertingGift(f);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ConvertingGift = d with { Stage = "reward-choice" } });
            PublishConvertingGift(f);
            return true;
        }

        if (d.Stage == "reward-movement")
        {
            FinishConvertingGift(f);
            return true;
        }

        throw new InvalidOperationException("Unrecognized converting gift cursor.");
    }

    internal void FinishConvertingGift(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ConvertingGift = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void MarkConvertingGiftExactDeath(long deathFrameId, int victimSeat)
    {
        foreach (var due in _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Where(d => d.Source.OwnerSeat == victimSeat).ToArray())
        {
            _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Remove(due);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GiftHandRetentionConsumedEvent(due.Id, victimSeat, false));
        }

        var death = _resolutionStackOperations._resolutionStack.OfType<DeathFrame>().Single(f => f.Id == deathFrameId);
        var dying = _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(f => f.Id == death.ParentFrameId);
        if (dying is not { Continuation: DyingContinuationKind.Damage })
            return;
        var damage = _resolutionStackOperations._resolutionStack.OfType<DamageFrame>().SingleOrDefault(f => f.Id == dying.ParentFrameId);
        if (damage is null || damage.TargetSeat != victimSeat || damage.Amount <= 0)
            return;
        var program = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == damage.ParentFrameId);
        if (program?.ConvertingGift is not { Observe: false, Stage: "damage" } d || d.RecipientSeat != victimSeat || damage.SourceSeat != program.OwnerSeat)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(program.Id, program with { ConvertingGift = d with { ExactDamageDeath = true } });
    }

    internal static TurnEndingBoundaryItem CreateGiftRetentionEndingItem(GiftHandRetentionObligation due) => RuntimeSkills.RuntimeRules_Core_ConvertingGifts.CreateGiftRetentionEndingItem(due);
    internal void AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items)
    {
        foreach (var due in _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Where(d => d.CreatedTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber).OrderBy(d => d.Id))
            items.Add(CreateGiftRetentionEndingItem(due));
    }

    internal void AppendGiftRetentionToActiveEnding(GiftHandRetentionObligation due)
    {
        var frame = _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(f => f.TurnNumber == due.CreatedTurn && f.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        if (frame is null || frame.Items.Any(item => item.RetentionId == due.Id))
            return;
        // Keep the running item and completed prefix stable while its child adds a new due.
        var suffix = frame.Items.Skip(frame.ItemIndex + 1).Append(CreateGiftRetentionEndingItem(due)).OrderBy(item => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, item.Candidate?.OwnerSeat ?? item.RetentionOwnerSeat!.Value)).ThenByDescending(item => item.Priority).ThenBy(item => item.StableIdentity, StringComparer.Ordinal);
        var items = Array.AsReadOnly(frame.Items.Take(frame.ItemIndex + 1).Concat(suffix).ToArray());
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame with { Items = items });
    }

    internal bool ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item)
    {
        var due = _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.SingleOrDefault(d => d.Id == item.RetentionId);
        if (due is null)
        {
            _coreProgramLifecycleOperations.AdvanceTurnEndingBoundaryCursor(frame);
            return false;
        }
        _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Remove(due);
        var applied = _coreMatchParticipantsPlayersAccess._players[due.Source.OwnerSeat].IsAlive && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > due.GiftOrdinal && m.CardId == due.CardId && m.From == CardLocation.Hand(due.RecipientSeat) && m.To != m.From);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GiftHandRetentionConsumedEvent(due.Id, due.Source.OwnerSeat, applied));
        if (applied)
        {
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[due.Source.OwnerSeat], Math.Max(0, due.HandLimit - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[due.Source.OwnerSeat]).Count), true, new("skill-program.gift-retention.draw"));
            if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                return true;
        }

        _coreProgramLifecycleOperations.AdvanceTurnEndingBoundaryCursor(frame);
        return false;
    }

    internal void ExpireGiftRetentionObligations()
    {
        foreach (var due in _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Where(d => d.CreatedTurn <= _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber).ToArray())
        {
            _coreGameEngineConvertingGiftsGiftHandRetentionObligationsGiftHandRetentionSequenceAccess._giftHandRetentionObligations.Remove(due);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GiftHandRetentionConsumedEvent(due.Id, due.Source.OwnerSeat, false));
        }
    }
}
