using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceOrdinaryTroopChecks
{
    private static readonly int[] Groups = [2474, 2467, 2487, 2473, 2480, 2497];

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
                        body.SourceDifficultyAttributes is { Count: 0 } && body.StartingHand is null &&
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
                if (difficulty == 25 && (group is 2473 or 2480 or 2497))
                    Need(actual.Length == (group == 2473 ? 14 : 56),
                        "With every source region open, the first-chapter raider retains14 cities and both second-chapter groups retain56 actual source-city memberships.");
            }
            var probe = new ShanheBattleSetup(83, 1, "fixture:ordinary-source-dependencies", "classic:guan-yu",
                4, 0, 0, 0, 0, [], [], [], Groups.Select(group => new ShanheCombatantDefinition(General(group, difficulty))
                    { StartingCards = ExpectedStartingCards(group, difficulty) }).ToArray(),
                [], "fixture:ordinary-source-dependencies") { SourceDifficultyId = difficulty };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(probe);
            foreach (var group in Groups) AssertGeneralAndPrograms(registry, group, difficulty);
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
                foreach (var group in Groups.Where(group => group != 2473))
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
                        "All five complete second-chapter local troop recipes survive both normal nodes and all five terrain clones, retaining each original body's own maximum HP, exact extra hand cards and no fabricated source identities.");
                }
            }
            foreach (var id in new[] { "hulao", "guangcheng", "hulao-start", "guangcheng-start" })
            {
                var node = definition.Nodes.Single(n => n.Id == id);
                var encounter = node.Encounters!.Single(e => e.Enemies.Count == 1 &&
                    e.Enemies[0].GeneralId == General(2473, LocalTier(2473, difficulty)));
                var body = encounter.Enemies.Single();
                Need(node.ChapterNumber == 1 && encounter.SourceBattleId is null && encounter.SourceCityId is null &&
                    encounter.SourceVisibleEnemyCount is null && encounter.BossTransition is null &&
                    (encounter.Allies?.Count ?? 0) == 0 && body.Hp == 4 + difficulty && body.MaxHpBonus == difficulty &&
                    body.DrawBonus == difficulty && body.InitialHandBonus == 0 && body.SourceDifficultyAttributes is null &&
                    body.StartingHand is null && (body.StartingCards?.Count ?? 0) == 0 &&
                    (body.StartingEquipment?.Count ?? 0) == 0 && 1 + encounter.Enemies.Count <= 8,
                    "The complete local Qiang raider survives both first-chapter routes and their random-opening copies, with its authored first-chapter growth and no second-chapter hand bonus or source labels.");
                if (id.EndsWith("-start", StringComparison.Ordinal))
                {
                    var original = definition.Nodes.Single(n => n.Id == id[..^6]).Encounters!
                        .Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == body.GeneralId);
                    Need(SameMembers(encounter.Enemies, original.Enemies),
                        "Random-opening copies retain the actual source-body recipe rather than adding another local growth pass.");
                }
            }
        }
    }

    public static void SourceAndLocalPreviewMoveAndStartUseOneProjectionAndRealOpeningEntities()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(25);
        foreach (var group in new[] { 2474, 2480, 2497 })
        {
            var cityNode = source.Nodes.First(n => n.Kind == ShanheNodeKind.Battle && n.Encounters!.Any(e => e.SourceBattleId == group));
            var sourceEncounter = cityNode.Encounters!.Single(e => e.SourceBattleId == group);
            EnterAndStart(source, cityNode, sourceEncounter, group, 25);
        }

        var local = StandardShanheCampaignCatalog.CreateCodexDefinition(3, StandardShanheCampaignCatalog.Returning2026EditionId);
        var localNode = local.Nodes.Single(n => n.Id == "dagu");
        foreach (var group in new[] { 2474, 2480, 2497 })
        {
            var tier = LocalTier(group, 3);
            var localEncounter = localNode.Encounters!.Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == General(group, tier));
            EnterAndStart(local, localNode, localEncounter, group, tier, localDifficulty: 3);
        }
        var opening = local.Nodes.Single(n => n.Id == "hulao-start");
        var raider = opening.Encounters!.Single(e => e.Enemies.Count == 1 &&
            e.Enemies[0].GeneralId == StandardShanheSourceQiangRaiderPackage.NightmareId);
        EnterAndStart(local, opening, raider, 2473, 16, localDifficulty: 3);
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
            var hp = group == 2473 ? 4 + difficulty : 7 + 2 * growth;
            Need(member.Hp == hp && member.MaxHpBonus == hp - baseHp &&
                member.InitialHandBonus == (group == 2473 ? 0 : 1 + growth) &&
                member.DrawBonus == (group == 2473 ? difficulty : 1) && SameMembers([member], selected.Enemies) &&
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
            initial.Concat(extraHand).Select(m => m.CardId).Order().SequenceEqual(enemy.Hand.Select(c => c.Id).Order()) &&
            ExpectedSkills(group, tier).All(id => enemy.Skills!.Count(s => s.ContentId == id) == 1) &&
            (group != 2497 || enemy.Skills!.All(s => s.ContentId != StandardShanheZhaoguMirrorPackage.SkillId)) &&
            game.Events.Where(e => e.Payload is TurnStartedEvent).All(e => ((TurnStartedEvent)e.Payload).ActorSeat == 0),
            "The real first opening uses the prepared health and body's maximum once, deals the random opening independently of exact extra hand entities, installs each cumulative printed skill once without auto-equipping a held mirror, and stops before an enemy turn.");
    }

    private static void AssertGeneralAndPrograms(ContentRegistry registry, int group, int difficulty)
    {
        var general = registry.Generals[General(group, difficulty)];
        var expected = ExpectedSkills(group, difficulty);
        var baseHp = SourceBaseHp(group);
        Need(general.BaseHp == baseHp && general.InitialHp == baseHp && registry.IsGeneralPlayable(general.Id) &&
            general.SkillIds.Order().SequenceEqual(expected.Order()) &&
            expected.All(id => registry.GetSkill(id).Program is not null),
            "The actual factory loads the complete selected source tier and every executable skill dependency.");
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
    }

    private static string[] ExpectedSkills(int group, int difficulty)
    {
        var skills = new List<string> { group switch
        {
            2474 => "classic:zhanjue", 2467 => "classic:huaiyi", 2487 => "ol:tuifeng", 2473 => "ol:zhidao",
            2480 => StandardShanheSourceXunYuePackage.QiangzhiSkillId, 2497 => "classic:xuanfeng",
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
        return skills.ToArray();
    }
    private static string General(int group, int difficulty) => group switch
    {
        2474 => StandardShanheSourceVeteranPackage.GetGeneralId(difficulty),
        2467 => StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty),
        2487 => StandardShanheSourceWarriorPackage.GetGeneralId(difficulty),
        2473 => StandardShanheSourceQiangRaiderPackage.GetGeneralId(difficulty),
        2480 => StandardShanheSourceXunYuePackage.GetGeneralId(difficulty),
        2497 => StandardShanheSourceJiangdongVeteranPackage.GetGeneralId(difficulty),
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static int LocalTier(int group, int difficulty) => group switch
    {
        2474 => difficulty switch { 1 => 6, 2 => 16, _ => 21 },
        2473 => difficulty switch { 1 => 1, 2 => 11, _ => 16 },
        2467 or 2487 or 2480 or 2497 => difficulty switch { 1 => 1, 2 => 11, _ => 21 },
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static int SourceBaseHp(int group) => group switch
    {
        2480 => 3,
        2474 or 2467 or 2487 or 2473 or 2497 => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(group))
    };
    private static IReadOnlyList<ShanheStartingCard> ExpectedStartingCards(int group, int difficulty)
    {
        if (group != 2497) return [];
        var cards = new List<ShanheStartingCard> { new(CardKind.QinggangSword, Suit.Club, 6) };
        if (difficulty >= 11) cards.Add(new(CardKind.SilverLion, Suit.Diamond, 1));
        if (difficulty >= 21) cards.Add(new(CardKind.ZhaoguMirror, Suit.Heart, 1));
        return cards;
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
