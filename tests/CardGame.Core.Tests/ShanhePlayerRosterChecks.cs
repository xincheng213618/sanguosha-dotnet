using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanhePlayerRosterChecks
{
    private static readonly string[] LegacyRoster =
    [
        "classic:guan-yu", "classic:zhao-yun", "classic:zhang-fei", "classic:ma-chao",
        "classic:sun-quan", "classic:cao-cao", "classic:huang-yueying", "classic:zhou-yu"
    ];
    private const string ExpandedPlayer = "classic:lu-xun";

    public static void SavedRostersFreezeLegacyCompatibilityAndRejectForgedPools()
    {
        foreach (var edition in new[] { StandardShanheCampaignCatalog.LocalEditionId, StandardShanheCampaignCatalog.Returning2026EditionId })
        {
            var current = StandardShanheCampaignCatalog.CreateDefinition(1, edition);
            var legacy = Create(current with { PlayerGeneralIds = LegacyRoster }, LegacyRoster[0]).CreateCheckpoint() with { PlayerGeneralIds = null };
            var legacyJson = ShanheCampaignJson.Serialize(legacy);
            Require(!legacyJson.Contains("\"PlayerGeneralIds\"", StringComparison.Ordinal),
                "The additive roster field must remain absent from a genuine fixed-roster legacy checkpoint.");
            MustThrow(() => Restore(ShanheCampaignJson.Deserialize(legacyJson)));
            var restoredLegacy = ShanheCampaignSession.Restore(ShanheCampaignJson.Deserialize(legacyJson),
                current with { PlayerGeneralIds = LegacyRoster }, StandardShanheCampaignCatalog.CreateBattleRegistry);
            Require(restoredLegacy.Definition.PlayerGeneralIds.SequenceEqual(LegacyRoster) && restoredLegacy.ContentHash == legacy.CampaignHash,
                "Absent roster metadata keeps its original fingerprint and cannot select a different current pool.");

            var savedRoster = LegacyRoster.Append(ExpandedPlayer).ToList();
            var partialSession = Create(current with { PlayerGeneralIds = savedRoster }, ExpandedPlayer);
            var partialDefinition = partialSession.Definition;
            var saved = partialSession.CreateCheckpoint();
            var frozenRoster = saved.PlayerGeneralIds ?? throw new InvalidOperationException("New checkpoints must retain their player roster.");
            savedRoster.Clear();
            Require(frozenRoster.Count == LegacyRoster.Length + 1, "Checkpoint publication must detach the definition's source list.");
            RejectMutation(frozenRoster);
            var restored = Restore(saved);
            Require(restored.Definition.PlayerGeneralIds.SequenceEqual(LegacyRoster.Append(ExpandedPlayer)) &&
                restored.ContentHash == saved.CampaignHash && restored.Definition.PlayerGeneralIds.Count < current.PlayerGeneralIds.Count,
                "A saved roster remains exact when the current catalogue offers additional generals.");

            var callerRoster = frozenRoster.ToList();
            var detached = saved with { PlayerGeneralIds = callerRoster };
            callerRoster.Clear();
            Require(detached.PlayerGeneralIds!.Count == frozenRoster.Count, "The checkpoint roster init accessor must freeze caller-owned lists too.");
            RejectMutation(ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(saved)).PlayerGeneralIds!);
            MustThrow(() => Restore(saved with { CampaignHash = "invalid" }));
            MustThrow(() => Restore(saved with { PlayerGeneralIds = frozenRoster.Reverse().ToArray() }));
            MustThrow(() => Restore(saved with { PlayerGeneralIds = null }));
            var currentSaved = Create(current, current.PlayerGeneralIds[0]).CreateCheckpoint();
            Require(Restore(currentSaved with { PlayerGeneralIds = null }).Definition.PlayerGeneralIds.SequenceEqual(current.PlayerGeneralIds),
                "Missing optional roster metadata is valid when the current definition actually matches its saved fingerprint.");
            MustThrow(() => Restore(saved with { PlayerGeneralIds = frozenRoster.Append(ExpandedPlayer).ToArray() }));
            MustThrow(() => ShanheCampaignSession.Restore(saved with { PlayerGeneralIds = LegacyRoster }, partialDefinition,
                StandardShanheCampaignCatalog.CreateBattleRegistry));

            var bossRoster = LegacyRoster.Append(StandardShanheBossPackage.ShenguiLvbuId).ToArray();
            var forged = saved with
            {
                PlayerGeneralIds = bossRoster,
                CampaignHash = ShanheCampaignContent.GetContentHash(current with { PlayerGeneralIds = bossRoster })
            };
            MustThrow(() => Restore(forged));
        }
    }

    private static ShanheCampaignSession Create(ShanheCampaignDefinition definition, string player) =>
        ShanheCampaignSession.Create(definition, new(71, player) { SourceDifficultyId = definition.SourceDifficultyId }, StandardShanheCampaignCatalog.CreateBattleRegistry);

    private static ShanheCampaignSession Restore(ShanheCampaignCheckpoint checkpoint)
    {
        var cold = ShanheCampaignJson.Deserialize(ShanheCampaignJson.Serialize(checkpoint));
        return ShanheCampaignSession.Restore(cold, StandardShanheCampaignCatalog.CreateDefinitionForCheckpoint(cold, 1),
            StandardShanheCampaignCatalog.CreateBattleRegistry);
    }

    private static void RejectMutation(IReadOnlyList<string> values)
    {
        Require(values is IList<string> { IsReadOnly: true }, "Published player rosters must be read-only.");
        try { ((IList<string>)values)[0] = values[0]; }
        catch (NotSupportedException) { return; }
        throw new InvalidOperationException("A published player roster accepted mutation.");
    }

    private static void MustThrow(Action action)
    {
        try { action(); }
        catch (Exception error) when (error is ArgumentException or InvalidOperationException) { return; }
        throw new InvalidOperationException("Invalid campaign checkpoint was accepted.");
    }

    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
