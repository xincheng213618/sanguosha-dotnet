using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShenwuFieldEquipmentChecks
{
    private const string Driver = "fixture:shenwu-card-driver", Owner = "fixture:shenwu-card-owner";
    private const string Mode = "fixture:shenwu-card-mode", Deck = "fixture:shenwu-card-deck";
    private const string FirstAlly = "fixture:shenwu-card-peer-1", SecondAlly = "fixture:shenwu-card-peer-2";
    private const string Enemy = "fixture:shenwu-card-boss";

    internal static void QimenAndDustlessUseTheirOriginalFieldEquipmentCold()
    {
        var (armor, armorContent) = Start(CardKind.QimenEightTrigrams);
        var equipped = Equip(armor, CardKind.QimenEightTrigrams, EquipmentSlot.Armor);
        armor = Cold(armor, armorContent);
        var hp = armor.CreateSnapshot(0).Players[1].Hp;
        var slash = armor.GetLegalActions(0).First(action => action.Kind == LegalActionKind.Slash &&
            action.ConversionSource is null && action.TargetSeats.SequenceEqual([1]));
        var card = slash.CardId!.Value;
        var eventStart = armor.Events.Count;
        Accept(armor, new PlayCardCommand(0, card, [1], armor.Revision, P(armor)!.PromptId, CardKind.Slash));
        Settle(armor);
        var use = Facts<CardUseDeclaredEvent>(armor).Single(fact => fact.CardId == card && fact.SourceSeat == 0);
        Require(Facts<ArmorEffectAppliedEvent>(armor).SequenceEqual([new(use.ResolutionId, CardKind.QimenEightTrigrams, 0, 1, CardKind.Slash)]) &&
            Facts<CardUseFinishedEvent>(armor).Count(fact => fact.ResolutionId == use.ResolutionId && fact.CardId == card && fact.CardKind == CardKind.Slash) == 1 &&
            armor.CardMovements.Count(move => move.CardId == card && move.From == CardLocation.Hand(0) &&
                move.To == CardLocation.Processing && move.Reason == CardMoveReasons.Use) == 1 &&
            !armor.Events.Skip(eventStart).Any(entry => entry.Payload is ResponseRequestedEvent { SourceSeat: 0, TargetSeat: 1, RequiredCardKind: CardKind.Dodge }
                or DamageAppliedEvent { TargetSeat: 1 }) && armor.CreateSnapshot(0).Players[1] is { } holder &&
            holder.Hp == hp && holder.Equipment.Single().Id == equipped.CardId,
            "The original equipped Qimen blocks the ordinary physical Slash before Dodge and that same native use finishes once.");

        var (distance, distanceContent) = Start(CardKind.DustlessGoldenSpear);
        var seats = distance.CreateSnapshot(0).Players;
        Require(seats[0].TeamId == "team:allies" && seats[1].TeamId == seats[0].TeamId && seats[2].TeamId == seats[0].TeamId &&
            seats[3].TeamId == "team:boss", "The fixed activity has three original allies and a distinct enemy.");
        var enemyToPeer = distance.GetCombatDistance(3, 2);
        var peerToEnemy = distance.GetCombatDistance(2, 3);
        var allyToPeer = distance.GetCombatDistance(0, 2);
        var enemyToHolder = distance.GetCombatDistance(3, 1);
        var spear = Equip(distance, CardKind.DustlessGoldenSpear, EquipmentSlot.DefensiveHorse);
        CheckDistances(distance);
        CheckDistances(Cold(distance, distanceContent));

        void CheckDistances(GameEngine game) => Require(game.GetCombatDistance(3, 2) == enemyToPeer + 1 &&
            game.GetCombatDistance(2, 3) == peerToEnemy && game.GetCombatDistance(0, 2) == allyToPeer &&
            game.GetCombatDistance(3, 1) == enemyToHolder + 1 && game.GetCombatDistance(3, 1) == game.GetSeatDistance(3, 1) + 1 &&
            game.CreateSnapshot(0).Players[1].Equipment.Single().Id == spear.CardId,
            "Dustless extends enemy distance to the holder's other ally only in that direction. The holder has its own printed defensive horse effect.");
    }

    private static EquipmentChangedEvent Equip(GameEngine game, CardKind kind, EquipmentSlot slot)
    {
        var before = game.CardMovements.Count;
        Accept(game, new UseProgramSkillCommand(0, Driver, "equip", [], [1], game.Revision, P(game)!.PromptId));
        Settle(game);
        var entry = Facts<EquipmentChangedEvent>(game).Single(fact => fact.PlayerSeat == 1 && fact.CardKind == kind && fact.Slot == slot);
        var moves = game.CardMovements.Skip(before).Where(move => move.CardId == entry.CardId).ToArray();
        Require(moves is [var payment, var installed] && payment.From == CardLocation.DrawPile && payment.To == CardLocation.Processing &&
            payment.Reason == CardMoveReasons.EquipmentUse && installed.From == payment.To && installed.To == CardLocation.Equipment(1) &&
            installed.Reason == CardMoveReasons.EquipmentEnter && Facts<CardUseFinishedEvent>(game).Count(fact =>
                fact.ResolutionId == entry.ResolutionId && fact.CardId == entry.CardId && fact.CardKind == kind) == 1,
            $"The public action installs and completes the exact original {kind} from the real draw pile.");
        return entry;
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            Facts<EquipmentChangedEvent>(restored).SequenceEqual(Facts<EquipmentChangedEvent>(game)) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay keeps the native installed entity, its placement and each seat's view.");
        return restored;
    }

    private static (GameEngine, ContentRegistry) Start(CardKind kind)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new StandardActiveSkillExpansionPackage(true),
            new StandardRescueSkillExpansionPackage(), new StandardClassicGeneralPackage(), new StandardShenwuActivityPackage(), new Fixture(kind));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = null, HumanTeamId = "team:allies", ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8
        }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral });
        Accept(game, new SelectGeneralCommand(0, Owner, game.Revision, P(game)!.PromptId));
        Settle(game);
        return (game, content);
    }

    private sealed class Fixture(CardKind kind) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shenwu-field-equipment", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{Driver}}","revision":1,
                  "modifiers":[{"id":"opening-hand","query":"initialHandSize","operation":"add","value":27,"priority":0}],
                  "activations":[{"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"equipped"}]}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "取用装备", description = "令目标使用牌堆中的固定种类装备牌。" } } }));
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Owner, "实际出牌者", "supporter", Driver, "wei", 5));
            foreach (var ally in new[] { FirstAlly, SecondAlly }) builder.AddGeneral(new(ally, "同队", "supporter", "standard:none", "wei", 8));
            builder.AddGeneral(new(Enemy, "敌队", "supporter", "standard:none", "qun", 8));
            var cardId = kind == CardKind.QimenEightTrigrams ? "shenwu:equipment-qimen-eight-trigrams" : "shenwu:equipment-dustless-golden-spear";
            builder.AddDeck(new(Deck, "固定神武装备", 1, 0, []) { PhysicalCards = Enumerable.Range(0, 180)
                .Select(index => new ContentDeckPhysicalCard(index % 4 == 0 ? cardId : "standard:slash", Suit.Heart, index % 13 + 1)).ToArray() });
            builder.AddMode(new(Mode, "真实神武装备", 4, 4, new Dictionary<string, int>(), Deck, 3,
                [Owner, FirstAlly, SecondAlly, Enemy], ContentModeKind.Team, new Dictionary<string, int> { ["team:allies"] = 3, ["team:boss"] = 1 })
            { BossActivity = new([Owner, FirstAlly, SecondAlly], [new("equipment", [Enemy], 0)], 1, []) });
        }
    }
}
