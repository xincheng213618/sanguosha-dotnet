using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheStoryEventChecks
{
    public static void RandomStoriesRecruitmentAndSkillOffersRecoverCold()
    {
        var production = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var village = production.Nodes.Single(node => node.Id == "village");
        Need(village.StoryEvents!.Count >= 8 && village.StoryEvents.Select(story => story.Id).Distinct().Count() == village.StoryEvents.Count,
            "Production has distinct social, recruitment, ordinary/elite combat and skill stories.");
        Need(village.StoryEvents is IList<ShanheStoryEventDefinition> { IsReadOnly: true } &&
            village.StoryEvents.All(story => story.Choices is IList<ShanheNodeChoiceDefinition> { IsReadOnly: true }),
            "The published event pool and each nested choice list are frozen.");
        var social = village.StoryEvents.Where(story => story.Id is "story:village" or "story:caravan" or "story:rumor").ToArray();
        var twice = Fixture(production, social);
        twice = twice with { Nodes = twice.Nodes.Select(node => node.Id == "event" ? node with { NextNodeIds = ["event2"] } : node)
            .Append(new("event2", "下一处奇遇", ShanheNodeKind.Event, ["after"]) { StoryEvents = social }).ToArray() };
        var distinct = Create(twice);
        Accept(distinct, new MoveShanheNodeCommand("event", distinct.Revision));
        var firstStory = distinct.Snapshot.CurrentStoryEventName;
        var lastChoice = distinct.Snapshot.NodeChoices.Last();
        Accept(distinct, new ChooseShanheNodeOptionCommand(lastChoice.Id, null, distinct.Revision));
        Accept(distinct, new MoveShanheNodeCommand("event2", distinct.Revision));
        Need(distinct.Snapshot.CurrentStoryEventName != firstStory, "The next event location prefers a story not yet visited in this expedition.");
        Cold(distinct);
        var session = Create(Fixture(production, village.StoryEvents));
        Accept(session, new MoveShanheNodeCommand("event", session.Revision));
        Cold(session);
        Need(!session.Submit(new ChooseShanheNodeOptionCommand("fight-elite", null, session.Revision)).Accepted ||
            session.Snapshot.Phase == ShanheCampaignPhase.Battle, "Only an actually revealed battle option can begin combat.");

        foreach (var storyId in new[] { StandardShanheCampaignCatalog.RecruitmentStoryId, "story:jianluojinpao" })
        foreach (var elite in new[] { false, true })
        {
            var story = village.StoryEvents.Single(story => story.Id == storyId);
            var definition = Fixture(production, [story]);
            var run = Create(definition);
            Accept(run, new MoveShanheNodeCommand("event", run.Revision));
            var option = run.Snapshot.NodeChoices.Single(choice => choice.Companion is not null && choice.GoldCost == (elite ? 400 : 0));
            var gold = run.Snapshot.Gold;
            Accept(run, new ChooseShanheNodeOptionCommand(option.Id, null, run.Revision));
            Need(run.Snapshot.Gold == gold - option.GoldCost && run.Snapshot.RecruitedCompanions.Count == 1 &&
                run.Snapshot.ExtraSkillIds.Count == 0, "Recruiting pays the published price and uses no player skill slot.");
            Cold(run);
            var preview = run.PreviewEncounter("after");
            Accept(run, new MoveShanheNodeCommand("after", run.Revision));
            Need(JsonSerializer.Serialize(preview.Allies) == JsonSerializer.Serialize(run.PendingBattleSetup!.AllyRoster),
                "The recruited variant appears in the exact previewed native roster.");
            var game = StandardShanheCampaignCatalog.CreateBattle(run.PendingBattleSetup!);
            Need(game.Submit(new StartGameCommand()).Accepted, "Both variants start real native battles.");
            var member = game.CreateSnapshot(0).Players.Single(player => player.Seat == 1);
            var expected = storyId == StandardShanheCampaignCatalog.RecruitmentStoryId
                ? elite ? new[] { "boundary:beige", "boundary:duanchang", StandardShanheRecruitmentPackage.HuihuoSkillId }
                    : ["boundary:beige", "boundary:duanchang"]
                : new[] { "boundary:quhu-current", elite ? "boundary:jieming-current" : "classic:jieming" };
            Need(member.Skills!.Select(skill => skill.Id).ToHashSet().SetEquals(expected),
                "The paid variant has the screenshot's actual additional or replacement skill, without retaining both Jieming versions.");
            Cold(run);
        }

        var recruit = village.StoryEvents.Single(story => story.Id == StandardShanheCampaignCatalog.RecruitmentStoryId);
        var poor = Create(Fixture(production, [recruit]) with { StartingGold = 399 });
        Accept(poor, new MoveShanheNodeCommand("event", poor.Revision));
        var before = JsonSerializer.Serialize(poor.Snapshot);
        Need(!poor.Submit(new ChooseShanheNodeOptionCommand("recruit-elite", null, poor.Revision)).Accepted &&
            JsonSerializer.Serialize(poor.Snapshot) == before, "An unaffordable elite choice spends nothing and keeps the revealed event.");
        Accept(poor, new ChooseShanheNodeOptionCommand("decline-recruit", null, poor.Revision));
        Need(poor.Snapshot.Gold == 599 && poor.Snapshot.RecruitedCompanions.Count == 0, "Declining recruitment credits exactly 200 copper.");

        var skills = village.StoryEvents.Single(story => story.SkillRewardIds is not null);
        foreach (var decline in new[] { false, true })
        {
            var run = Create(Fixture(production, [skills]));
            Accept(run, new MoveShanheNodeCommand("event", run.Revision));
            var options = run.Snapshot.NodeChoices.Where(choice => choice.Id.StartsWith("story-skill:", StringComparison.Ordinal)).ToArray();
            Need(options.Length == 2 && options.All(choice => choice.GoldCost == 0 &&
                production.Rewards.Single(reward => reward.Id == choice.RewardId).Rarity == 2),
                "The skill story reveals two distinct rare skills even with no research unlocks.");
            Cold(run);
            Accept(run, new ChooseShanheNodeOptionCommand(decline ? "decline-story-skills" : options[0].Id, null, run.Revision));
            Need(decline ? run.Snapshot.Gold == 810 && run.Snapshot.ExtraSkillIds.Count == 0 :
                run.Snapshot.Gold == 600 && run.Snapshot.ExtraSkillIds.Count == 1,
                "The player either receives one free skill or exactly 210 copper, never both.");
            Cold(run);
        }
    }

    internal static ShanheCampaignDefinition Fixture(ShanheCampaignDefinition production, IReadOnlyList<ShanheStoryEventDefinition> stories)
    {
        var battleIds = stories.SelectMany(story => story.Choices).Select(choice => choice.BattleNodeId).OfType<string>().ToHashSet();
        foreach (var battle in production.Nodes.Where(node => battleIds.Contains(node.Id)).ToArray())
            if (battle.EliteUpgradeNodeId is { } eliteId) battleIds.Add(eliteId);
        return new("fixture:story-events", "奇遇机制", ["classic:guan-yu"], ["event"],
            new ShanheNodeDefinition[]
            {
                new("event", "奇遇", ShanheNodeKind.Event, ["after"]) { StoryEvents = stories, ChapterNumber = 1 },
                new("after", "后续战斗", ShanheNodeKind.Battle, [], [new("after", "后续战斗", [new("classic:cao-cao")])])
            }.Concat(production.Nodes.Where(node => battleIds.Contains(node.Id)).Select(node => node with
            { NextNodeIds = node.EliteUpgradeNodeId is { } eliteId ? ["after", eliteId] : ["after"] }))
            // The fixture opens individual stories, not a partial chapter route.
            // Keep their real upgrade profiles and the full reward/school contract.
            .Select(node => node with { ChapterRouteId = null, ChapterRole = ShanheChapterNodeRole.None,
                ChapterProgressStep = 0, RequiredChapterProgress = 0 }).ToArray(),
            production.Rewards, StartingGold: 600, ExtraSkillSlots: 2) { Schools = production.Schools };
    }

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition) => ShanheCampaignSession.Create(definition,
        new(71, "classic:guan-yu") { UseCodexRewardPool = true, UnlockedRewardIds = [] }, StandardShanheCampaignCatalog.CreateBattleRegistry);
    private static void Cold(ShanheCampaignSession run)
    {
        var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(run.CreateCheckpoint())),
            run.Definition, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Need(JsonSerializer.Serialize(restored.Snapshot) == JsonSerializer.Serialize(run.Snapshot), "Cold recovery preserves the exact event, options, payment, companion and pending battle.");
    }
    private static void Accept(ShanheCampaignSession run, ShanheCampaignCommand command)
    { var result = run.Submit(command); Need(result.Accepted, result.Error ?? "Story command rejected."); }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
