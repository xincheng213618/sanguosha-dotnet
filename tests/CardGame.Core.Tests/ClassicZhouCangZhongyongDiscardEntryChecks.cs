using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ClassicZhouCangZhongyongDiscardEntryChecks
{
    private const string Hero = "classic:zhou-cang", Skill = "classic:zhongyong";
    private const string Opponent = "fixture:classic-zhongyong-dodger", Recipient = "fixture:classic-zhongyong-recipient";
    private const string Watch = "fixture:classic-zhongyong-reentry", Deck = "fixture:classic-zhongyong-discard-entry-deck";
    private const string Rediscard = "skill-program." + Watch + ".SelectAndMoveOwnedCard";
    private const string Gift = "skill-program." + Skill + ".completed-card-gift";

    internal static void ZhongyongCannotGiftANewerEntryOfTheOriginalDodgeCold()
    {
        GiveDodgeWithoutCompletionObserver();
        var (game, content) = Start();
        var slash = PlaySlash(game);
        Require(Next(game).SkillPrompt?.SkillId == Watch, "The earlier observer pauses on the real completed Dodge before Zhongyong.");
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash);
        var response = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ResponseCompletion is not null);
        var original = response.ResponseCompletion!.NativeCosts.Single(move => move.Reason == CardMoveReasons.ResponseFinished);
        Require(response.ParentFrameId == use.Id && response.ResponseCompletion is
            { CostsDrained: true, CompletionActorSeat: 1, OriginalContinuation: ProgramCardContinuation.Dodge } &&
            response.Action is { Type: CardActionType.Response, ActorSeat: 1, ResponderSeat: 1, EffectiveKind: CardKind.Dodge } &&
            response.Action.ParentActionId == use.Action!.ActionId && response.Action.PhysicalCards.Single().CardId == original.CardId &&
            original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
            Facts<CardUseFinishedEvent>(game).All(fact => fact.ResolutionId != use.Id),
            "The other player actually pays this Dodge in the original Slash response before that Slash has finished.");

        var control = Cold(game, content);
        AnswerHuman(control, choice => Action(choice) == "skip");
        var untouched = OriginalGift(control, use);
        Require(untouched.Choices.Any(choice => Gives(choice, slash)) && untouched.Choices.Any(choice => Gives(choice, original.CardId)),
            "The untouched original Slash and the target's original Dodge are both offered as separate gifts.");
        AnswerHuman(control, choice => Gives(choice, original.CardId));
        Require(IsPlay(Next(control)), "Giving the original black Dodge completes the classic gift.");
        AssertGift(control, use.Action!.ActionId, original.CardId);

        AnswerHuman(game, choice => Action(choice) == "activate");
        var discard = Next(game);
        var returned = game.CardMovements.Where(move => move.CardId == original.CardId && move.Sequence > original.Sequence).ToArray();
        Require(discard.SkillPrompt?.SkillId == Watch && discard.Choices.Any(choice => choice.Cards.SequenceEqual([original.CardId])) &&
            returned is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
            shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0),
            "The completed-response child truly shuffles and draws the same paid Dodge while the original Slash is still active.");
        game = Cold(game, content);
        AnswerHuman(game, choice => choice.Cards.SequenceEqual([original.CardId]));
        var fresh = game.CardMovements.Single(move => move.CardId == original.CardId && move.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(0) && fresh.To == CardLocation.DiscardPile && fresh.BatchId != original.BatchId &&
            fresh.Sequence > returned[^1].Sequence, "The later hand discard gives the same physical Dodge a different entry.");
        var pending = OriginalGift(game, use);
        var slashChoice = pending.Choices.Single(choice => Gives(choice, slash));
        var stale = pending.Choices.SingleOrDefault(choice => Gives(choice, original.CardId));
        game = Cold(game, content);
        AnswerHuman(game, choice => choice.Id == (stale ?? slashChoice).Id);
        Require(IsPlay(Next(game)), "The actual gift returns through the same finished Slash to Play.");
        Require(stale is null && game.CardMovements.All(move => move.Reason.Value != Gift || move.CardId != original.CardId) &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == original.CardId).Location == CardLocation.DiscardPile,
            $"Classic Zhongyong may not offer or gift entry {fresh.Sequence} through the original Dodge response entry {original.Sequence}.");
        AssertGift(game, use.Action.ActionId, slash);
    }

    private static void GiveDodgeWithoutCompletionObserver()
    {
        var (game, content) = Start(withObserver: false);
        var slash = PlaySlash(game);
        var pending = Next(game);
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == slash);
        var dodge = game.CardMovements.Single(move => move.Reason == CardMoveReasons.ResponseFinished);
        Require(OriginalGift(game, use).PromptId == pending.PromptId && dodge.To == CardLocation.DiscardPile &&
            pending.Choices.Any(choice => Gives(choice, dodge.CardId)) && !Facts<CardResponseCompletionStartedEvent>(game).Any(),
            "An ordinary Dodge is still available to give without opening any response-completion observer window.");
        var trustedFrame = JsonSerializer.SerializeToElement<ResolutionFrame>(use);
        Require(trustedFrame.TryGetProperty("NativeDodgeResponses", out var receipts) &&
            receipts.EnumerateArray().Single().GetProperty("Entries").EnumerateArray().Single().GetProperty("Sequence").GetInt32() == dodge.Sequence,
            "The trusted native use frame serializes the exact original Dodge entry.");
        Require(Enumerable.Range(0, game.PlayerCount).All(seat =>
            !SnapshotJson.Serialize(game.CreateSnapshot(seat)).Contains("NativeDodgeResponses", StringComparison.Ordinal)),
            "Player views expose their normal choices and omit the trusted Dodge receipt.");
        game = Cold(game, content);
        AnswerHuman(game, choice => Gives(choice, dodge.CardId));
        Require(IsPlay(Next(game)), "The ordinary Dodge gift returns to the original Play phase.");
        AssertGift(game, use.Action!.ActionId, dodge.CardId);
    }

    private static int PlaySlash(GameEngine game)
    {
        var play = Next(game);
        var opening = game.CreateSnapshot(0);
        Require(IsPlay(play) && opening is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The real first draw leaves no original cards in either pile before the Slash.");
        var slash = opening.Players[0].Hand.Single(card => card.Kind == CardKind.Slash).Id;
        var legal = game.GetHumanLegalActions().Single(action => action.CardId == slash && action.TargetSeats.SequenceEqual([1]));
        Accept(game, new PlayCardCommand(0, slash, legal.TargetSeats, game.Revision, play.PromptId, legal.PlayedCardKind));
        return slash;
    }

    private static PendingDecision OriginalGift(GameEngine game, CardUseFrame use)
    {
        var pending = Next(game);
        Require(pending is { PlayerSeat: 0, IsPrivate: true, SkillPrompt.SkillId: Skill } &&
            game.ResolutionStack.OfType<ProgramSkillFrame>().Any(frame => frame.SkillId == Skill &&
                frame.WindowContext?.CardUse?.ParentCardUseFrameId == use.Id && frame.CompletedCardGiftDraft?.CardActionId == use.Action!.ActionId) &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.Id) == 1 &&
            !Facts<DamageAppliedEvent>(game).Any(fact => fact.SourceCardUseResolutionId == use.Id),
            "The original Slash finishes once, is still genuinely dodged, and opens the owner's private classic gift.");
        return pending;
    }

    private static void AssertGift(GameEngine game, long actionId, int cardId)
    {
        var gifted = Facts<CompletedCardGiftedEvent>(game).Single();
        var moved = game.CardMovements.Single(move => move.Reason.Value == Gift);
        Require(gifted is { OwnerSeat: 0, RecipientSeat: 2, IsRed: false } && gifted.CardActionId == actionId &&
            gifted.CardIds.SequenceEqual([cardId]) && moved.CardId == cardId && moved.To == CardLocation.Hand(2),
            "The accepted classic gift moves exactly one eligible original entity to the other chosen player.");
    }

    private static bool Gives(PromptChoice choice, int cardId) => choice.Parameters.GetValueOrDefault("gift-option") == "give" &&
        choice.Cards.SequenceEqual([cardId]) && choice.Targets.SequenceEqual([2]);

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
        throw new InvalidOperationException("Classic Zhongyong did not reach its next public decision.");
    }

    private static void AnswerHuman(GameEngine game, Func<PromptChoice, bool> predicate)
    {
        var prompt = game.CreateSnapshot(0).PendingDecision!;
        Accept(game, new AnswerPromptCommand(0, prompt.PromptId, prompt.Choices.Single(predicate).Id, game.Revision));
    }

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold public-command replay preserves all seat views, the private gift choices, and the exact movements.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start(bool withObserver = true)
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture(withObserver));
        var setup = new ShanheBattleSetup(43, 1, "classic-zhongyong-discard-entry", Hero, 4, 0, 0, 0, 0,
            withObserver ? [Watch] : [], [new(CardKind.Slash, Suit.Spade, 7)], [],
            [new(Opponent, Hp: 2) { StartingHand = [new(CardKind.Dodge, Suit.Club, 2)] }, new(Recipient) { StartingHand = [] }],
            [], new string('a', 64));
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 3, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture(bool withObserver) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-zhongyong-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            if (withObserver)
                RegisterObserver(builder);
            builder.AddGeneral(new(Opponent, "实际打出闪的目标", "supporter", "standard:none", "qun", 4));
            builder.AddGeneral(new(Recipient, "第三位受领者", "supporter", "standard:none", "qun", 4));
            builder.AddDeck(new(Deck, "固定两张普通摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Heart, 5), new("standard:dodge", Suit.Diamond, 6)] });
            builder.AddMode(new(ShanheBattleSetup.BattleModeId, "经典忠勇原响应入堆", 3, 3, new Dictionary<string, int>(), Deck,
                GeneralPoolIds: [Hero, Opponent, Recipient], ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int>
                { [ShanheBattleSetup.PlayerTeamId] = 1, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }

        private static void RegisterObserver(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"skills":[{"id":"{{{Watch}}}","revision":1,"triggers":[{"id":"draw-then-discard-completed-dodge",
                    "window":"cardResponseCompleted","ownerRelation":"observer","cardKinds":["dodge"],"optional":true,
                    "effects":[{"op":"draw","target":"owner","amount":1},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "响应后摸弃", description = "其他角色响应闪结算后，可以摸一张牌，然后弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Watch, catalog.Presentations[Watch].Name, catalog.Presentations[Watch].Description)
                { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
        }
    }
}
