namespace CardGame.Core.Campaigns;

public static partial class ShanheCampaignContent
{
    private static void ValidateChapterRoutes(ShanheCampaignDefinition definition)
    {
        foreach (var node in definition.Nodes)
        {
            Require(Enum.IsDefined(node.ChapterRole) && node.ChapterProgressStep is >= 0 and <= 6 &&
                node.RequiredChapterProgress is >= 0 and <= 6 &&
                (node.ChapterRouteId is not null ? node.ChapterRole != ShanheChapterNodeRole.None
                    : node.ChapterRole == ShanheChapterNodeRole.None &&
                    node.ChapterProgressStep == 0 && node.RequiredChapterProgress == 0),
                "Chapter route metadata must have a declared route and bounded ordered progress.");
            Require(node.ChapterRole switch
            {
                ShanheChapterNodeRole.None => true,
                ShanheChapterNodeRole.Event => node.Kind == ShanheNodeKind.Event,
                ShanheChapterNodeRole.Rest => node.Kind == ShanheNodeKind.Camp,
                ShanheChapterNodeRole.Market => node.Kind == ShanheNodeKind.Shop,
                ShanheChapterNodeRole.Boss => node.Kind == ShanheNodeKind.BossBattle,
                ShanheChapterNodeRole.Profile => IsBattle(node.Kind),
                _ => node.Kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle
            }, "A chapter route role must match its actual interaction kind.");
        }
        foreach (var group in definition.Nodes.Where(node => node.ChapterRouteId is not null).GroupBy(node => node.ChapterRouteId))
        {
            RequireText(group.Key!, "chapter route");
            Require(group.Select(node => node.ChapterNumber).Distinct().Count() == 1 && group.All(node => node.ChapterNumber > 0),
                "One chapter route must stay in its actual chapter.");
            Require(group.Count(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle && node.ChapterProgressStep == 1 &&
                    node.RequiredChapterProgress == 0 && IsBattle(node.Kind)) == 2 &&
                Enumerable.Range(2, 5).All(step => group.Count(node => node.ChapterProgressStep == step &&
                    node.RequiredChapterProgress == step - 1) == 1) &&
                group.All(node => node.ChapterProgressStep == 0 || node.ChapterRole is ShanheChapterNodeRole.OpeningBattle or
                    ShanheChapterNodeRole.Event or ShanheChapterNodeRole.MainBattle or ShanheChapterNodeRole.Rest),
                "Chapter routes need two opening alternatives and five sequential mainline steps.");
            Require(group.Count(node => node.ChapterRole == ShanheChapterNodeRole.Boss && node.Kind == ShanheNodeKind.BossBattle &&
                    node.RequiredChapterProgress == 6 && node.ChapterProgressStep == 0) == 1 &&
                group.Count(node => node.ChapterRole == ShanheChapterNodeRole.Market && node.Kind == ShanheNodeKind.Shop &&
                    node.RequiredChapterProgress == 3) == 1 &&
                group.Count(node => node.ChapterRole == ShanheChapterNodeRole.Arena && IsBattle(node.Kind) &&
                    node.RequiredChapterProgress == 3 && node.ChapterProgressStep == 0) == 1 &&
                group.Count(node => node.ChapterRole == ShanheChapterNodeRole.OptionalBattle && IsBattle(node.Kind) &&
                    node.RequiredChapterProgress == 6 && node.ChapterProgressStep == 0) == 1 &&
                group.All(node => node.ChapterRole != ShanheChapterNodeRole.ExtraBattle ||
                    node.ExtraChapterBattleIndex > 0 && node.RequiredChapterProgress == 6 && node.ChapterProgressStep == 0),
                "Boss, market, arena and optional pursuits must retain their distinct unlock and progress contracts.");
        }
    }
}
