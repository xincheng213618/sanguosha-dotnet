using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.Persistence;

/// <summary>Only the explicitly authored source edition uses twenty-five source difficulty identities.</summary>
public static class ShanheCampaignDifficultyContract
{
    public static bool IsSupported(string campaignId, int difficulty, int? sourceDifficultyId)
    {
        if (string.IsNullOrWhiteSpace(campaignId)) return false;
        if (sourceDifficultyId is { } source)
            return source == difficulty && StandardShanheSourceDifficultyCatalog.Season5.BySourceId.ContainsKey(source) &&
                StandardShanheCampaignCatalog.IsSource2026Campaign(campaignId, source);
        return difficulty is >= 1 and <= 3 &&
            !campaignId.StartsWith(StandardShanheCampaignCatalog.Source2026CampaignIdPrefix, StringComparison.Ordinal);
    }

    public static bool IsSupported(ShanheCampaignSession session, int difficulty)
    {
        var options = session.CreateCheckpoint().Options;
        return session.Definition.SourceDifficultyId == options.SourceDifficultyId &&
            IsSupported(session.Definition.Id, difficulty, options.SourceDifficultyId);
    }
}
