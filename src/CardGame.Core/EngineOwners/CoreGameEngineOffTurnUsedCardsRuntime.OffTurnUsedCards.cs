#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOffTurnUsedCardsRuntime
{
    internal bool TracksOffTurnUsedCards => _contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.GiveOffTurnUsedCards) == true;

    internal bool HasOffTurnUsedCardGiftSource(CharacterState owner) => owner.IsAlive && _coreNationalSkillsOperations.GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.DiscardPileReceived).Any(b => b.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.GiveOffTurnUsedCards));
    internal int? OffTurnGiftActualTurnOwner() => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat) ? _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat : null;
    internal static bool IsOffTurnNativeCompletion(CardMovementRecord m, OffTurnUsedCardKind kind) => RuntimeSkills.RuntimeRules_Core_OffTurnUsedCards.IsOffTurnNativeCompletion(m, kind);
    internal void CaptureOffTurnUsedCards(long batchId, int turnNumber, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!TracksOffTurnUsedCards || !_coreMatchParticipantsPlayersAccess._players.Any(HasOffTurnUsedCardGiftSource) || turnNumber != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || OffTurnGiftActualTurnOwner()is null)
            return;
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().ToArray())
        {
            if (use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != use.SourceSeat || !(use.PhysicalCardIds ?? [use.CardId]).SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || !movements.Any(m => action.PhysicalCards.Any(c => c.CardId == m.CardId) && IsOffTurnNativeCompletion(m, OffTurnUsedCardKind.CardUse)))
                continue;
            CaptureOffTurnUsedAction(action, use.Id, OffTurnUsedCardKind.CardUse, batchId, movements);
        }

        foreach (var action in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).Where(a => a.Type == CardActionType.Response && movements.Any(m => a.PhysicalCards.Any(c => c.CardId == m.CardId))).Reverse().DistinctBy(a => a.ActionId).ToArray())
        {
            if (!TryGetOffTurnResponseUse(action, out var useId, out var kind) || !movements.Any(m => action.PhysicalCards.Any(c => c.CardId == m.CardId) && IsOffTurnNativeCompletion(m, kind)))
                continue;
            // A recycled entity in the same parent belongs only to the most recent response.
            if (action.PhysicalCards.Any(c => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId > action.ActionId && e.Action.PhysicalCards.Any(p => p.CardId == c.CardId))))
                continue;
            CaptureOffTurnUsedAction(action, useId, kind, batchId, movements);
        }
    }

    internal void CaptureOffTurnResponseUsedCards(CardActionContext action)
    {
        if (!TracksOffTurnUsedCards || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || !HasOffTurnUsedCardGiftSource(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]) || !TryGetOffTurnResponseUse(action, out var useId, out var kind) || kind != OffTurnUsedCardKind.NullificationUse)
            return;
        var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.LastOrDefault(b => b.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && b.Movements.Any(m => action.PhysicalCards.Any(c => c.CardId == m.CardId) && IsOffTurnNativeCompletion(m, kind)));
        if (batch is not null)
            CaptureOffTurnUsedAction(action, useId, kind, batch.Id, batch.Movements);
    }

    internal void CaptureOffTurnResponseUseOrigin(long owningUseId, CardActionContext action)
    {
        if (!TracksOffTurnUsedCards || action.Type != CardActionType.Response || action.ActorSeat != action.ProviderSeat && !_characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action) || action.RequesterSeat is not null || action.ResponderSeat != action.ActorSeat || action.ParentActionId is not null || action.OpponentSeat is not { } opponent || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == owningUseId)is not { Action: null } parent)
            return;
        OffTurnUsedCardKind kind;
        if (action.EffectiveKind == CardKind.Dodge && parent.CardAttack is { } attack && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? parent.CardKind) && attack.TargetSeat == action.ActorSeat && attack.SourceSeat == opponent)
            kind = OffTurnUsedCardKind.DodgeUse;
        else if (action.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } pending && pending.ParentFrameId == parent.Id && pending.SourceSeat == opponent)
            kind = OffTurnUsedCardKind.NullificationUse;
        else
            return;
        if (!_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.ActorSeat == action.ActorSeat && e.Action.ProviderSeat == action.ProviderSeat && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards)) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OffTurnResponseUseOriginEvent>().Any(e => e.ActionId == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OffTurnResponseUseOriginEvent(parent.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, opponent, parent.CardKind, kind));
    }

    internal bool TryGetOffTurnResponseUse(CardActionContext action, out long useId, out OffTurnUsedCardKind kind)
    {
        useId = 0;
        kind = default;
        if (action.Type != CardActionType.Response || action.ActorSeat != action.ProviderSeat && !_characterLuSuMobileShiOperations.IsShiLsAutonomousResponse(action) || action.RequesterSeat is not null || action.ResponderSeat != action.ActorSeat)
            return false;
        if (action.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { DelayedReturn: not null } delayed && _coreDelayedNullificationOperations.Value.HasNullificationEffectOwner(delayed) && action.ParentActionId is null && action.OpponentSeat == delayed.SourceSeat)
        {
            useId = delayed.Id;
            kind = OffTurnUsedCardKind.NullificationUse;
            return true;
        }

        var legacy = action.ParentActionId is null ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OffTurnResponseUseOriginEvent>().SingleOrDefault(e => e.ActionId == action.ActionId && e.ActorSeat == action.ActorSeat && e.ProviderSeat == action.ProviderSeat && e.OpponentSeat == action.OpponentSeat) : null;
        var parent = action.ParentActionId is { } parentActionId ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == parentActionId) : legacy is null ? null : _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Id == legacy.CardUseFrameId && u.CardKind == legacy.ParentKind);
        if (parent is null)
            return false;
        if (action.EffectiveKind == CardKind.Dodge && parent.CardAttack is { } attack && CoreNativeCardClassificationsRuntime.IsSlashCard(attack.EffectiveCardKind ?? parent.CardKind) && attack.TargetSeat == action.ActorSeat && attack.SourceSeat == action.OpponentSeat)
            kind = OffTurnUsedCardKind.DodgeUse;
        else if (action.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } pending && pending.ParentFrameId == parent.Id && pending.SourceSeat == action.OpponentSeat)
            kind = OffTurnUsedCardKind.NullificationUse;
        else
            return false;
        if (legacy is not null && legacy.Kind != kind)
            return false;
        useId = parent.Id;
        return true;
    }

    internal void CaptureOffTurnUsedAction(CardActionContext action, long useId, OffTurnUsedCardKind kind, long anchorBatchId, IReadOnlyList<CardMovementRecord> anchorMovements)
    {
        if (OffTurnGiftActualTurnOwner()is not { } actualTurnOwner || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(action.ActorSeat) || action.ActorSeat == actualTurnOwner || action.ActorSeat != action.ProviderSeat || action.RequesterSeat is not null || !HasOffTurnUsedCardGiftSource(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]) || action.PhysicalCards.Count == 0 || action.PhysicalCards.Any(c => c.CardId <= 0) || action.PhysicalCards.Select(c => c.CardId).Distinct().Count() != action.PhysicalCards.Count || action.PhysicalCards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(e => e.Action.ActionId == action.ActionId && e.Action.PhysicalCards.SequenceEqual(action.PhysicalCards)))
            return;
        var entities = new List<OffTurnUsedDiscardEntity>();
        foreach (var cost in action.PhysicalCards)
        {
            var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId);
            if (movement is null || movement.TurnNumber != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || movement.CardKind != cost.CardKind || !IsOffTurnNativeCompletion(movement, kind) || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.DiscardPile)
                continue;
            var nativeBatchId = anchorMovements.Contains(movement) ? anchorBatchId : _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.LastOrDefault(b => b.Movements.Contains(movement))?.Id;
            if (nativeBatchId is null)
                continue;
            entities.Add(new(cost.CardId, cost.CardKind, cost.From, nativeBatchId.Value, movement.Sequence));
        }

        if (entities.Count == 0 || !entities.Any(e => e.NativeBatchId == anchorBatchId))
            return;
        foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], SkillProgramTriggerWindow.DiscardPileReceived).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.GiveOffTurnUsedCards)))
        {
            var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId);
            if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OffTurnUsedMaterialDiscardedEvent>().Any(e => e.ActionId == action.ActionId && e.Source == source))
                continue;
            foreach (var entity in entities)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OffTurnUsedMaterialDiscardedEvent(anchorBatchId, useId, action.ActionId, action.ActorSeat, action.ProviderSeat, action.EffectiveKind, kind, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, actualTurnOwner, source, candidate.GameplayHash, entity));
        }
    }

    internal OffTurnUsedMaterialDiscardedEvent[] OffTurnUsedFacts(long batchId, CardConversionSource source, string hash) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OffTurnUsedMaterialDiscardedEvent>().Where(e => e.AnchorBatchId == batchId && e.Source == source && e.GameplayHash == hash).OrderBy(e => e.Entity.MovementSequence).ToArray();
    internal bool OffTurnUsedEntityStillAvailable(OffTurnUsedDiscardEntity e) => _cardZonesOperations._cardZones.GetLocation(e.CardId) == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == e.CardId)is { } last && last.Sequence == e.MovementSequence;
    internal int[] MatchingOffTurnUsedCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate c)
    {
        var facts = OffTurnUsedFacts(batch.Id, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash);
        if (!facts.Any(e => e.ActorSeat == c.OwnerSeat && e.ActualTurnNumber == batch.TurnNumber && e.ActualTurnOwnerSeat != c.OwnerSeat && OffTurnUsedEntityStillAvailable(e.Entity)))
            return[];
        return batch.Movements.Select((m, i) => (m, i)).Where(x => facts.Any(e => e.Entity.NativeBatchId == batch.Id && e.Entity.MovementSequence == x.m.Sequence)).Select(x => x.i).ToArray();
    }

    internal int[] OffTurnGiftRecipients(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count).Select(p => p.Seat).ToArray();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 970)]
    internal bool CanOfferOffTurnUsedCards(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (!_coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects.Any(e => e.Op == SkillProgramEffectOp.GiveOffTurnUsedCards))
            return true;
        return context is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } b } && b.TurnNumber == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && OffTurnGiftActualTurnOwner()is { } turnOwner && turnOwner != c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && OffTurnUsedFacts(b.Id, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash).All(e => e.ActualTurnOwnerSeat == turnOwner) && MatchingOffTurnUsedCardIndexes(b, c).Length > 0 && OffTurnGiftRecipients(c.OwnerSeat).Length > 0;
    }

    internal SkillProgramStepOutcome BeginOffTurnUsedCardGift(ProgramSkillFrame f)
    {
        if (f.OffTurnUsedCardGift is not null || f.WindowContext is not { MovementBatch: { } batch } context || !CanOfferOffTurnUsedCards(new(f.OwnerSeat, f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), context))
            throw new InvalidOperationException("An off-turn used-card gift requires its unpaid native discard opportunity.");
        var source = new CardConversionSource(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
        var facts = OffTurnUsedFacts(batch.Id, source, f.GameplayHash);
        var first = facts[0];
        var receipt = new ProgramOffTurnUsedCardGiftReceipt(f.InstructionIndex, context.ParentFrameId, batch.Id, first.CardUseFrameId, first.ActionId, first.EffectiveKind, first.Kind, first.ActualTurnNumber, first.ActualTurnOwnerSeat, source, f.GameplayHash, facts.Select(e => e.Entity).ToArray(), OffTurnGiftRecipients(f.OwnerSeat), OffTurnUsedCardGiftStage.ChoosingRecipient);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { OffTurnUsedCardGift = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OffTurnUsedCardGiftStartedEvent(f.Id, receipt.WindowFrameId, batch.Id, receipt.CardUseFrameId, receipt.ActionId, source, f.GameplayHash, receipt.OriginalEntities.Count));
        var choices = OffTurnUsedCardGiftChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "选择手牌数不大于你的其他角色，获得本次使用后仍在弃牌堆中的牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), receipt.CandidateSeats, f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> OffTurnUsedCardGiftChoices(ProgramSkillFrame f)
    {
        var r = f.OffTurnUsedCardGift!;
        var cards = r.OriginalEntities.Where(OffTurnUsedEntityStillAvailable).Select(e => e.CardId).ToArray();
        return Array.AsReadOnly(r.CandidateSeats.Select(seat => new PromptChoice(new($"off-turn-used-card-gift.frame-{(f.Id)}.seat-{(seat)}"), $"将本次使用后仍在弃牌堆中的牌交给 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}。", cards, [seat], new Dictionary<string, string> { ["program-action"] = "off-turn-used-card-gift", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) })).ToArray());
    }

    [ProgramAnswer("off-turn-used-card-gift")]
    internal void ResolveOffTurnUsedCardGift(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Off-turn gift lost its exact program.");
        var r = f.OffTurnUsedCardGift!;
        if (r.Stage != OffTurnUsedCardGiftStage.ChoosingRecipient || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || p.PlayerSeat != f.OwnerSeat || !OffTurnUsedCardGiftChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets) && c.Parameters.OrderBy(x => x.Key).SequenceEqual(selected.Parameters.OrderBy(x => x.Key))))
            throw new InvalidOperationException("Off-turn gift requires its exact published cards and recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var recipient = selected.Targets.Single();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || !OffTurnGiftRecipients(f.OwnerSeat).Contains(recipient) || selected.Cards.Count == 0 || _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber != r.ActualTurnNumber || OffTurnGiftActualTurnOwner() != r.ActualTurnOwnerSeat)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "本次使用的弃牌、技能来源或赠牌目标已失效。");
            return;
        }

        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()?.Sequence ?? 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null), OffTurnUsedCardGift = r with { Stage = OffTurnUsedCardGiftStage.GiftChildren, RecipientSeat = recipient, PaidCardIds = selected.Cards, Before = before } });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(selected.Cards, CardLocation.Hand(recipient), new($"skill-program.{(f.SkillId)}.off-turn-used-card-gift"), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, selected.Cards, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (batchId, moves) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { OffTurnUsedCardGift = active.OffTurnUsedCardGift!with { GiftBatchId = batchId, After = moves.Last().Sequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OffTurnUsedCardGiftPaidEvent(f.Id, r.ActionId, recipient, selected.Cards.Count, batchId, before, moves.Last().Sequence));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 110)]
    internal bool ResumeOffTurnUsedCardGift(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.OffTurnUsedCardGift is not { } r)
            return false;
        if (r.Stage == OffTurnUsedCardGiftStage.ChoosingRecipient)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OffTurnUsedCardGiftResolvedEvent(f.Id, r.ActionId, r.RecipientSeat!.Value, r.PaidCardIds!.Count, r.GiftBatchId));
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 90)]
    internal bool ReturnOffTurnUsedCardGiftMovement(ProgramSkillFrame f)
    {
        if (f.OffTurnUsedCardGift is null)
            return false;
        if (f.OffTurnUsedCardGift.Stage != OffTurnUsedCardGiftStage.GiftChildren || f.PendingMovementContinuation is null)
            throw new InvalidOperationException("Off-turn gift returned without its already-paid movement receipt.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    SkillProgramStepOutcome IOffTurnUsedCardsProgramHost.GiveOffTurnUsedCards(ProgramSkillFrame f) => BeginOffTurnUsedCardGift(f);
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 300)]
    internal int[]? SelectWindowIndexes_MatchingOffTurnUsedCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.GiveOffTurnUsedCards))
            return MatchingOffTurnUsedCardIndexes(batch, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 800)]
    internal void CaptureMovementBatchFactsCaptureOffTurnUsedCards(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureOffTurnUsedCards(batch.Id, batch.TurnNumber, movements);
}
