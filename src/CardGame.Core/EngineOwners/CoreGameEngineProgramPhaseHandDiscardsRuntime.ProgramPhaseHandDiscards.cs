#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramPhaseHandDiscardsRuntime
{
    internal SkillProgramStepOutcome RestoreProgramPhaseHandDiscards(ProgramSkillFrame frame, ProgramParticipantReference chooser, ProgramParticipantReference phaseOwnerRef)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var chooserSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(active, chooser);
        var phaseOwnerSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(active, phaseOwnerRef);
        if (!_coreMatchParticipantsPlayersAccess._players[chooserSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[phaseOwnerSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "技能参与者已失效，技能剩余步骤取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var discardIds = CollectPhaseHandLimitDiscardIds(phaseOwnerSeat);
        if (discardIds.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var choices = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(card => discardIds.Contains(card.Id)).OrderBy(card => card.Id).Select(card => new PromptChoice(new ChoiceId($"restore-phase-hand.frame-{(active.Id)}.card-{(card.Id)}"), $"将【{(card.DisplayName)}】交还给 {(_coreMatchParticipantsPlayersAccess._players[phaseOwnerSeat].Name)}，然后获得其余该角色于此阶段内弃置的牌。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "restore-phase-hand-discard", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["phase-owner-seat"] = phaseOwnerSeat.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList();
        choices.Add(new PromptChoice(new ChoiceId($"restore-phase-hand.frame-{(active.Id)}.decline"), $"不交还 {(_coreMatchParticipantsPlayersAccess._players[phaseOwnerSeat].Name)} 的牌。", [], [], new Dictionary<string, string> { ["program-action"] = "restore-phase-hand-decline", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, chooserSeat, $"【{(skill.Name)}】可以将弃牌堆里的一张牌交还给 {(_coreMatchParticipantsPlayersAccess._players[phaseOwnerSeat].Name)}，然后获得其余该角色于此阶段内弃置的牌。", choices.SelectMany(choice => choice.Cards).Distinct().Order().ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            SkillPrompt = new SkillPromptPresentation(active.SkillId, skill.Name, $"{(skill.Name)} · 选择交还的牌", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<int> CollectPhaseHandLimitDiscardIds(int phaseOwnerSeat)
    {
        var pileIds = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Select(card => card.Id).ToHashSet();
        return _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(movement => movement.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && movement.From == CardLocation.Hand(phaseOwnerSeat) && movement.To == CardLocation.DiscardPile && movement.Reason == CardMoveReasons.HandLimitDiscard && pileIds.Contains(movement.CardId)).Select(movement => movement.CardId).Distinct().Order().ToArray();
    }

    internal void ResolveProgramPhaseHandDiscardChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The phase-hand restore choice lost its program frame.");
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.RestorePhaseHandDiscards || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The phase-hand restore choice does not match its suspended instruction.");
        var chooserSeat = _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, effect.ChooserRef!);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != chooserSeat)
            throw new InvalidOperationException("The phase-hand restore chooser changed while suspended.");
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[chooserSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "技能参与者或技能实例已失效，技能剩余步骤取消。");
            return;
        }

        var action = selected.Parameters.GetValueOrDefault("program-action");
        if (action == "restore-phase-hand-decline")
        {
            if (selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The decline branch must not name a card or target.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        if (action != "restore-phase-hand-discard" || !int.TryParse(selected.Parameters.GetValueOrDefault("phase-owner-seat"), out var phaseOwnerSeat) || phaseOwnerSeat < 0 || phaseOwnerSeat >= _coreMatchDefinitionPlayerCountAccess._playerCount || !_coreMatchParticipantsPlayersAccess._players[phaseOwnerSeat].IsAlive || !int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), out var cardId))
            throw new InvalidOperationException("The phase-hand restore selection is malformed.");
        var discardIds = CollectPhaseHandLimitDiscardIds(phaseOwnerSeat);
        var returned = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).FirstOrDefault(card => card.Id == cardId);
        if (returned is null || !discardIds.Contains(cardId) || selected.Cards.Count != 1 || selected.Cards[0] != cardId)
            throw new InvalidOperationException("The phase-hand restore card is no longer available.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _corePhysicalMovementFacadesOperations.MoveCard(returned, CardLocation.DiscardPile, CardLocation.Hand(phaseOwnerSeat), new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}"));
        var remaining = CollectPhaseHandLimitDiscardIds(phaseOwnerSeat);
        foreach (var card in _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(card => remaining.Contains(card.Id)).OrderBy(card => card.Id).ToArray())
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(chooserSeat), new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}"));
        }

        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[chooserSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Name)}】，" + $"将一张牌交还给 {(_coreMatchParticipantsPlayersAccess._players[phaseOwnerSeat].Name)}，然后获得其余该角色于此阶段内弃置的牌。", chooserSeat, phaseOwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.RestorePhaseHandDiscards)]
    internal PromptChoice SelectAiProgramPhaseHandDiscardRestore(PendingDecision decision, ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        var decline = decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("program-action") == "restore-phase-hand-decline");
        var giveChoices = decision.Choices.Where(choice => choice.Parameters.GetValueOrDefault("program-action") == "restore-phase-hand-discard").ToArray();
        if (giveChoices.Length <= 1)
        {
            // Returning the only discarded card is a pure gift with nothing to gain.
            return decline;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var pile = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).ToDictionary(card => card.Id);
        var selected = giveChoices.OrderBy(choice => CoreNativeAiDecisionsRuntime.GetKeepValue(pile[choice.Cards[0]], owner)).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
        _coreNativeAiDecisionsOperations.AddThought(new AiThoughtRecord(++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, _coreActualTurnProgressTurnNumberAccess._turnNumber, decision.PlayerSeat, selected.Description, [], $"【{(skill.Name)}】交还一张牌，换取其余 {(giveChoices.Length - 1)} 张牌。"));
        return selected;
    }
}
