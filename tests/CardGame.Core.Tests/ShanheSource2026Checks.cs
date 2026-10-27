using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSource2026Checks
{
    private const string Driver = "fixture:source-route-driver", DriverReward = "fixture:source-route-reward";

    public static void SourceBranchesKnownStagesAndLocalBodiesKeepTheirIdentities()
    {
        foreach (var difficulty in new[] { 1, 2, 3, 6, 11, 16, 25 })
        {
            var definition = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            Need(StandardShanheCampaignCatalog.IsSource2026Campaign(definition.Id, difficulty) &&
                definition.SourceDifficultyId == difficulty && definition.SourceEnemyGrowthHash == StandardShanheSourceEnemyGrowth.Season5.ContentHash,
                "Every source recipe binds its real selected tier and immutable growth table.");
            var graph = StandardShanheSourceRouteCatalog.Season5;
            var expectedRegions = graph.Regions.Where(region => region.MinimumSourceDifficultyId <= difficulty).ToArray();
            Need(definition.Nodes.Count == expectedRegions.Length * 12 && definition.StartingNodeIds.SequenceEqual(["source-region-2:market", "source-region-4:market"]),
                "Source region gates and the two random first-chapter entries survive itinerary compression.");
            foreach (var region in expectedRegions)
            {
                var exit = definition.Nodes.Single(node => node.Id == "source-region-" + region.SourceUniqueId + ":story");
                var expected = region.ChapterNumber == 3 ? new[] { "source-region-" + region.SourceUniqueId + ":camp" } :
                    region.NextRegionIds.Where(next => graph.ByRegionId[next].MinimumSourceDifficultyId <= difficulty)
                        .Select(next => "source-region-" + next + ":market").ToArray();
                Need(exit.NextNodeIds.SequenceEqual(expected), "Only the original available next-region edges may leave a region.");
                string[] firstChapterOnly = ["story:zhijinanpeng", "story:recruit-qinmi", "story:recruit-lusu", "story:recruit-cao-ang"];
                Need(exit.StoryEvents is { Count: > 0 } && firstChapterOnly.All(id =>
                    exit.StoryEvents.Any(story => story.Id == id) == (region.ChapterNumber == 1)),
                    "All three source chapters have a real story pool; Cao Ang, Qin Mi, Lu Su and Cao Rui remain restricted to chapter one.");
            }
            var all = definition.Nodes.SelectMany(node => node.Encounters ?? []).ToArray();
            Need(all.Where(encounter => encounter.Id.StartsWith("source-local:", StringComparison.Ordinal)).All(encounter =>
                    encounter.SourceBattleId is null && encounter.SourceCityId is > 0) &&
                all.All(encounter => difficulty >= 11 || encounter.TerrainSkillId is null),
                "Local troop bodies carry their genuinely chosen city but never an invented source battle, and terrain keeps its separate tier gate.");
            var fengxian = all.Single(encounter => encounter.SourceBattleId == 2644);
            var rageId = difficulty >= 21 ? StandardShanheFengxianBossPackage.SourceRageHellId :
                difficulty >= 11 ? StandardShanheFengxianBossPackage.SourceRageHardId : StandardShanheFengxianBossPackage.RageId;
            var rageEquipment = new List<ShanheStartingCard> { new(CardKind.WushuangHalberd, Suit.Heart, 12) };
            if (difficulty >= 6) rageEquipment.Add(new(CardKind.PurpleGoldCrown, Suit.Heart, 1));
            if (difficulty >= 11) rageEquipment.Add(new(CardKind.RedCottonRobe, Suit.Diamond, 1));
            Need((fengxian.BossTransition is not null) == (difficulty >= 3) &&
                (difficulty < 3 || fengxian.BossTransition is { MaxHp: 5, Hp: 5,
                    Combatant.SourceDifficultyAttributes: not null } &&
                    fengxian.BossTransition.StartingEquipment!.SequenceEqual(rageEquipment) &&
                    fengxian.BossTransition.GeneralId == rageId && fengxian.BossTransition.Combatant.GeneralId == rageId),
                "Fengxian's real known second form begins only at ordinary three, with its own base data, verified halberd and cumulative tier equipment.");
            Need(all.Where(encounter => encounter.SourceBattleId == 2397).Count() == 7 &&
                all.Where(encounter => encounter.SourceBattleId == 2397).All(encounter => encounter.SourceVisibleEnemyCount == 1 && encounter.Enemies.Count == 5),
                "Every genuinely selectable Luoyang ordinary city may contain Mulu; four hidden source minions do not inflate the visible-enemy multiplier.");
            if (difficulty >= 16)
                Need(all.Count(encounter => encounter.SourceBattleId is >= 2632 and <= 2687) == 50 &&
                    all.All(encounter => encounter.SourceBattleId is not (2635 or 2655 or 2669 or 2675 or 2684 or 2687)) &&
                    all.Count(encounter => encounter.BossTransition is not null) == 5,
                    "The high-tier route exposes all fourteen regions with fifty supported base groups and five known playable stage transitions.");
        }
        Need(!StandardShanheCampaignCatalog.IsSource2026Campaign("shanhe:zhuhou-source-2026-11", 1) &&
            !StandardShanheCampaignCatalog.IsSource2026Campaign("shanhe:zhuhou-1", 11) &&
            !StandardShanheCampaignCatalog.IsSource2026Campaign("shanhe:zhuhou-source-2026-26", 26),
            "An arbitrary campaign ID or local tier cannot be relabelled as an executable source difficulty.");
    }

    public static void ThreeSourceChaptersNativeStagesCityReceiptsAndColdRecovery()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(11);
        // Only selection is fixed: each original region edge, native roster, source growth and stage remains executable.
        var selectedBosses = new Dictionary<string, int> { ["source-city-5048"] = 2644, ["source-city-5088"] = 2660, ["source-city-5108"] = 2671 };
        var definition = ShanheCampaignContent.FreezeAndValidate(source with
        {
            RandomStartingNode = false, StartingBonuses = new(MaxHp: 30),
            Rewards = source.Rewards.Append(new ShanheRewardDefinition(DriverReward, "路线检查") { TacticSkillId = Driver }).ToArray(),
            Nodes = source.Nodes.Select(node => selectedBosses.TryGetValue(node.Id, out var sourceBattle)
                ? node with { Encounters = node.Encounters!.Where(encounter => encounter.SourceBattleId == sourceBattle).ToArray() }
                : node.Kind == ShanheNodeKind.Battle ? node with { Encounters = node.Encounters!.Where(encounter => encounter.SourceBattleId is null).ToArray() } : node).ToArray()
        });
        var expeditionId = Guid.Parse("bc930b3d-e35d-4300-b3c0-34ea4daaf305").ToString("N");
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        { ExpeditionId = expeditionId, SourceDifficultyId = 11, UseCodexRewardPool = true, EquippedTacticIds = [DriverReward] }, Registry);
        string[] path = ["source-region-4:market", "source-city-5041", "source-region-4:camp", "source-city-5048", "source-region-4:story",
            "source-region-8:market", "source-city-5081", "source-region-8:camp", "source-city-5088", "source-region-8:story",
            "source-region-10:market", "source-city-5107", "source-region-10:story", "source-region-10:camp", "source-city-5108"];
        var stages = 0;
        foreach (var id in path)
        {
            Need(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Any(node => node.Id == id),
                "A real source route must offer its next selected city: " + id);
            Accept(session, new MoveShanheNodeCommand(id, session.Revision));
            if (session.PendingBattleSetup is { } setup)
            {
                Need(setup.SourceDifficultyId == 11 && setup.EncounterOrdinal == session.Snapshot.CompletedBattles.Count + 1,
                    "The frozen battle carries the selected source tier and actual monotonically increasing battle day.");
                var scene = new NativeBattle(setup);
                scene.Finish(); stages += scene.StageCount;
                Need(session.CompleteBattle(scene.Game, session.Revision).Accepted,
                    "A completed native source battle must pass the campaign's independent replay receipt verification.");
            }
            CompleteChoices(session);
            var checkpoint = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(checkpoint), definition, Registry);
            Need(JsonSerializer.Serialize(session.Snapshot) == JsonSerializer.Serialize(restored.Snapshot),
                "Cold campaign replay preserves the selected source route, growth and accepted battle identities.");
            session = restored;
        }
        var snapshot = session.Snapshot;
        Need(snapshot.Phase == ShanheCampaignPhase.Won && snapshot.CompletedBattles.Count == 6 && stages == 2 &&
            snapshot.CompletedBattles.All(battle => battle.Won) && snapshot.CompletedBattles.Count(battle => battle.SourceCityId == 5107) == 1 &&
            snapshot.CompletedBattles.Single(battle => battle.SourceBattleId == 2644).SourceCityId == 5048 &&
            snapshot.PlayerGeneralId == "classic:cao-cao",
            "A genuine six-battle expedition wins three original chapters including two full native stage battles and the selected carbon-ruins city.");
        var progress = ShanheGuideTaskProgress.Advance(ShanheGuideTaskProgressState.Empty, "expedition:" + expeditionId, 11,
            snapshot.PlayerGeneralId, true, snapshot.CompletedBattles, Enumerable.Range(1, 25));
        Need(progress.Count(200357) == 1 && progress.Count(200352) == 1 && progress.Count(200358) == 1 && progress.Count(200347) == 0 &&
            ShanheGuideTaskCatalog.Tasks.Where(task => task.RequiresScore).All(task => progress.Count(task.SourceId) == 0),
            "Native accepted receipts open the correct Cao Cao, full Fengxian and actual city tasks while keeping score tasks deferred.");
    }

    private static void CompleteChoices(ShanheCampaignSession session)
    {
        for (var step = 0; step < 30; step++)
        {
            var snapshot = session.Snapshot;
            if (snapshot.Phase is ShanheCampaignPhase.Map or ShanheCampaignPhase.Won) return;
            if (snapshot.Phase == ShanheCampaignPhase.NodeChoice)
            {
                var node = session.Definition.Nodes.Single(node => node.Id == snapshot.CurrentNodeId);
                var choice = node.Kind == ShanheNodeKind.Shop ? "$leave" : snapshot.NodeChoices
                    .Where(choice => choice.Companion is null && choice.BattleNodeId is null && choice.HpCost == 0 && choice.GoldCost == 0)
                    .OrderBy(choice => choice.RewardId is null ? 0 : 1).First().Id;
                Accept(session, new ChooseShanheNodeOptionCommand(choice, null, session.Revision));
            }
            else if (snapshot.BossRewardSummary is { } summary)
            {
                var group = summary.Groups.FirstOrDefault(group => !group.IsResolved);
                if (group is null) Accept(session, new ConfirmShanheBossRewardCommand(session.Revision));
                else if (group.CanSkip) Accept(session, new SkipShanheRewardCommand(session.Revision) { GroupId = group.Id });
                else Accept(session, new ChooseShanheBossRewardCommand(group.Id, group.Offers[0].Id, null, session.Revision));
            }
            else if (snapshot.CanSkipReward) Accept(session, new SkipShanheRewardCommand(session.Revision));
            else if (snapshot.RewardCategories.Count > 0) Accept(session, new ChooseShanheRewardCategoryCommand(snapshot.RewardCategories[0], session.Revision));
            else if (snapshot.RewardOffers.Count > 0) Accept(session, new ChooseShanheRewardCommand(snapshot.RewardOffers[0].Id, null, session.Revision));
            else throw new InvalidOperationException("Unexpected source campaign boundary: " + JsonSerializer.Serialize(snapshot));
        }
        throw new InvalidOperationException("A source chapter's bounded native reward/choice sequence did not finish.");
    }

    private static ContentRegistry Registry(ShanheBattleSetup setup) => StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
    { TacticSkillIds = setup.TacticSkillIds.Where(id => id != Driver).ToArray() }).WithExtension(new RouteDriverPackage());

    private static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
    { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "Source campaign command rejected."); }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class NativeBattle
    {
        private readonly ContentRegistry _registry;
        public GameEngine Game { get; private set; }
        public int StageCount => Game.Events.Count(envelope => envelope.Payload is ShanheBossStageStartedEvent);
        public NativeBattle(ShanheBattleSetup setup)
        {
            _registry = Registry(setup);
            Game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count,
                HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 20 }, _registry);
            Submit(new StartGameCommand());
        }
        public void Finish()
        {
            var coldStageCount = 0;
            for (var step = 0; step < 650; step++)
            {
                if (StageCount > coldStageCount)
                {
                    var before = SnapshotJson.Serialize(Game.CreateSnapshot(0));
                    Game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(Game.CreateCheckpoint())), _registry);
                    Need(before == SnapshotJson.Serialize(Game.CreateSnapshot(0)), "Native source second-form state survives exact command cold replay.");
                    coldStageCount = StageCount;
                }
                if (Game.State.Status == EngineStatus.Completed && Game.ResolutionStack.Count == 0) return;
                var prompt = Game.PendingDecision;
                if (prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && Game.ResolutionStack.Count == 0)
                {
                    var enemy = Game.CreateSnapshot(0).Players.First(player => player.Seat != 0 && player.IsAlive && player.TeamId == ShanheBattleSetup.EnemyTeamId);
                    Submit(new UseProgramSkillCommand(0, Driver, "finish", [], [enemy.Seat], Game.Revision, prompt.PromptId));
                }
                else if (prompt is { PlayerSeat: 0, Kind: DecisionKind.DiscardCards })
                    Submit(new DiscardCardsCommand(0, prompt.ValidCardIds.Take(prompt.RequiredCardCount).ToArray(), prompt.PromptId, Game.Revision));
                else if (prompt is { PlayerSeat: 0, Choices.Count: > 0 })
                {
                    var choice = prompt.Choices.FirstOrDefault(choice => choice.Cards.Count == 0 &&
                        choice.Parameters.GetValueOrDefault("response") is "pass" or "take-damage" or "let-die") ??
                        prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("decision") == "skip") ?? prompt.Choices[0];
                    Submit(new AnswerPromptCommand(0, prompt.PromptId, choice.Id, Game.Revision));
                }
                else Submit(new AdvanceOneStepCommand(Game.Revision));
            }
            throw new InvalidOperationException("The bounded native source battle did not finish: " + JsonSerializer.Serialize(Game.PendingDecision));
        }
        private void Submit(GameCommand command)
        { var result = Game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Native source battle command rejected."); }
    }

    private sealed class RouteDriverPackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-route-package", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,"activations":[{"id":"finish","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                    "effects":[{"op":"damage","target":"selectedTarget","amount":20}]}]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "路线检查", description = "Bounded native route driver" } } }));
            builder.AddSkill(new(Driver, "路线检查", "Bounded native route driver")
            { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
        }
    }
}
