#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangOriginalSkillFlowsRuntime
{
    internal void ContinueWushuangYiji(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
        {
            _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            var cards = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().Take(2).ToArray();
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "yiji-give", Cards = cards.Select(c => c.Id).ToArray() });
            foreach (var card in cards)
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, new("skill-program.wushuang.yiji.view"), disclosure: CardMovementDisclosure.FaceDown);
            _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, "wushuang-yiji", cards.Select(c => c.Id).ToArray(), SkillProgramCardSetVisibility.Private, cards.Select(_ => CardLocation.Processing).ToArray());
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        var remaining = r.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
        if (remaining.Length == 0)
        {
            _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
            return;
        }

        var recipients = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            foreach (var id in remaining)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, new("skill-program.wushuang.yiji.cleanup"));
            _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
            return;
        }

        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, remaining.SelectMany(id => recipients.Select(p => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "yiji-give", $"将【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】交给 {(p.Name)}", [id], [p.Seat]))).ToArray());
    }

    internal void ContinueWushuangJieming(ProgramSkillFrame f)
    {
        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count < Math.Min(5, p.MaxHp)).Select(p => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "jieming-target", $"令 {(p.Name)} 将手牌摸至 {(Math.Min(5, p.MaxHp))} 张", targets: [p.Seat])).ToArray());
    }

    internal void ContinueWushuangYashi(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage != "begin")
        {
            ContinueWushuangGuanxu(f);
            return;
        }

        var choices = _coreWushuangOriginalSkillsOperations.WushuangOtherTargets(f, "guanxu-target", p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).ToList();
        if (f.WindowContext?.SourceSeat is { } source && _coreMatchParticipantsPlayersAccess._players[source].IsAlive)
            choices.Insert(0, _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "yashi-suppress", $"令 {(_coreMatchParticipantsPlayersAccess._players[source].Name)} 的非锁定技失效至其下回合开始", targets: [source]));
        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, choices);
    }

    internal void ContinueWushuangGuanxu(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
        {
            _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, _coreWushuangOriginalSkillsOperations.WushuangOtherTargets(f, "guanxu-target", p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0));
            return;
        }

        if (r.Stage == "guanxu-view")
        {
            var hand = r.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(r.Target));
            var top = r.Top.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.DrawPile);
            _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, hand.SelectMany(h => top.Select(d => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "guanxu-exchange", $"以手牌【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(h).DisplayName)}】交换牌堆【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(d).DisplayName)}】", [h, d], [r.Target]))).Append(_coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "finish", "观看完毕，不交换")).ToArray());
            return;
        }
        var triples = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(r.Target, c, CardLocation.Hand(r.Target), OwnedCardMoveIntent.Discard)).GroupBy(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.Target], c)).SelectMany(group => global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.EnumerateCardCombinations(group.ToArray(), 3));
        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, triples.Select(cards => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "guanxu-discard", "弃置三张同花色手牌：" + string.Join('、', cards.Select(c => c.DisplayName)), cards.Select(c => c.Id).ToArray(), [r.Target])).ToArray());
    }

    internal void ContinueWushuangChoufa(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
        {
            _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, _coreWushuangOriginalSkillsOperations.WushuangOtherTargets(f, "choufa-target", p => _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0));
            return;
        }

        // These choices deliberately disclose positions, never the unseen names.
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).ToArray();
        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, hand.Select((card, i) => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "choufa-reveal", $"展示 {(_coreMatchParticipantsPlayersAccess._players[r.Target].Name)} 的第 {(i + 1)} 张手牌", targets: [r.Target])).Select((choice, index) => choice with { Parameters = new Dictionary<string, string>(choice.Parameters) { ["index"] = index.ToString(System.Globalization.CultureInfo.InvariantCulture) }, Id = new(choice.Id.Value + "." + index) }).ToArray());
    }

    internal void ContinueWushuangShenfu(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r = r with { Stage = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count % 2 == 1 ? "shenfu-odd" : "shenfu-even" });
        if (r.Stage == "shenfu-odd-after")
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            {
                _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
                return;
            }

            f = _coreWushuangOriginalSkillsOperations.WushuangOriginalStage(f, "shenfu-odd");
            r = f.WushuangOriginalSkill!;
        }

        if (r.Stage == "shenfu-even-after")
        {
            if (!_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Count != _coreMatchParticipantsPlayersAccess._players[r.Target].Hp)
            {
                _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
                return;
            }

            f = _coreWushuangOriginalSkillsOperations.WushuangOriginalStage(f, "shenfu-even");
            r = f.WushuangOriginalSkill!;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            _coreWushuangOriginalSkillsOperations.FinishWushuangOriginal(f);
            return;
        }

        if (r.Stage == "shenfu-odd")
        {
            var oddChoices = _coreWushuangOriginalSkillsOperations.WushuangOtherTargets(f, "shenfu-damage").ToList();
            if (r.Count > 0)
                oddChoices.Add(_coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "finish", "不再造成雷电伤害"));
            _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, oddChoices);
            return;
        }

        if (r.Stage != "shenfu-even")
            throw new InvalidOperationException("Shenfu lost its fixed parity branch.");
        var choices = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "shenfu-draw", $"令 {(p.Name)} 摸一张牌", targets: [p.Seat])).ToList();
        choices.AddRange(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).Select(p => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "shenfu-discard-target", $"弃置 {(p.Name)} 一张手牌", targets: [p.Seat])));
        if (r.Count > 0)
            choices.Add(_coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "finish", "结束神赋"));
        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, choices);
    }

    internal void ContinueWushuangLingren(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
        {
            var use = _coreCardUseLifecycleOperations.LifecycleCardUse(f.WindowContext!.CardUse!.ParentCardUseFrameId)!;
            _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, use.TargetSeats.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive).Distinct().Select(t => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "lingren-target", $"猜测 {(_coreMatchParticipantsPlayersAccess._players[t].Name)} 的手牌类型", targets: [t])).ToArray());
            return;
        }

        if (r.Stage != "lingren-guess")
            throw new InvalidOperationException("Lingren lost its frozen target categories.");
        _coreWushuangOriginalSkillsOperations.PublishWushuangOriginalChoice(f, Enumerable.Range(0, 8).Select(mask => _coreWushuangOriginalSkillsOperations.WushuangOriginalChoice(f, "lingren-guess-" + mask, $"基本牌：{(((mask & 1) != 0 ? "有" : "无"))}；锦囊牌：{(((mask & 2) != 0 ? "有" : "无"))}；装备牌：{(((mask & 4) != 0 ? "有" : "无"))}")).ToArray());
    }

    internal void ContinueWushuangTairan(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
        {
            var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "tairan-recovered", Count = Math.Max(0, owner.MaxHp - owner.Hp), Index = owner.Hp });
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, Math.Max(0, owner.MaxHp - owner.Hp), null, null);
            _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
            return;
        }

        if (r.Stage != "tairan-recovered")
            throw new InvalidOperationException("Tairan lost its once-recovered debt.");
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WushuangTairanHealedEvent>().Any(e => e.FrameId == f.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangTairanHealedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, _coreActualTurnProgressTurnNumberAccess._turnNumber, 0));
        _coreWushuangOriginalSkillsOperations.DrawWushuangOriginal(f, f.OwnerSeat, Math.Max(0, _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count), "finish");
    }

    internal void ContinueWushuangTairanSettlement(ProgramSkillFrame f)
    {
        var r = f.WushuangOriginalSkill!;
        if (r.Stage == "begin")
        {
            var settled = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangTairanSettledEvent>().Select(e => e.ProducerId).ToHashSet();
            var healing = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangTairanHealedEvent>().Where(e => e.Owner == f.OwnerSeat && !settled.Contains(e.FrameId)).ToArray();
            var drawn = _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangTairanDrawnEvent>().Where(e => e.Owner == f.OwnerSeat && !settled.Contains(e.FrameId)).ToArray();
            foreach (var id in healing.Select(e => e.FrameId).Concat(drawn.Select(e => e.FrameId)).Distinct())
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangTairanSettledEvent(id, f.OwnerSeat, f.Id));
            f = _coreWushuangOriginalSkillsOperations.UpdateWushuangOriginal(f, r with { Stage = "tairan-discard", Count = healing.Sum(e => e.Recovered), Cards = drawn.SelectMany(e => e.CardIds).Distinct().ToArray() });
            if (f.WushuangOriginalSkill!.Count > 0)
            {
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, f.WushuangOriginalSkill.Count);
                _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
                return;
            }

            r = f.WushuangOriginalSkill;
        }

        f = _coreWushuangOriginalSkillsOperations.WushuangOriginalStage(f, "finish");
        var held = r.Cards.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(f.OwnerSeat)).ToArray();
        DiscardWushuangOriginal(f, f.OwnerSeat, held);
        _coreWushuangOriginalSkillsOperations.RunWushuangOriginalSkill(f.Id);
    }

    internal void DiscardWushuangOriginal(ProgramSkillFrame f, int owner, IReadOnlyList<int> ids)
    {
        if (ids.Count == 0)
            return;
        var reason = new CardMoveReason("skill-program." + f.SkillId + ".discard");
        var invoice = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(f.OwnerSeat, owner, ids, reason, f.Id);
        foreach (var id in ids)
        {
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            if (from.OwnerSeat != owner || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner, card, from, OwnedCardMoveIntent.Discard))
                throw new InvalidOperationException("An official skill discard lost its exact owned legal entity.");
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.DiscardPile, reason, disclosure: CardMovementDisclosure.FaceUp);
        }

        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(invoice);
    }
}
