using System.Windows.Media;
using CardGame.Content.Standard;
using CardGame.Core.Campaigns;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private static ShanheRewardArtwork CreateShanheRewardArtwork(ShanheRewardDefinition reward, bool isPreview = false)
    {
        var card = reward.StartingEquipment?.FirstOrDefault() ?? reward.StartingCards?.FirstOrDefault();
        var sourceId = StandardShanheCampaignCatalog.ShopMetadataByRewardId.TryGetValue(reward.Id, out var metadata)
            ? metadata.SourceRewardId
            : ShanheCodexTree.Entries.FirstOrDefault(entry => ShanheCodexTree.RuntimeRewardId(entry.Id) == reward.Id)?.RewardId ?? 0;
        const string chapterCardPrefix = "chapter-epic-card-";
        if (sourceId == 0 && reward.Id.StartsWith(chapterCardPrefix, StringComparison.Ordinal))
            int.TryParse(reward.Id.AsSpan(chapterCardPrefix.Length), out sourceId);
        var image = ShanheArt.Get(sourceId) ?? (card is null ? null : CardArt.Get(card.Kind));
        return new(ShanheDisplayText.RewardName(reward.Name), image, card is not null, isPreview);
    }
}

public sealed record ShanheRewardArtwork(string Name, ImageSource? Image, bool IsCard, bool IsPreview)
{
    public bool HasImage => Image is not null;
    public string Glyph => Name[..Math.Min(2, Name.Length)];
    public double Width => IsPreview ? IsCard ? 62 : 68 : 112;
    public double Height => IsCard ? IsPreview ? 87 : 156 : Width;
}
