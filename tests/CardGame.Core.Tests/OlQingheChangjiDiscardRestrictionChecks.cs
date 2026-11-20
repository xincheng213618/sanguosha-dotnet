using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class OlQingheChangjiDiscardRestrictionChecks
{
    private const string Changji = "ol:changji", Jilei = "ol:jilei", Driver = "fixture:changji-sourced-damage";
    private const string Mode = "identity:changji-discard-restriction", Deck = "fixture:changji-discard-restriction";
    private const string DiscardReason = "skill-program.ol:changji.changji-discard";
    private static readonly string[] Generals = ["fixture:changji-attacker", "ol:yang-xiu", "ol:qinghe-gongzhu", "fixture:changji-peer"];

    internal static void ChangjiDoesNotDiscardTheTurnOwnersJileiHandCardCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var card = game.CreateSnapshot(0).Players[0].Hand.Single();
        Require(card.Kind == CardKind.Dodge, "The turn owner starts with one physical basic hand card.");
        Damage(game, 1);
        Require(Next(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Jilei } &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceLess: false },
            "The first real damage grants Yang Xiu the actual Jilei choice.");

        var control = Cold(game, content);
        Skip(control);
        control = ReachEndingChoice(control, content, card.Id);
        Require(OfferSameDiscard(control, card.Id), "Qinghe's real discard branch lets the unrestricted turn owner pay the original hand card.");
        FinishTurn(control);
        var discarded = control.CardMovements.Single(move => move.CardId == card.Id && move.Reason.Value == DiscardReason);
        Require(discarded.From == CardLocation.Hand(0) && discarded.To == CardLocation.DiscardPile &&
            Facts<ProgramChangjiEndingEvent>(control).Single() is { OwnerSeat: 2, EndingSeat: 0, Branch: "discard" } result &&
            result.DiscardedCardIds.SequenceEqual([card.Id]) && !Facts<HandCategoryRestrictionGrantedEvent>(control).Any(),
            "The same nominal Changji choice discards the physical card when Yang Xiu declined.");

        Activate(game);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "basic");
        var restriction = Facts<HandCategoryRestrictionGrantedEvent>(game).Single().Restriction;
        Require(restriction is { Category: SkillProgramCardCategory.Basic, AffectedSeat: 0, TurnSeat: 0, Source.SkillId: Jilei },
            "That same physical Dodge is forbidden for the actual turn owner to discard through this turn.");

        game = ReachEndingChoice(game, content, card.Id);
        _ = OfferSameDiscard(game, card.Id);
        FinishTurn(game);
        var moves = game.CardMovements.Where(move => move.CardId == card.Id && move.Reason.Value == DiscardReason).ToArray();
        Require(moves.Length == 0 && game.CreateSnapshot(0).Players[0].Hand.Single().Id == card.Id,
            $"Changji must leave the Jilei-protected Dodge in the actual turn owner's hand. Committed Changji moves: [{string.Join(", ", moves.Select(move => move.Sequence))}].");
    }

    private static GameEngine ReachEndingChoice(GameEngine game, ContentRegistry content, int cardId)
    {
        Damage(game, 2);
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            Facts<DamageAppliedEvent>(game).Last() is { SourceSeat: 0, TargetSeat: 2, Amount: 1, SourceLess: false } &&
            game.CreateSnapshot(0).Players[0].Hand.Single().Id == cardId &&
            game.CreateSnapshot(2).Players[0].Hand.Count == 0 && !Facts<TurnEndedEvent>(game).Any(),
            "The actual Qinghe has taken damage in the same ongoing turn and the turn owner's card remains private.");
        game = Cold(game, content);
        play = Next(game);
        Accept(game, new EndPlayPhaseCommand(0, game.Revision, play.PromptId));
        return game;
    }

    private static bool OfferSameDiscard(GameEngine game, int cardId)
    {
        var choice = Next(game);
        if (choice.SkillPrompt?.SkillId != Changji)
            return false;

        Require(choice is { PlayerSeat: 2, TargetSeat: 0 } &&
            choice.Choices.All(item => item.Cards.Count == 0 && item.Parameters.GetValueOrDefault("branch") != "draw") &&
            !Facts<TurnEndedEvent>(game).Any(),
            "Qinghe has only taken damage, so Changji concerns the actual ending seat and exposes no card choice to Qinghe.");
        if (!choice.Choices.Any(item => item.Parameters.GetValueOrDefault("branch") == "discard"))
        {
            Answer(game, item => item.Parameters.GetValueOrDefault("branch") == "skip");
            return false;
        }

        Answer(game, item => item.Parameters.GetValueOrDefault("branch") == "discard");
        var payment = Next(game);
        if (payment.SkillPrompt?.SkillId != Changji)
            return false;

        Require(payment.PlayerSeat == 0, "Any self-discard selection is answered by the actual ending player.");
        Answer(game, item => item.Cards.SequenceEqual([cardId]));
        return true;
    }

    private static void FinishTurn(GameEngine game)
    {
        var next = Next(game);
        Require(next is { PlayerSeat: 1, Kind: DecisionKind.PlayCard } &&
            Facts<TurnEndedEvent>(game).Single() is { ActorSeat: 0 },
            "The first turn closes normally and advances to the next player's Play phase.");
    }

    private static void Damage(GameEngine game, int target)
    {
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The damage starts from the same real Play phase.");
        Accept(game, new UseProgramSkillCommand(0, Driver, "hit", [], [target], game.Revision, play.PromptId));
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
            "Cold restore preserves the exact movement, frames and the four seat-specific views.");
        return restored;
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-ol-changji-discard-restriction", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:qinghe-gongzhu", "ol:yang-xiu");
            var rules = $$"""
                {"skills":[{"id":"{{Driver}}","revision":1,"activations":[
                  {"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,
                   "effects":[{"op":"damage","target":"selectedTarget","amount":1}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "真实伤害", description = "对一名其他角色造成一点伤害。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Generals[0], "实际伤害来源", "supporter", Driver, "wei", 5));
            builder.AddGeneral(new(Generals[3], "同桌", "supporter", "standard:none", "shu", 5));
            builder.AddDeck(new(Deck, "真实方片闪", InitialHandSize: 1, DrawPerTurn: 0, [])
            {
                PhysicalCards = Enumerable.Range(2, 8).Select(rank => new ContentDeckPhysicalCard("standard:dodge", Suit.Diamond, rank)).ToArray()
            });
            builder.AddMode(new(Mode, "长姬禁弃检查", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
