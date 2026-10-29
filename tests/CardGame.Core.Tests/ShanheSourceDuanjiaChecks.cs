using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSourceDuanjiaChecks
{
    private const string Peer = "fixture:source-duanjia-peer", Deck = "fixture:source-duanjia-deck";
    private const string Capacity = "fixture:source-duanjia-capacity";

    public static void MissingArmorCreatesNativeEntityOutsideDrawPileAndPreservesSlots()
    {
        foreach (var (filled, available, capacity, general, expectedNew) in new[]
        {
            (false, true, 1, StandardShanheSourceArmorerPackage.BaseId, 1),
            (true, true, 2, StandardShanheSourceArmorerPackage.BaseId, 0),
            (false, false, 1, StandardShanheSourceArmorerPackage.BaseId, 0),
            (false, true, 0, StandardShanheSourceArmorerPackage.BaseId, 0),
            (true, true, 2, StandardShanheEliteHandangPackage.HandangId, 1)
        })
        {
            var equipment = new[] { new ShanheStartingCard(CardKind.QinggangSword, Suit.Spade, 6) }
                .Concat(filled ? [new ShanheStartingCard(CardKind.SilverLion, Suit.Club, 1)] : []).ToArray();
            var setup = new ShanheBattleSetup(109, 1, "source-duanjia-native", general, 5,
                0, 0, 0, 0, [Capacity], [], equipment,
                [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
                [], new string('d', 64)) { StartingResourcesExpireOnUse = true };
            var registry = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with
                { EnemyRoster = setup.EnemyRoster.Select(p => p with { GeneralId = "classic:guan-yu" }).ToArray() })
                .WithExtension(new Fixture(available, capacity), mode => mode.Id == ShanheBattleSetup.BattleModeId
                    ? mode with { DeckId = Deck, GeneralPoolIds = mode.GeneralPoolIds!.Append(Peer).Distinct().ToArray() } : mode);
            var game = GameEngine.CreateStandard(new()
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
                ShanheBattle = setup, UseInteractiveSetup = false, ConfirmOpeningHand = false,
                UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
            }, registry);
            var original = game.CreateSnapshot(0).Players[0].Equipment.Select(c => c.Id).ToArray();
            Submit(new StartGameCommand());
            for (var step = 0; game.CreateSnapshot(0).PendingDecision is not { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }; step++)
            {
                Need(step < 140 && game.CreateSnapshot(0).PendingDecision is null && game.State.Winner == Winner.None,
                    "The fresh native placement reaches first human Play without another actor's turn.");
                Submit(new AdvanceOneStepCommand(game.Revision));
            }
            var owner = game.CreateSnapshot(0).Players[0];
            var created = game.Events.Select(e => e.Payload).OfType<ShanheRandomEquipmentCreatedEvent>().ToArray();
            var placed = game.CardMovements.Where(m => m.From == CardLocation.OutsideGame &&
                m.To == CardLocation.Equipment(0) && created.Any(e => e.CardId == m.CardId)).ToArray();
            Need(placed.Length == expectedNew && placed.All(m => m.Reason == CardMoveReasons.EquipmentEnter) &&
                placed.Select(m => m.CardId).Distinct().Count() == placed.Length &&
                created.Length == expectedNew && created.All(e => e.OwnerSeat == 0 && e.CardKind == CardKind.SilverLion &&
                    e.Suit == Suit.Club && e.Rank == 1 && placed.Count(m => m.CardId == e.CardId) == 1) &&
                owner.Equipment.Count == original.Length + expectedNew && original.All(id => owner.Equipment.Any(c => c.Id == id)) &&
                placed.All(m => owner.Equipment.Single(c => c.Id == m.CardId).Kind == CardKind.SilverLion) &&
                game.Events.Select(e => e.Payload).OfType<EquipmentChangedEvent>().Count(e => placed.Any(m => m.CardId == e.CardId)) == expectedNew &&
                !game.Events.Select(e => e.Payload).OfType<CardUseDeclaredEvent>().Any(e => placed.Any(m => m.CardId == e.CardId)) &&
                !game.CardMovements.Any(m => placed.Any(p => p.CardId == m.CardId) && m.To == CardLocation.Processing) &&
                !game.CardMovements.Any(m => EquipmentCatalog.IsEquipment(m.CardKind) &&
                    (m.From == CardLocation.DrawPile || m.To == CardLocation.DrawPile)) &&
                owner.EquipmentSlotCapacities!.GetValueOrDefault(EquipmentSlot.Armor, 1) == capacity &&
                game.State.CurrentSeat == 0 && game.State.TurnNumber == 1 && game.ResolutionStack.Count == 0,
                "Duanjia only creates and places one registered armor when none is equipped and a slot remains; no armor template or abolished slot does nothing. Existing Zizhong still fills spare capacity beside an equipped armor. Each native entity enters once directly from OutsideGame, with no replacement or use declaration, while the actual draw pile remains equipment-free." +
                JsonSerializer.Serialize(new { filled, available, capacity, general, expectedNew, original, placed, created,
                    owner.Equipment, owner.EquipmentSlotCapacities, game.State.CurrentSeat, game.State.TurnNumber,
                    Frames = game.ResolutionStack.Select(f => f.GetType().Name),
                    Recent = game.Events.TakeLast(8).Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())) }));

            void Submit(GameCommand command)
            { var result = game.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Source14095 native command rejected."); }
        }
    }

    private static void Need(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture(bool armorAvailable, int capacity) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:source-duanjia", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Capacity}}","revision":1,"triggers":[{"id":"initial-capacity","window":"gameStarting","subject":"owner","optional":false,
                    "effects":[{"op":"alterEquipmentSlots","target":"owner","equipmentSlots":["armor"],"amount":{{capacity}}}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Capacity] = new { name = "固定防具栏", description = "测试实际防具栏容量" } } }));
            builder.AddSkill(new(Capacity, catalog.Presentations[Capacity].Name, catalog.Presentations[Capacity].Description)
                { Program = catalog.Programs[Capacity], ProgramPresentation = catalog.Presentations[Capacity] });
            builder.AddGeneral(new(Peer, "短局角色", "supporter", null, "wei", 10));
            builder.AddDeck(new(Deck, "实际装备牌堆", 0, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 48).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3))
                    .Concat(Enumerable.Range(0, 16).Select(_ => new ContentDeckPhysicalCard("standard:qinggang_sword", Suit.Spade, 6)))
                    .Concat(armorAvailable ? Enumerable.Range(0, 16).Select(_ => new ContentDeckPhysicalCard("classic:silver-lion", Suit.Club, 1)) : [])
                    .ToArray()
            });
        }
    }
}
