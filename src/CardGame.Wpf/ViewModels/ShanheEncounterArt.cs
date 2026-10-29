using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

/// <summary>Presentation-only source roles, matched to every actual enemy seat before use.</summary>
internal static class ShanheEncounterArt
{
    private sealed record Catalog(int SchemaVersion, string SourceMemberSha256, Profile[] Encounters);
    private sealed record Profile(int SourceBattleId, int SourceGroupId, int ExpectedVisibleEnemyCount,
        Variant[] Variants, Member[] Members);
    private sealed record Variant(int MinDifficulty, int MaxDifficulty, string[] ExpectedRuntimeGeneralIds,
        int[]? SourceOrdinalsInRuntimeOrder = null);
    private sealed record Member(int RawOrdinal, int SourceGeneralId, int? SkinId, bool IsBoss,
        bool Hide, int StartChapter, int GeneralShow);
    private static readonly Lazy<IReadOnlyDictionary<int, Profile>> Profiles = new(() =>
    {
        using var stream = typeof(ShanheEncounterArt).Assembly.GetManifestResourceStream("CardGame.ShanheEncounterLayouts.json")
            ?? throw new InvalidOperationException("Missing source encounter layouts.");
        var catalog = JsonSerializer.Deserialize<Catalog>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Invalid source encounter layouts.");
        if (catalog.SchemaVersion != 1 || catalog.SourceMemberSha256.Length != 64 || catalog.Encounters.Length == 0)
            throw new InvalidOperationException("Invalid source encounter layout metadata.");
        foreach (var profile in catalog.Encounters)
        {
            if (profile.SourceBattleId <= 0 || profile.SourceGroupId <= 0 || profile.Members.Length == 0 ||
                profile.ExpectedVisibleEnemyCount != profile.Members.Length || profile.Variants.Length == 0 ||
                profile.Members.Where((member, index) => member.RawOrdinal != index || member.SourceGeneralId <= 0 ||
                    member.Hide || member.StartChapter < 1 || member.GeneralShow != 0).Any() ||
                profile.Variants.Any(variant => variant.MinDifficulty < 1 || variant.MaxDifficulty > 25 ||
                    variant.MinDifficulty > variant.MaxDifficulty || variant.ExpectedRuntimeGeneralIds.Length != profile.Members.Length ||
                    variant.ExpectedRuntimeGeneralIds.Any(string.IsNullOrWhiteSpace) ||
                    variant.SourceOrdinalsInRuntimeOrder is { } ordinals &&
                        !ordinals.Order().SequenceEqual(Enumerable.Range(0, profile.Members.Length))) ||
                Enumerable.Range(1, 25).Any(difficulty => profile.Variants.Count(variant =>
                    difficulty >= variant.MinDifficulty && difficulty <= variant.MaxDifficulty) > 1))
                throw new InvalidOperationException("Source encounter layout has ambiguous seats or difficulty variants.");
        }
        return catalog.Encounters.ToDictionary(profile => profile.SourceBattleId);
    });

    public static IReadOnlyList<bool> Match(ShanheCampaignDefinition definition, ShanheNodeDefinition node,
        ShanheEncounterDefinition encounter)
    {
        if (!StandardShanheCampaignCatalog.IsSource2026Campaign(definition.Id, definition.SourceDifficultyId) ||
            encounter.SourceBattleId is not { } battleId || !Profiles.Value.TryGetValue(battleId, out var profile) ||
            encounter.Enemies.Count != profile.Members.Length || encounter.SourceVisibleEnemyCount != profile.ExpectedVisibleEnemyCount ||
            profile.Members.Any(member => member.StartChapter > node.ChapterNumber)) return [];
        var difficulty = definition.SourceDifficultyId!.Value;
        var variant = profile.Variants.SingleOrDefault(item => difficulty >= item.MinDifficulty && difficulty <= item.MaxDifficulty);
        if (variant is null || !encounter.Enemies.Select(enemy => enemy.GeneralId)
            .SequenceEqual(variant.ExpectedRuntimeGeneralIds, StringComparer.Ordinal)) return [];
        // An audited local seat order can differ from the raw source row order.
        // Its explicit full permutation is paired with the complete runtime ID
        // sequence above, never inferred from a name or a first-seat convention.
        return Array.AsReadOnly((variant.SourceOrdinalsInRuntimeOrder ?? Enumerable.Range(0, profile.Members.Length))
            .Select(ordinal => profile.Members[ordinal].IsBoss).ToArray());
    }
}
