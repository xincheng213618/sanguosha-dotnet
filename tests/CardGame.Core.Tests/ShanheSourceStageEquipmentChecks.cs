using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceStageEquipmentChecks
{
    private const string Driver = "fixture:source-stage-equipment-driver", Deck = "fixture:source-stage-equipment-deck";
    private const string Wushuang = "shanhe:shengui-wushuang", Shenqu = "shanhe:shengui-shenqu";

    public static void SourceThresholdsResolveExactFormsAndCumulativeStageEquipment()
    {
        foreach (var difficulty in new[] { 3, 6, 11, 16, 21, 25 })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            foreach (var battleId in new[] { 2644, 2645, 2671 })
            {
                var encounter = Encounter(source, battleId);
                var stage = encounter.BossTransition!;
                var setup = RawSetup(source, encounter, difficulty, battleId, withDriver: false);
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
                var expectedId = battleId switch
                {
                    2645 => difficulty >= 21 ? StandardShanheBossPackage.ShenguiLvbuId : difficulty >= 16
                        ? StandardShanheBossPackage.SourceShenguiNightmareId : "classic:lu-bu",
                    2671 => difficulty >= 16 ? StandardShanheS5BossPackage.DongMinCuanchaoId : StandardShanheS5BossPackage.DongMinBaseId,
                    _ => difficulty >= 21 ? StandardShanheFengxianBossPackage.SourceRageHellId : difficulty >= 11
                        ? StandardShanheFengxianBossPackage.SourceRageHardId : StandardShanheFengxianBossPackage.RageId
                };
                Need(stage.GeneralId == expectedId && stage.Combatant?.GeneralId == expectedId &&
                    stage.Hp == (battleId == 2645 ? 3 : 5) && stage.MaxHp == stage.Hp && stage.DrawCards == 4,
                    "The real source transition selects its tier's form while retaining source three/three or five/five and the existing phase-card recipe, rather than copying the printed body's health.");
                Need((stage.StartingEquipment ?? []).Select(Face).SequenceEqual(ExpectedEquipment(battleId, difficulty).Select(Face)) &&
                    (stage.StartingEquipment ?? []).All(card => registry.Cards.Values.Any(definition => definition.LegacyKind == card.Kind)),
                    "Every stage's cumulative equipment thresholds must resolve to the actual registered physical card faces.");
                var general = registry.Generals[expectedId];
                var skills = new[] { general.SkillId }.OfType<string>().Concat(general.AdditionalSkillIds ?? []).Order().ToArray();
                if (battleId == 2645)
                    Need(skills.SequenceEqual((difficulty >= 21 ? new[] { Wushuang, Shenqu } : difficulty >= 16 ? new[] { Wushuang } : ["classic:wushuang"]).Order()) &&
                        (difficulty < 21 || general.BaseHp == 6 && stage.MaxHp == 3),
                        "The source 264501 panel has classic Wushuang through fifteen, only dynamic Wushuang at sixteen, and adds Shenqu at twenty-one without adopting printed six HP.");
                if (battleId == 2671)
                    Need(skills.SequenceEqual((difficulty >= 16 ? new[] { StandardShanheS5BossPackage.DungongId, StandardShanheS5BossPackage.CuanchaoId }
                            : [StandardShanheS5BossPackage.DungongId]).Order()),
                        "The Dong Min phase retains its real shield skill and adds Cuanchao only at sixteen.");
                if (battleId is 2644 or 2645)
                    Need((encounter.Enemies[0].StartingHand?.Count ?? 4) == 4 && encounter.Enemies[0].InitialHandBonus == 0 &&
                        (encounter.Enemies[0].StartingCards ?? []).Count(card => card.Kind == CardKind.SilverLion) == (difficulty >= 11 ? 1 : 0) &&
                        !(encounter.Enemies[0].StartingEquipment ?? []).Any(card => card.Kind == CardKind.SilverLion),
                        "The extra hard-tier Silver Lion is added to the first form's hand recipe, preserving its four-card base and independent source-growth attributes.");
            }
            if (difficulty is 16 or 21) VerifyFengyiSourceIdentity(source, difficulty);
        }
    }

    private static void VerifyFengyiSourceIdentity(ShanheCampaignDefinition source, int difficulty)
    {
        const string dong = StandardShanheS5BossPackage.DongZhuoFengyiId;
        const string lvbu = StandardShanheS5BossPackage.LvbuBajiId;
        const string diaochan = StandardShanheS5BossPackage.DiaoChanBiyueId;
        string[] order = [dong, lvbu, diaochan];
        var node = source.Nodes.Single(candidate => candidate.Encounters?.Any(encounter => encounter.SourceBattleId == 2686) == true);
        var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2686);
        Need(encounter.Enemies.Select(member => member.GeneralId).SequenceEqual(order) && encounter.SourceVisibleEnemyCount == 3,
            "The actual source2686 recipe retains its established Dong Zhuo, Lu Bu, Diao Chan seats and complete three-member roster.");
        // Admit the actual later source node without changing its roster or any
        // opening quantities, then exercise the normal preview/entry projection.
        var entry = source with
        {
            StartingNodeIds = source.StartingNodeIds.Append(node.Id).Distinct(StringComparer.Ordinal).ToArray(),
            RandomStartingNode = false,
            Nodes = source.Nodes.Select(candidate => candidate.Id == node.Id ? candidate with { Encounters = [encounter] } : candidate).ToArray()
        };
        var run = ShanheCampaignSession.Create(entry, new(71, "classic:guan-yu") { SourceDifficultyId = difficulty },
            StandardShanheCampaignCatalog.CreateBattleRegistry);
        var preview = run.PreviewEncounter(node.Id);
        var grownDong = preview.Enemies.Single(member => member.GeneralId == dong);
        var grownLvbu = preview.Enemies.Single(member => member.GeneralId == lvbu);
        var grownDiaochan = preview.Enemies.Single(member => member.GeneralId == diaochan);
        Need(grownDong.Hp.HasValue && grownLvbu.Hp.HasValue && grownDong.Hp.Value - grownLvbu.Hp.Value == 3 &&
            grownDong.DrawBonus - grownLvbu.DrawBonus == 2 && grownDiaochan.DrawBonus == grownDong.DrawBonus,
            $"At the real source{difficulty} boundary, Dong Zhuo keeps his own health+3 and draw+2 instead of lending them to Lu Bu; Diao Chan retains her separate draw+2.");
        var moved = run.Submit(new MoveShanheNodeCommand(node.Id, run.Revision));
        Need(moved.Accepted, moved.Error ?? "The real source2686 entry was rejected.");
        var setup = run.PendingBattleSetup!;
        Need(setup.SourceDifficultyId == difficulty &&
            JsonSerializer.Serialize(preview.Enemies) == JsonSerializer.Serialize(setup.EnemyRoster),
            "The identity-matched source attributes reach the frozen native battle setup without changing seat order or opening quantities.");
        if (difficulty == 16)
        {
            var game = StandardShanheCampaignCatalog.CreateBattle(setup);
            var players = game.CreateSnapshot(0).Players;
            var dongPlayer = players.Single(player => player.GeneralId == dong);
            var lvbuPlayer = players.Single(player => player.GeneralId == lvbu);
            Need(players.Where(player => player.TeamId == ShanheBattleSetup.EnemyTeamId).Select(player => player.GeneralId).SequenceEqual(order) &&
                dongPlayer.Hp - lvbuPlayer.Hp == 3 && dongPlayer.MaxHp - dongPlayer.Hp == 5 && lvbuPlayer.MaxHp == lvbuPlayer.Hp,
                "The corrected source growth enters real native seats while Dong Zhuo remains initially wounded and Lu Bu starts at full health.");
        }
    }

    private static ShanheEncounterDefinition Encounter(ShanheCampaignDefinition source, int id) =>
        source.Nodes.SelectMany(node => node.Encounters ?? []).Single(encounter => encounter.SourceBattleId == id);
    private static ShanheStartingCard[] ExpectedEquipment(int battle, int difficulty)
    {
        if (battle == 2671) return difficulty >= 11 ? [new(CardKind.DelusionCrown, Suit.Diamond, 4)] : [];
        var result = new List<ShanheStartingCard> { new(battle == 2645 ? CardKind.ShuraInfernoHalberd : CardKind.WushuangHalberd, Suit.Heart, 12) };
        if (difficulty >= 6) result.Add(new(CardKind.PurpleGoldCrown, Suit.Heart, 1));
        if (difficulty >= 11) result.Add(battle == 2645 ? new(CardKind.LionBelt, Suit.Club, 2) : new(CardKind.RedCottonRobe, Suit.Diamond, 1));
        return result.ToArray();
    }
    private static (CardKind Kind, Suit Suit, int Rank) Face(ShanheStartingCard card) => (card.Kind, card.Suit, card.Rank);
    private static ShanheBattleSetup RawSetup(ShanheCampaignDefinition source, ShanheEncounterDefinition encounter, int difficulty, int battleId, bool withDriver) =>
        new(71, 1, "fixture:source-stage-equipment-" + battleId + "-" + difficulty, "classic:guan-yu", 34, 30, 0, 0, 0,
            withDriver ? [Driver] : [], [new(CardKind.Slash, Suit.Heart, 10), new(CardKind.Dodge, Suit.Club, 2)], [],
            encounter.Enemies.Select(member => member with
            {
                // Keep the actual body, source fixed cards and every stage input.
                // Only the ordinary opening four become fixed Slashes here.
                StartingHand = Enumerable.Repeat(new ShanheStartingCard(CardKind.Slash, Suit.Spade, 7), 4).ToArray()
            }).ToArray(), [], ShanheCampaignContent.GetContentHash(source))
        { SourceDifficultyId = difficulty, BossTransition = encounter.BossTransition };
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
