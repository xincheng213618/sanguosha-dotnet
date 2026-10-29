using System.Windows;
using System.Windows.Media;
using CardGame.Core.Campaigns;
using CardGame.Wpf.Resources;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private int _shanheChapterStepCount;
    private int _shanheChapterExtraBattleCount;
    private int _shanheChapterBattleCount;
    private int _shanheChapterEncounterCount;
    private double _shanheMapCanvasHeight = 560;
    private string _shanheChapterBossText = "击败本章首领，开启下一章";
    private IReadOnlyList<ShanheMapSceneryItem> _shanheMapScenery = [];

    public int ShanheChapterStepCount => _shanheChapterStepCount;
    public bool HasShanheNativeMap => ShanheMapArt.Region(_shanheSession?.ActiveChapterRouteId) is not null;
    public ImageSource? ShanheMapBackground => ShanheMapArt.Region(_shanheSession?.ActiveChapterRouteId)?.Artwork ?? AssetImages.Load("Assets/shanhe-map.png");
    public string ShanheMapLandscapeRoads => _shanheSession?.ActiveChapterRouteId == "source-region-14"
        ? "M 138,179 C 238,160 330,241 408,279 C 510,286 572,173 638,164 C 726,156 801,235 868,294 C 825,315 775,316 718,319 C 721,369 723,420 718,464 C 665,478 613,460 568,464 C 491,463 431,461 378,469 C 282,462 183,484 143,424 C 114,350 116,256 138,179 M 408,279 C 487,318 530,398 568,464 M 718,319 C 649,348 587,389 568,464"
        : "M 158,199 C 268,209 353,151 478,159 C 579,138 677,175 793,189 C 828,229 826,287 798,354 C 789,414 839,449 838,499 C 749,513 642,498 538,489 C 421,501 337,501 218,489 C 179,449 162,400 158,359 C 191,305 311,325 458,319 C 477,263 407,219 478,159 M 158,199 C 199,249 175,318 158,359 M 458,319 C 568,318 671,347 798,354 M 458,319 C 509,387 555,432 538,489";
    public int ShanheChapterCompletedCount => _shanheChapterCompleted;
    public double ShanheMapCanvasHeight => _shanheMapCanvasHeight;
    public bool IsShanheMapScrollable => !HasShanheNativeMap && _shanheMapCanvasHeight > 560;
    public IReadOnlyList<ShanheMapSceneryItem> ShanheMapScenery => _shanheMapScenery;
    public string ShanheChapterRouteText => _shanheUsesChapterProgress ? $"主线 {_shanheChapterCompleted} / 6 · 可选战斗与擂台可跳过" : $"本章 {_shanheChapterStepCount} 程 · {_shanheChapterBattleCount} 处战斗 · {_shanheChapterEncounterCount} 处奇遇与休整";
    public string ShanheChapterRewardHint => "战斗获得铜币与战利品，铜币可在常驻集市换取战法、技能和装备。";
    public string ShanheExtraBattleText => _shanheChapterExtraBattleCount > 0
        ? $"日行千里 · 本章额外 {_shanheChapterExtraBattleCount} 场战斗"
        : "日行千里Ⅰ / Ⅱ：每章额外挑战 1 / 2 场战斗";
    public string ShanheChapterBossText => _shanheChapterBossText;

    private bool _shanheUsesChapterProgress;
    public int ShanheChapterMainProgress => _shanheUsesChapterProgress ? _shanheChapterCompleted : 0;
    public int ShanheChapterProgressTarget => _shanheUsesChapterProgress ? 6 : 0;
    public ImageSource? ShanheChapterBossArtwork => ShanheMapArt.BossEmblem(ShanheChapterProgressTarget == 6 && ShanheChapterMainProgress >= 6);
    public IReadOnlyList<ShanheChapterProgressSlot> ShanheChapterProgressSlots => _shanheUsesChapterProgress
        ? Enumerable.Range(1, 6).Select(index => new ShanheChapterProgressSlot(index,
            index <= ShanheChapterMainProgress, index == ShanheChapterMainProgress + 1)).ToArray() : [];

    private void BuildShanheChapterRouteMap(ShanheCampaignDefinition definition,
        IReadOnlyDictionary<string, int> depths, string? currentNodeId, IReadOnlySet<string> completed)
    {
        ShanheMapNodes.Clear();
        ShanheMapLinks.Clear();
        var visible = CurrentShanheChapterNodes(definition, depths, currentNodeId, completed).ToArray();
        _shanheUsesChapterProgress = visible.Any(node => node.ChapterRouteId is not null);
        var presentations = ShanheMapRows.SelectMany(row => row.Nodes).ToDictionary(node => node.Id, StringComparer.Ordinal);
        var stages = ShanheChapterStages(visible);
        var opening = visible.Where(node => node.ChapterRole == ShanheChapterNodeRole.OpeningBattle).ToArray();
        var firstOpeningId = _shanheSession?.Snapshot.CompletedNodeIds.FirstOrDefault(id => opening.Any(node => node.Id == id));
        var chosenOpening = opening.FirstOrDefault(node => node.Id == firstOpeningId) ?? opening.FirstOrDefault();
        var chosenOpeningIndex = chosenOpening is null ? 0 : Array.IndexOf(opening, chosenOpening);
        var assignments = visible.Where(node => node.ChapterRole is not (ShanheChapterNodeRole.Profile or ShanheChapterNodeRole.Market))
            .Select((node, index) => (Node: node, Place: _shanheUsesChapterProgress
                ? ShanheChapterPlace(node, Array.IndexOf(opening, node), chosenOpeningIndex)
                : ShanheLegacyPlace(index, node))).ToArray();
        // A later event can occupy an already visited landmark. Keep every real
        // available choice, and replace a completed marker only when its new use opens.
        var locations = assignments.GroupBy(item => item.Place.Key, StringComparer.Ordinal).Select(group => group
            .OrderByDescending(item => item.Node.Id == currentNodeId && !completed.Contains(item.Node.Id) ? 5
                : presentations[item.Node.Id].IsAvailable ? 4
                : _shanheSession?.IsChapterNodeUnlocked(item.Node.Id) == true && item.Node.RequiredChapterProgress > 0 ? 3
                : completed.Contains(item.Node.Id) ? 2 : 1)
            .ThenByDescending(item => completed.Contains(item.Node.Id) ? item.Node.RequiredChapterProgress : -item.Node.RequiredChapterProgress)
            .First()).ToArray();
        var currentPlace = assignments.FirstOrDefault(item => item.Node.Id == currentNodeId).Place.Key;
        var playerLocation = locations.FirstOrDefault(item => item.Node.Id == currentNodeId ||
                currentPlace is not null && item.Place.Key == currentPlace).Node?.Id
            ?? locations.FirstOrDefault(item => presentations[item.Node.Id].IsAvailable).Node?.Id;
        _shanheMapCanvasHeight = HasShanheNativeMap ? 600 : _shanheUsesChapterProgress ? 560 : Math.Max(560, 560 + ((visible.Length - 1) / 9) * 440);
        foreach (var (node, place) in locations)
        {
            var native = ShanheMapArt.Landmark(node.ChapterRouteId, place.Key);
            var mainline = _shanheUsesChapterProgress && _shanheSession?.IsChapterNodeMainline(node.Id) == true;
            var routeFlag = ShanheRouteFlagText(node, presentations[node.Id].IsAvailable, mainline);
            ShanheMapNodes.Add(new(presentations[node.Id], native is null ? place.X : native.MapX + native.MapWidth / 2,
                native is null ? place.Y : native.MapY + native.MapHeight / 2, node.Kind,
                native?.Name ?? (_shanheUsesChapterProgress ? ShanheChapterPlaceLabel(node, place) : ShanheMapShortName(node)),
                node.Id == currentNodeId, completed.Contains(node.Id))
            {
                NativeLandmark = native,
                IsPlayerLocation = node.Id == playerLocation,
                SceneLandmark = place.Landmark,
                StageLabel = node.Kind == ShanheNodeKind.Shop ? "集市" : _shanheUsesChapterProgress
                    ? routeFlag == "主线" ? $"主线{node.ChapterProgressStep}" : routeFlag.Length > 0 ? routeFlag
                        : node.ChapterRole == ShanheChapterNodeRole.Boss ? "关底" : string.Empty
                    : (stages[node.Id] + 1).ToString("00"),
                IsMainline = mainline,
                IsExtraBattle = node.ExtraChapterBattleIndex > 0,
                RouteFlagText = routeFlag,
                RouteSourceText = routeFlag switch
                {
                    "追击" => "日行千里 · 额外战斗",
                    "主线" => "主线行军",
                    "可选" => node.ChapterRole == ShanheChapterNodeRole.Arena ? "可选擂台" : "可选战斗",
                    _ => presentations[node.Id].IsAvailable && node.Kind == ShanheNodeKind.BossBattle ? "本章关底" : string.Empty
                },
                RewardText = ShanheMapRewardText(node),
                EnemyText = string.Empty
            });
        }
        var occupiedLocations = locations.Select(item => item.Place.Key).ToHashSet(StringComparer.Ordinal);
        _shanheMapScenery = _shanheUsesChapterProgress && _shanheSession is { } scenerySession
            ? scenerySession.Definition.Nodes.Where(node => node.ChapterRouteId == scenerySession.ActiveChapterRouteId &&
                    node.ChapterRole == ShanheChapterNodeRole.ExtraBattle)
                .Select(node => (Node: node, Place: ShanheChapterPlace(node, -1, chosenOpeningIndex)))
                .Where(item => !occupiedLocations.Contains(item.Place.Key)).DistinctBy(item => item.Place.Key)
                .Select(item => new ShanheMapSceneryItem(item.Place.Landmark, ShanheChapterPlaceLabel(item.Node, item.Place),
                    item.Place.X - 68, item.Place.Y - 44) { NativeLandmark = ShanheMapArt.Landmark(item.Node.ChapterRouteId, item.Place.Key) }).ToArray()
            : [];
        if (!_shanheUsesChapterProgress) BuildShanheLegacyMapLinks(visible);
        _shanheChapterStepCount = _shanheUsesChapterProgress ? 6 : visible.Where(node => node.Kind != ShanheNodeKind.Shop)
            .Select(node => stages[node.Id]).DefaultIfEmpty(-1).Max() + 1;
        if (_shanheUsesChapterProgress) _shanheChapterCompleted = _shanheSession?.ChapterProgress ?? 0;
        _shanheChapterExtraBattleCount = _shanheSession?.ChapterExtraBattleCount ?? 0;
        _shanheChapterBattleCount = visible.Count(node => IsShanheMapBattle(node.Kind));
        _shanheChapterEncounterCount = visible.Count(node => !IsShanheMapBattle(node.Kind) && node.Kind != ShanheNodeKind.Shop);
        _shanheChapterBossText = _shanheUsesChapterProgress && _shanheChapterCompleted < 6 ? "完成六步主线后，关底首领开放"
            : (_shanheUsesChapterProgress ? _shanheChapter >= 3 : visible.Any(node => node.Kind == ShanheNodeKind.BossBattle && node.NextNodeIds.Count == 0))
                ? "击败最终首领，完成山河征程" : "击败本章首领，领取奖励后进入下一章";
        foreach (var property in new[] { nameof(ShanheChapterRouteText), nameof(ShanheChapterRewardHint),
            nameof(ShanheChapterStepCount), nameof(ShanheChapterCompletedCount), nameof(ShanheMapCanvasHeight), nameof(IsShanheMapScrollable), nameof(ShanheMapScenery), nameof(ShanheMapLandscapeRoads),
            nameof(ShanheChapterMainProgress), nameof(ShanheChapterProgressTarget), nameof(ShanheChapterProgressSlots), nameof(ShanheChapterBossArtwork),
            nameof(ShanheExtraBattleText), nameof(ShanheChapterBossText), nameof(HasShanheNativeMap), nameof(ShanheMapBackground) }) RaisePropertyChanged(property);
    }

    private string ShanheRouteFlagText(ShanheNodeDefinition node, bool available, bool mainline)
    {
        if (!available) return string.Empty;
        if (node.ExtraChapterBattleIndex > 0) return "追击";
        if (!_shanheUsesChapterProgress) return string.Empty;
        if (mainline) return "主线";
        return node.ChapterRole is ShanheChapterNodeRole.Arena or ShanheChapterNodeRole.OptionalBattle or
            ShanheChapterNodeRole.OpeningBattle ? "可选" : string.Empty;
    }

    private static (string Key, string Landmark, double X, double Y) ShanheChapterPlace(ShanheNodeDefinition node,
        int openingIndex, int completedOpeningIndex)
    {
        if (ShanheMapArt.Region(node.ChapterRouteId) is { } region)
        {
            var cityId = ShanheChapterSceneLocation.NativeCityId(node, openingIndex, completedOpeningIndex);
            var ordinal = cityId % 10 ?? 0;
            var key = ordinal == 0 ? "arena" : "native-city-" + ordinal;
            var art = ordinal == 0 ? region.Arena : region.Cities.Single(city => city.CityId == cityId);
            var landmark = ordinal == 0 ? "Arena" : ordinal == 8 ? "Boss" : region.RegionId switch
            {
                3 => ordinal switch { 2 or 7 => "Cave", 3 => "Platform", 4 => "Forest", 5 => "Bridge", 6 => "Lake", _ => "City" },
                14 when ordinal == 4 => "Cave",
                _ => "City"
            };
            return (key, landmark,
                art.MapX + art.MapWidth / 2, art.MapY + art.MapHeight / 2);
        }
        // The user's supplied third-chapter map identifies route 14 as 崤函道.
        if (node.ChapterRouteId == "source-region-14") return node.ChapterRole switch
        {
            ShanheChapterNodeRole.OpeningBattle => openingIndex == 0 ? ("puban", "City", 150, 145) : ("hebei", "City", 420, 245),
            ShanheChapterNodeRole.Event when node.ChapterProgressStep == 2 => ("caoyang", "City", 730, 285),
            ShanheChapterNodeRole.Event when node.ChapterProgressStep == 3 => ("shanjin", "City", 880, 260),
            ShanheChapterNodeRole.Event => ("cave", "Cave", 580, 430),
            ShanheChapterNodeRole.Arena => ("arena", "Arena", 650, 130),
            ShanheChapterNodeRole.MainBattle => completedOpeningIndex == 0 ? ("puban", "City", 150, 145) : ("hebei", "City", 420, 245),
            ShanheChapterNodeRole.Rest => ("caoyang", "City", 730, 285),
            ShanheChapterNodeRole.OptionalBattle => ("cave", "Cave", 580, 430),
            ShanheChapterNodeRole.ExtraBattle when node.ExtraChapterBattleIndex == 1 => ("huxian", "City", 390, 435),
            ShanheChapterNodeRole.ExtraBattle => ("hongnong", "City", 730, 430),
            ShanheChapterNodeRole.Boss => ("tongguan", "Boss", 155, 390),
            _ => (node.Id, "City", 100, 100)
        };
        return node.ChapterRole switch
        {
            ShanheChapterNodeRole.OpeningBattle => openingIndex == 0 ? ("upper-cave", "Cave", 490, 125) : ("platform", "Platform", 805, 155),
            ShanheChapterNodeRole.Event when node.ChapterProgressStep == 2 => ("bridge", "Bridge", 470, 285),
            ShanheChapterNodeRole.Event when node.ChapterProgressStep == 3 => ("city", "City", 170, 165),
            ShanheChapterNodeRole.Event => ("lower-cave", "Cave", 850, 465),
            ShanheChapterNodeRole.Arena => ("arena", "Arena", 170, 325),
            ShanheChapterNodeRole.MainBattle => completedOpeningIndex == 0 ? ("upper-cave", "Cave", 490, 125) : ("platform", "Platform", 805, 155),
            ShanheChapterNodeRole.Rest => ("bridge", "Bridge", 470, 285),
            ShanheChapterNodeRole.OptionalBattle => ("lower-cave", "Cave", 850, 465),
            ShanheChapterNodeRole.ExtraBattle when node.ExtraChapterBattleIndex == 1 => ("forest", "Forest", 230, 465),
            ShanheChapterNodeRole.ExtraBattle => ("lake", "Lake", 810, 320),
            ShanheChapterNodeRole.Boss => ("boss", "Boss", 550, 455),
            _ => (node.Id, "City", 100, 100)
        };
    }

    private static string ShanheChapterPlaceLabel(ShanheNodeDefinition node,
        (string Key, string Landmark, double X, double Y) place)
    {
        if (node.ChapterRouteId == "source-region-14") return place.Key switch
        {
            "puban" => "蒲坂", "hebei" => "河北", "caoyang" => "曹阳", "shanjin" => "陕津",
            "huxian" => "湖县", "hongnong" => "弘农", "tongguan" => "潼关", "arena" => "擂台", _ => "洞窟"
        };
        if (node.ChapterRouteId == "source-region-3")
        {
            if (place.Landmark == "City") return "中条山";
            if (place.Landmark == "Lake") return "湖县";
            if (place.Landmark == "Boss") return "函谷关";
        }
        return place.Landmark switch
        {
            "Cave" => "洞窟", "Platform" => "聚义台", "Bridge" => "栈桥", "Forest" => "密林",
            "Lake" => "湖畔", "Arena" => "擂台", "Boss" =>
                !string.IsNullOrWhiteSpace(node.ChapterTitle) ? node.ChapterTitle.Split('·').Last().Trim() : node.Name.Split('·').First().Trim(),
            _ => "山道"
        };
    }

    private static (string Key, string Landmark, double X, double Y) ShanheLegacyPlace(int index, ShanheNodeDefinition node)
    {
        (string Landmark, double X, double Y)[] places = [("City",170,165),("Cave",490,125),("Platform",805,155),
            ("Bridge",470,285),("Lake",810,320),("Arena",170,325),("Forest",230,465),("Boss",550,455),("Cave",850,465)];
        var place = places[index % places.Length];
        return (node.Id, place.Landmark, place.X, place.Y + index / places.Length * 440);
    }

    private void BuildShanheLegacyMapLinks(IReadOnlyList<ShanheNodeDefinition> nodes)
    {
        var positions = ShanheMapNodes.ToDictionary(node => node.Node.Id, StringComparer.Ordinal);
        foreach (var node in nodes)
            foreach (var next in node.NextNodeIds)
            {
                if (!positions.TryGetValue(node.Id, out var start) || !positions.TryGetValue(next, out var end)) continue;
                var route = new StreamGeometry();
                using (var drawing = route.Open())
                {
                    drawing.BeginFigure(new Point(start.CenterX, start.CenterY + 24), false, false);
                    drawing.LineTo(new Point(end.CenterX, end.CenterY + 24), true, false);
                }
                route.Freeze();
                ShanheMapLinks.Add(new(route, start.IsCompleted && (end.IsCurrent || end.IsCompleted), end.IsAvailable));
            }
    }

    private static IReadOnlyDictionary<string, int> ShanheChapterStages(IReadOnlyList<ShanheNodeDefinition> nodes)
    {
        var byId = nodes.ToDictionary(node => node.Id, StringComparer.Ordinal);
        var remaining = nodes.ToDictionary(node => node.Id, _ => 0, StringComparer.Ordinal);
        foreach (var node in nodes)
            foreach (var next in node.NextNodeIds)
                if (remaining.ContainsKey(next)) remaining[next]++;
        var stages = nodes.ToDictionary(node => node.Id, _ => 0, StringComparer.Ordinal);
        var pending = new Queue<string>(nodes.Where(node => remaining[node.Id] == 0).Select(node => node.Id));
        while (pending.TryDequeue(out var id))
            foreach (var next in byId[id].NextNodeIds)
            {
                if (!remaining.ContainsKey(next)) continue;
                stages[next] = Math.Max(stages[next], stages[id] + (byId[id].Kind == ShanheNodeKind.Shop ? 0 : 1));
                if (--remaining[next] == 0) pending.Enqueue(next);
            }
        return stages;
    }

    private static bool IsShanheMapBattle(ShanheNodeKind kind) =>
        kind is ShanheNodeKind.Battle or ShanheNodeKind.EliteBattle or ShanheNodeKind.BossBattle;

    private static string ShanheMapRewardText(ShanheNodeDefinition node)
    {
        if (!IsShanheMapBattle(node.Kind)) return node.Kind switch
        {
            ShanheNodeKind.Event => "抉择与机缘", ShanheNodeKind.Camp => "休整与补给",
            ShanheNodeKind.Shop => "铜币换取成长", _ => string.Empty
        };
        var gold = node.Encounters?.Select(encounter => encounter.VictoryGold).Distinct().Order().ToArray() ?? [];
        var amount = node.BossRewards is { } boss ? boss.FixedGold.ToString() : gold.Length switch
        {
            0 => "0", 1 => gold[0].ToString(), _ => $"{gold[0]}–{gold[^1]}"
        };
        return $"铜币 {amount}" + (node.BossRewards is not null || node.RewardIds?.Count > 0 ? " · 战利品" : string.Empty);
    }


}

public sealed record ShanheChapterProgressSlot(int Index, bool IsCompleted, bool IsCurrent);
public sealed record ShanheMapSceneryItem(string SceneLandmark, string ShortName, double FallbackX, double FallbackY)
{
    public ShanheNativeMapLandmark? NativeLandmark { get; init; }
    public bool HasNativeArtwork => NativeLandmark is not null;
    public ImageSource? NativeArtwork => NativeLandmark?.DormantArtwork;
    public bool IsAvailable => false;
    public double X => NativeLandmark?.MapX ?? FallbackX;
    public double Y => NativeLandmark?.MapY ?? FallbackY;
}
