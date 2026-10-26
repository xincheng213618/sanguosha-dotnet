using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheEliteHandangChecks
{
    public static void EmptySlotUsesRealDeckEquipmentAndMengwuDrawsAfterOwnDodgeCold()
    {
        foreach (var armorFilled in new[] { false, true })
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
                new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(),
                new StandardShanheEliteHandangPackage(), new Fixture());
            var setup = new ShanheBattleSetup(91, 1, "fixture:elite-handang", StandardShanheEliteHandangPackage.HandangId,
                5, 0, 0, 0, 0, [], [new(CardKind.Slash, Suit.Spade, 7), new(CardKind.Slash, Suit.Club, 8)],
                armorFilled ? [new(CardKind.QinggangSword, Suit.Spade, 6), new(CardKind.SilverLion, Suit.Club, 1)]
                    : [new(CardKind.QinggangSword, Suit.Spade, 6)],
                [new("fixture:elite-handang-peer") { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] },
                 new("fixture:elite-handang-peer") { StartingHand = [] }, new("fixture:elite-handang-peer") { StartingHand = [] }],
                [new("fixture:elite-handang-peer") { StartingHand = [] }, new("fixture:elite-handang-peer") { StartingHand = [] }], new string('h', 64));
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 6, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            var originalWeapon = game.CreateSnapshot(0).Players[0].Equipment.Single(card => card.Kind == CardKind.QinggangSword).Id;
            Submit(game, new StartGameCommand()); ReachPlay(game);
            var placements = game.CardMovements.Where(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Equipment(0)).ToArray();
            Need(placements.Length == (armorFilled ? 0 : 1) && placements.All(move => move.CardKind == CardKind.SilverLion) &&
                game.CreateSnapshot(0).Players[0].Equipment.Any(card => card.Id == originalWeapon) &&
                !Events<CardUseDeclaredEvent>(game).Any(use => placements.Any(move => move.CardId == use.CardId)) &&
                placements.All(placed => !game.CardMovements.Any(move => move.CardId == placed.CardId && move.From == CardLocation.OutsideGame)),
                "Zizhong places one actual draw-pile entity into an empty slot, never replaces the occupied weapon, generates a copy or declares a card Use; no eligible slot means no placement.");
            game = Cold(game, registry);
            if (armorFilled) continue;

            var beforeHand = game.CreateSnapshot(0).Players[0].HandCount;
            var slash = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([3]));
            Need(game.GetAttackRange(0) == 2 && slash.TargetSeats.SequenceEqual([3]),
                "A real opponent at six-seat distance three remains a legal Slash target through Mengwu, beyond the installed two-range weapon.");
            Submit(game, new PlayCardCommand(0, slash.CardId!.Value, [3], game.Revision, Prompt(game)!.PromptId, slash.PlayedCardKind));
            ReachPlay(game);
            var reason = "skill-program." + StandardShanheEliteHandangPackage.MengwuId + ".Draw";
            Need(game.CardMovements.Count(move => move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0) && move.Reason.Value == reason) == 1 &&
                game.CreateSnapshot(0).Players[0].HandCount == beforeHand &&
                game.GetHumanLegalActions().Any(action => action.Kind == LegalActionKind.Slash && action.TargetSeats.SequenceEqual([3])),
                "The owner's real Slash is paid once and fully dodged, Mengwu draws one native card, and another Slash stays legal despite the ordinary one-use quota.");
            _ = Cold(game, registry);
        }
    }

    private static T[] Events<T>(GameEngine game) where T : IGameEvent => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static PendingDecision? Prompt(GameEngine game) => game.CreateSnapshot(0).PendingDecision;
    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Elite Han Dang native command rejected.");
    }
    private static void ReachPlay(GameEngine game)
    {
        for (var step = 0; step < 160; step++)
        {
            if (Prompt(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }) return;
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Elite Han Dang missed its native play boundary: " + JsonSerializer.Serialize(Prompt(game)));
    }
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(SnapshotJson.Serialize(game.CreateSnapshot(0)) == SnapshotJson.Serialize(restored.CreateSnapshot(0)) &&
            JsonSerializer.Serialize(game.Events) == JsonSerializer.Serialize(restored.Events) &&
            JsonSerializer.Serialize(game.CardMovements) == JsonSerializer.Serialize(restored.CardMovements),
            "Cold command replay preserves the one native installation, actual equipment source and one cancelled-Slash draw.");
        return restored;
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shanhe-elite-handang", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddGeneral(new("fixture:elite-handang-peer", "固定短局角色", "supporter", null, "wei", 10));
            builder.AddDeck(new("fixture:elite-handang-deck", "真实装备牌堆", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 20).Select(_ => new ContentDeckPhysicalCard("classic:silver-lion", Suit.Club, 1))
                    .Concat(Enumerable.Range(0, 20).Select(_ => new ContentDeckPhysicalCard("standard:qinggang_sword", Suit.Spade, 6)))
                    .Concat(Enumerable.Range(0, 40).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3))).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "精英韩当原生短局", 6, 6, new Dictionary<string, int>(), "fixture:elite-handang-deck",
                GeneralPoolIds: [StandardShanheEliteHandangPackage.HandangId, "fixture:elite-handang-peer"], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 3, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
