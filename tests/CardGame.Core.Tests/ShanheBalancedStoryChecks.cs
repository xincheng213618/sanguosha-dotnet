using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheBalancedStoryChecks
{
    public static void CategoriesRareTacticsLegacyUpgradeAndColdReplay()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(1);
        var first = source.Nodes.First(node => node.ChapterRole == ShanheChapterNodeRole.Event && node.ChapterNumber == 1);
        var second = source.Nodes.First(node => node.ChapterRole == ShanheChapterNodeRole.Event && node.ChapterNumber == 2);
        var tacticStory = first.SupplementalStoryEvents!.Single(story => story.Id == StandardShanheCampaignCatalog.RareTacticStoryId);
        Need(first.StoryEvents!.Count == 21 && second.StoryEvents!.Count == 17 &&
                first.StoryEvents.All(story => story.TacticRewardIds is null) &&
                tacticStory.TacticRewardIds is IList<string> { IsReadOnly: true } && tacticStory.TacticRewardIds.Count > 2 &&
                tacticStory.TacticRewardIds.All(id => source.Rewards.Single(reward => reward.Id == id).Rarity == 2),
            "The legacy source lists retain their exact sizes and the opt-in supplement holds a frozen complete rare tactic pool.");
        var options = new ShanheCampaignOptions(71, "classic:guan-yu")
            { SourceDifficultyId = 1, UseCodexRewardPool = true, UseBalancedJourneyRewards = true, UnlockedRewardIds = [] };
        var definition = Make(Enumerable.Range(0, 6).Select(index => index < 3 ? first : second).ToArray());
        var session = Create(definition, options);
        for (var chapter = 0; chapter < 2; chapter++)
        {
            var categories = new HashSet<string>();
            for (var index = 1; index <= 3; index++)
            {
                Enter(session, "event-" + (chapter * 3 + index));
                var choices = session.Snapshot.NodeChoices;
                var category = Category(choices);
                Need(categories.Add(category), "Three available categories appear once each in the current chapter, independently of recruitment entry count.");
                Cold(session);
                var choice = category == "recruit" ? choices.First(item => item.Companion is not null && item.GoldCost == 0)
                    : choices.First(item => item.RewardId is { } id && (category == "skill" ? item.Id.StartsWith("story-skill:", StringComparison.Ordinal)
                        : item.Id.StartsWith("story-tactic:", StringComparison.Ordinal) && source.Rewards.Single(reward => reward.Id == id).CampaignEconomy?.FavorOwnedSchoolRewards != true));
                var gold = session.Snapshot.Gold;
                var tacticCount = session.Snapshot.TacticIds.Count;
                var skillCount = session.Snapshot.ExtraSkillIds.Count;
                if (category == "tactic")
                    Need(choices.Count(item => item.Id.StartsWith("story-tactic:", StringComparison.Ordinal)) == 2 &&
                            choices.Where(item => item.Id.StartsWith("story-tactic:", StringComparison.Ordinal))
                                .All(item => source.Rewards.Single(reward => reward.Id == item.RewardId).Rarity >= 2),
                        "The revealed choices are actual rare-or-better rewards rather than recoloured white tactics.");
                Accept(session, new ChooseShanheNodeOptionCommand(choice.Id, null, session.Revision));
                Need(session.Snapshot.Gold == gold && (category != "tactic" || session.Snapshot.TacticIds.Count == tacticCount + 1 && session.Snapshot.TacticIds.Contains(choice.RewardId!)) &&
                        (category != "skill" || session.Snapshot.ExtraSkillIds.Count == skillCount + 1),
                    "One real free story choice grants exactly its selected tactic or skill, without a research unlock or extra payment.");
                var after = JsonSerializer.Serialize(session.Snapshot);
                Need(!session.Submit(new ChooseShanheNodeOptionCommand(choice.Id, null, session.Revision)).Accepted && JsonSerializer.Serialize(session.Snapshot) == after,
                    "A consumed story choice cannot grant or pay again.");
                Cold(session);
            }
            Need(categories.SetEquals(["recruit", "tactic", "skill"]), "Each chapter restarts its three-category cycle.");
        }

        var recruit = first.StoryEvents.First(story => story.Choices.Any(choice => choice.Companion is not null));
        var upgradeNodes = new[] { first with { StoryEvents = [recruit], SupplementalStoryEvents = null }, first, first };
        var upgradeDefinition = Make(upgradeNodes);
        var legacyOptions = options with { UseBalancedJourneyRewards = false };
        var legacy = Create(upgradeDefinition, legacyOptions);
        Enter(legacy, "event-1");
        Need(Category(legacy.Snapshot.NodeChoices) == "recruit", "The legacy command really revealed a recruitment event before opt-in.");
        var oldJson = ShanheCampaignJson.Serialize(legacy.CreateCheckpoint());
        Need(!oldJson.Contains("UseBalancedJourneyRewards", StringComparison.Ordinal), "The missing optional field retains the historical JSON form.");
        var explicitFalse = oldJson.Replace("\"Options\":{", "\"Options\":{\"UseBalancedJourneyRewards\":false,", StringComparison.Ordinal);
        Need(explicitFalse != oldJson, "The compatibility case actually inserts explicit false.");
        foreach (var json in new[] { oldJson, explicitFalse })
        {
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(json), upgradeDefinition, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(!restored.HasBalancedJourneyRewards && JsonSerializer.Serialize(restored.Snapshot) == JsonSerializer.Serialize(legacy.Snapshot),
                "Missing and false options cold-replay the same historical pending story and choices.");
        }
        var pending = JsonSerializer.Serialize(legacy.Snapshot.NodeChoices);
        Accept(legacy, new EnableShanheBalancedRewardsCommand(legacy.Revision));
        Need(legacy.HasBalancedJourneyRewards && JsonSerializer.Serialize(legacy.Snapshot.NodeChoices) == pending,
            "An explicit safe upgrade preserves the already-revealed legacy choices.");
        Accept(legacy, new ChooseShanheNodeOptionCommand(legacy.Snapshot.NodeChoices.First(choice => choice.Companion is not null && choice.GoldCost == 0).Id, null, legacy.Revision));
        var upgradedCategories = new HashSet<string>();
        foreach (var id in new[] { "event-2", "event-3" })
        {
            Enter(legacy, id);
            var category = Category(legacy.Snapshot.NodeChoices);
            Need(category != "recruit" && upgradedCategories.Add(category), "Previously visited legacy recruitment counts toward this chapter after safe upgrade.");
            var choice = legacy.Snapshot.NodeChoices.First(item => item.Id.StartsWith(category == "tactic" ? "story-tactic:" : "story-skill:", StringComparison.Ordinal) &&
                source.Rewards.Single(reward => reward.Id == item.RewardId).CampaignEconomy?.FavorOwnedSchoolRewards != true);
            Accept(legacy, new ChooseShanheNodeOptionCommand(choice.Id, null, legacy.Revision));
            Cold(legacy);
        }
        Need(upgradedCategories.SetEquals(["tactic", "skill"]), "The upgraded current chapter still supplies the two previously missing categories.");

        var tacticOnly = first with { StoryEvents = [tacticStory], SupplementalStoryEvents = null };
        var decline = Create(Make([tacticOnly]), options);
        Enter(decline, "event-1");
        var declineGold = decline.Snapshot.Gold;
        Accept(decline, new ChooseShanheNodeOptionCommand("decline-story-tactics", null, decline.Revision));
        Need(decline.Snapshot.Gold == declineGold + 210 && decline.Snapshot.TacticIds.Count == 0,
            "Declining the two source1928 tactic options credits exactly 210 coins and no tactic.");
        Cold(decline);
        var ownedPool = tacticStory.TacticRewardIds!.Where(id => source.Rewards.Single(reward => reward.Id == id).CampaignEconomy is
            { StartingGold: 0, FavorOwnedSchoolRewards: false }).Take(2).ToArray();
        var exhausted = first with { StoryEvents = [tacticStory with { TacticRewardIds = ownedPool }], SupplementalStoryEvents = null };
        var unavailable = Create(Make([exhausted]), options with { UnlockedRewardIds = ownedPool, EquippedTacticIds = ownedPool });
        Enter(unavailable, "event-1");
        Need(ownedPool.Length == 2 && unavailable.Snapshot.NodeChoices is [{ Id: "decline-story-tactics" }],
            "An exhausted tactic pool retains its real decline continuation without inventing another candidate or blocking the route.");
        Accept(unavailable, new ChooseShanheNodeOptionCommand("decline-story-tactics", null, unavailable.Revision));
        Cold(unavailable);

        var rumor = first.StoryEvents.Single(story => story.Id == "story:rumor");
        var filtered = Create(Make([first with { StoryEvents = [rumor], SupplementalStoryEvents = null }], withCamp: true), options);
        Enter(filtered, "event-1");
        Need(filtered.Snapshot.NodeChoices.All(choice => choice.RewardId != "opening"), "A newly revealed balanced story cannot offer the old white numeric opening-hand tactic.");
        Accept(filtered, new ChooseShanheNodeOptionCommand(filtered.Snapshot.NodeChoices.Single(choice => choice.RewardId == "medicine").Id, null, filtered.Revision));
        Enter(filtered, "camp");
        var draw = filtered.Snapshot.Bonuses.Draw;
        Accept(filtered, new ChooseShanheNodeOptionCommand("draw", null, filtered.Revision));
        Need(filtered.Snapshot.Bonuses.Draw == draw + 1, "The story tactic floor never removes ordinary camp growth.");
        Cold(filtered);

        var recruits = first.StoryEvents.Where(story => story.Choices.Any(choice => choice.Companion is not null && choice.GoldCost == 0))
            .DistinctBy(story => story.Choices.First(choice => choice.Companion is not null && choice.GoldCost == 0).Companion!.GeneralId).Take(6).ToArray();
        var fullDefinition = Make(recruits.Select(story => first with { StoryEvents = [story], SupplementalStoryEvents = null }).Append(first).ToArray());
        var full = Create(fullDefinition, options);
        for (var index = 1; index <= 6; index++)
        {
            Enter(full, "event-" + index);
            Accept(full, new ChooseShanheNodeOptionCommand(full.Snapshot.NodeChoices.First(choice => choice.Companion is not null && choice.GoldCost == 0).Id, null, full.Revision));
        }
        Enter(full, "event-7");
        Need(full.Snapshot.RecruitedCompanions.Count == 6 && Category(full.Snapshot.NodeChoices) != "recruit",
            "A genuinely full recruited roster falls back to obtainable tactic or skill rewards instead of another unusable recruitment event.");
        Cold(full);

        var wolongReward = source.Rewards.Single(reward => reward.CampaignEconomy?.FavorOwnedSchoolRewards == true);
        // Seed71 first selects skill, then tactics twice while recruitment is
        // still available: this distinguishes Wolong from the default cycle.
        var repeatedDefinition = Make([first, first, first]);
        var repeated = Create(repeatedDefinition, options with { UnlockedRewardIds = [wolongReward.Id], EquippedTacticIds = [wolongReward.Id] });
        var repeatedCategories = new List<string>();
        foreach (var id in new[] { "event-1", "event-2", "event-3" })
        {
            Enter(repeated, id);
            var category = Category(repeated.Snapshot.NodeChoices);
            repeatedCategories.Add(category);
            var choice = category == "tactic" ? "decline-story-tactics" : repeated.Snapshot.NodeChoices.First(item =>
                item.Id.StartsWith("story-skill:", StringComparison.Ordinal)).Id;
            Accept(repeated, new ChooseShanheNodeOptionCommand(choice, null, repeated.Revision));
            Cold(repeated);
        }
        Need(repeatedCategories.SequenceEqual(["skill", "tactic", "tactic"]),
            "The actual owned Wolong map permits a repeated category before every other available category has appeared, under the declared local category weights.");

        ShanheCampaignDefinition Make(IReadOnlyList<ShanheNodeDefinition> stages, bool withCamp = false)
        {
            var nodes = stages.Select((stage, index) => new ShanheNodeDefinition("event-" + (index + 1), "实际奇遇池", ShanheNodeKind.Event,
                [index + 1 < stages.Count ? "event-" + (index + 2) : withCamp ? "camp" : "after"])
            { ChapterNumber = stage.ChapterNumber, StoryEvents = stage.StoryEvents, SupplementalStoryEvents = stage.SupplementalStoryEvents }).ToList();
            if (withCamp) nodes.Add(new("camp", "休息", ShanheNodeKind.Camp, ["after"], Choices: [new("draw", "摸牌数+1", RewardId: "chapter-rest-draw")]));
            nodes.Add(new("after", "后续战斗", ShanheNodeKind.Battle, [], source.Nodes.First(node => node.Kind == ShanheNodeKind.Battle).Encounters, []));
            return source with { Id = "fixture:balanced-source-stories", PlayerGeneralIds = ["classic:guan-yu"], StartingNodeIds = ["event-1"],
                RandomStartingNode = false, Nodes = nodes, StartingGold = 600, ExtraSkillSlots = 2 };
        }
        static ShanheCampaignSession Create(ShanheCampaignDefinition definition, ShanheCampaignOptions options) =>
            ShanheCampaignSession.Create(definition, options, StandardShanheCampaignCatalog.CreateBattleRegistry);
        static string Category(IReadOnlyList<ShanheNodeChoiceDefinition> choices) => choices.Any(choice => choice.Id.StartsWith("story-tactic:", StringComparison.Ordinal)) ? "tactic"
            : choices.Any(choice => choice.Id.StartsWith("story-skill:", StringComparison.Ordinal)) ? "skill" : choices.Any(choice => choice.Companion is not null) ? "recruit" : "other";
        static void Enter(ShanheCampaignSession session, string id) => Accept(session, new MoveShanheNodeCommand(id, session.Revision));
        static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
        { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Balanced story command rejected."); }
        static void Cold(ShanheCampaignSession session)
        {
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())), session.Definition, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(JsonSerializer.Serialize(restored.Snapshot) == JsonSerializer.Serialize(session.Snapshot) && restored.HasBalancedJourneyRewards == session.HasBalancedJourneyRewards,
                "Cold replay preserves the exact revealed event, generated reward candidates, category history, payment and opt-in boundary.");
        }
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
