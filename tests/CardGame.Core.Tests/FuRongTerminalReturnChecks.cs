using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class FuRongTerminalReturnChecks
{
    private const string Mode = "identity:classic-fu-rong-terminal", Owner = "fixture:fu-rong-terminal-owner";
    private const string Peer = "fixture:fu-rong-terminal-peer", Watch = "fixture:fu-rong-native-watch";
    private const string Skill = "ol:xiaosi";

    public static void LethalUseAndRealLeijiRetaliationFinishPaidAncestorCold()
    {
        Run(retaliation: false);
        Run(retaliation: true);
    }

    private static void Run(bool retaliation)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(retaliation));
        var game = GameEngine.CreateStandard(new GameOptions
        {
            Seed = 31, PlayerCount = 2, HumanSeat = 0, HumanRole = null, HumanTeamId = "player",
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 6
        }, registry);
        var originalIds = game.CreateCardZoneDiagnostics().Select(c => c.CardId).Order().ToArray();
        Submit(game, new StartGameCommand());
        Reach(game, () => Pending(game) is { PlayerSeat: 0, Kind: DecisionKind.SelectGeneral });
        Submit(game, new SelectGeneralCommand(0, Owner, game.Revision, Pending(game)!.PromptId));
        Reach(game, () => Pending(game) is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var initialCombat = game.CreateSnapshot(-1, true).Players;
        Need(retaliation ? initialCombat[0].Hp == 2 : initialCombat[1].Hp == 1,
            "The real seeded first turn leaves the selected owner at two HP for Leiji, or the original opponent at one HP for native Slash.");
        var ordinaryDebits = Events<CardUseDebitRecordedEvent>(game).ToArray();
        var ownerCard = game.CreateSnapshot(0).Players[0].Hand[0].Id;
        Submit(game, new UseProgramSkillCommand(0, Skill, "paired-basic-discard", [], [1], game.Revision, Pending(game)!.PromptId));
        Answer(game, c => c.Cards.SequenceEqual([ownerCard]));
        Reach(game, () => Pending(game)?.Choices.Any(c => c.Parameters.GetValueOrDefault("branch") == "use") == true);
        var payments = Events<FuRongPairPaidEvent>(game).ToArray();
        Need(payments.Length == 2 && payments.All(p => p.Payment.EffectiveKind == CardKind.Slash),
            "Both participants actually discard one original basic Slash before either native use.");
        Answer(game, c => c.Parameters.GetValueOrDefault("branch") == "use" && c.Cards.SequenceEqual([ownerCard]) && c.Targets.SequenceEqual([1]));
        Reach(game, () => Pending(game)?.SkillPrompt?.SkillId == Watch);
        var origin = Events<FuRongNativeIssuedEvent>(game).Single().Return;
        Need(game.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.Id == origin.ProgramFrameId).FuRongPair is
                { Stage: FuRongPairStage.NativeUse, NativeCompleted: false } &&
                origin.CardId == ownerCard && game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.Id == origin.CardUseFrameId).FuRongReturn == origin,
            "The paid original discard pauses inside its exact real native committed child before lethal resolution.");
        game = Cold(game, registry);
        Answer(game, c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        Reach(game, () => game.State.Winner != Winner.None);
        var winner = retaliation ? 1 : 0;
        var loser = 1 - winner;
        var players = game.CreateSnapshot(-1, true).Players;
        Need(game.State.Status == EngineStatus.Completed && players[winner].IsAlive && !players[loser].IsAlive &&
                game.ResolutionStack.Count == 0 && Pending(game) is null && game.CreateSnapshot(0).ProcessingCardCount == 0 &&
                Events<FuRongPairCompletedEvent>(game).Count(e => e.ProgramFrameId == origin.ProgramFrameId && e.OwnerPaid && e.TargetPaid && e.UsedCount == 1) == 1 &&
                Events<ProgramSkillResolvedEvent>(game).Count(e => e.FrameId == origin.ProgramFrameId && e.Completed) == 1 &&
                Events<FuRongNativeReturnedEvent>(game).Count(e => e.Return == origin) == 1 &&
                Events<CardUseFinishedEvent>(game).Count(e => e.ResolutionId == origin.CardUseFrameId) == 1,
            "Lethal native damage or real Leiji retaliation completes the same paid ancestor once, with no post-win step or second material use.");
        Need(Events<FuRongPairPaidEvent>(game).Count() == 2 && Events<FuRongNativeIssuedEvent>(game).Count() == 1 &&
                Events<CardUseDebitRecordedEvent>(game).SequenceEqual(ordinaryDebits) &&
                game.CardMovements.Count(m => m.CardId == ownerCard && m.From == CardLocation.DiscardPile &&
                    m.To == CardLocation.Processing && m.Reason == CardMoveReasons.Use) == 1 &&
                payments.All(p => game.CreateCardZoneDiagnostics().Single(c => c.CardId == p.Payment.CardId).Location == CardLocation.DiscardPile) &&
                originalIds.SequenceEqual(game.CreateCardZoneDiagnostics().Select(c => c.CardId).Order()),
            "Terminal completion preserves both actual payments, the unlimited-use allowance and every original physical entity.");
        if (retaliation)
            Need(Events<DamageAppliedEvent>(game).Any(e => e.SourceSeat == 1 && e.TargetSeat == 0 && e.Amount == 2),
                "The owner really dies to the defender's spade Leiji while its issued native Slash is outstanding.");
        _ = Cold(game, registry);
    }

    private static IEnumerable<T> Events<T>(GameEngine game) => game.Events.Select(e => e.Payload).OfType<T>();
    private static PendingDecision? Pending(GameEngine game) => Enumerable.Range(0, 2)
        .Select(s => game.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
        Need(result.Accepted, result.Error?.Message ?? "Rejected terminal Fu Rong command.");
    }
    private static void Answer(GameEngine game, Func<PromptChoice, bool> choose)
    {
        var pending = Pending(game)!;
        Submit(game, new AnswerPromptCommand(pending.PlayerSeat, pending.PromptId, pending.Choices.First(choose).Id, game.Revision));
    }
    private static void Reach(GameEngine game, Func<bool> reached)
    {
        for (var step = 0; step < 120; step++)
        {
            if (reached()) return;
            if (Pending(game) is { PlayerSeat: 0, Kind: DecisionKind.RescueDying })
            {
                Answer(game, c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
                continue;
            }
            if (Pending(game) is { PlayerSeat: 0 } prompt)
                throw new InvalidOperationException("Unexpected terminal Fu Rong prompt: " + JsonSerializer.Serialize(prompt));
            Submit(game, new AdvanceOneStepCommand(game.Revision));
        }
        throw new InvalidOperationException("Terminal Fu Rong boundary was not reached.");
    }
    private static string State(GameEngine game) => JsonSerializer.Serialize(new
    {
        Views = Enumerable.Range(0, 2).Select(s => SnapshotJson.Serialize(game.CreateSnapshot(s))),
        Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())),
        Events = game.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())),
        game.CardMovements, Commands = CommandJson.Serialize(game.AcceptedCommands), Zones = game.CreateCardZoneDiagnostics()
    });
    private static GameEngine Cold(GameEngine game, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
        Need(State(game) == State(restored), "Cold replay preserves both exact paid entities, private choices, native return and final inventory.");
        return restored;
    }

    private sealed class SetupFactory : IGameModeSetupFeatureFactory
    {
        public string Id => "fixture:fu-rong-terminal-setup";
        public string ModeId => Mode;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase)
        {
            if (options.ModeId != Mode || options.PlayerCount != 2)
                throw new ArgumentException("The Fu Rong terminal fixture requires its exact two-seat team mode.");
        }
        public IGameModeFeature Create(ModeFeatureCreationContext context) =>
            new CardGame.Modes.Standard.StandardModeSetupFactory().Create(context);
    }

    private sealed class Fixture(bool retaliation) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-fu-rong-terminal", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "ol:fu-rong", "classic:zhao-yun", "boundary:zhang-jiao");
            var rules = $$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Watch}}","revision":1,"triggers":[{"id":"native-pause","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"native","options":[{"id":"continue"}]}]}]}
                ]}
                """;
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new
            {
                schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object> { [Watch] = new { name = "原生用牌继续", description = "真实效死原生用牌子树。", optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } }
            }));
            builder.AddSkill(new(Watch, "原生用牌继续", "真实效死原生用牌子树。") { Program = catalog.Programs[Watch], ProgramPresentation = catalog.Presentations[Watch] });
            builder.AddGeneral(new(Owner, "原生傅肜", "ol-fu-rong", Skill, "shu", retaliation ? 3 : 4, [Watch]));
            builder.AddSkill(new("fixture:fu-rong-peer-pick", "固定对手", "公开选将顺序。")
                { SelectionWeights = Enum.GetValues<Role>().ToDictionary(role => role, _ => 1000000d) });
            builder.AddGeneral(new(Peer, "原生对手", "supporter", "fixture:fu-rong-peer-pick", "qun", retaliation ? 4 : 1,
                retaliation ? ["classic:longdan", "boundary:leiji"] : []));
            builder.AddDeck(new("fixture:fu-rong-terminal-deck", "真实基本牌", 4, 2, [])
                { PhysicalCards = Enumerable.Range(0, 64).Select(i => new ContentDeckPhysicalCard("standard:slash", Suit.Spade, i % 13 + 1)).ToArray() });
            builder.AddModeFeatureFactory(new SetupFactory());
            builder.AddMode(new(Mode, "效死原生终局", 2, 2, new Dictionary<string, int>(), "fixture:fu-rong-terminal-deck", 2,
                [Owner, Peer], ContentModeKind.Team, new Dictionary<string, int> { ["player"] = 1, ["other"] = 1 }));
        }
    }
}
