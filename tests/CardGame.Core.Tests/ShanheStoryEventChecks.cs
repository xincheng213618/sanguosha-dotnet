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

    public static void OptionalBattlesPayVictoryCopperAndEliteQuality()
    {
        var production = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var story = production.Nodes.Single(node => node.Id == "village").StoryEvents!.Single(story => story.Id == "story:huangjin-elite");
        var definition = Fixture(production, [story]);
        var skipped = Create(definition);
        Accept(skipped, new MoveShanheNodeCommand("event", skipped.Revision));
        Accept(skipped, new ChooseShanheNodeOptionCommand("avoid-battle", null, skipped.Revision));
        Need(skipped.Snapshot.Gold == 720 && skipped.PendingBattleSetup is null, "Avoiding combat credits only the smaller 120 copper.");
        Cold(skipped);
        foreach (var elite in new[] { false, true })
        {
            var run = Create(definition);
            Accept(run, new MoveShanheNodeCommand("event", run.Revision));
            Accept(run, new ChooseShanheNodeOptionCommand(elite ? "fight-elite" : "fight-normal", null, run.Revision));
            Need(run.Snapshot.Phase == ShanheCampaignPhase.Battle && run.Snapshot.Gold == 600,
                "Choosing combat creates a real pending battle and pays no victory reward in advance.");
            Cold(run);
            var node = definition.Nodes.Single(node => node.Id == run.PendingBattleSetup!.NodeId);
            Need(node.Encounters!.Single().VictoryGold >= 200 && node.Kind == (elite ? ShanheNodeKind.EliteBattle : ShanheNodeKind.Battle) &&
                (!elite || node.RewardIds!.All(id => definition.Rewards.Single(reward => reward.Id == id).Rarity >= 2)),
                "Ordinary combat pays at least 200 and every eligible elite reward is rare or better.");
            // Use the existing small native victory driver; this checks real completion, not a supplied win flag.
            var small = definition with
            {
                PlayerGeneralIds = ["fixture:shanhe-player"],
                Nodes = definition.Nodes.Select(item => item.Encounters is not { Count: > 0 } ? item : item with
                { Encounters = [new("fixture", "真实奇遇战斗", [new("fixture:shanhe-enemy")], VictoryGold: elite ? 300 : 200)] }).ToArray(),
                Rewards = [new("loot", "精英战利品", StartingEquipment: [new(CardKind.BaguaFormation)]) { Rarity = 2 }]
            };
            // Keep only the exercised combat route and its authored skip reward in this mechanism fixture.
            small = small with { Rewards = [.. small.Rewards, new("story-avoid-battle", "避战", Gold: 120)],
                Nodes = small.Nodes.Select(item => item.IsStoryBattle ? item with { RewardIds = ["loot"], CategorizedRewards = false } : item).ToArray() };
            var native = ShanheCampaignSession.Create(small, new(71, "fixture:shanhe-player"), ShanheCampaignChecks.Registry);
            Accept(native, new MoveShanheNodeCommand("event", native.Revision));
            Accept(native, new ChooseShanheNodeOptionCommand(elite ? "fight-elite" : "fight-normal", null, native.Revision));
            var battle = ShanheCampaignChecks.CreateBattle(native.PendingBattleSetup!);
            ShanheCampaignChecks.FinishBattle(battle, "defeat");
            Need(native.CompleteBattle(battle, native.Revision).Accepted && native.Snapshot.Gold == (elite ? 900 : 800) &&
                native.Snapshot.RewardOffers.Single().Rarity == 2, "Independent native replay credits victory copper and exposes the earned item.");
            Accept(native, new ChooseShanheRewardCommand("loot", null, native.Revision));
            Need(native.Snapshot.StartingEquipment.Count == 1 && native.Snapshot.AvailableNodes.Single().Id == "after",
                "Claiming the earned item returns to the event's original onward route.");
        }
    }

    public static void EliteDeathFireUsesNativeEnemyDamageAndColdReplay()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
            new StandardShanheRecruitmentPackage(), new DeathFixture());
        var setup = new ShanheBattleSetup(71, 1, "death", "fixture:story-driver", 20, 0, 0, 0, 0, [], [], [],
            [new("fixture:story-enemy-a"), new("fixture:story-enemy-b")],
            [new("fixture:story-ally", ExtraSkillIds: [StandardShanheRecruitmentPackage.HuihuoSkillId])], new string('a', 64));
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
            ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, AiPolicyVersion = AiDecisionPolicy.CurrentVersion
        }, registry);
        void Submit(GameCommand command) { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native death command rejected."); }
        Submit(new StartGameCommand());
        for (var step = 0; step < 40 && game.PendingDecision is not { PlayerSeat: 0, Kind: DecisionKind.PlayCard }; step++)
            Submit(new AdvanceOneStepCommand(game.Revision));
        Need(game.PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The real fixture reaches its ordinary play prompt.");
        Submit(new UseProgramSkillCommand(0, "fixture:story-driver-skill", "kill-ally", [], [1], game.Revision, game.PendingDecision!.PromptId));
        for (var step = 0; step < 100 && game.Events.Count(e => e.Payload is DamageAppliedEvent) < 2; step++)
        {
            if (game.PendingDecision is { PlayerSeat: 0, Choices.Count: > 0 } prompt)
            {
                if (prompt.Kind == DecisionKind.PlayCard) break;
                var pass = prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") is "pass" or "let-die") ?? prompt.Choices[0];
                Submit(new AnswerPromptCommand(0, prompt.PromptId, pass.Id, game.Revision));
            }
            else Submit(new AdvanceOneStepCommand(game.Revision));
            if (game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == StandardShanheRecruitmentPackage.HuihuoSkillId))
            {
                var checkpoint = game.CreateCheckpoint();
                game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(checkpoint)), registry);
            }
        }
        Need(!game.State.Players[1].IsAlive && game.Events.Select(e => e.Payload).OfType<DamageAppliedEvent>()
            .Where(e => e.SourceSeat == 1).Select(e => (e.TargetSeat, e.Amount, e.Nature))
            .SequenceEqual([(2, 3, DamageNature.Fire), (3, 3, DamageNature.Fire)]),
            "The dead elite deals exactly three real fire damage to each enemy.");
        for (var step = 0; step < 100 && game.State.Status != EngineStatus.Completed; step++)
            Submit(new AdvanceOneStepCommand(game.Revision));
        Need(game.State.Status == EngineStatus.Completed && game.ResolutionStack.Count == 0 &&
            game.CreateSnapshot(0).WinnerTeamId == ShanheBattleSetup.PlayerTeamId,
            "Both lethal segments finish their nested deaths and the original death before victory is published.");
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(restored.CreateSnapshot(0)) == SnapshotJson.Serialize(game.CreateSnapshot(0)),
            "Replaying the native death journal reproduces the same enemy damage without repeating a segment.");
    }

    private sealed class DeathFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-story-death", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"fixture:story-driver-skill","revision":1,"activations":[{"id":"kill-ally","minCards":0,"maxCards":0,
                   "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"loseHp","target":"selectedTarget","amount":10}]}]}
                ]}
                """, $$"""
                {"schemaVersion":{{SkillProgramCatalog.PresentationSchemaVersion}},"skills":{
                  "fixture:story-driver-skill":{"name":"真实死亡驱动","description":"令指定角色失去体力"}
                }
                }
                """);
            builder.AddSkill(new("fixture:story-driver-skill", "驱动", "真实死亡") { Program = catalog.Programs["fixture:story-driver-skill"] });
            builder.AddGeneral(new("fixture:story-driver", "行者", "", "fixture:story-driver-skill", "shu", 20));
            builder.AddGeneral(new("fixture:story-ally", "精英帮手", "", null, "qun", 3));
            builder.AddGeneral(new("fixture:story-enemy-a", "敌甲", "", null, "wei", 2));
            builder.AddGeneral(new("fixture:story-enemy-b", "敌乙", "", null, "wei", 2));
            builder.AddDeck(new("fixture:story-deck", "实体闪", 0, 0, [new("standard:dodge", 40)]));
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "回火", 4, 4, new Dictionary<string, int>(), "fixture:story-deck",
                GeneralPoolIds: ["fixture:story-driver", "fixture:story-ally", "fixture:story-enemy-a", "fixture:story-enemy-b"],
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    internal static ShanheCampaignDefinition Fixture(ShanheCampaignDefinition production, IReadOnlyList<ShanheStoryEventDefinition> stories)
    {
        var battleIds = stories.SelectMany(story => story.Choices).Select(choice => choice.BattleNodeId).OfType<string>().ToHashSet();
        return new("fixture:story-events", "奇遇机制", ["classic:guan-yu"], ["event"],
            new ShanheNodeDefinition[]
            {
                new("event", "奇遇", ShanheNodeKind.Event, ["after"]) { StoryEvents = stories, ChapterNumber = 1 },
                new("after", "后续战斗", ShanheNodeKind.Battle, [], [new("after", "后续战斗", [new("classic:cao-cao")])])
            }.Concat(production.Nodes.Where(node => battleIds.Contains(node.Id)).Select(node => node with { NextNodeIds = ["after"] })).ToArray(),
            production.Rewards, StartingGold: 600, ExtraSkillSlots: 2);
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
