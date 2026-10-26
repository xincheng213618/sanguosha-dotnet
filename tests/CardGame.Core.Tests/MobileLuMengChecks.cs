using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileLuMengChecks
{
    private const string Skill = "mobile:keji", Watch = "fixture:mobile-lu-meng-watch", Driver = "fixture:mobile-lu-meng-driver";
    private const string Extra = "fixture:mobile-lu-meng-extra", Owner = "fixture:mobile-lu-meng-owner", Peer = "fixture:mobile-lu-meng-peer-";
    private const string Mode = "identity:classic-mobile-lu-meng";

    public static void BeforeEntrySkipDiscardedSlashChoiceColdAndDecline()
    {
        var s = Scene.Start();
        var discarded = s.Hand(0).First(c => c.Kind == CardKind.Slash).Id;
        s.Submit(new UseProgramSkillCommand(0, Driver, "discard", [discarded], [], s.G.Revision, s.P!.PromptId)); s.Play();
        s.Need(s.G.CardMovements.Any(m => m.CardId == discarded && m.From == CardLocation.Hand(0) &&
                m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.fixture:mobile-lu-meng-driver.DiscardSelected"),
            "The real Slash entity has been discarded as a native cost, rather than used or played.");
        s.EndPlay(); s.Reach(() => s.P?.Kind == DecisionKind.SkipDiscardPolicy);
        var frame = s.G.ResolutionStack.OfType<NativeDiscardEntryFrame>().Single();
        s.Need(s.G.State.Phase == TurnPhase.Play && frame.Stage == NativeDiscardEntryStage.Choosing &&
            frame.Source.SkillId == Skill && frame.BeforePhase == TurnPhase.Play &&
            s.Events<ActualDiscardRecoveryPhaseStartedEvent>().Any(e => e.Phase.ActorSeat == 0 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play) &&
            !s.DiscardEntered() && !s.Watching("discard-start"),
            "Discarding Slash leaves Keji legal at the exact original Play boundary, before any real Discard token or starting observer.");
        s.Cold(); s.Policy(true); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        s.Need(!s.DiscardEntered() && !s.Events<ProgramSkillStartedEvent>().Any(e => e.SkillId == Watch && e.ActivationId == "discard-start") &&
            s.Events<NativeDiscardEntryPolicyChosenEvent>().Count(e => e.FrameId == frame.Id && e.Applied) == 1 &&
            s.Events<ProgramCardPolicyResolvedEvent>().Count(e => e.SkillId == Skill && e.Applied) == 1 &&
            s.Hand(0).Count == 7 && !s.G.ResolutionStack.OfType<NativeDiscardEntryFrame>().Any(),
            "Cold accepts the original choice once and really skips Discard without starting its phase, observer or hand-limit payment.");

        var decline = Scene.Start(); decline.EndPlay(); decline.Reach(() => decline.P?.Kind == DecisionKind.SkipDiscardPolicy);
        decline.Policy(false); decline.Reach(() => decline.Watching("discard-start"));
        decline.Need(decline.DiscardEntered() && decline.G.State.Phase == TurnPhase.Discard &&
            decline.Events<NativeDiscardEntryPolicyChosenEvent>().Count(e => !e.Applied) == 1,
            "Declining the before-entry choice enters the real Discard phase and only then runs its native starting observer.");
        decline.Continue(); decline.Reach(() => decline.P?.Kind == DecisionKind.DiscardCards);
        decline.Need(decline.P!.RequiredCardCount == 3 &&
            decline.Events<ProgramCardPolicyResolvedEvent>().Count(e => e.SkillId == Skill) == 1,
            "Decline reaches genuine hand-limit discard without asking the mobile policy again through the old classic window.");

        var classic = Scene.Start(classic: true); classic.EndPlay(); classic.Reach(() => classic.Watching("discard-start"));
        classic.Need(classic.DiscardEntered() && !classic.G.ResolutionStack.OfType<NativeDiscardEntryFrame>().Any(),
            "The old nullable classic policy still enters Discard and starts its observer before offering the optional skip.");
        classic.Continue(); classic.Reach(() => classic.P?.Kind == DecisionKind.SkipDiscardPolicy);
        classic.Need(classic.G.State.Phase == TurnPhase.Discard && !classic.Events<NativeDiscardEntryPolicyChosenEvent>().Any(),
            "The unchanged classic prompt retains its old native Discard-phase contract.");
        classic.Policy(true); classic.Reach(() => classic.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
    }

    public static void UsedSlashInExtraPlayAndPlayedDuelSlashDisableSkip()
    {
        var used = Scene.Start(extra: true);
        var first = used.Events<ActualDiscardRecoveryPhaseStartedEvent>()
            .Last(e => e.Phase.ActorSeat == 0 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play).Phase.Token;
        used.EndPlay(); used.Play();
        used.Need(used.Events<ActualDiscardRecoveryPhaseStartedEvent>()
                .Last(e => e.Phase.ActorSeat == 0 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play).Phase.Token != first &&
            used.G.State.TurnNumber == 1 && !used.Events<NativeDiscardEntryPolicyChosenEvent>().Any(),
            "The legal native inserted Play happens before any Keji offer and remains within the same actual turn.");
        var slash = used.Hand(0).First(c => c.Kind == CardKind.Slash).Id;
        used.Submit(new PlayCardCommand(0, slash, [1], used.G.Revision, used.P!.PromptId)); used.Play();
        used.Need(used.G.CardMovements.Any(m => m.CardId == slash && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Use),
            "The new actual Play really uses its physical Slash.");
        used.EndPlay(); used.Reach(() => used.Watching("discard-start"));
        used.Need(used.DiscardEntered() && !used.Events<NativeDiscardEntryPolicyChosenEvent>().Any() &&
            !used.G.ResolutionStack.OfType<NativeDiscardEntryFrame>().Any(),
            "A genuine Slash in the later actual Play invalidates Keji; it is not a promise frozen at an earlier Play ending.");
        used.Continue(); used.Reach(() => used.P?.Kind == DecisionKind.DiscardCards);

        var played = Scene.Start();
        var duel = played.Hand(0).First(c => c.Kind == CardKind.Duel).Id;
        played.Submit(new PlayCardCommand(0, duel, [1], played.G.Revision, played.P!.PromptId));
        played.Reach(() => played.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 1 });
        var opposing = played.Hand(1).First(c => c.Kind == CardKind.Slash).Id;
        played.Answer(c => c.Cards.SequenceEqual(new[] { opposing }));
        played.Reach(() => played.P is { Kind: DecisionKind.RespondSlash, PlayerSeat: 0 });
        var response = played.Hand(0).First(c => c.Kind == CardKind.Slash).Id;
        played.Answer(c => c.Cards.SequenceEqual(new[] { response })); played.Play();
        played.Need(played.Events<DuelResponseEvent>().Any(e => e.ResponderSeat == 0 && e.UsedSlash && e.SlashCardId == response) &&
            played.G.CardMovements.Any(m => m.CardId == response && m.From == CardLocation.Hand(0) && m.Reason == CardMoveReasons.Respond),
            "Lu Meng really plays the selected physical Slash in his own Play's native Duel exchange.");
        played.EndPlay(); played.Reach(() => played.Watching("discard-start"));
        played.Need(played.DiscardEntered() && !played.G.ResolutionStack.OfType<NativeDiscardEntryFrame>().Any() &&
            !played.Events<NativeDiscardEntryPolicyChosenEvent>().Any(),
            "A real played Slash also disables Keji, even though the card initially used was Duel.");
        played.Continue(); played.Reach(() => played.P?.Kind == DecisionKind.DiscardCards);
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game)
    {
        public GameEngine G { get; private set; } = game;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(s => G.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
        public IReadOnlyList<CardSnapshot> Hand(int seat) => G.CreateSnapshot(seat).Players[seat].Hand;
        public T[] Events<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public bool Watching(string trigger) => P?.SkillPrompt?.SkillId == Watch &&
            G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.TriggerId == trigger;
        public bool DiscardEntered() => Events<PhaseChangedEvent>().Any(e => e.ActorSeat == 0 && e.Phase == TurnPhase.Discard) ||
            Events<ActualDiscardRecoveryPhaseStartedEvent>().Any(e => e.Phase.ActorSeat == 0 && e.Phase.Kind == ActualDiscardRecoveryPhaseKind.Discard);
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Mobile Lu Meng command rejected.");
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var pending = P!; var choice = pending.Choices.FirstOrDefault(predicate);
            Need(choice is not null, "The real native prompt lacks the requested choice.");
            Submit(new AnswerPromptCommand(pending.PlayerSeat, pending.PromptId, choice!.Id, G.Revision));
        }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Policy(bool apply) => Answer(c => c.Parameters.GetValueOrDefault("action") == (apply ? "discard-policy-use" : "discard-policy-skip"));
        public void EndPlay() => Submit(new EndPlayPhaseCommand(0, G.Revision, P!.PromptId));
        public void Play() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 160; step++)
            {
                if (done()) return;
                var pending = P;
                if (pending is { Kind: DecisionKind.SelectGeneral })
                    Submit(new SelectGeneralCommand(pending.PlayerSeat, pending.PlayerSeat == 0 ? Owner : Peer + pending.PlayerSeat,
                        G.Revision, pending.PromptId));
                else if (pending is { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash })
                    Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage");
                else if (pending?.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue") == true) Continue();
                else if (pending?.Choices.Any(c => c.Targets.SequenceEqual(new[] { 0 })) == true) Answer(c => c.Targets.SequenceEqual(new[] { 0 }));
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The actual Lu Meng flow missed its named boundary.");
        }
        private string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(G.CreateSnapshot(s))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold retains every viewer, the owning pre-entry choice/source/phase, and real movement history without creating a Discard token.");
        }
        public void Need(bool ok, string reason)
        {
            if (!ok) throw new InvalidOperationException(reason + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.Phase, G.State.CurrentSeat,
                Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId,
                    Choices = p.Choices.Select(c => new { c.Id, c.Cards, c.Targets, c.Parameters }).ToArray() } : null,
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
                Phases = Events<ActualDiscardRecoveryPhaseStartedEvent>(), Choices = Events<NativeDiscardEntryPolicyChosenEvent>()
            }));
        }
        public static Scene Start(bool classic = false, bool extra = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(classic, extra));
            var scene = new Scene(registry, GameEngine.CreateStandard(new GameOptions
            {
                Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 5
            }, registry));
            scene.Submit(new StartGameCommand()); scene.Play();
            scene.Need(scene.Hand(0).Any(c => c.Kind == CardKind.Duel) && scene.Hand(0).Any(c => c.Kind == CardKind.Slash) &&
                scene.Hand(1).Any(c => c.Kind == CardKind.Slash),
                "The fixed seed's actual legally dealt hands must supply owner Duel/Slash and the peer's real Duel response.");
            return scene;
        }
    }

    private sealed class Fixture(bool classic, bool extra) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ordinary-mobile-lu-meng", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:lu-meng", "classic:lu-meng");
            var definitions = new List<JsonNode>
            {
                JsonNode.Parse("""{"id":"fixture:mobile-lu-meng-watch","revision":1,"triggers":[{"id":"track-native-phases","window":"nativeDrawStarting","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"native","options":[{"id":"continue"}]}]},{"id":"discard-start","window":"discardPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"started","options":[{"id":"continue"}]}]}]}""")!,
                JsonNode.Parse("""{"id":"fixture:mobile-lu-meng-driver","revision":1,"activations":[{"id":"discard","minCards":1,"maxCards":1,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardSelected","target":"owner","amount":1}]}]}""")!
            };
            if (extra) definitions.Add(JsonNode.Parse("""{"id":"fixture:mobile-lu-meng-extra","revision":1,"triggers":[{"id":"extra-play","window":"playEnding","subject":"owner","optional":false,"usageScope":"game","usageLimit":1,"effects":[{"op":"selectTarget","target":"owner","targetKind":"currentTurnPlayer"},{"op":"insertPhase","target":"selectedTarget","phase":"play","phaseContinuation":"beforeNormalPreparation"}]}]}""")!);
            var rules = new JsonObject { ["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion,
                ["skills"] = new JsonArray(definitions.ToArray()) }.ToJsonString();
            var labels = definitions.ToDictionary(d => d["id"]!.GetValue<string>(), d => (object)new
                { name = d["id"]!.GetValue<string>(), description = "Native fixture" });
            labels[Watch] = new { name = "Native phases", description = "Native phases", optionLabels = new { @continue = "Continue" } };
            var catalog = SkillProgramCatalog.Load(rules, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id) { Program = program });
            builder.AddSkill(new("fixture:mobile-lu-meng-none", "none", "none"));
            builder.AddGeneral(new(Owner, "owner", "supporter", classic ? "classic:keji" : Skill, "wu", 4,
                extra ? [Watch, Driver, Extra] : [Watch, Driver]));
            var peers = Enumerable.Range(1, 3).Select(i => Peer + i).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "other", "supporter", "fixture:mobile-lu-meng-none", "wei", 8, []));
            builder.AddDeck(new("fixture:mobile-lu-meng-deck", "fixed", 8, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard(
                    i % 4 == 0 ? "standard:duel" : "standard:slash", Suit.Spade, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "ordinary mobile Lu Meng", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:mobile-lu-meng-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
