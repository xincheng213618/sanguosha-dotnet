using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceMouDongZhuoMapChecks
{
    private const string LocalEncounter = "luoyang:mou-dongzhuo-source-native";
    private const string Deck = "fixture:source-mou-map-quiet";

    public static void ActualSourceCityAndRecommendedRouteLoadCompleteNativeBossAndGear()
    {
        Need(StandardShanheCampaignCatalog.CreateSourceCodexDefinition(15).Nodes
                .SelectMany(node => node.Encounters ?? []).All(encounter => encounter.SourceBattleId != 2684),
            "The source boss does not appear before its actual minimum region difficulty16.");
        foreach (var difficulty in new[] { 16, 21, 25 })
        {
            var definition = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var node = definition.Nodes.Single(candidate => candidate.Id == "source-city-5148");
            var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2684);
            var raw = encounter.Enemies.Single();
            Need(encounter.SourceCityId == 5148 && encounter.SourceVisibleEnemyCount == 1 && encounter.BossTransition is null &&
                raw is { GeneralId: StandardShanheSourceMouDongZhuoPackage.GeneralId, Hp: 20, MaxHpBonus: 0,
                    InitialHandBonus: 0, DrawBonus: 0, SlashBonus: 0, InitialArmor: 0 } && raw.SourceDifficultyAttributes is not null &&
                raw.StartingHand is null && (raw.StartingEquipment?.Count ?? 0) == 0 &&
                raw.StartingCards!.SequenceEqual([new ShanheStartingCard(CardKind.KunlunMirror, Suit.Spade, 12),
                    new(CardKind.Duanhun, Suit.Spade, 5)]),
                "Source2684 retains its true605 twenty-HP body, cumulative extra hand equipment and source growth, without a local second form.");
            var setup = Enter(definition, node, encounter);
            AssertFactory(setup);
            if (difficulty == 16) AssertFreshOpening(setup);
        }
        foreach (var difficulty in new[] { 1, 2, 3 })
        foreach (var codex in new[] { false, true })
        {
            var definition = codex
                ? StandardShanheCampaignCatalog.CreateCodexDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId)
                : StandardShanheCampaignCatalog.CreateDefinition(difficulty, StandardShanheCampaignCatalog.Returning2026EditionId);
            var node = definition.Nodes.Single(candidate => candidate.Id == "luoyang");
            var encounter = node.Encounters!.Single(candidate => candidate.Id == LocalEncounter);
            var setup = Enter(definition, node, encounter);
            Need(setup.SourceDifficultyId is null && encounter.SourceBattleId is null && encounter.SourceCityId is null &&
                setup.EnemyRoster.Single().Hp == 20 + 4 * (difficulty - 1) &&
                setup.BossTransition is { GeneralId: StandardShanheSourceShenNuStagePackage.GeneralId } stage &&
                stage.Hp == setup.EnemyRoster.Single().Hp && stage.MaxHp == stage.Hp && stage.DrawCards == difficulty + 2 &&
                1 + setup.EnemyRoster.Count + setup.AllyRoster.Count <= 8,
                "Both recommended entrypoints offer the complete local final encounter, with explicitly local tiers and its real Shen Nu stage.");
            AssertFactory(setup);
            if (codex)
            foreach (var (source, kind, rank) in new[] { (4144, CardKind.Duanhun, 5), (4145, CardKind.KunlunMirror, 12) })
            {
                var rewardId = "codex-card-" + source;
                var reward = definition.Rewards.Single(candidate => candidate.Id == rewardId);
                Need(StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId] is { Price: 600, Rarity: 4, Kind: "装备", Suit: Suit.Spade } metadata &&
                    metadata.CardKind == kind && metadata.Rank == rank &&
                    reward.StartingEquipment!.SequenceEqual([new ShanheStartingCard(kind, Suit.Spade, rank)]) &&
                    definition.Nodes.Where(candidate => candidate.Kind == ShanheNodeKind.Shop).All(shop =>
                        shop.Choices!.Any(choice => choice.RewardId == rewardId && choice.GoldCost == 600)),
                    "The actual original catalogue weapon and Treasure offers are wired into the recommended Codex shop at their real rarity and price.");
            }
            if (difficulty == 3 && codex) AssertFreshOpening(setup);
        }
    }

    private static ShanheBattleSetup Enter(ShanheCampaignDefinition definition, ShanheNodeDefinition selectedNode,
        ShanheEncounterDefinition encounter)
    {
        // Admit only this real node as an additional start; keep the production
        // graph, original source identity and every growth contract intact.
        var entry = definition with
        {
            StartingNodeIds = [.. definition.StartingNodeIds, selectedNode.Id], RandomStartingNode = false,
            Nodes = definition.Nodes.Select(node => node.Id == selectedNode.Id ? node with { Encounters = [encounter] } : node).ToArray()
        };
        var session = ShanheCampaignSession.Create(entry, new(71, "classic:guan-yu")
        { SourceDifficultyId = definition.SourceDifficultyId }, StandardShanheCampaignCatalog.CreateBattleRegistry);
        var preview = session.PreviewEncounter(selectedNode.Id);
        var moved = session.Submit(new MoveShanheNodeCommand(selectedNode.Id, session.Revision));
        Need(moved.Accepted && session.PendingBattleSetup is not null && preview.Id == encounter.Id &&
            preview.Enemies.Select(member => (member.GeneralId, member.Hp, member.MaxHpBonus, member.InitialHandBonus,
                    member.DrawBonus, member.SlashBonus, member.InitialArmor, member.ArmorRecoveryPerRound, member.ExtraDamage))
                .SequenceEqual(session.PendingBattleSetup.EnemyRoster.Select(member => (member.GeneralId, member.Hp,
                    member.MaxHpBonus, member.InitialHandBonus, member.DrawBonus, member.SlashBonus, member.InitialArmor,
                    member.ArmorRecoveryPerRound, member.ExtraDamage))),
            moved.Error ?? "Real Preview and Move must carry the exact selected body and grown source profile.");
        return session.PendingBattleSetup!;
    }

    private static void AssertFactory(ShanheBattleSetup setup)
    {
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
        var general = registry.Generals[StandardShanheSourceMouDongZhuoPackage.GeneralId];
        Need(general.BaseHp == 20 && general.PortraitKey == "mou-dong-zhuo-ol" &&
            new[] { "mou-ol:guanbian", StandardShanheSourceMouDongZhuoPackage.XiongniSkillId,
                StandardShanheSourceMouDongZhuoPackage.FengshangSkillId, StandardShanheSourceMouDongZhuoPackage.ZhibingSkillId }
                .All(id => registry.Skills.ContainsKey(id)) &&
            registry.Cards.Values.Any(card => card.LegacyKind == CardKind.Duanhun) &&
            registry.Cards.Values.Any(card => card.LegacyKind == CardKind.KunlunMirror) &&
            (setup.BossTransition is null || registry.IsGeneralPlayable(setup.BossTransition.GeneralId)),
            "The actual production registry registers the source-specific skills and both physical equipment packages, along with any local future form.");
    }

    private static void AssertFreshOpening(ShanheBattleSetup setup)
    {
        var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new QuietDeck(),
            mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        var game = GameEngine.CreateStandard(new()
        {
            Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null,
            HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 3
        }, registry);
        Need(game.Submit(new StartGameCommand()).Accepted, "The real boss opening must accept StartGame.");
        for (var step = 0; !(game.CreateSnapshot(0).PendingDecision is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
                 game.ResolutionStack.Count == 0); step++)
            Need(step < 120 && game.PendingDecision is null && game.State.Winner == Winner.None &&
                game.Submit(new AdvanceOneStepCommand(game.Revision)).Accepted,
                "The initial Guanbian must finish before the human's first actual Play without unrelated choices.");
        var boss = game.CreateSnapshot(1).Players[1]; var member = setup.EnemyRoster.Single();
        Need(boss.GeneralId == StandardShanheSourceMouDongZhuoPackage.GeneralId && boss.Hp == member.Hp &&
            boss.MaxHp == 20 + member.MaxHpBonus && boss.Armor == member.InitialArmor && boss.Equipment.Count == 0 &&
            boss.Markers!.Single(marker => marker.Kind == PlayerMarkerKind.MouDongZhuoGuanbian).Count == 2 &&
            game.CardMovements.Count(move => move.To == CardLocation.Hand(1) && move.From == CardLocation.DrawPile &&
                move.Reason == CardMoveReasons.InitialDeal) == 4 + member.InitialHandBonus &&
            member.StartingCards!.All(face => boss.Hand.Count(card => card.Kind == face.Kind && card.Suit == face.Suit &&
                card.Rank == face.Rank) == 1),
            "The selected source and local boss really starts with its exact health/armor, frozen two-seat Guanbian and physical extra equipment in hand.");
    }

    private sealed class QuietDeck : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-mou-map-quiet", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddDeck(new(Deck, "固定闪牌堆", 4, 2, [new("standard:dodge", 80)]));
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
}
