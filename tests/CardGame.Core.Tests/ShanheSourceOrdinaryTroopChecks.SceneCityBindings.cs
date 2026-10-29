using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static partial class ShanheSourceOrdinaryTroopChecks
{
    public static void SceneCityBindingsKeepReferenceGeographyAndEffectiveBattlesConsistentCold()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(16);
        // Reuse the bounded native source-route driver and genuine complete city
        // recipes. All original entrances keep the original graph reachable.
        var definition = source with
        {
            RandomStartingNode = false,
            StartingNodeIds = source.Nodes.Where(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle).Select(node => node.Id).ToArray(),
            StartingBonuses = new(MaxHp: 30),
            Rewards = source.Rewards.Append(new ShanheRewardDefinition(ShanheSource2026Checks.DriverReward, "路线检查")
                { TacticSkillId = ShanheSource2026Checks.Driver }).ToArray()
        };
        var options = new ShanheCampaignOptions(71, "classic:cao-cao")
        {
            SourceDifficultyId = 16, UseCodexRewardPool = true, PreferSourceOrdinaryEncounters = true,
            UseSourceSceneCityBindings = true,
            EquippedTacticIds = [ShanheSource2026Checks.DriverReward, StandardShanheCampaignCatalog.CurrentDailyTravel2RewardId]
        };
        Need(ShanheChapterSceneLocation.NativeCityId(source.Nodes.First(node => node.ChapterRole == ShanheChapterNodeRole.Arena), 0, 0) is null,
            "The original standalone arena remains an arena portal rather than acquiring a fabricated city identity.");
        foreach (var region in new[] { 3, 14 })
        foreach (var selected in new[] { 0, 1 })
        {
            var session = ShanheCampaignSession.Create(definition, options, ShanheSource2026Checks.Registry);
            var firstCity = region == 3 ? 5032 : 5141;
            var secondCity = region == 3 ? 5033 : 5142;
            AssertPreview(session, City(region, 1), firstCity);
            AssertPreview(session, City(region, 2), secondCity);
            var opening = City(region, selected + 1);
            var selectedCity = selected == 0 ? firstCity : secondCity;
            Win(session, opening, selectedCity);
            foreach (var index in new[] { 1, 2, 3 })
            {
                Accept(session, new MoveShanheNodeCommand(Route(region, "story-" + index), session.Revision));
                ShanheSource2026Checks.CompleteChoices(session);
            }
            Need(session.ChapterProgress == 4, "Genuine opening victory and three original story choices unlock the fifth-step battle.");
            Win(session, City(region, 3), selectedCity);
            Accept(session, new MoveShanheNodeCommand(Route(region, "camp"), session.Revision));
            Accept(session, new ChooseShanheNodeOptionCommand("draw", null, session.Revision));
            Need(session.ChapterProgress == 6, "The real camp completes the same six mainline slots despite relocated enemy pools.");
            var optionalCity = region == 3 ? 5037 : 5144;
            var pursuitCities = region == 3 ? new[] { 5034, 5036 } : new[] { 5143, 5145 };
            AssertPreview(session, City(region, 4), optionalCity);
            AssertPreview(session, Route(region, "extra-battle-1"), pursuitCities[0]);
            AssertPreview(session, Route(region, "extra-battle-2"), pursuitCities[1]);
            // Across the two selected branches, enter the actual late ordinary
            // battle and each pursuit without paying for another long battle.
            var tail = selected == 0 ? City(region, 4) : Route(region, "extra-battle-" + (region == 3 ? 1 : 2));
            AssertMove(session, tail, selected == 0 ? optionalCity : pursuitCities[region == 3 ? 0 : 1]);
            Cold(session);
        }

        var historicalOptions = options with { UseSourceSceneCityBindings = false, EquippedTacticIds = [] };
        var historical = ShanheCampaignSession.Create(definition, historicalOptions, ShanheSource2026Checks.Registry);
        AssertMove(historical, City(3, 1), 5031);
        var oldJson = ShanheCampaignJson.Serialize(historical.CreateCheckpoint());
        Need(historical.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                !oldJson.Contains("UseSourceSceneCityBindings", StringComparison.Ordinal),
            "Existing source-preference saves omit the new default-false field and retain their original city pool.");
        var falseJson = oldJson.Replace("\"Options\":{", "\"Options\":{\"UseSourceSceneCityBindings\":false,", StringComparison.Ordinal);
        Need(falseJson != oldJson && falseJson.Contains("\"UseSourceSceneCityBindings\":false", StringComparison.Ordinal),
            "The compatibility fixture really supplies explicit false as well as genuinely missing JSON.");
        foreach (var json in new[] { oldJson, falseJson })
        {
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(json), definition, ShanheSource2026Checks.Registry);
            Need(!restored.CreateCheckpoint().Options.UseSourceSceneCityBindings && restored.CreateCheckpoint().Options.PreferSourceOrdinaryEncounters &&
                    ShanheBattleSetup.ValueEquals(restored.PendingBattleSetup, historical.PendingBattleSetup) &&
                    ShanheCampaignJson.Serialize(restored.CreateCheckpoint()) == oldJson,
                "Missing and false scene bindings replay the identical pre-existing preferred-source pending encounter without migration.");
        }

        var local = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var localOptions = new ShanheCampaignOptions(71, "classic:guan-yu");
        var localBaseline = ShanheCampaignSession.Create(local, localOptions, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var localEnabled = ShanheCampaignSession.Create(local, localOptions with { UseSourceSceneCityBindings = true }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var localNode = localBaseline.Snapshot.AvailableNodes.First(node => node.Kind == ShanheNodeKind.Battle).Id;
        var localPreview = localBaseline.PreviewEncounter(localNode);
        var enabledPreview = localEnabled.PreviewEncounter(localNode);
        Need(local.SourceDifficultyId is null && localPreview.Id == enabledPreview.Id && SameMembers(localPreview.Enemies, enabledPreview.Enemies) &&
                ShanheChapterSceneLocation.NativeCityId(local.Nodes.Single(node => node.Id == localNode), 0, 0) is null,
            "The source-only opt-in and pure location lookup remain inert in a genuine retained local campaign.");
        Accept(localBaseline, new MoveShanheNodeCommand(localNode, localBaseline.Revision));
        Accept(localEnabled, new MoveShanheNodeCommand(localNode, localEnabled.Revision));
        Need(ShanheBattleSetup.ValueEquals(localBaseline.PendingBattleSetup, localEnabled.PendingBattleSetup),
            "Enabling source scene bindings cannot change the actual pending setup of a retained local campaign.");

        var targetCity = City(3, 2);
        var wolfId = StandardShanheCampaignCatalog.CurrentWolfBattlesRewardId;
        var eliteId = definition.Nodes.Single(node => node.Id == City(3, 1)).EliteUpgradeNodeId!;
        var nullTerrain = definition with { Nodes = definition.Nodes.Select(node => node.Id == targetCity ? node with
            { Encounters = node.Encounters!.Select(encounter => encounter with { TerrainSkillId = null }).ToArray() }
            : node.Id == eliteId ? node with { Encounters = node.Encounters!.Select(encounter => encounter with
                { TerrainSkillId = StandardShanheTerrainPackage.CityId }).ToArray() } : node).ToArray() };
        var wolf = ShanheCampaignSession.Create(nullTerrain, options with { EquippedTacticIds = [wolfId] }, ShanheSource2026Checks.Registry);
        var wolfProfile = wolf.PreviewBattleNode(City(3, 1));
        var wolfPreview = wolf.PreviewEncounter(City(3, 1));
        Need(wolfProfile.Kind == ShanheNodeKind.EliteBattle && wolfProfile.Encounters!.All(encounter =>
                encounter.SourceBattleId is null && encounter.SourceCityId == 5032 && encounter.TerrainSkillId is null) &&
                wolfPreview.SourceCityId == 5032 && wolfPreview.TerrainSkillId is null &&
                wolfPreview.Enemies.Select(enemy => enemy.GeneralId).SequenceEqual(nullTerrain.Nodes.Single(node => node.Id == eliteId)
                    .Encounters!.Single().Enemies.Select(enemy => enemy.GeneralId)),
            "Wolf upgrades retain their elite bodies after repeated resolution, inheriting the displayed city's real null terrain instead of another profile's terrain.");
        Accept(wolf, new MoveShanheNodeCommand(City(3, 1), wolf.Revision));
        Need(wolf.PendingBattleSetup!.TerrainSkillId is null && SameMembers(wolf.PendingBattleSetup.EnemyRoster, wolfPreview.Enemies) &&
                wolf.Snapshot.CurrentBattleNode!.Kind == ShanheNodeKind.EliteBattle,
            "The effective Wolf setup enters the exact already-previewed elite roster and null terrain at the correct source city.");
        var wolfCold = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(wolf.CreateCheckpoint())), nullTerrain, ShanheSource2026Checks.Registry);
        Need(ShanheBattleSetup.ValueEquals(wolfCold.PendingBattleSetup, wolf.PendingBattleSetup), "The derived city and Wolf profile replay together without a new checkpoint sidecar.");

        void Win(ShanheCampaignSession session, string id, int city)
        {
            AssertMove(session, id, city);
            var battle = new ShanheSource2026Checks.NativeBattle(session.PendingBattleSetup!);
            battle.Finish();
            Need(session.CompleteBattle(battle.Game, session.Revision).Accepted, "Scene progress requires the existing independently verified native battle receipt.");
            ShanheSource2026Checks.CompleteChoices(session);
        }
        void AssertMove(ShanheCampaignSession session, string id, int city)
        {
            var preview = AssertPreview(session, id, city);
            Accept(session, new MoveShanheNodeCommand(id, session.Revision));
            var setup = session.PendingBattleSetup!;
            var actual = session.Snapshot.CurrentBattleNode!.Encounters!.Single(encounter => encounter.Id == preview.Id);
            Need(setup.NodeId == id && setup.TerrainSkillId == preview.TerrainSkillId && SameMembers(setup.EnemyRoster, preview.Enemies) &&
                    actual.SourceCityId == city && actual.SourceBattleId == preview.SourceBattleId,
                "Actual Move and its effective current profile retain the exact previewed source city, recipe, grown roster and terrain.");
        }
        ShanheEncounterDefinition AssertPreview(ShanheCampaignSession session, string id, int city)
        {
            var before = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var logical = session.Definition.Nodes.Single(node => node.Id == id);
            var profile = session.PreviewBattleNode(id);
            var target = session.Definition.Nodes.Single(node => node.Id == "source-city-" + city);
            var expected = logical.ExtraChapterBattleIndex > 0 ? target.Encounters!.Select(encounter => encounter with
                { Id = id + ":" + encounter.Id, SourceBattleId = null }).ToArray() : target.Encounters!;
            Need(profile.Id == logical.Id && profile.ChapterRole == logical.ChapterRole && profile.ChapterProgressStep == logical.ChapterProgressStep &&
                    profile.RewardChoiceCount == logical.RewardChoiceCount && (profile.RewardIds ?? []).SequenceEqual(logical.RewardIds ?? []) &&
                    JsonSerializer.Serialize(profile.Encounters) == JsonSerializer.Serialize(expected),
                "The displayed city's full authored candidates and terrain move together while the original logical route and reward contract stay unchanged.");
            var preview = session.PreviewEncounter(id);
            var repeated = session.PreviewEncounter(id);
            Need(preview.SourceCityId == city && expected.Any(encounter => encounter.Id == preview.Id && encounter.TerrainSkillId == preview.TerrainSkillId) &&
                    (logical.ExtraChapterBattleIndex == 0 || preview.SourceBattleId is null) && preview.Id == repeated.Id &&
                    SameMembers(preview.Enemies, repeated.Enemies) && ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == before && session.PendingBattleSetup is null,
                "Repeated source scene preview reads the correct city's existing recipe without claiming a source battle for a local pursuit or advancing the journal.");
            return preview;
        }
        void Cold(ShanheCampaignSession session)
        {
            var json = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(json), definition, ShanheSource2026Checks.Registry);
            Need(json.Contains("\"UseSourceSceneCityBindings\":true", StringComparison.Ordinal) && restored.CreateCheckpoint().Options.UseSourceSceneCityBindings &&
                    ShanheBattleSetup.ValueEquals(restored.PendingBattleSetup, session.PendingBattleSetup) && ShanheCampaignJson.Serialize(restored.CreateCheckpoint()) == json,
                "The new opt-in and dynamically selected opening landmark survive real command replay with the same pending battle.");
        }
        static string City(int region, int ordinal) => StandardShanheCampaignCatalog.Source2026CityNodeId(5000 + region * 10 + ordinal);
        static string Route(int region, string suffix) => "source-region-" + region + ":" + suffix;
        static void Accept(ShanheCampaignSession session, ShanheCampaignCommand command)
        { var result = session.Submit(command); Need(result.Accepted, result.Error ?? "The original source route command was rejected."); }
    }
}
