using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheCycleLoadoutChecks
{
    public static void CurrentSixTacticLoadoutStartsWithRegisteredNativePrograms()
        => StartRegisteredLoadout(["tactic-build-2079", "tactic-build-2080", "tactic-build-2081",
            "tactic-build-2227", "tactic-build-2182", "tactic-economy-2016"], [65, 17], 5);

    public static void CurrentPassiveStockLoadoutStartsWithRegisteredNativePrograms()
        => StartRegisteredLoadout(["tactic-build-2132", "tactic-build-2133", "tactic-build-2135",
            "tactic-build-2194", "tactic-build-2167", "tactic-build-2308"], [46], 6);

    private static void StartRegisteredLoadout(string[] equipped, int[] researchNodes, int nativeCount)
    {
        var source = StandardShanheCampaignCatalog.CreateCodexDefinition(1, StandardShanheCampaignCatalog.Returning2026EditionId);
        var definition = source with
        {
            Id = "fixture:shanhe-cycle-loadout", StartingNodeIds = ["battle"],
            Nodes = [new("battle", "构筑试阵", ShanheNodeKind.Battle, [],
                [new("guard", "步卒", [new(StandardShanheLocalTroopPackage.FootmanId) { StartingHand = [] }])])]
        };
        var session = ShanheCampaignSession.Create(definition, new(71, "classic:cao-cao")
        {
            UseCodexRewardPool = true,
            UnlockedRewardIds = [.. StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(source),
                .. researchNodes.Select(node => StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[node])],
            EquippedTacticIds = equipped
        }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var moved = session.Submit(new MoveShanheNodeCommand("battle", session.Revision));
        Require(moved.Accepted, moved.Error ?? "The actual loadout must enter its battle.");
        var setup = session.PendingBattleSetup!;
        var nativeIds = source.Rewards.Where(reward => equipped.Contains(reward.Id)).Select(reward => reward.TacticSkillId)
            .OfType<string>().ToHashSet(StringComparer.Ordinal);
        Require(nativeIds.Count == nativeCount && setup.TacticSkillIds.ToHashSet(StringComparer.Ordinal).SetEquals(nativeIds) &&
            session.Snapshot.TacticIds.ToHashSet(StringComparer.Ordinal).SetEquals(equipped) &&
            session.Snapshot.ExtraSkillIds.Count == 0,
            "The real catalogue tactics equip through the codex path, with their native programs and no general-skill slot consumed.");
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, StandardShanheCampaignCatalog.CreateBattleRegistry(setup));
        var started = game.Submit(new StartGameCommand());
        Require(started.Accepted, started.Error?.Message ?? "The registered loadout must start normally.");
        var player = game.CreateSnapshot(0).Players[0];
        Require(nativeIds.IsSubsetOf(player.Skills!.Select(skill => skill.ContentId)) &&
            game.CreateSnapshot(0).Players[1].Skills!.All(skill => !nativeIds.Contains(skill.ContentId)),
            "The actual battle registry installs the complete programs on the holder and keeps them off the enemy.");
    }

    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
