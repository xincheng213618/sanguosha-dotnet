#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramHoldCardsRuntime
{
    internal SkillProgramStepOutcome HoldProgramTargetCards(ProgramSkillFrame frame, int chooserSeat, int holderSeat, IReadOnlyList<CardZoneKind> zones, string resultBind, int minimumCards)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.HoldCardSelection is not null || active.CardSetBindings.Any(binding => binding.Name == resultBind))
            throw new InvalidOperationException("A card hold cannot overwrite an existing draft or binding.");
        if (holderSeat == frame.OwnerSeat)
            throw new InvalidOperationException("A card hold cannot target the skill owner's own cards.");
        var candidates = zones.SelectMany(zone =>
        {
            var location = new CardLocation(zone, holderSeat);
            return _cardZonesOperations._cardZones.CardsAt(location).Select((card, index) => (card.Id, Location: location, Slot: index));
        }).ToArray();
        var requiredCount = candidates.Length == 0 ? 0 : Math.Clamp(_coreMatchParticipantsPlayersAccess._players[holderSeat].Hp, minimumCards, candidates.Length);
        if (requiredCount < minimumCards)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "目标没有可扣置的区域牌，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        active = active with
        {
            HoldCardSelection = new(holderSeat, chooserSeat, resultBind, requiredCount, Array.AsReadOnly(candidates.Select(item => item.Id).ToArray()), Array.AsReadOnly(candidates.Select(item => item.Location).ToArray()), Array.Empty<int>(), minimumCards)
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramHoldCardSelection(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramHoldCardSelection(ProgramSkillFrame frame)
    {
        var draft = frame.HoldCardSelection ?? throw new InvalidOperationException("Missing card-hold draft.");
        var selected = draft.SelectedCardIds.ToHashSet();
        var handSlotsSeen = 0;
        var choices = draft.SelectedCardIds.Count >= draft.RequiredCount ? [] : draft.CandidateCardIds.Select((id, index) => (Id: id, Location: draft.CandidateLocations[index])).Where(item => !selected.Contains(item.Id)).Select(item =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(item.Location).Single(card => card.Id == item.Id);
            var hidden = item.Location.Zone == CardZoneKind.Hand && draft.ChooserSeat != draft.HolderSeat;
            var slot = item.Location.Zone == CardZoneKind.Hand ? ++handSlotsSeen : 0;
            return new PromptChoice(new ChoiceId($"program-hold.frame-{(frame.Id)}.pick-{(selected.Count)}.card-{(item.Id)}"), hidden ? $"选择目标第 {(slot)} 张手牌" : $"选择【{(card.DisplayName)}】（{(card.RankText)}）", hidden ? [] : [item.Id], [], new Dictionary<string, string> { ["program-action"] = "select-owned-cards", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind, ["source-zone"] = item.Location.Zone.ToString(), ["slot-index"] = slot.ToString(System.Globalization.CultureInfo.InvariantCulture), ["selection-index"] = selected.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) });
        }).ToList();
        if (draft.SelectedCardIds.Count >= draft.MinimumCount)
        {
            choices.Add(new PromptChoice(new ChoiceId($"program-hold.frame-{(frame.Id)}.finish-{(draft.SelectedCardIds.Count)}"), draft.SelectedCardIds.Count > 0 ? $"完成扣置（已选 {(draft.SelectedCardIds.Count)} 张）。" : "不扣置任何牌。", [], [], new Dictionary<string, string> { ["program-action"] = "finish-owned-cards", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["result-bind"] = draft.ResultBind, ["selection-index"] = draft.SelectedCardIds.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        }

        var skill = _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, draft.ChooserSeat, draft.RequiredCount > draft.SelectedCardIds.Count ? $"请选择要扣置到目标武将牌旁的牌（至多 {(draft.RequiredCount)} 张，已选 {(draft.SelectedCardIds.Count)} 张；完成前不移动牌）。" : "请确认扣置的牌。", choices.SelectMany(choice => choice.Cards).ToArray(), [], draft.ChooserSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = draft.HolderSeat,
            SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · 选择扣置的牌", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.ChooserSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveProgramHoldCardSelection(ProgramSkillFrame frame, PromptChoice selected)
    {
        var draft = frame.HoldCardSelection ?? throw new InvalidOperationException("Missing card-hold draft.");
        var finish = selected.Parameters.GetValueOrDefault("program-action") == "finish-owned-cards";
        int? chosenCard = null;
        if (!finish)
        {
            // Visible candidates carry their card id in the choice; hidden hand slots
            // only carry it inside the stable choice id.
            if (selected.Cards.Count == 1)
                chosenCard = selected.Cards[0];
            else if (selected.Cards.Count == 0)
            {
                var value = selected.Id.Value;
                var marker = value.LastIndexOf(".card-", StringComparison.Ordinal);
                if (marker >= 0 && int.TryParse(value[(marker + 6)..], out var parsed))
                    chosenCard = parsed;
            }
            else
                chosenCard = -1;
        }

        if (draft.ResultBind != selected.Parameters.GetValueOrDefault("result-bind") || selected.Parameters.GetValueOrDefault("selection-index") != draft.SelectedCardIds.Count.ToString(System.Globalization.CultureInfo.InvariantCulture) || (finish ? draft.SelectedCardIds.Count < draft.MinimumCount || selected.Cards.Count != 0 : chosenCard is not { } cardId || cardId < 0 || !draft.CandidateCardIds.Contains(cardId) || draft.SelectedCardIds.Contains(cardId)))
            throw new InvalidOperationException("The card-hold choice does not match its suspended instruction.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[draft.HolderSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) || draft.CandidateCardIds.Where((id, index) => _cardZonesOperations._cardZones.GetLocation(id) != draft.CandidateLocations[index]).Any())
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "扣置参与者、技能实例或冻结来源已失效，未移动任何牌。");
            return;
        }

        var ids = finish ? draft.SelectedCardIds.ToArray() : draft.SelectedCardIds.Append(chosenCard!.Value).ToArray();
        if (!finish && ids.Length < draft.RequiredCount)
        {
            frame = frame with
            {
                HoldCardSelection = draft with
                {
                    SelectedCardIds = Array.AsReadOnly(ids)
                }
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame);
            PublishProgramHoldCardSelection(frame);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HoldCardSelection = null });
        if (ids.Length > 0)
        {
            var moves = ids.Select(id => (Card: _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(card => card.Id == id), From: _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
            foreach (var(card, from)in moves)
                _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.PojunHold(draft.HolderSeat), CardMoveReasons.PojunHold);
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, draft.ResultBind, ids, SkillProgramCardSetVisibility.Private, moves.Select(_ => CardLocation.PojunHold(draft.HolderSeat)).ToArray());
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Name)} 将 {(ids.Length)} 张牌扣置于 {(_coreMatchParticipantsPlayersAccess._players[draft.HolderSeat].Name)} 的武将牌旁。", frame.OwnerSeat, draft.HolderSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramHoldCardsPlacedEvent(frame.Id, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, draft.HolderSeat, draft.ResultBind, Array.AsReadOnly(ids)));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void ReturnProgramPojunHoldsAtTurnEnd()
    {
        for (var seat = 0; seat < _coreMatchDefinitionPlayerCountAccess._playerCount; seat++)
        {
            var held = _cardZonesOperations._cardZones.CardsAt(CardLocation.PojunHold(seat)).ToArray();
            if (held.Length == 0)
                continue;
            _corePhysicalMovementFacadesOperations.MoveCards(held, CardLocation.PojunHold(seat), CardLocation.Hand(seat), CardMoveReasons.PojunHoldReturn);
            _coreMatchLogOperations.AddLog("SkillResolved", $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 获得武将牌旁的 {(held.Length)} 张破军扣置牌。", seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PojunHoldReturnedEvent(seat, Array.AsReadOnly(held.Select(card => card.Id).ToArray())));
        }
    }
}
