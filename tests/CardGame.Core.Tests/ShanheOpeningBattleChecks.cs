using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheOpeningBattleChecks
{
    public static void ChapterPoolsTwoLocationsPreviewChallengeAndInheritanceAreDeterministic()
    {
        var definition = StandardShanheCampaignCatalog.CreateCodexDefinition(2, StandardShanheCampaignCatalog.Returning2026EditionId);
        var options = new ShanheCampaignOptions(71, "classic:guan-yu") { UseCodexRewardPool = true };
        ShanheCampaignSession Create(ShanheCampaignOptions? current = null) => ShanheCampaignSession.Create(definition,
            current ?? options, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var session = Create();
        Need(session.Snapshot.Phase == ShanheCampaignPhase.Map && session.Snapshot.AvailableNodes.Count == 2 &&
            session.Snapshot.AvailableNodes.All(node => node.Kind == ShanheNodeKind.Battle && node.ChapterNumber == 1) &&
            session.Snapshot.OpeningSkillOffers.Count == 0 && session.Snapshot.ExtraSkillIds.Count == 0,
            "Without inheritance the expedition opens directly on two first-chapter battle locations and grants no forced initial skill.");
        var laterEncounterIds = definition.Nodes.Where(node => node.ChapterNumber > 1)
            .SelectMany(node => node.Encounters ?? []).Select(encounter => encounter.Id).ToHashSet(StringComparer.Ordinal);
        foreach (var node in session.Snapshot.AvailableNodes)
        {
            Need(node.Encounters!.Count > 2 && node.Encounters.All(encounter =>
                    encounter.Enemies.Count is 1 or 2 && encounter.BossTransition is null && !laterEncounterIds.Contains(encounter.Id)) &&
                node.Encounters.All(encounter => encounter.VictoryGold == 200 && encounter.HealAfterVictory == 0) &&
                node.RewardIds!.All(id => definition.Rewards.Single(reward => reward.Id == id) is { SkillId: not null, Rarity: 2 }),
                "Each opening location draws from a substantial first-chapter pool, keeps later chapter recipes out, and awards copper plus a skill without auto-healing.");
            var before = Serialize(session);
            var checkpointBefore = ShanheCampaignJson.Serialize(session.CreateCheckpoint());
            var preview = session.PreviewEncounter(node.Id);
            Need(ReferenceEquals(preview, session.PreviewEncounter(node.Id)) && Serialize(session) == before &&
                ShanheCampaignJson.Serialize(session.CreateCheckpoint()) == checkpointBefore && session.PendingBattleSetup is null,
                "Repeated previews return the same immutable recipe without a move, journal write or battle preparation.");
            var cold = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(checkpointBefore), definition,
                StandardShanheCampaignCatalog.CreateBattleRegistry);
            Need(JsonSerializer.Serialize(cold.PreviewEncounter(node.Id)) == JsonSerializer.Serialize(preview),
                "Cold restoration retains the exact random story and entire encounter roster.");
            Need(cold.Submit(new MoveShanheNodeCommand(node.Id, cold.Revision)).Accepted, "The previewed location accepts its real move command.");
            var setup = cold.PendingBattleSetup!;
            Need(JsonSerializer.Serialize(setup.EnemyRoster) == JsonSerializer.Serialize(preview.Enemies) &&
                JsonSerializer.Serialize(setup.AllyRoster) == JsonSerializer.Serialize(preview.Allies) &&
                setup.EncounterOrdinal == 1 && cold.AcceptedCommands.Count == 1,
                "The first challenge uses exactly the previewed roster and commits only the normal move.");
            var battle = StandardShanheCampaignCatalog.CreateBattle(setup);
            Need(battle.Submit(new StartGameCommand()).Accepted &&
                battle.CreateSnapshot(0).Players.Count == 1 + preview.Enemies.Count + (preview.Allies?.Count ?? 0),
                "The selected first-chapter recipe starts a real native battle with every previewed seat.");
        }

        var inherited = Create(options with { EquippedTacticIds = [StandardShanheCampaignCatalog.OpeningInheritanceRewardId] });
        Need(inherited.Snapshot.Phase == ShanheCampaignPhase.OpeningSkillDraft && inherited.Snapshot.AvailableNodes.Count == 0 &&
            inherited.Snapshot.OpeningSkillOffers.Count == 3, "Only equipped inheritance pauses the map for an initial skill draft.");
        var chosen = inherited.Snapshot.OpeningSkillOffers[0];
        Need(inherited.Submit(new ChooseShanheOpeningSkillCommand(chosen.Id, inherited.Revision)).Accepted &&
            inherited.Snapshot.AvailableNodes.Select(node => node.Id).SequenceEqual(session.Snapshot.AvailableNodes.Select(node => node.Id)) &&
            inherited.Snapshot.ExtraSkillIds.SequenceEqual([chosen.SkillId!]),
            "One inheritance choice grants its skill and resumes both original battle locations.");
        foreach (var node in session.Snapshot.AvailableNodes)
            Need(inherited.PreviewEncounter(node.Id).Id == session.PreviewEncounter(node.Id).Id,
                "Choosing an initial skill must not reroll either first-chapter story.");
        var legacy = StandardShanheCampaignCatalog.CreateDefinition(2, StandardShanheCampaignCatalog.LocalEditionId);
        Need(legacy.RandomStartingNode && legacy.StartingNodeIds.All(id => legacy.Nodes.Single(node => node.Id == id).Kind == ShanheNodeKind.Event),
            "The explicitly named local story edition retains its original preparation entries.");
    }

    private static string Serialize(ShanheCampaignSession session) => JsonSerializer.Serialize(session.Snapshot);
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
