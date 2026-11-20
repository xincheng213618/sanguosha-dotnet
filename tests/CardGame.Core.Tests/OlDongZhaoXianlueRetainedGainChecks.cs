using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlDongZhaoXianlueRetainedGainChecks
{
    private const string Xianlue = "ol:xianlue", Gujing = "happy:gujing", Chain = "fixture:xianlue-iron-chain";
    private const string Mode = "identity:xianlue-retained-gift", Deck = "fixture:xianlue-retained-gift";
    private const string DrawReason = "program.xianlue.draw", GiftReason = "program.xianlue.gift";
    private static readonly string[] Generals = ["fixture:xianlue-owner", "fixture:xianlue-abolished", "fixture:xianlue-peer-a", "fixture:xianlue-peer-b"];

    internal static void XianlueContinuesAfterFirstGiftStaysInItsOriginalHandCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Xianlue }, "The actual Lord's turn start offers the original Xianlue record.");
        Activate(game);
        Record(game, CardKind.IronChain);
        var play = Next(game);
        var bone = game.CreateSnapshot(0).Players[0].Hand.Single().Id;
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && game.CreateSnapshot(0).DrawPileCount == 4,
            "The original owner has a real weapon and the next four cards remain on the draw pile.");
        Accept(game, new UseProgramSkillCommand(0, Gujing, "bone-path", [], [1], game.Revision, play.PromptId));
        GujingChoice(game, "card:" + bone);
        GujingChoice(game, "slot:Weapon");
        GujingChoice(game, "abolish:" + bone);
        GujingChoice(game, "finish");
        var gujing = Facts<HappyWeiYanGujingIssuedEvent>(game).Single();
        Require(Facts<HappyWeiYanGujingAbolishedEvent>(game).Single() is { Holder: 1, Slot: EquipmentSlot.Weapon } abolished && abolished.FrameId == gujing.FrameId &&
            game.CardMovements.Any(move => move.CardId == bone && move.Sequence == abolished.After && move.From == CardLocation.Equipment(1) && move.To == CardLocation.DiscardPile),
            "Real Gujing pays the placed bone and issues the lasting replacement for seat one's abolished weapon slot.");
        play = Next(game);
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        play = Next(game);
        Require(play is { PlayerSeat: 1, Kind: DecisionKind.PlayCard }, "A different living character reaches an ordinary Play phase.");
        var chain = game.GetLegalActions(1).First(action => action.PlayedCardKind == CardKind.IronChain && action.ConversionSource?.SkillId == Chain && action.TargetSeats.SequenceEqual([3]));
        Accept(game, new PlayCardCommand(1, chain.CardId!.Value, chain.TargetSeats, game.Revision, play.PromptId, CardKind.IronChain) { ConversionSource = chain.ConversionSource });

        var firstChoice = Next(game);
        var reward = Facts<XianlueRewardIssuedEvent>(game).Single();
        var cards = game.CardMovements.Where(move => move.Reason.Value == DrawReason && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(0)).ToArray();
        Require(firstChoice is { PlayerSeat: 0, SkillPrompt.SkillId: Xianlue, IsPrivate: true } && reward.OwnerSeat == 0 &&
            Facts<IronChainResolvedEvent>(game).Single() is { SourceSeat: 1 } native && native.ResolutionId == reward.CardUseId &&
            Facts<CardUseFinishedEvent>(game).Single(fact => fact.ResolutionId == native.ResolutionId).CardKind == CardKind.IronChain &&
            cards.Length == 2 && cards.All(move => move.CardKind == CardKind.Crossbow),
            "The other character's real completed Iron Chain entitles Xianlue to allocate exactly its two actually drawn physical weapons.");
        var first = cards[0].CardId;
        var second = cards[1].CardId;

        var control = Cold(game, content);
        Give(control, first, 2);
        var sent = control.CardMovements.Single(move => move.CardId == first && move.Reason.Value == GiftReason);
        Require(sent.From == CardLocation.Hand(0) && sent.To == CardLocation.Hand(2) && sent.HappyWeiYanGainDestination is null &&
            Facts<NativeCardsGivenEvent>(control).Single(fact => fact.ProducerId == reward.FrameId) is { Giver: 0, Recipient: 2, Count: 1 },
            "With the first recipient's intact slot, the first physical transfer and actual native Give both occur.");
        ContinueSecond(control, reward, second);
        Require(Zone(control, first) == CardLocation.Hand(2), "The ordinary first recipient keeps the real transferred card.");

        game = Cold(game, content);
        Give(game, first, 1);
        _ = Next(game);
        var original = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == reward.FrameId);
        Require(original.HappyWeiYanRetainedGains is [var retained] && retained.ProducerFrameId == original.Id && retained.Plan.CardId == first &&
            retained.Plan.RequestedTo == CardLocation.Hand(1) && retained.Plan.From == CardLocation.Hand(0) && retained.Plan.ActualTo == retained.Plan.From &&
            retained.Plan.IssueFrameId == gujing.FrameId && Zone(game, first) == retained.Plan.From &&
            !game.CardMovements.Any(move => move.CardId == first && move.Sequence > cards[0].Sequence) &&
            !Facts<NativeCardsGivenEvent>(game).Any(fact => fact.ProducerId == reward.FrameId),
            "The paid first request retains the original weapon in the same hand, without inventing a physical movement or Native Give.");
        game = Cold(game, content);
        ContinueSecond(game, reward, second);
        Require(Zone(game, first) == CardLocation.Hand(0) &&
            Facts<NativeCardsGivenEvent>(game).Where(fact => fact.ProducerId == reward.FrameId).Select(fact => fact.Recipient).SequenceEqual([3]),
            "Only the second weapon really reaches a recipient after the first request resolves by retaining the card.");
    }

    private static void ContinueSecond(GameEngine game, XianlueRewardIssuedEvent reward, int card)
    {
        var current = Next(game);
        Require(current is { PlayerSeat: 0, SkillPrompt.SkillId: Xianlue } && current.Choices.All(choice => choice.Cards.SequenceEqual([card])) &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == reward.FrameId).XianluePending is { Stage: XianlueStage.Distribute, CardIndex: 1 },
            "Xianlue resumes its original private allocation at the second card exactly once.");
        Give(game, card, 3);
        Record(game, CardKind.Duel);
        Require(Next(game) is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } && Zone(game, card) == CardLocation.Hand(3) &&
            Facts<XianlueRecordedEvent>(game).Select(fact => fact.Kind).SequenceEqual([CardKind.IronChain, CardKind.Duel]) &&
            Facts<XianlueRewardIssuedEvent>(game).Count() == 1 && !game.ResolutionStack.Any(frame => frame.Id == reward.FrameId),
            "The original reward finishes after the remaining real gift and rerecording, returning to the actual Iron Chain user's Play phase.");
    }

    private static void Record(GameEngine game, CardKind kind)
    {
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Xianlue }, "Xianlue presents a real trick record prompt.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("value") == kind.ToString());
    }

    private static void Give(GameEngine game, int id, int recipient)
    {
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Xianlue }, "Xianlue presents an original drawn card for allocation.");
        Answer(game, choice => choice.Cards.SequenceEqual([id]) && choice.Targets.SequenceEqual([recipient]));
    }

    private static void GujingChoice(GameEngine game, string option)
    {
        Require(Next(game) is { PlayerSeat: 0, SkillPrompt.SkillId: Gujing }, "Gujing presents the original physical bone choice.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option") == option);
    }

    private static GameEngine Start(ContentRegistry content)
    {
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        while (Next(game) is { Kind: DecisionKind.SelectGeneral } selection)
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
        return game;
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold restore preserves the native use, original allocation cursor, and all public and private views.");
        return restored;
    }

    private static CardLocation Zone(GameEngine game, int id) => game.CreateCardZoneDiagnostics().Single(fact => fact.CardId == id).Location;
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-dong-zhao-xianlue-retained-gain", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:dong-zhao", "happy:shen-wei-yan", "mou:huang-quan");
            var rules = $$"""
                {"skills":[{"id":"{{Chain}}","revision":1,"viewAs":[{"id":"chain","inputKinds":[],"inputSuits":[],"sourceZones":["hand"],
                  "outputKind":"ironChain","forPlay":true,"forResponse":false}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Chain] = new { name = "实体铁索", description = "将一张手牌当铁索连环使用或重铸。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Chain, catalog.Presentations[Chain].Name, catalog.Presentations[Chain].Description)
                { Program = catalog.Programs[Chain], ProgramPresentation = catalog.Presentations[Chain] });
            builder.AddGeneral(new(Generals[0], "董昭及骨径", "supporter", Xianlue, "wei", 4, [Gujing]));
            builder.AddGeneral(new(Generals[1], "铁索使用者", "supporter", Chain, "shu", 4));
            foreach (var seat in new[] { 2, 3 })
                builder.AddGeneral(new(Generals[seat], "同桌", "supporter", "standard:none", "shu", 5));
            builder.AddDeck(new(Deck, "八张真实诸葛连弩", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 8).Select(rank => new ContentDeckPhysicalCard("standard:crossbow", Suit.Club, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "OL 董昭先略零移动结算", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
