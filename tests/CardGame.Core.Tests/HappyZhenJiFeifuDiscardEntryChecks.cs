using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class HappyZhenJiFeifuDiscardEntryChecks
{
    private const string Hero = "happy:shen-zhen-ji", Feifu = "happy:feifu", Earlier = "fixture:feifu-earlier-response";
    private const string Mode = "identity:feifu-response-entry", Deck = "fixture:feifu-response-deck";
    private const string Store = "skill.happy-feifu.store";
    private static readonly string[] Generals = ["fixture:feifu-attacker", Hero, "fixture:feifu-watcher", "fixture:feifu-peer"];

    internal static void FeifuStoresOnlyTheOriginalPaidDodgeEntry()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false
        }, content);
        Accept(game, new StartGameCommand());
        var setup = Next(game);
        while (setup.Kind is DecisionKind.SelectGeneral or DecisionKind.SelectFaction)
        {
            if (setup.Kind == DecisionKind.SelectGeneral)
                Accept(game, new SelectGeneralCommand(setup.PlayerSeat, Generals[setup.PlayerSeat], game.Revision, setup.PromptId));
            else
                Answer(game, _ => true);
            setup = Next(game);
        }

        Require(setup is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.CreateSnapshot(0) is { DrawPileCount: 0, DiscardPileCount: 0 },
            "The actual Play phase starts after all four physical cards were dealt.");
        var slash = game.CreateSnapshot(0).Players[0].Hand.Single();
        var cost = game.CreateSnapshot(1).Players[1].Hand.Single();
        var hp = game.CreateSnapshot(1).Players[1].Hp;
        Accept(game, new PlayCardCommand(0, slash.Id, [1], game.Revision, setup.PromptId));
        Require(Next(game) is { Kind: DecisionKind.RespondDodge, PlayerSeat: 1 },
            "The physical Slash asks the real Happy Zhen Ji to respond.");
        Answer(game, choice => choice.Cards.SequenceEqual([cost.Id]) &&
            choice.Parameters.GetValueOrDefault("conversion-skill-id") == Feifu);
        var earlier = Next(game);
        var original = game.CardMovements.Single(move => move.CardId == cost.Id && move.Reason == CardMoveReasons.ResponseFinished);
        var window = game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(frame => frame.ResponseCompletion is not null);
        Require(earlier is { PlayerSeat: 2, SkillPrompt.SkillId: Earlier } &&
            window.ResponseCompletion is { CostsDrained: true, OriginalContinuation: ProgramCardContinuation.Dodge } receipt &&
            window.Candidates.Any(candidate => candidate.SkillId == Feifu) &&
            receipt.NativeCosts.Contains(original) && window.Action is { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge, ProviderSeat: 1 } &&
            window.Action.ConversionChain.Single().SkillId == Feifu &&
            window.Action.PhysicalCards is [{ CardId: var physical, From: var source }] && physical == cost.Id && source == CardLocation.Hand(1) &&
            original.From == CardLocation.Processing && original.To == CardLocation.DiscardPile &&
            Facts<CardActionAcceptedEvent>(game).Count(e => e.Action.ActionId == window.Action.ActionId) == 1,
            "The original response receipt records one genuine Feifu cost and schedules its storage after the earlier response candidate.");

        var control = Cold(game, content);
        Skip(control);
        FinishSlash(control, slash.Id, hp, window);
        var stored = Facts<HappyZjFeifuClaimedEvent>(control).Single();
        Require(stored.ActionId == window.Action.ActionId && stored.CardId == cost.Id &&
            stored.From == CardLocation.DiscardPile && stored.To is { Zone: CardZoneKind.PublicPersistentPile, OwnerSeat: 1 } &&
            control.CardMovements.Single(move => move.Reason.Value == Store).Sequence == stored.Sequence && Zone(control, cost.Id) == stored.To,
            "Feifu stores the original paid physical card on Zhen Ji's character card when it stays in the discard pile.");

        Activate(game);
        var rediscard = Next(game);
        var pickup = game.CardMovements.Where(move => move.CardId == cost.Id && move.Sequence > original.Sequence).ToArray();
        Require(rediscard is { PlayerSeat: 2, SkillPrompt.SkillId: Earlier } && rediscard.Choices.Any(choice => choice.Cards.SequenceEqual([cost.Id])) &&
            game.ResolutionStack.OfType<ProgramCardTriggerWindowFrame>().Any(frame => frame.Id == window.Id) &&
            pickup is [var shuffled, var drawn] && shuffled.From == CardLocation.DiscardPile && shuffled.To == CardLocation.DrawPile &&
            shuffled.Reason == CardMoveReasons.Reshuffle && drawn.From == CardLocation.DrawPile && drawn.To == CardLocation.Hand(2),
            "The actual previous candidate reshuffles and draws the paid entity while the original response window is still open.");
        game = Cold(game, content);
        Answer(game, choice => choice.Cards.SequenceEqual([cost.Id]));
        FinishSlash(game, slash.Id, hp, window);
        var fresh = game.CardMovements.Last(move => move.CardId == cost.Id && move.To == CardLocation.DiscardPile);
        Require(fresh.Sequence > pickup[^1].Sequence && fresh.BatchId != original.BatchId &&
            game.CardMovements.Any(move => move.CardId == cost.Id && move.Sequence >= pickup[^1].Sequence && move.Sequence <= fresh.Sequence &&
                move.From == CardLocation.Hand(2) && move.Reason.Value == $"skill-program.{Earlier}.SelectAndMoveOwnedCard"),
            "The same card is physically discarded from a new hand and enters the discard pile in a new batch.");
        Require(!Facts<HappyZjFeifuClaimedEvent>(game).Any() && !game.CardMovements.Any(move => move.Reason.Value == Store) &&
            Zone(game, cost.Id) == CardLocation.DiscardPile,
            "The original Feifu response cannot store the same physical card's later discard entry.");
    }

    private static void FinishSlash(GameEngine game, int slash, int hp, ProgramCardTriggerWindowFrame window)
    {
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && game.CreateSnapshot(1).Players[1].Hp == hp &&
            Facts<CardResponseCompletedEvent>(game).Count(fact => fact.ActionId == window.Action.ActionId && fact.ParentFrameId == window.ParentFrameId) == 1 &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.CardId == slash && fact.ResolutionId == window.ParentFrameId) == 1,
            "The native Dodge remains effective and the same actual Slash finishes exactly once.");
    }

    private static CardLocation Zone(GameEngine game, int id) =>
        game.CreateCardZoneDiagnostics().Single(fact => fact.CardId == id).Location;

    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) &&
            JsonSerializer.Serialize(restored.ResolutionStack) == JsonSerializer.Serialize(game.ResolutionStack) &&
            Enumerable.Range(0, game.PlayerCount).All(seat => SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold public replay preserves the accepted response, the earlier candidate and all viewer projections.");
        return restored;
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-feifu-response-entry", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, Hero);
            var rules = $$"""
                {"skills":[{"id":"{{Earlier}}","revision":1,"triggers":[{"id":"from-native-response-finish",
                  "window":"cardResponseCompleted","ownerRelation":"observer","cardKinds":["dodge"],"priority":1000,
                  "optional":true,"effects":[{"op":"draw","target":"owner","amount":1},
                   {"op":"selectAndMoveOwnedCard","target":"owner","chooserRef":{"kind":"owner"},"cardOwnerRef":{"kind":"owner"},
                    "zones":["hand"],"count":1,"destination":"discardPile","awaitMovementTriggers":true}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [Earlier] = new { name = "响应牌移动", description = "响应牌结清时，可摸一张牌并弃置一张手牌。" }
                }
            });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            var shown = catalog.Presentations[Earlier];
            builder.AddSkill(new(Earlier, shown.Name, shown.Description)
                { Program = catalog.Programs[Earlier], ProgramPresentation = shown });
            builder.AddGeneral(new(Generals[0], "出杀者", "supporter", "standard:none", "wei", 4));
            builder.AddGeneral(new(Generals[2], "响应窗口的观察者", "supporter", Earlier, "shu", 4));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "wu", 4));
            builder.AddDeck(new(Deck, "四张黑色实体杀", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 4).Select(rank => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "飞凫原响应入堆", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
