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
            (["tactic-economy-2017"], ShanheRewardCategory.Tactic, 4),
            (["tactic-economy-2017"], ShanheRewardCategory.Card, 3),
            (["tactic-economy-2018"], ShanheRewardCategory.Tactic, 3),
            (["tactic-economy-2018"], ShanheRewardCategory.Card, 4),
            (["tactic-economy-2017", "tactic-economy-2018"], ShanheRewardCategory.Skill, 2),
            (["tactic-economy-2017", "tactic-economy-2018"], ShanheRewardCategory.Supply, 3)
        ];
        foreach (var item in cases)
        {
            var session = Create(definition, item.Equipped);
            CompleteFirstVictory(session);
            var categories = session.Snapshot;
            Require(categories.Phase == ShanheCampaignPhase.Reward && categories.RewardOffers.Count == 0 &&
                categories.RewardCategories.SequenceEqual(Enum.GetValues<ShanheRewardCategory>()) &&
                categories.CanSkipReward && categories.RewardSkipGold == 100 && categories.AvailableNodes.Count == 0,
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
                "The owned tactic increases only its own category's three choices to four; skills and supplies retain their own pools.");
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

    public static void CategorySkippingPaysOnceAndClearsBeforeNextBattle()
    {
        var definition = FixtureDefinition();
        definition = definition with
        {
            Nodes = definition.Nodes.Select(node => node.Id == "start" ? node with
            {
                RewardIds = node.RewardIds!.Where(id => id.StartsWith("tactic-", StringComparison.Ordinal) ||
                    id.StartsWith("card-", StringComparison.Ordinal)).ToArray()
            } : node).ToArray()
        };
        foreach (var openCategory in new[] { false, true })
        {
            var session = Create(definition, ["tactic-economy-2017"]);
            CompleteFirstVictory(session);
            var before = Snapshot(session);
            Require(!session.Submit(new ChooseShanheRewardCategoryCommand(ShanheRewardCategory.Supply, session.Revision)).Accepted &&
                Snapshot(session) == before,
                "A known but empty category is not selectable.");
            if (openCategory) Accept(session, new ChooseShanheRewardCategoryCommand(ShanheRewardCategory.Tactic, session.Revision));
            var gold = session.Snapshot.Gold;
            Accept(session, new SkipShanheRewardCommand(session.Revision));
            Require(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.Gold == gold + 100 &&
                session.Snapshot.RewardCategories.Count == 0 && session.Snapshot.RewardOffers.Count == 0 &&
                session.Snapshot.Bonuses == new ShanheTacticBonuses() &&
                session.Snapshot.StartingCards.Count == 0 && session.Snapshot.StartingEquipment.Count == 0 &&
                session.Snapshot.CompletedNodeIds.SequenceEqual(["start"]) &&
                session.AcceptedCommands.OfType<SkipShanheRewardCommand>().Count() == 1,
                "Skipping before or after category selection pays the node's compensation exactly once and grants no candidate reward.");
            before = Snapshot(session);
            Require(!session.Submit(new SkipShanheRewardCommand(session.Revision)).Accepted &&
                !session.Submit(new ChooseShanheRewardCategoryCommand(ShanheRewardCategory.Card, session.Revision)).Accepted &&
                Snapshot(session) == before, "A skipped primary reward cannot be reopened or paid twice.");
            Accept(session, new MoveShanheNodeCommand("next", session.Revision));
            Require(session.Snapshot.Phase == ShanheCampaignPhase.Battle && session.PendingBattleSetup?.NodeId == "next" &&
                session.Snapshot.RewardCategories.Count == 0 && session.Snapshot.RewardOffers.Count == 0 &&
                !session.Snapshot.CanSkipReward && session.Snapshot.Gold == gold + 100,
                "The actual next battle starts without carrying the prior category, candidates or skip allowance.");
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
            new("skill-raid", "突袭", SkillId: "fixture:shanhe-raid")
        ];
        var supplies = Enumerable.Range(1, 4).Select(index => new ShanheRewardDefinition("supply-" + index,
            "铜钱" + index, Gold: index)).ToArray();
        var rewards = tactics.Concat(cards).Concat(skills).Concat(supplies).ToArray();
        var economy = StandardShanheCampaignCatalog.CurrentEconomyTactics.Where(source => source.SourceId is 2017 or 2018)
            .Select(source => new ShanheRewardDefinition(source.RewardId, source.Name)
            { CampaignEconomy = source.Effect, Rarity = source.Rarity }).ToArray();
        Require(economy.Length == 2, "Both authored category-choice tactics must be registered in the current catalogue.");
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
