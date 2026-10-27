using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceReinforcementChecks
{
    private const string Deck = "fixture:source-qiangwang-quiet-deck";

    public static void ActualSourceCitiesKeepFourIndependentRostersAndCumulativeHandEquipment()
    {
        var market = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(16);
        var defaults = StandardShanheCampaignCatalog.GetDefaultCodexUnlockedRewardIds(market);
        foreach (var (id, kind, suit) in new[] { (4050, CardKind.BlackLightArmor, Suit.Club), (4153, CardKind.GouXiang, Suit.Spade) })
        {
            var rewardId = "codex-card-" + id;
            var reward = market.Rewards.Single(candidate => candidate.Id == rewardId);
            Need(StandardShanheCampaignCatalog.ShopMetadataByRewardId[rewardId] is
                    { Price: 200, Rarity: 2, Kind: "装备", Rank: 2 } metadata && metadata.SourceRewardId == id &&
                metadata.CardKind == kind && metadata.Suit == suit &&
                reward.StartingEquipment is [var card] && card == new ShanheStartingCard(kind, suit, 2) &&
                market.Nodes.Where(node => node.Kind == ShanheNodeKind.Shop).All(node =>
                    node.Choices!.Any(choice => choice.RewardId == rewardId && choice.GoldCost == 200)) &&
                (id == 4050 ? StandardShanheCampaignCatalog.CodexRewardIdsByNodeId[23] == rewardId : defaults.Contains(rewardId)),
                "Both real source armor offers have their catalogue face, price and runtime reward; Black Light keeps its research node and Gouxiang its explicit default stock.");
        }
        foreach (var difficulty in new[] { 1, 6, 11, 16, 21, 25 })
        {
            var definition = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            foreach (var (group, city, minimum) in new[] { (2635, 5018, 1), (2655, 5068, 6), (2669, 5108, 1), (2675, 5118, 11) })
            {
                var node = definition.Nodes.SingleOrDefault(candidate => candidate.Id == "source-city-" + city);
                if (difficulty < minimum)
                {
                    Need(node is null && definition.Nodes.SelectMany(candidate => candidate.Encounters ?? [])
                        .All(encounter => encounter.SourceBattleId != group), "A source boss cannot appear before its real region opens.");
                    continue;
                }
                var encounter = node!.Encounters!.Single(candidate => candidate.SourceBattleId == group);
                var member = encounter.Enemies.Single();
                var expectedGeneral = group switch
                {
                    2635 => difficulty >= 6 ? StandardShanheSourceXuShengPackage.MediumGeneralId : StandardShanheSourceXuShengPackage.BaseGeneralId,
                    2655 => StandardShanheSourceSunCePackage.SunCeId,
                    2669 => StandardShanheSourceQiangWangPackage.GeneralId,
                    _ => StandardShanheSourceXunYuPackage.GeneralId
                };
                var gear = new List<ShanheStartingCard>();
                if (group == 2635 && difficulty >= 6) gear.Add(new(CardKind.GhostDragonCrescentBlade, Suit.Club, 5));
                if (group == 2655 && difficulty >= 16) gear.Add(new(CardKind.BlackLightArmor, Suit.Diamond, 2));
                if (group == 2669 && difficulty >= 11) gear.Add(new(CardKind.WushuangHalberd, Suit.Heart, 12));
                if (group == 2669 && difficulty >= 16) gear.Add(new(CardKind.LionBelt, Suit.Club, 2));
                if (group == 2675 && difficulty >= 11) gear.Add(new(CardKind.GouXiang, Suit.Club, 2));
                Need(encounter.SourceCityId == city && encounter.SourceVisibleEnemyCount == 1 && encounter.BossTransition is null &&
                    member.GeneralId == expectedGeneral && member.StartingHand is null && member.InitialHandBonus == 0 &&
                    (member.StartingEquipment?.Count ?? 0) == 0 && (member.StartingCards ?? []).SequenceEqual(gear) &&
                    member.Hp == (group is 2655 or 2675 ? 3 : 4),
                    "Each real source city retains its own single body, four random opening cards and cumulative extra hand equipment, without inherited template companions or equipment.");
                var setup = new ShanheBattleSetup(71, 1, node.Id, "classic:guan-yu", 4, 0, 0, 0, 0,
                    [], [], [], encounter.Enemies, [], ShanheCampaignContent.GetContentHash(definition)) { SourceDifficultyId = difficulty };
                var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup);
                Need(registry.Generals.ContainsKey(expectedGeneral) && gear.All(card =>
                    registry.Cards.Values.Any(content => content.LegacyKind == card.Kind)),
                    "The production factory must register the actually selected source form and every cumulative physical equipment kind.");
            }
        }
    }

    public static void InitialDongdangUsesItsEffectiveOwnerBeforeTheFirstNaturalTurn()
    {
        foreach (var (difficulty, effective) in new[] { (1, true), (16, true), (16, false) })
        {
            var source = StandardShanheCampaignCatalog.CreateSourceCodexDefinition(difficulty);
            var node = source.Nodes.Single(candidate => candidate.Id == "source-city-5108");
            var encounter = node.Encounters!.Single(candidate => candidate.SourceBattleId == 2669);
            var setup = new ShanheBattleSetup(71, 1, node.Id, "classic:guan-yu", 34, 30, 0, 0, 0,
                [], [], [], encounter.Enemies, [], ShanheCampaignContent.GetContentHash(source)) { SourceDifficultyId = difficulty };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup).WithExtension(new QuietDeck(),
                mode => mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
            if (!effective) setup = setup with { EnemyRoster = [setup.EnemyRoster.Single() with { GeneralId = "classic:guan-yu" }] };
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId,
                ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup, UseInteractiveSetup = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 6
            }, registry);
            Submit(game, new StartGameCommand());
            for (var step = 0; !(game.CreateSnapshot(0).PendingDecision is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
                     game.ResolutionStack.Count == 0); step++)
            {
                Need(step < 300 && game.State.Winner == Winner.None, "The initial-entry fixture must reach the next human Play without a completed match.");
                Need(game.CreateSnapshot(0).PendingDecision is null, "The all-Dodge fixture must not invent an unrelated human choice.");
                Submit(game, new AdvanceOneStepCommand(game.Revision));
            }
            var entries = Facts<ShanheSourceDongdangInitialEntryEvent>(game);
            Need(Facts<TurnStartedEvent>(game).First().ActorSeat == (effective ? 1 : 0) &&
                entries.Length == (effective ? 1 : 0) &&
                (!effective || entries is [{ OwnerSeat: 1, GeneralId: StandardShanheSourceQiangWangPackage.GeneralId,
                    PreviousStartingSeat: 0, Source: { OwnerSeat: 1, SkillId: ShanheSourceDongdangRules.SkillId,
                        BindingId: ShanheSourceDongdangRules.InitialBindingId } }] && !string.IsNullOrWhiteSpace(entries[0].Source.SkillInstanceId)) &&
                Facts<TurnEndedEvent>(game).All(fact => fact.TurnNumber > 0) &&
                Facts<ShanheBossStageStartedEvent>(game).Length == 0 && Facts<ShanheBossTurnInterruptedEvent>(game).Length == 0,
                "Only the effective actual initial owner starts first; initial entry does not invent a retired turn zero or a second-form transformation.");
            Need(game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(1) &&
                    move.Reason == CardMoveReasons.InitialDeal) == 4 &&
                (setup.EnemyRoster.Single().StartingCards ?? []).All(card => game.CardMovements.Count(move =>
                    move.From == CardLocation.OutsideGame && move.To == CardLocation.Hand(1) &&
                    move.CardKind == card.Kind) == 1),
                "Initial Dongdang retains the four real random opening entities and each configured extra equipment hand entity exactly once.");
        }
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static void Submit(GameEngine game, GameCommand command)
    { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Rejected source-entry command."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
    private sealed class QuietDeck : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-qiangwang-quiet", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder) => builder.AddDeck(new(Deck, "真实闪牌堆", 4, 2, [new("standard:dodge", 80)]));
    }
}
