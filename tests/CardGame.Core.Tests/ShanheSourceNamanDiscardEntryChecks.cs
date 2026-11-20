using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;
using static BoundaryLiDianChecks;

internal static class ShanheSourceNamanDiscardEntryChecks
{
    private const string Skill = StandardShanheSourceRangerPackage.NamanSkillId;
    private const string Recycler = "fixture:source-naman-response-recycle", Deck = "fixture:source-naman-response-deck";
    private const string Rediscard = "skill-program." + Recycler + ".SelectAndMoveOwnedCard";
    private const string Claim = "skill-program." + Skill + ".naman-response-claim";

    internal static void NamanCannotClaimARedroppedNativeResponseCostCold()
    {
        var (game, content) = Start();
        var play = Next(game);
        var opening = game.CreateSnapshot(0);
        Require(IsPlay(play) && opening is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The Ranger's first real draw empties the fixed deck before the native response.");
        var assault = opening.Players[0].Hand.Single(card => card.Kind == CardKind.BarbarianAssault);
        var action = game.GetHumanLegalActions().Single(option => option.CardId == assault.Id && option.ConversionSource is null);
        Accept(game, new PlayCardCommand(0, assault.Id, action.TargetSeats, game.Revision, play.PromptId, action.PlayedCardKind, action.TargetCardId));
        Require(Next(game).SkillPrompt?.SkillId == Recycler, "The earlier real response-completed observer pauses before Naman.");

        var parent = Facts<CardUseDeclaredEvent>(game).Single(fact => fact.CardId == assault.Id);
        var window = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ResponseCompletion is not null);
        var zhangba = Facts<ZhangbaSerpentSpearConvertedEvent>(game).Single();
        var original = window.ResponseCompletion!.NativeCosts.Where(move => move.Reason == CardMoveReasons.ResponseFinished).ToArray();
        Require(window.ParentFrameId == parent.ResolutionId && window.ResponseCompletion is
            { CostsDrained: true, CompletionActorSeat: 1, OriginalContinuation: ProgramCardContinuation.GroupResponse } &&
            window.Action is { ProviderSeat: 1, Type: CardActionType.Response, EffectiveKind: CardKind.Slash } &&
            zhangba is { UserSeat: 1, IsUse: false } && zhangba.ResolutionId == parent.ResolutionId && original.Length == 2 &&
            original.Select(move => move.CardId).SequenceEqual(zhangba.PhysicalCardIds) && original.All(move =>
                move.CardKind == CardKind.Dodge && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile) &&
            window.Candidates.Skip(window.CandidateIndex + 1).Any(candidate => candidate.SkillId == Skill && candidate.OwnerSeat == 0),
            "The official Naman candidate follows the other player's paid native two-card Zhangba response to Barbarian Assault.");

        var control = Cold(game, content);
        AnswerHuman(control, choice => Action(choice) == "skip");
        Require(Next(control).SkillPrompt?.SkillId == Skill, "Naman is offered when both response entries are still current.");
        AnswerHuman(control, choice => Action(choice) == "activate");
        Require(IsPlay(Next(control)) && control.CardMovements.Where(move => move.Reason.Value == Claim).Select(move => move.CardId)
            .SequenceEqual(original.Select(move => move.CardId)) && original.All(entry =>
                control.CreateCardZoneDiagnostics().Single(card => card.CardId == entry.CardId).Location == CardLocation.Hand(0)),
            "Naman obtains both untouched Dodge entities in original response-material order.");
        AssertReturned(control, parent.ResolutionId, window.Action.ActionId);

        AnswerHuman(game, choice => Action(choice) == "activate");
        var payment = Next(game);
        Require(payment.SkillPrompt?.SkillId == Recycler && original.All(entry =>
                payment.Choices.Any(choice => choice.Cards.SequenceEqual([entry.CardId])) &&
                game.CardMovements.Where(move => move.CardId == entry.CardId && move.Sequence > entry.Sequence).ToArray()
                    is [var shuffled, var drawn] && shuffled.Reason == CardMoveReasons.Reshuffle &&
                shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
                drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(0)),
            "The earlier observer really shuffles and draws both of the original paid response materials.");
        var repaid = original[0];
        game = Cold(game, content);
        Require(game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(frame => frame.Id == window.Id &&
            frame.ResponseCompletion is { CostsDrained: true } receipt && receipt.NativeCosts.Contains(repaid)),
            "The cold private discard choice retains the same original native response receipt.");
        AnswerHuman(game, choice => choice.Cards.SequenceEqual([repaid.CardId]));
        var fresh = game.CardMovements.Single(move => move.CardId == repaid.CardId && move.Reason.Value == Rediscard);
        Require(fresh.From == CardLocation.Hand(0) && fresh.To == CardLocation.DiscardPile &&
            fresh.Sequence > repaid.Sequence && fresh.BatchId != repaid.BatchId,
            "The paid private discard makes one separate, newer entry for the same physical Dodge.");
        var next = Next(game);
        var offered = next.SkillPrompt?.SkillId == Skill;
        if (offered)
        {
            AnswerHuman(game, choice => Action(choice) == "activate");
            next = Next(game);
        }
        Require(IsPlay(next) && !offered && game.CardMovements.All(move => move.Reason.Value != Claim) &&
            game.CreateCardZoneDiagnostics().Single(card => card.CardId == fresh.CardId).Location == CardLocation.DiscardPile,
            $"Naman may not offer or obtain discard entry {fresh.Sequence} through the older response entry {repaid.Sequence}.");
        AssertReturned(game, parent.ResolutionId, window.Action.ActionId);
    }

    private static void AssertReturned(GameEngine game, long parent, long response)
    {
        Require(Facts<CardResponseCompletedEvent>(game).Count(fact => fact.ActionId == response) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == parent) == 1 &&
            Facts<CardActionAcceptedEvent>(game).Count(fact => fact.Action.ActionId == response) == 1 &&
            !Facts<DamageAppliedEvent>(game).Any(),
            "The original response and Barbarian Assault both finish once without inventing a second payment or damage.");
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
        throw new InvalidOperationException("Source 231 Naman did not reach the next human decision.");
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
            "Cold replay reproduces the actual response settlement and private observer choice for each seat.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var enemy = new ShanheCombatantDefinition("classic:ma-chao", Hp: 1)
        {
            StartingHand = [new(CardKind.Dodge, Suit.Club, 2), new(CardKind.Dodge, Suit.Spade, 3)],
            StartingEquipment = [new(CardKind.ZhangbaSerpentSpear, Suit.Spade, 12)]
        };
        var setup = new ShanheBattleSetup(71, 1, "source-naman-discard-entry", StandardShanheSourceRangerPackage.HellId,
            4, 0, 0, 0, 0, [Recycler], [new(CardKind.BarbarianAssault, Suit.Spade, 7)], [], [enemy], [], new string('a', 64));
        var content = StandardShanheCampaignCatalog.CreateBattleRegistry(setup with { ExtraSkillIds = [] }).WithExtension(new Fixture(), mode =>
            mode.Id == ShanheBattleSetup.BattleModeId ? mode with { DeckId = Deck } : mode);
        var game = GameEngine.CreateStandard(new GameOptions { Seed = setup.BattleSeed, PlayerCount = 2, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId, ShanheBattle = setup,
            UseInteractiveSetup = false, ConfirmOpeningHand = false, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-source-naman-response-discard-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            var rules = $$$"""
                {"skills":[{"id":"{{{Recycler}}}","revision":1,"triggers":[{"id":"draw-then-discard-native-response",
                    "window":"cardResponseCompleted","ownerRelation":"observer","cardKinds":["slash"],"priority":100,"optional":true,
                    "effects":[{"op":"draw","target":"owner","amount":2},
                      {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                        "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Recycler] = new { name = "回收响应牌", description = "其他角色打出杀结算后摸两张牌，再弃置一张手牌。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Recycler, catalog.Presentations[Recycler].Name, catalog.Presentations[Recycler].Description)
                { Program = catalog.Programs[Recycler], ProgramPresentation = catalog.Presentations[Recycler] });
            builder.AddDeck(new(Deck, "固定两张普通摸牌", 0, 2, []) { PhysicalCards =
                [new("standard:dodge", Suit.Heart, 5), new("standard:dodge", Suit.Diamond, 6)] });
        }
    }
}
