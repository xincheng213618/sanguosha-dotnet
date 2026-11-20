using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class AnniversaryHeYanYachaiDiscardChoicesChecks
{
    private const string Yachai = "anniversary:yachai", Jilei = "ol:jilei", Driver = "fixture:yachai-sourced-damage";
    private const string Mode = "identity:yachai-discard-choices", Deck = "fixture:yachai-discard-choices";
    private const string DiscardReason = "skill.anniversary-he-yan.yachai-discard", DrawReason = "skill.anniversary-he-yan.draw";
    private static readonly string[] Generals = ["fixture:yachai-attacker", "ol:yang-xiu", "anniversary:he-yan", "fixture:yachai-peer"];

    internal static void YachaiOmitsDiscardWhenJileiBlocksTheOnlyHandCardCold()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = Start(content);
        var card = game.CreateSnapshot(0).Players[0].Hand.Single();
        Require(card.Kind == CardKind.Dodge, "The original damage source holds one real basic card throughout both damage attempts.");
        Damage(game, 1);
        Require(Next(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Jilei } &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 1, Amount: 1, SourceLess: false },
            "A first native damage attempt offers the real Yang Xiu's category restriction to its actual trigger owner.");

        var control = Cold(game, content);
        Skip(control);
        var normal = BeginYachai(control);
        Require(Branches(normal).SequenceEqual(["ban", "discard", "reveal"]), "All three Yachai choices are present when the same hand card can pay.");
        Branch(control, "discard");
        Require(Next(control) is { PlayerSeat: 0, SkillPrompt.SkillId: Yachai } cost &&
            cost.Choices.Any(choice => choice.Cards.SequenceEqual([card.Id])), "The original physical basic card is a legal discard cost.");
        Answer(control, choice => choice.Cards.SequenceEqual([card.Id]));
        var discarded = control.CardMovements.Single(move => move.CardId == card.Id && move.Reason.Value == DiscardReason);
        Require(Next(control) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } && discarded.From == CardLocation.Hand(0) &&
            discarded.To == CardLocation.DiscardPile && Facts<NativeLogicalDiscardMaterialCompletedEvent>(control)
                .Any(fact => fact.CardId == card.Id && fact.CompletionSequence == discarded.Sequence),
            "The unrestricted Yachai branch genuinely discards the single original card and resumes Play.");

        Activate(game);
        Require(Next(game) is { PlayerSeat: 1, SkillPrompt.SkillId: Jilei }, "The real Jilei selection belongs to Yang Xiu.");
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "basic");
        var restriction = Facts<HandCategoryRestrictionGrantedEvent>(game).Single().Restriction;
        Require(restriction is { Category: SkillProgramCardCategory.Basic, AffectedSeat: 0, TurnSeat: 0, Source.SkillId: Jilei },
            "The actual turn-scoped Jilei restriction forbids the damage source from discarding basic hand cards.");
        _ = BeginYachai(game);
        game = Cold(game, content);
        var blocked = Next(game);
        var branches = Branches(blocked);
        Require(game.CreateSnapshot(0).Players[0].Hand.Single().Id == card.Id &&
            blocked is { PlayerSeat: 0, SkillPrompt.SkillId: Yachai } && branches.Contains("ban") && branches.Contains("reveal"),
            "The exact same hand card remains owned and the other two original branches are still possible.");
        Require(!branches.Contains("discard"), "Yachai cannot publish an entirely unpayable discard branch while both other effects are legal.");

        Branch(game, "ban");
        Require(Next(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 } &&
            game.CreateSnapshot(0).Players[0].Hand.Single().Id == card.Id && !game.CardMovements.Any(move => move.Reason.Value == DiscardReason) &&
            Facts<AnniversaryHeYanHandUseBannedEvent>(game).Single() is { AffectedSeat: 0, TurnOwner: 0 } &&
            Facts<AnniversaryHeYanDrawReturnedEvent>(game).Single() is { TargetSeat: 2, Requested: 2, Actual: 2 } &&
            game.CardMovements.Count(move => move.Reason.Value == DrawReason && move.To == CardLocation.Hand(2)) == 2,
            "The legal alternative really restricts the source and draws two physical cards for He Yan.");
    }

    private static PendingDecision BeginYachai(GameEngine game)
    {
        Damage(game, 2);
        Require(Next(game) is { PlayerSeat: 2, SkillPrompt.SkillId: Yachai } &&
            Facts<DamageAppliedEvent>(game).Last() is { SourceSeat: 0, TargetSeat: 2, Amount: 1, SourceLess: false },
            "The second real sourced damage opens He Yan's original trigger in the same Play phase.");
        Activate(game);
        var options = Next(game);
        Require(options is { PlayerSeat: 0, SkillPrompt.SkillId: Yachai }, "The damaged He Yan makes the actual damage source choose.");
        return options;
    }

    private static string[] Branches(PendingDecision prompt) =>
        prompt.Choices.Select(choice => choice.Parameters["branch"]).ToArray();

    private static void Branch(GameEngine game, string branch) =>
        Answer(game, choice => choice.Parameters.GetValueOrDefault("branch") == branch);

    private static void Damage(GameEngine game, int target)
    {
        var play = Next(game);
        Require(play is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 }, "The same actor's actual Play phase owns both damage attempts.");
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
            "Cold restore preserves the actual damage frames, issued restriction, and each seat's prompt.");
        return restored;
    }

    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();
    private static PendingDecision Next(GameEngine game) { Reach(game, _ => true); return P(game)!; }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-anniversary-he-yan-yachai-discard-choices", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "anniversary:he-yan", "ol:yang-xiu");
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
            builder.AddMode(new(Mode, "何晏崖柴可弃牌选项", 4, 4, new Dictionary<string, int>
            {
                [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2
            }, Deck, GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
