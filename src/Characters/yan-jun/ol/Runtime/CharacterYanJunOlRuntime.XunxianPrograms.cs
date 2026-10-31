#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanJunOlRuntime
{
    internal SkillProgramStepOutcome XunxianProgramGiftUsedCard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || _cardZonesOperations._cardZones.GetLocation(batch.Movements[index].CardId) != CardLocation.DiscardPile)
            return SkillProgramStepOutcome.Continue;
        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat && (_corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count || player.Hp > owner.Hp)).Select(player => player.Seat).ToArray();
        if (recipients.Length == 0)
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 逊贤：没有手牌数或体力值大于你的角色，牌留在弃牌堆。", active.OwnerSeat);
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return SkillProgramStepOutcome.Continue;
        }

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
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat || !int.TryParse(selected.Parameters.GetValueOrDefault("recipient"), System.Globalization.CultureInfo.InvariantCulture, out var recipient) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } index } || index < 0 || index >= batch.Movements.Count)
            throw new InvalidOperationException("The Xunxian gift choice lost its chooser or settled card.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var movement = batch.Movements[index];
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || recipient == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _cardZonesOperations._cardZones.GetLocation(movement.CardId) != CardLocation.DiscardPile || !(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[recipient]).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count || _coreMatchParticipantsPlayersAccess._players[recipient].Hp > owner.Hp))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "逊贤的目标或弃牌堆中的牌已失效，剩余结算取消。");
            return;
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Single(item => item.Id == movement.CardId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Hand(recipient), new($"skill-program.{(active.SkillId)}.xunxian-gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(active.Id, active.OwnerSeat, recipient, [card.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，将 {(card.DisplayName)} 交给 {(_coreMatchParticipantsPlayersAccess._players[recipient].Name)}。", active.OwnerSeat, recipient);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramXunxianGiftEvent(active.Id, active.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, recipient, card.Id, movement.Sequence));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }
}
