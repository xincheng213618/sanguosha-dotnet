using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceMixedTroopChecks
{
    private static readonly int[] Groups = [2465, 2499, 2464, 2492, 2475, 2485, 2471];
    private static bool SecondChapter(int group) => group is 2499 or 2475 or 2485 or 2471;

    public static void CompleteMixedSourceRecipesAndBothRecommendedRoutes()
    {
        var graph = StandardShanheSourceRouteCatalog.Season5;
        foreach (var tier in new[] { 1, 6, 11, 16, 21, 25 })
        {
            var definition = StandardShanheCampaignCatalog.CreateSourceDefinition(tier);
            var cities = graph.Regions.Where(r => r.MinimumSourceDifficultyId <= tier).SelectMany(r => r.CityIds)
                .Select(id => graph.ByCityId[id]).Where(c => !c.IsBoss).ToArray();
            foreach (var group in Groups)
            {
                var expectedCities = cities.Where(c => graph.GetPotentialBattleIds(c.SourceCityId).Contains(group))
                    .Select(c => c.SourceCityId).Order().ToArray();
                var actualCities = new List<int>();
                foreach (var city in cities)
                {
                    var node = definition.Nodes.Single(n => n.Id == StandardShanheCampaignCatalog.Source2026CityNodeId(city.SourceCityId));
                    var matches = node.Encounters!.Where(e => e.SourceBattleId == group).ToArray();
                    Need(matches.Length == (expectedCities.Contains(city.SourceCityId) ? 1 : 0),
                        $"Source{group}/{tier} must occur exactly once in each real open source pool and nowhere else.");
                    if (matches.Length == 0) continue;
                    actualCities.Add(city.SourceCityId);
                    var encounter = matches[0];
                    Need(encounter.SourceCityId == city.SourceCityId && encounter.SourceVisibleEnemyCount == Count(group) &&
                        encounter.BossTransition is null && (encounter.Allies?.Count ?? 0) == 0,
                        "The complete mixed source group retains its original visible member count, city and ordinary single-stage identity.");
                    AssertRawMembers(encounter.Enemies, group, tier);
                }
                Need(actualCities.Order().SequenceEqual(expectedCities) && expectedCities.Length > 0 &&
                    (tier != 25 || actualCities.Count == (SecondChapter(group) ? 56 : 28)),
                    "Complete mixed troops follow actual open city pools; at source25 each chapter-two group has56 cities and each chapter-three group28.");
            }
            foreach (var cohort in Groups.Chunk(2))
            {
                var probe = new ShanheBattleSetup(83, 1, "fixture:mixed-source-dependencies", "classic:guan-yu",
                    4, 0, 0, 0, 0, [], [], [], cohort.SelectMany(group => GeneralIds(group, tier))
                        .Select(id => new ShanheCombatantDefinition(id)).ToArray(), [], "fixture:mixed-source-dependencies")
                    { SourceDifficultyId = tier };
                Need(1 + probe.EnemyRoster.Count <= 8, "Dependency probes retain whole groups inside the real eight-seat limit.");
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
                foreach (var group in cohort) AssertGenerals(registry, group, tier);
            }
        }

        (string Node, string Template, string Terrain)[] sites =
        [
            ("dagu", "dagu", StandardShanheTerrainPackage.RiverId), ("huanyuan", "huanyuan", StandardShanheTerrainPackage.ForestId),
            ("yingyang-terrain", "dagu", StandardShanheTerrainPackage.CityId), ("mengjin-terrain", "huanyuan", StandardShanheTerrainPackage.BattlefieldId),
            ("xiaopingjin-terrain", "dagu", StandardShanheTerrainPackage.RiverId), ("yingchuan-terrain", "huanyuan", StandardShanheTerrainPackage.PlainId),
            ("yique-terrain", "dagu", StandardShanheTerrainPackage.MountainId)
        ];
        foreach (var difficulty in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        {
            var definition = codex
                ? StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var tier = LocalTier(difficulty);
            Need(definition.SourceDifficultyId is null && definition.SourceEnemyGrowth is null,
                "Recommended mixed formations use the local three-tier recipe without borrowing source difficulty growth.");
            foreach (var (id, template, terrain) in sites)
            foreach (var group in Groups.Where(SecondChapter))
            {
                var node = definition.Nodes.Single(n => n.Id == id);
                var roster = GeneralIds(group, tier);
                var encounter = node.Encounters!.Single(e => e.Enemies.Select(m => m.GeneralId).SequenceEqual(roster));
                var original = definition.Nodes.Single(n => n.Id == template).Encounters!
                    .Single(e => e.Id == template + LocalSuffix(group));
                var expectedId = id == template ? original.Id : id + ":" + original.Id;
                Need(node.ChapterNumber == 2 && encounter.Id == expectedId && SameMembers(encounter.Enemies, original.Enemies) &&
                    encounter.TerrainSkillId == (difficulty == 3 ? terrain : null),
                    "Each complete chapter-two formation survives both routes and each terrain clone with its original template identity, complete ordered members and unchanged growth.");
                AssertLocalMembers(encounter, group, difficulty);
            }
            var camp = definition.Nodes.Single(n => n.Id == "finalcamp");
            var patrol = definition.Nodes.Single(n => n.Id == StandardShanheCampaignCatalog.ThirdChapterPatrolNodeId);
            foreach (var group in Groups.Where(g => !SecondChapter(g)))
            {
                var trio = patrol.Encounters!.Single(e => e.Id == patrol.Id + LocalSuffix(group));
                AssertLocalMembers(trio, group, difficulty);
            }
            Need(patrol.Kind == ShanheNodeKind.Battle && patrol.ChapterNumber == 3 && patrol.BossRewards is null &&
                camp.NextNodeIds.Count(id => id == patrol.Id) == 1 &&
                patrol.NextNodeIds.SequenceEqual(camp.NextNodeIds.Where(id => id != patrol.Id)) &&
                new[] { "luoyang", "suanzao", "henei", StandardShanheCampaignCatalog.ThirdChapterDongwuEliteId,
                    StandardShanheCampaignCatalog.MuluEncounterNodeId }.All(patrol.NextNodeIds.Contains) &&
                !patrol.NextNodeIds.Contains(patrol.Id) && !patrol.NextNodeIds.Contains(camp.Id),
                "Adding a complete trio to the optional patrol preserves every prior boss, elite and Mulu route, without a cycle or boss rewards.");
            Need(GeneralIds(2465, tier).Count == 3 && GeneralIds(2464, tier).Count == 3 && GeneralIds(2499, tier).Count == 2 && GeneralIds(2475, tier).Count == 3 && GeneralIds(2485, tier).Count == 3 && GeneralIds(2471, tier).Count == 2,
                "All local tiers retain every source member rather than substituting one leader body.");
        }
    }

    public static void MixedPreviewMoveAndNativeOpeningKeepMembersAndSeparateSkillDraws()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(16);
        foreach (var group in Groups)
        {
            var node = source.Nodes.First(n => n.Kind == ShanheNodeKind.Battle && n.Encounters!.Any(e => e.SourceBattleId == group));
            var encounter = node.Encounters!.Single(e => e.SourceBattleId == group);
            // One actual three-enemy opening also exercises the exact eight-seat boundary.
            EnterAndStart(source, node, encounter, group, 16, allies: group == 2465 ? 4 : group == 2471 ? 5 : 0);
        }
        var local = StandardShanheCampaignCatalog.CreateCodexDefinition(3, StandardShanheCampaignCatalog.Returning2026EditionId);
        foreach (var group in Groups)
        {
            var node = local.Nodes.Single(n => n.Id == (SecondChapter(group)
                ? "dagu" : StandardShanheCampaignCatalog.ThirdChapterPatrolNodeId));
            var encounter = node.Encounters!.Single(e => e.Id == node.Id + LocalSuffix(group));
            EnterAndStart(local, node, encounter, group, 21, localDifficulty: 3);
        }
    }

    private static void EnterAndStart(ShanheCampaignDefinition definition, ShanheNodeDefinition node,
        ShanheEncounterDefinition selected, int group, int tier, int? localDifficulty = null, int allies = 0)
    {
        if (allies > 0) selected = selected with
        {
            Allies = Enumerable.Range(0, allies).Select(_ => new ShanheCombatantDefinition("classic:guan-yu")).ToArray()
        };
        // Keep all original roots, so this fresh later-node probe leaves no unreachable earlier nodes.
        var entry = definition with
        {
            StartingNodeIds = definition.StartingNodeIds.Append(node.Id).Distinct(StringComparer.Ordinal).ToArray(),
            RandomStartingNode = false,
            Nodes = definition.Nodes.Select(n => n.Id == node.Id ? n with { Encounters = [selected] } : n).ToArray()
        };
        var session = ShanheCampaignSession.Create(entry, new(83, "classic:guan-yu")
            { SourceDifficultyId = definition.SourceDifficultyId }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var revision = session.Revision; var ordinal = session.Snapshot.EncounterOrdinal;
        var preview = session.PreviewEncounter(node.Id); var repeated = session.PreviewEncounter(node.Id);
        Need(session.Revision == revision && session.Snapshot.EncounterOrdinal == ordinal && session.PendingBattleSetup is null &&
            preview.Id == selected.Id && SameMembers(preview.Enemies, repeated.Enemies),
            "Repeated preview freezes the full ordered formation without consuming an encounter or changing the session.");
        var moved = session.Submit(new MoveShanheNodeCommand(node.Id, session.Revision));
        Need(moved.Accepted && session.PendingBattleSetup is not null, moved.Error ?? "The complete mixed formation must enter its real native battle.");
        var setup = session.PendingBattleSetup!;
        Need(SameMembers(setup.EnemyRoster, preview.Enemies) && setup.EnemyRoster.Count == Count(group) &&
            setup.EncounterOrdinal == ordinal + 1 && setup.SourceDifficultyId == definition.SourceDifficultyId &&
            setup.BossTransition is null && setup.AllyRoster.Count == allies &&
            1 + setup.EnemyRoster.Count + setup.AllyRoster.Count == (allies > 0 ? 8 : 1 + Count(group)),
            "Move retains every previewed member and the exact full eight-seat boundary without truncating the formation.");
        if (definition.SourceDifficultyId is { } sourceDifficulty)
        {
            AssertRawMembers(selected.Enemies, group, tier);
            for (var i = 0; i < Count(group); i++)
            {
                var grown = StandardShanheSourceEnemyGrowth.Season5.Calculate(new(BaseHp(group, i), 2, 0, 0, 4, 0, 0),
                    new(sourceDifficulty, setup.EncounterOrdinal, node.ChapterNumber, Count(group), ShanheSourceEncounterCategory.Ordinary),
                    ExpectedAttributes(group, i)).Attributes;
                var member = setup.EnemyRoster[i];
                Need(member.Hp == grown.Hp && member.MaxHpBonus == grown.Hp - BaseHp(group, i) &&
                    member.InitialHandBonus == grown.StartingCards - 4 && member.DrawBonus == grown.Draw - 2 &&
                    member.SlashBonus == grown.ExtraSlash && member.InitialArmor == grown.Armor &&
                    member.ArmorRecoveryPerRound == grown.ArmorGain && member.ExtraDamage == grown.ExtraDamage &&
                    preview.SourceBattleId == group && preview.SourceCityId == selected.SourceCityId,
                    "Each original member receives exactly one source projection with the complete frozen visible enemy count and its own tier attributes.");
            }
        }
        else
        {
            Need(localDifficulty is >= 1 and <= 3 && SameMembers(selected.Enemies, preview.Enemies),
                "Local mixed formations retain the authored recipe without a second source growth pass.");
            AssertLocalMembers(selected, group, localDifficulty!.Value);
        }
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        AssertGenerals(registry, group, tier);
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        Need(game.Submit(new StartGameCommand()).Accepted, "The full production formation accepts native StartGame.");
        for (var step = 0; !(game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.ResolutionStack.Count == 0); step++)
            Need(step < 180 && game.CreateSnapshot(0).PendingDecision is null && game.State.Winner == Winner.None &&
                game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted,
                "The real mixed opening must settle its game-start children and stop at the first human Play without advancing enemy turns.");
        var deck = registry.Decks[registry.Modes[ShanheBattleSetup.BattleModeId].DeckId!];
        for (var i = 0; i < Count(group); i++)
        {
            var seat = 1 + setup.AllyRoster.Count + i; var member = setup.EnemyRoster[i];
            var player = game.CreateSnapshot(seat).Players[seat];
            var initial = game.CardMovements.Where(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat) &&
                m.Reason == CardMoveReasons.InitialDeal).Select(m => m.CardId).ToArray();
            var extra = group == 2499 && i == 1 && tier >= 16 ? QiongshouDraw(game, seat) : [];
            Need(player.GeneralId == GeneralIds(group, tier)[i] && player.Hp == member.Hp &&
                player.MaxHp == BaseHp(group, i) + member.MaxHpBonus && player.Armor == member.InitialArmor &&
                player.Equipment.Count == 0 && initial.Length == deck.InitialHandSize + member.InitialHandBonus &&
                initial.Distinct().Count() == initial.Length && !initial.Intersect(extra).Any() &&
                initial.Concat(extra).Order().SequenceEqual(player.Hand.Select(c => c.Id).Order()) &&
                !game.CardMovements.Any(m => m.From == CardLocation.OutsideGame && m.To == CardLocation.Hand(seat)) &&
                ExpectedSkills(group, i, tier).All(id => player.Skills!.Count(s => s.ContentId == id) == 1),
                "Every actual enemy preserves its original seat/body and complete skill set; random initial entities and real skill draws conserve separately, with no fabricated fixed cards or auto-installed gear.");
            if (group == 2465 && i == 2 && tier >= 11) AssertInitialGrain(game, seat);
        }
        Need(game.State.TurnNumber == 1 && game.State.CurrentSeat == 0 && game.State.Winner == Winner.None &&
            game.CreateSnapshot(0).Players.Count == 1 + setup.AllyRoster.Count + Count(group) &&
            game.Events.Select(e => e.Payload).OfType<TurnStartedEvent>().All(e => e.ActorSeat == 0),
            "All members, including an eight-seat source roster, reach the same first natural human Play without simulating later turns or a battle result.");
    }

    private static void AssertRawMembers(IReadOnlyList<ShanheCombatantDefinition> members, int group, int tier)
    {
        Need(members.Count == Count(group), "The original mixed group cannot lose a follower or acquire a hidden extra body.");
        for (var i = 0; i < members.Count; i++)
        {
            var body = members[i];
            Need(body.GeneralId == GeneralIds(group, tier)[i] && body.Hp == BaseHp(group, i) &&
                body is { MaxHpBonus: 0, InitialHandBonus: 0, DrawBonus: 0, SlashBonus: 0, InitialArmor: 0,
                    ArmorRecoveryPerRound: 0, ArmorRecoveryCap: 5, ExtraDamage: 0 } &&
                (body.SourceDifficultyAttributes ?? []).SequenceEqual(ExpectedAttributes(group, i)) && NoGear(body),
                $"Source{group} member{i} preserves its ordered raw body, draw2/hand4 and complete cumulative att fields without local growth or equipment.");
        }
    }

    private static void AssertLocalMembers(ShanheEncounterDefinition encounter, int group, int difficulty)
    {
        var growth = difficulty - 1; var tier = LocalTier(difficulty);
        Need(encounter.SourceBattleId is null && encounter.SourceCityId is null && encounter.SourceVisibleEnemyCount is null &&
            encounter.BossTransition is null && (encounter.Allies?.Count ?? 0) == 0 && encounter.Enemies.Count == Count(group),
            "A recommended local formation retains all ordered enemies without fabricated source labels, allies or a boss transition.");
        for (var i = 0; i < Count(group); i++)
        {
            var member = encounter.Enemies[i];
            var bonus = (SecondChapter(group) ? 3 : 4) + 2 * growth +
                (group == 2464 && i == 2 ? (tier >= 11 ? 1 : 0) + (tier >= 21 ? 1 : 0) : 0);
            var hand = (SecondChapter(group) ? 1 : 2) + growth + (group == 2465 && i == 2
                ? (tier >= 16 ? 1 : 0) + (tier >= 21 ? 2 : 0) : group == 2499 && i == 1 && tier >= 16 ? 2 : group == 2485 && i > 0 ? (tier >= 16 ? 1 : 0) + (tier >= 21 ? 1 : 0) : 0);
            Need(member.GeneralId == GeneralIds(group, tier)[i] && member.MaxHpBonus == bonus &&
                member.Hp == BaseHp(group, i) + bonus && member.InitialHandBonus == hand &&
                member.DrawBonus == (SecondChapter(group) ? 1 : 2) && member.SourceDifficultyAttributes is null &&
                member is { SlashBonus: 0, InitialArmor: 0, ArmorRecoveryPerRound: 0, ArmorRecoveryCap: 0, ExtraDamage: 0 } && NoGear(member) &&
                1 + encounter.Enemies.Count <= 8,
                "Each local member uses its own source base HP plus one explicit local curve and its own cumulative health/hand increments without gear or a source-growth pass.");
        }
    }

    private static void AssertGenerals(ContentRegistry registry, int group, int tier)
    {
        for (var i = 0; i < Count(group); i++)
        {
            var general = registry.Generals[GeneralIds(group, tier)[i]];
            var sourceGeneral = group == 2471 ? (i == 0 ? 580 : 4861) : group == 2475 ? 4861 + i : group == 2485 ? (i == 0 ? 438 : 4860 + i) : group == 2499 ? i == 0 ? 4871 : 4861 : group == 2464 ? 4861 + i : i switch { 0 => 4861, 1 => 4862, _ => 4871 };
            var character = "character:shanhe-source-" + (sourceGeneral == 4871 ? "general-" : "soldier-") + sourceGeneral;
            if (group == 2492) character = Character(i);
            if (group == 2485 && i == 0) character = "character:cao-song";
            if (group == 2471 && i == 0) character = "character:qian-zhao";
            Need(general.CharacterId == character && general.BaseHp == BaseHp(group, i) && general.InitialHp == BaseHp(group, i) &&
                registry.IsGeneralPlayable(general.Id) && general.SkillIds.Order().SequenceEqual(ExpectedSkills(group, i, tier).Order()) &&
                general.SkillIds.All(id => registry.GetSkill(id).Program is not null),
                "Production dependency discovery loads every selected original SourceGeneralId body and all exact cumulative executable skills once.");
        }
    }

    private static string Character(int index) => index switch
    {
        0 => "character:cheng-pu", 1 => "character:han-dang", _ => "character:huang-gai"
    };

    private static IReadOnlyList<int> QiongshouDraw(GameEngine game, int seat)
    {
        const string skill = "ol:qiongshou";
        var start = game.Events.Select(e => e.Payload).OfType<ProgramBindingStartedEvent>()
            .Single(e => e.SkillId == skill && e.OwnerSeat == seat && e.Window == SkillProgramTriggerWindow.GameStarting);
        var draw = game.Events.Select(e => e.Payload).OfType<NativeDrawInvocationRecordedEvent>().Single(e => e.ParentFrameId == start.FrameId);
        var moves = game.CardMovements.Where(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter &&
            m.Reason == draw.Reason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(seat)).ToArray();
        var player = game.CreateSnapshot(seat).Players[seat];
        Need(start.BindingId == "initial-equipment-abolition" && draw.OwnerSeat == seat && draw.Requested == 4 && draw.ActualCount == 4 &&
            draw.TurnNumber == 0 && moves.Length == 4 && moves.Select(m => m.CardId).Distinct().Count() == 4 &&
            moves.All(m => m.Reason != CardMoveReasons.InitialDeal) && player.IsEquipmentAreaAbolished && player.Equipment.Count == 0 &&
            game.Events.Select(e => e.Payload).OfType<PlayerAreasAbolishedEvent>().Count(e => e.Seat == seat && e.Equipment && !e.Judgment) == 1 &&
            game.State.CurrentSeat != seat && player.HandLimit is null &&
            game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e => e.FrameId == start.FrameId && e.Completed) == 1 &&
            !game.ResolutionStack.Any(f => f.Id == start.FrameId),
            "The shared guard's source16-plus Qiongshou genuinely abolishes equipment and completes exactly four separate skill-draw entities at game start.");
        return moves.Select(m => m.CardId).ToArray();
    }

    private static void AssertInitialGrain(GameEngine game, int seat)
    {
        var skill = StandardShanheSourceCoalitionTrioPackage.CangchuSkillId;
        var start = game.Events.Select(e => e.Payload).OfType<ProgramBindingStartedEvent>()
            .Single(e => e.SkillId == skill && e.OwnerSeat == seat && e.Window == SkillProgramTriggerWindow.GameStarting);
        Need(start.BindingId == "initial-three-grain" &&
            game.Events.Select(e => e.Payload).OfType<PlayerMarkerChangedEvent>().Count(e => e.ResolutionId == start.FrameId &&
                e.PlayerSeat == seat && e.SkillOwnerSeat == seat && e.Marker == PlayerMarkerKind.GuanduGrain && e.Delta == 3 && e.Count == 3) == 1 &&
            game.CreateSnapshot(seat).Players[seat].Markers!.Single(m => m.Kind == PlayerMarkerKind.GuanduGrain).Count == 3 &&
            game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e => e.FrameId == start.FrameId && e.Completed) == 1 &&
            !game.ResolutionStack.Any(f => f.Id == start.FrameId),
            "The source11-plus quartermaster receives three real grain markers through its original game-start frame and finishes before first human Play.");
    }

    private static IReadOnlyList<string> GeneralIds(int group, int tier) => group switch
    {
        2499 => StandardShanheSourceYulinOfficerPackage.GetRosterGeneralIds(tier),
        2492 => StandardShanheSourceJiangdongTrioPackage.GetRosterGeneralIds(tier),
        2475 => StandardShanheSourceMilitiaTrioPackage.GetRosterGeneralIds(tier),
        2485 => StandardShanheSourceCaoSongTrioPackage.GetRosterGeneralIds(tier),
        2471 => StandardShanheSourceQianZhaoPairPackage.GetRosterGeneralIds(tier),
        2465 => new[] { StandardShanheSourceCoalitionTrioPackage.GetSpyGeneralId(tier),
            StandardShanheSourceCoalitionTrioPackage.GetScoutGeneralId(tier), StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(tier) },
        2464 => new[] { StandardShanheSourceDemonAssassinPackage.GetGeneralId(tier),
            StandardShanheSourceDemonArcherPackage.GetTrioGeneralId(tier), StandardShanheSourceDemonRaiderPackage.GetTrioGeneralId(tier) },
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static string[] ExpectedSkills(int group, int index, int tier)
    {
        if (group == 2471)
            return index == 0
                ? new[] { StandardShanheSourceQianZhaoPairPackage.WeifuSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceQianZhaoPairPackage.KuansaiSkillId] : Array.Empty<string>()).ToArray()
                : (tier >= 11 ? new[] { StandardShanheSourceQianZhaoPairPackage.XiaosiSkillId } : Array.Empty<string>())
                    .Concat(tier >= 16 ? [StandardShanheSourceQianZhaoPairPackage.YuanchouSkillId] : Array.Empty<string>()).ToArray();
        if (group == 2475)
            return index switch
            {
                0 => (tier >= 11 ? new[] { StandardShanheSourceMilitiaTrioPackage.SujianSkillId } : Array.Empty<string>())
                    .Concat(tier >= 21 ? [StandardShanheSourceMilitiaTrioPackage.YingyuanSkillId] : Array.Empty<string>()).ToArray(),
                1 => new[] { StandardShanheSourceMilitiaTrioPackage.QingjianSkillId }
                    .Concat(tier >= 16 ? [StandardShanheSourceMilitiaTrioPackage.JushouSkillId] : Array.Empty<string>()).ToArray(),
                _ => (tier >= 11 ? new[] { StandardShanheSourceMilitiaTrioPackage.YuejianSkillId } : Array.Empty<string>())
                    .Concat(tier >= 16 ? [StandardShanheSourceMilitiaTrioPackage.ZhuhaiSkillId] : Array.Empty<string>()).ToArray()
            };
        if (group == 2485)
            return index == 0
                ? new[] { StandardShanheSourceCaoSongTrioPackage.YizhengSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceCaoSongTrioPackage.WeizhongSkillId] : Array.Empty<string>())
                    .Concat(tier >= 21 ? [StandardShanheSourceCaoSongTrioPackage.LongweiSkillId] : Array.Empty<string>()).ToArray()
                : tier >= 11 ? [StandardShanheSourceCaoSongTrioPackage.ChuanyunSkillId] : [];
        if (group == 2492)
            return index switch
            {
                0 => new[] { StandardShanheSourceJiangdongTrioPackage.LihuoSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceJiangdongTrioPackage.ChunlaoSkillId] : Array.Empty<string>())
                    .Concat(tier >= 21 ? ["classic:qixi"] : Array.Empty<string>()).ToArray(),
                1 => new[] { "classic:gongqi" }.Concat(tier >= 11 ? ["classic:jiefan"] : Array.Empty<string>())
                    .Concat(tier >= 16 ? ["classic:anjian"] : Array.Empty<string>()).ToArray(),
                _ => new[] { "boundary:kurou", "boundary:zhaxiang" }.Concat(tier >= 11 ? ["classic:qiangxi"] : Array.Empty<string>()).ToArray()
            };
        if (group == 2499)
            return index == 0
                ? new[] { "ol:kuangshou" }.Concat(tier >= 11 ? [StandardShanheSourceKeshouPackage.SkillId] : Array.Empty<string>())
                    .Concat(tier >= 21 ? ["ol:weijing"] : Array.Empty<string>()).ToArray()
                : new[] { StandardShanheSourceYulinGuardPackage.JushouSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceKeshouPackage.SkillId] : Array.Empty<string>())
                    .Concat(tier >= 16 ? ["ol:qiongshou"] : Array.Empty<string>()).ToArray();
        if (group == 2464)
            return index switch
            {
                0 => new[] { StandardShanheSourceDemonAssassinPackage.QianxiSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceDemonAssassinPackage.YoudiSkillId] : Array.Empty<string>())
                    .Concat(tier >= 21 ? [StandardShanheSourceDemonAssassinPackage.YiduSkillId] : Array.Empty<string>()).ToArray(),
                1 => new[] { StandardShanheSourceDemonArcherPackage.GongqiSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceDemonArcherPackage.YouyanSkillId] : Array.Empty<string>())
                    .Concat(tier >= 16 ? [StandardShanheSourceDemonArcherPackage.SaoguSkillId] : Array.Empty<string>()).ToArray(),
                _ => new[] { StandardShanheSourceDemonRaiderPackage.QiangxiSkillId }
                    .Concat(tier >= 11 ? [StandardShanheSourceDemonRaiderPackage.JielueSkillId] : Array.Empty<string>())
                    .Concat(tier >= 21 ? [StandardShanheSourceDemonRaiderPackage.LangdaoSkillId] : Array.Empty<string>()).ToArray()
            };
        return index switch
        {
            0 => new[] { "classic:qice" }.Concat(tier >= 11 ? ["boundary:tishen"] : Array.Empty<string>())
                .Concat(tier >= 16 ? ["mobile:duanfa"] : Array.Empty<string>()).ToArray(),
            1 => new[] { "classic:dangxian" }.Concat(tier >= 11 ? ["classic:kanpo"] : Array.Empty<string>())
                .Concat(tier >= 21 ? ["ol:dianhu"] : Array.Empty<string>()).ToArray(),
            _ => new[] { "classic:duliang" }.Concat(tier >= 11
                ? new[] { StandardShanheSourceCoalitionTrioPackage.CangchuSkillId, StandardShanheSourceCoalitionTrioPackage.SushouSkillId }
                : Array.Empty<string>()).ToArray()
        };
    }
    private static IReadOnlyList<ShanheSourceEnemyTierAttributes> ExpectedAttributes(int group, int index) =>
        group == 2465 && index == 2 ? new[] { new ShanheSourceEnemyTierAttributes(4, new(StartingCards: 1)), new(5, new(StartingCards: 2)) }
        : group == 2499 && index == 1 ? new[] { new ShanheSourceEnemyTierAttributes(4, new(StartingCards: 2)) }
        : group == 2485 && index > 0 ? new[] { new ShanheSourceEnemyTierAttributes(4, new(StartingCards: 1)), new(5, new(StartingCards: 1)) }
        : group == 2464 && index == 2 ? new[] { new ShanheSourceEnemyTierAttributes(3, new(Hp: 1)), new(5, new(Hp: 1)) } : [];
    private static int Count(int group) => group is 2499 or 2471 ? 2 : 3;
    private static int BaseHp(int group, int index) => group switch
    {
        2499 => index == 0 ? 5 : 4,
        2465 => index == 1 ? 4 : 3,
        2464 => index == 0 ? 3 : 4,
        2492 or 2475 => 4,
        2485 => index == 0 ? 6 : 2,
        2471 => index == 0 ? 4 : 2,
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static string LocalSuffix(int group) => group switch
    {
        2499 => ":yulin-officer-2499-local", 2465 => ":coalition-trio-2465-local", 2464 => ":demon-trio-2464-local", 2492 => ":jiangdong-trio-2492-local",
        2475 => ":militia-trio-2475-local", 2485 => ":caosong-trio-2485-local", 2471 => ":qianzhao-pair-2471-local",
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static int LocalTier(int difficulty) => difficulty switch { 1 => 1, 2 => 11, 3 => 21, _ => throw new ArgumentOutOfRangeException(nameof(difficulty)) };
    private static bool NoGear(ShanheCombatantDefinition member) => member.StartingHand is null &&
        (member.StartingCards?.Count ?? 0) == 0 && (member.StartingEquipment?.Count ?? 0) == 0 &&
        (member.ExtraSkillIds?.Count ?? 0) == 0 && (member.QualifiedPrintedLordSkillIds?.Count ?? 0) == 0;
    private static bool SameMembers(IReadOnlyList<ShanheCombatantDefinition> left, IReadOnlyList<ShanheCombatantDefinition> right) =>
        left.Count == right.Count && left.Zip(right).All(p => p.First.GeneralId == p.Second.GeneralId && p.First.Hp == p.Second.Hp &&
            p.First.MaxHpBonus == p.Second.MaxHpBonus && p.First.InitialHandBonus == p.Second.InitialHandBonus &&
            p.First.DrawBonus == p.Second.DrawBonus && p.First.SlashBonus == p.Second.SlashBonus &&
            p.First.InitialArmor == p.Second.InitialArmor && p.First.ArmorRecoveryPerRound == p.Second.ArmorRecoveryPerRound &&
            p.First.ArmorRecoveryCap == p.Second.ArmorRecoveryCap && p.First.ExtraDamage == p.Second.ExtraDamage &&
            (p.First.SourceDifficultyAttributes ?? []).SequenceEqual(p.Second.SourceDifficultyAttributes ?? []) && NoGear(p.First) && NoGear(p.Second));
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
