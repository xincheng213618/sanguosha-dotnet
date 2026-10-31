#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPanShuOlRuntime
{
    internal int ZhirenNameLength(ProgramSkillFrame frame) => frame.WindowContext?.CardUse is { } use ? CardCatalog.Get(use.EffectiveKind).DisplayName.Length : throw new InvalidOperationException("The zhiren tiers lost their current card use.");
    internal SkillProgramStepOutcome ZhirenProgramReorderTopByLength(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var length = ZhirenNameLength(active);
        return _coreProgramTopReorderOperations.BeginProgramTopReorder(active, length, null, requireCurrentSeat: false);
    }

    internal SkillProgramStepOutcome ZhirenProgramResolveFieldTiers(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var length = ZhirenNameLength(active);
        if (length < 2)
            return ContinueZhirenTiers(active, length, skippedDiscards: true);
        if (ZhirenFieldEquipmentCandidates().Count > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { PanShuZhirenField = new(false, length) });
            PresentZhirenFieldPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), delayedTrick: false);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (ZhirenFieldDelayedCandidates().Count > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { PanShuZhirenField = new(true, length) });
            PresentZhirenFieldPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), delayedTrick: true);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        return ContinueZhirenTiers(active, length, skippedDiscards: true);
    }

    internal void ContinueZhirenFieldAfterEquipment(ProgramSkillFrame frame)
    {
        if (ZhirenFieldDelayedCandidates().Count > 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { PanShuZhirenField = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).PanShuZhirenField!with { AwaitingDelayedTrick = true } });
            PresentZhirenFieldPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), delayedTrick: true);
            return;
        }

        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var length = active.PanShuZhirenField!.NameLength;
        _ = ContinueZhirenTiers(active, length, skippedDiscards: false);
    }

    internal SkillProgramStepOutcome ContinueZhirenTiers(ProgramSkillFrame active, int length, bool skippedDiscards)
    {
        active = _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { PanShuZhirenField = null });
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id);
        if (length >= 3)
            ApplyProgramSkillRecovery(active.Id, active.OwnerSeat, active.OwnerSeat);
        if (length >= 4)
            _coreProgramLifecycleOperations.DrawProgramCards(active.Id, active.OwnerSeat, 3, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(active.SkillId)}.zhiren-tiers"));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【织纴】按牌名字数 {(length)} 结算" + (skippedDiscards ? "（未弃置场上牌）。" : "。"), active.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal IReadOnlyList<Card> ZhirenFieldEquipmentCandidates() => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(player => _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Select(card => (Player: player, Card: card))).OrderBy(item => item.Player.Seat).ThenBy(item => item.Card.Id).Select(item => item.Card).ToArray();
    internal IReadOnlyList<Card> ZhirenFieldDelayedCandidates() => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).SelectMany(player => _corePhysicalOwnedCardQueriesOperations.GetJudgment(player).Select(card => (Player: player, Card: card))).OrderBy(item => item.Player.Seat).ThenBy(item => item.Card.Id).Select(item => item.Card).ToArray();
    internal void PresentZhirenFieldPrompt(ProgramSkillFrame frame, bool delayedTrick)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var candidates = delayedTrick ? ZhirenFieldDelayedCandidates() : ZhirenFieldEquipmentCandidates();
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(card =>
        {
            var holder = _cardZonesOperations._cardZones.GetLocation(card.Id);
            var holderName = holder.OwnerSeat is { } seat ? _coreMatchParticipantsPlayersAccess._players[seat].Name : "";
            return new PromptChoice(new ChoiceId($"zhiren-field.frame-{(frame.Id)}.card-{(card.Id)}"), $"弃置 {(holderName)} 的【{(card.DisplayName)}】。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "zhiren-field-discard", ["stage"] = delayedTrick ? "delayed" : "equipment", ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        }).ToList();
        choices.Add(new PromptChoice(new ChoiceId($"zhiren-field.frame-{(frame.Id)}.skip-{((delayedTrick ? "delayed" : "equipment"))}"), delayedTrick ? "不弃置延时锦囊牌。" : "不弃置装备牌。", [], [], new Dictionary<string, string> { ["program-action"] = "zhiren-field-discard", ["stage"] = delayedTrick ? "delayed" : "equipment", ["action"] = "skip" }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, delayedTrick ? $"【{(presentation.Name)}】可弃置场上的一张延时锦囊牌。" : $"【{(presentation.Name)}】可弃置场上的一张装备牌。", candidates.Select(card => card.Id).ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · {((delayedTrick ? "延时锦囊" : "装备"))}弃置", $"此牌牌名字数为 {(active.PanShuZhirenField!.NameLength)}，可弃置场上的一张{((delayedTrick ? "延时锦囊牌" : "装备牌"))}，也可以放弃。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("zhiren-field-discard")]
    internal void ResolveZhirenFieldChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The zhiren field choice lost its program frame.");
        var state = frame.PanShuZhirenField ?? throw new InvalidOperationException("The zhiren field choice lost its stage state.");
        var delayedStage = selected.Parameters.GetValueOrDefault("stage") == "delayed";
        if (delayedStage != state.AwaitingDelayedTrick)
            throw new InvalidOperationException("The zhiren field choice does not match its stage.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (selected.Parameters.GetValueOrDefault("action") != "skip")
        {
            if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId))
                throw new InvalidOperationException("The zhiren field choice has no card.");
            var location = _cardZonesOperations._cardZones.GetLocation(cardId);
            var expectedZone = delayedStage ? CardZoneKind.Judgment : CardZoneKind.Equipment;
            if (location.Zone != expectedZone || location.OwnerSeat is null)
                throw new InvalidOperationException("The zhiren field card left its field zone.");
            var card = _cardZonesOperations._cardZones.CardsAt(location).Single(item => item.Id == cardId);
            active = _coreNativeLogicalDiscardsOperations.AppendFrameNativeLogicalDiscard(active, active.OwnerSeat, [card.Id], new($"skill-program.{(active.SkillId)}.zhiren-field"));
            _corePhysicalMovementFacadesOperations.MoveCards([card], location, CardLocation.DiscardPile, new($"skill-program.{(active.SkillId)}.zhiren-field"), movement => _characterBianFuRenOlOperations.RecordBianFuRenForeignDiscardProducer(movement, active.OwnerSeat, active.Id));
        }

        if (!delayedStage)
        {
            ContinueZhirenFieldAfterEquipment(active);
            return;
        }

        active = _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { PanShuZhirenField = null });
        active = _coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id);
        var length = state.NameLength;
        if (length >= 3)
            ApplyProgramSkillRecovery(active.Id, active.OwnerSeat, active.OwnerSeat);
        if (length >= 4)
            _coreProgramLifecycleOperations.DrawProgramCards(active.Id, active.OwnerSeat, 3, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(active.SkillId)}.zhiren-tiers"));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【织纴】按牌名字数 {(length)} 结算。", active.OwnerSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }
}
