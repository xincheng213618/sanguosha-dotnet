using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Controls;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static partial class ShanheChapterMapLayoutUiChecks
{
    private static void VerifyNativeSceneCities(string output)
    {
        const BindingFlags hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        var save = new MemoryShanheCampaignSaveStore();
        using var model = new MainViewModel(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: save, shanheCodexStore: new MemoryShanheCodexStore());
        model.SelectedShanheScenario = model.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        using var host = new NativeResponseStatusChecks.NativeHost(model);
        host.Root.Width = 1120; host.Root.Height = 780;
        host.Window.SizeToContent = SizeToContent.WidthAndHeight;
        // Hangu and Xiaohan first appear in published source difficulty 16.
        // Keep the common stage fixture at that actual supported difficulty.
        var published = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(16);
        foreach (var region in new[] { 3, 4, 14 })
        {
            var definition = RegionFixture(published, region);
            var openings = definition.Nodes.Where(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle).ToArray();
            foreach (var chosen in new[] { 0, 1 })
            {
                var session = Create(definition);
                var main = definition.Nodes.Single(node => node.ChapterRole == ShanheChapterNodeRole.MainBattle);
                var steps = new[] { openings[chosen] }.Concat(definition.Nodes.Where(node =>
                    node.ChapterRole is ShanheChapterNodeRole.Event or ShanheChapterNodeRole.MainBattle or ShanheChapterNodeRole.Rest)
                    .OrderBy(node => node.ChapterProgressStep)).ToArray();
                foreach (var stage in new[] { 0, 1, 4, 5, 6 })
                {
                    if (stage > 0) SetUiMapStage(session, steps.Take(stage).Select(node => node.Id).ToArray());
                    Attach(session);
                    Inspect(session, region, chosen, stage);
                    if (stage == 4)
                    {
                        var city = model.ShanheMapNodes.Single(node => node.Node.Id == main.Id);
                        Program.Assert(city.IsAvailable && city.NativeLandmark!.CityId ==
                                (region == 3 ? 5032 + chosen : 5000 + region * 10 + 1 + chosen),
                            "The real main-battle marker reuses the selected opening's city on both opening branches.");
                    }
                }
                var pursuit = definition.Nodes.Single(node => node.ExtraChapterBattleIndex == 1);
                SetUiMapStage(session, steps.Select(node => node.Id).Append(pursuit.Id).ToArray());
                Attach(session);
                Inspect(session, region, chosen, 6);
                var secondPursuit = definition.Nodes.Single(node => node.ExtraChapterBattleIndex == 2).Id;
                Program.Assert(model.ShanheMapNodes.Single(node => node.Node.Id == secondPursuit).IsAvailable &&
                        model.ShanheChapterMainProgress == 6,
                    "After the first pursuit's UI stage, the second real pursuit keeps its city and remains outside the six mainline steps.");
                if (chosen == 1)
                    Program.Render(host.Root, 1120, 780, Path.Combine(output, $"shanhe-native-scene-city-region-{region}.png"));
            }
        }

        // One real entry checks the preview/setup boundary with terrain enabled.
        // Its preceding stages remain the explicitly seeded UI fixture below.
        var terrainDefinition = RegionFixture(published, 3);
        var terrainSession = Create(terrainDefinition);
        var selected = terrainDefinition.Nodes.Where(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle).Last();
        SetUiMapStage(terrainSession, new[] { selected.Id }.Concat(terrainDefinition.Nodes.Where(node =>
            node.ChapterRole == ShanheChapterNodeRole.Event).OrderBy(node => node.ChapterProgressStep).Select(node => node.Id)).ToArray());
        Attach(terrainSession);
        Inspect(terrainSession, 3, 1, 4);
        var targetId = terrainDefinition.Nodes.Single(node => node.ChapterRole == ShanheChapterNodeRole.MainBattle).Id;
        var target = model.ShanheMapNodes.Single(node => node.Node.Id == targetId);
        var preview = terrainSession.PreviewEncounter(target.Node.Id);
        var moved = terrainSession.Submit(new MoveShanheNodeCommand(target.Node.Id, terrainSession.Revision));
        var setup = terrainSession.PendingBattleSetup;
        var entered = terrainSession.Snapshot.CurrentBattleNode!.Encounters!.Single(encounter => encounter.Id == preview.Id);
        Program.Assert(moved.Accepted && setup is not null && setup.NodeId == target.Node.Id &&
                entered.SourceCityId == preview.SourceCityId && entered.SourceBattleId == preview.SourceBattleId &&
                preview.SourceCityId == target.NativeLandmark!.CityId && preview.TerrainSkillId is not null &&
                setup.TerrainSkillId == preview.TerrainSkillId &&
                JsonSerializer.Serialize(setup.EnemyRoster) == JsonSerializer.Serialize(preview.Enemies),
            "The actual Move at the reused source16 main-battle city locks the preview's source encounter, complete grown enemy roster and real target-city terrain.");

        ShanheCampaignSession Create(ShanheCampaignDefinition definition) => ShanheCampaignSession.Create(definition,
            new(71, model.SelectedShanheGeneral!.Id)
            {
                SourceDifficultyId = definition.SourceDifficultyId, UseCodexRewardPool = true,
                PreferSourceOrdinaryEncounters = true, UseSourceSceneCityBindings = true,
                EquippedTacticIds = [StandardShanheCampaignCatalog.CurrentDailyTravel2RewardId],
                UnlockedRewardIds = [StandardShanheCampaignCatalog.CurrentDailyTravel2RewardId]
            }, StandardShanheCampaignCatalog.CreateBattleRegistry);

        void Attach(ShanheCampaignSession session)
        {
            typeof(MainViewModel).GetField("_shanheSession", hidden)!.SetValue(model, session);
            typeof(MainViewModel).GetField("_shanheRunDifficulty", hidden)!.SetValue(model, session.Definition.SourceDifficultyId!.Value);
            model.OpenShanheCommand.Execute(null);
            Program.Render(host.Root, 1120, 780, null);
            host.Root.Dispatcher.Invoke(() => { }, DispatcherPriority.DataBind);
            host.Root.UpdateLayout();
        }

        void Inspect(ShanheCampaignSession session, int region, int chosen, int stage)
        {
            var journal = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var snapshot = JsonSerializer.Serialize(session.Snapshot);
            var ordinary = Program.Engine(model);
            var game = GameCheckpointJson.Serialize(ordinary.CreateCheckpoint());
            var map = Program.Find<ShanheJourneyMap>(host.Root).Single(control => control.IsVisible);
            var nodes = model.ShanheMapNodes.Where(node => node.IsAvailable && node.IsBattle).ToArray();
            Program.Assert(nodes.Length > 0 && model.ShanheChapterMainProgress == stage && !model.IsShanheEncounterPreviewOpen,
                $"Region {region}, opening {chosen}, stage {stage} uses its available native battle markers before any selection.");
            if (stage == 6)
                Program.Assert(session.Snapshot.AvailableNodes.Where(node => node.ChapterRole is
                            ShanheChapterNodeRole.OptionalBattle or ShanheChapterNodeRole.ExtraBattle)
                        .All(available => nodes.Any(node => node.Node.Id == available.Id)),
                    "Every actual unlocked optional battle and pursuit is represented by its available native city sprite.");
            foreach (var node in nodes)
            {
                var art = node.NativeLandmark ?? throw new InvalidOperationException(
                    "The actual available native sprite must retain its source landmark.");
                var button = Program.Find<ShanheMapNodeButton>(map).Single(button => ReferenceEquals(button.DataContext, node));
                var image = Program.Find<Image>(button).Single(image => image.Name == "NativeLandmarkImage");
                var marker = (Image)button.Template.FindName("BattleMarker", button);
                var expected = session.PreviewEncounter(node.Node.Id);
                Program.Assert(image.Source is BitmapSource { PixelWidth: > 0, PixelHeight: > 0 } &&
                        ReferenceEquals(image.Source, node.NativeArtwork) && image.ActualWidth > 0 && image.ActualHeight > 0 &&
                        button.IsVisible && button.IsEnabled && ReferenceEquals(button.Command, model.PreviewShanheNodeCommand) &&
                        ReferenceEquals(button.CommandParameter, node.Node) && button.Command!.CanExecute(button.CommandParameter),
                    "The actual available native sprite retains its real executable preview binding.");
                var authored = session.Definition.Nodes.Single(candidate => candidate.Id == node.Node.Id);
                if (authored.ChapterRole == ShanheChapterNodeRole.Arena)
                    Program.Assert(node.SceneLandmark == "Arena" && art.CityId == 0 &&
                            marker.Visibility == Visibility.Collapsed && authored.Encounters!.Any(encounter =>
                                encounter.SourceCityId == expected.SourceCityId && encounter.SourceCityId is not null),
                        "The arena remains a portal with no geographical city ID; its unchanged authored arena recipe is the explicit exception to sprite-city equality.");
                else
                    Program.Assert(expected.SourceCityId == art.CityId &&
                            (node.IsBoss ? marker.Visibility == Visibility.Collapsed : marker.Visibility == Visibility.Visible),
                        $"Region {region}, opening {chosen}, stage {stage}, {node.Node.Id}: sprite city {art.CityId} must match prepared encounter city {expected.SourceCityId}.");
                button.Command!.Execute(button.CommandParameter);
                var actual = (ShanheEncounterDefinition)typeof(MainViewModel).GetField("_shanhePreviewEncounter", hidden)!.GetValue(model)!;
                Program.Assert(model.IsShanheEncounterPreviewOpen && JsonSerializer.Serialize(actual) == JsonSerializer.Serialize(expected) &&
                        model.ShanheEncounterTitle == expected.Name && model.ChallengeShanheEncounterCommand.CanExecute(null),
                    "Selecting the actual map sprite shows the same native city, source identity, terrain and enemy recipe as the session's read-only preview.");
                model.CloseShanheEncounterPreviewCommand.Execute(null);
            }
            Program.Assert(!model.IsShanheEncounterPreviewOpen && session.PendingBattleSetup is null &&
                    ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == journal && JsonSerializer.Serialize(session.Snapshot) == snapshot &&
                    ReferenceEquals(ordinary, Program.Engine(model)) && GameCheckpointJson.Serialize(ordinary.CreateCheckpoint()) == game && !save.Exists,
                "Reading and closing every available city preview preserves the complete UI-stage snapshot, campaign journal, ordinary game and absent save.");
        }
    }

    private static ShanheCampaignDefinition RegionFixture(ShanheCampaignDefinition source, int region)
    {
        // Isolate an existing region graph for UI coverage; every authored enemy,
        // source identity, reward and terrain recipe remains unchanged.
        var nodes = source.Nodes.Where(node => node.ChapterRouteId == "source-region-" + region).ToArray();
        var ids = nodes.Select(node => node.Id).ToHashSet(StringComparer.Ordinal);
        return ShanheCampaignContent.FreezeAndValidate(source with
        {
            StartingNodeIds = nodes.Where(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle).Select(node => node.Id).ToArray(),
            Nodes = nodes.Select(node => node with { NextNodeIds = node.NextNodeIds.Where(ids.Contains).ToArray() }).ToArray()
        });
    }

    private static void SetUiMapStage(ShanheCampaignSession session, string[] completed)
    {
        // Detached UI-stage seam only: this is not a replayable battle journal or
        // evidence that a complete source campaign was played through.
        const BindingFlags hidden = BindingFlags.Instance | BindingFlags.NonPublic;
        var type = typeof(ShanheCampaignSession);
        var field = type.GetField("_state", hidden)!;
        var original = field.GetValue(session)!;
        var next = original.GetType().GetMethod("<Clone>$", BindingFlags.Instance | BindingFlags.Public)!.Invoke(original, null)!;
        var stateType = next.GetType();
        stateType.GetProperty("Phase")!.SetValue(next, ShanheCampaignPhase.Map);
        stateType.GetProperty("CompletedNodeIds")!.SetValue(next, Array.AsReadOnly(completed));
        stateType.GetProperty("CurrentNodeId")!.SetValue(next, completed.LastOrDefault());
        stateType.GetProperty("BattleSetup")!.SetValue(next, null);
        stateType.GetProperty("BattleProfileNodeId")!.SetValue(next, null);
        stateType.GetProperty("EncounterId")!.SetValue(next, null);
        var route = session.Definition.Nodes.First().ChapterRouteId!;
        var available = type.GetMethod("RouteAvailableNodes", hidden)!.Invoke(session, [next, route]);
        stateType.GetProperty("AvailableNodeIds")!.SetValue(next, available);
        var snapshot = type.GetMethod("CreateSnapshot", hidden)!.Invoke(session, [next, session.Revision]);
        field.SetValue(session, next);
        type.GetField("_snapshot", hidden)!.SetValue(session, snapshot);
    }
}
