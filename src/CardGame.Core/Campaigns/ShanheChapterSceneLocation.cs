using System.Globalization;

namespace CardGame.Core.Campaigns;

/// <summary>Original city identities occupied by the chapter itinerary's successive scenes.</summary>
public static class ShanheChapterSceneLocation
{
    public static int? NativeCityId(ShanheNodeDefinition node, int openingIndex, int completedOpeningIndex)
    {
        const string prefix = "source-region-";
        if (node.ChapterRouteId is not { } route || !route.StartsWith(prefix, StringComparison.Ordinal) ||
            !int.TryParse(route.AsSpan(prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out var region) || region is < 1 or > 14)
            return null;
        var ordinal = node.ChapterRole switch
        {
            ShanheChapterNodeRole.OpeningBattle => region == 3 ? openingIndex == 0 ? 2 : 3 : openingIndex == 0 ? 1 : 2,
            ShanheChapterNodeRole.MainBattle => region == 3 ? completedOpeningIndex == 0 ? 2 : 3 : completedOpeningIndex == 0 ? 1 : 2,
            ShanheChapterNodeRole.Event when region == 3 => node.ChapterProgressStep switch { 2 => 5, 3 => 1, _ => 7 },
            ShanheChapterNodeRole.Event when region == 14 => node.ChapterProgressStep switch { 2 => 6, 3 => 7, _ => 4 },
            ShanheChapterNodeRole.Event => node.ChapterProgressStep + 1,
            ShanheChapterNodeRole.Rest => region == 3 ? 5 : region == 14 ? 6 : 3,
            ShanheChapterNodeRole.OptionalBattle => region == 3 ? 7 : region == 14 ? 4 : 5,
            ShanheChapterNodeRole.ExtraBattle when region == 3 => node.ExtraChapterBattleIndex == 1 ? 4 : 6,
            ShanheChapterNodeRole.ExtraBattle when region == 14 => node.ExtraChapterBattleIndex == 1 ? 3 : 5,
            ShanheChapterNodeRole.ExtraBattle => 5 + node.ExtraChapterBattleIndex,
            ShanheChapterNodeRole.Boss => 8,
            _ => 0
        };
        return ordinal is >= 1 and <= 8 ? 5000 + region * 10 + ordinal : null;
    }
}
