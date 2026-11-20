using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class WuLuKangShenduanChecks
{
    private const string Shenduan = "wumiao:shenduan", Driver = "fixture:shenduan-contest", Watcher = "fixture:shenduan-watch-draw";
    private const string Mode = "identity:shenduan-fixture", Deck = "fixture:shenduan-deck";
    private const string Bottom = "skill-program.wu-lu-kang.shenduan.bottom", Draw = "skill-program.wu-lu-kang.shenduan.draw";
    private static readonly string[] Generals = ["fixture:shenduan-owner", "fixture:shenduan-target", "fixture:shenduan-peer-2", "fixture:shenduan-peer-3"];

    internal static void WinningOriginalEntryBottomsBeforeKingDrawAndReplays()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 47, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        ReachSetup();
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            ReachSetup();
        }

        var hand = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        Accept(game, new UseProgramSkillCommand(0, Driver, "contest", [hand], [1], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: 0 } && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-top"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("action") == "pindian-top");
        var entry = game.CardMovements.Single(move => move.Reason.Value == "pindian.reserve-top");
        Reach(game, prompt => prompt is { PlayerSeat: 1 } && prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card"));
        Answer(game, choice => choice.Parameters.GetValueOrDefault("action") == "pindian-card");
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Shenduan });
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "decline");

        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Watcher });
        var contest = game.ResolutionStack.OfType<PindianFrame>().Single();
        Require(contest.Result is { SourceRank: 13, OpponentRank: 7, SourceWon: true } result && result.SourceCardId == entry.CardId &&
            contest.ProcessingEntries!.Single(material => material.CardId == entry.CardId).EntrySequence == entry.Sequence &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.WuLuKangShenduan is
                { Stage: WuLuKangShenduanStage.DrawChildren, Draw.ActualDrawIds.Count: 1 }),
            "Tianbian's original reservation wins with final rank 13, and Shenduan awaits the actual King draw child.");
        CheckPaid(game);
        ReplayFourViews(game, content);
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        foreach (var run in new[] { game, restored })
        {
            Answer(run, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
            Settle(run);
            CheckPaid(run);
            Require(run.CardMovements.Count(move => move.Reason == CardMoveReasons.PindianFinish && move.CardId == contest.Result!.OpponentCardId) == 1 &&
                !run.CardMovements.Any(move => move.Reason == CardMoveReasons.PindianFinish && move.CardId == entry.CardId),
                "The original losing card is discarded once after the child returns, and the bottomed winner stays in the deck.");
        }

        void ReachSetup() => Reach(game, prompt => prompt.Kind == DecisionKind.SelectGeneral || prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });

        void CheckPaid(GameEngine run)
        {
            var bottom = run.CardMovements.Single(move => move.Reason.Value == Bottom);
            var draw = run.CardMovements.Single(move => move.Reason.Value == Draw);
            Require(run.CardMovements.Count(move => move.CardId == entry.CardId && move.To == CardLocation.Processing) == 1 &&
                bottom.CardId == entry.CardId && bottom.From == CardLocation.Processing && bottom.To == CardLocation.DrawPile &&
                draw.CardId != entry.CardId && draw.From == CardLocation.DrawPile && draw.To == CardLocation.Hand(0) &&
                entry.Sequence < bottom.Sequence && bottom.Sequence < draw.Sequence &&
                run.CreateCardZoneDiagnostics().Single(card => card.CardId == entry.CardId) is { Location.Zone: CardZoneKind.DrawPile, ZoneIndex: 0 },
                "Shenduan puts the actual winning entry at the bottom once, before the King participant's native draw.");
        }
    }

    internal static void EmptyHandTianyiPaysActualEquipmentForMaximumCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new EquipmentFixture());
        var unavailable = Start(null);
        var revision = unavailable.Revision;
        Require(!unavailable.GetHumanLegalActions().Any(action => action.SkillId == EquipmentFixture.Tianyi) &&
            !unavailable.Submit(new UseProgramSkillCommand(0, EquipmentFixture.Tianyi, "contest", [], [1], revision, P(unavailable)!.PromptId)).Accepted &&
            unavailable.Revision == revision, "The same empty hand without equipment cannot advertise or accept an empty contest selection.");

        var game = Start(new(CardKind.Crossbow, Suit.Club, 8));
        var owner = game.CreateSnapshot(0).Players[0];
        var equipment = owner.Equipment.Single().Id;
        var action = game.GetHumanLegalActions().Single(item => item.SkillId == EquipmentFixture.Tianyi);
        var target = action.SelectableTargetSeats.Single();
        Require(owner.HandCount == 0 && action.MinCardCount == 0 && action.SelectableCardIds.Count == 0 &&
            game.CreateSnapshot(0).Players[target].GeneralId == Generals[1],
            "Tianyi advertises the native empty selection while actual equipment can pay Shenduan.");
        Accept(game, new UseProgramSkillCommand(0, EquipmentFixture.Tianyi, "contest", [], [target], game.Revision, P(game)!.PromptId));
        var native = game.ResolutionStack.OfType<PindianFrame>().Single();
        var admission = game.Events.Select(item => item.Payload).OfType<WuLuKangPindianSelectionAdmittedEvent>().Single().Admission;
        Require(native is { SourceSeat: 0, SourceCardId: 0, PindianStep: PindianStep.ChooseOpponentCard } && native.OpponentSeat == target &&
            admission.FrameId == native.ParentFrameId && admission.ProgramSource is { SkillId: EquipmentFixture.Tianyi, BindingId: "contest" } &&
            admission.CapabilitySource is { SkillId: Shenduan, BindingId: "maximum-deck-source" } &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == equipment).Location == CardLocation.Equipment(0),
            "The original activation admits its own Shenduan instance without paying equipment before the private contest.");
        Accept(game, new AdvanceOneStepCommand(game.Revision));
        Reach(game, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Shenduan });
        Require(P(game)!.Choices.Where(choice => choice.Parameters.GetValueOrDefault("branch") == "discard")
            .Single().Cards.SequenceEqual([equipment]), "The private payment offers only the original equipment entity.");

        ReplayFourViews(game, content);
        game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == "discard" && choice.Cards.SequenceEqual([equipment]));
        Settle(game);
        var paid = game.Events.Select(item => item.Payload).OfType<WuLuKangShenduanDiscardPaidEvent>().Single();
        var result = game.Events.Select(item => item.Payload).OfType<PindianResultDeterminedEvent>().Single();
        var maximum = game.CardMovements.Single(move => move.Reason.Value == "skill-program.wu-lu-kang.shenduan.reserve");
        Require(paid.ContestId == native.Id && paid.CardId == equipment && paid.From == CardLocation.Equipment(0) &&
            paid.To == CardLocation.DiscardPile && paid.Sequence < maximum.Sequence && result.FrameId == native.Id &&
            result.Result is { SourceRank: 13, OpponentRank: 7, SourceWon: true } &&
            result.Result.SourceCardId == maximum.CardId && maximum.From == CardLocation.DrawPile && maximum.To == CardLocation.Processing &&
            game.CardMovements.Count(move => move.CardId == equipment && move.From == CardLocation.Equipment(0)) == 1,
            "Returning the original prompt pays the actual equipment once and uses the original contest's maximum deck entity.");

        GameEngine Start(ShanheStartingCard? equipmentCard)
        {
            var setup = new ShanheBattleSetup(7, 1, "shenduan-equipment", EquipmentFixture.Owner, 4, 0, 0, 0, 0, [], [],
                equipmentCard is { } card ? [card] : [],
                [new(Generals[1]) { StartingHand = [new(CardKind.Dodge, Suit.Spade, 7)] }, new(Generals[2]) { StartingHand = [] }],
                [new(Generals[3]) { StartingHand = [] }], new string('d', 64));
            var match = GameEngine.CreateStandard(new GameOptions
            {
                Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
                HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
                UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 4
            }, content);
            Accept(match, new StartGameCommand());
            Settle(match);
            return match;
        }
    }

    private sealed class EquipmentFixture : IGameContentPackage
    {
        internal const string Owner = "fixture:shenduan-equipment-owner", Tianyi = "classic:tianyi";
        private const string EquipmentDeck = "fixture:shenduan-equipment-deck";
        public PackageManifest Manifest { get; } = new("fixture-shenduan-equipment", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "wumiao:lu-kang", "classic:taishi-ci");
            builder.AddGeneral(new(Owner, "审断只付装备", "supporter", Tianyi, "wu", 4, [Shenduan]));
            foreach (var id in Generals.Skip(1))
                builder.AddGeneral(new(id, "固定同桌", "supporter", "standard:none", "wei", 10));
            builder.AddDeck(new(EquipmentDeck, "固定十三点", 0, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 40)
                    .Select(_ => new ContentDeckPhysicalCard("standard:dodge", Suit.Heart, 13)).ToArray()
            });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "实际空手审断", 4, 4, new Dictionary<string, int>(), EquipmentDeck,
                GeneralPoolIds: [Owner, .. Generals.Skip(1)], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shenduan-bottom", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "wumiao:lu-kang", "classic:qin-mi");
            var rules = $$$"""
                {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[
                  {"id":"{{{Driver}}}","revision":1,"activations":[{"id":"contest","minCards":1,"maxCards":1,"sourceZones":["hand"],
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLivingWithHand","usesPerTurn":1,
                    "effects":[{"op":"pindian","target":"selectedTarget","amount":1}]}]},
                  {"id":"{{{Watcher}}}","revision":1,"triggers":[{"id":"after-king-draw","window":"cardsGained","subject":"owner",
                    "destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["{{{Draw}}}"],"optional":false,
                    "effects":[{"op":"chooseOption","target":"owner","resultBind":"acknowledged","options":[{"id":"continue"}]}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Driver] = new { name = "拼点", description = "发起一次真实拼点。" },
                    [Watcher] = new { name = "摸牌观察", description = "审断摸牌后等待。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } }
                } });
            var catalog = SkillProgramCatalog.Load(rules, presentation);
            foreach (var id in new[] { Driver, Watcher })
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });

            builder.AddGeneral(new(Generals[0], "审断角色", "supporter", Driver, "wu", 4, [Shenduan, "classic:tianbian", Watcher]));
            foreach (var id in Generals.Skip(1))
                builder.AddGeneral(new(id, "其他角色", "supporter", "standard:none", "wei", 4));
            builder.AddDeck(new(Deck, "固定红桃七", 4, 2, []) { PhysicalCards = Enumerable.Range(0, 48)
                .Select(_ => new ContentDeckPhysicalCard("standard:crossbow", Suit.Heart, 7)).ToArray() });
            builder.AddMode(new(Mode, "审断置底", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
