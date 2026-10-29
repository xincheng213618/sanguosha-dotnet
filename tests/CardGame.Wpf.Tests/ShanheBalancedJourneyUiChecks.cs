using System.Reflection;
using System.Windows.Media.Imaging;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Persistence;
using CardGame.Wpf.ViewModels;

internal static class ShanheBalancedJourneyUiChecks
{
    public static void NewRunsOptInAndLegacyContinueUpgradesOnlyFutureRewards()
    {
        var definition = StandardShanheCampaignCatalog.CreateCodexDefinition(25, StandardShanheCampaignCatalog.Source2026EditionId);
        Program.Assert(definition.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.CurrentWolongMapRewardId)
                    is { Rarity: 2, CampaignEconomy.FavorOwnedSchoolRewards: true } &&
                StandardShanheCampaignCatalog.ShopMetadataByRewardId[StandardShanheCampaignCatalog.CurrentWolongMapRewardId].SourceRewardId == 2253 &&
                ShanheArt.Get(2253) is BitmapSource { PixelWidth: > 0 },
            "The real rare Wolong Map reward2253 resolves its original locally installed artwork through the ordinary Shanhe asset path.");
        var legacy = ShanheCampaignSession.Create(definition,
            new(340638884, "classic:guan-yu") { SourceDifficultyId = 25, UseCodexRewardPool = true, PersistentMarket = true },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        var progress = new MemoryShanheCampaignSaveStore();
        var original = legacy.CreateCheckpoint();
        progress.Write(new(ShanheCampaignSaveFile.CurrentFormatVersion, DateTimeOffset.UnixEpoch, 25, original));
        using (var model = Create(progress))
        {
            model.ContinueShanheCommand.Execute(null);
            var loaded = Session(model);
            Program.Assert(loaded.HasBalancedJourneyRewards && loaded.Revision == original.Revision + 1 &&
                    !loaded.CreateCheckpoint().Options.UseBalancedJourneyRewards &&
                    loaded.AcceptedCommands.Last() is EnableShanheBalancedRewardsCommand &&
                    loaded.Snapshot.Gold == legacy.Snapshot.Gold && loaded.Snapshot.Hp == legacy.Snapshot.Hp &&
                    loaded.Snapshot.AvailableNodes.Select(node => node.Id).SequenceEqual(legacy.Snapshot.AvailableNodes.Select(node => node.Id)) &&
                    loaded.Snapshot.AvailableNodes.All(node => loaded.PreviewBattleNode(node.Id).PrimaryTacticRarity >= 2),
                "Continuing the old source25 map appends one policy command, preserves its wallet, health and destinations, and upgrades future actual tactic profiles.");
            Program.Assert(ShanheCampaignJson.Serialize(progress.Read().Campaign) == ShanheCampaignJson.Serialize(loaded.CreateCheckpoint()),
                "The normal save store persists the journaled upgrade so another process replays the same rules.");
        }
        var upgraded = ShanheCampaignJson.Serialize(progress.Read().Campaign);
        using (var cold = Create(progress))
        {
            cold.ContinueShanheCommand.Execute(null);
            Program.Assert(Session(cold).HasBalancedJourneyRewards &&
                    ShanheCampaignJson.Serialize(Session(cold).CreateCheckpoint()) == upgraded &&
                    ShanheCampaignJson.Serialize(progress.Read().Campaign) == upgraded,
                "A second cold continue does not append another enable command or rewrite the saved journal.");
        }

        // This unfinished legacy entry must keep its original profile and battle
        // seed until that battle and its frozen reward offer have resolved.
        var entered = legacy.Submit(new MoveShanheNodeCommand(legacy.Snapshot.AvailableNodes[0].Id, legacy.Revision));
        Program.Assert(entered.Accepted, "The old expedition enters its genuine ordinary source encounter.");
        var oldSetup = legacy.PendingBattleSetup!;
        var battleProgress = new MemoryShanheCampaignSaveStore();
        var battleCheckpoint = legacy.CreateCheckpoint();
        battleProgress.Write(new(ShanheCampaignSaveFile.CurrentFormatVersion, DateTimeOffset.UnixEpoch, 25, battleCheckpoint));
        var savedBattle = ShanheCampaignJson.Serialize(battleProgress.Read().Campaign);
        using (var retry = Create(battleProgress))
        {
            retry.ContinueShanheCommand.Execute(null);
            var loaded = Session(retry);
            Program.Assert(!loaded.HasBalancedJourneyRewards && retry.IsShanheBattleRetryReady &&
                    !retry.IsShanheBattleSession && loaded.PendingBattleSetup is { } setup &&
                    ShanheBattleSetup.ValueEquals(setup, oldSetup) &&
                    ShanheCampaignJson.Serialize(loaded.CreateCheckpoint()) == savedBattle &&
                    ShanheCampaignJson.Serialize(battleProgress.Read().Campaign) == savedBattle,
                "Continuing inside an old battle defers policy migration, retains its exact setup, and leaves the persisted source untouched.");
        }

        using var fresh = Create(new MemoryShanheCampaignSaveStore());
        fresh.SelectedShanheScenario = fresh.ShanheScenarios.Single(option => option.Id == StandardShanheCampaignCatalog.Source2026EditionId);
        fresh.SelectedShanheDifficulty = fresh.ShanheDifficulties.Single(option => option.Value == 25);
        fresh.StartShanheCommand.Execute(null);
        var started = Session(fresh);
        Program.Assert(started.HasBalancedJourneyRewards && started.CreateCheckpoint().Options.UseBalancedJourneyRewards &&
                started.AcceptedCommands.All(command => command is not EnableShanheBalancedRewardsCommand),
            "The actual new-expedition command opts into the corrected pool directly and needs no migration command.");
    }

    private static MainViewModel Create(MemoryShanheCampaignSaveStore progress) =>
        new(false, 71, true, new MemorySaveStore(), useExpandedContent: true,
            shanheSaveStore: progress, shanheCodexStore: new MemoryShanheCodexStore());

    private static ShanheCampaignSession Session(MainViewModel model) =>
        (ShanheCampaignSession)(typeof(MainViewModel).GetField("_shanheSession", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(model) ?? throw new InvalidOperationException("The actual command did not create or continue an expedition: " + model.ShanheStatusText));
}
