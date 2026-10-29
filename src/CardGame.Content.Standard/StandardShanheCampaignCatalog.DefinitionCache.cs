using System.Collections.Concurrent;
using System.Threading;
using CardGame.Core.Campaigns;

namespace CardGame.Content.Standard;

public static partial class StandardShanheCampaignCatalog
{
    private readonly record struct DefinitionCacheKey(bool Codex, int Difficulty, string EditionId);
    private static readonly ConcurrentDictionary<DefinitionCacheKey, Lazy<ShanheCampaignDefinition>> FinalDefinitions = new();

    private static ShanheCampaignDefinition GetCachedFinalDefinition(bool codex, int difficulty, string editionId)
    {
        var key = new DefinitionCacheKey(codex, difficulty, editionId);
        var cacheable = editionId == Source2026EditionId ? difficulty is >= 1 and <= 25 :
            (editionId is LocalEditionId or Returning2026EditionId) && difficulty is >= 1 and <= 3;
        // Invalid inputs still follow the original builder's validation and exceptions.
        if (!cacheable) return BuildFinalDefinition(key);
        // Both builders publish fully validated, detached definitions. Only these
        // final snapshots are shared; external with-variants retain the ordinary
        // FreezeAndValidate boundary. Source templates use a different edition key.
        return FinalDefinitions.GetOrAdd(key, static entry => new Lazy<ShanheCampaignDefinition>(
            () => BuildFinalDefinition(entry), LazyThreadSafetyMode.ExecutionAndPublication)).Value;
    }

    private static ShanheCampaignDefinition BuildFinalDefinition(DefinitionCacheKey key) => key.Codex
        ? BuildCodexDefinition(key.Difficulty, key.EditionId)
        : BuildDefinition(key.Difficulty, key.EditionId);
}
