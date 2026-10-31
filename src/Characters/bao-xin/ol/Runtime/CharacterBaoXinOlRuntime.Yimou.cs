#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBaoXinOlRuntime
{
    internal PromptChoice BaoXinChoice(ProgramSkillFrame f, string branch, string label, int? card = null, int? target = null) => new(new($"bao-xin.{(f.Id)}.{(branch)}.{(card ?? target ?? -1)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { { "program-action", "bao-xin" }, { "branch", branch } });
    internal IReadOnlyList<PromptChoice> BaoXinChoices(ProgramSkillFrame f)
    {
        var r = f.BaoXin!;
        var list = new List<PromptChoice>();
        if (!_coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
            return list;
        if (r.Stage == BaoXinStage.Mode)
        {
            list.Add(BaoXinChoice(f, "obtain", "令受伤角色随机获得牌堆中的一张杀"));
            if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Holder]).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != r.Holder))
                list.Add(BaoXinChoice(f, "gift", "令受伤角色自行交出一张手牌，然后摸一张牌"));
        }

        if (r.Stage == BaoXinStage.GiftCard)
            foreach (var c in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Holder, r.Holder, [CardZoneKind.Hand], OwnedCardMoveIntent.Transfer))
                list.Add(BaoXinChoice(f, "card", c.Description, c.Cards.Single()));
        if (r.Stage == BaoXinStage.GiftTarget)
            list.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != r.Holder).Select(p => BaoXinChoice(f, "target", p.Name, target: p.Seat)));
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishBaoXin(ProgramSkillFrame f)
    {
        var r = f.BaoXin!;
        var choices = BaoXinChoices(f);
        if (choices.Count == 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishBaoXin(f);
            return;
        }

        var chooser = r.Stage == BaoXinStage.Mode ? f.OwnerSeat : r.Holder;
        if (!_coreMatchParticipantsPlayersAccess._players[chooser].IsAlive)
        {
            FinishBaoXin(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == BaoXinStage.Mode ? "毅谋：选择援助方式。" : "毅谋：由受伤者选择真实手牌及接收者。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), r.Holder)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("bao-xin")]
    internal void AnswerBaoXin(PromptChoice chosen)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BaoXin: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == BaoXinStage.Mode ? f.OwnerSeat : r.Holder) || !BaoXinChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [chosen])))
            throw new InvalidOperationException("Yimou requires its exact current private chooser and original choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = chosen.Parameters["branch"];
        if (branch is "obtain" or "gift")
        {
            r = r with
            {
                Paid = true,
                Branch = branch == "obtain" ? 1 : 2
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BaoXinYimouBranchEvent(f.Id, r.Branch, r.Holder));
            if (branch == "gift")
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { Stage = BaoXinStage.GiftCard } });
                PublishBaoXin(f);
                return;
            }

            var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind)).OrderBy(c => c.Id).ToArray();
            if (pool.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[r.Holder].IsAlive)
            {
                FinishBaoXin(f);
                return;
            }

            var before = _randomOperations._random.State;
            var index = _randomOperations._random.Next(pool.Length);
            var card = pool[index];
            r = r with
            {
                Eligible = pool.Select(c => c.Id).ToArray(),
                RandomBefore = before,
                RandomAfter = _randomOperations._random.State,
                RandomIndex = index,
                SelectedCard = card.Id
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r });
            RecordBaoXinRandom(f, 0, r.Holder);
            MoveBaoXin(f, [new(card.Id, card.Kind, card.Kind, CardLocation.DrawPile)], BaoXinStage.ObtainChildren, CardLocation.Hand(r.Holder), BaoXinObtain);
            ContinueBaoXin(f.Id);
            return;
        }

        if (branch == "card")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { SelectedCard = chosen.Cards.Single(), Stage = BaoXinStage.GiftTarget } });
            PublishBaoXin(f);
            return;
        }

        if (branch != "target" || r.SelectedCard is not { } selected || _cardZonesOperations._cardZones.GetLocation(selected) != CardLocation.Hand(r.Holder))
            throw new InvalidOperationException("Yimou cannot substitute another original private Hand entity.");
        var recipient = chosen.Targets.Single();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BaoXin = r with { Recipient = recipient } });
        var gift = _coreAdvancedLifecycleOperations.GetAdvancedCard(selected);
        MoveBaoXin(f, [new(gift.Id, gift.Kind, _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[r.Holder], gift), CardLocation.Hand(r.Holder))], BaoXinStage.YimouGiftChildren, CardLocation.Hand(recipient), BaoXinYimouGift);
        ContinueBaoXin(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveBaoXinYimou)]
    internal PromptChoice ChooseBaoXinAi(PendingDecision prompt, ProgramSkillFrame f)
    {
        var r = f.BaoXin!;
        if (r.Stage == BaoXinStage.Mode)
            return prompt.Choices.First(c => c.Parameters["branch"] == "obtain");
        if (r.Stage == BaoXinStage.GiftCard)
            return prompt.Choices.OrderBy(c => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).HandKeepValue).First();
        return prompt.Choices.OrderByDescending(c => _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]) == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[r.Holder])).ThenBy(c => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]).Count).ThenBy(c => c.Targets.Single()).First();
    }
}
