using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class NativeCardEffectEpisodeFangtianChecks
{
    private const string Source = "fixture:episode-fangtian-source", Quiet = "fixture:episode-fangtian-quiet";
    private const string Hero = "fixture:episode-fangtian-owner", Peer = "fixture:episode-fangtian-peer";

    public static void DeclaredWholeAndLocalEffectsKeepTheRealThreeTargetFangtianCursorCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture());
        var setup = new ShanheBattleSetup(31, 1, "native-episode-fangtian", Hero, 12, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 7)], [new(CardKind.FangtianHalberd, Suit.Spade, 12)],
            [new(Peer, Hp: 12) { StartingHand = [] }, new(Peer, Hp: 12) { StartingHand = [] }],
            [new(Peer, Hp: 12) { StartingHand = [] }], new string('f', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
        Accept(new StartGameCommand()); Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var material = View(0).Hand.Single().Id;
        var chosen = game.GetHumanLegalActions().FirstOrDefault(a => a.Kind == LegalActionKind.Slash &&
            a.CardId == material && a.TargetSeats.SequenceEqual([1, 2, 3]));
        Need(chosen is not null && View(0).Equipment.Single().Kind == CardKind.FangtianHalberd,
            "One physical last-Hand Slash and the real installed weapon publish the native three-target Fangtian action.");
        Accept(new PlayCardCommand(0, material, chosen!.TargetSeats, game.Revision, Prompt()!.PromptId));
        Drive(() => Prompt()?.SkillPrompt?.SkillId == Source &&
            Prompt()!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        Need(use.TargetSeats.SequenceEqual([1, 2, 3]) && use.EffectEpisodes is { Episodes.Count: 2 } &&
            Events<FangtianHalberdUsedEvent>().Count(e => e.ResolutionId == use.Id && e.TargetSeats.SequenceEqual([1, 2, 3])) == 1,
            "The ordinary JSON writer declares whole-use and exact-slot episodes on the one actual Fangtian use before damage.");
        Cold(); AnswerContinue(); Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var started = Events<NativeCardEffectEpisodeStartedEvent>().Where(e => e.UseFrameId == use.Id).ToArray();
        Need(started.Length == 2 && started[0].Episode.Issue.Spec.Scope == NativeCardEffectEpisodeScope.WholeUse &&
            started[0].Cursor.TargetSeats.SequenceEqual([1, 2, 3]) &&
            started[1].Episode.Issue.Spec.Scope == NativeCardEffectEpisodeScope.ExecutionOccurrence &&
            started[1].Cursor.TargetIndex == 0 && started[1].Cursor.TargetSeats.SequenceEqual([1]) &&
            Enumerable.Range(1, 3).All(seat => View(seat).Hp == 12 - (seat == 1 ? 3 : 2) &&
                Events<DamageAppliedEvent>().Count(e => e.SourceSeat == 0 && e.TargetSeat == seat && e.Amount == 1) == (seat == 1 ? 3 : 2)) &&
            Events<NativeCardEffectEpisodeCompletedEvent>().Count(e => e.UseFrameId == use.Id && e.Executed) == 2 &&
            Events<FangtianHalberdUsedEvent>().Count(e => e.ResolutionId == use.Id) == 1 &&
            Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1,
            "The whole pass walks all three original Fangtian slots, while the local pass hits only slot zero and never restarts the full weapon cursor or pays another Slash.");
        Cold();

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot View(int seat) => game.CreateSnapshot(0, true).Players[seat];
        T[] Events<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        void Accept(GameCommand command)
        { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected Fangtian episode command."); }
        void AnswerContinue()
        { var p = Prompt()!; Accept(new AnswerPromptCommand(0, p.PromptId, p.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision)); }
        void Drive(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++)
            {
                if (ready()) return;
                Need(Prompt() is not { PlayerSeat: 0 }, "Unexpected original Fangtian prompt.");
                Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The bounded native Fangtian cursor did not return.");
        }
        string State() => JsonSerializer.Serialize(new { game.State, Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), game.CardMovements });
        void Cold()
        {
            var before = State(); game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(before == State(), "Cold replay retains the real Fangtian owner, declared episodes, physical material and execution slots.");
        }
        void Need(bool value, string message)
        { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Prompt = Prompt(), Frames = game.ResolutionStack, Facts = game.Events.TakeLast(8).Select(e => e.Payload.ToString()) })); }
    }

    public static void ExtraPassStartsBeyondTheDeadOriginalFangtianPrefixOnceCold()
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(latePrefix: true));
        var setup = new ShanheBattleSetup(31, 1, "native-episode-fangtian-late-prefix", Hero, 12, 0, 0, 0, 0, [],
            [new(CardKind.Slash, Suit.Heart, 7)], [new(CardKind.FangtianHalberd, Suit.Spade, 12)],
            [new(Peer, Hp: 1) { StartingHand = [] }, new(Peer, Hp: 12) { StartingHand = [] }],
            [new(Peer, Hp: 1) { StartingHand = [] }], new string('e', 64)) { StartingResourcesExpireOnUse = false };
        var game = GameEngine.CreateStandard(new() { Seed = setup.BattleSeed, PlayerCount = 4, HumanSeat = 0,
            HumanRole = null, HumanTeamId = ShanheBattleSetup.PlayerTeamId, ModeId = ShanheBattleSetup.BattleModeId,
            ShanheBattle = setup, UseInteractiveSetup = false, UseInteractiveDiscard = false,
            AdvanceAfterHumanCommands = false, MaxTurns = 3 }, registry);
        Accept(new StartGameCommand()); Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        Need(View(1).Hp == 1 && View(2).Hp == 1 && View(3).Hp == 12 &&
            View(1).TeamId == View(0).TeamId && View(2).TeamId == View(3).TeamId && View(3).TeamId != View(0).TeamId,
            "The fixed native roster puts one ally and one enemy in the original prefix, while a healthy enemy keeps the battle live.");
        Accept(new UseProgramSkillCommand(0, Source, "late-target-right", [], [3], game.Revision, Prompt()!.PromptId));
        Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var material = View(0).Hand.Single().Id;
        var chosen = game.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.Slash &&
            a.CardId == material && a.TargetSeats.SequenceEqual([1, 2]));
        Accept(new PlayCardCommand(0, material, chosen.TargetSeats, game.Revision, Prompt()!.PromptId));
        Drive(() => Prompt()?.SkillPrompt?.SkillId == Source && Prompt()!.Choices.Any(c =>
            c.Parameters.GetValueOrDefault("program-action") == "original-target-addition" && c.Targets.SequenceEqual([3])));
        Cold();
        var add = Prompt()!;
        Accept(new AnswerPromptCommand(0, add.PromptId, add.Choices.Single(c => c.Targets.SequenceEqual([3])).Id, game.Revision));
        Drive(() => Prompt()?.SkillPrompt?.SkillId == Source && Prompt()!.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue"));
        var use = game.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardId == material);
        Need(use.TargetSeats.SequenceEqual([1, 2, 3]) && use.EffectEpisodes is { Episodes: [var episode] } &&
            episode.Issue.Spec.Scope == NativeCardEffectEpisodeScope.WholeUse &&
            Events<FangtianHalberdUsedEvent>().Single(e => e.ResolutionId == use.Id).TargetSeats.SequenceEqual([1, 2]) &&
            Events<OriginalTargetAdditionGrantedEvent>().Count(e => e.Grant.TargetSeat == 3) == 1 &&
            Events<OriginalTargetAdditionResolvedEvent>().Count(e => e.CardUseFrameId == use.Id && e.TargetSeat == 3 && e.Added) == 1,
            "A real JSON right and native finalized-target choice append slot two after the original two-slot Fangtian declaration, retaining one whole-use episode.");
        Cold();
        var held = Prompt()!;
        Accept(new AnswerPromptCommand(0, held.PromptId, held.Choices.Single(c => c.Parameters.GetValueOrDefault("option-id") == "continue").Id, game.Revision));
        Drive(() => Prompt() is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        var started = Events<NativeCardEffectEpisodeStartedEvent>().Where(e => e.UseFrameId == use.Id).ToArray();
        Need(game.State.Winner == Winner.None && !View(1).IsAlive && !View(2).IsAlive && View(3).Hp == 10 &&
            Events<DamageAppliedEvent>().Where(e => e.SourceSeat == 0).Select(e => (e.TargetSeat, e.Amount)).SequenceEqual([(1, 1), (2, 1), (3, 1), (3, 1)]) &&
            started is [var round] && round.Cursor.TargetSeats.SequenceEqual([1, 2, 3]) &&
            Events<NativeCardEffectEpisodeCompletedEvent>().Count(e => e.UseFrameId == use.Id && e.Executed && e.Episode.Completed == 1) == 1 &&
            Events<FangtianHalberdUsedEvent>().Count(e => e.ResolutionId == use.Id) == 1 &&
            Events<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == material) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Hand(0) && m.To == CardLocation.Processing) == 1 &&
            game.CardMovements.Count(m => m.CardId == material && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile) == 1 &&
            !game.ResolutionStack.OfType<CardUseFrame>().Any(f => f.Id == use.Id),
            "After both original Fangtian targets die, the extra pass starts at the surviving late slot once and cannot rewind into that same slot through the retired weapon prefix; one original use and material finish once.");
        Cold();

        PendingDecision? Prompt() => game.CreateSnapshot(0).PendingDecision;
        PlayerSnapshot View(int seat) => game.CreateSnapshot(0, true).Players[seat];
        T[] Events<T>() where T : IGameEvent => game.Events.Select(e => e.Payload).OfType<T>().ToArray();
        void Accept(GameCommand command)
        { var result = game.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Need(result.Accepted, result.Error?.Message ?? "Rejected late-prefix Fangtian command."); }
        void Drive(Func<bool> ready)
        {
            for (var i = 0; i < 180; i++)
            {
                if (ready()) return;
                Need(Prompt() is not { PlayerSeat: 0 }, "Unexpected late-prefix Fangtian prompt.");
                Accept(new AdvanceOneStepCommand(game.Revision));
            }
            throw new InvalidOperationException("The bounded late-prefix Fangtian cursor did not return.");
        }
        string State() => JsonSerializer.Serialize(new { game.State, Frames = game.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), game.CardMovements });
        void Cold()
        {
            var before = State(); game = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(game.CreateCheckpoint())), registry);
            Need(before == State(), "Cold replay retains the issued late target, original Fangtian prefix, native episode and one physical payment.");
        }
        void Need(bool value, string message)
        { if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new { Prompt = Prompt(), Frames = game.ResolutionStack, Facts = game.Events.TakeLast(8).Select(e => e.Payload.ToString()) })); }
    }

    private sealed class Fixture(bool latePrefix = false) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-native-episode-fangtian", new(1, 0, 0));
        public void Register(IContentRegistryBuilder b)
        {
            var rules = $$$"""
                {"skills":[
                  {"id":"{{{Quiet}}}","revision":1,"triggers":[{"id":"fixed-opening","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]},
                  {"id":"{{{Source}}}","revision":1,"triggers":[{"id":"original-use-episodes","window":"cardUseCommitted","ownerRelation":"actor","cardKinds":["slash"],"optional":false,
                    "effects":[{"op":"issueCurrentCardEffectEpisode","target":"owner","priority":0,"scope":"wholeUse"},
                      {"op":"issueCurrentCardEffectEpisode","target":"owner","priority":20,"scope":"executionOccurrence","targetIndex":0},
                      {"op":"chooseOption","target":"owner","resultBind":"issued","options":[{"id":"continue"}]}]}]}]}
                """;
            if (latePrefix)
            {
                var root = JsonNode.Parse(rules)!;
                var source = root["skills"]!.AsArray().Single(skill => skill!["id"]!.GetValue<string>() == Source)!;
                source["activations"] = JsonNode.Parse("""
                    [{"id":"late-target-right","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"otherLiving","usesPerTurn":1,
                      "effects":[{"op":"grantTurnOriginalTargetAddition","target":"selectedTarget"}]}]
                    """);
                var triggers = source["triggers"]!.AsArray();
                triggers[0]!["effects"] = new JsonArray(triggers[0]!["effects"]![0]!.DeepClone());
                triggers.Add(JsonNode.Parse("""
                    {"id":"append-real-late-target","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["slash"],"optional":false,
                     "effects":[{"op":"offerOriginalTargetAddition","target":"owner"},
                       {"op":"chooseOption","target":"owner","resultBind":"late-added","options":[{"id":"continue"}]}]}
                    """));
                rules = root.ToJsonString();
            }
            var catalog = SkillProgramCatalog.LoadCurrent(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new[] { Source, Quiet }.ToDictionary(id => id, id => new { name = id, description = "Native Fangtian episode scope",
                        optionLabels = id == Source ? new Dictionary<string, string> { ["continue"] = "Continue" } : [] }) }));
            foreach (var (id, program) in catalog.Programs) b.AddSkill(new(id, id, id) { Program = program, ProgramPresentation = catalog.Presentations[id] });
            b.AddGeneral(new(Hero, "Fangtian episode owner", "", Source, "wei", 12, [Quiet]));
            b.AddGeneral(new(Peer, "Fangtian episode peer", "", Quiet, "shu", 12));
            var card = CardCatalog.Get(CardKind.FangtianHalberd); b.AddCard(new("fixture:episode-fangtian-weapon", card.DisplayName, card.CategoryName, card.Description, CardKind.FangtianHalberd));
            b.AddDeck(new("fixture:episode-fangtian-deck", "Fixed Fangtian episode cards", 0, 0, [])
            { PhysicalCards = Enumerable.Range(0, 24).Select(_ => new ContentDeckPhysicalCard("standard:peach", Suit.Heart, 3)).ToArray() });
            b.AddMode(new(ShanheBattleSetup.BattleModeId, "Native Fangtian episode fixture", 4, 4, new Dictionary<string, int>(), "fixture:episode-fangtian-deck",
                GeneralPoolIds: [Hero, Peer], ModeKind: ContentModeKind.Team,
                TeamCounts: new Dictionary<string, int> { [ShanheBattleSetup.PlayerTeamId] = 2, [ShanheBattleSetup.EnemyTeamId] = 2 }));
        }
    }
}
