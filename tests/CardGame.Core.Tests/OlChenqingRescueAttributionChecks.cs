using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlChenqingRescueAttributionChecks
{
    private const string SunQuan = "classic:sun-quan", CaiWenJi = "ol:sp-cai-wen-ji", Chenqing = "ol:chenqing", Jiuyuan = "classic:jiuyuan";
    private const string Source = "fixture:chenqing-rescue-source", Peer = "fixture:chenqing-rescue-peer";
    private const string Driver = "fixture:chenqing-rescue-driver", Watch = "fixture:chenqing-rescue-payment";
    private const string Mode = "identity:classic-chenqing-rescue-source", Deck = "fixture:chenqing-rescue-deck";
    private const string DiscardReason = "skill-program.ol:chenqing.DrawThenDiscardSuitsForDyingPeach.discard";
    private const int Lord = 0, SkillOwner = 1, Payer = 2;
    private static readonly string[] Generals = [SunQuan, CaiWenJi, Source, Peer];

    internal static void ChenqingPeachUsesActualPayerFactionForJiuyuanCold()
    {
        VerifyRescue("wei", 1);
        VerifyRescue("wu", 2);
    }

    private static void VerifyRescue(string faction, int expectedRecovery)
    {
        var (game, content) = Start(faction);
        for (var seat = Lord; seat < Payer; seat++)
        {
            Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == seat);
            Accept(game, new EndPlayPhaseCommand(seat, game.Revision, P(game)!.PromptId));
        }

        Reach(game, prompt => prompt.Kind == DecisionKind.PlayCard && prompt.PlayerSeat == Payer);
        Accept(game, new UseProgramSkillCommand(Payer, Driver, "enter-dying", [], [Lord], game.Revision, P(game)!.PromptId));
        Reach(game, prompt => prompt is { PlayerSeat: SkillOwner, SkillPrompt.SkillId: Chenqing });
        var dying = game.ResolutionStack.OfType<DyingFrame>().Single();
        Require(dying.VictimSeat == Lord && game.CreateSnapshot(Lord).Players[Lord] is { Role: Role.Lord, IsAlive: true, Hp: -1 },
            "The selected classic Sun Quan is the Lord and needs two recovery points in the original Dying window.");
        Activate(game);
        Reach(game, prompt => IsChenqingSelection(prompt, SkillOwner) && prompt.ValidTargetSeats.Contains(Payer));
        Answer(game, choice => choice.Targets.SequenceEqual([Payer]));
        Reach(game, prompt => IsChenqingSelection(prompt, Payer) && prompt.ValidCardIds.Count >= 4);
        var cards = game.CreateSnapshot(Payer).Players[Payer].Hand.Where(card => P(game)!.ValidCardIds.Contains(card.Id))
            .GroupBy(card => card.Suit).Select(group => group.First().Id).ToArray();
        Require(cards.Length == 4, "Chenqing offers real discard payments of all four suits to its selected player.");
        foreach (var card in cards)
            Answer(game, choice => choice.Cards.SequenceEqual([card]));

        Reach(game, prompt => prompt is { PlayerSeat: Payer, SkillPrompt.SkillId: Watch });
        var payment = Facts<DyingSuitsDiscardPaidEvent>(game).Single();
        Require(payment.RecipientSeat == Payer && payment.CardIds.SequenceEqual(cards) && payment.Suits.Distinct().Count() == 4 &&
            cards.All(card => game.CardMovements.Count(move => move.CardId == card && move.To == CardLocation.DiscardPile &&
                move.Reason.Value == DiscardReason && move.Sequence > payment.Before && move.Sequence <= payment.After) == 1) &&
            Facts<DyingSuitsPeachIssuedEvent>(game).Length == 0,
            "The real payment observer pauses after the four physical discards and before the original Peach is issued.");
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "continue");
        for (var step = 0; step < 50 && !Facts<DyingResolvedEvent>(game).Any(fact => fact.ResolutionId == dying.Id); step++)
            Accept(game, new AdvanceOneStepCommand(game.Revision));

        var peach = Facts<DyingSuitsPeachIssuedEvent>(game).Single().Return;
        Require(peach.DyingFrameId == dying.Id && peach.VictimSeat == Lord && peach.ActorSeat == Payer &&
            Facts<CardUseDeclaredEvent>(game).Count(fact => fact.ResolutionId == peach.CardUseFrameId && fact is { CardId: 0, CardKind: CardKind.Peach, SourceSeat: Payer }) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == peach.CardUseFrameId) == 1 &&
            Facts<DyingSuitsPeachReturnedEvent>(game).Count(fact => fact.Return == peach) == 1,
            "The payer uses one actual zero-material Peach on the original victim and returns once to Chenqing.");
        var recovery = Facts<RecoveryAppliedEvent>(game).Single(fact => fact.TargetSeat == Lord);
        Require(recovery == new RecoveryAppliedEvent(Payer, Lord, expectedRecovery, expectedRecovery - 1),
            $"The actual {faction} Chenqing Peach user must recover Sun Quan by {expectedRecovery}; observed {recovery.Amount}.");
        var bonuses = Facts<ProgramRecoveryPolicyAppliedEvent>(game).Where(fact => fact.SkillId == Jiuyuan).ToArray();
        Require((expectedRecovery == 2 ? bonuses.SequenceEqual([new(peach.CardUseFrameId, Lord, Payer, 0, 2, Jiuyuan, "allied-rescue")]) : bonuses.Length == 0) &&
            Facts<DyingResolvedEvent>(game).Single(fact => fact.ResolutionId == dying.Id).Survived == (expectedRecovery == 2),
            "Only the Wu payer's Peach carries the classic Jiuyuan bonus and saves the Lord.");
        _ = Cold(game, content);
    }

    private static bool IsChenqingSelection(PendingDecision prompt, int seat) => prompt.PlayerSeat == seat &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "dying-suits");

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            game.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType())).SequenceEqual(
                restored.Events.Select(entry => JsonSerializer.Serialize(entry.Payload, entry.Payload.GetType()))) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold accepted-command replay preserves the original Dying and payment frames, facts and visible state.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start(string faction)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(faction));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, ModeId = Mode, HumanSeat = Lord,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        return (game, content);
    }

    private sealed class Fixture(string faction) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-chenqing-rescue-source", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, SunQuan, CaiWenJi);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"enter-dying","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,
                    "targetKind":"otherLiving","usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":20},{"op":"loseHp","target":"selectedTarget","amount":6}]}]},
                {"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"paid","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch",
                    "movementReasons":["{{{DiscardReason}}}"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}]}]}
                """;
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "准备实牌", description = "摸二十张牌，然后令一名其他角色失去六点体力。" },
                    [Watch] = new { name = "原弃牌完毕", description = "陈情实际付款后暂停。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(Source, "陈情实际用桃者", "supporter", Driver, faction, 6, [Watch]));
            builder.AddGeneral(new(Peer, "其他角色", "supporter", "standard:none", "wei", 6));
            // Each suit has seven copies, while the other three players receive six cards total.
            builder.AddDeck(new(Deck, "固定四花色", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 28)
                .Select(index => new ContentDeckPhysicalCard("standard:slash", (Suit)(index % 4), 7)).ToArray() });
            builder.AddMode(new(Mode, "陈情原救援来源", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
