using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheExpandedCompanionChecks
{
    public static void SourceOpeningPoolsAndRecruitmentEntriesStartNativeBattles()
    {
        var production = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var ordinary = production.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.OpeningCompanionRewardId);
        var elite = production.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.EliteOpeningCompanionRewardId);
        Need(ordinary.OpeningCompanionPool is { Count: 4 } && elite.OpeningCompanionPool is { Count: 4 } &&
            !ordinary.OpeningCompanionPool.Select(member => member.GeneralId)
                .Intersect(elite.OpeningCompanionPool.Select(member => member.GeneralId), StringComparer.Ordinal).Any() &&
            StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[1] == ordinary.Id &&
            StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[75] == elite.Id,
            "Both source Heguangmeng codex nodes bind their separate four-member opening pools.");

        var eventNodes = production.Nodes.Where(node => node.StoryEvents is not null).ToArray();
        var recruits = eventNodes.SelectMany(node => node.StoryEvents!).Where(story => story.Choices.Any(choice => choice.Companion is not null))
            .GroupBy(story => story.Id, StringComparer.Ordinal).Select(group => group.First()).ToArray();
        Need(recruits.Length >= 17, "Every recruitment panel documented in the guide has a live event entry.");
        foreach (var story in recruits)
        {
            var companions = story.Choices.Where(choice => choice.Companion is not null).ToArray();
            var decline = story.Choices.Single(choice => choice.Companion is null);
            Need(companions.Length == 2 && companions.Count(choice => choice.GoldCost == 0) == 1 &&
                companions.Count(choice => choice.GoldCost == 400) == 1 && decline.GoldCost == 0 &&
                decline.RewardId is { } rewardId && production.Rewards.Single(reward => reward.Id == rewardId).Gold == 200,
                "Recruitment offers a free ordinary member, a 400-copper elite and a 200-copper refusal: " + story.Id);
        }

        string[] chapterOneMembers = ["classic:qin-mi", "boundary:lu-su", "shanhe:companion-cao-ang-normal", StandardShanheCaoruiCompanionPackage.NormalId];
        foreach (var generalId in chapterOneMembers)
            Need(eventNodes.Any(node => node.ChapterNumber == 1 && HasMember(node, generalId)) &&
                eventNodes.Where(node => node.ChapterNumber > 1).All(node => !HasMember(node, generalId)),
                "The guide's first-chapter-only recruitment does not leak into later chapters: " + generalId);

        var members = ordinary.OpeningCompanionPool!.Concat(elite.OpeningCompanionPool!)
            .Concat(recruits.SelectMany(story => story.Choices).Select(choice => choice.Companion).OfType<ShanheCombatantDefinition>())
            .GroupBy(member => JsonSerializer.Serialize(member), StringComparer.Ordinal).Select(group => group.First()).ToList();
        var ordinal = 0;
        while (members.Count > 0)
        {
            // Cover every recipe in a handful of native starts rather than simulating individual matches.
            var allies = members.DistinctBy(member => member.GeneralId).Take(6).ToArray();
            foreach (var ally in allies) members.Remove(ally);
            var setup = new ShanheBattleSetup(71, ++ordinal, "source-companions", "classic:guan-yu", 4,
                0, 0, 0, 0, [], [], [], [new("classic:zhang-liao")], allies, new string('a', 64));
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
            foreach (var member in allies)
            {
                Need(registry.IsGeneralPlayable(member.GeneralId) && registry.Generals[member.GeneralId].BaseHp > 0,
                    "A published recruitment/opening recipe has a complete positive-health native body: " + member.GeneralId);
                foreach (var skill in registry.Generals[member.GeneralId].SkillIds.Concat(member.ExtraSkillIds ?? []))
                    Need(registry.Skills.TryGetValue(skill, out var skillDefinition) &&
                        skillDefinition.ImplementationStatus == SkillImplementationStatus.Complete,
                        "The companion's actual source and additional skills load as complete native content: " + skill);
            }
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            var started = game.Submit(new StartGameCommand());
            Need(started.Accepted, started.Error?.Message ?? "The source companion batch could not start.");
            var view = game.CreateSnapshot(0);
            Need(view.Players.Count == allies.Length + 2 && allies.Select((member, index) =>
                view.Players[index + 1].GeneralId == member.GeneralId && view.Players[index + 1].MaxHp > 0).All(value => value),
                "All ordinary and elite source recipes occupy real allied seats in a native battle.");
        }

        var representative = recruits.Single(story => story.Id == StandardShanheCampaignCatalog.QinmiRecruitmentStoryId);
        var definition = new ShanheCampaignDefinition("fixture:expanded-companions", "招募入口", ["classic:guan-yu"], ["event"],
            [new("event", "招募", ShanheNodeKind.Event, ["battle"]) { ChapterNumber = 1, StoryEvents = [representative] },
             new("battle", "下一场", ShanheNodeKind.Battle, [], [new("battle", "下一场", [new("classic:zhang-liao")])])],
            [production.Rewards.Single(reward => reward.Id == "story-decline-recruit")], StartingGold: 600);
        var run = ShanheCampaignSession.Create(definition, new(71, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        Accept(new MoveShanheNodeCommand("event", run.Revision));
        var paid = run.Snapshot.NodeChoices.Single(choice => choice.Companion is not null && choice.GoldCost == 400);
        Accept(new ChooseShanheNodeOptionCommand(paid.Id, null, run.Revision));
        Need(run.Snapshot.Gold == 200 && run.Snapshot.RecruitedCompanions.Count == 1 && run.Snapshot.ExtraSkillIds.Count == 0,
            "An expanded elite recruitment pays exactly once and occupies the companion roster rather than a player skill slot.");
        var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(run.CreateCheckpoint())),
            definition, StandardShanheCampaignCatalog.CreateBattleRegistry);
        Need(JsonSerializer.Serialize(restored.Snapshot) == JsonSerializer.Serialize(run.Snapshot),
            "Cold recovery preserves the chosen source member, the paid fee and the onward route.");
        var preview = restored.PreviewEncounter("battle");
        run = restored;
        Accept(new MoveShanheNodeCommand("battle", run.Revision));
        Need(JsonSerializer.Serialize(preview.Allies) == JsonSerializer.Serialize(run.PendingBattleSetup!.AllyRoster) &&
            run.PendingBattleSetup.AllyRoster.Single().GeneralId == paid.Companion!.GeneralId,
            "The cold-restored recruitment enters the same real battle roster that was previewed.");
        return;

        void Accept(ShanheCampaignCommand command)
        {
            var result = run.Submit(command);
            Need(result.Accepted, result.Error ?? "Expanded companion campaign command rejected.");
        }
        static bool HasMember(ShanheNodeDefinition node, string generalId) =>
            node.StoryEvents!.Any(story => story.Choices.Any(choice => choice.Companion?.GeneralId == generalId));
    }

    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
