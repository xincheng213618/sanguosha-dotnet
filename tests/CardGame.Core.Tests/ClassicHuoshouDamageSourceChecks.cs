using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using static BoundaryLiDianChecks;

internal static class ClassicHuoshouDamageSourceChecks
{
    private const string MengHuo = "classic:meng-huo", XiahouDun = "classic:xiahou-dun", Ganglie = "classic:ganglie";
    private const string Driver = "fixture:classic-huoshou-driver", Source = "fixture:classic-huoshou-user", Peer = "fixture:classic-huoshou-peer";
    private const string Mode = "identity:classic-huoshou-source", Deck = "fixture:classic-huoshou-deck";
    private static readonly string[] Generals = [Source, XiahouDun, MengHuo, Peer];

    internal static void HuoshouDeathLeavesLaterBarbarianDamageWithoutSourceCold()
    {
        var (game, content) = Start();
        Accept(game, new UseProgramSkillCommand(0, Driver, "wound-meng-huo", [], [2], game.Revision, P(game)!.PromptId));
        Settle(game);
        Require(game.CreateSnapshot(2).Players[2].Hp == 1, "The original Huoshou holder enters the actual Barbarian at one HP.");
        var barbarian = game.GetLegalActions(0).First(action => action.Kind == LegalActionKind.BarbarianAssault);
        var cardId = barbarian.CardId!.Value;
        Accept(game, new PlayCardCommand(0, cardId, barbarian.TargetSeats, game.Revision, P(game)!.PromptId, barbarian.PlayedCardKind));
        Reach(game, prompt => prompt is { PlayerSeat: 1, SkillPrompt.SkillId: Ganglie });
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(frame => frame.CardId == cardId);
        Require(use is { SourceSeat: 0, Action.ActorSeat: 0, Continuations.GroupCard.DamageSourceSeat: 2 } &&
            Facts<HuoshouAttributedEvent>(game).Single() == new HuoshouAttributedEvent(use.Id, 0, 2) &&
            Facts<DamageAppliedEvent>(game).Single() is { SourceSeat: 2, TargetSeat: 1, Amount: 1, SourceLess: false },
            "The real Barbarian retains its original user and first damages Xiahou Dun from the living Huoshou holder.");

        var control = Cold(game, content);
        Skip(control);
        Settle(control);
        Require(control.CreateSnapshot(2).Players[2].IsAlive &&
            Facts<DamageAppliedEvent>(control).Single(fact => fact.TargetSeat == 3) is { SourceSeat: 2, Amount: 1, SourceLess: false } &&
            Facts<CardUseFinishedEvent>(control).Count(fact => fact.ResolutionId == use.Id && fact.CardId == cardId) == 1,
            "Skipping Ganglie keeps the original Huoshou source for the later target of the same Barbarian.");

        Activate(game);
        Reach(game, prompt => prompt is { PlayerSeat: 2, SkillPrompt.SkillId: Ganglie } &&
            prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "damage"));
        Require(Facts<JudgmentResolvedEvent>(game).Single(fact => fact.Reason == "skill.ganglie").Suit == Suit.Spade &&
            Facts<DamageAppliedEvent>(game).All(fact => fact.TargetSeat != 3),
            "The real non-heart Ganglie judgment asks the attributed source before the later Barbarian target is processed.");
        game = Cold(game, content);
        Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "damage");
        Settle(game);
        var death = game.Events.Single(entry => entry.Payload is PlayerDiedEvent { VictimSeat: 2, KillerSeat: 1 });
        var later = game.Events.Single(entry => entry.Payload is DamageAppliedEvent { TargetSeat: 3, Amount: 1 });
        Require(death.Sequence < later.Sequence && !game.CreateSnapshot(2).Players[2].IsAlive &&
            Facts<GroupResponseEvent>(game).Any(fact => fact.ResolutionId == use.Id && fact.ResponderSeat == 3 && !fact.UsedResponse) &&
            Facts<GroupCardUsedEvent>(game).Single() is { SourceSeat: 0 } original && original.ResolutionId == use.Id &&
            Facts<CardUseFinishedEvent>(game).Count(fact => fact.ResolutionId == use.Id && fact.CardId == cardId) == 1,
            "Ganglie kills Meng Huo before the later seat takes damage from the original still-resolving Barbarian.");
        Require(later.Payload is DamageAppliedEvent { SourceLess: true } &&
            Facts<DamageRequestedEvent>(game).Single(fact => fact.TargetSeat == 3 && fact.SourceCard == CardKind.BarbarianAssault).SourceLess,
            "After the attributed Meng Huo dies, the original Barbarian must deal source-less damage to its later target.");
        _ = Cold(game, content);
    }

    private static T[] Facts<T>(GameEngine game) => game.Events.Select(entry => entry.Payload).OfType<T>().ToArray();

    private static GameEngine Cold(GameEngine game, ContentRegistry content)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), content);
        Require(restored.CardMovements.SequenceEqual(game.CardMovements) && Enumerable.Range(0, game.PlayerCount).All(seat =>
            SnapshotJson.Serialize(restored.CreateSnapshot(seat)) == SnapshotJson.Serialize(game.CreateSnapshot(seat))) &&
            Facts<HuoshouAttributedEvent>(restored).SequenceEqual(Facts<HuoshouAttributedEvent>(game)) &&
            Facts<DamageAppliedEvent>(restored).SequenceEqual(Facts<DamageAppliedEvent>(game)),
            "Cold accepted-command replay preserves the original use, source attribution, damage and visible state.");
        return restored;
    }

    private static (GameEngine Game, ContentRegistry Content) Start()
    {
        var content = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var game = GameEngine.CreateStandard(new GameOptions { Seed = 47, PlayerCount = 4, ModeId = Mode, HumanSeat = 0,
            HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord, UseInteractiveSetup = true,
            UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false }, content);
        Accept(game, new StartGameCommand());
        Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        while (P(game) is { Kind: DecisionKind.SelectGeneral } selection)
        {
            Accept(game, new SelectGeneralCommand(selection.PlayerSeat, Generals[selection.PlayerSeat], game.Revision, selection.PromptId));
            Reach(game, prompt => prompt.Kind is DecisionKind.SelectGeneral or DecisionKind.PlayCard);
        }
        Require(P(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard }, "The real Barbarian user starts the first Play phase.");
        return (game, content);
    }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-classic-huoshou-source", new(1, 0));

        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, MengHuo, XiahouDun);
            var rules = $$$"""
                {"skills":[{"id":"{{{Driver}}}","revision":1,"activations":[{"id":"wound-meng-huo","minCards":0,"maxCards":0,
                    "minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                    "effects":[{"op":"loseHp","target":"selectedTarget","amount":3}]}]}]}
                """;
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Driver] = new { name = "失去体力", description = "令一名其他角色失去3点体力。" } } });
            var catalog = SkillProgramCatalog.LoadCurrent(rules, presentation);
            builder.AddSkill(new(Driver, catalog.Presentations[Driver].Name, catalog.Presentations[Driver].Description)
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            builder.AddGeneral(new(Source, "南蛮使用者", "supporter", Driver, "wei", 6));
            builder.AddGeneral(new(Peer, "后续目标", "supporter", "standard:none", "wei", 6));
            builder.AddDeck(new(Deck, "固定黑桃南蛮", 2, 0, []) { PhysicalCards = Enumerable.Range(0, 40)
                .Select(_ => new ContentDeckPhysicalCard("standard:barbarian_assault", Suit.Spade, 7)).ToArray() });
            builder.AddMode(new(Mode, "祸首原伤害来源", 4, 4, new Dictionary<string, int>
                { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: Generals));
        }
    }
}
