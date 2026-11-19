using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class OlYisuanChecks
{
    private const string LiJue = "ol:li-jue", Yisuan = "ol:yisuan";
    private const string Watch = "fixture:yisuan-maximum-hp-child", Peer = "fixture:yisuan-peer", Deck = "fixture:yisuan-deck";
    private const string Rediscard = "skill-program.fixture:yisuan-maximum-hp-child.SelectAndMoveOwnedCard";
    private const string Gain = "skill-program.yisuan.actual-used-trick";

    public static void PaidMaximumHpCannotObtainTheSameTrickAtANewerDiscardEntryCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(418, 1, "yisuan-original-discard", LiJue, 6, 0, 0, 0, 0,
            [Watch], [new(CardKind.Duel, Suit.Club, 7)], [],
            [new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }, new(Peer) { StartingHand = [] }],
            [], new string('d', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 4,
            HumanSeat = 0, HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Submit(game, new StartGameCommand());
        var play = Next(game);
        var initial = game.CreateSnapshot(0);
        Require(play is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && initial.DrawPileCount == 0 && initial.DiscardPileCount == 0 &&
            initial.Players[0] is { Hp: 6, MaxHp: 6 } && initial.Players[1].HandCount == 0,
            "The initial natural draw exhausts the two-card stock and leaves Li Jue at full health and the Duel target empty-handed.");
        var duel = initial.Players[0].Hand.Single(card => card.Kind == CardKind.Duel).Id;
        Submit(game, new PlayCardCommand(0, duel, [1], game.Revision, play.PromptId, CardKind.Duel));

        var offer = Next(game);
        Require(offer is { PlayerSeat: 0, SkillPrompt.SkillId: Yisuan }, "The actual used Duel qualifies for Yisuan.");
        var original = game.CardMovements.Single(move => move.CardId == duel && move.Reason == CardMoveReasons.UseFinished);
        var invoice = game.ResolutionStack.OfType<CardsMovedTriggerWindowFrame>().Single(frame => frame.Batch.Id == original.BatchId).Batch.LiJueUseDiscard;
        Require(original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
            invoice is { Action.Type: CardActionType.Use, Action.ActorSeat: 0, Action.EffectiveKind: CardKind.Duel } &&
            invoice.FinishedMovements.SequenceEqual([original]) && game.CreateSnapshot(0).DiscardPileCount == 1 &&
            !Facts<YisuanMaximumHpPaidEvent>(game).Any(),
            "The native use retains the original finish entry and no maximum HP has been paid at the offer.");
        Answer(game, offer, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");

        var discard = Next(game);
        var paid = Facts<YisuanMaximumHpPaidEvent>(game).Single();
        var yisuan = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == paid.FrameId);
        var returnedToHand = game.CardMovements.Where(move => move.CardId == duel && move.Sequence > original.Sequence).ToArray();
        Require(discard is { PlayerSeat: 0, SkillPrompt.SkillId: Watch } && discard.Choices.Any(choice => choice.Cards.SequenceEqual([duel])) &&
            paid is { MaxBefore: 6, MaxAfter: 5, HpBefore: 6, HpAfter: 5 } && paid.NativeUseId == invoice!.NativeUseId &&
            yisuan.LiJue is { Stage: LiJueStage.MaximumHpChildren, OriginalUse: { } held } && held.FinishedMovements.SequenceEqual([original]) &&
            returnedToHand is [var reshuffle, var drawn] && reshuffle.From == CardLocation.DiscardPile && reshuffle.To == CardLocation.DrawPile &&
            reshuffle.Reason == CardMoveReasons.Reshuffle && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0) &&
            !Facts<YisuanGainPaidEvent>(game).Any(),
            "The genuine maximum-HP child has drawn the original Duel through a real reshuffle and awaits its public discard decision.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(game.CardMovements.SequenceEqual(restored.CardMovements) && Facts<YisuanMaximumHpPaidEvent>(restored).SequenceEqual([paid]) &&
            Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))),
            "Cold restore reproduces the once-paid cost and the same re-discard prompt for every seat.");
        game = restored;
        Answer(game, Next(game), choice => choice.Cards.SequenceEqual([duel]));
        Require(Next(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The original use and its child must settle back into Li Jue's Play phase.");

        var newer = game.CardMovements.Single(move => move.CardId == duel && move.Reason.Value == Rediscard);
        Require(newer.Sequence > original.Sequence && newer.BatchId != original.BatchId && newer.From == CardLocation.Hand(0) &&
            newer.To == CardLocation.DiscardPile && Facts<CardUseFinishedEvent>(game).Count(e => e.ResolutionId == paid.NativeUseId && e.CardId == duel) == 1,
            "The same physical Duel really enters the discard pile again after its one original native use.");
        Require(Facts<YisuanMaximumHpPaidEvent>(game).SequenceEqual([paid]) &&
            Facts<MaximumHpChangedEvent>(game).Count(e => e.PlayerSeat == 0 && e.SkillId == Yisuan && e.Delta == -1) == 1 &&
            game.CreateSnapshot(0).Players[0] is { Hp: 5, MaxHp: 5 },
            "The accepted original payment remains in force and is charged only once.");
        var settled = Facts<YisuanGainPaidEvent>(game).Single();
        Require(settled.NativeUseId == paid.NativeUseId && settled.NativeActionId == paid.NativeActionId && settled.ActualGain == 0 &&
            !game.CardMovements.Any(move => move.Reason.Value == Gain) &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == duel).Location == CardLocation.DiscardPile,
            $"The paid Yisuan has zero gain when the original entry is gone. Its actual gain was {settled.ActualGain}.");
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100 && game.PendingDecision is null; step++)
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        return game.PendingDecision ?? throw new InvalidOperationException("The Yisuan fixture did not reach a command boundary.");
    }

    private static void Answer(GameEngine game, PendingDecision prompt, Func<PromptChoice, bool> choose) =>
        Submit(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.Single(choose).Id, game.Revision));

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Require(result.Accepted, result.Error?.Message ?? "The Yisuan fixture command was rejected.");
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-yisuan", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, LiJue);
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"cycle-after-maximum-hp","window":"afterHealthChanged",
                    "subject":"owner","condition":{"kind":"healthChangeKindIs","hpChangeKind":"maximumHp"},"optional":false,"isSkillInvocation":false,
                    "effects":[{"op":"draw","target":"owner","amount":1},
                        {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                         "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "上限支付观察", description = "体力上限变化后，摸一张牌并弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Peer, "决斗对手", "", "standard:none", "qun", 8));
            builder.AddDeck(new(Deck, "两张固定闪", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Spade, 4), new("standard:dodge", Suit.Spade, 5)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "亦算原入堆", 4, 4, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [LiJue, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 3 }));
        }
    }
}
