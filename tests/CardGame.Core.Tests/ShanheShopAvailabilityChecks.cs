using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheShopAvailabilityChecks
{
    public static void VerifiedUnmappedSkillsReachTheActualShopWithoutUnlockingResearchSkills()
    {
        var source = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var defaults = StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(source);
        Need(StandardShanheOriginalSkills.DefaultStockRewardIds.Count == 12 &&
            StandardShanheOriginalSkills.DefaultStockRewardIds.All(defaults.Contains) &&
            new[] { "original-shizhan", "original-jueqing", "original-sanyao", "original-qingxian", "original-huituo" }
                .All(id => !defaults.Contains(id)),
            "Only the verified source skills without S5 research bindings receive default supply eligibility.");
        var ids = new[] { "original-jijiu", "original-kuanggu", "original-lianying", "original-jishe", "original-wusheng",
            "original-huituo", "codex-card-4059" };
        var choices = source.Nodes.First(node => node.Kind == ShanheNodeKind.Shop).Choices!
            .Where(choice => choice.RewardId is { } id && ids.Contains(id)).ToArray();
        Need(choices.Length == ids.Length, "The actual catalogue must have authored prices for all selected goods.");
        var definition = source with
        {
            Id = "fixture:shanhe-source-stock-availability", StartingNodeIds = ["shop"], StartingGold = 2000,
            Nodes = [new("shop", "随行集市", ShanheNodeKind.Shop, ["battle"], Choices: choices),
                new("battle", "下一场", ShanheNodeKind.Battle, [],
                    [new("guard", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])]
        };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        {
            UseCodexRewardPool = true, UnlockedRewardIds = defaults
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(session, new MoveShanheNodeCommand("shop", session.Revision));
        var stock = session.Snapshot.ShopItems;
        Need(stock.Count(item => item.Reward?.SkillId is not null) == 5 &&
            stock.All(item => item.Reward?.Id != "original-huituo") &&
            stock.Single(item => item.Reward?.Id == "codex-card-4059") is { GoldCost: 600, CanPurchase: true } axe &&
            axe.Reward!.StartingEquipment is [{ Kind: CardKind.ShanheXingtianAxe }],
            "Real stock includes the five available construction skills and the original-price Xingtian axe while retaining Huituo's research gate.");
        var emergency = stock.Single(item => item.Reward?.Id == "original-jijiu");
        Need(emergency is { CanPurchase: true, GoldCost: 300 }, "The existing Jijiu source keeps its real 300-coin price.");
        Accept(session, new ChooseShanheNodeOptionCommand(emergency.ChoiceId, null, session.Revision));
        Need(session.Snapshot.Gold == 1700 && session.Snapshot.ExtraSkillIds.Contains("classic:jijiu"),
            "The newly reachable good pays once and grants the actual skill through the ordinary skill slot path.");
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Rejected real stock command."); }
    private static void Need(bool valid, string message)
    { if (!valid) throw new InvalidOperationException(message); }
}
