#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHellKingProgramsRuntime
{
    SkillProgramStepOutcome IHellKingProgramHost.ExecuteHellKing(ProgramSkillFrame frame, SkillProgramEffect effect) => ExecuteHellKing(frame, effect);
    internal SkillProgramStepOutcome ExecuteHellKing(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        switch (effect.Op)
        {
            case SkillProgramEffectOp.BindHellRandomOther:
                BindHellParticipant(frame, effect, RandomHellSeat(HellOtherSeats(frame.OwnerSeat)));
                break;
            case SkillProgramEffectOp.BindHellOtherDamageSource:
                if (frame.WindowContext?.Window != SkillProgramTriggerWindow.AfterDamageApplied)
                    throw new InvalidOperationException("Abi requires the actual applied-damage window.");
                BindHellParticipant(frame, effect, frame.WindowContext.SourceSeat is { } source && source != frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[source].IsAlive ? source : null);
                break;
            case SkillProgramEffectOp.BindHellPeachPoisonTarget:
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { EffectiveKind: CardKind.Peach } peach })
                    throw new InvalidOperationException("Poison requires a real committed Peach Use.");
                BindHellParticipant(frame, effect, peach.ActorSeat == frame.OwnerSeat ? null : RandomHellSeat(HellOtherSeats(frame.OwnerSeat).Where(s => s != peach.ActorSeat).ToArray()));
                break;
            case SkillProgramEffectOp.ChooseHellLowestHpOther:
            case SkillProgramEffectOp.ChooseHellHighestHpOther:
            case SkillProgramEffectOp.ChooseHellNextHighestHpOther:
            case SkillProgramEffectOp.ChooseHellReincarnationTarget:
                return ChooseHellExtremeParticipant(frame, effect);
            case SkillProgramEffectOp.RollHellElement:
                BindHellChoice(frame, effect, null, frame.SelectedTargetSeats.Count == 0 ? "none" : _randomOperations._random.Next(2) == 0 ? "fire" : "thunder", selectsTarget: false);
                break;
            case SkillProgramEffectOp.RollHellAssault:
                BindHellChoice(frame, effect, null, _randomOperations._random.Next(2) == 0 ? "nanman" : "wanjian", selectsTarget: false);
                break;
            case SkillProgramEffectOp.BindHellRepeatedDamageTarget:
            {
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, DamageFrameId: { } damage })
                    throw new InvalidOperationException("Fanshi requires one actual damage instance.");
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<HellFanshiDamageCountedEvent>().Any(e => e.OwnerSeat == frame.OwnerSeat && e.DamageFrameId == damage))
                    throw new InvalidOperationException("Fanshi cannot count the same damage twice.");
                var later = _coreProgramEventHistoryOperations.ProgramEventHistory<HellFanshiDamageCountedEvent>().Any(e => e.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber && e.OwnerSeat == frame.OwnerSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellFanshiDamageCountedEvent(_coreActualTurnProgressTurnNumberAccess._turnNumber, frame.OwnerSeat, damage));
                BindHellParticipant(frame, effect, later ? RandomHellSeat(HellOtherSeats(frame.OwnerSeat)) : null);
                break;
            }

            case SkillProgramEffectOp.DiscardHellRandomOtherMount:
            {
                var cards = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat).SelectMany(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Where(c => EquipmentCatalog.Get(c.Kind).Slot is (EquipmentSlot.OffensiveHorse or EquipmentSlot.DefensiveHorse) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(p.Seat, c, CardLocation.Equipment(p.Seat), OwnedCardMoveIntent.Discard)).Select(c => (Card: c, Owner: p.Seat))).ToArray();
                if (cards.Length == 0)
                    break;
                var item = cards[_randomOperations._random.Next(cards.Length)];
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, item.Owner, item.Card, CardLocation.Equipment(item.Owner), reason, parentFrameId: frame.Id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellRandomCardDiscardedEvent(frame.Id, frame.OwnerSeat, item.Owner, item.Card.Id));
                break;
            }

            case SkillProgramEffectOp.RewardHellRecoverySource:
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery } recovery } || recovery.TargetSeat != frame.OwnerSeat)
                    throw new InvalidOperationException("Enyuan requires its actual owner recovery.");
                if (recovery.SourceSeat is { } healer && healer != frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[healer].IsAlive)
                    return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, healer, 1, reason);
                break;
            case SkillProgramEffectOp.DrawHellPreparation:
                return _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, frame.OwnerSeat, 2, reason);
            case SkillProgramEffectOp.DiscardHellOutsideDrawGain:
            {
                if (frame.WindowContext is not { TargetSeat: { } gainer } context || !IsHellZhiwangGain(context.MovementBatch, frame.OwnerSeat, gainer))
                    throw new InvalidOperationException("Zhiwang lost its exact outside-draw hand-gain batch.");
                var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[gainer]).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(gainer, c, CardLocation.Hand(gainer), OwnedCardMoveIntent.Discard)).ToArray();
                if (!_coreMatchParticipantsPlayersAccess._players[gainer].IsAlive || hand.Length == 0)
                    break;
                var card = hand[_randomOperations._random.Next(hand.Length)];
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(frame.OwnerSeat, gainer, card, CardLocation.Hand(gainer), reason, parentFrameId: frame.Id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellRandomCardDiscardedEvent(frame.Id, frame.OwnerSeat, gainer, card.Id));
                break;
            }

            case SkillProgramEffectOp.ExchangeHellReincarnationHp:
            {
                if (frame.SelectedTargetSeats is not [var other] || !owner.IsAlive || !_coreMatchParticipantsPlayersAccess._players[other].IsAlive || !owner.HasHpValue || !_coreMatchParticipantsPlayersAccess._players[other].HasHpValue)
                    break;
                var peer = _coreMatchParticipantsPlayersAccess._players[other];
                var ownBefore = owner.Hp;
                var otherBefore = peer.Hp;
                owner.Hp = Math.Min(owner.MaxHp, otherBefore);
                peer.Hp = Math.Min(peer.MaxHp, ownBefore);
                _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, owner.Seat, owner.Seat, ownBefore, owner.Hp, HpChangeKind.Setting);
                _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, owner.Seat, other, otherBefore, peer.Hp, HpChangeKind.Setting);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellCurrentHpExchangedEvent(frame.Id, owner.Seat, other, ownBefore, otherBefore, owner.Hp, peer.Hp));
                break;
            }

            case SkillProgramEffectOp.UseHellPreparationAlcohol:
                return BeginHellPreparationAlcohol(frame);
            case SkillProgramEffectOp.SetHellOthersChained:
            case SkillProgramEffectOp.SetHellAllLivingChained:
                foreach (var peer in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (effect.Op == SkillProgramEffectOp.SetHellAllLivingChained || p.Seat != frame.OwnerSeat)).ToArray())
                    _coreProgramLifecycleOperations.SetProgramChainedState(frame, true, peer.Seat);
                break;
            case SkillProgramEffectOp.TurnHellFaceUpKiller:
                if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } death)
                    throw new InvalidOperationException("Icebound requires its actual owner death.");
                if (death.SourceSeat is { } killer && killer != frame.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[killer].IsAlive && !_coreMatchParticipantsPlayersAccess._players[killer].IsFaceDown)
                    _coreProgramLifecycleOperations.TurnOverProgramTarget(frame.Id, frame.OwnerSeat, killer);
                break;
            default:
                throw new InvalidOperationException("Unsupported Hell king operation.");
        }

        return SkillProgramStepOutcome.Continue;
    }

    internal int[] HellOtherSeats(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).Select(p => p.Seat).ToArray();
    internal int? RandomHellSeat(IReadOnlyList<int> seats) => seats.Count == 0 ? null : seats[_randomOperations._random.Next(seats.Count)];
    internal void BindHellParticipant(ProgramSkillFrame frame, SkillProgramEffect effect, int? seat) => BindHellChoice(frame, effect, seat, seat is null ? "none" : "eligible", selectsTarget: true);
    internal void BindHellChoice(ProgramSkillFrame frame, SkillProgramEffect effect, int? target, string option, bool selectsTarget)
    {
        if (frame.ChoiceBindings.Any(b => b.Name == effect.ResultBind))
            throw new InvalidOperationException("A Hell choice cannot be rolled or selected twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { SelectedTargetSeats = selectsTarget ? target is { } actual ? Array.AsReadOnly(new[] { actual }) : [] : frame.SelectedTargetSeats, ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, option, frame.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HellKingChoiceRecordedEvent(frame.Id, frame.InstructionIndex, frame.OwnerSeat, effect.ResultBind!, option, target, _coreActualTurnProgressTurnNumberAccess._turnNumber));
    }

    internal int[] HellExtremeParticipants(ProgramSkillFrame frame, SkillProgramEffectOp op)
    {
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != frame.OwnerSeat && p.HasHpValue);
        if (op == SkillProgramEffectOp.ChooseHellReincarnationTarget)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].HasHpValue || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp > 2)
                return[];
            candidates = candidates.Where(p => p.Hp > 2);
        }

        var all = candidates.ToArray();
        if (all.Length == 0)
            return[];
        var extreme = op == SkillProgramEffectOp.ChooseHellLowestHpOther ? all.Min(p => p.Hp) : all.Max(p => p.Hp);
        return all.Where(p => p.Hp == extreme).Select(p => p.Seat).ToArray();
    }

    internal SkillProgramStepOutcome ChooseHellExtremeParticipant(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var seats = HellExtremeParticipants(frame, effect.Op);
        if (seats.Length <= 1)
        {
            BindHellParticipant(frame, effect, seats.Length == 1 ? seats[0] : null);
            return SkillProgramStepOutcome.Continue;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "选择一名满足当前体力条件的角色。", [], seats, frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(seats.Select(seat => new PromptChoice(new($"hell-extreme.{(frame.Id)}.{(frame.InstructionIndex)}.{(seat)}"), $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", [], [seat], new Dictionary<string, string> { ["program-action"] = "hell-king-extreme", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture) })).ToArray())
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("hell-king-extreme")]
    internal void AnswerHellExtreme(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("A Hell target lost its program parent.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (!IsHellExtremeOp(effect.Op) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || choice.Cards.Count != 0 || choice.Targets is not [var target] || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || !HellExtremeParticipants(frame, effect.Op).Contains(target))
            throw new InvalidOperationException("A Hell target lost its exact instruction or current extreme HP.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        BindHellParticipant(frame, effect, target);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseHellLowestHpOther, SkillProgramEffectOp.ChooseHellHighestHpOther, SkillProgramEffectOp.ChooseHellNextHighestHpOther, SkillProgramEffectOp.ChooseHellReincarnationTarget)]
    internal PromptChoice SelectAiHellExtreme(PendingDecision decision) => decision.Choices.OrderBy(c => c.Targets.Single()).First();
    internal static bool IsHellExtremeOp(SkillProgramEffectOp op) => op is SkillProgramEffectOp.ChooseHellLowestHpOther or SkillProgramEffectOp.ChooseHellHighestHpOther or SkillProgramEffectOp.ChooseHellNextHighestHpOther or SkillProgramEffectOp.ChooseHellReincarnationTarget;
    internal static bool IsHellTargetOp(SkillProgramEffectOp op) => IsHellExtremeOp(op) || op is SkillProgramEffectOp.BindHellRandomOther or SkillProgramEffectOp.BindHellOtherDamageSource or SkillProgramEffectOp.BindHellPeachPoisonTarget or SkillProgramEffectOp.BindHellRepeatedDamageTarget;
    internal bool IsHellKingChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        var producers = plan.Instructions.Take(frame.InstructionIndex).Select((e, i) => (Effect: e, Index: i + 1)).Where(p => p.Effect.ResultBind == binding.Name && (IsHellTargetOp(p.Effect.Op) || p.Effect.Op is SkillProgramEffectOp.RollHellElement or SkillProgramEffectOp.RollHellAssault)).ToArray();
        if (producers is not [var producer] || binding.ChooserSeat != frame.OwnerSeat)
            return false;
        var receipt = _coreProgramEventHistoryOperations.ProgramEventHistory<HellKingChoiceRecordedEvent>().SingleOrDefault(e => e.ProgramFrameId == frame.Id && e.InstructionIndex == producer.Index && e.ResultBind == binding.Name);
        return receipt is not null && receipt.OwnerSeat == frame.OwnerSeat && receipt.OptionId == binding.OptionId && (IsHellTargetOp(producer.Effect.Op) ? receipt.OptionId == (receipt.TargetSeat is null ? "none" : "eligible") && (receipt.TargetSeat is null || _coreTableSeatQueriesOperations.IsValidPlayerSeat(receipt.TargetSeat.Value) && receipt.TargetSeat != frame.OwnerSeat) : receipt.TargetSeat is null && (producer.Effect.Op == SkillProgramEffectOp.RollHellElement ? receipt.OptionId is "fire" or "thunder" or "none" : receipt.OptionId is "nanman" or "wanjian"));
    }

    internal bool IsHellRandomAssaultBranch(ProgramSkillFrame frame, SkillProgramEffect effect) => effect is { Op: SkillProgramEffectOp.UseVirtualOrdinaryTrick, Condition: { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: { } bind, OptionId: { } option } } && frame.WindowContext?.Window == SkillProgramTriggerWindow.PlayPhaseStarting && ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Take(frame.InstructionIndex - 1).Any(e => e.Op == SkillProgramEffectOp.RollHellAssault && e.ResultBind == bind) && frame.ChoiceBindings.SingleOrDefault(b => b.Name == bind)is { } result && result.OptionId == option && IsHellKingChoiceResult(frame, result) && (effect.OutputKind == CardKind.BarbarianAssault && option == "nanman" || effect.OutputKind == CardKind.ArrowBarrage && option == "wanjian");

    [ProgramNativeDrawReturn(SkillProgramEffectOp.RewardHellRecoverySource, SkillProgramEffectOp.DrawHellPreparation)]
    internal void ReturnHellNativeDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var recipient = effect.Op == SkillProgramEffectOp.DrawHellPreparation ? frame.OwnerSeat : frame.WindowContext?.HpChange?.SourceSeat;
        var amount = effect.Op == SkillProgramEffectOp.DrawHellPreparation ? 2 : 1;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.NativeDraw is not null || recipient is null || outcome.RecipientSeat != recipient || outcome.RequestedCount != amount || outcome.SequenceAfter < outcome.SequenceBefore || outcome.SequenceAfter > _coreOutsidePhaseDrawDiscardOperations.OutsidePhaseMovementSequence || outcome.ActualDrawIds.Count > amount || outcome.ActualDrawIds.Distinct().Count() != outcome.ActualDrawIds.Count || outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0 || outcome.AttemptFrameId is { } attempt && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawAttemptDecidedEvent>().Count(e => e.AttemptFrameId == attempt && e.ParentFrameId == frame.Id && e.RecipientSeat == recipient && e.RequestedCount == amount && e.Replaced == (outcome.Replacement is not null)) != 1)
            throw new InvalidOperationException("A Hell draw lost its exact recipient, operation or typed native return.");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(frame.Id, recipient.Value, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(frame.SkillId)}.{(effect.Op)}"), 0, outcome.ActualDrawIds);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool HasExactHellPreparationParent(ProgramSkillFrame frame) => frame.WindowContext is { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, TargetSeat: { } actor } context && actor != frame.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(actor) && frame.SelectedTargetSeats.SequenceEqual([actor]) && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(parent => parent.Id == context.ParentFrameId && parent.OwnerSeat == actor && parent.Window == context.Window && parent.CandidateIndex < parent.Candidates.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]));
    internal SkillProgramStepOutcome BeginHellPreparationAlcohol(ProgramSkillFrame frame)
    {
        if (!HasExactHellPreparationParent(frame) || frame.VirtualBasicDraft is not null)
            throw new InvalidOperationException("Bitter Wine requires its exact other-character preparation parent.");
        var actor = _coreMatchParticipantsPlayersAccess._players[frame.SelectedTargetSeats.Single()];
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || actor.HasAlcoholEffect || actor.UsedPlayPhaseAlcoholThisTurn || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, CardKind.Alcohol, CardActionType.Use) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, CardKind.Alcohol) || _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, CardKind.Alcohol, Suit.None))
            return SkillProgramStepOutcome.Continue;
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { VirtualBasicDraft = new(frame.InstructionIndex, id) });
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Use, actor.Seat, frame.OwnerSeat, null, null, null, CardKind.Alcohol, [actor.Seat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(id, actor.Seat, 0, CardKind.Alcohol, [actor.Seat], PhysicalCardIds: []) { Action = action, VirtualBasicReturn = new(frame.Id, frame.InstructionIndex), FirstOwnPlayUseDistanceUnlimited = _coreIssuedPlayUsePoliciesOperations.HasFirstActualPlayUseDistance(actor) });
        _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(id, 0, CardKind.Alcohol, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(id, [actor.Seat]));
        _characterLuMengMouOperations.RecordYingboCardUse(id, actor.Seat, CardKind.Alcohol);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Alcohol);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
        return SkillProgramStepOutcome.AwaitChild;
    }
    internal bool HasHellZhiwangHolder => _contentRegistryOperations._contentRegistry.Skills.ContainsKey("hell:zhiwang") && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(p, "hell:zhiwang"));

    internal static bool IsHellZhiwangTrigger(SkillProgramTrigger trigger) => trigger.Window == SkillProgramTriggerWindow.CardsGained && trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardHellOutsideDrawGain);
    internal bool IsHellZhiwangMovement(CardMovementBatchContext batch, CardMovementRecord move, int gainer)
    {
        if (move.To != CardLocation.Hand(gainer) || move.From == move.To || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move))
            return false;
        if (move.From != CardLocation.DrawPile)
            return true;
        var invocations = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == gainer && e.ParentFrameId == batch.ParentFrameId && e.TurnNumber == batch.TurnNumber && e.Reason == move.Reason && move.Sequence > e.SequenceBefore && move.Sequence <= e.SequenceAfter).ToArray();
        // Taking a particular deck card is a gain, but is not necessarily DrawCards.
        if (invocations.Length == 0)
            return true;
        if (invocations is not [var actual])
            throw new InvalidOperationException("A Hell hand gain belongs to overlapping native draw invocations.");
        return actual.LastBatchId == batch.Id && batch.NativeDrawInvocation is { } proof && proof.InvocationId == actual.InvocationId && proof.OwnerSeat == gainer && proof.LastBatchId == batch.Id && proof.ParentFrameId == batch.ParentFrameId && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(proof) && proof.Materials.Any(m => m.CardId == move.CardId && m.PrintedKind == move.CardKind && m.MovementSequence == move.Sequence);
    }

    internal bool IsHellZhiwangGain(CardMovementBatchContext? batch, int owner, int gainer) => batch?.MovementTiming is { Phase: not TurnPhase.Draw } && owner != gainer && _coreTableSeatQueriesOperations.IsValidPlayerSeat(gainer) && batch.Movements.Any(m => IsHellZhiwangMovement(batch, m, gainer));
    internal IEnumerable<ProgramTriggerCandidate> CollectHellZhiwangCandidates(CardMovementBatchContext batch) => batch.MovementTiming is not { Phase: not TurnPhase.Draw } ? [] : _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).SelectMany(p => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(p, SkillProgramTriggerWindow.CardsGained)).Where(c => IsHellZhiwangTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(c))).SelectMany(c => batch.Movements.Where(m => m.To.Zone == CardZoneKind.Hand && m.To.OwnerSeat is { } s && s != c.OwnerSeat && IsHellZhiwangMovement(batch, m, s)).Select(m => m.To.OwnerSeat!.Value).Distinct().Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Select(s => c with { OccurrenceIndex = s }));
    internal ProgramSkillWindowContext CreateHellZhiwangContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate) => new(SkillProgramTriggerWindow.CardsGained, parent.Id, candidate.OwnerSeat, SourceSeat: candidate.OwnerSeat, TargetSeat: candidate.OccurrenceIndex, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { MovedCardCount = parent.Batch.Movements.Count(m => m.To == CardLocation.Hand(candidate.OccurrenceIndex) && m.From != m.To) }, MovementBatch: parent.Batch);
    internal bool CanRunHellZhiwangCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.CardsGained && context.OwnerSeat == candidate.OwnerSeat && context.SourceSeat == candidate.OwnerSeat && context.TargetSeat == candidate.OccurrenceIndex && IsHellZhiwangGain(context.MovementBatch, candidate.OwnerSeat, candidate.OccurrenceIndex) && _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is { } parent && parent.Id == context.ParentFrameId && parent.Batch == context.MovementBatch && parent.Candidates.ElementAtOrDefault(parent.CandidateIndex) == candidate;
    internal bool HasExactHellKingDeathPrefix(ProgramSkillFrame root, out DyingFrame? originalDying, out long? originalAttack)
    {
        originalDying = null;
        originalAttack = null;
        return root.SkillId is ("hell:wangxiang" or "hell:bingfeng" or "hell:heisheng" or "hell:zhuxin" or "hell:leizhu" or "hell:renao" or "hell:huoxing" or "hell:pingdeng") && HasExactHellDeathOriginalCursor(root, out originalDying, out originalAttack);
    }

    internal bool HasExactHellDeathOriginalCursor(ProgramSkillFrame root, out DyingFrame? originalDying, out long? originalAttack, int directDepth = 0)
    {
        originalDying = null;
        originalAttack = null;
        if (directDepth >= _coreMatchDefinitionPlayerCountAccess._playerCount || root.WindowContext is not { Window: SkillProgramTriggerWindow.OwnerDied } context || context.OwnerSeat != root.OwnerSeat || context.TargetSeat != root.OwnerSeat || root.TriggerId != root.ActivationId || _coreMatchParticipantsPlayersAccess._players[root.OwnerSeat].IsAlive)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        if (index < 3 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramDeathTriggerWindowFrame window || window.Id != context.ParentFrameId || window.OwnerSeat != root.OwnerSeat || window.KillerSeat != context.SourceSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, window.Candidates[window.CandidateIndex]) || _coreProgramLifecycleOperations.GetProgramTrigger2(window.Candidates[window.CandidateIndex]).Window != context.Window || _resolutionStackOperations._resolutionStack[index - 2] is not DeathFrame death || death.Id != window.DeathFrameId || death.VictimSeat != root.OwnerSeat || death.KillerSeat != window.KillerSeat)
            return false;
        if (death.ReturnKind == DeathReturnKind.ProgramSkill)
            return _resolutionStackOperations._resolutionStack[index - 3] is ProgramSkillFrame causer && HasExactHellWuhunDirectDeath(causer, death) && HasExactHellDeathOriginalCursor(causer, out originalDying, out originalAttack, directDepth + 1);
        if (death.ReturnKind != DeathReturnKind.Dying || _resolutionStackOperations._resolutionStack[index - 3] is not DyingFrame dying || dying.Id != death.ParentFrameId || dying.VictimSeat != death.VictimSeat || dying.KillerSeat != death.KillerSeat || dying.ResponderIndex != dying.ResponderSeats.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == dying.VictimSeat && e.KillerSeat == dying.KillerSeat) != 1)
            return false;
        if (dying.Continuation == DyingContinuationKind.Damage)
        {
            if (index < 4 || _resolutionStackOperations._resolutionStack[index - 4] is not DamageFrame damage || damage.Id != dying.ParentFrameId || damage.TargetSeat != dying.VictimSeat || damage.Amount <= 0)
                return false;
            var parent = _resolutionStackOperations._resolutionStack.Take(index - 4).SingleOrDefault(f => f.Id == damage.ParentFrameId);
            var cardState = parent switch
            {
                CardUseFrame card => card.CardAttack,
                ProgramSkillFrame program => program.CardAttack,
                JudgmentFrame judgment => judgment.CardAttack,
                _ => null
            };
            var paid = parent is ProgramSkillFrame { AttackAttempt: { DamageWasApplied: true } attack, AttackReturn: not null } ? attack.TargetSeat == damage.TargetSeat && attack.SourceSeat == damage.SourceSeat && attack.DamageAmount == damage.Amount && dying.KillerSeat == (attack.SourceLess ? null : attack.SourceSeat) : cardState is { Active: true, DamageWasApplied: true } cardAttack && cardAttack.TargetSeat == damage.TargetSeat && cardAttack.SourceSeat == damage.SourceSeat && cardAttack.DamageAmount == damage.Amount && dying.KillerSeat == (cardAttack.IsSourceLess ? null : cardAttack.SourceSeat);
            if (!paid)
                return false;
            originalAttack = damage.ParentFrameId;
        }
        else if (dying.Continuation == DyingContinuationKind.ProgramSkill)
        {
            if (index < 4 || _resolutionStackOperations._resolutionStack[index - 4] is not ProgramSkillFrame loss || loss.Id != dying.ParentFrameId || dying.KillerSeat is not null || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == loss.Id && e.SkillId == loss.SkillId && e.TargetSeat == dying.VictimSeat && e.Amount > 0 && e.RemainingHp <= 0))
                return false;
        }
        else if (dying.Continuation == DyingContinuationKind.AttackHpLoss)
        {
            if (index < 4 || dying.KillerSeat is not null || _resolutionStackOperations._resolutionStack[index - 4] is not { } parent || parent.Id != dying.ParentFrameId)
                return false;
            var cardAttack = parent switch
            {
                CardUseFrame card => card.CardAttack,
                ProgramSkillFrame program => program.CardAttack,
                JudgmentFrame judgment => judgment.CardAttack,
                _ => null
            };
            var attack = parent is ProgramSkillFrame { AttackAttempt: { } programAttack, AttackReturn: not null } ? programAttack : null;
            var target = attack?.TargetSeat ?? cardAttack?.TargetSeat;
            var source = attack?.SourceSeat ?? cardAttack?.SourceSeat;
            var amount = attack?.DamageAmount ?? cardAttack?.DamageAmount;
            if (target != dying.VictimSeat || amount is not > 0 || !_coreProgramEventHistoryOperations.ProgramEventHistory<DamageReplacedWithHpLossEvent>().Any(e => e.ParentResolutionId == parent.Id && e.TargetSeat == target && e.PolicyOwnerSeat == source && e.Amount == amount && e.RemainingHp <= 0) || _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == target && e.KillerSeat is null) != 1)
                return false;
            originalAttack = parent.Id;
        }
        else
            return false;
        originalDying = dying;
        return true;
    }

    internal bool HasExactHellWuhunDirectDeath(ProgramSkillFrame causer, DeathFrame death)
    {
        if (causer.Id != death.ParentFrameId || death.KillerSeat is not null || causer.SkillId is not ("classic:wuhun" or "mobile:wuhun" or "anniversary-god:wuhun") || causer.TriggerId != "death-judgment" || causer.InstructionIndex < 1 || !causer.SelectedTargetSeats.SequenceEqual([death.VictimSeat]))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(causer, _contentRegistryOperations._contentRegistry.GetSkill(causer.SkillId).Program!).GetPausedInstruction(causer.InstructionIndex).Effect;
        if (effect is not { Op: SkillProgramEffectOp.CauseDeathUnlessBoundCardKind, Target: SkillProgramEffectTarget.SelectedTarget, Condition.Kind: SkillProgramConditionKind.Always, SourceBind: { } bind } || !effect.CardKinds.SequenceEqual([CardKind.Peach, CardKind.PeachGarden]) || causer.CardSetBindings.SingleOrDefault(b => b.Name == bind)is not { Visibility: SkillProgramCardSetVisibility.Public, CardIds: [var cardId], SourceLocations: [var from] })
            return false;
        var judgment = _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Where(e => e.ParentResolutionId == causer.Id && e.TargetSeat == death.VictimSeat && e.Reason == "skill.wuhun.death").ToArray();
        return judgment is [var result] && result.CardId == cardId && result.CardKind is { } kind && !effect.CardKinds.Contains(kind) && _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentRequestedEvent>().Count(e => e.ResolutionId == result.ResolutionId && e.ParentResolutionId == causer.Id && e.TargetSeat == death.VictimSeat && e.Reason == result.Reason) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillCauseDeathDeclaredEvent>().Count(e => e.FrameId == causer.Id && e.SkillId == causer.SkillId && e.BindingId == causer.TriggerId && e.SourceSeat == causer.OwnerSeat && e.TargetSeat == death.VictimSeat) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == cardId && m.CardKind == kind && m.From == from && m.To == CardLocation.DiscardPile && m.Reason.Value == $"skill-program.{(causer.SkillId)}.{(causer.TriggerId)}.judgment-finish");
    }

    internal bool IsOriginalDyingSuspendedByHellKing(DyingFrame dying) => !_coreMatchParticipantsPlayersAccess._players[dying.VictimSeat].IsAlive && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(root => HasExactHellKingDeathPrefix(root, out var original, out _) && original?.Id == dying.Id);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 900)]
    internal void CollectWindow_CollectHellZhiwangCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectHellZhiwangCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 1600)]
    internal bool ExcludeOrdinaryMovement_IsHellZhiwangTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsHellZhiwangTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 2000)]
    internal ProgramSkillWindowContext? TryCreateHellZhiwangContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsHellZhiwangTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateHellZhiwangContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 70, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveHellZhiwangMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsHellZhiwangTrigger(trigger) ? CanRunHellZhiwangCandidate(candidate, context) : null;
}
