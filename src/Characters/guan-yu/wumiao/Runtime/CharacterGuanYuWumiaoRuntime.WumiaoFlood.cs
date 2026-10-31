#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuWumiaoRuntime
{
    [ProgramCardAction(ProgramCardActionPhase.BeforeProgram, 28510)]
    internal void AddWumiaoFloodNaturalActions(List<LegalAction> actions, CharacterState actor, IReadOnlyList<Card> playable, int? selectedPhysicalCardId = null)
    {
        foreach (var card in playable.Where(card => card.Kind == CardKind.WumiaoFlood && (selectedPhysicalCardId is null || card.Id == selectedPhysicalCardId) && !_coreCardConversionsOperations.HasProgramCardIdentity(actor, card)))
            foreach (GameEngine.ProgramOrdinaryTrickUseOption option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, CardKind.WumiaoFlood, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card), enforceUsePermission: true, physicalCardIds: [card.Id]))
                actions.Add(new LegalAction(option.ActionKind, card.Id, option.TargetSeats.Count == 1 ? option.TargetSeats[0] : null, option.Description, TargetSeats: option.TargetSeats));
    }

    internal bool TryExecuteWumiaoFloodNaturalUse(CharacterState actor, Card card, LegalAction action)
    {
        if (card.Kind != CardKind.WumiaoFlood || action.Kind != LegalActionKind.WumiaoFlood || action.ConversionSource is not null || action.PlayedCardKind is not null)
            return false;
        if (!_coreNativeLegalActionsOperations.BuildLegalActions(actor, selectedPhysicalCardId: card.Id).Any(candidate => candidate.Kind == action.Kind && candidate.CardId == action.CardId && candidate.ConversionSource is null && candidate.PlayedCardKind is null && candidate.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("The physical Wumiao Flood lost its actual ordered targets.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, action.TargetSeats, LegalActionKind.WumiaoFlood);
        return true;
    }

    internal void ResolveWumiaoFloodEffect(NullificationWindowFrame pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != pending.ParentFrameId || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.WumiaoFlood || pending.ActionKind != LegalActionKind.WumiaoFlood || pending.EffectCardKind != CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || use.WumiaoFlood is not null || CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending)is not { } target || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != target || use.Action is not { Type: CardActionType.Use } action)
            throw new InvalidOperationException("Wumiao Flood lost its exact resolved target and owning use.");
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
            return;
        }

        var receipt = new WumiaoFloodReceipt(action.ActionId, pending.Id, target, use.TargetIndex, _characterFeiYiOlOperations.NativeExtraEffectRoundOrdinal(use.Id), WumiaoFloodStage.AwaitingDamage)
        {
            AwaitingProgramFrameId = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { } parent && CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(parent) ? parent.Id : null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { WumiaoFlood = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodEffectStartedEvent(use.Id, action.ActionId, pending.Id, target, use.TargetIndex, receipt.ExtraEffectOrdinal));
        if (!_coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(use.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(use.CardId, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), use.SourceSeat, target));
        var card = _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending);
        NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, target, card, playedCardKind: CardKind.WumiaoFlood, damageNatureOverride: DamageNature.Thunder, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(use.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal bool TryCompleteWumiaoFloodAttack(NativeOwners.CardAttackHandle attack)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { WumiaoFlood: { } receipt } use)
            return false;
        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.WumiaoFlood || attack.EffectiveCardKind != CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || use.Action?.ActionId != receipt.ActionId || attack.CardUserSeat != use.SourceSeat || use.TargetIndex != receipt.TargetIndex || _coreDyingCursorRuntimeOperations.Value.ActiveDying is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id)
            throw new InvalidOperationException("Wumiao Flood lost its completed damage subtree.");
        if (receipt.Stage == WumiaoFloodStage.FinishingAttack)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { WumiaoFlood = null });
            return false;
        }

        if (receipt.Stage != WumiaoFloodStage.AwaitingDamage)
            throw new InvalidOperationException("Wumiao Flood returned native damage twice.");
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = null;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { WumiaoFlood = receipt with { Stage = WumiaoFloodStage.AfterDamage } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodDamageReturnedEvent(use.Id, receipt.NullificationFrameId, receipt.TargetSeat, receipt.TargetIndex));
        ContinueWumiaoFlood(use.Id);
        return true;
    }

    internal IReadOnlyList<Card> WumiaoFloodDiscardable(int target) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target])).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(target, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal void BeginWumiaoFloodDiscard(CardUseFrame use)
    {
        var receipt = use.WumiaoFlood!;
        var choices = WumiaoFloodDiscardable(receipt.TargetSeat).Select(card => new PromptChoice(new ChoiceId($"wumiao-flood.{(use.Id)}.{(receipt.NullificationFrameId)}.discard-{(card.Id)}"), $"弃置【{(card.DisplayName)}】", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "wumiao-flood-discard", ["frame-id"] = use.Id.ToString(CultureInfo.InvariantCulture) })).ToArray();
        if (choices.Length == 0)
            throw new InvalidOperationException("Wumiao Flood cannot ask for an unavailable discard.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, receipt.TargetSeat, "【水淹七军】请弃置一张牌。", choices.SelectMany(c => c.Cards).ToArray(), [], use.SourceSeat, CardKind.WumiaoFlood)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal bool HasWumiaoFloodDiscardPrompt() => _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { WumiaoFlood: { Stage: WumiaoFloodStage.ChoosingDiscard } r } use && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IncomingCard: CardKind.WumiaoFlood } prompt && prompt.PlayerSeat == r.TargetSeat && prompt.SourceSeat == use.SourceSeat;
    [ProgramAnswer("wumiao-flood-discard")]
    internal void ResolveWumiaoFloodDiscard(PromptChoice selected)
    {
        if (!HasWumiaoFloodDiscardPrompt() || _resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { WumiaoFlood: { } r } use || selected.Cards is not [var cardId] || selected.Targets.Count != 0 || selected.Parameters.GetValueOrDefault("frame-id") != use.Id.ToString(CultureInfo.InvariantCulture) || !_corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Any(choice => choice.Id == selected.Id && choice.Cards.SequenceEqual(selected.Cards)) || WumiaoFloodDiscardable(r.TargetSeat).SingleOrDefault(card => card.Id == cardId)is not { } card)
            throw new InvalidOperationException("Wumiao Flood changed its exact target's mandatory discard.");
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var grains = _coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx && card.Kind == CardKind.WoodenOx && from == CardLocation.Equipment(r.TargetSeat) ? _cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(r.TargetSeat)).Select(c => new WumiaoFloodGrainMaterial(c.Id, c.Kind, CardLocation.WoodenOxGrain(r.TargetSeat))).ToArray() : [];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { WumiaoFlood = r with { Stage = WumiaoFloodStage.DiscardChildren, DiscardCardId = cardId, DiscardFrom = from, DiscardPrintedKind = card.Kind, DiscardSequenceBefore = before, DiscardSequenceAfter = before, GrainMaterials = grains, DiscardNativeDestruction = from.Zone == CardZoneKind.Equipment && (card.IsGeneralWeapon || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, from, CardLocation.DiscardPile) || _characterDiaoChanLeOperations.IsDiouCarrier(card.Id)) } });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(r.TargetSeat, r.TargetSeat, card, from, CharacterGuanYuWumiaoDefinitions.WumiaoFloodDiscardReason, movement =>
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, current => current with { WumiaoFlood = current.WumiaoFlood!with { DiscardSequence = movement.Sequence, DiscardTo = movement.To } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodDiscardPaidEvent(use.Id, r.NullificationFrameId, r.TargetSeat, cardId, from, movement.Sequence));
        });
        SealWumiaoFloodDiscardInvoice(use.Id);
        ContinueWumiaoFlood(use.Id);
    }

    internal void SealWumiaoFloodDiscardInvoice(long useId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!;
        var r = use.WumiaoFlood!;
        if (r.DiscardInvoiceSealed || r.DiscardSequence <= r.DiscardSequenceBefore || r.DiscardCardId is null || r.DiscardFrom is null || r.DiscardTo is null || r.DiscardPrintedKind is null)
            throw new InvalidOperationException("Wumiao Flood cannot seal an unissued or repeated native discard invoice.");
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var primary = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence == r.DiscardSequence);
        var movements = new List<CardMovementRecord>
        {
            primary
        };
        foreach (var grain in r.GrainMaterials)
            movements.Add(_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > r.DiscardSequence && m.Sequence <= after && m.CardId == grain.CardId && m.CardKind == grain.PrintedKind && m.From == grain.From && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && (m.To == CardLocation.DiscardPile || _coreHellMengpoProgramsOperations.IsHellShiyouPaidDiscardMovement(m, grain.From, CardMoveReasons.WoodenOxGrainDiscard))));
        var invoice = movements.OrderBy(m => m.Sequence).Select(m => new WumiaoFloodDiscardMovement(m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)).ToArray();
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId == r.AwaitingProgramFrameId && b.Movements.Count > 0 && b.Movements.All(m => invoice.Any(e => WumiaoFloodDiscardEntryMatches(e, m)))).OrderBy(b => b.Id).Select(b => new WumiaoFloodDiscardBatch(b.Id, b.ParentBatchId, b.Movements.Count)).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { WumiaoFlood = r with { DiscardInvoiceSealed = true, DiscardSequenceAfter = after, DiscardMovements = invoice, DiscardBatches = batches } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodDiscardInvoiceSealedEvent(use.Id, r.NullificationFrameId, r.DiscardSequence, r.DiscardSequenceBefore, after, r.GrainMaterials.Count, invoice.Length, batches.Length));
        for (var i = 0; i < invoice.Length; i++)
        {
            var m = invoice[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodDiscardMaterialPaidEvent(use.Id, r.NullificationFrameId, i, m.CardId, m.PrintedKind, m.From, m.To, m.Reason, m.MovementSequence));
        }

        foreach (var b in batches)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodDiscardBatchIssuedEvent(use.Id, r.NullificationFrameId, b.BatchId, b.ParentBatchId, b.MovementCount));
    }

    internal static bool WumiaoFloodDiscardEntryMatches(WumiaoFloodDiscardMovement entry, CardMovementRecord movement) => RuntimeSkills.RuntimeRules_guan_yu_wumiao_WumiaoFlood.WumiaoFloodDiscardEntryMatches(entry, movement);
    internal bool IsWumiaoFloodPrimaryDiscardMovement(WumiaoFloodReceipt r, CardMovementRecord movement) => movement.Sequence == r.DiscardSequence && movement.CardId == r.DiscardCardId && movement.CardKind == r.DiscardPrintedKind && movement.From == r.DiscardFrom && movement.To == r.DiscardTo && movement.Reason == CharacterGuanYuWumiaoDefinitions.WumiaoFloodDiscardReason && (r.DiscardNativeDestruction ? movement.From.Zone == CardZoneKind.Equipment && movement.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(movement.CardId).IsGeneralWeapon || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(_coreAdvancedLifecycleOperations.GetAdvancedCard(movement.CardId), movement.From, CardLocation.DiscardPile) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(movement)) : movement.To == CardLocation.DiscardPile || _coreHellMengpoProgramsOperations.IsHellShiyouPaidDiscardMovement(movement, movement.From, CharacterGuanYuWumiaoDefinitions.WumiaoFloodDiscardReason));
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeDeclaration, 28510)]
    internal bool AdvanceWumiaoFloodDiscardAi()
    {
        if (!HasWumiaoFloodDiscardPrompt() || _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat].IsHuman)
            return false;
        ResolveWumiaoFloodDiscard(_corePendingDecisionPolicyPipelineOperations._pendingDecision.Choices.OrderBy(choice => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single()).Kind).HandKeepValue).ThenBy(choice => choice.Cards.Single()).First());
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return true;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 50, typeof(NativeFrameRouteConditions), "WumiaoFlood")]
    internal void ContinueWumiaoFlood(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { WumiaoFlood: { } r } use && use.Id == id)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
                return;
            AssertWumiaoFlood(use);
            if (r.Stage is WumiaoFloodStage.AwaitingDamage or WumiaoFloodStage.FinishingAttack)
                return;
            if (r.Stage == WumiaoFloodStage.AfterDamage)
            {
                var next = !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None ? WumiaoFloodStage.FinishingAttack : r.TargetIndex == 0 && WumiaoFloodDiscardable(r.TargetSeat).Count > 0 ? WumiaoFloodStage.ChoosingDiscard : r.TargetIndex == 0 ? WumiaoFloodStage.FinishingAttack : WumiaoFloodStage.Drawing;
                _coreRuntimeOperations.ReplaceRuntimeTop(use with { WumiaoFlood = r with { Stage = next } });
                if (next != WumiaoFloodStage.FinishingAttack)
                    continue;
            }
            else if (r.Stage == WumiaoFloodStage.ChoosingDiscard)
            {
                BeginWumiaoFloodDiscard(use);
                return;
            }
            else if (r.Stage is WumiaoFloodStage.DiscardChildren or WumiaoFloodStage.DrawingChildren)
            {
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.WumiaoFlood) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.WumiaoFlood) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.WumiaoFlood) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
                    return;
                use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
                _coreRuntimeOperations.ReplaceRuntimeTop(use with { WumiaoFlood = use.WumiaoFlood!with { Stage = WumiaoFloodStage.FinishingAttack } });
            }
            else if (r.Stage == WumiaoFloodStage.Drawing)
            {
                if (use.NativeDraw is null)
                    _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { NativeDraw = new NativeDrawProducerReceipt(0, 1, null, SkillProgramCardSetVisibility.Private, CardMoveReasons.Draw) { Recipients = [r.TargetSeat], ReturnKind = NativeDrawAttemptReturnKind.WumiaoFlood } });
                var paid = use.NativeDraw!;
                if (paid.ActiveAttemptId is not null && !paid.AttemptReturned)
                    throw new InvalidOperationException("Wumiao Flood resumed before its actual draw request returned.");
                if (!paid.AttemptReturned && _coreNativeDrawAttemptsOperations.TryBeginNativeDrawAttempt(use))
                    return;
                var before = _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence;
                var ids = paid.Replacement is null && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat], 1, true, CardMoveReasons.Draw) : Array.AsReadOnly(Array.Empty<int>());
                var outcome = new NativeDrawOutcome(r.TargetSeat, 1, paid.ActiveAttemptId, before, _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence, paid.Replacement)
                {
                    ActualDrawIds = ids
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreCardUseLifecycleOperations.LifecycleCardUse(id)!with { NativeDraw = null, WumiaoFlood = r with { Stage = WumiaoFloodStage.DrawingChildren, DrawOutcome = outcome } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WumiaoFloodDrawReturnedEvent(id, r.NullificationFrameId, r.TargetSeat, ids.Count, before, outcome.SequenceAfter));
                continue;
            }
            else
                throw new InvalidOperationException("Wumiao Flood lost its exact effect stage.");
            NativeOwners.CardAttackHandle attack = _kernelCoreMainOperations.CreateCardAttackHandle2(id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }
    }

    internal bool IsWumiaoFloodMovement(CardUseFrame use, CardMovementBatchContext batch) => use.WumiaoFlood is { } r && batch.ParentFrameId == use.Id && batch.AwaitingProgramFrameId == r.AwaitingProgramFrameId && batch.Movements.Count > 0 && (r.Stage == WumiaoFloodStage.DrawingChildren && r.DrawOutcome is { } draw && batch.Movements.All(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && (draw.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.TargetSeat) && m.Reason == CardMoveReasons.Draw || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)) || r.Stage == WumiaoFloodStage.DiscardChildren && r.DiscardInvoiceSealed && r.DiscardBatches.Any(b => b.BatchId == batch.Id && b.ParentBatchId == batch.ParentBatchId && b.MovementCount == batch.Movements.Count) && batch.Movements.All(m => r.DiscardMovements.Any(e => WumiaoFloodDiscardEntryMatches(e, m))));
    internal bool IsWumiaoFloodMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => frame.ResumeWumiaoFloodFrameId == use.Id && frame.Id == frame.Batch.Id && frame.ResumeProgramFrameId is null && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(frame with { ResumeWumiaoFloodFrameId = null }) && IsWumiaoFloodMovement(use, frame.Batch);
    internal bool IsWumiaoFloodHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame frame) => use.WumiaoFlood is { Stage: WumiaoFloodStage.DiscardChildren or WumiaoFloodStage.DrawingChildren } && frame.Continuation == PostEventContinuation.WumiaoFlood && frame.ResumeFrameId == use.Id && frame.Change.ParentFrameId == use.Id && frame.CardId is null && frame.CardKind is null;
    internal bool IsWumiaoFloodRecoveryReturn(CardUseFrame use, RecoveryReplacementFrame frame) => frame.ParentFrameId == use.Id && frame.Return == new RecoveryReplacementReturn(PostEventContinuation.WumiaoFlood, use.Id) && IsWumiaoFloodRecoveryAttempt(use, frame.Attempt);
    internal bool IsWumiaoFloodRecoveryAttempt(CardUseFrame use, RecoveryAttempt attempt) => use.WumiaoFlood is { Stage: WumiaoFloodStage.DiscardChildren, DiscardCardId: { } card, DiscardFrom: { Zone: CardZoneKind.Equipment } from, DiscardSequence: > 0 } r && from.OwnerSeat == r.TargetSeat && r.DiscardInvoiceSealed && attempt.SourceSeat == r.TargetSeat && attempt.TargetSeat == r.TargetSeat && attempt.Amount == 1 && attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.SilverLion, MoveReason: CharacterGuanYuWumiaoDefinitions.WumiaoFloodDiscardReason) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.DiscardSequence && m.CardId == card && m.CardKind == CardKind.SilverLion && m.From == from && IsWumiaoFloodPrimaryDiscardMovement(r, m));

    internal void AssertWumiaoFlood(CardUseFrame use)
    {
        if (use.WumiaoFlood is not { } r)
            return;
        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.WumiaoFlood || use.Action is not { Type: CardActionType.Use } action || action.ActionId != r.ActionId || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || r.TargetIndex != use.TargetIndex || r.TargetIndex < 0 || r.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[r.TargetIndex] != r.TargetSeat || !Enum.IsDefined(r.Stage) || r.ExtraEffectOrdinal < 0 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodEffectStartedEvent>().Count(e => e.UseId == use.Id && e.ActionId == r.ActionId && e.NullificationId == r.NullificationFrameId && e.TargetSeat == r.TargetSeat && e.TargetIndex == r.TargetIndex && e.ExtraEffectOrdinal == r.ExtraEffectOrdinal) != 1)
            throw new InvalidOperationException("Wumiao Flood lost its exact action, target cursor or once-only effect.");
        if (r.Stage != WumiaoFloodStage.AwaitingDamage && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodDamageReturnedEvent>().Count(e => e.UseId == use.Id && e.NullificationId == r.NullificationFrameId && e.TargetSeat == r.TargetSeat && e.TargetIndex == r.TargetIndex) != 1)
            throw new InvalidOperationException("Wumiao Flood lost its completed native damage receipt.");
        if (use.NativeDraw is { } paid && (r.Stage != WumiaoFloodStage.Drawing || paid.ReturnKind != NativeDrawAttemptReturnKind.WumiaoFlood || paid.RequestedCount != 1 || paid.Recipients is not [var recipient] || recipient != r.TargetSeat || paid.RecipientIndex != 0 || paid.InstructionIndex != 0 || paid.ResultBind is not null || paid.Operation is not null || paid.Reason != CardMoveReasons.Draw))
            throw new InvalidOperationException("Wumiao Flood changed its actual once-only draw producer.");
        if (r.Stage == WumiaoFloodStage.DrawingChildren && (r.DrawOutcome is not { } draw || use.NativeDraw is not null || draw.RecipientSeat != r.TargetSeat || draw.RequestedCount != 1 || draw.SequenceBefore > draw.SequenceAfter || draw.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || draw.ActualDrawIds.Count > 1 || draw.Replacement is not null && draw.ActualDrawIds.Count != 0 || draw.ActualDrawIds is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodDrawReturnedEvent>().Count(e => e.UseId == use.Id && e.NullificationId == r.NullificationFrameId && e.RecipientSeat == r.TargetSeat && e.ActualCount == draw.ActualDrawIds.Count && e.SequenceBefore == draw.SequenceBefore && e.SequenceAfter == draw.SequenceAfter) != 1))
            throw new InvalidOperationException("Wumiao Flood lost its real draw result and child boundary.");
        if (r.DiscardSequence > 0 && (r.DiscardCardId is null || r.DiscardFrom is null || r.DiscardPrintedKind is null || !r.DiscardInvoiceSealed || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => IsWumiaoFloodPrimaryDiscardMovement(r, m)) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodDiscardPaidEvent>().Count(e => e.UseId == use.Id && e.NullificationId == r.NullificationFrameId && e.TargetSeat == r.TargetSeat && e.CardId == r.DiscardCardId && e.From == r.DiscardFrom && e.MovementSequence == r.DiscardSequence) != 1))
            throw new InvalidOperationException("Wumiao Flood lost its original target's real discard.");
        if (r.DiscardInvoiceSealed)
            AssertWumiaoFloodDiscardInvoice(use, r);
    }

    internal void AssertWumiaoFloodDiscardInvoice(CardUseFrame use, WumiaoFloodReceipt r)
    {
        var sealedFacts = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodDiscardInvoiceSealedEvent>().Where(e => e.UseId == use.Id && e.NullificationId == r.NullificationFrameId).ToArray();
        var materials = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodDiscardMaterialPaidEvent>().Where(e => e.UseId == use.Id && e.NullificationId == r.NullificationFrameId).ToArray();
        var batches = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WumiaoFloodDiscardBatchIssuedEvent>().Where(e => e.UseId == use.Id && e.NullificationId == r.NullificationFrameId).ToArray();
        if (r.Stage is not (WumiaoFloodStage.DiscardChildren or WumiaoFloodStage.FinishingAttack) || r.DiscardFrom != CardLocation.Hand(r.TargetSeat) && r.DiscardFrom != CardLocation.Equipment(r.TargetSeat) || r.DiscardSequenceBefore < 0 || r.DiscardSequence <= r.DiscardSequenceBefore || r.DiscardSequenceAfter < r.DiscardSequence || r.DiscardSequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.GrainMaterials is not System.Collections.IList { IsReadOnly: true } || r.DiscardMovements is not System.Collections.IList { IsReadOnly: true } || r.DiscardBatches is not System.Collections.IList { IsReadOnly: true } || r.GrainMaterials.Select(g => g.CardId).Distinct().Count() != r.GrainMaterials.Count || r.GrainMaterials.Any(g => g.CardId == r.DiscardCardId || g.From != CardLocation.WoodenOxGrain(r.TargetSeat)) || r.GrainMaterials.Count > 0 && (r.DiscardPrintedKind != CardKind.WoodenOx || r.DiscardFrom != CardLocation.Equipment(r.TargetSeat)) || r.DiscardMovements.Count != r.GrainMaterials.Count + 1 || r.DiscardMovements.Select(m => m.CardId).Distinct().Count() != r.DiscardMovements.Count || !r.DiscardMovements.Select(m => m.MovementSequence).SequenceEqual(r.DiscardMovements.Select(m => m.MovementSequence).Order()) || r.DiscardMovements[0].MovementSequence != r.DiscardSequence || r.DiscardMovements.Any(m => m.MovementSequence <= r.DiscardSequenceBefore || m.MovementSequence > r.DiscardSequenceAfter || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(actual => WumiaoFloodDiscardEntryMatches(m, actual)) != 1) || sealedFacts is not [var sealedFact] || sealedFact != new WumiaoFloodDiscardInvoiceSealedEvent(use.Id, r.NullificationFrameId, r.DiscardSequence, r.DiscardSequenceBefore, r.DiscardSequenceAfter, r.GrainMaterials.Count, r.DiscardMovements.Count, r.DiscardBatches.Count) || materials.Length != r.DiscardMovements.Count || !materials.Select((e, i) => e == new WumiaoFloodDiscardMaterialPaidEvent(use.Id, r.NullificationFrameId, i, r.DiscardMovements[i].CardId, r.DiscardMovements[i].PrintedKind, r.DiscardMovements[i].From, r.DiscardMovements[i].To, r.DiscardMovements[i].Reason, r.DiscardMovements[i].MovementSequence)).All(v => v) || batches.Length != r.DiscardBatches.Count || r.DiscardBatches.Select(b => b.BatchId).Distinct().Count() != r.DiscardBatches.Count || !batches.Select((e, i) => e == new WumiaoFloodDiscardBatchIssuedEvent(use.Id, r.NullificationFrameId, r.DiscardBatches[i].BatchId, r.DiscardBatches[i].ParentBatchId, r.DiscardBatches[i].MovementCount)).All(v => v))
            throw new InvalidOperationException("Wumiao Flood lost its sealed original discard and exact native equipment-child invoice.");
        foreach (var grain in r.GrainMaterials)
            if (r.DiscardMovements.Where(m => m.CardId == grain.CardId && m.PrintedKind == grain.PrintedKind && m.From == grain.From && m.Reason == CardMoveReasons.WoodenOxGrainDiscard).ToArray()is not [var paid] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => WumiaoFloodDiscardEntryMatches(paid, m) && (m.To == CardLocation.DiscardPile || _coreHellMengpoProgramsOperations.IsHellShiyouPaidDiscardMovement(m, grain.From, CardMoveReasons.WoodenOxGrainDiscard))))
                throw new InvalidOperationException("Wumiao Flood's Ox child lost an originally frozen grain or its real native destination.");
    }
}
