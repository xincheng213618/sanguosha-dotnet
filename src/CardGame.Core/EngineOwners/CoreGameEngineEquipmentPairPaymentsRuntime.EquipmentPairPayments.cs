#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentPairPaymentsRuntime
{
    internal IReadOnlyList<(int First, int Second)> PayableEquipmentPairs(int ownerSeat, string skillId, string instanceId)
    {
        var result = new List<(int, int)>();
        var living = _coreTableOrderOperations.CurrentTableOrder().Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).ToArray();
        for (var first = 0; first < living.Length; first++)
            for (var second = first + 1; second < living.Length; second++)
                if (IsPayableEquipmentPair(ownerSeat, living[first], living[second], skillId, instanceId))
                    result.Add((living[first], living[second]));
        return result.AsReadOnly();
    }

    internal bool IsPayableEquipmentPair(int ownerSeat, int first, int second, string skillId, string instanceId)
    {
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(first) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(second) || first == second || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[first].IsAlive || !_coreMatchParticipantsPlayersAccess._players[second].IsAlive)
            return false;
        var a = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[first]).Count;
        var b = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[second]).Count;
        var x = Math.Abs(a - b);
        var lost = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[ownerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[ownerSeat].Hp);
        return a + b > 0 && (x <= lost || LegalEquipmentPairCostCount(ownerSeat, skillId, instanceId) >= x);
    }

    internal int LegalEquipmentPairCostCount(int ownerSeat, string skillId, string instanceId) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(ownerSeat, c, CardLocation.Equipment(ownerSeat), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, skillId, instanceId, c));
    internal bool MatchesEquipmentPairOwnedCost(ProgramSkillFrame frame, string bind, Card card, CardLocation from) => frame.EquipmentPairPayment is not { } paid || paid.ResultBind != bind || from.OwnerSeat == frame.OwnerSeat && (from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, from, OwnedCardMoveIntent.Discard) && !(from.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, card));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 90)]
    internal bool CanActivateEquipmentPairPayment(CharacterState owner, string skillId, ProgramInstructionFeatures features) => !features.HasOperation(SkillProgramEffectOp.SelectEquipmentPairAndPayment) || PayableEquipmentPairs(owner.Seat, skillId, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, skillId)).Count > 0;
    internal SkillProgramStepOutcome SelectEquipmentPairAndPayment(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != 1 || frame.EquipmentPairPayment is not null || frame.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("A paid equipment pair requires its fresh active selection instruction.");
        var choices = PayableEquipmentPairs(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId).Select(pair => new PromptChoice(new ChoiceId($"equipment-pair.frame-{(frame.Id)}.first-{(pair.First)}.second-{(pair.Second)}"), $"选择{(_coreMatchParticipantsPlayersAccess._players[pair.First].Name)}与{(_coreMatchParticipantsPlayersAccess._players[pair.Second].Name)}", [], [pair.First, pair.Second], new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(new Dictionary<string, string> { ["program-action"] = "equipment-pair-payment", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }))).ToArray();
        if (choices.Length == 0)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "没有可支付的装备交换对象。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, frame.OwnerSeat, "请选择两名角色；选定时冻结装备差及所需弃牌。", [], choices.SelectMany(c => c.Targets).Distinct().Order().ToArray(), SourceSeat: frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("equipment-pair-payment")]
    internal void ResolveEquipmentPairPaymentChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.InstructionIndex != 1 || frame.EquipmentPairPayment is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != frame.OwnerSeat || choice.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) || choice.Cards.Count != 0 || choice.Targets is not [var first, var second])
            throw new InvalidOperationException("The equipment pair choice lost its exact active program.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.SelectEquipmentPairAndPayment)
            throw new InvalidOperationException("The equipment pair choice belongs to another instruction.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || !IsPayableEquipmentPair(frame.OwnerSeat, first, second, frame.SkillId, frame.SkillInstanceId))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "交换对象或足额付款已失效，尚未移动牌。");
            return;
        }

        var a = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[first]).Count;
        var b = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[second]).Count;
        var lost = Math.Max(0, _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].MaxHp - _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp);
        var required = Math.Abs(a - b) > lost ? Math.Abs(a - b) : 0;
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        var receipt = new ProgramEquipmentPairPaymentReceipt(0, first, second, a, b, lost, required, effect.ResultBind!, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { SelectedTargetSeats = Array.AsReadOnly(new[] { first, second }), EquipmentPairPayment = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramEquipmentPairPaymentFrozenEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, first, second, a, b, lost, required, effect.ResultBind!, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, before));
        var outcome = _coreProgramOwnedCardsOperations.SelectProgramOwnedCards(frame, frame.OwnerSeat, required, null, [CardZoneKind.Hand, CardZoneKind.Equipment], effect.ResultBind!, required, 0, [], []);
        if (outcome == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal SkillProgramEffect EquipmentPairOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect effect) => effect.Op != SkillProgramEffectOp.SelectEquipmentPairAndPayment ? effect : frame.EquipmentPairPayment is { } paid ? new(SkillProgramEffectOp.SelectOwnedCards, SkillProgramEffectTarget.Owner, paid.RequiredPaymentCount, effect.Condition, zones: [CardZoneKind.Hand, CardZoneKind.Equipment], resultBind: paid.ResultBind, minimumCards: paid.RequiredPaymentCount) : effect;

    internal bool IsIssuedEquipmentPairPayment(ProgramSkillFrame frame)
    {
        if (frame.EquipmentPairPayment is not { InstructionIndex: 0 } p || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex is < 1 or > 4 || p.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || frame.SelectedTargetSeats.Count != 2 || frame.SelectedTargetSeats[0] != p.FirstSeat || frame.SelectedTargetSeats[1] != p.SecondSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.FirstSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.SecondSeat) || p.FirstSeat == p.SecondSeat || p.FirstEquipmentCount < 0 || p.SecondEquipmentCount < 0 || p.FirstEquipmentCount + p.SecondEquipmentCount == 0 || p.OwnerLostHp < 0 || p.RequiredPaymentCount != (Math.Abs(p.FirstEquipmentCount - p.SecondEquipmentCount) > p.OwnerLostHp ? Math.Abs(p.FirstEquipmentCount - p.SecondEquipmentCount) : 0) || p.MovementSequenceBefore < 0)
            return false;
        var program = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program;
        if (program is null || program.GameplayHash != frame.GameplayHash)
            return false;
        var plan = ProgramInstructionResolver.Default.Resolve(frame, program);
        if (plan.Instructions.Count != 4 || plan.Instructions[0].Op != SkillProgramEffectOp.SelectEquipmentPairAndPayment || plan.Instructions[0].ResultBind != p.ResultBind)
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramEquipmentPairPaymentFrozenEvent>().Any(e => e.FrameId == frame.Id && e.Source == new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) && e.GameplayHash == frame.GameplayHash && e.FirstSeat == p.FirstSeat && e.SecondSeat == p.SecondSeat && e.FirstEquipmentCount == p.FirstEquipmentCount && e.SecondEquipmentCount == p.SecondEquipmentCount && e.OwnerLostHp == p.OwnerLostHp && e.RequiredPaymentCount == p.RequiredPaymentCount && e.ResultBind == p.ResultBind && e.ActualTurnNumber == p.ActualTurnNumber && e.MovementSequenceBefore == p.MovementSequenceBefore);
    }

    internal bool EquipmentPairCostIsPaid(ProgramSkillFrame frame)
    {
        if (!IsIssuedEquipmentPairPayment(frame) || frame.EquipmentPairPayment is not { } paid)
            return false;
        var set = frame.CardSetBindings.SingleOrDefault(b => b.Name == paid.ResultBind);
        if (set is null || set.CardIds.Count != paid.RequiredPaymentCount || set.SourceLocations.Count != set.CardIds.Count || set.SourceLocations.Any(l => l.OwnerSeat != frame.OwnerSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            return false;
        var reason = $"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}";
        return !set.CardIds.Where((id, n) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > paid.MovementSequenceBefore && m.CardId == id && m.From == set.SourceLocations[n] && m.To == OwnedPaymentDiscardDestination(id) && m.Reason.Value == reason) != 1).Any();
    }

    internal CardLocation OwnedPaymentDiscardDestination(int cardId) => (_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(c => c.Id == cardId).IsGeneralWeapon || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierDestroyedEvent>().Any(e => e.CardId == cardId && _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial2(cardId, e.From, CardLocation.OutsideGame))) ? CardLocation.OutsideGame : CardLocation.DiscardPile;
    internal bool CanResumePaidEquipmentPair(ProgramSkillFrame frame) => EquipmentPairCostIsPaid(frame) && frame.EquipmentPairPayment is { } paid && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[paid.FirstSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[paid.SecondSeat].IsAlive && _coreProgramLifecycleOperations.CanInvokeProgramSkill(frame);
    internal ProgramSkillFrame FreezePaidEquipmentExchangeStart(ProgramSkillFrame frame)
    {
        if (frame.EquipmentPairPayment is not { } p)
            return frame;
        if (p.ExchangeMovementSequenceBefore is not null)
            throw new InvalidOperationException("An equipment pair must not exchange twice.");
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { EquipmentPairPayment = p with { ExchangeMovementSequenceBefore = before } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramEquipmentPairExchangeStartedEvent(frame.Id, p.FirstSeat, p.SecondSeat, before));
        return frame;
    }

    [ProgramAiChoice(SkillProgramEffectOp.SelectEquipmentPairAndPayment)]
    internal PromptChoice SelectAiEquipmentPairPayment(PendingDecision decision, ProgramSkillFrame frame) => frame.OwnedCardSelection is not null ? _coreProgramLifecycleOperations.SelectAiProgramOwnedCards(decision, frame) : decision.Choices.OrderBy(c => Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets[0]]).Count - _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[c.Targets[1]]).Count)).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    SkillProgramStepOutcome IEquipmentPairPaymentProgramHost.SelectEquipmentPairAndPayment(ProgramSkillFrame f, SkillProgramEffect e) => SelectEquipmentPairAndPayment(f, e);
}
