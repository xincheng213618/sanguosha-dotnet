using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheRewardCategoryChecks
{
    public static void CategoryChoiceCountsFollowOwnedTacticsAndStayFrozen()
    {
        var definition = FixtureDefinition();
        (string[] Equipped, ShanheRewardCategory Category, int Count)[] cases =
        [
            ([], ShanheRewardCategory.Tactic, 3),
            ([], ShanheRewardCategory.Card, 3),
            ([], ShanheRewardCategory.Skill, 3),
            (["tactic-economy-2017"], ShanheRewardCategory.Tactic, 4),
            (["tactic-economy-2017"], ShanheRewardCategory.Card, 3),
            (["tactic-economy-2018"], ShanheRewardCategory.Tactic, 3),
            (["tactic-economy-2018"], ShanheRewardCategory.Card, 4),
            (["tactic-economy-2017", "tactic-economy-2018"], ShanheRewardCategory.Skill, 3),
            (["tactic-economy-2016"], ShanheRewardCategory.Skill, 4),
            (["tactic-economy-2016"], ShanheRewardCategory.Tactic, 3),
            (["tactic-economy-2016"], ShanheRewardCategory.Card, 3),
            (["tactic-economy-2016", "tactic-economy-2017", "tactic-economy-2018"], ShanheRewardCategory.Supply, 3)
        ];
        foreach (var item in cases)
        {
            var session = Create(definition, item.Equipped);
            CompleteFirstVictory(session);
            var categories = session.Snapshot;
            Require(categories.Phase == ShanheCampaignPhase.Reward && categories.RewardOffers.Count == 0 &&
                categories.RewardCategories.SequenceEqual(Enum.GetValues<ShanheRewardCategory>()) &&
                !categories.CanSkipReward && categories.RewardSkipGold == 0 && categories.AvailableNodes.Count == 0,
                "A real victory publishes nonempty categories before any candidate rewards are revealed.");
            var before = Snapshot(session);
            Require(!session.Submit(new ChooseShanheRewardCommand("tactic-0", null, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCategoryCommand((ShanheRewardCategory)99, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCategoryCommand(item.Category, session.Revision - 1)).Accepted &&
                Snapshot(session) == before,
                "Direct claims, unknown categories and stale category revisions cannot bypass the category choice.");

            var categoryRevision = session.Revision;
            Accept(session, new ChooseShanheRewardCategoryCommand(item.Category, categoryRevision));
            var offer = session.Snapshot;
            var prefix = item.Category.ToString().ToLowerInvariant() + "-";
            Require(offer.RewardCategories.Count == 0 && offer.RewardOffers.Count == item.Count &&
                offer.RewardOffers.All(reward => reward.Id.StartsWith(prefix, StringComparison.Ordinal)) &&
                ReferenceEquals(offer.RewardOffers, session.Snapshot.RewardOffers) &&
                categories.RewardCategories.Count == 4 && categories.RewardOffers.Count == 0,
                "Each owned choice tactic increases only its own category's three choices to four; supplies retain their own pool.");
            before = Snapshot(session);
            var other = item.Category == ShanheRewardCategory.Tactic ? ShanheRewardCategory.Card : ShanheRewardCategory.Tactic;
            Require(!session.Submit(new ChooseShanheRewardCategoryCommand(other, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCategoryCommand(item.Category, session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCommand(offer.RewardOffers[0].Id, null, categoryRevision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCommand("not-published", null, session.Revision)).Accepted &&
                Snapshot(session) == before,
                "Committing a category freezes its candidates; changing or reopening it and stale or unpublished claims reject without rerolling.");

            var selected = offer.RewardOffers[0];
            Accept(session, new ChooseShanheRewardCommand(selected.Id, null, session.Revision));
            var result = session.Snapshot;
            Require(result.Phase == ShanheCampaignPhase.Map && result.AvailableNodes.Single().Id == "next" &&
                result.CompletedNodeIds.SequenceEqual(["start"]) && result.RewardCategories.Count == 0 &&
                result.RewardOffers.Count == 0 && result.BossRewardSummary is null && !result.CanSkipReward &&
                (item.Category switch
                {
                    ShanheRewardCategory.Tactic => result.Bonuses != new ShanheTacticBonuses(),
                    ShanheRewardCategory.Card => result.StartingCards.Count + result.StartingEquipment.Count == 1,
                    ShanheRewardCategory.Skill => result.ExtraSkillIds.SequenceEqual([selected.SkillId!]),
                    _ => result.Gold == 17 + selected.Gold
                }),
                "One actual selected reward is applied and the next node opens with the primary reward state cleared.");
            before = Snapshot(session);
            Require(!session.Submit(new ChooseShanheRewardCommand(selected.Id, null, session.Revision)).Accepted &&
                !session.Submit(new SkipShanheRewardCommand(session.Revision)).Accepted && Snapshot(session) == before,
                "A completed primary claim cannot be repeated or exchanged for skip compensation.");
        }
    }

    private static ShanheCampaignDefinition FixtureDefinition()
    {
        var tactics = Enumerable.Range(0, 5).Select(index => new ShanheRewardDefinition("tactic-" + index,
            "操练" + index, Bonuses: new(MaxHp: index + 1))).ToArray();
        ShanheRewardDefinition[] cards =
        [
            new("card-slash-1", "军资一", StartingCards: [new(CardKind.Slash, Rank: 1)]),
            new("card-slash-2", "军资二", StartingCards: [new(CardKind.Slash, Rank: 2)]),
            new("card-crossbow", "诸葛连弩", StartingEquipment: [new(CardKind.Crossbow)]),
            new("card-qinglong", "青龙偃月刀", StartingEquipment: [new(CardKind.QinglongCrescentBlade)])
        ];
        ShanheRewardDefinition[] skills =
        [
            new("skill-guard", "守御", SkillId: "fixture:shanhe-guard"),
            new("skill-raid", "突袭", SkillId: "fixture:shanhe-raid"),
            new("skill-yingzi", "英姿", SkillId: "standard:yingzi"),
            new("skill-wusheng", "武圣", SkillId: "standard:wusheng"),
            new("skill-paoxiao", "咆哮", SkillId: "standard:paoxiao")
        ];
        var supplies = Enumerable.Range(1, 4).Select(index => new ShanheRewardDefinition("supply-" + index,
            "铜钱" + index, Gold: index)).ToArray();
        var rewards = tactics.Concat(cards).Concat(skills).Concat(supplies).ToArray();
        var economy = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => source.SourceId is 2016 or 2017 or 2018)
            .Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity }).ToArray();
        Require(economy.Length == 3, "All three authored category-choice tactics must be registered in the current catalogue.");
        return new("fixture:reward-categories", "分类奖励", [ShanheCampaignChecks.Player], ["start"],
        [
            new("start", "渡口", ShanheNodeKind.Battle, ["next"],
                [new("guard", "守军", [new(ShanheCampaignChecks.Enemy)], VictoryGold: 7)], rewards.Select(reward => reward.Id).ToArray())
                { CategorizedRewards = true, RewardSkipGold = 100 },
            new("next", "关隘", ShanheNodeKind.Battle, [], [new("guard-next", "守军", [new(ShanheCampaignChecks.Enemy)])])
        ], rewards.Concat(economy).ToArray(), StartingGold: 10, ExtraSkillSlots: 1);
    }

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, IReadOnlyList<string> tactics) =>
        ShanheCampaignSession.Create(definition, new(71, ShanheCampaignChecks.Player) { EquippedTacticIds = tactics }, ShanheCampaignChecks.Registry);

    private static void CompleteFirstVictory(ShanheCampaignSession session)
    {
        Accept(session, new MoveShanheNodeCommand("start", session.Revision));
        var battle = ShanheCampaignChecks.CreateBattle(session.PendingBattleSetup!);
        ShanheCampaignChecks.FinishBattle(battle, "defeat");
        var result = session.CompleteBattle(battle, session.Revision);
        Require(result.Accepted, result.Error ?? "A real victory must create its categorized reward selection.");
    }

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    {
        var result = session.Submit(command);
        Require(result.Accepted, result.Error ?? "Campaign command rejected.");
    }

    private static string Snapshot(ShanheCampaignSession session) => JsonSerializer.Serialize(session.Snapshot);
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
