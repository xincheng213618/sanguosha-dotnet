using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Content.Standard;

internal static partial class ShanheCampaignChecks
{
    public static void SkillShopEscalationUsesActualPaymentAndResetsAtNextShopCold()
    {
        var definition = new ShanheCampaignDefinition("fixture:skill-shop-price", "技能同行加价", [Player], ["shop"],
        [
            new("shop", "第一家集市", ShanheNodeKind.Shop, ["next"], Choices:
                [new("guard", "守御", 200, RewardId: "guard", LeavesNode: false),
                 new("raid", "突袭", 200, RewardId: "raid", LeavesNode: false)]),
            new("next", "下一家集市", ShanheNodeKind.Shop, ["battle"], Choices:
                [new("guard", "守御", 200, RewardId: "guard", LeavesNode: false),
                 new("raid", "突袭", 200, RewardId: "raid", LeavesNode: false)]),
            new("battle", "终点", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new(Enemy)])])
        ], [new("guard", "守御", SkillId: Guard), new("raid", "突袭", SkillId: Raid)],
            StartingGold: 1000, ExtraSkillSlots: 1);
        var options = new ShanheCampaignOptions(71, Player) { UseCodexRewardPool = true };
        var session = ShanheCampaignSession.Create(definition, options, Registry);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        Require(session.Snapshot.ShopItems.All(item => item.GoldCost == 200),
            "The first actual purchase of either skill uses its authored two-hundred-coin price.");
        var guard = session.Snapshot.ShopItems.Single(item => item.Reward.Id == "guard");
        Accept(session, new ChooseShanheNodeOptionCommand(guard.ChoiceId, null, session.Revision));
        var raid = session.Snapshot.ShopItems.Single(item => item.Reward.Id == "raid");
        Require(session.Snapshot.Gold == 800 && raid.GoldCost == 300 && raid.RequiresSkillReplacement,
            "Buying one skill spends two hundred and raises the other skill to three hundred, through its real replacement flow.");
        var stale = new ChooseShanheNodeOptionCommand(raid.ChoiceId, Guard, session.Revision - 1);
        var before = Snapshot(session);
        Require(!session.Submit(stale).Accepted && Snapshot(session) == before,
            "A stale skill quote cannot spend coins or consume the next category purchase.");
        Accept(session, new ChooseShanheNodeOptionCommand(raid.ChoiceId, Guard, session.Revision));
        Require(session.Snapshot.Gold == 500 && session.Snapshot.ExtraSkillIds.SequenceEqual([Raid]),
            "The second skill really spends the displayed three hundred and replaces only the paid original extra skill.");
        Cold(session, definition);
        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Accept(session, new MoveShanheNodeCommand("next", session.Revision));
        Require(session.Snapshot.ShopItems.Single(item => item.Reward.Id == "guard").GoldCost == 200,
            "Entering another shop resets that shop's skill-category surcharge instead of charging an earlier shop's purchase count.");
        Cold(session, definition);
    }

    public static void EconomyVictoryInterestDiscountRefundAndFiveRefreshesUseRealCommandsCold()
    {
        var economy = EconomyRewards();
        var offers = new ShanheRewardDefinition[]
        {
            new("old", "旧弩", StartingEquipment: [new(CardKind.Crossbow)]),
            new("sword", "青釭剑", StartingEquipment: [new(CardKind.QinggangSword)]),
            new("bow", "另一把连弩", StartingEquipment: [new(CardKind.Crossbow)]),
            new("guard", "守御", SkillId: Guard), new("raid", "突袭", SkillId: Raid)
        };
        var definition = new ShanheCampaignDefinition("fixture:economy", "征程经济", [Player], ["gear"],
        [
            new("gear", "行前军械", ShanheNodeKind.Event, ["battle"], Choices: [new("take", "领取旧弩", RewardId: "old")]),
            new("battle", "前哨", ShanheNodeKind.Battle, ["shop"], [new("enemy", "守军", [new(Enemy)], VictoryGold: 100)]),
            new("shop", "集市", ShanheNodeKind.Shop, ["final"], Choices:
            [new("sword", "青釭剑", 200, RewardId: "sword", LeavesNode: false),
             new("bow", "另一把连弩", 200, RewardId: "bow", LeavesNode: false),
             new("guard", "守御", 200, RewardId: "guard", LeavesNode: false),
             new("raid", "突袭", 200, RewardId: "raid", LeavesNode: false),
             new("member", "会员Ⅱ", 500, RewardId: "tactic-economy-2107", LeavesNode: false)]),
            new("final", "终点", ShanheNodeKind.Battle, [], [new("enemy", "守军", [new(Enemy)], VictoryGold: 100)])
        ], economy.Concat(offers).ToArray(), StartingGold: 1000, ExtraSkillSlots: 1);
        var options = new ShanheCampaignOptions(71, Player)
        {
            UseCodexRewardPool = true,
            EquippedTacticIds = ["tactic-economy-2089", "tactic-economy-2090", "tactic-economy-2146",
                "tactic-economy-2091", "tactic-economy-2145", "tactic-economy-2092"]
        };
        var session = ShanheCampaignSession.Create(definition, options, Registry);
        Accept(session, new MoveShanheNodeCommand("gear", session.Revision));
        Accept(session, new ChooseShanheNodeOptionCommand("take", null, session.Revision));
        Accept(session, new MoveShanheNodeCommand("battle", session.Revision));
        Require(session.PendingBattleSetup!.TacticSkillIds.Count == 0 && session.Snapshot.TacticIds.Count == 6,
            "Expedition-only tactics remain real inventory without manufacturing empty battle-skill grants.");
        var battle = CreateBattle(session.PendingBattleSetup!); FinishBattle(battle, "defeat");
        Require(session.CompleteBattle(battle, session.Revision).Accepted && session.Snapshot.Gold == 1288,
            "A real victory pays 100 plus twenty percent, then fifteen percent interest on the resulting 1120 balance.");
        var before = Snapshot(session);
        Require(!session.CompleteBattle(battle, session.Revision).Accepted && Snapshot(session) == before,
            "Submitting a completed battle again cannot mint another victory bonus or interest payment.");
        Cold(session, definition);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        Buy("sword");
        Require(session.Snapshot.Gold == 1348 && session.Snapshot.StartingEquipment.Single().Kind == CardKind.QinggangSword &&
            Item("bow").GoldCost == 210,
            "The real weapon purchase pays 140 after the thirty-percent discount, refunds 200 for replacing the old slot and raises the next weapon's authored price before discount.");
        Buy("bow"); Buy("guard"); Buy("raid", Guard); Buy("tactic-economy-2107");
        Require(session.Snapshot.Gold == 638 && session.Snapshot.ShopRefreshRemaining == 5 &&
            session.Snapshot.ExtraSkillIds.SequenceEqual([Raid]),
            "Actual equipment, skill and tactic payments use their own same-category counters; acquiring membership changes this shop's real allowance to five.");
        for (var count = 0; count < 5; count++)
        {
            var gold = session.Snapshot.Gold;
            Accept(session, new RefreshShanheShopCommand(session.Revision));
            Require(session.Snapshot.Gold == gold - 100 && session.Snapshot.ShopRefreshRemaining == 4 - count,
                "Each accepted membership refresh spends exactly one hundred and consumes one of the five issued opportunities.");
        }
        before = Snapshot(session);
        Require(!session.Submit(new RefreshShanheShopCommand(session.Revision)).Accepted && Snapshot(session) == before,
            "The sixth refresh cannot spend money or mutate already-exhausted stock.");
        Cold(session, definition);
        Accept(session, new ChooseShanheNodeOptionCommand("$leave", null, session.Revision));
        Accept(session, new MoveShanheNodeCommand("final", session.Revision));
        var final = CreateBattle(session.PendingBattleSetup!); FinishBattle(final, "defeat");
        Require(session.CompleteBattle(final, session.Revision).Accepted && session.Snapshot.Phase == ShanheCampaignPhase.Won &&
            session.Snapshot.Gold == 296,
            "The terminal real victory still pays its economy effect once before final settlement, rounding fractional copper down.");
        Cold(session, definition);

        var loss = ShanheCampaignSession.Create(definition, options, Registry);
        Accept(loss, new MoveShanheNodeCommand("gear", loss.Revision));
        Accept(loss, new ChooseShanheNodeOptionCommand("take", null, loss.Revision));
        Accept(loss, new MoveShanheNodeCommand("battle", loss.Revision));
        var losingBattle = CreateBattle(loss.PendingBattleSetup!); FinishBattle(losingBattle, "surrender");
        Require(loss.CompleteBattle(losingBattle, loss.Revision).Accepted && loss.Snapshot.Phase == ShanheCampaignPhase.Lost && loss.Snapshot.Gold == 1000,
            "A genuine lost expedition does not receive victory income or local interest.");

        ShanheShopItemSnapshot Item(string reward) => session.Snapshot.ShopItems.Single(item => item.Reward?.Id == reward);
        void Buy(string reward, string? replacement = null) =>
            Accept(session, new ChooseShanheNodeOptionCommand(Item(reward).ChoiceId, replacement, session.Revision));
    }

    private static ShanheRewardDefinition[] EconomyRewards() => StandardShanheCampaignCatalog.CurrentEconomyTactics
        .Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
        { CampaignEconomy = source.Effect, Description = source.Description }).ToArray();
}
