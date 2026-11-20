#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanJunOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 7198, SkillProgramEffectOp.XunxianGiftUsedCard)]
    internal bool CanOfferXunxian(CharacterState owner) =>
        _coreMatchParticipantsPlayersAccess._players.Any(player => IsXunxianRecipient(owner, player));

    SkillProgramStepOutcome IYanJunProgramHost.XunxianGiftUsedCard(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (XunxianSettledMovement(active) is not { } movement ||
            _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive ||
            !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) ||
            !IsXunxianDiscardEntryCurrent(movement))
            return SkillProgramStepOutcome.Continue;
        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(player => IsXunxianRecipient(owner, player)).Select(player => player.Seat).ToArray();
        if (recipients.Length == 0)
            return SkillProgramStepOutcome.Continue;

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = recipients.Select(seat => new PromptChoice(new ChoiceId($"xunxian-gift.frame-{(frame.Id)}.seat-{(seat)}"), $"交给 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}。", [], [seat], new Dictionary<string, string> { ["program-action"] = "xunxian-gift", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["recipient"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择接受牌的角色。", [], recipients, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择角色", "将置入弃牌堆的牌交给一名手牌数或体力值大于你的角色。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("xunxian-gift")]
    internal void ResolveXunxianGiftChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Xunxian gift choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.XunxianGiftUsedCard } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Xunxian gift choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision ||
            decision.PlayerSeat != active.OwnerSeat ||
            !int.TryParse(selected.Parameters.GetValueOrDefault("recipient"), System.Globalization.CultureInfo.InvariantCulture, out var recipient) ||
            XunxianSettledMovement(active) is not { } movement)
            throw new InvalidOperationException("The Xunxian gift choice lost its chooser or settled card.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive ||
            !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) ||
            !IsXunxianRecipient(owner, _coreMatchParticipantsPlayersAccess._players[recipient]) ||
            !IsXunxianDiscardEntryCurrent(movement) ||
            _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).SingleOrDefault(item => item.Id == movement.CardId) is not { } card)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "逊贤的目标或弃牌堆中的牌已失效，剩余结算取消。");
            return;
        }

        _boundCardMovement.Move(card, CardLocation.DiscardPile, CardLocation.Hand(recipient),
            new($"skill-program.{active.SkillId}.xunxian-gift"),
            new(active.Id, active.OwnerSeat, recipient, [card.Id], _boundCardMovement.Sequence));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，将 {(card.DisplayName)} 交给 {(_coreMatchParticipantsPlayersAccess._players[recipient].Name)}。", active.OwnerSeat, recipient);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramXunxianGiftEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, recipient, card.Id, movement.Sequence));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    private static CardMovementRecord? XunxianSettledMovement(ProgramSkillFrame frame) =>
        frame.WindowContext is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } &&
        index >= 0 && index < batch.Movements.Count ? batch.Movements[index] : null;

    private bool IsXunxianDiscardEntryCurrent(CardMovementRecord movement) =>
        movement.To == CardLocation.DiscardPile &&
        _zoneEntries.CurrentEntry(CardLocation.DiscardPile, movement.CardId) == movement.Sequence;

    private bool IsXunxianRecipient(CharacterState owner, CharacterState recipient) =>
        recipient.IsAlive && recipient.Seat != owner.Seat &&
        (_corePhysicalOwnedCardQueriesOperations.GetHand(recipient).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count ||
         recipient.Hp > owner.Hp);
}
