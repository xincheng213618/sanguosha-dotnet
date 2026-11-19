using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
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
