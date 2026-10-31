#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduGeneralChoicesRuntime
{
    internal PromptChoice GuanduChoice(ProgramSkillFrame f, string option, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"guandu-general.{(f.Id)}.{(f.GuanduGeneral!.Stage)}.{(option)}.cards-{(string.Join("-", cards ?? []))}.targets-{(string.Join("-", targets ?? []))}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = "guandu-general", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
    internal void PublishGuanduGeneralChoice(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices)
    {
        if (choices.Count == 0)
        {
            _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, $"【{(skill.Name)}】请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal IReadOnlyList<PromptChoice> GuanduOwnCardChoices(ProgramSkillFrame f, int seat, string option, bool discard = false, bool allowSkip = true, bool nonEquipment = false, bool handOnly = false)
    {
        var cards = discard ? _coreGuanduGeneralsOperations.GuanduDiscardable(seat, seat) : handOnly ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).ToArray() : _coreGuanduGeneralsOperations.GuanduOwnedCards(seat);
        var choices = cards.Where(c => !nonEquipment || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind)).Select(c => GuanduChoice(f, option, $"{((discard ? "弃置" : "交出"))}【{(c.DisplayName)}】", [c.Id], [seat])).ToList();
        if (allowSkip)
            choices.Add(GuanduChoice(f, "skip", "不执行"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal IReadOnlyList<PromptChoice> GuanduForeignGainChoices(ProgramSkillFrame f, int seat)
    {
        var choices = new List<PromptChoice>();
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count != 0)
            choices.Add(GuanduChoice(f, "take-hand", $"随机获得 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的一张手牌", targets: [seat]));
        choices.AddRange(_coreGuanduGeneralsOperations.GuanduOwnedCards(seat, judgment: true).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id).Zone != CardZoneKind.Hand).Select(c => GuanduChoice(f, "take-public", $"获得 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的【{(c.DisplayName)}】", [c.Id], [seat])));
        return Array.AsReadOnly(choices.ToArray());
    }

    [ProgramAnswer("guandu-general")]
    internal void ResolveGuanduGeneralChoice(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GuanduGeneral: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || choice.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !prompt.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Activity choice lost its exact published prompt.");
        _coreGuanduGeneralsOperations.AssertGuanduGeneral(f);
        var option = choice.Parameters["option"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == "shicai-cost")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduStage(f, "shicai-obtain");
            GuanduDiscardOne(f, choice.Cards.Single(), f.OwnerSeat);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "jieliang-cost")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduStage(f, "jieliang-paid");
            GuanduDiscardOne(f, choice.Cards.Single(), f.OwnerSeat);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "jieliang-claim")
        {
            if (option != "skip")
            {
                var id = choice.Cards.Single();
                if (!_coreGuanduGeneralBenefitsOperations.GuanduClaimableDiscards(r.TargetSeat).Any(e => e.CardId == id))
                    throw new InvalidOperationException("The original discard entity is no longer claimable.");
                f = _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.DiscardPile, CardLocation.Hand(f.OwnerSeat), new("activity.guandu.jieliang.claim"));
            }
            else
                f = _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "zezhu-take")
        {
            var seat = r.Seats![r.Index];
            var card = option == "take-hand" ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat])[_randomOperations._random.Next(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count)] : _coreAdvancedLifecycleOperations.GetAdvancedCard(choice.Cards.Single());
            if (_cardZonesOperations._cardZones.GetLocation(card.Id).OwnerSeat != seat)
                throw new InvalidOperationException("The commander's selected card changed owner.");
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Stage = "zezhu-take-next", Index = r.Index + 1 });
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Hand(f.OwnerSeat), new("activity.guandu.zezhu.take"));
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "zezhu-give")
        {
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Stage = "zezhu-give-next", Index = r.Index + 1 });
            GuanduGiveOne(f, choice.Cards.Single(), f.OwnerSeat, r.Seats![r.Index]);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "sushou-give")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
                _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
                return;
            }

            var target = choice.Targets.Single();
            if (!r.Seats!.Contains(target) || target == f.OwnerSeat || !_coreGuanduActivitiesOperations.GuanduAllies(f.OwnerSeat, target))
                throw new InvalidOperationException("Sushou changed its frozen allied recipient.");
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Seats = global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime.FreezeGuanduSeats(r.Seats!.Where(s => s != target)) });
            GuanduGiveOne(f, choice.Cards.Single(), f.OwnerSeat, target);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "xiying-cost")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Stage = "xiying-target", Seats = global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralsRuntime.FreezeGuanduSeats(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => p.Seat)), Index = 0 });
            GuanduDiscardOne(f, choice.Cards.Single(), f.OwnerSeat);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "xiying-answer")
        {
            var target = r.Seats![r.Index];
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { Stage = "xiying-target", Index = r.Index + 1 });
            if (option == "ban")
                _coreGuanduGeneralQualificationOperations.IssueGuanduAllCardBan(f, target);
            else
                GuanduDiscardOne(f, choice.Cards.Single(), target);
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage is "beizhan-target" or "zhongjie-target")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            var target = choice.Targets.Single();
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { TargetSeat = target, Stage = r.Stage == "beizhan-target" ? "beizhan-draw" : "zhongjie-grow" });
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage is "yuanlve-give" or "moushi-give")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            var target = choice.Targets.Single();
            var card = choice.Cards.Single();
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { TargetSeat = target, CardId = card, MovementBefore = before, Stage = r.Stage == "yuanlve-give" ? "yuanlve-offer" : "moushi-arm" });
            GuanduGiveOne(f, card, f.OwnerSeat, target);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreGuanduGeneralsOperations.GuanduUpdate(f, f.GuanduGeneral!with { MovementAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence });
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "choulue-target")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { TargetSeat = choice.Targets.Single(), Stage = "choulue-gift" });
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        if (r.Stage == "choulue-gift")
        {
            if (option == "skip")
            {
                _coreGuanduGeneralsOperations.FinishGuanduGeneral(f, false);
                return;
            }

            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            var id = choice.Cards.Single();
            f = _coreGuanduGeneralsOperations.GuanduUpdate(f, r with { CardId = id, MovementBefore = before, Stage = "choulue-arm" });
            GuanduGiveOne(f, id, r.TargetSeat, f.OwnerSeat);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreGuanduGeneralsOperations.GuanduUpdate(f, f.GuanduGeneral!with { MovementAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence });
            _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
            return;
        }

        throw new InvalidOperationException("Activity choice does not belong to the current receipt stage.");
    }

    internal void GuanduDiscardOne(ProgramSkillFrame f, int id, int owner)
    {
        if (!_coreGuanduGeneralsOperations.GuanduDiscardable(owner, owner).Any(c => c.Id == id))
            throw new InvalidOperationException("A paid discard is no longer owned or legal.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
        var location = _cardZonesOperations._cardZones.GetLocation(id);
        var reason = new CardMoveReason("activity.guandu." + f.SkillId + ".discard");
        var invoice = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, owner, [id], reason, f.Id);
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DiscardPile, reason, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(invoice);
    }

    internal void GuanduGiveOne(ProgramSkillFrame f, int id, int owner, int target)
    {
        if (!_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreGuanduGeneralsOperations.GuanduOwnedCards(owner).Any(c => c.Id == id))
            throw new InvalidOperationException("A gift lost its exact living participants or physical card.");
        var from = _cardZonesOperations._cardZones.GetLocation(id);
        if (from == CardLocation.Hand(target))
            return;
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), from, CardLocation.Hand(target), new("activity.guandu." + f.SkillId + ".gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, owner, target, [id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null), disclosure: from.Zone == CardZoneKind.Equipment ? CardMovementDisclosure.FaceUp : CardMovementDisclosure.FaceDown);
    }
}
