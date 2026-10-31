using System.Reflection;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGamblerChecks
{
    private const string First = "tactic-economy-2217", Second = "tactic-economy-2218";
    private static readonly FieldInfo StateField = typeof(ShanheCampaignSession).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly MethodInfo BalanceMethod = typeof(ShanheCampaignSession).GetMethod("EconomyBattleBalance", BindingFlags.NonPublic | BindingFlags.Instance)!;

    public static void PaidRangesKeepOneDeterministicIncomeAndLeaveQueriesPure()
    {
        var session = Enter(71);
        Need(Balance(session, 200) == 10200, "Without a gambler, the real two-hundred battle base remains two hundred.");
        Buy(session, First, 200);
        Need(session.Snapshot.Gold == 9800 && session.Snapshot.TacticIds.Contains(First) &&
            Balance(session, 200) == 10110 && Balance(session, 200) == 10110,
            "The actual common purchase pays two hundred once; fixed seed 71 yields 155 percent, rather than a permanent flat bonus.");
        Buy(session, Second, 400);
        Need(session.Snapshot.Gold == 9400 && session.Snapshot.TacticIds.Contains(First) && session.Snapshot.TacticIds.Contains(Second) &&
            Balance(session, 200) == 9656 && Balance(session, 200) == 9656,
            "The second real tactic costs its three-hundred base plus the category surcharge; the wider held tier replaces the first range and yields 128 percent.");

        var higherOnly = Enter(71);
        Buy(higherOnly, Second, 300);
        Need(Balance(higherOnly, 200) - higherOnly.Snapshot.Gold == Balance(session, 200) - session.Snapshot.Gold,
            "Owning both absolute ranges has exactly the higher tier's income, without multiplying, adding or drawing twice.");
        var oldRevision = session.Revision;
        var beforeRefresh = session.Snapshot.Gold;
        Accept(session, new RefreshShanheShopCommand(session.Revision));
        Need(session.Revision == oldRevision + 1 && session.Snapshot.Gold == beforeRefresh - 100 &&
            Balance(session, 200) - session.Snapshot.Gold == 256,
            "A real stock refresh pays its own hundred coins and advances its own sampling without consuming or rerolling this encounter's income sample.");
    }

    private static ShanheCampaignSession Enter(int seed)
    {
        var sources = StandardShanheCampaignCatalog.CurrentEconomyTactics
            .Where(source => source.SourceId is 2217 or 2218 or 2089 or 2090 or 2092).ToArray();
        Need(sources.Length == 5, "All five actual economy catalogue rows must be registered.");
        var rewards = sources.Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
        { CampaignEconomy = source.Effect, Rarity = source.Rarity, Description = source.Description }).Concat(new[]
        {
            new ShanheRewardDefinition("initial-weapon", "旧弩", StartingEquipment: [new(CardKind.Crossbow)]),
            new ShanheRewardDefinition("replacement-weapon", "新刀", StartingEquipment: [new(CardKind.QinglongCrescentBlade)]),
            new ShanheRewardDefinition("direct-gold", "赠金", Gold: 37)
        }).ToArray();
        var choices = sources.Select(source => new ShanheNodeChoiceDefinition("buy-" + source.RewardId, source.Name,
            source.Price, RewardId: source.RewardId, LeavesNode: false)).Concat(new[]
        {
            new ShanheNodeChoiceDefinition("buy-initial", "旧弩", 10, RewardId: "initial-weapon", LeavesNode: false),
            new ShanheNodeChoiceDefinition("buy-replacement", "新刀", 10, RewardId: "replacement-weapon", LeavesNode: false),
            new ShanheNodeChoiceDefinition("gift", "赠金", RewardId: "direct-gold", LeavesNode: false)
        }).ToArray();
        var definition = new ShanheCampaignDefinition("fixture:gambler", "赌徒真实集市", [ShanheCampaignChecks.Player], ["shop"],
            [new("shop", "集市", ShanheNodeKind.Shop, ["battle"], Choices: choices),
             new("battle", "驻军", ShanheNodeKind.Battle, [], [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)], VictoryGold: 200)])],
            rewards, StartingGold: 10000);
        var session = ShanheCampaignSession.Create(definition,
            new(seed, ShanheCampaignChecks.Player) { UseCodexRewardPool = true }, ShanheCampaignChecks.Registry);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        return session;
    }

    private static void Buy(ShanheCampaignSession session, string rewardId, int price, int expectedGoldGrant = 0)
    {
        var item = session.Snapshot.ShopItems.Single(value => value.Reward?.Id == rewardId);
        Need(item.CanPurchase && item.GoldCost == price, $"The real published quote for {rewardId} must be {price}, actual {item.GoldCost}.");
        var before = session.Snapshot.Gold;
        Accept(session, new ChooseShanheNodeOptionCommand(item.ChoiceId, null, session.Revision));
        Need(session.Snapshot.Gold == before - price + expectedGoldGrant, "The real purchase pays its quote once and applies only its authored immediate copper.");
    }

    private static int Balance(ShanheCampaignSession session, int battleGold)
    {
        // Invoke only the existing pure arithmetic entry point, with the real
        // state produced by fresh native purchase commands; no battle receipt.
        var state = StateField.GetValue(session)!;
        var snapshot = session.Snapshot;
        var revision = session.Revision;
        var gold = snapshot.Gold;
        var result = (int)BalanceMethod.Invoke(session, [state, battleGold])!;
        Need(ReferenceEquals(state, StateField.GetValue(session)) && ReferenceEquals(snapshot, session.Snapshot) &&
            session.Revision == revision && session.Snapshot.Gold == gold,
            "Computing a prospective balance never mutates the actual principal, purchase state, snapshot or revision.");
        return result;
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected gambler purchase command."); }
    private static void Need(bool valid, string message) { if (!valid) throw new InvalidOperationException(message); }
}
