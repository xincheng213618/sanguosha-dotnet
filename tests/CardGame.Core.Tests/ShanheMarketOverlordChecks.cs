using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheMarketOverlordChecks
{
    private const string Overlord = "tactic-economy-2012", Bargain = "tactic-economy-2091",
        Bargain2 = "tactic-economy-2145", Membership = "tactic-economy-2107";

    public static void ActualOwnershipSuppressesAllCategorySurchargesBeforeDiscount()
    {
        var sources = StandardShanheCampaignCatalog.CurrentEconomyTactics
            .Where(source => source.RewardId is Overlord or Bargain or Bargain2 or Membership).ToArray();
        var offers = new ShanheRewardDefinition[]
        {
            new("training-one", "操练一", Bonuses: new(MaxHp: 1)),
            new("training-two", "操练二", Bonuses: new(MaxHp: 1)),
            new("guard", "守御", SkillId: "fixture:shanhe-guard"),
            new("raid", "突袭", SkillId: "fixture:shanhe-raid"),
            new("crossbow", "连弩", StartingEquipment: [new(CardKind.Crossbow)]),
            new("armor", "八卦阵", StartingEquipment: [new(CardKind.BaguaFormation)]),
            new("slash", "杀", StartingCards: [new(CardKind.Slash)]),
            new("dodge", "闪", StartingCards: [new(CardKind.Dodge)])
        };
        var prices = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["training-one"] = 200, ["training-two"] = 300, ["guard"] = 80, ["raid"] = 180,
            ["crossbow"] = 100, ["armor"] = 220, ["slash"] = 150, ["dodge"] = 90,
            [Overlord] = sources.Single(source => source.RewardId == Overlord).Price
        };
        var rewards = offers.Concat(sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
        { CampaignEconomy = source.Effect, Description = source.Description })).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:market-overlord", "集市霸王实购", [ShanheCampaignChecks.Player], ["shop"],
        [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices: prices.Select(pair =>
            new ShanheNodeChoiceDefinition("buy-" + pair.Key, rewards.Single(reward => reward.Id == pair.Key).Name,
                pair.Value, RewardId: pair.Key, LeavesNode: false)).ToArray()),
         new("battle", "终点", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new(ShanheCampaignChecks.Enemy)])])],
            rewards, StartingGold: 10_000, ExtraSkillSlots: 2);
        var options = new ShanheCampaignOptions(71, ShanheCampaignChecks.Player) { UseCodexRewardPool = true };

        var ordinary = Create([]);
        Buy(ordinary, "training-one", 200);
        Require(Item(ordinary, "training-two").GoldCost == 400 && Item(ordinary, Overlord).GoldCost == 600,
            "The real Overlord offer has no effect until acquired: an ordinary tactic purchase still adds 100 to that category.");

        var session = Create([Bargain, Bargain2]);
        Buy(session, "training-one", 140); Buy(session, "crossbow", 70);
        Buy(session, "slash", 105); Buy(session, "guard", 56);
        Require(Item(session, Overlord).GoldCost == 420 && Item(session, "training-two").GoldCost == 280 &&
            Item(session, "armor").GoldCost == 224 && Item(session, "dodge").GoldCost == 133 && Item(session, "raid").GoldCost == 196,
            "Before acquiring Overlord, every category retains its independent 100-coin surcharge before the combined thirty-percent bargain discount.");
        Buy(session, Overlord, 420);
        Require(session.Snapshot.TacticIds.Contains(Overlord, StringComparer.Ordinal), "The actual market purchase must acquire the source economy tactic.");
        foreach (var pair in prices)
            Require(Item(session, pair.Key).GoldCost == pair.Value * 70 / 100,
                "Holding Overlord must remove existing surcharges from every category, then discount each authored price.");
        Buy(session, "training-two", 210); Buy(session, "armor", 154);
        Buy(session, "dodge", 63); Buy(session, "raid", 126);
        foreach (var pair in prices)
            Require(Item(session, pair.Key).GoldCost == pair.Value * 70 / 100,
                "Later actual purchases must continue to use discounted authored prices in all four categories.");
        RefreshToLimit(session, 1);

        var member = Create([Overlord, Bargain, Bargain2, Membership]);
        Require(member.Snapshot.ShopItems.All(item => item.GoldCost == prices[item.Reward!.Id] * 70 / 100),
            "An actually equipped Overlord starts the shop at authored prices with the same bargain discount.");
        RefreshToLimit(member, 5);

        ShanheCampaignSession Create(string[] equipped)
        {
            var created = ShanheCampaignSession.Create(definition, options with { EquippedTacticIds = equipped }, ShanheCampaignChecks.Registry);
            Accept(created, new MoveShanheNodeCommand("shop", created.Revision));
            return created;
        }
    }

    private static ShanheShopItemSnapshot Item(ShanheCampaignSession session, string rewardId) =>
        session.Snapshot.ShopItems.Single(item => item.Reward?.Id == rewardId);

    private static void Buy(ShanheCampaignSession session, string rewardId, int expectedPrice)
    {
        var item = Item(session, rewardId);
        Require(item.GoldCost == expectedPrice && item.CanPurchase, "The real market item must publish the exact payable price.");
        var gold = session.Snapshot.Gold;
        Accept(session, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
        Require(session.Snapshot.Gold == gold - expectedPrice, "An actual purchase must debit its price from the state before the reward is acquired.");
    }

    private static void RefreshToLimit(ShanheCampaignSession session, int limit)
    {
        Require(session.Snapshot.ShopRefreshCost == 100 && session.Snapshot.ShopRefreshRemaining == limit,
            "Overlord must preserve the ordinary or membership refresh allowance and the undiscounted 100-coin refresh fee.");
        for (var remaining = limit; remaining > 0; remaining--)
        {
            var gold = session.Snapshot.Gold;
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Require(session.Snapshot.Gold == gold - 100 && session.Snapshot.ShopRefreshRemaining == remaining - 1,
                "Every actual refresh must pay exactly 100 and consume one opportunity.");
        }
        var snapshot = session.Snapshot;
        Require(!session.Submit(new RefreshShanheShopCommand(session.Revision)).Accepted && ReferenceEquals(snapshot, session.Snapshot),
            "An exhausted ordinary or membership allowance must reject atomically.");
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var result = session.Submit(command);
        Require(result.Accepted, result.Error ?? "Campaign command rejected.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
