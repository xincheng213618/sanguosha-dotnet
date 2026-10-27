namespace CardGame.Core.Campaigns;

/// <summary>Relative stock probabilities for the four existing catalogue qualities.</summary>
public sealed record ShanheShopRarityWeights(int Common, int Rare, int Epic, int Legendary)
{
    internal int Weight(int rarity) => rarity switch
    { 1 => Common, 2 => Rare, 3 => Epic, 4 => Legendary, _ => 0 };
}

/// <summary>Chapter three keeps the later-chapter balance, including detached battle markets.</summary>
public sealed record ShanheShopRarityPolicy(ShanheShopRarityWeights FirstChapter, ShanheShopRarityWeights LaterChapters);

public static partial class ShanheCampaignContent
{
    private static void ValidateShopRarityPolicy(ShanheShopRarityPolicy? policy)
    {
        if (policy is null) return;
        foreach (var weights in new[] { policy.FirstChapter, policy.LaterChapters })
            Require(weights is not null && weights.Common is >= 0 and <= 100 && weights.Rare is >= 0 and <= 100 &&
                weights.Epic is >= 0 and <= 100 && weights.Legendary is >= 0 and <= 100 &&
                weights.Common + weights.Rare + weights.Epic + weights.Legendary == 100,
                "Shop rarity weights must be bounded percentages totaling one hundred for each chapter profile.");
    }
}
