using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ShiHuangChongSiliFirstHealthChecks
{
    private const string Hero = "mobile:shi-huang-chong", Sili = "mobile:shi-sili";
    private const string Driver = "fixture:sili-health";
    private const string Gain = "skill-program.mobile:shi-sili.gain";
    private const string Mode = "identity:classic-sili-first-health", Deck = "fixture:sili-first-health-deck";
    private static readonly string[] Generals = [Hero, "fixture:sili-peer-1", "fixture:sili-peer-2", "fixture:sili-peer-3"];

    internal static void SiliOnlyOffersTheFirstActualHealthChangeEvenAfterRefusalCold()
    {
        var (game, content) = Start();
        var turn = game.State.TurnNumber;
        var hp = game.CreateSnapshot(0).Players[0].Hp;
        MaximumChangeOnlyOffersWhenItClamps(Cold(game, content), content);
        Use(game, "lose");
        Reach(game, IsSiliOffer);
        var first = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single().Change;
        Require(first is { Kind: HpChangeKind.Loss, TargetSeat: 0, Amount: 1 } && first.HpBefore == hp && first.HpAfter == hp - 1 &&
            Facts<ProgramSkillHpLostEvent>(game).Single() is { SkillId: Driver, TargetSeat: 0, Amount: 1 } &&
            game.State.TurnNumber == turn,
            "The very first real health change is the public loss of one HP in the current actual turn.");

        foreach (var (choice, invoked) in new[] { ("gain-slash", "slash"), ("draw-plus", "draw") })
        {
            var control = Cold(game, content);
            Activate(control);
            Reach(control, prompt => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Sili } &&
                prompt.Choices.Any(item => Branch(item) == choice));
            Require(P(control)!.Choices.Select(Branch).Order().SequenceEqual(new[] { "draw-plus", "gain-slash" }),
                "Accepting the first health change offers both real Sili branches.");
            Answer(control, item => Branch(item) == choice);
            Settle(control);
            var accepted = Facts<ShiHcSiliInvokedEvent>(control).Single();
            Require(accepted is { OwnerSeat: 0 } && accepted.Branch == invoked && accepted.TurnNumber == turn &&
                Facts<ShiHcSiliBeishuiArmedEvent>(control).Single() is { OwnerSeat: 0 } obligation &&
                obligation.FrameId == accepted.FrameId && obligation.TurnNumber == turn,
                "Whichever first-change branch was accepted, the original frame arms exactly one Beishui cost.");
            if (choice == "gain-slash")
            {
                var slash = Facts<ShiHcSiliSlashArmedEvent>(control).Single();
                Require(slash.FrameId == accepted.FrameId && control.CardMovements.Single(move => move.Reason.Value == Gain) is
                    { To: var recipient, CardId: var id, CardKind: CardKind.Slash } && recipient == CardLocation.Hand(0) && id == slash.CardId,
                    "The Slash branch really obtains and arms the same physical Slash.");
            }
            else
                Require(Facts<ShiHcSiliDrawGrantedEvent>(control).Single() is { OwnerSeat: 0, GrantedInDrawPhase: false } draw &&
                    draw.FrameId == accepted.FrameId, "The draw branch retains its real next-draw grant.");
        }

        game = Cold(game, content);
        Skip(game);
        Settle(game);
        Use(game, "recover");
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || IsSiliOffer(prompt));
        Require(Facts<RecoveryAppliedEvent>(game).Single() is { SourceSeat: 0, TargetSeat: 0, Amount: 1 } healed &&
            healed.RemainingHp == hp && game.CreateSnapshot(0).Players[0].Hp == hp && game.State.TurnNumber == turn,
            "The second public action really recovers one HP in the same actual turn after the refused loss trigger.");
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } && !Facts<ShiHcSiliInvokedEvent>(game).Any() &&
            !Facts<ShiHcSiliBeishuiArmedEvent>(game).Any(),
            $"Refusing Sili at first health change {first.Id} cannot leave an opportunity on this same turn's later recovery.");
    }

    private static void MaximumChangeOnlyOffersWhenItClamps(GameEngine game, ContentRegistry content)
    {
        var turn = game.State.TurnNumber;
        var starting = game.CreateSnapshot(0).Players[0];
        Require(starting.Hp == starting.MaxHp && starting.Hp > 1, "The maximum-HP control starts at full health.");

        Use(game, "raise-maximum");
        Reach(game, prompt => prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } || IsSiliOffer(prompt));
        var raised = game.CreateSnapshot(0).Players[0];
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard } &&
            Facts<MaximumHpChangedEvent>(game).Single(entry => entry.SkillId == Driver) is { PlayerSeat: 0, Delta: 1 } &&
            raised.Hp == starting.Hp && raised.MaxHp == starting.MaxHp + 1,
            "An actual maximum-HP increase with unchanged HP leaves the first Sili opportunity available.");

        game = Cold(game, content);
        Use(game, "clamp-maximum");
        Reach(game, IsSiliOffer);
        var clamp = game.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Single().Change;
        var clamped = game.CreateSnapshot(0).Players[0];
        Require(Facts<MaximumHpChangedEvent>(game).Last(entry => entry.SkillId == Driver) is { PlayerSeat: 0, Delta: -2 } &&
            clamp is { Kind: HpChangeKind.MaximumHp, TargetSeat: 0 } && clamp.HpBefore == starting.Hp &&
            clamp.HpAfter == starting.Hp - 1 && clamped.Hp == clamp.HpAfter && clamped.MaxHp == clamped.Hp &&
            game.State.TurnNumber == turn, "The following real HP clamp is the first actual health change and offers Sili.");
    }

    private static void Use(GameEngine game, string id) =>
        Accept(game, new UseProgramSkillCommand(0, Driver, id, [], [], game.Revision, P(game)!.PromptId));
    private static bool IsSiliOffer(PendingDecision prompt) => prompt is { PlayerSeat: 0, SkillPrompt.SkillId: Sili } &&
        prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
    private static string? Branch(PromptChoice choice) => choice.Parameters.GetValueOrDefault("branch");
    private static IEnumerable<T> Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))),
            "Cold replay preserves the first health change and its published decision for each viewer.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode,
            HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
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

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-sili-first-health", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(new OwnerBuilder(builder), Hero);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[
                    {"id":"lose","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"loseHp","target":"owner","amount":1}]},
                    {"id":"recover","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"recover","target":"owner","amount":1}]},
                    {"id":"raise-maximum","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"changeMaximumHp","target":"owner","amount":1}]},
                    {"id":"clamp-maximum","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,
                     "effects":[{"op":"changeMaximumHp","target":"owner","amount":-2}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "体力变化", description = "失去或回复一点体力。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            foreach (var peer in Generals.Skip(1))
                builder.AddGeneral(new(peer, "同桌", "supporter", "standard:none", "shu", 8));
            builder.AddDeck(new(Deck, "固定实体杀", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 24)
                .Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "死励首次体力变化", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }

    private sealed class OwnerBuilder(IContentRegistryBuilder inner) : IContentRegistryBuilder
    {
        public void AddGeneral(ContentGeneralDefinition general) => inner.AddGeneral(general.Id == Hero ?
            general with { AdditionalSkillIds = [.. general.AdditionalSkillIds ?? [], Driver] } : general);
        public void AddSkill(ContentSkillDefinition skill) => inner.AddSkill(skill);
        public void AddCard(ContentCardDefinition card) => inner.AddCard(card);
        public void AddDeck(ContentDeckRecipe deck) => inner.AddDeck(deck);
        public void AddMode(ContentModeDefinition mode) => inner.AddMode(mode);
    }
}
