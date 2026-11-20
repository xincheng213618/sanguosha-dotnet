using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class OlQingheZengouDiscardEntryChecks
{
    private const string Hero = "ol:qinghe-gongzhu", Zengou = "ol:zengou";
    private const string Recycler = "fixture:zengou-paid-hp-child", Deck = "fixture:zengou-discard-entry-deck";
    private const string Enemy = "fixture:zengou-dodger";
    private const string Rediscard = "skill-program." + Recycler + ".SelectAndMoveOwnedCard";
    private const string Gain = "skill-program.wushuang-zengou.gain";

    internal static void ZengouInvalidatesThePaidDodgeWithoutTakingANewerDiscardCold()
    {
        var (game, content) = Start();
        var play = Next(game);
        var opening = game.CreateSnapshot(0);
        Require(IsPlay(play) && opening is { DrawPileCount: 0, DiscardPileCount: 0 },
            "Qinghe's first real draw leaves only the actual response entity available for recycling.");
        var slash = opening.Players[0].Hand.Single(card => card.Kind == CardKind.Slash);
        var use = game.GetHumanLegalActions().Single(action => action.CardId == slash.Id && action.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash.Id, use.TargetSeats, game.Revision, play.PromptId, use.PlayedCardKind));
        Require(Next(game).SkillPrompt?.SkillId == Zengou, "An in-range opponent's real Dodge offers the OL Zengou.");
        var response = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ResponseCompletion is not null);
        var original = response.ResponseCompletion!.NativeCosts.Single(move => move.Reason == CardMoveReasons.ResponseFinished);
        Require(response.ResponseCompletion is { CostsDrained: true, CompletionActorSeat: 1, OriginalContinuation: ProgramCardContinuation.Dodge } &&
            response.Action is { Type: CardActionType.Response, ActorSeat: 1, EffectiveKind: CardKind.Dodge } &&
            response.Action.PhysicalCards.Single().CardId == original.CardId && original is { CardKind: CardKind.Dodge } &&
            original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile,
            "The actual Slash owner retains the opponent's original completed physical Dodge entry.");

        AnswerHuman(game, choice => Action(choice) == "activate");
        var payment = Next(game);
        var zengouFrame = game.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zengou);
        Require(payment.SkillPrompt?.SkillId == Zengou && zengouFrame.WushuangZengou is { Stage: "choosing" } choosing &&
            choosing.ResponseWindowId == response.Id && choosing.ActionId == response.Action.ActionId &&
            choosing.Cards.SequenceEqual([original.CardId]), "Zengou's real private payment is bound to that Dodge.");
        AnswerHuman(game, choice => Action(choice) == "wushuang-zengou" && choice.Parameters.GetValueOrDefault("cost") == "hp");
        Require(Next(game).SkillPrompt?.SkillId == Recycler, "The real paid HP-loss child interrupts before the Dodge is invalidated or gained.");
        var health = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single();
        var paid = Facts<WushuangZengouPaidEvent>(game).Single();
        Require(health.ResumeFrameId == zengouFrame.Id && health.Change is { TargetSeat: 0, Kind: HpChangeKind.Loss, Amount: 1, HpBefore: 3, HpAfter: 2 } &&
            paid.ProgramFrameId == zengouFrame.Id && paid.ActionId == response.Action.ActionId && paid.CostCardId is null &&
            !Facts<WushuangZengouInvalidatedEvent>(game).Any() && !Facts<WushuangZengouGainedCardEvent>(game).Any(),
            "The one-point HP payment and original response are retained across the real health child.");

        var control = Cold(game, content);
        AnswerHuman(control, choice => Action(choice) == "skip");
        AssertPaidAttackReturns(control, paid);
        var gain = Facts<WushuangZengouGainedCardEvent>(control).Single();
        var gainMove = control.CardMovements.Single(move => move.Reason.Value == Gain);
        Require(gain.ActionId == paid.ActionId && gain.OwnerSeat == 0 && gain.CardId == original.CardId && gainMove.Sequence == gain.MovementSequence &&
            gainMove.CardId == original.CardId && gainMove.From == CardLocation.DiscardPile && gainMove.To == CardLocation.Hand(0),
            "The untouched original Dodge is obtained once on the same real paid Zengou.");

        AnswerHuman(game, choice => Action(choice) == "activate");
        var discard = Next(game);
        var returned = game.CardMovements.Where(move => move.CardId == original.CardId && move.Sequence > original.Sequence).ToArray();
        Require(discard.SkillPrompt?.SkillId == Recycler && discard.Choices.Any(choice => choice.Cards.SequenceEqual([original.CardId])) &&
            returned is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The paid health child truly shuffles and draws the original Dodge before the attack returns.");
        game = Cold(game, content);
        AnswerHuman(game, choice => choice.Cards.SequenceEqual([original.CardId]));
        var fresh = game.CardMovements.Single(move => move.CardId == original.CardId && move.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(0) && fresh.To == CardLocation.DiscardPile && fresh.BatchId != original.BatchId &&
            fresh.Sequence > returned[^1].Sequence, "A new paid hand discard restores the entity to the discard pile at a different entry.");
        AssertPaidAttackReturns(game, paid);
        Require(!Facts<WushuangZengouGainedCardEvent>(game).Any() && !game.CardMovements.Any(move => move.Reason.Value == Gain) &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == original.CardId).Location == CardLocation.DiscardPile,
            $"Paid Zengou invalidates the original Dodge but cannot obtain discard entry {fresh.Sequence} through response entry {original.Sequence}.");
    }

    private static void AssertPaidAttackReturns(GameEngine game, WushuangZengouPaidEvent payment)
    {
        Require(IsPlay(Next(game)) && Facts<WushuangZengouPaidEvent>(game).SequenceEqual([payment]) &&
            Facts<ProgramSkillHpLostEvent>(game).Count(fact => fact.FrameId == payment.ProgramFrameId && fact.SkillId == Zengou &&
                fact.TargetSeat == 0 && fact.Amount == 1 && fact.RemainingHp == 2) == 1 &&
            Facts<WushuangZengouInvalidatedEvent>(game).Single().Payment == payment &&
            Facts<CardResponseCompletedEvent>(game).Count(fact => fact.ActionId == payment.ActionId) == 1 &&
            Facts<DamageAppliedEvent>(game).Single(fact => fact.SourceCardUseResolutionId == payment.AttackFrameId) is
                { SourceSeat: 0, TargetSeat: 1, Amount: 1 } &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == payment.AttackFrameId) == 1,
            "The exact paid Dodge is invalidated and the same native Slash applies one damage and finishes once.");
    }

    private static bool IsPlay(PendingDecision prompt) => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard };

    private static string? Action(PromptChoice choice) => choice.Parameters.GetValueOrDefault("program-action");

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static PendingDecision Next(GameEngine game)
    {
        for (var step = 0; step < 100; step++)
        {
            if (game.CreateSnapshot(0).PendingDecision is { } prompt)
                return prompt;
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The OL Zengou fixture missed the next human decision.");
    }

    private static void AnswerHuman(GameEngine game, Func<PromptChoice, bool> predicate)
    {
        var prompt = game.CreateSnapshot(0).PendingDecision!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold public-command replay retains the paid health child and each seat's view.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(23, 1, "ol-zengou-discard-entry", Hero, 3, 0, 0, 0, 0,
            [Recycler], [new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(Enemy, Hp: 2) { StartingHand = [new(CardKind.Dodge, Suit.Heart, 2)] }], [], new string('b', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-zengou-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Recycler}}}","revision":1,"triggers":[{"id":"draw-then-discard-on-hp-loss",
                    "window":"afterHpLost","subject":"owner","optional":true,
                    "effects":[{"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Recycler] = new { name = "付体力后摸弃", description = "失去体力后，可以摸一张牌，然后弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Recycler, catalog.Presentations[Recycler].Name, catalog.Presentations[Recycler].Description)
                { Program = catalog.Programs[Recycler], ProgramPresentation = catalog.Presentations[Recycler] });
            builder.AddGeneral(new(Enemy, "闪响应方", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定两张普通摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Club, 5), new("standard:dodge", Suit.Diamond, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "OL 谮构原入堆", 2, 2, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Enemy], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 1 }));
        }
    }
}
