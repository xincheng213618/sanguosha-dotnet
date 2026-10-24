using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class TacticalPlanningIntegrationChecks
{
    public static void WineUsesFinalAttackScoreAndKeepsHistoricalPolicy()
    {
        var wine = Card(1, CardKind.Alcohol, Suit.Heart);
        var slashCard = Card(2, CardKind.Slash, Suit.Spade);
        var shield = Card(3, CardKind.RenwangShield, Suit.Club);
        var view = new GameSnapshot(null, 0, EngineStatus.AwaitingHumanPlay, Winner.None, 1, 0,
            TurnPhase.Play, 80, 0,
            [new(0, "owner", true, null, false, "fixture:wine-owner", "owner", "", 4, 4, true, 2, [wine, slashCard])
                { TeamId = "ally", Skills = [], SecondarySkills = [] },
             new(1, "enemy", false, null, false, "fixture:wine-enemy", "enemy", "", 4, 4, true, 0, [])
                { TeamId = "enemy", Equipment = [shield], Skills = [], SecondarySkills = [] }], null)
            { ModeKind = ContentModeKind.Team };
        var slash = new LegalAction(LegalActionKind.Slash, slashCard.Id, 1, "known black Slash")
            { SlashTraits = new(false, false, DamageNature.Normal) };
        var drink = new LegalAction(LegalActionKind.Alcohol, wine.Id, null, "drink");
        var end = new LegalAction(LegalActionKind.EndPlay, null, null, "end");
        var actions = new[] { drink, slash, end };
        var before = SnapshotJson.Serialize(view);
        var old = Brain(AiDecisionPolicy.CooperativeCategoryGiftVersion).ChoosePlay(view, actions, 1);
        var current = Brain().ChoosePlay(view, actions, 1);
        Need(old.Action == drink && current.Action == end &&
            current.Thought.Candidates.Single(candidate => candidate.Action == drink).Score < 0 &&
            current.Thought.Candidates.Single(candidate => candidate.Action == slash).Score < 0,
            "The current policy must not drink for a Slash rejected by its final public-armor scorer; historical wine priority stays unchanged.");

        var bypass = slash with { SlashTraits = new(false, true, DamageNature.Normal) };
        var useful = Brain().ChoosePlay(view, [drink, bypass, end], 1);
        Need(useful.Action == drink && useful.Thought.Candidates.Single(candidate => candidate.Action == bypass).Score > 0,
            "Published armor bypass makes the same physical Slash useful, so Alcohol may prepare it instead of being blanket-disabled.");
        Need(SnapshotJson.Serialize(view) == before && slash.SlashTraits is { IsRed: false, IgnoresArmor: false },
            "Nested follow-up scoring must not alter the original player view or the published attack traits.");
    }

    public static void NativeLiubingTraitsArePureAndOverridePrintedSlashColor()
    {
        // Reuse the existing real-command fixture rather than duplicate its
        // native Liubing registration, guaranteed black deck and paced setup.
        var (game, registry) = StartExistingWuJingBlackFixture();
        var checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var zones = JsonSerializer.Serialize(game.CreateCardZoneDiagnostics());
        var eventCount = game.Events.Count;
        var source = game.CreateSnapshot(0).Players.Single(player => player.Seat == 0);
        Need(source.Skills?.Any(skill => skill.ContentId == "ol:liubing") == true &&
            !game.Events.Select(item => item.Payload).OfType<WuJingActualPhysicalSlashEvent>().Any(),
            "The fixture must really own native Liubing before its first physical Slash, rather than forge an AI hint.");

        var actions = game.GetHumanLegalActions();
        var slash = actions.First(action => action.Kind == LegalActionKind.Slash && action.CardId is > 0 &&
            action.PlayedCardKind is null && action.TargetSeats.Count == 1 &&
            source.Hand.Any(card => card.Id == action.CardId && card.Kind == CardKind.Slash && card.Suit is Suit.Spade or Suit.Club));
        Need(slash.SlashTraits is { IsRed: true, IgnoresArmor: false, Nature: DamageNature.Normal },
            "Core must publish the actual first Liubing Slash as red despite its physical black suit.");
        var repeated = game.GetHumanLegalActions();
        Need(JsonSerializer.Serialize(actions) == JsonSerializer.Serialize(repeated) &&
            GameCheckpointJson.Serialize(game.CreateCheckpoint()) == checkpoint && game.Events.Count == eventCount &&
            JsonSerializer.Serialize(game.CreateCardZoneDiagnostics()) == zones,
            "Publishing current Slash traits must be a pure query: no usage, command, event, inventory or save state may change.");

        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(checkpoint), registry);
        var restoredSlash = restored.GetHumanLegalActions().Single(action => action.Kind == slash.Kind &&
            action.CardId == slash.CardId && action.PlayedCardKind == slash.PlayedCardKind &&
            action.TargetSeats.SequenceEqual(slash.TargetSeats) && action.ConversionSource == slash.ConversionSource);
        Need(restoredSlash.SlashTraits == slash.SlashTraits &&
            GameCheckpointJson.Serialize(restored.CreateCheckpoint()) == checkpoint,
            "A cold restore must recompute the same ephemeral native Slash traits without adding them to the checkpoint.");

        // Keep the real issued use and private owner hand, then isolate its AI
        // consequence against a public shield. Changing the defender's armor
        // does not create a new action or reveal any other player's hand.
        var shield = Card(int.MaxValue, CardKind.RenwangShield, Suit.Club);
        var targetSeat = slash.TargetSeat!.Value;
        var view = game.CreateSnapshot(0) with { ModeKind = ContentModeKind.Team };
        view = view with
        {
            Players = view.Players.Select(player => player with
            {
                IsAlive = player.Seat == 0 || player.Seat == targetSeat,
                TeamId = player.Seat == 0 ? "ally" : "enemy",
                Equipment = player.Seat == targetSeat ? [shield] : player.Equipment
            }).ToArray()
        };
        var end = new LegalAction(LegalActionKind.EndPlay, null, null, "end");
        var native = Brain().ChoosePlay(view, [slash, end], 1);
        var ordinaryBlack = slash with { SlashTraits = new(false, false, DamageNature.Normal) };
        var blocked = Brain().ChoosePlay(view, [ordinaryBlack, end], 1);
        Need(native.Action == slash && native.Thought.Candidates.Single(candidate => candidate.Action == slash).Score > 0 &&
            blocked.Action == end,
            "The planner must use Core's actual red declaration for Liubing; an otherwise equal certainly black Slash is blocked by Renwang.");
    }

    public static void NativeYuanchouTraitsRespectOwnerAndTargetVisibility()
    {
        var (ownerGame, _) = StartExistingFixture(typeof(FanJiangZhangDaChecks), "black");
        var ownerCheckpoint = GameCheckpointJson.Serialize(ownerGame.CreateCheckpoint());
        var ownerUse = ownerGame.GetHumanLegalActions().First(action => action.Kind == LegalActionKind.Slash &&
            action.TargetSeats.Count == 1 && action.SlashTraits?.IsRed == false);
        Need(ownerGame.CreateSnapshot(0).Players[0].Skills?.Any(skill => skill.ContentId == "ol:yuanchou") == true &&
            ownerUse.SlashTraits is { IgnoresArmor: true } &&
            GameCheckpointJson.Serialize(ownerGame.CreateCheckpoint()) == ownerCheckpoint,
            "The native Fan Jiang Zhang Da fixture must publish its owner's black-Slash armor bypass without consuming any rule state.");

        // Reuse the retained fixture's native Yuanchou registration and real
        // physical card definition; the projection scene adds a minimal source.
        var fixtureType = typeof(FanJiangZhangDaChecks).GetNestedType("Fixture", BindingFlags.NonPublic) ??
            throw new InvalidOperationException("The retained native Fan Jiang Zhang Da content fixture is missing.");
        var native = Activator.CreateInstance(fixtureType, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null, args: ["black"], culture: null) as IGameContentPackage ??
            throw new InvalidOperationException("The retained native Fan Jiang Zhang Da content fixture could not be loaded.");
        var registry = ContentRegistry.Build(new StandardContentPackage(), new YuanchouProjectionFixture(native));
        var options = new GameOptions
        {
            Seed = 7, PlayerCount = 4, HumanSeat = 0, HumanRole = null,
            ModeId = YuanchouProjectionFixture.Mode, UseInteractiveSetup = false,
            AdvanceAfterHumanCommands = false, UseInteractiveDiscard = false, MaxTurns = 12
        };
        // Automatic fixed-seed national setup reserves two generals per faction
        // without AI choices. Keep the human in the starting seat; if that seat
        // was assigned the other faction, exchange only the fixture's faction
        // labels so it still owns the driver rather than Yuanchou.
        var setup = GameEngine.CreateStandard(options, registry).CreateSnapshot(-1, true);
        if (!setup.Players[0].GeneralId.StartsWith(YuanchouProjectionFixture.SourcePrefix, StringComparison.Ordinal))
        {
            registry = ContentRegistry.Build(new StandardContentPackage(), new YuanchouProjectionFixture(native, "qun"));
            setup = GameEngine.CreateStandard(options, registry).CreateSnapshot(-1, true);
        }
        const int source = 0;
        Need(setup.Players[source].GeneralId.StartsWith(YuanchouProjectionFixture.SourcePrefix, StringComparison.Ordinal),
            "The fixed-seed national fixture must start with the non-Yuanchou human source.");
        var target = setup.Players.First(player => player.GeneralId.StartsWith(YuanchouProjectionFixture.TargetPrefix,
            StringComparison.Ordinal)).Seat;
        var game = GameEngine.CreateStandard(options, registry);
        Submit(game, new StartGameCommand());
        AdvanceUntil(game, () => CurrentDecision(game) is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat == source);
        Submit(game, new RevealGeneralCommand(source, GeneralSelectionSlot.Primary, game.Revision, CurrentDecision(game)!.PromptId));
        AdvanceUntil(game, () => CurrentDecision(game) is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat == source);
        Need(game.CreateSnapshot(source).Players[source].Skills?.All(skill => skill.ContentId != "ol:yuanchou") == true &&
            game.CreateSnapshot(target).Players[target].Skills?.Any(skill => skill.ContentId == "ol:yuanchou") == true &&
            game.CreateSnapshot(source).Players[target].Skills is null &&
            game.CreateSnapshot(source).Players[target].SecondarySkills is null,
            "The actual source must lack Yuanchou while the target owns it privately behind the engine's genuine unrevealed-general projection.");
        var checkpoint = GameCheckpointJson.Serialize(game.CreateCheckpoint());
        var hiddenUse = BlackSlashAt(game, target);
        Need(hiddenUse.SlashTraits is { IsRed: false, IgnoresArmor: false } &&
            GameCheckpointJson.Serialize(game.CreateCheckpoint()) == checkpoint,
            "A hidden opponent's native two-way policy must not leak through the source's legal-action metadata.");

        Submit(game, new EndPlayPhaseCommand(source, game.Revision, CurrentDecision(game)!.PromptId));
        AdvanceUntil(game, () => game.State.TurnNumber > 1 &&
            CurrentDecision(game) is { Kind: DecisionKind.PlayCard } play && play.PlayerSeat == source);
        Need(game.CreateSnapshot(source).Players[target].Skills?.Any(skill => skill.ContentId == "ol:yuanchou") == true,
            "The target's actual national-war AI turn must publicly reveal its native skill before the second query.");
        var visibleUse = BlackSlashAt(game, target);
        Need(visibleUse.SlashTraits is { IsRed: false, IgnoresArmor: true },
            "Once the target's Yuanchou is public, Core must publish its opposite-direction black-Slash armor bypass for the same source.");
    }

    private static LegalAction BlackSlashAt(GameEngine game, int target)
    {
        var actions = game.GetHumanLegalActions();
        return actions.FirstOrDefault(action => action.Kind == LegalActionKind.Slash && action.CardId is > 0 &&
            action.TargetSeats.SequenceEqual([target]) && action.SlashTraits?.IsRed == false) ??
            throw new InvalidOperationException("The native Yuanchou projection fixture has no current physical black Slash: " +
                JsonSerializer.Serialize(new { target, game.State.CurrentSeat, game.State.Phase, Prompt = CurrentDecision(game),
                    Owner = game.CreateSnapshot(0).Players[0], Actions = actions }));
    }

    private static PendingDecision? CurrentDecision(GameEngine game) => Enumerable.Range(0, game.State.Players.Count)
        .Select(seat => game.CreateSnapshot(seat).PendingDecision).FirstOrDefault(decision => decision is not null);

    private static void Submit(GameEngine game, GameCommand command)
    {
        var result = game.Submit(command);
        Need(result.Accepted, result.Error?.Message ?? "The native projection fixture rejected its command.");
    }

    private static void AdvanceUntil(GameEngine game, Func<bool> done)
    {
        for (var step = 0; step < 160; step++)
        {
            if (done()) return;
            Need(game.State.Status is not (EngineStatus.Completed or EngineStatus.Faulted),
                "The quiet native projection fixture stopped before its requested setup/turn boundary.");
            var pending = CurrentDecision(game);
            var human = pending is not null && game.CreateSnapshot(pending.PlayerSeat).Players[pending.PlayerSeat].IsHuman;
            if (human && pending is { Kind: DecisionKind.PlayCard })
                Submit(game, new EndPlayPhaseCommand(pending.PlayerSeat, game.Revision, pending.PromptId));
            else
            {
                Need(!human,
                    "The native projection fixture unexpectedly requires another human choice.");
                Submit(game, new AdvanceOneStepCommand(game.Revision));
            }
        }
        var stalled = CurrentDecision(game);
        throw new InvalidOperationException("The native projection fixture exceeded its bounded setup/turn steps: " +
            JsonSerializer.Serialize(new { game.State.Status, game.State.Phase, game.State.CurrentSeat, game.State.TurnNumber,
                Pending = stalled is null ? null : new
                {
                    stalled.Kind, stalled.PlayerSeat, SkillId = stalled.SkillPrompt?.SkillId,
                    Choices = stalled.Choices.Take(6).Select(choice => new { choice.Id, choice.Parameters })
                } }));
    }

    private sealed class YuanchouProjectionFixture(IGameContentPackage native, string sourceFaction = "wu") : IGameContentPackage
    {
        internal const string SourcePrefix = "fixture:ai-yuanchou-source-", TargetPrefix = "fixture:ai-yuanchou-target-";
        internal const string Mode = "national:ai-yuanchou-projection";
        private const string Driver = "fixture:ai-yuanchou-driver", Deck = "fixture:ai-yuanchou-deck";
        public PackageManifest Manifest { get; } = new("fixture-ai-yuanchou-projection", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            native.Register(builder);
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{Driver}}","revision":1,
                   "modifiers":[{"id":"range","query":"attackRange","operation":"set","value":4,"priority":0}],
                   "viewAs":[{"id":"black-physical-slash","inputKinds":["crossbow"],"inputSuits":["spade"],
                     "sourceZones":["hand"],"outputKind":"slash","forPlay":true,"forResponse":false}]}]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [Driver] = new { name = "实际黑色杀来源", description = "仅用于原生可见信息投影的单张手牌转换。" } } }));
            builder.AddSkill(new(Driver, "实际黑色杀来源", "固定实体黑色杀转换")
                { Program = catalog.Programs[Driver], ProgramPresentation = catalog.Presentations[Driver] });
            for (var index = 0; index < 4; index++)
            {
                builder.AddGeneral(new(SourcePrefix + index, "公开攻击来源" + index, "supporter", Driver, sourceFaction, 12, []));
                builder.AddGeneral(new(TargetPrefix + index, "真实怨仇目标" + index, "ol-fan-jiang-zhang-da", "ol:yuanchou",
                    sourceFaction == "wu" ? "qun" : "wu", 12, []));
            }
            builder.AddDeck(new(Deck, "单张黑色实体费用", 1, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 16).Select(_ =>
                    new ContentDeckPhysicalCard("fixture:fjzd-card-Crossbow", Suit.Spade, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "原生怨仇可见信息投影", 4, 4,
                new Dictionary<string, int>(),
                Deck, GeneralCandidateCount: 4,
                GeneralPoolIds: Enumerable.Range(0, 4).SelectMany(index => new[] { SourcePrefix + index, TargetPrefix + index }).ToArray(),
                ModeKind: ContentModeKind.NationalWarLite,
                FactionCounts: new Dictionary<string, int> { ["wu"] = 2, ["qun"] = 2 }));
        }
    }

    private static (GameEngine Game, ContentRegistry Registry) StartExistingWuJingBlackFixture()
        => StartExistingFixture(typeof(WuJingChecks), "black");

    private static (GameEngine Game, ContentRegistry Registry) StartExistingFixture(Type fixture, string scene)
    {
        var start = fixture.GetMethod("Start", BindingFlags.Static | BindingFlags.NonPublic) ??
            throw new InvalidOperationException("The retained native fixture no longer exposes its paced setup helper.");
        try { return ((GameEngine, ContentRegistry))start.Invoke(null, [scene])!; }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }

    private static SimpleAiBrain Brain(int policy = AiDecisionPolicy.TacticalPlanningVersion) => new(0, 91, policy);
    private static CardSnapshot Card(int id, CardKind kind, Suit suit) =>
        new(id, kind, suit, 7, CardCatalog.Get(kind).DisplayName, "7");
    private static void Need(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
