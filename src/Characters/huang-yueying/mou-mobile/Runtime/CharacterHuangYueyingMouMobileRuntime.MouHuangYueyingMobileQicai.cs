#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangYueyingMouMobileRuntime
{
    internal IEnumerable<(Card Card, CardLocation From)> MouHyEquipmentChoices(int owner, int target)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || owner == target || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchParticipantsPlayersAccess._players[target].EquipmentAreaAbolished)
            return[];
        return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Select(c => (Card: c, From: CardLocation.Hand(owner))).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Select(c => (Card: c, From: CardLocation.DiscardPile))).Where(p => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(p.Card.Kind) && !p.Card.IsGeneralWeapon && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, MouHuangYueyingMobileComposition.Qicai, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[owner], MouHuangYueyingMobileComposition.Qicai), p.Card) && _coreMatchParticipantsPlayersAccess._players[target].EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(p.Card.Kind).Slot) > 0).ToArray();
    }

    internal int MouHyQicaiPhaseUses(int owner, ActualDiscardRecoveryPhaseKey phase) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQicaiReservedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Phase == phase);
    [ProgramPhaseUsedProjection(90)]
    internal int MouHyQicaiProjectedUses(int owner, string skill, int fallback) => skill == MouHuangYueyingMobileComposition.Qicai && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p ? MouHyQicaiPhaseUses(owner, p) : fallback;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 31500, SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment)]
    internal bool CanMouHyQicai(ProgramActivationLegalityContext c) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && p.ActorSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && MouHyQicaiPhaseUses(c.OwnerSeat, p) == 0;
    internal SkillProgramStepOutcome BeginMouHyQicai(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MouHuangYueyingMobile is not null)
        {
            ContinueMouHy(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (f.SkillId != MouHuangYueyingMobileComposition.Qicai || f.TriggerId is not null || f.ActivationId != "place-equipment" || f.InstructionIndex != 1 || f.SelectedTargetSeats is not [var target] || f.SelectedCardIds.Count != 0 || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } p || p.ActorSeat != f.OwnerSeat || MouHyQicaiPhaseUses(f.OwnerSeat, p) != 0 || !MouHyEquipmentChoices(f.OwnerSeat, target).Any())
            throw new InvalidOperationException("Qicai needs its actual-Play once native equipment placement entry.");
        var source = MouHySource(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = new(source, f.GameplayHash, SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment, MouHuangYueyingMobileStage.Choosing, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, 0, target, p) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingQicaiReservedEvent(f.Id, source, f.GameplayHash, p, target));
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, f.OwnerSeat, MouHyQicaiChoices(f), "选择手牌或弃牌堆的一张装备牌置入目标装备区。", target);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<PromptChoice> MouHyQicaiChoices(ProgramSkillFrame f) => Array.AsReadOnly(MouHyEquipmentChoices(f.OwnerSeat, f.MouHuangYueyingMobile!.RecipientSeat).Select(p => new PromptChoice(new($"mou-huang-yueying.qicai.{(f.Id)}.{(p.Card.Id)}"), $"置入【{(p.Card.DisplayName)}】", [p.Card.Id], [f.MouHuangYueyingMobile.RecipientSeat], new Dictionary<string, string> { ["program-action"] = "mou-huang-yueying-qicai", ["frame-id"] = f.Id.ToString(), ["source-zone"] = p.From.Zone.ToString() })).ToArray());
    [ProgramAnswer("mou-huang-yueying-qicai")]
    internal void AnswerMouHyQicai(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHuangYueyingMobile: { Stage: MouHuangYueyingMobileStage.Choosing } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], MouHyQicaiChoices(f).Where(p => p.Id == c.Id).ToArray()))
            throw new InvalidOperationException("Qicai requires its current real Hand/public-discard entity and original recipient.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].IsAlive)
        {
            FinishMouHy(f);
            return;
        }

        var pair = MouHyEquipmentChoices(f.OwnerSeat, r.RecipientSeat).Single(p => p.Card.Id == c.Cards.Single());
        var card = pair.Card;
        var from = pair.From;
        var slot = _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot;
        var current = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.RecipientSeat]).Where(x => _coreEquipmentContentOperations.EquipmentRules.Get(x.Kind).Slot == slot).ToArray();
        var replaced = current.Length >= _coreMatchParticipantsPlayersAccess._players[r.RecipientSeat].EquipmentSlotCapacity(slot) ? current[0] : null;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = r = r with { Stage = MouHuangYueyingMobileStage.MovementChildren, Paid = true, CardId = card.Id, From = from, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (replaced is not null)
        {
            _coreAdvancedWeaponsOperations.CopyFirstReplacedWeapon(card, replaced);
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(r.RecipientSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        }

        _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Equipment(r.RecipientSeat), CardMoveReasons.EquipmentEnter);
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.CardId == card.Id && m.From == from && m.Sequence > before);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouHuangYueyingMobile = f.MouHuangYueyingMobile!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingQicaiEquipmentPlacedEvent(f.Id, r.Source, r.GameplayHash, r.RecipientSeat, card.Id, from, paid.To, paid.Sequence, replaced?.Id));
        if (paid.To == CardLocation.Equipment(r.RecipientSeat))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, r.RecipientSeat, slot, card.Id, card.Kind, replaced?.Id));
            var old = CurrentMouHyQi(r.RecipientSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouHuangYueyingQiGrantedEvent(f.Id, r.Source, r.GameplayHash, r.RecipientSeat, paid.Sequence, old?.GrantId));
            SyncMouHyQiMarker(f.Id, r.RecipientSeat);
        }

        ContinueMouHy(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.PlaceMouHuangYueyingQicaiEquipment)]
    internal PromptChoice AiMouHyQicai(PendingDecision p, ProgramSkillFrame f) => p.Choices.OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
    internal MouHuangYueyingQiGrantedEvent? CurrentMouHyQi(int target) => _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGrantedEvent>().LastOrDefault(e => e.RecipientSeat == target);
    internal int MouHyQiRemaining(MouHuangYueyingQiGrantedEvent grant) => Math.Max(0, 3 - _coreProgramEventHistoryOperations.ProgramEventHistory<MouHuangYueyingQiGainClaimedEvent>().Count(e => e.GrantId == grant.GrantId));
    internal void SyncMouHyQiMarker(long frame, int target)
    {
        var grant = CurrentMouHyQi(target);
        var count = grant is null ? 0 : MouHyQiRemaining(grant);
        var before = _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.MouHuangYueyingQi);
        if (count == before)
            return;
        if (count == 0)
            _coreMatchParticipantsPlayersAccess._players[target].Markers.Remove(PlayerMarkerKind.MouHuangYueyingQi);
        else
            _coreMatchParticipantsPlayersAccess._players[target].Markers[PlayerMarkerKind.MouHuangYueyingQi] = count;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(frame, target, PlayerMarkerKind.MouHuangYueyingQi, count - before, count, grant?.Source.OwnerSeat, "skill-program.mou-mobile:qicai.mark"));
    }
}
