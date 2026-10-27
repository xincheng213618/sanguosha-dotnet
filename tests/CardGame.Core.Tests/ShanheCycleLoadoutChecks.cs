using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCycleLoadoutChecks
{
    public static void PersistentMarketUsesTheActualFullCatalogueAndBattleRegistry()
    {
        foreach (var (rewardId, skillId, price, maximumReduction) in new[]
        {
            ("tactic-xuezhan-current-2265", StandardShanheRoundRecoveryPackage.Xuezhan2SkillId, 700, 2),
            ("build-skill-3128", "boundary:tishen", 300, 0),
            ("build-skill-3199", "boundary:zhaxiang", 600, 0),
            ("build-skill-3291", StandardShanheDingcuoPackage.SkillId, 300, 0),
            ("build-skill-3076", StandardShanheGuanchaoPackage.SkillId, 600, 0),
            ("build-skill-3153", "ol:luanzhan", 1000, 0),
            ("build-skill-3092", "ol:guanwei", 600, 0)
        }) CheckLivePurchase(rewardId, skillId, price, maximumReduction);
    }

    private static void CheckLivePurchase(string rewardId, string skillId, int price, int maximumReduction)
    {
        var source = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var merchant = source.Nodes.First(node => node.Kind == ShanheNodeKind.Shop);
        var definition = source with
        {
            Id = "fixture:shanhe-real-persistent-catalogue", StartingNodeIds = ["shop"], StartingGold = 20000,
            ExtraSkillSlots = 2,
            Nodes = [merchant with { Id = "shop", ChapterNumber = 1, NextNodeIds = ["battle"],
                    Choices = merchant.Choices!.Where(choice => choice.RewardId == rewardId ||
                        rewardId == "build-skill-3199" && choice.RewardId == "build-skill-3297").ToArray() },
                new("battle", "真实注册表试阵", ShanheNodeKind.Battle, [],
                    [new("guard", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])]
        };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        {
            UseCodexRewardPool = true, PersistentMarket = true,
            UnlockedRewardIds = [.. StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(source), rewardId]
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var moved = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
        Require(moved.Accepted, moved.Error ?? "The real persistent catalogue must reach its battle without a shop node.");
        var setup = session.PendingBattleSetup!;
        Require(setup.MarketSetup?.Definition.Rewards.Count == source.Rewards.Count,
            "The frozen battle catalogue retains all future purchase dependencies rather than only owned skills.");
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        Require(game.Submit(new StartGameCommand()).Accepted, "The actual complete market registry must start normally.");
        for (var step = 0; step < 120 && game.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "Real catalogue opening must advance through its native flow.");
        Require(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The actual market battle must reach Play.");
        var before = game.CreateSnapshot(0).Players[0];
        var expectedMaximum = Math.Max(1, before.MaxHp - maximumReduction);
        Require(before.Skills!.All(skill => skill.ContentId != skillId),
            "Discovering a future purchase program must not grant it before payment.");
        var item = game.CreateShanheMarketSnapshot(0)!.ShopItems.Single(item => item.Reward?.Id == rewardId);
        var bought = game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, item.ChoiceId, null, game.Revision));
        var after = game.CreateSnapshot(0).Players[0];
        Require(bought.Accepted && after.MaxHp == expectedMaximum && after.Hp == Math.Min(before.Hp, expectedMaximum) &&
            game.CreateShanheMarketSnapshot(0)!.Gold == 20000 - price &&
            after.Skills!.Any(skill => skill.ContentId == skillId) &&
            game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 },
            bought.Error?.Message ?? $"The real {price}-coin source {rewardId} must pay and initialize immediately through production registry wiring.");
        if (rewardId == "build-skill-3199") CheckPurchasedLossDrawCombo(game);
    }

    private static void CheckPurchasedLossDrawCombo(GameEngine game)
    {
        var kurou = game.CreateShanheMarketSnapshot(0)!.ShopItems.Single(item => item.Reward?.Id == "build-skill-3297");
        Require(kurou.GoldCost == 1100, "The next skill quote includes the existing same-category purchase surcharge.");
        var purchase = game.Submit(new CardGame.Core.BuyShanheMarketItemCommand(0, kurou.ChoiceId, null, game.Revision));
        Require(purchase.Accepted, purchase.Error?.Message ?? "The real unlimited Kurou source must be purchasable after Zhaxiang.");
        var before = game.CreateSnapshot(0).Players[0];
        var activated = game.Submit(new UseProgramSkillCommand(0, "classic:kujin", "lose-hp-and-draw", [], [],
            game.Revision, game.PendingDecision!.PromptId));
        Require(activated.Accepted, activated.Error?.Message ?? "Purchased Kurou must be immediately usable in the original Play phase.");
        for (var step = 0; step < 120 && game.PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            Require(game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted, "The acquired loss/draw combo must return through its native children.");
        var after = game.CreateSnapshot(0).Players[0];
        Require(game.PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            after.Hp == before.Hp - 1 && after.Hand.Count == before.Hand.Count + 5 &&
            game.CreateShanheMarketSnapshot(0)!.Gold == 18300 &&
            game.Events.Select(e => e.Payload).OfType<TurnRedSlashPolicyGrantedEvent>()
                .Count(e => e.Policy.Source.SkillId == "boundary:zhaxiang") == 1,
            "Actual paid Kurou and Zhaxiang must lose one HP, draw five cards and issue one current-turn red-Slash benefit without another payment.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
