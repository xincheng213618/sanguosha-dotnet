using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceOrdinaryTroopChecks
{
    private static readonly int[] Groups = [2474, 2467, 2487];

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
                    Need(encounter.SourceCityId == city.SourceCityId && encounter.SourceVisibleEnemyCount == 1 &&
                        encounter.BossTransition is null && (encounter.Allies?.Count ?? 0) == 0 &&
                        body.GeneralId == General(group, difficulty) &&
                        body is { Hp: 4, MaxHpBonus: 0, DrawBonus: 0, InitialHandBonus: 0, SlashBonus: 0,
                            InitialArmor: 0, ArmorRecoveryPerRound: 0, ArmorRecoveryCap: 5, ExtraDamage: 0 } &&
                        body.SourceDifficultyAttributes is { Count: 0 } && body.StartingHand is null &&
                        (body.StartingCards?.Count ?? 0) == 0 && (body.StartingEquipment?.Count ?? 0) == 0,
                        "The authored recipe is one complete soldier4861 at base4/4, draw2/hand4, without gear or a second local chapter bonus; source growth belongs to entry projection.");
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
            }
            var probe = new ShanheBattleSetup(83, 1, "fixture:ordinary-source-dependencies", "classic:guan-yu",
                4, 0, 0, 0, 0, [], [], [], Groups.Select(group => new ShanheCombatantDefinition(General(group, difficulty))).ToArray(),
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
                foreach (var group in Groups)
                {
                    var tier = LocalTier(group, difficulty);
                    var encounter = node.Encounters!.Single(e => e.Enemies.Count == 1 && e.Enemies[0].GeneralId == General(group, tier));
                    var body = encounter.Enemies.Single();
                    Need(encounter.SourceBattleId is null && encounter.SourceCityId is null && encounter.SourceVisibleEnemyCount is null &&
                        encounter.BossTransition is null && (encounter.Allies?.Count ?? 0) == 0 &&
                        encounter.TerrainSkillId == (difficulty == 3 ? terrain : null) &&
                        body.Hp == 7 + 2 * growth && body.MaxHpBonus == 3 + 2 * growth &&
                        body.InitialHandBonus == 1 + growth && body.DrawBonus == 1 && body.SourceDifficultyAttributes is null &&
                        (body.StartingCards?.Count ?? 0) == 0 && (body.StartingEquipment?.Count ?? 0) == 0 &&
                        1 + encounter.Enemies.Count + (encounter.Allies?.Count ?? 0) <= 8,
                        "All three complete local troop recipes survive both normal nodes and all five terrain clones, with local stats and no fabricated source identities.");
                }
            }
        }
    }

    public static void SourceAndLocalPreviewMoveAndStartUseOneProjectionAndRealOpeningEntities()
    {
        var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(25);
        var cityNode = source.Nodes.First(n => n.Kind == ShanheNodeKind.Battle && n.Encounters!.Any(e => e.SourceBattleId == 2474));
        var sourceEncounter = cityNode.Encounters!.Single(e => e.SourceBattleId == 2474);
        EnterAndStart(source, cityNode, sourceEncounter, sourceBacked: true);

        var local = StandardShanheCampaignCatalog.CreateCodexDefinition(3, StandardShanheCampaignCatalog.Returning2026EditionId);
        var localNode = local.Nodes.Single(n => n.Id == "dagu");
        var localEncounter = localNode.Encounters!.Single(e => e.Enemies.Count == 1 &&
            e.Enemies[0].GeneralId == StandardShanheSourceVeteranPackage.HellGeneralId);
        EnterAndStart(local, localNode, localEncounter, sourceBacked: false);
    }

    private static void EnterAndStart(ShanheCampaignDefinition definition, ShanheNodeDefinition node,
        ShanheEncounterDefinition selected, bool sourceBacked)
    {
        // Admit a real later node as a fresh entry without manufacturing any wins.
        var entry = definition with
        {
            StartingNodeIds = [.. definition.StartingNodeIds, node.Id], RandomStartingNode = false,
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
        Need(SameMembers(setup.EnemyRoster, preview.Enemies) && setup.SourceDifficultyId == definition.SourceDifficultyId &&
            setup.EncounterOrdinal == ordinal + 1 && setup.BossTransition is null,
            "Move carries exactly the already-previewed troop and one actual encounter ordinal into its native battle.");
        if (sourceBacked)
        {
            var raw = selected.Enemies.Single();
            var grown = StandardShanheSourceEnemyGrowth.Season5.Calculate(new(4, 2, 0, 0, 4, 0, 0),
                new(25, setup.EncounterOrdinal, node.ChapterNumber, 1, ShanheSourceEncounterCategory.Ordinary),
                raw.SourceDifficultyAttributes).Attributes;
            Need(raw is { Hp: 4, MaxHpBonus: 0, InitialHandBonus: 0, DrawBonus: 0 } &&
                member.Hp == grown.Hp && member.MaxHpBonus == grown.Hp - 4 &&
                member.InitialHandBonus == grown.StartingCards - 4 && member.DrawBonus == grown.Draw - 2 &&
                member.SlashBonus == grown.ExtraSlash && member.InitialArmor == grown.Armor &&
                member.ArmorRecoveryPerRound == grown.ArmorGain && member.ExtraDamage == grown.ExtraDamage &&
                preview.SourceBattleId == 2474 && preview.SourceCityId == selected.SourceCityId,
                "Only session entry projects the untouched original4/4,2/4 member with the real day, chapter, single visible enemy and source25 calculator.");
        }
        else
            Need(member is { Hp: 11, MaxHpBonus: 7, InitialHandBonus: 3, DrawBonus: 1 } &&
                member.SourceDifficultyAttributes is null && preview.SourceBattleId is null && preview.SourceCityId is null,
                "Recommended local Hell keeps its authored11HP and local draw/hand bonuses without source growth.");

        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        AssertGeneralAndPrograms(registry, 2474, 25);
        var game = StandardShanheCampaignCatalog.CreateBattle(setup);
        Need(game.Submit(new StartGameCommand()).Accepted, "The exact production troop setup accepts native StartGame.");
        for (var step = 0; !(game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                game.ResolutionStack.Count == 0); step++)
            Need(step < 120 && game.PendingDecision is null && game.State.Winner == Winner.None &&
                game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted,
                "The ordinary troop must reach the first natural human Play without simulating enemy turns or inventing a result.");
        var enemySeat = 1 + setup.AllyRoster.Count;
        var enemy = game.CreateSnapshot(enemySeat).Players.Single(p => p.Seat == enemySeat);
        var deck = registry.Decks[registry.Modes[ShanheBattleSetup.BattleModeId].DeckId!];
        var initial = game.CardMovements.Where(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(enemySeat) &&
            m.Reason == CardMoveReasons.InitialDeal).ToArray();
        Need(game.State.TurnNumber == 1 && game.State.CurrentSeat == 0 && game.State.Winner == Winner.None &&
            game.CreateSnapshot(0).Players.Count == 1 + setup.AllyRoster.Count + setup.EnemyRoster.Count &&
            game.CreateSnapshot(0).Players.Count <= 8 && enemy.GeneralId == member.GeneralId &&
            enemy.Hp == member.Hp && enemy.MaxHp == 4 + member.MaxHpBonus && enemy.Armor == member.InitialArmor &&
            enemy.Equipment.Count == 0 && initial.Length == deck.InitialHandSize + member.InitialHandBonus &&
            initial.Select(m => m.CardId).Distinct().Count() == initial.Length &&
            initial.Select(m => m.CardId).Order().SequenceEqual(enemy.Hand.Select(c => c.Id).Order()) &&
            enemy.Skills!.Count(s => s.ContentId == StandardShanheCampaignCatalog.S5ZaiqiSkillId) == 1 &&
            game.Events.Where(e => e.Payload is TurnStartedEvent).All(e => ((TurnStartedEvent)e.Payload).ActorSeat == 0),
            "The real first opening uses prepared health once, deals each actual enemy entity once, installs the shared S5再起 once, and stops before an enemy turn.");
    }

    private static void AssertGeneralAndPrograms(ContentRegistry registry, int group, int difficulty)
    {
        var general = registry.Generals[General(group, difficulty)];
        var expected = ExpectedSkills(group, difficulty);
        Need(general.BaseHp == 4 && general.InitialHp == 4 && registry.IsGeneralPlayable(general.Id) &&
            general.SkillIds.Order().SequenceEqual(expected.Order()) &&
            expected.All(id => registry.GetSkill(id).Program is not null),
            "The actual factory loads the complete selected source tier and every executable skill dependency.");
        if (group == 2474 && difficulty >= 21)
            Need(registry.Skills.Values.Count(s => s.Id == StandardShanheCampaignCatalog.S5ZaiqiSkillId) == 1 &&
                general.SkillIds.Single(id => id == StandardShanheCampaignCatalog.S5ZaiqiSkillId) ==
                    StandardShanheSourceVeteranPackage.ZaiqiSkillId,
                "Veteran and catalogue dependency discovery share the same existing S5再起 identity without missing or duplicate registration.");
    }

    private static string[] ExpectedSkills(int group, int difficulty)
    {
        var skills = new List<string> { group switch { 2474 => "classic:zhanjue", 2467 => "classic:huaiyi", _ => "ol:tuifeng" } };
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
        else
        {
            if (difficulty >= 11) skills.Add(StandardShanheSourceWarriorPackage.YoudiSkillId);
            if (difficulty >= 21) skills.Add(StandardShanheSourceWarriorPackage.LuoyiSkillId);
        }
        return skills.ToArray();
    }
    private static string General(int group, int difficulty) => group switch
    {
        2474 => StandardShanheSourceVeteranPackage.GetGeneralId(difficulty),
        2467 => StandardShanheSourceWuhuanRiderPackage.GetGeneralId(difficulty),
        _ => StandardShanheSourceWarriorPackage.GetGeneralId(difficulty)
    };
    private static int LocalTier(int group, int difficulty) => group == 2474
        ? difficulty switch { 1 => 6, 2 => 16, _ => 21 }
        : difficulty switch { 1 => 1, 2 => 11, _ => 21 };
    private static bool SameMembers(IReadOnlyList<ShanheCombatantDefinition> left, IReadOnlyList<ShanheCombatantDefinition> right) =>
        left.Select(m => (m.GeneralId, m.Hp, m.MaxHpBonus, m.InitialHandBonus, m.DrawBonus, m.SlashBonus,
                m.InitialArmor, m.ArmorRecoveryPerRound, m.ArmorRecoveryCap, m.ExtraDamage))
            .SequenceEqual(right.Select(m => (m.GeneralId, m.Hp, m.MaxHpBonus, m.InitialHandBonus, m.DrawBonus, m.SlashBonus,
                m.InitialArmor, m.ArmorRecoveryPerRound, m.ArmorRecoveryCap, m.ExtraDamage)));
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
