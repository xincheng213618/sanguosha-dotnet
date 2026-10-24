using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class SourceLessGanglieChecks
{
    private const string Driver = "fixture:ganglie-source-driver", Owner = "fixture:ganglie-source-owner";
    private const string Mode = "identity:ganglie-source-fixture", Deck = "fixture:ganglie-source-deck";

    public static void LightningKeepsJudgmentButSkipsMissingSourceAndColdRestores()
    {
        foreach (var skill in new[] { "standard:ganglie", "classic:ganglie" })
        {
            var (game, registry) = Start(skill, lightning: true);
            var delayed = game.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Lightning);
            Accept(game, new PlayCardCommand(0, delayed.CardId!.Value, [0], game.Revision, P(game)!.PromptId));
            Play(game);
            Use(game, "order");
            var viewed = game.ResolutionStack.OfType<ProgramSkillFrame>().Single().TopReorder!.ViewedCardIds;
            var kinds = game.CreateCardZoneDiagnostics().ToDictionary(card => card.CardId, card => card.CardKind);
            var hit = viewed.First(id => kinds[id] == CardKind.Lightning);
            var retaliation = viewed.First(id => kinds[id] == CardKind.Peach);
            var rest = viewed.Where(id => id != hit && id != retaliation).ToArray();
            // Three other seats each draw two before the original owner's
            // Lightning judgment, followed by the genuine Ganglie judgment.
            var order = rest.Take(6).Concat([hit, retaliation]).Concat(rest.Skip(6)).ToArray();
            foreach (var id in order)
                Answer(game, choice => choice.Cards.SequenceEqual([id]) && choice.Parameters.GetValueOrDefault("action") == "top");
            Play(game);
            var beforeHp = game.CreateSnapshot(0).Players[0].Hp;
            Accept(game, new EndPlayPhaseCommand(0, game.Revision, P(game)!.PromptId));
            Reach(game, () => P(game) is { SkillPrompt.SkillId: var id } prompt && id == skill &&
                prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("program-action") == "activate"));
            Need(Events<DamageAppliedEvent>(game).Last() is { TargetSeat: 0, Amount: 3, SourceLess: true },
                "Actual placed Lightning must deal its native three-point source-less damage before Ganglie is offered.");
            Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Play(game);
            var judged = Events<JudgmentResolvedEvent>(game).Single(item => item.Reason == "skill.ganglie");
            Need(judged.CardId == retaliation && judged.Suit == Suit.Diamond &&
                 Events<JudgmentResolvedEvent>(game).Single(item => item.Reason == "trick.lightning").CardId == hit,
                "Both variants must still judge the real next Diamond card after source-less Lightning.");
            Need(game.CreateSnapshot(0).Players[0].Hp == beforeHp - 3 &&
                 !Events<ProgramOptionChosenEvent>(game).Any(item => item.SkillId == skill && item.ResultBind == "punishment") &&
                 Events<DamageAppliedEvent>(game).Count() == 1,
                "A missing source must create no punishment chooser, invented self-source or retaliatory damage.");
            Need(game.CardMovements.Any(move => move.CardId == retaliation && move.To == CardLocation.DiscardPile) &&
                 game.CreateSnapshot(0).ProcessingCardCount == 0 && game.ResolutionStack.Count == 0,
                "The judgment binding must finish in discard and release its original program frame normally.");
            Cold(game, registry);
        }
    }

    public static void SourcedJudgmentStillPunishesOriginalParticipantAndConditionShapeIsValidated()
    {
        foreach (var skill in new[] { "standard:ganglie", "classic:ganglie" })
        {
            var (game, registry) = Start(skill, lightning: false);
            var before = game.CreateSnapshot(0).Players[1].Hp;
            Use(game, "hit", [1]);
            Reach(game, () => P(game)?.SkillPrompt?.SkillId == skill);
            Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("program-action") == "activate");
            Reach(game, () => P(game) is { SkillPrompt.SkillId: var id } prompt && id == skill &&
                prompt.Choices.Any(choice => choice.Parameters.GetValueOrDefault("option-id") == "damage"));
            Need(P(game)!.PlayerSeat == 1 && game.ResolutionStack.OfType<ProgramSkillFrame>().Last().WindowContext?.SourceSeat == 1,
                "The paused punishment must still belong to the original real source after its judgment child returns.");
            Cold(game, registry);
            Answer(game, choice => choice.Parameters.GetValueOrDefault("option-id") == "damage");
            Play(game);
            Need(game.CreateSnapshot(0).Players[1].Hp == before - 1 &&
                 Events<DamageAppliedEvent>(game).Any(item => item.SourceSeat == 0 && item.TargetSeat == 1 && item.Amount == 1 && !item.SourceLess),
                "A present source must retain the ordinary Ganglie damage branch and exact native damage source.");
            Cold(game, registry);
        }

        var valid = Trigger("\"kind\":\"eventSourceExists\"");
        var condition = Load(valid).Programs["fixture:source-condition"].Triggers.Single().Effects.Single().Condition;
        var context = new PlayerSkillContext(0, 4, 4, 0, TurnPhase.Play, true);
        Need(condition.Evaluate(context, null, _ => false, _ => false, eventSourceExists: true) &&
             !condition.Evaluate(context, null, _ => false, _ => false, eventSourceExists: false),
            "The new frame condition must use explicit event-source presence rather than owner or selected-target identity.");
        Reject(Trigger("\"kind\":\"eventSourceExists\",\"value\":1"), "does not accept value");
        Reject("""{"id":"fixture:source-condition","revision":1,"activations":[{"id":"bad","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,"effects":[{"op":"draw","target":"owner","amount":1,"condition":{"kind":"eventSourceExists"}}]}]}""",
            "trigger event context");
    }

    private static string Trigger(string condition) =>
        $$$$"""{"id":"fixture:source-condition","revision":1,"triggers":[{"id":"test","window":"afterDamageApplied","subject":"owner","damageOccurrence":"perDamage","optional":true,"effects":[{"op":"draw","target":"owner","amount":1,"condition":{ {{{{condition}}}} }}]}]}""";

    private static SkillProgramCatalog Load(string skill) => SkillProgramCatalog.Load(
        $$$$"""{"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[{{{{skill}}}}]}""",
        $$$$"""{"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{"fixture:source-condition":{"name":"source condition","description":"source condition"}}}""");

    private static void Reject(string skill, string text)
    {
        try { _ = Load(skill); }
        catch (InvalidOperationException exception) when (exception.Message.Contains(text, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException("The condition loader failed to reject malformed eventSourceExists: " + text);
    }

    private static (GameEngine, ContentRegistry) Start(string skill, bool lightning)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(skill, lightning));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = false, AdvanceAfterHumanCommands = false, MaxTurns = 8,
            AiPolicyVersion = AiDecisionPolicy.TacticalPlanningVersion
        }, registry);
        Accept(game, new StartGameCommand());
        Accept(game, new SelectGeneralCommand(0, Owner, game.Revision, P(game)!.PromptId));
        Play(game);
        return (game, registry);
    }

    private static T[] Events<T>(GameEngine game) => game.Events.Select(item => item.Payload).OfType<T>().ToArray();
    private static PendingDecision? P(GameEngine game) => game.PendingDecision ?? Enumerable.Range(0, 4)
        .Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(prompt => prompt is not null);
    private static void Use(GameEngine game, string activation, int[]? targets = null) =>
        Accept(game, new UseProgramSkillCommand(0, Driver, activation, [], targets ?? [], game.Revision, P(game)!.PromptId));
    private static void Answer(GameEngine game, Func<PromptChoice, bool> pick)
    {
        var prompt = P(game) ?? throw new InvalidOperationException("Expected a real native Ganglie prompt.");
        Accept(game, new AnswerPromptCommand(prompt.PlayerSeat, prompt.PromptId, prompt.Choices.First(pick).Id, game.Revision));
    }
    private static void Play(GameEngine game) => Reach(game, () => P(game) is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
    private static void Reach(GameEngine game, Func<bool> ready)
    {
        for (var step = 0; step < 160; step++)
        {
            if (ready()) return;
            Need(game.State.Status != EngineStatus.Completed && P(game)?.PlayerSeat != 0,
                "Unexpected Ganglie fixture boundary: " + JsonSerializer.Serialize(P(game)));
            Accept(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("The bounded native Ganglie boundary was not reached.");
    }
    private static void Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(Enumerable.Range(0, 4).All(seat => SnapshotJson.Serialize(game.CreateSnapshot(seat)) == SnapshotJson.Serialize(restored.CreateSnapshot(seat))) &&
             game.CardMovements.SequenceEqual(restored.CardMovements) && JsonSerializer.Serialize(game.ResolutionStack) == JsonSerializer.Serialize(restored.ResolutionStack),
            "Ganglie accepted commands must recover the same player views, original frames and physical movements.");
    }
    private static void Accept(GameEngine game, GameCommand command)
    {
        var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
        Need(result.Accepted, result.Error?.ToString() ?? "Native Ganglie fixture command rejected.");
    }
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class Fixture(string skill, bool lightning) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ganglie-source", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            if (skill == "classic:ganglie") CharacterFixtureContent.Register(builder, "classic:xiahou-dun");
            var catalog = SkillProgramCatalog.Load(
                $$$$"""{"schemaVersion":{{{{SkillProgramCatalog.RulesSchemaVersion}}}},"skills":[{"id":"{{{{Driver}}}}","revision":1,"activations":[{"id":"order","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"reorderTopCards","target":"owner","amount":12}]},{"id":"hit","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":null,"effects":[{"op":"damage","target":"owner","sourceRef":{"kind":"selectedTarget"},"amount":1}]}]}]}""",
                $$$$"""{"schemaVersion":{{{{SkillProgramCatalog.PresentationSchemaVersion}}}},"skills":{"{{{{Driver}}}}":{"name":"真实判定及伤害","description":"固定牌序与真实来源"}}}""");
            builder.AddSkill(new(Driver, "真实判定及伤害", "固定牌序与真实来源") { Program = catalog.Programs[Driver] });
            builder.AddSkill(new("fixture:ganglie-source-quiet", "quiet", "quiet"));
            builder.AddGeneral(new(Owner, "Ganglie owner", "supporter", skill, "wei", 8, [Driver]));
            for (var seat = 1; seat < 4; seat++)
                builder.AddGeneral(new($"fixture:ganglie-source-other-{seat}", "other", "supporter", "fixture:ganglie-source-quiet", "wei", 12));
            builder.AddDeck(new(Deck, "fixed", 4, 2, [])
            {
                PhysicalCards = Enumerable.Range(0, 96).Select(index => new ContentDeckPhysicalCard(
                    lightning && index % 2 == 0 ? "standard:lightning" : "standard:peach",
                    lightning && index % 2 == 0 ? Suit.Spade : Suit.Diamond, 5)).ToArray()
            });
            builder.AddMode(new(Mode, "Ganglie source fixture", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 }, Deck,
                GeneralCandidateCount: 4, GeneralPoolIds: [Owner, "fixture:ganglie-source-other-1", "fixture:ganglie-source-other-2", "fixture:ganglie-source-other-3"]));
        }
    }
}
