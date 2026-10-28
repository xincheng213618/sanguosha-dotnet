using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceOrdinaryTroopChecks
{
    private static readonly int[] Groups = [2474, 2467, 2487, 2473, 2480, 2497, 2455, 2459, 2460, 2476, 2489, 2493, 2495, 2457, 2498, 2451, 2500, 2454, 2456, 2458, 2461, 2452, 2468, 2494, 2453, 2469, 2481, 2466, 2484, 2486, 2462, 2470, 2479, 2478];
    private static bool FirstChapter(int group) => group is 2473 or 2455 or 2459 or 2460 or 2457 or 2498 or 2454 or 2456 or 2458 or 2461 or 2452 or 2453 or 2466 or 2484 or 2486 or 2470 or 2479;
    private static bool ThirdChapter(int group) => group is 2493 or 2495 or 2468 or 2478;

    public static void SourcePoolsAndBothRecommendedEntrypointsRetainCompleteTieredRecipes()
    {
        var graph = StandardShanheSourceRouteCatalog.Season5;
        foreach (var difficulty in new[] { 1, 6, 11, 16, 21, 25 })
        {
            var definition = StandardShanheCampaignCatalog.CreateSourceDefinition(difficulty);
            var cities = graph.Regions.Where(r => r.MinimumSourceDifficultyId <= difficulty)
                .SelectMany(r => r.CityIds).Select(id => graph.ByCityId[id]).Where(c => !c.IsBoss).ToArray();
            foreach (var city in cities)
            {
                var node = definition.Nodes.Single(n => n.Id == StandardShanheCampaignCatalog.Source2026CityNodeId(city.SourceCityId));
                Need(node.Kind == ShanheNodeKind.Battle &&
                    node.Encounters!.Count(e => e.SourceBattleId is null) == 4,
                    "Each open original ordinary city retains all four explicitly local fallback recipes.");
                var potential = graph.GetPotentialBattleIds(city.SourceCityId);
                foreach (var group in Groups)
                {
                    var matches = node.Encounters!.Where(e => e.SourceBattleId == group).ToArray();
                    Need(matches.Length == (potential.Contains(group) ? 1 : 0),
                        $"Source{group} must appear exactly once only in the actual open city pool; difficulty={difficulty}, city={city.SourceCityId}.");
                    if (matches.Length == 0) continue;
                    var encounter = matches[0]; var body = encounter.Enemies.Single();
                    var baseHp = SourceBaseHp(group);
                    Need(encounter.SourceCityId == city.SourceCityId && encounter.SourceVisibleEnemyCount == 1 &&
                        encounter.BossTransition is null && (encounter.Allies?.Count ?? 0) == 0 &&
                        body.GeneralId == General(group, difficulty) && body.Hp == baseHp &&
                        body is { MaxHpBonus: 0, DrawBonus: 0, InitialHandBonus: 0, SlashBonus: 0,
                            InitialArmor: 0, ArmorRecoveryPerRound: 0, ArmorRecoveryCap: 5, ExtraDamage: 0 } &&
                        (body.SourceDifficultyAttributes ?? []).SequenceEqual(ExpectedSourceAttributes(group)) && body.StartingHand is null &&
                        (body.StartingCards ?? []).SequenceEqual(ExpectedStartingCards(group, difficulty)) &&
                        (body.StartingEquipment?.Count ?? 0) == 0,
                        $"Source{group} retains its complete original{baseHp}/{baseHp}, draw2/hand4 member and exact extra hand-card recipe without auto-equipped gear or a second local chapter bonus; source growth belongs to entry projection.");
                }
            }
            foreach (var group in Groups)
            {
                var expected = cities.Where(c => graph.GetPotentialBattleIds(c.SourceCityId).Contains(group))
                    .Select(c => c.SourceCityId).Order().ToArray();
                var actual = definition.Nodes.Where(n => n.Kind == ShanheNodeKind.Battle)
                    .SelectMany(n => n.Encounters!).Where(e => e.SourceBattleId == group)
                    .Select(e => e.SourceCityId!.Value).Order().ToArray();
                Need(actual.SequenceEqual(expected) && expected.Length > 0,
                    "The total recipe count follows currently open source regions, rather than prematurely requiring all56 cities at every tier.");
                if (difficulty == 25)
                    Need(actual.Length == (FirstChapter(group) ? 14 : ThirdChapter(group) ? 28 : 56),
                        "With every source region open, first/second/third-chapter troops retain14/56/28 actual source-city memberships respectively.");
            }
            foreach (var cohort in Groups.Chunk(7))
            {
                var probe = new ShanheBattleSetup(83, 1, "fixture:ordinary-source-dependencies", "classic:guan-yu",
                    4, 0, 0, 0, 0, [], [], [], cohort.Select(group => new ShanheCombatantDefinition(General(group, difficulty))
                        { StartingCards = ExpectedStartingCards(group, difficulty) }).ToArray(),
                    [], "fixture:ordinary-source-dependencies") { SourceDifficultyId = difficulty };
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
                foreach (var group in cohort) AssertGeneralAndPrograms(registry, group, difficulty);
            }
        }

        (string Node, string Terrain)[] sites =
        [
            ("dagu", StandardShanheTerrainPackage.RiverId), ("huanyuan", StandardShanheTerrainPackage.ForestId),
            ("yingyang-terrain", StandardShanheTerrainPackage.CityId), ("mengjin-terrain", StandardShanheTerrainPackage.BattlefieldId),
            ("xiaopingjin-terrain", StandardShanheTerrainPackage.RiverId), ("yingchuan-terrain", StandardShanheTerrainPackage.PlainId),
            ("yique-terrain", StandardShanheTerrainPackage.MountainId)
        ];
        foreach (var difficulty in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        {
            var definition = codex
                ? StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var growth = difficulty - 1;
            Need(definition.SourceDifficultyId is null && definition.SourceEnemyGrowth is null,
                "Both recommended entrypoints retain their local three-tier contract.");
            foreach (var (id, terrain) in sites)
            {
                var node = definition.Nodes.Single(n => n.Id == id);
                foreach (var group in Groups.Where(group => !FirstChapter(group) && !ThirdChapter(group)))
                {
                    var tier = LocalTier(group, difficulty);
                    var encounter = node.Encounters!.Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == General(group, tier));
                    var body = encounter.Enemies.Single();
                    Need(encounter.SourceBattleId is null && encounter.SourceCityId is null && encounter.SourceVisibleEnemyCount is null &&
                        encounter.BossTransition is null && (encounter.Allies?.Count ?? 0) == 0 &&
                        encounter.TerrainSkillId == (difficulty == 3 ? terrain : null) &&
                        body.Hp == 7 + 2 * growth && body.MaxHpBonus == 7 + 2 * growth - SourceBaseHp(group) &&
                        body.InitialHandBonus == 1 + growth && body.DrawBonus == 1 && body.SourceDifficultyAttributes is null &&
                        (body.StartingCards ?? []).SequenceEqual(ExpectedStartingCards(group, tier)) &&
                        (body.StartingEquipment?.Count ?? 0) == 0 &&
                        1 + encounter.Enemies.Count + (encounter.Allies?.Count ?? 0) <= 8,
                        "All complete second-chapter local troop recipes survive both normal nodes and all five terrain clones, retaining each original body's own maximum HP, exact extra hand cards and no fabricated source identities.");
                }
            }
            foreach (var id in new[] { "hulao", "guangcheng", "hulao-start", "guangcheng-start" })
            foreach (var group in Groups.Where(FirstChapter))
            {
                var node = definition.Nodes.Single(n => n.Id == id);
                var encounter = node.Encounters!.Single(e => e.Enemies.Count == 1 &&
                    e.Enemies[0].GeneralId == General(group, LocalTier(group, difficulty)));
                var body = encounter.Enemies.Single();
                Need(node.ChapterNumber == 1 && encounter.SourceBattleId is null && encounter.SourceCityId is null &&
                    encounter.SourceVisibleEnemyCount is null && encounter.BossTransition is null &&
                    (encounter.Allies?.Count ?? 0) == 0 && body.Hp == LocalFirstChapterHp(group, difficulty) &&
                    body.MaxHpBonus == LocalFirstChapterHp(group, difficulty) - SourceBaseHp(group) &&
                    body.DrawBonus == difficulty && body.InitialHandBonus == LocalFirstChapterHand(group, difficulty) && body.SourceDifficultyAttributes is null &&
                    body.StartingHand is null && (body.StartingCards?.Count ?? 0) == 0 &&
                    (body.StartingEquipment?.Count ?? 0) == 0 && 1 + encounter.Enemies.Count <= 8,
                    "Every complete local first-chapter troop survives both routes and opening copies, retaining only its own cumulative source health and hand increments without second-chapter bonuses or source labels.");
                if (id.EndsWith("-start", StringComparison.Ordinal))
                {
                    var original = definition.Nodes.Single(n => n.Id == id[..^6]).Encounters!
                        .Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == body.GeneralId);
                    Need(SameMembers(encounter.Enemies, original.Enemies),
                        "Random-opening copies retain the actual source-body recipe rather than adding another local growth pass.");
                }
            }
            var camp = definition.Nodes.Single(n => n.Id == "finalcamp");
            var patrol = definition.Nodes.Single(n => n.Id == StandardShanheCampaignCatalog.ThirdChapterPatrolNodeId);
            var originalRoutes = camp.NextNodeIds.Where(id => id != patrol.Id).ToArray();
            Need(patrol is { Kind: ShanheNodeKind.Battle, ChapterNumber: 3, BossRewards: null } &&
                camp.NextNodeIds.Count(id => id == patrol.Id) == 1 && patrol.NextNodeIds.SequenceEqual(originalRoutes) &&
                new[] { "luoyang", "suanzao", "henei", StandardShanheCampaignCatalog.ThirdChapterDongwuEliteId,
                    StandardShanheCampaignCatalog.MuluEncounterNodeId }.All(originalRoutes.Contains) &&
                !patrol.NextNodeIds.Contains(camp.Id) && !patrol.NextNodeIds.Contains(patrol.Id) &&
                patrol.Encounters is { Count: 7 },
                "The optional third-chapter ordinary patrol preserves every existing final route, elite and Mulu detour without a cycle or a chapter-boss reward contract.");
            foreach (var group in Groups.Where(ThirdChapter))
            {
                var tier = LocalTier(group, difficulty);
                var encounter = patrol.Encounters!.Single(e => e.Enemies.Count == 1 &&
                    e.Enemies[0].GeneralId == General(group, tier));
                var body = encounter.Enemies.Single();
                Need(encounter.SourceBattleId is null && encounter.SourceCityId is null &&
                    encounter.SourceVisibleEnemyCount is null && encounter.BossTransition is null &&
                    (encounter.Allies?.Count ?? 0) == 0 && body.Hp == 10 + 3 * growth &&
                    body.MaxHpBonus == 10 + 3 * growth - SourceBaseHp(group) &&
                    body.InitialHandBonus == LocalThirdChapterHand(group, difficulty) && body.DrawBonus == 2 &&
                    body.SlashBonus == LocalThirdChapterSlash(group, tier) && body.SourceDifficultyAttributes is null &&
                    body.StartingHand is null && (body.StartingEquipment?.Count ?? 0) == 0 &&
                    (body.StartingCards ?? []).SequenceEqual(ExpectedStartingCards(group, tier)) &&
                    1 + encounter.Enemies.Count <= 8,
                    "Both recommended entrypoints retain complete authored third-chapter troops, cumulative source Slash deltas and the high-tier held mirror, without fake source labels or auto-equipped gear.");
            }
            PreviewAndMovePatrolFromFreshCamp(definition, camp, patrol);
        }
    }

    public static void SourceAndLocalPreviewMoveAndStartUseOneProjectionAndRealOpeningEntities()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(25);
        foreach (var group in new[] { 2474, 2480, 2497, 2455, 2459, 2489, 2493, 2495, 2498, 2456, 2458, 2461, 2452, 2468, 2494, 2453, 2469, 2481, 2466, 2484, 2486, 2462, 2470, 2479, 2478 })
        {
            var cityNode = source.Nodes.First(n => n.Kind == ShanheNodeKind.Battle && n.Encounters!.Any(e => e.SourceBattleId == group));
            var sourceEncounter = cityNode.Encounters!.Single(e => e.SourceBattleId == group);
            EnterAndStart(source, cityNode, sourceEncounter, group, 25);
        }
        var nightmare = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(16);
        var nightmareCity = nightmare.Nodes.First(n => n.Kind == ShanheNodeKind.Battle &&
            n.Encounters!.Any(e => e.SourceBattleId == 2498));
        EnterAndStart(nightmare, nightmareCity,
            nightmareCity.Encounters!.Single(e => e.SourceBattleId == 2498), 2498, 16);

        var local = StandardShanheCampaignCatalog.CreateCodexDefinition(3, StandardShanheCampaignCatalog.Returning2026EditionId);
        var localNode = local.Nodes.Single(n => n.Id == "dagu");
        foreach (var group in new[] { 2474, 2480, 2497, 2476, 2494, 2469, 2481, 2462 })
        {
            var tier = LocalTier(group, 3);
            var localEncounter = localNode.Encounters!.Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == General(group, tier));
            EnterAndStart(local, localNode, localEncounter, group, tier, localDifficulty: 3);
        }
        var opening = local.Nodes.Single(n => n.Id == "hulao-start");
        var raider = opening.Encounters!.Single(e => e.Enemies.Count == 1 &&
            e.Enemies[0].GeneralId == StandardShanheSourceQiangRaiderPackage.NightmareId);
        EnterAndStart(local, opening, raider, 2473, 16, localDifficulty: 3);
        var spy = opening.Encounters!.Single(e => e.Enemies.Count == 1 &&
            e.Enemies[0].GeneralId == StandardShanheSourceCoalitionSpyPackage.NightmareId);
        EnterAndStart(local, opening, spy, 2459, 21, localDifficulty: 3);
        var guard = opening.Encounters!.Single(e => e.Enemies.Count == 1 &&
            e.Enemies[0].GeneralId == StandardShanheSourceYulinGuardPackage.HellId);
        EnterAndStart(local, opening, guard, 2498, 21, localDifficulty: 3);
        foreach (var group in new[] { 2456, 2458, 2461, 2452, 2453, 2466, 2484, 2486, 2470, 2479 })
        {
            var tier = LocalTier(group, 3);
            var encounter = opening.Encounters!.Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == General(group, tier));
            EnterAndStart(local, opening, encounter, group, tier, localDifficulty: 3);
        }
        var patrol = local.Nodes.Single(n => n.Id == StandardShanheCampaignCatalog.ThirdChapterPatrolNodeId);
        foreach (var group in Groups.Where(ThirdChapter))
        {
            var tier = LocalTier(group, 3);
            var encounter = patrol.Encounters!.Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == General(group, tier));
            EnterAndStart(local, patrol, encounter, group, tier, localDifficulty: 3);
        }
    }

    private static void EnterAndStart(ShanheCampaignDefinition definition, ShanheNodeDefinition node,
        ShanheEncounterDefinition selected, int group, int tier, int? localDifficulty = null)
    {
        // Admit a real later node as a fresh entry without manufacturing any wins.
        var entry = definition with
        {
            StartingNodeIds = definition.StartingNodeIds.Append(node.Id).Distinct(StringComparer.Ordinal).ToArray(),
            RandomStartingNode = false,
            Nodes = definition.Nodes.Select(n => n.Id == node.Id ? n with { Encounters = [selected] } : n).ToArray()
        };
        var session = ShanheCampaignSession.Create(entry, new(83, "classic:guan-yu")
            { SourceDifficultyId = definition.SourceDifficultyId }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var revision = session.Revision; var ordinal = session.Snapshot.EncounterOrdinal;
        var preview = session.PreviewEncounter(node.Id);
        var repeated = session.PreviewEncounter(node.Id);
        Need(session.Revision == revision && session.Snapshot.EncounterOrdinal == ordinal && session.PendingBattleSetup is null &&
            preview.Id == selected.Id && SameMembers(repeated.Enemies, preview.Enemies),
            "Repeated real preview consumes neither a move nor an encounter and preserves the same prepared member.");
        var result = session.Submit(new MoveShanheNodeCommand(node.Id, session.Revision));
        Need(result.Accepted && session.PendingBattleSetup is not null, result.Error ?? "Cannot enter the actual ordinary troop recipe.");
        var setup = session.PendingBattleSetup!; var member = setup.EnemyRoster.Single();
        var baseHp = SourceBaseHp(group);
        Need(SameMembers(setup.EnemyRoster, preview.Enemies) && setup.SourceDifficultyId == definition.SourceDifficultyId &&
            setup.EncounterOrdinal == ordinal + 1 && setup.BossTransition is null,
            "Move carries exactly the already-previewed troop and one actual encounter ordinal into its native battle.");
        if (definition.SourceDifficultyId is { } sourceDifficulty)
        {
            var raw = selected.Enemies.Single();
            var grown = StandardShanheSourceEnemyGrowth.Season5.Calculate(new(baseHp, 2, 0, 0, 4, 0, 0),
                new(sourceDifficulty, setup.EncounterOrdinal, node.ChapterNumber, 1, ShanheSourceEncounterCategory.Ordinary),
                raw.SourceDifficultyAttributes).Attributes;
            Need(raw.Hp == baseHp && raw is { MaxHpBonus: 0, InitialHandBonus: 0, DrawBonus: 0 } &&
                member.Hp == grown.Hp && member.MaxHpBonus == grown.Hp - baseHp &&
                member.InitialHandBonus == grown.StartingCards - 4 && member.DrawBonus == grown.Draw - 2 &&
                member.SlashBonus == grown.ExtraSlash && member.InitialArmor == grown.Armor &&
                member.ArmorRecoveryPerRound == grown.ArmorGain && member.ExtraDamage == grown.ExtraDamage &&
                preview.SourceBattleId == group && preview.SourceCityId == selected.SourceCityId,
                "Only session entry projects the untouched original body, draw2/hand4 member with the real day, chapter, single visible enemy and actual source-difficulty calculator.");
        }
        else
        {
            Need(localDifficulty is >= 1 and <= 3, "A local opening retains its explicit local difficulty.");
            var difficulty = localDifficulty!.Value; var growth = difficulty - 1;
            var hp = FirstChapter(group) ? LocalFirstChapterHp(group, difficulty) : ThirdChapter(group) ? 10 + 3 * growth : 7 + 2 * growth;
            Need(member.Hp == hp && member.MaxHpBonus == hp - baseHp &&
                member.InitialHandBonus == (FirstChapter(group) ? LocalFirstChapterHand(group, difficulty) : ThirdChapter(group) ? LocalThirdChapterHand(group, difficulty) : 1 + growth) &&
                member.DrawBonus == (FirstChapter(group) ? difficulty : ThirdChapter(group) ? 2 : 1) &&
                (!ThirdChapter(group) || member.SlashBonus == LocalThirdChapterSlash(group, tier)) && SameMembers([member], selected.Enemies) &&
                member.SourceDifficultyAttributes is null && preview.SourceBattleId is null && preview.SourceCityId is null,
                "Recommended local troops keep their authored chapter health, base-specific maximum HP and draw/hand bonuses without a source-growth pass.");
        }

        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        AssertGeneralAndPrograms(registry, group, tier);
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        Need(game.Submit(new StartGameCommand()).Accepted, "The exact production troop setup accepts native StartGame.");
        for (var step = 0; !(game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                game.ResolutionStack.Count == 0); step++)
            Need(step < 120 && game.CreateSnapshot(0).PendingDecision is null && game.State.Winner == Winner.None &&
                game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted,
                "The ordinary troop must reach the first natural human Play without simulating enemy turns or inventing a result.");
        var enemySeat = 1 + setup.AllyRoster.Count;
        var enemy = game.CreateSnapshot(enemySeat).Players.Single(p => p.Seat == enemySeat);
        var deck = registry.Decks[registry.Modes[ShanheBattleSetup.BattleModeId].DeckId!];
        var initial = game.CardMovements.Where(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(enemySeat) &&
            m.Reason == CardMoveReasons.InitialDeal).ToArray();
        var extraHand = game.CardMovements.Where(m => m.From == CardLocation.OutsideGame &&
            m.To == CardLocation.Hand(enemySeat) && m.Reason == CardMoveReasons.InitialDeal).ToArray();
        var expectedCards = ExpectedStartingCards(group, tier);
        var openingCosts = ExactOpeningZhenduCosts(game, setup, group, enemySeat);
        var skillOpeningDraw = ExactQiongshouOpeningDraw(game, group, tier, enemySeat);
        Need(game.State.TurnNumber == 1 && game.State.CurrentSeat == 0 && game.State.Winner == Winner.None &&
            game.CreateSnapshot(0).Players.Count == 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count &&
            game.CreateSnapshot(0).Players.Count <= 8 && enemy.GeneralId == member.GeneralId &&
            enemy.Hp == member.Hp && enemy.MaxHp == baseHp + member.MaxHpBonus && enemy.Armor == member.InitialArmor &&
            enemy.Equipment.Count == 0 && initial.Length == deck.InitialHandSize + member.InitialHandBonus &&
            initial.Select(m => m.CardId).Distinct().Count() == initial.Length &&
            extraHand.Length == expectedCards.Count && extraHand.Select(m => m.CardId).Distinct().Count() == extraHand.Length &&
            !initial.Select(m => m.CardId).Intersect(extraHand.Select(m => m.CardId)).Any() &&
            extraHand.Select(m => enemy.Hand.Single(c => c.Id == m.CardId)).Select(c => (c.Kind, c.Suit, c.Rank)).Order()
                .SequenceEqual(expectedCards.Select(c => (c.Kind, c.Suit, c.Rank)).Order()) &&
            initial.Concat(extraHand).Where(m => !openingCosts.Contains(m.CardId)).Select(m => m.CardId).Concat(skillOpeningDraw).Order()
                .SequenceEqual(enemy.Hand.Select(c => c.Id).Order()) &&
            ExpectedSkills(group, tier).All(id => enemy.Skills!.Count(s => s.ContentId == id) == 1) &&
            (group != 2497 || enemy.Skills!.All(s => s.ContentId != StandardShanheZhaoguMirrorPackage.SkillId)) &&
            (group != 2495 || enemy.Skills!.All(s => s.ContentId != StandardShanheHuxinMirrorPackage.SkillId)) &&
            game.Events.Where(e => e.Payload is TurnStartedEvent).All(e => ((TurnStartedEvent)e.Payload).ActorSeat == 0),
            "The real first opening uses the prepared health and body's maximum once, conserves all random and extra starting entities separately from exact Zhendu payment and Qiongshou's four-card draw, installs cumulative printed skills once without auto-equipping held gear, and stops before an enemy turn.");
        if (group == 2456 && tier >= 11)
        {
            var start = game.Events.Select(e => e.Payload).OfType<ProgramBindingStartedEvent>().Single(e =>
                e.SkillId == StandardShanheSourceCoalitionTrioPackage.CangchuSkillId && e.OwnerSeat == enemySeat &&
                e.Window == SkillProgramTriggerWindow.GameStarting);
            Need(start.BindingId == "initial-three-grain" &&
                game.Events.Select(e => e.Payload).OfType<PlayerMarkerChangedEvent>().Count(e => e.ResolutionId == start.FrameId &&
                    e.PlayerSeat == enemySeat && e.SkillOwnerSeat == enemySeat && e.Marker == PlayerMarkerKind.GuanduGrain && e.Delta == 3 && e.Count == 3) == 1 &&
                enemy.Markers!.Single(m => m.Kind == PlayerMarkerKind.GuanduGrain).Count == 3 &&
                game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e => e.FrameId == start.FrameId && e.Completed) == 1 &&
                !game.ResolutionStack.Any(f => f.Id == start.FrameId),
                "The independent2456 opening completes the shared exact Cangchu game-start frame and three grain once, separately from its cumulative initial-hand entities.");
        }
    }

    private static void AssertGeneralAndPrograms(ContentRegistry registry, int group, int difficulty)
    {
        var general = registry.Generals[General(group, difficulty)];
        var expected = ExpectedSkills(group, difficulty);
        var baseHp = SourceBaseHp(group);
        Need(general.BaseHp == baseHp && general.InitialHp == baseHp && registry.IsGeneralPlayable(general.Id) &&
            general.SkillIds.Order().SequenceEqual(expected.Order()) &&
            expected.All(id => registry.GetSkill(id).Program is not null ||
                id == StandardShanheSourceLiuSandaoPackage.NuzhanSkillId &&
                registry.GetSkill(id) is { ExecutionForms: SkillExecutionForm.State, Tags: SkillTag.Locked }),
            "The actual factory loads every selected executable source skill; Nuzhan retains its existing locked native State definition, whose real conversions have focused behavior coverage.");
        if (group is 2457 or 2498 or 2451 or 2500 or 2456 or 2458 or 2461)
            Need(general.CharacterId == (group is 2451 or 2500 or 2456
                    ? "character:shanhe-source-general-4871" : "character:shanhe-source-soldier-4861"),
                "The new complete source groups retain their actual soldier4861 or general4871 bodies instead of adopting a same-name printed general.");
        if (group is 2452 or 2468 or 2494)
            Need(general.CharacterId == (group == 2452 ? "character:ma-mi-di" : group == 2468 ? "character:zhang-he" : "character:shanhe-source-soldier-4861"),
                "The source520/357 characters and soldier4861 retain their own original bodies and selected cumulative skills.");
        if (group is 2453 or 2469 or 2481)
            Need(general.CharacterId == (group == 2469 ? "character:cai-wen-ji" : "character:shanhe-source-soldier-4861"),
                "The source505 Cai Wenji and original4861 elephant/ranger bodies retain their exact identity and full selected tier.");
        if (group is 2466 or 2484 or 2486 or 2462)
            Need(general.CharacterId == (group == 2484 ? "character:gao-gan" : group is 2486 or 2462
                    ? "character:shanhe-source-general-4871" : "character:shanhe-source-soldier-4861"),
                "Youyan, Gao Gan and the armory commander retain their actual4861/482/4871 bodies and cumulative skills.");
        if (group == 2478) Need(general.CharacterId == "character:shanhe-source-general-4871", "Wu Cong retains the source4871 officer body.");
        if (group is 2470 or 2479)
            Need(general.CharacterId == (group == 2470 ? "character:zuo-ci" : "character:shanhe-source-general-4871"),
                "Zuo Ci keeps source447 while Liu Sandao keeps source4871; neither imports another general's attached abilities.");
        if (group == 2474 && difficulty >= 21)
            Need(registry.Skills.Values.Count(s => s.Id == StandardShanheCampaignCatalog.S5ZaiqiSkillId) == 1 &&
                general.SkillIds.Single(id => id == StandardShanheCampaignCatalog.S5ZaiqiSkillId) ==
                    StandardShanheSourceVeteranPackage.ZaiqiSkillId,
                "Veteran and catalogue dependency discovery share the same existing S5再起 identity without missing or duplicate registration.");
        if (group == 2497)
        {
            Need(registry.Skills.Values.Count(s => s.Id == StandardShanheSourceJiangdongVeteranPackage.FenliSkillId) == 1 &&
                registry.GetSkill(StandardShanheSourceJiangdongVeteranPackage.FenliSkillId).Program is not null,
                "The source package imports exactly one executable existing Fenli definition rather than an unrelated full activity package.");
            if (difficulty >= 21)
                Need(registry.Cards.Values.Count(c => c.LegacyKind == CardKind.ZhaoguMirror) == 1 &&
                    registry.Cards[StandardShanheZhaoguMirrorPackage.CardId].LegacyKind == CardKind.ZhaoguMirror &&
                    registry.Skills.Values.Count(s => s.Id == StandardShanheZhaoguMirrorPackage.SkillId) == 1 &&
                    registry.GetSkill(StandardShanheZhaoguMirrorPackage.SkillId).Program is not null,
                    "The actual fixed-card recipe loads one physical mirror and exactly one executable source6018 equipment dependency.");
        }
        if (group == 2495 && difficulty >= 21)
            Need(registry.Cards.Values.Count(c => c.LegacyKind == CardKind.HuxinMirror) == 1 &&
                registry.Cards[StandardShanheHuxinMirrorPackage.CardId].LegacyKind == CardKind.HuxinMirror &&
                registry.GetSkill(StandardShanheHuxinMirrorPackage.SkillId).Program is not null,
                "The exact high-tier held20746 card loads its native2055 dependency without granting an uninstalled equipment skill.");
    }

    private static string[] ExpectedSkills(int group, int difficulty)
    {
        var skills = new List<string> { group switch
        {
            2474 => "classic:zhanjue", 2467 => "classic:huaiyi", 2487 => "ol:tuifeng", 2473 => "ol:zhidao",
            2480 => StandardShanheSourceXunYuePackage.QiangzhiSkillId, 2497 => "classic:xuanfeng",
            2455 => "classic:dangxian", 2459 => "classic:qice", 2460 => "ol:shuzi",
            2476 => "boundary:changbiao-current", 2489 => "ol:chuanwu", 2493 => "ol:zhendu",
            2495 => StandardShanheSourceExecutionerPackage.YishiSkillId,
            2454 => StandardShanheSourceWhiteHorsePackage.XiyingSkillId,
            2452 => StandardShanheSourceMaRiDiPackage.BingjieSkillId,
            2468 => StandardShanheSourceZhangHePackage.ZhouxuanSkillId,
            2494 => StandardShanheSourceVanguardPackage.QingxiSkillId,
            2453 => StandardShanheSourceWarElephantPackage.ShouyiSkillId,
            2469 => StandardShanheSourceCaiWenjiPackage.BeigeSkillId,
            2481 => StandardShanheSourceRangerPackage.YilieSkillId,
            2466 => StandardShanheSourceYouyanWarriorPackage.JuguanSkillId,
            2484 => StandardShanheSourceGaoGanPackage.JuguanSkillId,
            2486 => StandardShanheSourceArmoryCommanderPackage.XianshuaiSkillId,
            2462 => StandardShanheSourceChengZhanPackage.TousuiSkillId,
            2470 => StandardShanheSourceZuoCiPackage.HuashenSkillId,
            2479 => StandardShanheSourceLiuSandaoPackage.ChongzhenSkillId,
            2478 => StandardShanheSourceWuCongPackage.TiepaoSkillId,
            2456 => StandardShanheSourceCoalitionTrioPackage.DuliangSkillId,
            2458 => StandardShanheSourceDemonRaiderPackage.QiangxiSkillId,
            2461 => StandardShanheSourceDemonArcherPackage.GongqiSkillId,
            2457 => StandardShanheSourceDemonAssassinPackage.QianxiSkillId,
            2498 => StandardShanheSourceYulinGuardPackage.JushouSkillId, 2451 => "classic:zhengu",
            2500 => StandardShanheSourceAgricultureOfficerPackage.TuntianSkillId,
            _ => throw new ArgumentOutOfRangeException(nameof(group))
        } };
        if (group == 2474)
        {
            if (difficulty >= 6) skills.Add("classic:lianying");
            if (difficulty >= 11) skills.Add("boundary:longdan");
            if (difficulty >= 16) skills.Add("boundary:wusheng");
            if (difficulty >= 21) skills.Add(StandardShanheCampaignCatalog.S5ZaiqiSkillId);
        }
        else if (group == 2467)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWuhuanRiderPackage.TuxiSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceWuhuanRiderPackage.JixiSkillId);
        }
        else if (group == 2487)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWarriorPackage.YoudiSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceWarriorPackage.LuoyiSkillId);
        }
        else if (group == 2473)
        {
            if (difficulty >= 11) skills.Add("boundary:paoxiao");
            if (difficulty >= 16) skills.Add("ol:sidao");
        }
        else if (group == 2480)
        {
            if (difficulty >= 11) skills.Add("ol:mingzhe");
            if (difficulty >= 21) skills.Add("ol:yuxu");
        }
        else if (group == 2497)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceJiangdongVeteranPackage.FenliSkillId);
            if (difficulty >= 16) skills.Add("mobile:pingkou");
        }
        else if (group == 2455)
        {
            if (difficulty >= 11) skills.Add("classic:kanpo");
            if (difficulty >= 21) skills.Add("ol:dianhu");
        }
        else if (group == 2459)
        {
            if (difficulty >= 11) skills.Add("boundary:tishen");
            if (difficulty >= 16) skills.Add("mobile:duanfa");
        }
        else if (group == 2460)
        {
            if (difficulty >= 11) skills.Add("ol:weicheng");
            if (difficulty >= 16) skills.Add("ol:shunshi");
        }
        else if (group == 2476 && difficulty >= 16) skills.Add("classic:fulin");
        else if (group == 2493)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWuqueAssassinPackage.YiduSkillId);
            if (difficulty >= 21) skills.Add("classic:jueqing");
        }
        else if (group == 2495)
        {
            skills.Add("ol:gongqing");
            if (difficulty >= 6) skills.Add(StandardShanheSourceExecutionerPackage.HuanfuSkillId);
            if (difficulty >= 11) skills.Add("classic:junxing");
        }
        else if (group == 2454)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWhiteHorsePackage.QingjiaoSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceWhiteHorsePackage.YicongSkillId);
        }
        else if (group == 2452)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceMaRiDiPackage.ZhengdingSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceMaRiDiPackage.WeizhongSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceMaRiDiPackage.CaiwangSkillId);
        }
        else if (group == 2468)
        {
            if (difficulty >= 16) skills.Add(StandardShanheSourceZhangHePackage.WeijingSkillId);
        }
        else if (group == 2494)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceVanguardPackage.DuoruiSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceVanguardPackage.ZhuijiSkillId);
        }
        else if (group == 2453)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWarElephantPackage.XiongshouSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceWarElephantPackage.PojunSkillId);
        }
        else if (group == 2469)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceCaiWenjiPackage.MoshiSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceCaiWenjiPackage.TianduSkillId);
        }
        else if (group == 2481)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceRangerPackage.ChunlaoSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceRangerPackage.JuesiSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceRangerPackage.NamanSkillId);
        }
        else if (group == 2466)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceYouyanWarriorPackage.ZhaoranSkillId);
        }
        else if (group == 2484)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceGaoGanPackage.FengpoSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceGaoGanPackage.HulieSkillId);
        }
        else if (group == 2486)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceArmoryCommanderPackage.JueyanSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceArmoryCommanderPackage.TuxingSkillId);
        }
        else if (group == 2478)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWuCongPackage.AnjianSkillId);
        }
        else if (group == 2470)
        {
            skills.Add(StandardShanheSourceZuoCiPackage.XinshengSkillId);
            if (difficulty >= 11) skills.Add(StandardShanheSourceZuoCiPackage.BolanSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceZuoCiPackage.ZhimingSkillId);
        }
        else if (group == 2479)
        {
            skills.Add(StandardShanheSourceLiuSandaoPackage.HuchenSkillId);
            if (difficulty >= 6) { skills.Add(StandardShanheSourceLiuSandaoPackage.AozhanSkillId); skills.Add(StandardShanheSourceLiuSandaoPackage.NuzhanSkillId); }
            if (difficulty >= 11) skills.Add(StandardShanheSourceLiuSandaoPackage.WuqianSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceLiuSandaoPackage.ZhanshenSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceLiuSandaoPackage.DengjiSkillId);
        }
        else if (group == 2462)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceChengZhanPackage.LvliSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceChengZhanPackage.DuoruiSkillId);
        }
        else if (group == 2456)
        {
            if (difficulty >= 11)
            {
                skills.Add(StandardShanheSourceCoalitionTrioPackage.CangchuSkillId);
                skills.Add(StandardShanheSourceCoalitionTrioPackage.SushouSkillId);
            }
        }
        else if (group == 2458)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceDemonRaiderPackage.JielueSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceDemonRaiderPackage.LangdaoSkillId);
        }
        else if (group == 2461)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceDemonArcherPackage.YouyanSkillId);
            if (difficulty >= 16) skills.Add(StandardShanheSourceDemonArcherPackage.SaoguSkillId);
        }
        else if (group == 2457)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceDemonAssassinPackage.YoudiSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceDemonAssassinPackage.YiduSkillId);
        }
        else if (group == 2498)
        {
            if (difficulty >= 11) skills.Add("shanhe:source-keshou-993");
            if (difficulty >= 16) skills.Add("ol:qiongshou");
        }
        else if (group == 2451)
        {
            if (difficulty >= 11) skills.Add("shanhe:source-shengxi-2118");
            if (difficulty >= 16) skills.Add("shanhe:source-keshou-993");
        }
        else if (group == 2500)
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceAgricultureOfficerPackage.TianduSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceAgricultureOfficerPackage.DuanliangSkillId);
        }
        return skills.ToArray();
    }
    private static string General(int group, int difficulty) => group switch
    {
        2454 => StandardShanheSourceWhiteHorsePackage.GetGeneralId(difficulty),
        2452 => StandardShanheSourceMaRiDiPackage.GetGeneralId(difficulty),
        2468 => StandardShanheSourceZhangHePackage.GetGeneralId(difficulty),
        2494 => StandardShanheSourceVanguardPackage.GetGeneralId(difficulty),
        2453 => StandardShanheSourceWarElephantPackage.GetGeneralId(difficulty),
        2469 => StandardShanheSourceCaiWenjiPackage.GetGeneralId(difficulty),
        2481 => StandardShanheSourceRangerPackage.GetGeneralId(difficulty),
        2466 => StandardShanheSourceYouyanWarriorPackage.GetGeneralId(difficulty),
        2484 => StandardShanheSourceGaoGanPackage.GetGeneralId(difficulty),
        2486 => StandardShanheSourceArmoryCommanderPackage.GetGeneralId(difficulty),
        2462 => StandardShanheSourceChengZhanPackage.GetGeneralId(difficulty),
        2470 => StandardShanheSourceZuoCiPackage.GetGeneralId(difficulty),
        2479 => StandardShanheSourceLiuSandaoPackage.GetGeneralId(difficulty),
        2478 => StandardShanheSourceWuCongPackage.GetGeneralId(difficulty),
        2456 => StandardShanheSourceCoalitionTrioPackage.GetQuartermasterGeneralId(difficulty),
        2458 => StandardShanheSourceDemonRaiderPackage.GetGeneralId(difficulty),
        2461 => StandardShanheSourceDemonArcherPackage.GetGeneralId(difficulty),
        2474 => StandardShanheSourceVeteranPackage.GetGeneralId(difficulty),
        2467 => StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty),
        2487 => StandardShanheSourceWarriorPackage.GetGeneralId(difficulty),
        2473 => StandardShanheSourceQiangRaiderPackage.GetGeneralId(difficulty),
        2480 => StandardShanheSourceXunYuePackage.GetGeneralId(difficulty),
        2497 => StandardShanheSourceJiangdongVeteranPackage.GetGeneralId(difficulty),
        2455 => StandardShanheSourceCoalitionScoutPackage.GetGeneralId(difficulty),
        2459 => StandardShanheSourceCoalitionSpyPackage.GetGeneralId(difficulty),
        2460 => StandardShanheSourceLocalMagnatePackage.GetGeneralId(difficulty),
        2476 => StandardShanheSourceLinZhongGuPackage.GetGeneralId(difficulty),
        2489 => StandardShanheSourceXinPingPackage.GetGeneralId(difficulty),
        2493 => StandardShanheSourceWuqueAssassinPackage.GetGeneralId(difficulty),
        2495 => StandardShanheSourceExecutionerPackage.GetGeneralId(difficulty),
        2457 => StandardShanheSourceDemonAssassinPackage.GetGeneralId(difficulty),
        2498 => StandardShanheSourceYulinGuardPackage.GetGeneralId(difficulty),
        2451 => StandardShanheSourceBuHengPackage.GetGeneralId(difficulty),
        2500 => StandardShanheSourceAgricultureOfficerPackage.GetGeneralId(difficulty),
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static int LocalTier(int group, int difficulty) => group switch
    {
        2474 => difficulty switch { 1 => 6, 2 => 16, _ => 21 },
        2473 => difficulty switch { 1 => 1, 2 => 11, _ => 16 },
        2467 or 2487 or 2480 or 2497 or 2455 or 2459 or 2460 or 2476 or 2489 or 2493 or 2495 or 2457 or 2498 or 2451 or 2500 or 2454 or 2456 or 2458 or 2461 or 2452 or 2468 or 2494 or 2453 or 2469 or 2481 or 2466 or 2484 or 2486 or 2462 or 2470 or 2479 or 2478 => difficulty switch { 1 => 1, 2 => 11, _ => 21 },
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static int SourceBaseHp(int group) => group switch
    {
        2480 or 2459 or 2493 or 2457 or 2500 or 2456 or 2452 or 2469 or 2470 or 2478 => 3,
        2453 or 2479 => 5,
        2474 or 2467 or 2487 or 2473 or 2497 or 2455 or 2460 or 2476 or 2489 or 2495 or 2498 or 2451 or 2454 or 2458 or 2461 or 2468 or 2494 or 2481 or 2466 or 2484 or 2486 or 2462 => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static IReadOnlyList<ShanheStartingCard> ExpectedStartingCards(int group, int difficulty)
    {
        if (group == 2478) return difficulty >= 16 ? [new(CardKind.StoneAxe, Suit.Heart, 5)] : [];
        if (group == 2495) return difficulty >= 21 ? [new(CardKind.HuxinMirror, Suit.Diamond, 2)] : [];
        if (group == 2494) return [new(CardKind.FangtianHalberd, Suit.Heart, 12)];
        if (group != 2497) return [];
        var cards = new List<ShanheStartingCard> { new(CardKind.QinggangSword, Suit.Club, 6) };
        if (difficulty >= 11) cards.Add(new(CardKind.SilverLion, Suit.Diamond, 1));
        if (difficulty >= 21) cards.Add(new(CardKind.ZhaoguMirror, Suit.Heart, 1));
        return cards;
    }
    private static IReadOnlyList<ShanheSourceEnemyTierAttributes> ExpectedSourceAttributes(int group) => group switch
    {
        2493 => [new(2, new(ExtraSlash: 1)), new(4, new(ExtraSlash: 1))],
        2498 => [new(4, new(StartingCards: 2))],
        2456 => [new(4, new(StartingCards: 1)), new(5, new(StartingCards: 2))],
        2458 => [new(3, new(Hp: 1)), new(5, new(Hp: 1))],
        2452 => [new(5, new(Hp: 1))],
        2468 => [new(3, new(StartingCards: 1)), new(5, new(StartingCards: 1, ExtraSlash: 1))],
        2486 => [new(5, new(StartingCards: 2))],
        _ => []
    };
    private static int LocalFirstChapterHp(int group, int difficulty) =>
        4 + difficulty + (group == 2458 ? (difficulty >= 2 ? 1 : 0) + (difficulty == 3 ? 1 : 0) : group == 2452 && difficulty == 3 ? 1 : 0);
    private static int LocalFirstChapterHand(int group, int difficulty) => group switch
    {
        2498 when difficulty == 3 => 2,
        2456 when difficulty == 3 => 3,
        2486 when difficulty == 3 => 2,
        _ => 0
    };
    private static int LocalThirdChapterSlash(int group, int sourceTier) => group == 2493
        ? 1 + (sourceTier >= 6 ? 1 : 0) + (sourceTier >= 16 ? 1 : 0) : group == 2468 && sourceTier >= 21 ? 2 : 1;
    private static int LocalThirdChapterHand(int group, int difficulty) =>
        2 + difficulty - 1 + (group == 2468 ? (difficulty >= 2 ? 1 : 0) + (difficulty == 3 ? 1 : 0) : 0);

    private static void PreviewAndMovePatrolFromFreshCamp(ShanheCampaignDefinition definition,
        ShanheNodeDefinition camp, ShanheNodeDefinition patrol)
    {
        // Preserve the real route's original roots so its earlier nodes remain
        // reachable; admit the camp as an additional fresh entry for this probe.
        var entry = definition with
        {
            StartingNodeIds = definition.StartingNodeIds.Append(camp.Id).Distinct(StringComparer.Ordinal).ToArray(),
            RandomStartingNode = false
        };
        var session = ShanheCampaignSession.Create(entry, new(83, "classic:guan-yu"), StandardShanheCampaignCatalog.CreateBattleRegistry);
        var enteredCamp = session.Submit(new MoveShanheNodeCommand(camp.Id, session.Revision));
        Need(enteredCamp.Accepted && session.PendingBattleSetup is null && session.Snapshot.EncounterOrdinal == 0,
            enteredCamp.Error ?? "The actual final camp remains a non-battle entry.");
        var rested = session.Submit(new ChooseShanheNodeOptionCommand("rest", null, session.Revision));
        Need(rested.Accepted && session.Snapshot.Phase == ShanheCampaignPhase.Map &&
            session.Snapshot.AvailableNodes.Select(n => n.Id).SequenceEqual(camp.NextNodeIds),
            rested.Error ?? "A real camp choice retains every old route and the optional ordinary patrol.");
        var revision = session.Revision;
        var first = session.PreviewEncounter(patrol.Id);
        var again = session.PreviewEncounter(patrol.Id);
        Need(session.Revision == revision && session.Snapshot.EncounterOrdinal == 0 && session.PendingBattleSetup is null &&
            first.Id == again.Id && SameMembers(first.Enemies, again.Enemies) &&
            patrol.Encounters!.Any(e => e.Id == first.Id),
            "Repeated natural patrol previews retain the original candidate without consuming an encounter.");
        var moved = session.Submit(new MoveShanheNodeCommand(patrol.Id, session.Revision));
        Need(moved.Accepted && session.PendingBattleSetup is { } setup && setup.NodeId == patrol.Id &&
            setup.EncounterOrdinal == 1 && SameMembers(setup.EnemyRoster, first.Enemies) &&
            setup.BossTransition is null && session.Snapshot.CurrentBattleNode is
                { Kind: ShanheNodeKind.Battle, BossRewards: null } current && current.NextNodeIds.SequenceEqual(patrol.NextNodeIds),
            moved.Error ?? "The fresh camp-to-patrol move freezes an ordinary prebattle and preserves its final routes, without manufacturing a battle result.");
    }

    private static IReadOnlySet<int> ExactOpeningZhenduCosts(GameEngine game, ShanheBattleSetup setup, int group, int enemySeat)
    {
        if (group != 2493) return new HashSet<int>();
        const string skill = "ol:zhendu";
        var starts = game.Events.Select(e => e.Payload).OfType<ProgramBindingStartedEvent>()
            .Where(e => e.SkillId == skill && e.OwnerSeat == enemySeat).ToArray();
        var paid = game.CardMovements.Where(m => m.From == CardLocation.Hand(enemySeat) &&
            m.To == CardLocation.DiscardPile && m.Reason.Value == $"skill-program.{skill}.{SkillProgramEffectOp.MoveBoundCards}").ToArray();
        Need(starts.Length <= 1 && paid.Length == starts.Length,
            "Only one actual opening-phase Zhendu activation may explain the source2493 hand payment.");
        if (starts.Length == 0) return new HashSet<int>();
        var start = starts[0]; var payment = paid[0];
        var invocation = game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardInvocationRecordedEvent>()
            .Single(e => e.ParentFrameId == start.FrameId);
        var appearance = game.Events.Select(e => e.Payload).OfType<CardUseAppearanceCapturedEvent>()
            .Single(e => e.Action is { ActorSeat: 0, EffectiveKind: CardKind.Alcohol } && e.Action.ProviderSeat == enemySeat).Action;
        var use = game.Events.Select(e => e.Payload).OfType<CurrentTurnCardUseKindsRecordedEvent>()
            .Single(e => e.CardActionId == appearance.ActionId && e.ActorSeat == 0 && !e.IsNullificationUse);
        var declared = game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>()
            .Single(e => e.ResolutionId == use.OriginFrameId);
        var replaced = game.Events.Select(e => e.Payload).OfType<DamageReplacedWithHpLossEvent>()
            .Single(e => e.ParentResolutionId == start.FrameId);
        Need(start.BindingId == "other-play-start-wine" && start.Window == SkillProgramTriggerWindow.PlayPhaseStarting &&
            invocation.InitiatorSeat == enemySeat && invocation.ExecutorSeat == enemySeat &&
            invocation.SubjectSeat == enemySeat && invocation.ActualCount == 1 && invocation.HandCount == 1 &&
            invocation.Reason == payment.Reason && payment.Sequence > invocation.SequenceBefore &&
            payment.Sequence <= invocation.SequenceAfter &&
            game.Events.Select(e => e.Payload).OfType<NativeLogicalDiscardMaterialCompletedEvent>().Count(e =>
                e.InvocationId == invocation.InvocationId && e.CardId == payment.CardId && e.From == payment.From &&
                e.To == payment.To && e.CompletionSequence == payment.Sequence) == 1 &&
            appearance.Type == CardActionType.Use && appearance.PhysicalCards.Count == 0 &&
            appearance.TargetSeats.SequenceEqual([0]) && appearance.EffectiveSuit == Suit.None && appearance.EffectiveRank == 0 &&
            declared is { CardId: 0, CardKind: CardKind.Alcohol, SourceSeat: 0 } &&
            game.Events.Select(e => e.Payload).OfType<CardUseFinishedEvent>().Count(e =>
                e.ResolutionId == declared.ResolutionId && e.CardId == 0 && e.CardKind == CardKind.Alcohol) == 1 &&
            game.Events.Select(e => e.Payload).OfType<AlcoholAppliedEvent>().Count(e =>
                e.ResolutionId == declared.ResolutionId && e.SourceSeat == 0) == 1 &&
            replaced.PolicyOwnerSeat == enemySeat && replaced.TargetSeat == 0 && replaced.Amount == 1 &&
            replaced.RemainingHp == setup.Hp - 1 && game.CreateSnapshot(0).Players[0].Hp == replaced.RemainingHp &&
            !game.Events.Select(e => e.Payload).OfType<DamageAppliedEvent>().Any() &&
            game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e =>
                e.FrameId == start.FrameId && e.Activated && e.Completed) == 1 &&
            !game.ResolutionStack.Any(f => f.Id == start.FrameId || f.Id == declared.ResolutionId),
            "The tier21-plus NPC's exact hand cost funds one zero-material Wine by the real human phase owner; its original Zhendu frame then loses exactly one HP through that NPC's existing Jueqing and completes before first human Play.");
        return paid.Select(m => m.CardId).ToHashSet();
    }

    private static IReadOnlyList<int> ExactQiongshouOpeningDraw(GameEngine game, int group, int tier, int enemySeat)
    {
        if (group != 2498 || tier < 16) return [];
        const string skill = "ol:qiongshou";
        var start = game.Events.Select(e => e.Payload).OfType<ProgramBindingStartedEvent>()
            .Single(e => e.SkillId == skill && e.OwnerSeat == enemySeat && e.Window == SkillProgramTriggerWindow.GameStarting);
        var request = game.Events.Select(e => e.Payload).OfType<NativeDrawInvocationRecordedEvent>()
            .Single(e => e.ParentFrameId == start.FrameId);
        var moves = game.CardMovements.Where(m => m.Sequence > request.SequenceBefore && m.Sequence <= request.SequenceAfter &&
            m.Reason == request.Reason && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(enemySeat)).ToArray();
        var enemy = game.CreateSnapshot(enemySeat).Players[enemySeat];
        Need(start.BindingId == "initial-equipment-abolition" && request.OwnerSeat == enemySeat &&
            request.Requested == 4 && request.ActualCount == 4 && request.TurnNumber == 0 &&
            moves.Length == 4 && moves.Select(m => m.CardId).Distinct().Count() == 4 &&
            moves.All(m => m.Reason != CardMoveReasons.InitialDeal) &&
            game.Events.Select(e => e.Payload).OfType<PlayerAreasAbolishedEvent>().Count(e =>
                e.Seat == enemySeat && e.Equipment && !e.Judgment) == 1 &&
            enemy.IsEquipmentAreaAbolished && enemy.Equipment.Count == 0 &&
            // Whole-area abolition overrides default per-slot capacities; the
            // snapshot deliberately omits an empty override dictionary.
            Enum.GetValues<EquipmentSlot>().All(slot => (enemy.IsEquipmentAreaAbolished
                ? 0 : enemy.EquipmentSlotCapacities?.GetValueOrDefault(slot, 1) ?? 1) == 0) &&
            game.State.CurrentSeat != enemySeat && enemy.HandLimit is null &&
            game.Events.Select(e => e.Payload).OfType<ProgramBindingResolvedEvent>().Count(e =>
                e.FrameId == start.FrameId && e.SkillId == skill && e.Completed) == 1 &&
            !game.ResolutionStack.Any(f => f.Id == start.FrameId),
            "High-tier source2498 separately completes Qiongshou's real game-start area abolition and exact four-card draw without changing the source initial-deal quantity; the off-turn enemy's hand limit remains unprojected at the human Play boundary.");
        return moves.Select(m => m.CardId).ToArray();
    }
    private static bool SameMembers(IReadOnlyList<ShanheCombatantDefinition> left, IReadOnlyList<ShanheCombatantDefinition> right) =>
        left.Select(m => (m.GeneralId, m.Hp, m.MaxHpBonus, m.InitialHandBonus, m.DrawBonus, m.SlashBonus,
                m.InitialArmor, m.ArmorRecoveryPerRound, m.ArmorRecoveryCap, m.ExtraDamage))
            .SequenceEqual(right.Select(m => (m.GeneralId, m.Hp, m.MaxHpBonus, m.InitialHandBonus, m.DrawBonus, m.SlashBonus,
                m.InitialArmor, m.ArmorRecoveryPerRound, m.ArmorRecoveryCap, m.ExtraDamage))) &&
        left.Zip(right).All(pair => (pair.First.StartingCards ?? []).SequenceEqual(pair.Second.StartingCards ?? []) &&
            (pair.First.StartingEquipment ?? []).SequenceEqual(pair.Second.StartingEquipment ?? []));
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
