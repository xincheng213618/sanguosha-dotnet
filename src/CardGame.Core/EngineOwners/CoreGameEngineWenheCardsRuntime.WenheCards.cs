#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenheCardsRuntime
{
    internal IReadOnlyList<CardKind> WenheOrdinaryTrickKinds => _coreActivityBattleRulesOperations.Value.WenheRules?.OrdinaryTrickKinds ?? Enum.GetValues<CardKind>().Where(IsWenheCard).ToArray();

    internal static bool IsWenheCard(CardKind kind) => RuntimeSkills.RuntimeRules_Core_WenheCards.IsWenheCard(kind);
    internal static LegalActionKind WenheCardAction(CardKind kind) => RuntimeSkills.RuntimeRules_Core_WenheCards.WenheCardAction(kind);
    internal static CardMoveReason WenheRedistributionReason(CardKind kind) => RuntimeSkills.RuntimeRules_Core_WenheCards.WenheRedistributionReason(kind);
    internal bool CanBeWenheAdditionalTarget(CardUseFrame use, CharacterState target)
    {
        if (!target.IsAlive || target.Seat == use.SourceSeat || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive)
            return false;
        return use.CardKind switch
        {
            CardKind.StarShift => target.HasHpValue && target.Hp >= 1 && _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].HasHpValue && _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].Hp >= 1,
            CardKind.HandRedistribution => true,
            _ => false
        };
    }

    internal void AddWenheCardActions(List<LegalAction> actions, CharacterState actor, IEnumerable<Card> cards)
    {
        foreach (var card in cards.Where(card => IsWenheCard(card.Kind)))
        {
            var kind = card.Kind;
            if (_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
                continue;
            var self = kind is CardKind.BeamSwap or CardKind.WenheChaos;
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (self ? p.Seat == actor.Seat : p.Seat != actor.Seat)))
            {
                if (kind == CardKind.StarShift && (!actor.HasHpValue || !target.HasHpValue || actor.Hp < 1 || target.Hp < 1))
                    continue;
                if (_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, card.Suit, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, card))) || _coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, target.Seat, kind) || _coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, _coreNativeCardAppearanceOperations.EffectiveSuit(actor, card)))
                    continue;
                actions.Add(new(WenheCardAction(kind), card.Id, self ? null : target.Seat, self ? $"使用【{(CardCatalog.Get(kind).DisplayName)}】" : $"对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】"));
            }
        }
    }

    internal void ResolveWenheCard(CharacterState actor, Card card, CardKind kind, int? targetSeat = null)
    {
        if (!IsWenheCard(kind) || !_coreNativeLegalActionsOperations.BuildLegalActions(actor, selectedPhysicalCardId: card.Id).Any(a => a.Kind == WenheCardAction(kind) && a.CardId == card.Id && a.TargetSeat == targetSeat))
            throw new InvalidOperationException("A Wenhe card lost its published native entity or exact legal target.");
        var seat = kind is CardKind.BeamSwap or CardKind.WenheChaos ? actor.Seat : targetSeat ?? throw new InvalidOperationException("A Wenhe single-other trick requires its actual target.");
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [seat], kind);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, [seat], WenheCardAction(kind), playedCardKind: kind);
    }

    internal void ResolveWenheCardEffect(NullificationWindowFrame pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != pending.ParentFrameId || !IsWenheCard(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || pending.EffectCardKind != CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || pending.ActionKind != WenheCardAction(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || use.SourceSeat != pending.SourceSeat || use.CardId != pending.EffectCardId || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || use.WenheCardEffect is not null || CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending)is not { } seat || use.TargetIndex < 0 || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != seat || !_characterZhugeGuoLeOperations.IsLeChengyanEffectUse(use.Id) && (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)is CardKind.BeamSwap or CardKind.WenheChaos && seat != use.SourceSeat || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)is CardKind.StarShift or CardKind.HandRedistribution && seat == use.SourceSeat))
            throw new InvalidOperationException("A Wenhe effect lost its exact once-resolved native nullification and target cursor.");
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.ResolvingEffect);
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.StarShift && (!_coreMatchParticipantsPlayersAccess._players[seat].HasHpValue || !_coreMatchParticipantsPlayersAccess._players[use.SourceSeat].HasHpValue || _coreMatchParticipantsPlayersAccess._players[seat].Hp < 1 || _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].Hp < 1))
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
            return;
        }

        var receipt = new WenheCardEffectReceipt(action.ActionId, pending.Id, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), seat, use.TargetIndex, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.WenheChaos ? WenheCardEffectStage.AwaitingChaos : WenheCardEffectStage.Children, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            MovementAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        receipt = CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) switch
        {
            CardKind.StarShift => PlanWenheStarShift(use, receipt),
            CardKind.HandRedistribution => PlanWenheHandRedistribution(use, receipt),
            CardKind.BeamSwap => PlanWenheBeamSwap(use, receipt),
            _ => receipt
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { Step = ResolutionFrameStep.ResolvingEffect, WenheCardEffect = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheCardPlanIssuedEvent(use.Id, action.ActionId, pending.Id, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), use.SourceSeat, seat, receipt.Materials.Count, receipt.SourceHandCount, receipt.SourceHpBefore, receipt.TargetHpBefore, receipt.SourceHpAfter, receipt.TargetHpAfter));
        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.WenheChaos)
        {
            _coreWenheChaosOperations.Value.BeginWenheCardChaos(use);
            return;
        }

        if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.StarShift)
        {
            var source = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
            var target = _coreMatchParticipantsPlayersAccess._players[seat];
            source.Hp = receipt.SourceHpAfter!.Value;
            target.Hp = receipt.TargetHpAfter!.Value;
            _coreRuntimeOperations.ReplaceRuntimeTop(use with { WenheCardEffect = receipt = receipt with { Applied = true } });
            _coreHpChangeProgramsOperations.RecordHpChange(use.Id, use.SourceSeat, source.Seat, receipt.SourceHpBefore!.Value, source.Hp, HpChangeKind.Setting);
            _coreHpChangeProgramsOperations.RecordHpChange(use.Id, use.SourceSeat, target.Seat, receipt.TargetHpBefore!.Value, target.Hp, HpChangeKind.Setting);
        }
        else
        {
            var moving = receipt.Materials.Where(m => m.From != m.To).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(use with { WenheCardEffect = receipt = receipt with { Applied = true } });
            if (moving.Length != 0)
            {
                var destinations = moving.ToDictionary(m => m.CardId, m => m.To);
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(moving.Select(m => m.CardId).ToArray(), CardLocation.Processing, WenheRedistributionReason(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)), (batchId, records) =>
                {
                    var current = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
                    _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, f => f with { WenheCardEffect = current.WenheCardEffect!with { MovementBatchId = batchId, MovementAfter = records[^1].Sequence } });
                }, (card, _) => destinations[card.Id], disclosure: CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.HandRedistribution ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
            }
        }

        use = _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)!;
        receipt = use.WenheCardEffect!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheCardEffectAppliedEvent(use.Id, receipt.ActionId, receipt.NullificationFrameId, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), receipt.Materials.Count(m => m.From != m.To), receipt.MovementBefore, receipt.MovementAfter));
        ContinueWenheCardEffect(use.Id);
    }

    internal WenheCardEffectReceipt PlanWenheStarShift(CardUseFrame use, WenheCardEffectReceipt receipt)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat];
        var sum = checked(source.Hp + target.Hp);
        var minimum = Math.Max(1, sum - target.MaxHp);
        var maximum = Math.Min(source.MaxHp, sum - 1);
        if (minimum > maximum)
            throw new InvalidOperationException("The living HP pair has no total-preserving feasible allocation.");
        var assigned = minimum + _randomOperations._random.Next(maximum - minimum + 1);
        return receipt with
        {
            SourceHpBefore = source.Hp,
            TargetHpBefore = target.Hp,
            SourceHpAfter = assigned,
            TargetHpAfter = sum - assigned
        };
    }

    internal WenheCardEffectReceipt PlanWenheHandRedistribution(CardUseFrame use, WenheCardEffectReceipt receipt)
    {
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[use.SourceSeat]).Select(card => (Card: card, From: CardLocation.Hand(use.SourceSeat))).Concat(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat]).Select(card => (Card: card, From: CardLocation.Hand(receipt.TargetSeat)))).OrderBy(item => item.Card.Id).ToList();
        _randomOperations._random.Shuffle(cards);
        var sourceCount = _randomOperations._random.Next(cards.Count + 1);
        return receipt with
        {
            SourceHandCount = sourceCount,
            Materials = cards.Select((item, index) => new WenheRedistributionMaterial(item.Card.Id, item.Card.Kind, item.From, CardLocation.Hand(index < sourceCount ? use.SourceSeat : receipt.TargetSeat))).ToArray()
        };
    }

    internal WenheCardEffectReceipt PlanWenheBeamSwap(CardUseFrame use, WenheCardEffectReceipt receipt)
    {
        var all = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Select(card => (Card: card, Owner: p.Seat))).ToArray();
        var moves = new List<WenheRedistributionMaterial>();
        foreach (var group in all.Where(item => !item.Card.IsGeneralWeapon).GroupBy(item => _coreFieldEquipmentOperations.FieldEquipmentSlot(item.Card)).OrderBy(group => group.Key))
        {
            var tickets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => Enumerable.Repeat(p.Seat, Math.Max(0, p.EquipmentSlotCapacity(group.Key) - all.Count(item => item.Owner == p.Seat && item.Card.IsGeneralWeapon && _coreFieldEquipmentOperations.FieldEquipmentSlot(item.Card) == group.Key)))).ToList();
            var equipment = group.OrderBy(item => item.Card.Id).ToList();
            if (tickets.Count < equipment.Count)
                throw new InvalidOperationException("Existing movable equipment exceeds the living legal capacity.");
            _randomOperations._random.Shuffle(tickets);
            _randomOperations._random.Shuffle(equipment);
            moves.AddRange(equipment.Select((item, index) => new WenheRedistributionMaterial(item.Card.Id, item.Card.Kind, CardLocation.Equipment(item.Owner), CardLocation.Equipment(tickets[index]))));
        }

        // One real Ox carries its original private store together with its own
        // destination; the stores are captured before either treasure moves.
        if (_coreNativeDamageHandlesOperations.Value.UsesFormalWoodenOx)
            foreach (var ox in moves.Where(move => move.PrintedKind == CardKind.WoodenOx && move.From != move.To).ToArray())
                moves.AddRange(_cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(ox.From.OwnerSeat!.Value)).Select(card => new WenheRedistributionMaterial(card.Id, card.Kind, CardLocation.WoodenOxGrain(ox.From.OwnerSeat.Value), CardLocation.WoodenOxGrain(ox.To.OwnerSeat!.Value))));
        return receipt with
        {
            Materials = moves.ToArray()
        };
    }

    internal void CompleteWenheCardChaos(long useId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { WenheCardEffect: { Stage: WenheCardEffectStage.AwaitingChaos, Applied: false } receipt } use || use.Id != useId || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.WenheChaos || receipt.ActionId != use.Action?.ActionId || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Wenhe Chaos cannot return to a different or already completed native trick.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheCardChaosReturnedEvent(useId, receipt.ActionId, receipt.NullificationFrameId));
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { WenheCardEffect = receipt = receipt with { Applied = true, Stage = WenheCardEffectStage.Children } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenheCardEffectAppliedEvent(useId, receipt.ActionId, receipt.NullificationFrameId, CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use), 0, receipt.MovementBefore, receipt.MovementAfter));
        ContinueWenheCardEffect(useId);
    }

    internal void ContinueWenheCardEffect(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame { WenheCardEffect: { Applied: true, Stage: WenheCardEffectStage.Children } } use || use.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Wenhe's paid effect resumed without its exact quiet card owner.");
        AssertWenheCardReceipt(use);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.WenheCardEffect) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.WenheCardEffect) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.WenheCardEffect) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        AssertWenheCardReceipt(use);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, f => f with { WenheCardEffect = null });
        _coreNativeCardUseCompletionOperations.FinishCardUse(id, _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(id, use.CardId, requireProcessing: true), CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use));
    }

    internal bool IsWenheCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch)
    {
        if (use.WenheCardEffect is not { Applied: true, Stage: WenheCardEffectStage.Children, MovementBatchId: { } batchId } receipt || batch.Id != batchId || batch.ParentFrameId != use.Id || batch.Movements.Count == 0 || batch.Movements.Count != receipt.Materials.Count(m => m.From != m.To) || batch.Movements[0].Sequence != receipt.MovementBefore + 1 || batch.Movements[^1].Sequence != receipt.MovementAfter)
            return false;
        return batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Reason == WenheRedistributionReason(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) && receipt.Materials.Count(m => m.CardId == move.CardId && m.PrintedKind == move.CardKind && m.From == move.From && m.To == move.To && m.From != m.To) == 1);
    }

    internal bool IsWenheCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => movement.ResumeWenheCardEffectFrameId == use.Id && movement.Id == movement.Batch.Id && movement.ResumeProgramFrameId is null && !CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(movement with { ResumeWenheCardEffectFrameId = null }) && IsWenheCardEffectMovement(use, movement.Batch);
    internal bool IsWenheCardEffectHpReturn(CardUseFrame use, HpChangedTriggerWindowFrame hp) => use.WenheCardEffect is { Applied: true, Stage: WenheCardEffectStage.Children } && hp.Continuation == PostEventContinuation.WenheCardEffect && hp.ResumeFrameId == use.Id && hp.Change.ParentFrameId == use.Id && hp.CardId is null && hp.CardKind is null;

    internal void AssertWenheCardReceipt(CardUseFrame use)
    {
        if (use.WenheCardEffect is not { } r)
            return;
        if (!IsWenheCard(CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use)) || r.Kind != CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) || use.Action?.ActionId != r.ActionId || r.NullificationFrameId <= 0 || r.TargetIndex != use.TargetIndex || use.TargetIndex < 0 || use.TargetIndex >= CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use).Count || CharacterZhugeGuoLeRuntime.LeChengyanNativeTargets(use)[use.TargetIndex] != r.TargetSeat || !Enum.IsDefined(r.Stage) || r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || r.MovementBefore < 0 || r.MovementAfter < r.MovementBefore || r.MovementAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenheCardPlanIssuedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.NullificationFrameId == r.NullificationFrameId && e.Kind == r.Kind && e.SourceSeat == use.SourceSeat && e.TargetSeat == r.TargetSeat && e.MaterialCount == r.Materials.Count && e.SourceHandCount == r.SourceHandCount && e.SourceHpBefore == r.SourceHpBefore && e.TargetHpBefore == r.TargetHpBefore && e.SourceHpAfter == r.SourceHpAfter && e.TargetHpAfter == r.TargetHpAfter) != 1)
            throw new InvalidOperationException("Wenhe lost its frozen random plan, exact action, target or original material set.");
        if (r.Stage == WenheCardEffectStage.AwaitingChaos)
        {
            if (CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) != CardKind.WenheChaos || r.Applied || r.Materials.Count != 0 || r.MovementBatchId is not null)
                throw new InvalidOperationException("Wenhe Chaos lost its native child return.");
            return;
        }

        if (!r.Applied || CharacterZhugeGuoLeRuntime.LeChengyanNativeEffectKind(use) == CardKind.WenheChaos && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenheCardChaosReturnedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.NullificationFrameId == r.NullificationFrameId) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenheCardEffectAppliedEvent>().Count(e => e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.NullificationFrameId == r.NullificationFrameId && e.Kind == r.Kind && e.MovedCards == r.Materials.Count(m => m.From != m.To) && e.MovementBefore == r.MovementBefore && e.MovementAfter == r.MovementAfter) != 1 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter).Select(m => (m.CardId, m.CardKind, m.From, m.To)).SequenceEqual(r.Materials.Where(m => m.From != m.To).Select(m => (m.CardId, m.PrintedKind, m.From, m.To))))
            throw new InvalidOperationException("Wenhe changed its already applied entities or repeated a paid redistribution.");
    }
}
