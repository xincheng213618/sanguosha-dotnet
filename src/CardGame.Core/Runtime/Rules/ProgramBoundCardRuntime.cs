using CardGame.Core.RuntimeSkills;

namespace CardGame.Core;
/// <summary>Owns immutable bound-card selection, movement, cleanup and command answers on the original program frame.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramBoundCardRuntime(IProgramBoundCardFacts facts, IProgramBoundInstructionFacts instructions, IProgramBoundCardMovementOperations movement, IProgramBoundCardPaymentOperations payments, IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts, IProgramRuntimeEvents events, IProgramCardSnapshotQueries snapshots, INativeLogicalDiscardOperations logicalDiscards) : IProgramCardSetBindings, IProgramBoundCardCleanup
{
    void IProgramBoundCardCleanup.Cleanup(ProgramSkillFrame frame, bool completed) => CleanupProgramBoundCards(frame, completed);
    private readonly IProgramBoundCardFacts _facts = facts;
    private readonly IProgramBoundInstructionFacts _instructions = instructions;
    private readonly IProgramBoundCardMovementOperations _movement = movement;
    private readonly IProgramBoundCardPaymentOperations _payments = payments;
    private readonly IProgramRuntimeFrames _frames = frames;
    private readonly IProgramRuntimePrompts _prompts = prompts;
    private readonly IProgramRuntimeEvents _events = events;
    private readonly IProgramCardSnapshotQueries _snapshots = snapshots;
    private readonly INativeLogicalDiscardOperations _logicalDiscards = logicalDiscards;
    internal static ProgramSkillCardSetBinding GetProgramCardSet(ProgramSkillFrame frame, string bind) => frame.CardSetBindings.SingleOrDefault(binding => binding.Name == bind) ?? throw new InvalidOperationException($"Program card-set binding '{bind}' is unavailable.");
    private int CountAvailableBoundCards(ProgramSkillCardSetBinding source) => source.CardIds.Select((id, index) => (Id: id, Index: index)).Count(item => _facts.Location(item.Id) == source.SourceLocations[item.Index]);
    private int CountAvailableBoundSuits(ProgramSkillCardSetBinding source) => source.CardIds.Select((id, index) => (Id: id, Index: index)).Where(item => _facts.Location(item.Id) == source.SourceLocations[item.Index]).Select(item => _facts.CardsAt(source.SourceLocations[item.Index]).Single(card => card.Id == item.Id).Suit).Distinct().Count();
    public void SetProgramCardSet(long frameId, string bind, IReadOnlyList<int> cardIds, SkillProgramCardSetVisibility visibility, IReadOnlyList<CardLocation>? sourceLocations = null, Suit? frozenRevealedSuit = null, int? selectionActorSeat = null)
    {
        var frame = _frames.ActiveProgram(frameId);
        if (frame.CardSetBindings.Any(binding => binding.Name == bind))
            throw new InvalidOperationException($"Program card-set binding '{bind}' already exists.");
        var ids = cardIds.ToArray();
        var locations = (sourceLocations ?? ids.Select(cardId => _facts.Location(cardId)).ToArray()).ToArray();
        if (ids.Distinct().Count() != ids.Length || ids.Length != locations.Length)
            throw new InvalidOperationException("A program card-set binding requires unique cards and aligned source locations.");
        selectionActorSeat ??= _instructions.SelectionActor(frame, bind, ids, locations);
        if (selectionActorSeat is { } actor && !_facts.IsValidSeat(actor))
            throw new InvalidOperationException("The selection producer has an invalid actual actor.");
        _frames.ReplaceTop(frame with { CardSetBindings = Array.AsReadOnly(frame.CardSetBindings.Append(new ProgramSkillCardSetBinding(bind, Array.AsReadOnly(ids), visibility, Array.AsReadOnly(locations)) { FrozenRevealedSuit = frozenRevealedSuit, SelectionActorSeat = _instructions.HasForeignDiscardCapability || _logicalDiscards.TracksInvocations ? selectionActorSeat : null }).ToArray()) });
    }

    internal void RevealProgramTopCards(long frameId, int ownerSeat, int amount, SkillProgramNumberExpression? numberExpression, string resultBind, SkillProgramCardSetVisibility visibility)
    {
        var frame = _frames.ActiveProgram(frameId);
        if (numberExpression is (SkillProgramNumberExpression.OwnerLostHp) && !_facts.HasHpValue(ownerSeat))
        {
            _payments.Cancel(frame, "体力值未定义，无法计算此效果。");
            return;
        }

        var revealCount = numberExpression switch
        {
            null => amount,
            SkillProgramNumberExpression.OwnerLostHp => Math.Max(0, _facts.MaxHp(ownerSeat) - _facts.Hp(ownerSeat)),
            _ => throw new InvalidOperationException($"Unsupported reveal number expression '{numberExpression}'.")};
        var revealed = new List<Card>(revealCount);
        var reason = new CardMoveReason($"skill-program.{frame.SkillId}.{RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame)}.reveal");
        for (var index = 0; index < revealCount; index++)
        {
            if (_movement.DrawToProcessing(reason)is not { } card)
                break;
            revealed.Add(card);
        }

        SetProgramCardSet(frameId, resultBind, revealed.Select(card => card.Id).ToArray(), visibility);
        if (visibility == SkillProgramCardSetVisibility.Public)
            _events.AdvanceFact(new ProgramCardsRevealedEvent(frameId, frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, resultBind, Array.AsReadOnly(revealed.Select(_snapshots.Snapshot).ToArray())));
    }

    internal void FilterProgramBoundCards(long frameId, string sourceBind, string resultBind, IReadOnlyList<Suit> suits, ProgramParticipantReference? effectiveSuitFor, IReadOnlyList<SkillProgramCardCategory> categories, IReadOnlyList<EquipmentSlot> equipmentSlots, IReadOnlyList<CardKind> cardKinds, string? matchSuitOfBind, bool availableAtSourceOnly = false)
    {
        var frame = _frames.ActiveProgram(frameId);
        var source = GetProgramCardSet(frame, sourceBind);
        var matchedSuit = matchSuitOfBind is null ? (Suit? )null : GetProgramCardSet(frame, matchSuitOfBind).FrozenRevealedSuit ?? throw new InvalidOperationException("The referenced public suit is unavailable.");
        var effectiveSuitSeat = effectiveSuitFor is null ? (int? )null : _instructions.Participant(frame, effectiveSuitFor);
        var candidates = source.CardIds.Select((cardId, index) => (CardId: cardId, Index: index));
        if (availableAtSourceOnly)
            candidates = candidates.Where(item => _facts.Location(item.CardId) == source.SourceLocations[item.Index]);
        var selected = candidates.Select(item =>
        {
            var location = source.SourceLocations[item.Index];
            if (_facts.Location(item.CardId) != location)
                throw new InvalidOperationException("A bound card left its frozen source before its configured filter.");
            var card = _facts.CardsAt(location).Single(current => current.Id == item.CardId);
            return (Card: card, Location: location);
        }).Where(item =>
        {
            var suit = effectiveSuitSeat is { } seat ? _facts.EffectiveSuit(seat, item.Card) : item.Card.Suit;
            return matchedSuit is { } required ? suit == required : ProgramCardSetFilter.Matches(item.Card.Kind, suit, suits, categories, equipmentSlots, cardKinds);
        }).ToArray();
        SetProgramCardSet(frameId, resultBind, selected.Select(item => item.Card.Id).ToArray(), source.Visibility, selected.Select(item => item.Location).ToArray(), selectionActorSeat: source.SelectionActorSeat);
    }

    internal SkillProgramStepOutcome SelectProgramSourceCard(long frameId, int ownerSeat, SkillProgramCardSource cardSource, IReadOnlyList<CardZoneKind> zones, string resultBind, IReadOnlyList<EquipmentSlot> equipmentSlots, bool skipIfNoCards, bool allowSameSource)
    {
        var frame = _frames.ActiveProgram(frameId);
        var sourceSeat = cardSource switch
        {
            SkillProgramCardSource.Owner => ownerSeat,
            SkillProgramCardSource.DamageSource => frame.WindowContext?.SourceSeat,
            SkillProgramCardSource.EventTarget => frame.WindowContext?.TargetSeat,
            _ => null
        };
        if (sourceSeat is null || !_facts.IsValidSeat(sourceSeat.Value) || (cardSource == SkillProgramCardSource.DamageSource && sourceSeat == ownerSeat && !allowSameSource))
        {
            if (skipIfNoCards)
            {
                SetProgramCardSet(frameId, resultBind, [], SkillProgramCardSetVisibility.Private, []);
                return SkillProgramStepOutcome.Continue;
            }

            _payments.Cancel(frame, "伤害来源不再可供选择牌，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var choices = new List<PromptChoice>();
        if (zones.Contains(CardZoneKind.Hand))
        {
            var hand = _facts.Hand(sourceSeat.Value);
            for (var slot = 0; slot < hand.Count; slot++)
            {
                choices.Add(new PromptChoice(new ChoiceId($"program-source-card.frame-{frameId}.hand-slot-{slot}"), $"选择 {_facts.Name(sourceSeat.Value)} 的第 {slot + 1} 张暗置手牌。", [], [sourceSeat.Value], new Dictionary<string, string> { ["program-action"] = "select-source-card", ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = resultBind, ["source-zone"] = CardZoneKind.Hand.ToString(), ["slot-index"] = slot.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
            }
        }

        if (zones.Contains(CardZoneKind.Equipment))
        {
            foreach (var card in _facts.Equipment(sourceSeat.Value).Where(card => equipmentSlots.Count == 0 || equipmentSlots.Contains(_facts.FieldEquipmentSlot(card))))
            {
                choices.Add(new PromptChoice(new ChoiceId($"program-source-card.frame-{frameId}.equipment-{card.Id}"), $"选择 {_facts.Name(sourceSeat.Value)} 的装备【{card.DisplayName}】。", [card.Id], [sourceSeat.Value], new Dictionary<string, string> { ["program-action"] = "select-source-card", ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = resultBind, ["source-zone"] = CardZoneKind.Equipment.ToString() }));
            }
        }

        foreach (var zone in zones.Where(zone => zone is CardZoneKind.WoodenOxGrain or CardZoneKind.BuquWound or CardZoneKind.Authority or CardZoneKind.Chunlao))
        {
            if (cardSource != SkillProgramCardSource.Owner)
                throw new InvalidOperationException("A persistent source pile must belong to the program owner.");
            foreach (var card in _facts.CardsAt(new CardLocation(zone, ownerSeat)))
            {
                choices.Add(new PromptChoice(new ChoiceId($"program-source-card.frame-{frameId}.{zone}-{card.Id}"), $"选择 {_facts.Name(ownerSeat)} 的【{card.DisplayName}】。", [card.Id], [ownerSeat], new Dictionary<string, string> { ["program-action"] = "select-source-card", ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = resultBind, ["source-zone"] = zone.ToString() }));
            }
        }

        if (choices.Count == 0)
        {
            if (skipIfNoCards)
            {
                SetProgramCardSet(frameId, resultBind, [], SkillProgramCardSetVisibility.Private, []);
                return SkillProgramStepOutcome.Continue;
            }

            _payments.Cancel(frame, "伤害来源已没有可选择的牌，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var presentation = _instructions.Skill(frame.SkillId);
        _prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, ownerSeat, cardSource == SkillProgramCardSource.Owner ? $"【{presentation.Name}】请选择自己的一张牌。" : $"【{presentation.Name}】请选择伤害来源的一张牌。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [sourceSeat.Value], sourceSeat) { PromptId = _prompts.NextId(), IsPrivate = true, TargetSeat = sourceSeat, SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{presentation.Name} · 选择来源牌", presentation.Description), Choices = Array.AsReadOnly(choices.ToArray()) }, _facts.IsHuman(ownerSeat));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome GiveProgramBoundCard(long frameId, int ownerSeat, string sourceBind, SkillProgramTargetKind targetKind, CardMoveReason reason)
    {
        var frame = _frames.ActiveProgram(frameId);
        var source = GetProgramCardSet(frame, sourceBind);
        if (source.CardIds.Count != source.SourceLocations.Count)
            throw new InvalidOperationException("A program card-set binding lost its source locations.");
        var available = source.CardIds.Select((cardId, index) => (CardId: cardId, Location: source.SourceLocations[index])).Where(item => _facts.Location(item.CardId) == item.Location).ToArray();
        var targetSeats = _instructions.TargetSeats(ownerSeat, targetKind);
        if (available.Length == 0 || targetSeats.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var choices = (
            from item in available
            let card = _facts.CardsAt(item.Location).Single(card => card.Id == item.CardId)from targetSeat in targetSeats
            select new PromptChoice(new ChoiceId($"program-give.frame-{frameId}.card-{card.Id}.target-{targetSeat}"), $"将【{card.DisplayName}】交给 {_facts.Name(targetSeat)}（座位 {targetSeat + 1}）。", [card.Id], [targetSeat], new Dictionary<string, string> { ["program-action"] = "give-bound-card", ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["source-bind"] = sourceBind, ["target-kind"] = targetKind.ToString(), ["move-reason"] = reason.Value })).Append(new PromptChoice(new ChoiceId($"program-give.frame-{frameId}.keep"), "不分配，保留这些牌。", [], [], new Dictionary<string, string> { ["program-action"] = "keep-bound-cards", ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["source-bind"] = sourceBind, ["target-kind"] = targetKind.ToString(), ["move-reason"] = reason.Value })).ToArray();
        var presentation = _instructions.Skill(frame.SkillId);
        _prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, ownerSeat, $"【{presentation.Name}】可将一张牌交给一名角色。", available.Select(item => item.CardId).ToArray(), targetSeats, frame.WindowContext?.SourceSeat) { PromptId = _prompts.NextId(), IsPrivate = true, TargetSeat = ownerSeat, SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{presentation.Name} · 分配牌", presentation.Description), Choices = _instructions.PresentDistribution(frame, choices, sourceBind, targetKind) }, _facts.IsHuman(ownerSeat));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome SelectProgramCardSubset(long frameId, int ownerSeat, string sourceBind, string resultBind, int minimumCards, int maximumCards, int maximumRankSum, SkillProgramSubsetAiOrder aiOrder, bool allowFewerWhenInsufficient, bool onePerSuit, bool availableAtSourceOnly = false)
    {
        var frame = _frames.ActiveProgram(frameId);
        var source = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == sourceBind);
        if (source is null)
        {
            _payments.Cancel(frame, $"选牌来源绑定“{sourceBind}”未生成，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (source.CardIds.Count != source.SourceLocations.Count)
            throw new InvalidOperationException("A program card-set binding lost its source locations.");
        var candidates = source.CardIds.Select((id, index) => (Id: id, Index: index));
        if (availableAtSourceOnly)
            candidates = candidates.Where(item => _facts.Location(item.Id) == source.SourceLocations[item.Index]);
        var cards = candidates.Select(item => _facts.CardsAt(source.SourceLocations[item.Index]).SingleOrDefault(card => card.Id == item.Id) ?? throw new InvalidOperationException("A bound card left its frozen source before selection.")).ToArray();
        if (availableAtSourceOnly && cards.Length == 0)
        {
            _payments.Cancel(frame, "此次获得的牌已全部离开原区域，剩余交牌结束。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        // 涉猎-style one-per-suit subsets must take exactly one card of every
        // distinct suit; the constraint leaves no room for partial takes.
        var constraintMinimum = minimumCards;
        var constraintMaximum = maximumCards;
        var promptMinimum = minimumCards;
        var promptMaximum = maximumCards;
        if (onePerSuit)
        {
            var distinctSuits = cards.Select(card => card.Suit).Distinct().Count();
            constraintMinimum = constraintMaximum = distinctSuits;
            promptMinimum = promptMaximum = distinctSuits;
        }
        else
        {
            // A program may explicitly allow a depleted revealed set to satisfy an
            // otherwise exact count. All existing exact-count programs remain strict.
            constraintMinimum = allowFewerWhenInsufficient ? Math.Min(minimumCards, cards.Length) : minimumCards;
            constraintMaximum = allowFewerWhenInsufficient ? Math.Min(maximumCards, cards.Length) : maximumCards;
        }

        var options = CardSubsetSelector.Enumerate(cards.Select(card => new CardSubsetCandidate(card.Id, card.Rank, card.Suit)).ToArray(), new CardSubsetConstraint(constraintMinimum, constraintMaximum, maximumRankSum, onePerSuit));
        if (options.Count == 0)
        {
            _payments.Cancel(frame, "选牌约束没有合法结果，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var choices = options.Select(option => new PromptChoice(new ChoiceId($"program-subset.frame-{frameId}.mask-{option.SelectionMask}"), option.CardIds.Count == 0 ? "不选择牌。" : $"选择 {string.Join("、", option.CardIds.Select(cardId => cards.Single(card => card.Id == cardId)).Select(card => $"【{card.DisplayName}】{card.RankText}"))}（点数和 {option.RankSum}）。", option.CardIds, [], new Dictionary<string, string> { ["program-action"] = "select-subset", ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["source-bind"] = sourceBind, ["result-bind"] = resultBind, ["rank-sum"] = option.RankSum.ToString(System.Globalization.CultureInfo.InvariantCulture), ["ai-order"] = aiOrder.ToString() })).ToArray();
        var presentation = _instructions.Skill(frame.SkillId);
        _prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, ownerSeat, $"【{presentation.Name}】请选择符合约束的牌。", availableAtSourceOnly ? cards.Select(card => card.Id).ToArray() : source.CardIds, [], frame.WindowContext?.SourceSeat) { PromptId = _prompts.NextId(), IsPrivate = true, TargetSeat = ownerSeat, SkillPrompt = new SkillPromptPresentation(frame.SkillId, presentation.Name, $"{presentation.Name} · 选择牌", onePerSuit ? $"获得其中不同花色的牌各一张（共 {promptMaximum} 张），点数和不超过 {maximumRankSum}。" : $"选择 {promptMinimum} 至 {promptMaximum} 张牌，点数和不超过 {maximumRankSum}。"), Choices = Array.AsReadOnly(choices.ToArray()) }, _facts.IsHuman(ownerSeat));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void CleanupProgramBoundCards(ProgramSkillFrame frame, bool completed)
    {
        _payments.CleanupRepeatedJudgment(frame);
        _payments.CleanupDeckSlash(frame);
        var boundIds = frame.CardSetBindings.SelectMany(binding => binding.CardIds).Distinct().ToHashSet();
        var cards = _facts.CardsAt(CardLocation.Processing).Where(card => boundIds.Contains(card.Id)).ToArray();
        if (cards.Length > 0)
            _movement.MoveCards(cards, CardLocation.Processing, CardLocation.DiscardPile, new CardMoveReason($"skill-program.{frame.SkillId}.{RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame)}." + (completed ? "complete-cleanup" : "cancel-cleanup")));
    }

    internal SkillProgramStepOutcome MoveProgramBoundCards(long frameId, int ownerSeat, string sourceBind, string? exceptBind, SkillProgramCardDestination destination, CardZoneKind? destinationZone, CardMoveReason reason, IReadOnlyList<int>? bottomOrder = null)
    {
        var frame = _frames.ActiveProgram(frameId);
        frame = _payments.FreezeSequentialDiscardTop(frame, sourceBind, exceptBind, destination);
        frame = _payments.FreezeLostHpChained(frame, sourceBind, exceptBind, destination);
        var source = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == sourceBind);
        if (source is null)
        {
            _payments.Cancel(frame, $"移动来源绑定“{sourceBind}”未生成，技能结算已取消。");
            return SkillProgramStepOutcome.Continue;
        }

        var ids = source.CardIds.ToHashSet();
        if (exceptBind is { } excluded)
        {
            var except = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == excluded);
            if (except is null)
            {
                _payments.Cancel(frame, $"移动排除绑定“{excluded}”未生成，技能结算已取消。");
                return SkillProgramStepOutcome.Continue;
            }

            ids.ExceptWith(except.CardIds);
        }

        var selected = source.CardIds.Select((cardId, index) => (CardId: cardId, Location: source.SourceLocations[index])).Where(item => ids.Contains(item.CardId)).ToArray();
        if (selected.Any(item => _facts.Location(item.CardId) != item.Location))
            throw new InvalidOperationException("A bound card left its frozen source before its configured move.");
        if (destination == SkillProgramCardDestination.DiscardPile)
            selected = selected.Where(item => !_facts.ForeignDiscardPrevented(source.SelectionActorSeat ?? ownerSeat, _facts.CardsAt(item.Location).Single(card => card.Id == item.CardId), item.Location, OwnedCardMoveIntent.Discard)).ToArray();
        var pindianAcquisitionBefore = _payments.BeginWholeHandAcquisition(frame, sourceBind, destination);
        if (selected.Length == 0)
        {
            if (pindianAcquisitionBefore is { } emptyBefore)
                _payments.CommitWholeHandAcquisition(frameId, sourceBind, emptyBefore);
            return SkillProgramStepOutcome.Continue;
        }

        if (!_payments.PreparePhaseAlcohol(frame, sourceBind, reason))
            return SkillProgramStepOutcome.Continue;
        if (destination == SkillProgramCardDestination.DiscardPile)
            _payments.FreezeDiscardColors(frame, sourceBind, selected.Select(item => item.CardId).ToArray(), selected.Select(item => item.Location).ToArray());
        if (destination == SkillProgramCardDestination.DrawPileBottom && selected.Length > 1 && bottomOrder is null)
        {
            if (selected.Length > 4)
                throw new InvalidOperationException("At most four revealed cards may be privately ordered at the draw-pile bottom.");
            var orders = RuntimeRules_Core_ProgramLifecycle.EnumerateProgramCardOrders(selected.Select(item => item.CardId).ToArray()).ToArray();
            var choices = orders.Select((order, index) => new PromptChoice(new ChoiceId($"program-bottom.frame-{frameId}.order-{index}"), $"将 {string.Join("、", order.Select(cardId => _facts.CardsAt(CardLocation.Processing).Single(card => card.Id == cardId).DisplayName))} 依次置于牌堆底（从底向上）。", order, [], new Dictionary<string, string> { ["program-action"] = "order-bound-cards", ["source-bind"] = sourceBind, ["except-bind"] = exceptBind ?? string.Empty })).ToArray();
            var skill = _instructions.Skill(frame.SkillId);
            _prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, ownerSeat, $"【{skill.Name}】请安排牌堆底牌顺序。", selected.Select(item => item.CardId).ToArray(), [], ownerSeat) { PromptId = _prompts.NextId(), IsPrivate = true, TargetSeat = ownerSeat, SkillPrompt = new SkillPromptPresentation(frame.SkillId, skill.Name, $"{skill.Name} · 牌堆底顺序", "选择剩余牌从牌堆底向上的顺序。"), DeckReorder = new(frame.Id, Array.AsReadOnly(selected.Select(item => _snapshots.Snapshot(_facts.CardsAt(item.Location).Single(card => card.Id == item.CardId))).ToArray()), Array.AsReadOnly(Array.Empty<int>()), Array.AsReadOnly(Array.Empty<int>()), true, 0) { BottomOnly = true }, Choices = Array.AsReadOnly(choices) }, _facts.IsHuman(ownerSeat));
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (bottomOrder is not null)
        {
            if (destination != SkillProgramCardDestination.DrawPileBottom || !bottomOrder.OrderBy(id => id).SequenceEqual(selected.Select(item => item.CardId).OrderBy(id => id)))
                throw new InvalidOperationException("The bottom order must be an exact permutation of the bound cards.");
            selected = bottomOrder.Select(cardId => selected.Single(item => item.CardId == cardId)).ToArray();
        }

        if (destination == SkillProgramCardDestination.DrawPileBottom && selected.Any(item => item.Location != CardLocation.Processing))
            throw new InvalidOperationException("Draw-pile bottom placement requires revealed processing cards.");
        var target = destination switch
        {
            SkillProgramCardDestination.OwnerHand => CardLocation.Hand(ownerSeat),
            SkillProgramCardDestination.DiscardPile => CardLocation.DiscardPile,
            SkillProgramCardDestination.DrawPileBottom => CardLocation.DrawPile,
            SkillProgramCardDestination.DrawPileTop => CardLocation.DrawPile,
            SkillProgramCardDestination.SelectedTargetHand when frame.SelectedTargetSeats.Count == 1 => CardLocation.Hand(frame.SelectedTargetSeats.Single()),
            SkillProgramCardDestination.PhaseOwnerHand when frame.WindowContext is { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } phaseSeat } && _facts.IsValidSeat(phaseSeat) => CardLocation.Hand(phaseSeat),
            SkillProgramCardDestination.OwnerPersistentZone when destinationZone is CardZoneKind.WoodenOxGrain or CardZoneKind.BuquWound or CardZoneKind.Authority or CardZoneKind.Chunlao or CardZoneKind.PrivateReserve => new CardLocation(destinationZone.Value, ownerSeat),
            _ => throw new InvalidOperationException($"Unsupported program card destination '{destination}'.")};
        if (destination == SkillProgramCardDestination.SelectedTargetHand && ProgramInstructionResolver.Default.Resolve(frame, _instructions.Skill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.AwaitMovementTriggers)
            return _payments.BeginParticipantGift(frame, sourceBind, selected.Select(item => item.CardId).ToArray(), target, reason);
        NativeLogicalDiscardInvocationDraft? logicalDiscard = null;
        if (_logicalDiscards.TracksInvocations && destination == SkillProgramCardDestination.DiscardPile)
        {
            // Discarding owned materials is distinct from disposing of revealed
            // draw-pile cards or cleaning up a use's Processing materials.
            var primaryIds = selected.Where(item => RuntimeRules_Core_ProgramDiscardOrigins.IsOwnedDiscardSource(item.Location) || item.Location == CardLocation.Processing && _facts.LastMovement(item.CardId)is { To: var to } entry && to == CardLocation.Processing && RuntimeRules_Core_ProgramDiscardOrigins.IsOwnedDiscardSource(entry.From) && RuntimeRules_Core_ProgramDiscardOrigins.IsProgramDiscardOriginReason(entry.Reason)).Select(item => item.CardId).ToArray();
            logicalDiscard = _logicalDiscards.Capture(frame.OwnerSeat, source.SelectionActorSeat ?? ownerSeat, primaryIds, reason, frame.Id);
        }

        if (destination == SkillProgramCardDestination.DiscardPile && selected.Select(item => item.Location).Distinct().Count() > 1 && (ProgramInstructionResolver.Default.Resolve(frame, _instructions.Skill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.AwaitMovementTriggers || _instructions.IsAtomicDiscard(frame, sourceBind) || _instructions.HasAtomicDiscardCapability))
        {
            _movement.MoveMultipleSources(selected.Select(item => item.CardId).ToArray(), target, reason, disclosure: CardMovementDisclosure.FaceUp);
            _logicalDiscards.Complete(logicalDiscard);
            return SkillProgramStepOutcome.Continue;
        }

        var nativeBoundGiveBefore = _movement.Sequence;
        var nativeBoundGiver = NativeGiveSemantics.BoundGiver(frame, sourceBind, selected.Select(x => x.Location).ToArray(), target);
        foreach (var group in selected.GroupBy(item => item.Location))
        {
            if (group.Key == target)
                continue;
            var cards = group.Select(item => _facts.CardsAt(group.Key).Single(card => card.Id == item.CardId)).ToArray();
            _movement.MoveCards(cards, group.Key, target, reason, nativeBoundGiver is { } giver ? new ProgramBoundCardGive(frame.Id, giver, target.OwnerSeat!.Value, selected.Select(x => x.CardId).ToArray(), nativeBoundGiveBefore) : null, disclosure: destination == SkillProgramCardDestination.DiscardPile ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.Unspecified);
            switch (destination)
            {
                case SkillProgramCardDestination.DrawPileBottom:
                    if (group.Key != CardLocation.Processing)
                        throw new InvalidOperationException("Draw-pile bottom placement requires revealed processing cards.");
                    _movement.PlaceAtBottom(cards.Select(card => card.Id).ToArray());
                    break;
                case SkillProgramCardDestination.DrawPileTop:
                    // The moved segment sits at the pile's top; restore the
                    // binding order so its first id is drawn next.
                    _movement.PlaceAtTop(cards.Select(card => card.Id).ToArray());
                    break;
            }
        }

        _logicalDiscards.Complete(logicalDiscard);
        if (pindianAcquisitionBefore is { } acquisitionBefore)
            _payments.CommitWholeHandAcquisition(frameId, sourceBind, acquisitionBefore);
        _payments.CommitLostHpChained(frameId);
        _payments.CommitPhaseAlcohol(frameId);
        return SkillProgramStepOutcome.Continue;
    }

    internal void ResolveProgramBoundCardOrderAnswer(PromptChoice selected)
    {
        var frame = _frames.Top as ProgramSkillFrame ?? throw new InvalidOperationException("The bottom-order choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _instructions.Skill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.MoveBoundCards, Destination: SkillProgramCardDestination.DrawPileBottom } effect || selected.Parameters.GetValueOrDefault("source-bind") != effect.SourceBind || selected.Parameters.GetValueOrDefault("except-bind") != (effect.ExceptBind ?? string.Empty))
            throw new InvalidOperationException("The bottom-order choice does not match its suspended instruction.");
        _prompts.Clear();
        _ = MoveProgramBoundCards(frame.Id, frame.OwnerSeat, effect.SourceBind!, effect.ExceptBind, SkillProgramCardDestination.DrawPileBottom, null, new CardMoveReason($"skill-program.{frame.SkillId}.{effect.Op}"), selected.Cards);
        _frames.AdvanceProgram(frame.Id);
    }

    internal void ResolveProgramCardSubsetAnswer(PromptChoice selected)
    {
        var frame = _frames.Top as ProgramSkillFrame ?? throw new InvalidOperationException("The subset choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _instructions.Skill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.SelectCardSubset } effect || selected.Parameters.GetValueOrDefault("source-bind") != effect.SourceBind || selected.Parameters.GetValueOrDefault("result-bind") != effect.ResultBind)
            throw new InvalidOperationException("The subset choice does not match the suspended instruction.");
        var source = GetProgramCardSet(frame, effect.SourceBind!);
        var sourceIndexes = source.CardIds.Select((cardId, index) => (cardId, index)).ToDictionary(item => item.cardId, item => item.index);
        var cards = selected.Cards.Select(cardId =>
        {
            if (!sourceIndexes.TryGetValue(cardId, out var sourceIndex) || sourceIndex >= source.SourceLocations.Count)
            {
                throw new InvalidOperationException("The selected program card lost its frozen source.");
            }

            return _facts.CardsAt(source.SourceLocations[sourceIndex]).Single(card => card.Id == cardId);
        }).ToArray();
        var rankSum = cards.Sum(card => card.Rank);
        if (rankSum > effect.MaximumRankSum)
            throw new InvalidOperationException("The selected subset exceeds its rank-sum limit.");
        if (effect.OnePerSuit ? cards.Length != (effect.AvailableAtSourceOnly == true ? CountAvailableBoundSuits(source) : source.CardIds.Select((cardId, index) => _facts.CardsAt(source.SourceLocations[index]).Single(card => card.Id == cardId).Suit).Distinct().Count()) || cards.Select(card => card.Suit).Distinct().Count() != cards.Length : selected.Cards.Distinct().Count() != selected.Cards.Count || selected.Cards.Any(cardId => !source.CardIds.Contains(cardId)) || selected.Cards.Count < (effect.AllowFewerWhenInsufficient ? Math.Min(effect.MinimumCards, effect.AvailableAtSourceOnly == true ? CountAvailableBoundCards(source) : source.CardIds.Count) : effect.MinimumCards) || selected.Cards.Count > (effect.AllowFewerWhenInsufficient ? Math.Min(effect.MaximumCards, effect.AvailableAtSourceOnly == true ? CountAvailableBoundCards(source) : source.CardIds.Count) : effect.MaximumCards))
            throw new InvalidOperationException("The selected subset is no longer legal.");
        _prompts.Clear();
        SetProgramCardSet(frame.Id, effect.ResultBind!, selected.Cards, SkillProgramCardSetVisibility.Private, selectionActorSeat: source.SelectionActorSeat);
        _events.AdvanceFact(new ProgramCardSubsetSelectedEvent(frame.Id, frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), effect.SourceBind!, effect.ResultBind!, Array.AsReadOnly(selected.Cards.ToArray()), rankSum));
        _frames.AdvanceProgram(frame.Id);
    }

    internal void ResolveSourceCardChoice(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected)
    {
        if (_instructions.HasForeignDiscardCapability && _prompts.Current?.PlayerSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Source selection lost its actual choosing actor.");
        if (effect.Op != SkillProgramEffectOp.SelectSourceCard || selected.Parameters.GetValueOrDefault("result-bind") != effect.ResultBind || !Enum.TryParse<CardZoneKind>(selected.Parameters.GetValueOrDefault("source-zone"), out var zone) || !effect.Zones.Contains(zone) || (effect.CardSource switch
        {
            SkillProgramCardSource.Owner => frame.OwnerSeat,
            SkillProgramCardSource.DamageSource => frame.WindowContext?.SourceSeat,
            SkillProgramCardSource.EventTarget => frame.WindowContext?.TargetSeat,
            _ => null
        })is not { } sourceSeat || effect.CardSource == SkillProgramCardSource.DamageSource && sourceSeat == frame.OwnerSeat && !effect.AllowSameSource)
            throw new InvalidOperationException("The source-card choice does not match the suspended instruction.");
        Card card;
        CardLocation location;
        if (zone == CardZoneKind.Hand)
        {
            if (selected.Cards.Count != 0 || !int.TryParse(selected.Parameters.GetValueOrDefault("slot-index"), System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var slot) || slot < 0 || slot >= _facts.Hand(sourceSeat).Count)
                throw new InvalidOperationException("The selected opaque hand slot is no longer legal.");
            card = _facts.Hand(sourceSeat)[slot];
            location = CardLocation.Hand(sourceSeat);
        }
        else if (zone == CardZoneKind.Equipment)
        {
            if (selected.Cards.Count != 1)
                throw new InvalidOperationException("The equipment selection is malformed.");
            location = CardLocation.Equipment(sourceSeat);
            card = _facts.Equipment(sourceSeat).SingleOrDefault(item => item.Id == selected.Cards.Single()) ?? throw new InvalidOperationException("The selected equipment is no longer available.");
            if (effect.EquipmentSlots.Count > 0 && !effect.EquipmentSlots.Contains(_facts.FieldEquipmentSlot(card)))
                throw new InvalidOperationException("The selected equipment slot is no longer legal.");
        }
        else if (effect.CardSource == SkillProgramCardSource.Owner && zone is CardZoneKind.WoodenOxGrain or CardZoneKind.BuquWound or CardZoneKind.Authority or CardZoneKind.Chunlao)
        {
            if (selected.Cards.Count != 1)
                throw new InvalidOperationException("The owner-pile selection is malformed.");
            location = new CardLocation(zone, sourceSeat);
            card = _facts.CardsAt(location).SingleOrDefault(item => item.Id == selected.Cards.Single()) ?? throw new InvalidOperationException("The selected owner-pile card is no longer available.");
        }
        else
        {
            throw new InvalidOperationException("The configured source-card zone is unsupported.");
        }

        _prompts.Clear();
        SetProgramCardSet(frame.Id, effect.ResultBind!, [card.Id], SkillProgramCardSetVisibility.Private, [location], selectionActorSeat: frame.OwnerSeat);
        _frames.AdvanceProgram(frame.Id);
        return;
    }

    internal void ResolveBoundCardGiftChoice(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected, string action)
    {
        if (effect is not { Op: SkillProgramEffectOp.GiveBoundCard, SourceBind: { } sourceBind, TargetKind: { } targetKind } || selected.Parameters.GetValueOrDefault("source-bind") != sourceBind || selected.Parameters.GetValueOrDefault("target-kind") != targetKind.ToString())
            throw new InvalidOperationException("The bound-card choice does not match the suspended instruction.");
        var binding = GetProgramCardSet(frame, sourceBind);
        if (action == "keep-bound-cards")
        {
            if (selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The retain-cards choice is malformed.");
            _prompts.Clear();
            _frames.AdvanceProgram(frame.Id);
            return;
        }

        if (selected.Cards.Count != 1 || selected.Targets.Count != 1)
            throw new InvalidOperationException("The bound-card gift choice is malformed.");
        var cardId = selected.Cards.Single();
        var targetSeat = selected.Targets.Single();
        var indexes = binding.CardIds.Select((id, position) => (id, position)).ToDictionary(item => item.id, item => item.position);
        if (!indexes.TryGetValue(cardId, out var index) || index >= binding.SourceLocations.Count || !_instructions.TargetSeats(frame.OwnerSeat, targetKind).Contains(targetSeat))
            throw new InvalidOperationException("The bound-card gift is no longer legal.");
        var from = binding.SourceLocations[index];
        if (_facts.Location(cardId) != from)
            throw new InvalidOperationException("The bound card left its frozen source before the gift.");
        var card = _facts.CardsAt(from).Single(item => item.Id == cardId);
        _prompts.Clear();
        _movement.Move(card, from, CardLocation.Hand(targetSeat), new CardMoveReason($"skill-program.{frame.SkillId}.{effect.Op}"), new ProgramBoundCardGive(frame.Id, frame.OwnerSeat, targetSeat, [cardId], _movement.Sequence));
        _events.AdvanceFact(new ProgramBoundCardGivenEvent(frame.Id, frame.SkillId, RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, targetSeat, cardId));
        _frames.AdvanceProgram(frame.Id);
        return;
    }
}
