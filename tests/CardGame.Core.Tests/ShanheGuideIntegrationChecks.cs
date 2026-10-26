using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheGuideIntegrationChecks
{
    public static void GuideStockInheritanceTerrainAndMuluEnterNativeBattlesCold()
    {
        for (var difficulty = 1; difficulty <= 3; difficulty++)
        {
            var definition = StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var terrains = definition.Nodes.SelectMany(node => node.Encounters ?? []).Where(encounter => encounter.TerrainSkillId is not null).ToArray();
            Need(difficulty == 3 ? terrains.Select(encounter => encounter.TerrainSkillId).Distinct().Count() == 6 : terrains.Length == 0,
                "Only the local third difficulty enables all six authored terrain kinds.");
            var inherited = definition.Rewards.Single(reward => reward.Id == StandardShanheCampaignCatalog.OpeningInheritanceRewardId).OpeningSkillRewardIds!;
            Need(inherited.Contains("build-skill-3362") && definition.Rewards.Single(reward => reward.Id == "build-skill-3341").SkillId == "boundary:gongqi-current",
                "The guide's rare Xiasheng and exact Gongqi edition are reachable through the real catalogue and inheritance pool.");
            foreach (var id in new[] { "build-skill-3362", "build-skill-3365", "build-skill-3366", "tactic-duipai-2026", "tactic-duijia-2026" })
                Need(definition.Rewards.Single(reward => reward.Id == id).Rarity == StandardShanheCampaignCatalog.ShopMetadataByRewardId[id].Rarity,
                    "Source shop quality also drives real reward offers: " + id);
            Need(definition.Rewards.All(reward => reward.SchoolMembership?.SourceId != 2235),
                "The automatic team set cannot be claimed as ordinary loot.");
            var mulu = definition.Nodes.Single(node => node.Id == StandardShanheCampaignCatalog.MuluEncounterNodeId);
            Need(mulu.Kind == ShanheNodeKind.Battle && mulu.Encounters is [{ Enemies.Count: 5 }] && mulu.NextNodeIds.Count > 0,
                "The summoner and all four authored elephants enter a nonterminal third-chapter battle.");
            if (difficulty != 3) continue;
            foreach (var encounter in new[] { terrains.First(), mulu.Encounters!.Single() })
            {
                var fixture = new ShanheCampaignDefinition("fixture:guide-integration:" + encounter.Id, "攻略接线", ["classic:guan-yu"], ["battle"],
                    [new("battle", "实战", ShanheNodeKind.Battle, [], [encounter])], []);
                var session = ShanheCampaignSession.Create(fixture, new(71, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
                var preview = session.PreviewEncounter("battle");
                Need(session.Submit(new MoveShanheNodeCommand("battle", session.Revision)).Accepted &&
                    session.PendingBattleSetup!.TerrainSkillId == preview.TerrainSkillId,
                    "Preview and challenge carry the same optional terrain rather than silently dropping it.");
                var restored = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(session.CreateCheckpoint())),
                    fixture, StandardShanheCampaignCatalog.CreateBattleRegistry);
                Need(ShanheBattleSetup.ValueEquals(restored.PendingBattleSetup, session.PendingBattleSetup), "Cold campaign replay preserves guide recipes.");
                var game = StandardShanheCampaignCatalog.CreateBattle(restored.PendingBattleSetup!);
                Need(game.Submit(new StartGameCommand()).Accepted, "The real registered guide encounter starts through the normal battle factory.");
                if (preview.TerrainSkillId is { } terrain)
                    Need(game.CreateSnapshot(0).Players.All(player => player.Skills!.Any(skill => skill.Id == terrain)),
                        "Terrain reaches every player and enemy as a permanent mode grant.");
            }
        }
    }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
