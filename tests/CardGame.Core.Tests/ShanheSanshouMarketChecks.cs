using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSanshouMarketChecks
{
    public static void PaidSourceArmorJoinsCurrentHandThenEquipsThroughItsNativeUse()
    {
        CheckPaidEquipment("codex-98", CardKind.ShanheSanshou,
            StandardShanheSanshouArmorPackage.SkillId, Suit.Heart, 12);
        CheckPaidEquipment("codex-card-4060", CardKind.ZhaoguMirror,
            StandardShanheZhaoguMirrorPackage.SkillId, Suit.Diamond, 1);
    }

    private static void CheckPaidEquipment(string rewardId, CardKind kind, string skillId, Suit suit, int rank)
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(11);
        var metadata = StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId];
        var reward = source.Rewards.Single(candidate => candidate.Id == rewardId);
        var defaultUnlocked = StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(source);
        if (kind == CardKind.ShanheSanshou)
            Need(StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[98] == rewardId &&
                metadata is { SourceRewardId: 4061, Price: 600, Rarity: 4, Kind: "装备",
                    CardKind: CardKind.ShanheSanshou, Suit: Suit.Heart, Rank: 12 } &&
                reward.StartingEquipment is [{ Kind: CardKind.ShanheSanshou, Suit: Suit.Heart, Rank: 12 }],
                "The production codex node98 binds source4061 to the real epic600-coin HeartQ Sanshou equipment recipe.");
        else
            Need(kind == CardKind.ZhaoguMirror && rewardId == "codex-card-4060" &&
                metadata is { SourceRewardId: 4060, Price: 600, Rarity: 4, Kind: "装备",
                    CardKind: CardKind.ZhaoguMirror, Suit: Suit.Diamond, Rank: 1 } &&
                reward.StartingEquipment is [{ Kind: CardKind.ZhaoguMirror, Suit: Suit.Diamond, Rank: 1 }] &&
                defaultUnlocked.Contains(rewardId, StringComparer.Ordinal) &&
                !StandardShanheCampaignCatalog.CodexRewardIdsByNodeId.Values.Contains(rewardId, StringComparer.Ordinal),
                "Source4060 is the real epic600-coin DiamondA mirror in the default stock, with no invented codex research node.");
        var merchant = source.Nodes.First(node => node.Kind == ShanheNodeKind.Shop);
        var definition = source with
        {
            StartingNodeIds = ["shop"], RandomStartingNode = false, StartingGold = 2000,
            Nodes =
            [
                merchant with { Id = "shop", ChapterNumber = 1, NextNodeIds = ["battle"],
                    Choices = merchant.Choices!.Where(choice => choice.RewardId == rewardId).ToArray() },
                new("battle", reward.Name + "集市试阵", ShanheNodeKind.Battle, [],
                    [new("guard", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])
                { ChapterNumber = 1 }
            ]
        };
        Need(definition.Nodes[0].Choices is [{ RewardId: var offeredRewardId }] && offeredRewardId == rewardId,
            "The fresh market fixture retains the actual production equipment offer as its only authored shop choice.");
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        {
            SourceDifficultyId = 11, UseCodexRewardPool = true, PersistentMarket = true,
            UnlockedRewardIds = defaultUnlocked.Append(rewardId).Distinct(StringComparer.Ordinal).ToArray()
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var moved = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
        Need(moved.Accepted, moved.Error ?? "The persistent market must enter a real battle without visiting a shop node.");
        var setup = session.PendingBattleSetup!;
        if (kind == CardKind.ZhaoguMirror)
        {
            var withoutMarket = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { MarketSetup = null });
            var withMarket = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            Need(setup.StartingCards.Count == 0 && setup.StartingEquipment.Count == 0 &&
                !withoutMarket.Cards.Values.Any(card => card.LegacyKind == CardKind.ZhaoguMirror) &&
                !withoutMarket.Skills.ContainsKey(skillId) &&
                withMarket.Cards[StandardShanheZhaoguMirrorPackage.CardId].LegacyKind == CardKind.ZhaoguMirror &&
                withMarket.GetSkill(skillId).Program is not null,
                "Only the real future market reward requires the mirror package: no owned starting recipe or skill supplies it before purchase.");
        }
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        var started = game.Submit(new StartGameCommand());
        Need(started.Accepted, started.Error?.Message ?? "The production market registry must start the real battle.");
        for (var step = 0; step < 120 &&
            !(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0); step++)
        {
            var advanced = game.Submit(new AdvanceOneStepCommand(game.Revision));
            Need(advanced.Accepted, advanced.Error?.Message ?? "The native opening must reach the first human Play.");
        }
        Need(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0,
            "The purchase begins at the actual first human Play boundary.");
        var before = game.CreateSnapshot(0).Players[0];
        var turn = game.State.TurnNumber;
        Need(before.Equipment.Count == 0 && before.Skills!.All(skill => skill.ContentId != skillId),
            "Discovering a future equipment dependency neither installs its entity nor grants its skill before payment.");
        var market = game.CreateShanheMarketSnapshot(0)!;
        var item = market.ShopItems.Single(candidate => candidate.Reward?.Id == rewardId);
        Need(market.Gold == 2000 && item.GoldCost == 600, "The first real offer uses the exact source600 quote.");
        var bought = game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, game.Revision));
        Need(bought.Accepted, bought.Error?.Message ?? "The real equipment offer must be purchasable during Play.");
        for (var step = 0; step < 120 && !PurchaseReturnedToPlay(); step++)
        {
            Need(game.PendingDecision is null || game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                game.ResolutionStack.Count > 0,
                "Unexpected native equipment purchase decision: " + PurchaseDiagnostic());
            var advanced = game.Submit(new AdvanceOneStepCommand(game.Revision));
            Need(advanced.Accepted, advanced.Error?.Message ?? "The original paid purchase tree must continue: " + PurchaseDiagnostic());
        }
        Need(PurchaseReturnedToPlay(), "The paid native procurement tree did not return to its suspended Play: " + PurchaseDiagnostic());
        var after = game.CreateSnapshot(0).Players[0];
        var paid = game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketPurchaseCommittedEvent>().Single();
        var created = game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketCardCreatedEvent>().Single();
        Need(after.Equipment.Count == 0 && after.Skills!.All(skill => skill.ContentId != skillId) &&
            after.Hand.Count(card => card.Id == created.CardId) == 1,
            "The completed native purchase adds its entity to the current hand without automatically equipping or granting its equipment skill: " + PurchaseDiagnostic());
        var purchasedCard = after.Hand.Single(card => card.Id == created.CardId);
        Need(game.CreateShanheMarketSnapshot(0)!.Gold == 1400 && paid is { Price: 600, GoldAfter: 1400 } && paid.RewardId == rewardId &&
            purchasedCard.Kind == kind && purchasedCard.Suit == suit && purchasedCard.Rank == rank && purchasedCard.Id > 0 &&
            before.Hand.All(card => card.Id != purchasedCard.Id) &&
            game.CardMovements.Count(move => move.CardId == purchasedCard.Id && move.From == CardLocation.OutsideGame &&
                move.To == CardLocation.Hand(0)) == 1 &&
            created.PurchaseFrameId == paid.FrameId && created.RewardId == rewardId && created.EquipmentResource &&
            created.CardId == purchasedCard.Id &&
            game.State.TurnNumber == turn && game.State.Phase == TurnPhase.Play &&
            game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.ResolutionStack.Count == 0,
            "One native600 payment creates the exact real equipment face in the current hand and returns to the same live Play without a second payment or a premature equipment grant.");

        var equip = game.GetHumanLegalActions().Single(action => action.Kind == LegalActionKind.Equip && action.CardId == purchasedCard.Id);
        var used = game.Submit(new PlayCardCommand(0, purchasedCard.Id, equip.TargetSeats, game.Revision,
            game.PendingDecision!.PromptId, equip.PlayedCardKind, equip.TargetCardId)
        { ConversionSource = equip.ConversionSource, AdditionalConversionSources = equip.AdditionalConversionSources });
        Need(used.Accepted, used.Error?.Message ?? "The newly purchased physical equipment must use its real native equipment action.");
        for (var step = 0; step < 120 && !EquipmentUseReturnedToPlay(); step++)
        {
            Need(game.PendingDecision is null || game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                game.ResolutionStack.Count > 0, "Unexpected native equipment-use decision: " + PurchaseDiagnostic());
            var advanced = game.Submit(new AdvanceOneStepCommand(game.Revision));
            Need(advanced.Accepted, advanced.Error?.Message ?? "The real equipment use must complete its native children: " + PurchaseDiagnostic());
        }
        Need(EquipmentUseReturnedToPlay(), "The native equipment use did not complete and return to Play: " + PurchaseDiagnostic());
        var equipped = game.CreateSnapshot(0).Players[0];
        var use = game.Events.Select(envelope => envelope.Payload).OfType<CardUseDeclaredEvent>().Single(fact => fact.CardId == purchasedCard.Id);
        Need(use.SourceSeat == 0 && use.CardKind == kind &&
            equipped.Equipment is [{ Id: var equippedId }] && equippedId == purchasedCard.Id &&
            equipped.Hand.All(card => card.Id != purchasedCard.Id) &&
            game.CardMovements.Count(move => move.CardId == purchasedCard.Id && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.EquipmentUse) == 1 &&
            game.CardMovements.Count(move => move.CardId == purchasedCard.Id && move.From == CardLocation.Processing &&
                move.To == CardLocation.Equipment(0)) == 1 &&
            game.Events.Select(envelope => envelope.Payload).OfType<CardUseFinishedEvent>()
                .Count(fact => fact.CardId == purchasedCard.Id && fact.ResolutionId == use.ResolutionId) == 1 &&
            equipped.Skills!.Any(skill => skill.ContentId == skillId) &&
            game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketPurchaseCommittedEvent>().Count() == 1 &&
            game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketCardCreatedEvent>().Count() == 1 &&
            game.CreateShanheMarketSnapshot(0)!.Gold == 1400 && game.State.TurnNumber == turn && game.State.Phase == TurnPhase.Play,
            "The same paid equipment entity moves Hand→Processing→Equipment through one completed native use, gains its equipment skill and keeps the original Play without repaying or regenerating the card.");
        var continued = game.Submit(new EndPlayPhaseCommand(0, game.Revision, game.PendingDecision!.PromptId));
        Need(continued.Accepted, continued.Error?.Message ?? "The original Play remains usable after the paid equipment installation.");

        bool EquipmentUseReturnedToPlay() => PurchaseReturnedToPlay() &&
            game.Events.Select(envelope => envelope.Payload).OfType<CardUseFinishedEvent>().Count(fact => fact.CardId == purchasedCard.Id) == 1;
        bool PurchaseReturnedToPlay() => !game.HasPendingShanheMarketPurchase && game.ResolutionStack.Count == 0 &&
            game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketPurchaseAppliedEvent>()
                .Count(fact => fact.RewardId == rewardId) == 1;
        string PurchaseDiagnostic()
        {
            var view = game.CreateSnapshot(0).Players[0];
            return $"pending={game.PendingDecision?.Kind}/{game.PendingDecision?.SkillPrompt?.SkillId}; " +
                $"frames=[{string.Join(',', game.ResolutionStack.Select(frame => frame.Kind + "/" + frame.Id))}]; " +
                $"hand=[{string.Join(',', view.Hand.Select(card => card.Id + "/" + card.Kind))}]; " +
                $"equipment=[{string.Join(',', view.Equipment.Select(card => card.Id + "/" + card.Kind))}]; " +
                $"paid=[{string.Join(";", game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketPurchaseCommittedEvent>())}]; " +
                $"created=[{string.Join(";", game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketCardCreatedEvent>())}]; " +
                $"applied=[{string.Join(";", game.Events.Select(envelope => envelope.Payload).OfType<ShanheMarketPurchaseAppliedEvent>())}]";
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
