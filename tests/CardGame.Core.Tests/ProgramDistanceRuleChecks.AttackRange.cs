using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.EngineOwners;

internal static partial class ProgramDistanceRuleChecks
{
    private const string RangeIssuer = "fixture:range-issuer", RangeDriver = "fixture:range-driver",
        RangeLord = "fixture:range-lord", RangeQuiet = "fixture:range-quiet", RangeOwner = "fixture:range-owner",
        RangeMode = "identity:classic-shared-attack-range";

    private static void AttackRangeRelationsKeepTheirDirectionSourcesAndActualBoundaries()
    {
        var s = RangeScenario.Start();
        Need(!s.Range.HasRelation(0, 2) && !s.Range.IsWithinAttackRange(0, 2) &&
                !s.Range.HasRelation(-1, 2) && !s.Range.HasRelation(0, 0) && !s.Range.IsWithinAttackRange(0, 0),
            "The real four-seat fixture starts with no special relation across the opposite seats and rejects invalid or self pairs.");
        s.Use(RangeIssuer, "one", [2]); s.Play();
        Need(s.Range.HasRelation(0, 2) && s.Range.IsWithinAttackRange(0, 2) && !s.Range.HasRelation(2, 0),
            "An issued one-way relation puts the exact target in range without adding the reverse relation.");
        s.Use(RangeIssuer, "forbid", [2]); s.Play();
        Need(s.Range.HasRelation(0, 2) && s.Range.IsWithinAttackRange(0, 2) &&
                s.G.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>()
                    .HasDirectedTurnCardPolicy(0, 2, CardKind.Slash, DirectedTurnCardPolicyEffect.ForbidTarget),
            "A genuine directed target prohibition remains a separate card-use answer and cannot cancel an attack-range relation.");
        s.Use(RangeDriver, "lose-issuer"); s.Play(); s.Cold();
        Need(!s.View(0).Skills!.Any(skill => skill.ContentId == RangeIssuer) && s.Range.HasRelation(0, 2),
            "The already applied relation survives real source loss and command replay independently of future issuing eligibility.");
        s.End(); s.Reach(() => s.AtEndGate);
        Need(s.Events<TurnEndedEvent>().Any(e => e.ActorSeat == 0) && !s.Range.HasRelation(0, 2),
            "The declared own TurnEnded boundary removes the relation in its real after-End window, without waiting for cleanup.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");

        s = RangeScenario.Start();
        s.Use(RangeIssuer, "ignore", [2]); s.Play();
        Need(s.G.GetRuntimeModule<CoreDirectedTurnCardPoliciesRuntime>()
                    .HasDirectedTurnCardPolicy(0, 2, CardKind.Slash, DirectedTurnCardPolicyEffect.IgnoreDistance) &&
                !s.Range.HasRelation(0, 2) && !s.Range.IsWithinAttackRange(0, 2),
            "A real card-use distance exemption does not change the participant relation or ordinary attack-range arithmetic.");
        s.Use(RangeIssuer, "both", [0, 2]); s.Play();
        Need(s.Range.HasRelation(0, 2) && s.Range.HasRelation(2, 0),
            "Two explicit directed grants express the mutual relation without a separate symmetric-range mechanism.");
        s.Use(RangeIssuer, "conditional", [3]); s.Play();
        Need(s.Range.HasRelation(0, 3) && s.Range.HasRelation(2, 1),
            "The applied conditional relation and the currently effective Lord source use the same shared range owner.");
        s.CheckDynamicFaction();
        s.Use(RangeDriver, "lose-lord"); s.Play();
        Need(!s.Range.HasRelation(2, 1) && s.Range.HasRelation(0, 3),
            "Losing the continuous Lord source ends its own relation while an independently applied relation remains.");
        s.Use(RangeDriver, "gain-lord"); s.Play(); s.Cold();
        Need(s.Range.HasRelation(2, 1), "A newly effective qualified Lord source supplies its live declaration again.");
        var round = s.Events<RoundStartedEvent>().Last().RoundNumber;
        s.End(); s.Reach(() => s.AtEndGate);
        Need(s.Range.HasRelation(0, 2) && s.Range.HasRelation(2, 0),
            "A RoundAdvanced relation is still present at the earlier TurnEnded boundary.");
        s.Cold(); s.Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        s.Reach(() => s.Events<RoundStartedEvent>().Any(e => e.RoundNumber > round)); s.Play();
        Need(!s.Range.HasRelation(0, 2) && !s.Range.HasRelation(2, 0) && s.Range.HasRelation(0, 3),
            "The next actual round expires both issued directions while the independent lasting conditional relation remains.");
        s.Use(RangeIssuer, "both", [1, 3]); s.Play();
        Need(s.Range.HasRelation(1, 3) && s.Range.HasRelation(3, 1),
            "A source can issue mutual rights to two other actual participants.");
        s.Cold(); s.Use(RangeDriver, "die");
        s.Reach(() => !s.View(0).IsAlive);
        Need(s.Range.HasRelation(1, 3) && s.Range.HasRelation(3, 1) && !s.Range.HasRelation(2, 1) &&
                !s.Range.HasRelation(0, 3),
            "Actual caster death preserves issued relations between surviving recipients, ends its live Lord declaration and never creates a relation to a dead participant.");
        s.Cold();
        AttackRangeOwnerForwardsActualEquipmentExclusions();
    }

    private static void AttackRangeOwnerForwardsActualEquipmentExclusions()
    {
        var s = RangeScenario.Start(); s.Use(RangeDriver, "stock-equipment"); s.Play();
        s.Equip(CardKind.FangtianHalberd); s.Equip(CardKind.OffensiveHorse);
        var weapon = s.View(0).Equipment.Single(c => c.Kind == CardKind.FangtianHalberd).Id;
        var horse = s.View(0).Equipment.Single(c => c.Kind == CardKind.OffensiveHorse).Id;
        Need(!s.Range.HasRelation(0, 2) && s.Range.IsWithinAttackRange(0, 2) &&
                s.Range.IsWithinAttackRange(0, 2, excludedEquipmentId: weapon) &&
                s.Range.IsWithinAttackRange(0, 2, excludedEquipmentId: horse) &&
                !s.Range.IsWithinAttackRange(0, 2, excludedEquipmentId: horse, excludedEquipmentSlot: EquipmentSlot.Weapon) &&
                !s.Range.IsWithinAttackRange(0, 2, excludedEquipmentId: weapon, excludedEquipmentSlot: EquipmentSlot.OffensiveHorse),
            "The shared range owner forwards both exact-entity and slot exclusions to real weapon range and outgoing horse distance; excluding either alone retains range, excluding both does not.");
        s.Cold();
        Need(s.View(0).Equipment.Any(c => c.Id == weapon) && s.View(0).Equipment.Any(c => c.Id == horse),
            "Hypothetical equipment exclusions do not move or consume either genuinely equipped physical entity.");
    }

    private sealed class RangeScenario(GameEngine game, ContentRegistry registry)
    {
        public GameEngine G { get; private set; } = game;
        public AttackRangeRuntime Range => G.GetRuntimeModule<AttackRangeRuntime>();
        private PendingDecision? Prompt => G.CreateSnapshot(0).PendingDecision;
        public bool AtEndGate => Prompt?.SkillPrompt?.SkillId == RangeDriver &&
            Prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public PlayerSnapshot View(int seat) => G.CreateSnapshot(0).Players[seat];
        public T[] Events<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public void Use(string skill, string id, int[]? targets = null) => Accept(new UseProgramSkillCommand(0,
            skill, id, [], targets ?? [], G.Revision, Prompt!.PromptId));
        public void Answer(Func<PromptChoice, bool> choice) => Accept(new AnswerPromptCommand(0,
            Prompt!.PromptId, Prompt.Choices.First(choice).Id, G.Revision));
        public void End() => Accept(new EndPlayPhaseCommand(0, G.Revision, Prompt!.PromptId));
        public void Play() => Reach(() => Prompt is { PlayerSeat: 0, Kind: DecisionKind.PlayCard });
        public void Equip(CardKind kind)
        {
            var id = View(0).Hand.Single(c => c.Kind == kind).Id;
            var action = G.GetHumanLegalActions().Single(a => a.Kind == LegalActionKind.Equip && a.CardId == id && a.ConversionSource is null);
            Accept(new PlayCardCommand(0, id, action.TargetSeats, G.Revision, Prompt!.PromptId, action.PlayedCardKind, action.TargetCardId)); Play();
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                Need(G.State.Winner == Winner.None, "The shared range fixture ended before its expected boundary: " + State());
                if (Prompt is not { PlayerSeat: 0 } p) Accept(new AdvanceOneStepCommand(G.Revision));
                else if (p.Kind == DecisionKind.PlayCard) End();
                else if (p.Kind == DecisionKind.RescueDying)
                    Answer(c => c.Parameters.GetValueOrDefault("response") is "pass" or "let-die");
                else if (p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "skip"))
                    Answer(c => c.Parameters.GetValueOrDefault("program-action") == "skip");
                else throw new InvalidOperationException("Unexpected shared range prompt: " + State());
            }
            throw new InvalidOperationException("Missing shared range boundary: " + State());
        }
        public void CheckDynamicFaction()
        {
            var participants = ((MatchRuntime)typeof(GameEngine).GetProperty("MatchRuntime",
                BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(G)!).Capabilities.Get<IMatchParticipantStateAccess>();
            var player = participants.At(3); var original = player.ChosenFactionId;
            try
            {
                // This is a read-only query check over changing authoritative facts,
                // restored before any later command-prefix recovery assertion.
                player.ChosenFactionId = "shu";
                Need(!Range.HasRelation(0, 3), "An applied relation rereads its recipient's current faction condition.");
                player.ChosenFactionId = "wu";
                Need(Range.HasRelation(0, 3), "The same issued relation reactivates when its continuous condition becomes true again.");
            }
            finally { player.ChosenFactionId = original; }
        }
        private void Accept(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, "Rejected shared range command " + command.GetType().Name + ": " + JsonSerializer.Serialize(result.Error));
        }
        private string State() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(i => SnapshotJson.Serialize(G.CreateSnapshot(i))),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType()))
        });
        public void Cold()
        {
            var before = State(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(State() == before, "Command-prefix recovery preserves the original range grants, source state, native boundaries and all player projections.");
        }
        public static RangeScenario Start()
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(),
                new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
                new StandardClassicGeneralPackage(), new RangeFixture());
            var game = GameEngine.CreateStandard(new GameOptions { Seed = 7, PlayerCount = 4, HumanSeat = 0,
                HumanRole = Role.Lord, ModeId = RangeMode, UseInteractiveSetup = true, UseInteractiveDiscard = false,
                AdvanceAfterHumanCommands = false, MaxTurns = 8 }, registry);
            var s = new RangeScenario(game, registry); s.Accept(new StartGameCommand());
            s.Reach(() => s.Prompt is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
            s.Accept(new SelectGeneralCommand(0, RangeOwner, game.Revision, s.Prompt!.PromptId)); s.Play(); return s;
        }
    }

    private sealed class RangeFixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture-shared-attack-range", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            JsonNode Node(string json) => JsonNode.Parse(json)!;
            JsonObject Grant(string actor, string target, string effect, JsonArray until) => new()
            {
                ["op"] = "grantDirectedTurnCardPolicy", ["target"] = "owner",
                ["actorRef"] = new JsonObject { ["kind"] = actor }, ["targetRef"] = new JsonObject { ["kind"] = target },
                ["effects"] = new JsonArray(JsonValue.Create(effect)), ["requiresEffectiveSource"] = false, ["captureOnUse"] = false, ["until"] = until
            };
            JsonObject Activation(string id, int targets, params JsonNode[] effects) => new()
            {
                ["id"] = id, ["minCards"] = 0, ["maxCards"] = 0, ["minTargets"] = targets,
                ["maxTargets"] = targets, ["targetKind"] = "anyLiving", ["usesPerTurn"] = null,
                ["effects"] = new JsonArray(effects)
            };
            JsonArray Until(string kind) => (JsonArray)Node(kind == "turnEnded"
                ? """[{"event":"turnEnded","participant":{"kind":"owner"}}]"""
                : """[{"event":"roundAdvanced"}]""");
            var conditional = Grant("owner", "selectedTarget", "inAttackRange", new JsonArray());
            conditional["while"] = Node("""["isFaction","eventTarget","wu"]""");
            var issuer = new JsonObject { ["id"] = RangeIssuer, ["revision"] = 1, ["activations"] = new JsonArray(
                Activation("one", 1, Grant("owner", "selectedTarget", "inAttackRange", Until("turnEnded"))),
                Activation("both", 2, Grant("selectedFirst", "selectedSecond", "inAttackRange", Until("roundAdvanced")),
                    Grant("selectedSecond", "selectedFirst", "inAttackRange", Until("roundAdvanced"))),
                Activation("conditional", 1, conditional),
                Activation("forbid", 1, Grant("owner", "selectedTarget", "forbidTarget", new JsonArray())),
                Activation("ignore", 1, Grant("owner", "selectedTarget", "ignoreDistance", new JsonArray()))) };
            JsonNode Lose(string skill) => Node($$"""{"op":"skillState","target":"owner","action":"lose","match":{"skillIds":["{{skill}}"]},"until":[]}""");
            var driver = new JsonObject { ["id"] = RangeDriver, ["revision"] = 1, ["activations"] = new JsonArray(
                Activation("lose-issuer", 0, Lose(RangeIssuer)), Activation("lose-lord", 0, Lose(RangeLord)),
                Activation("gain-lord", 0, Node($$"""{"op":"grantSkills","target":"owner","skillIds":["{{RangeLord}}"]}""")),
                Activation("stock-equipment", 0, Node("""{"op":"draw","target":"owner","amount":20}"""), Node("""{"op":"draw","target":"owner","amount":20}""")),
                Activation("die", 0, Node("""{"op":"loseHp","target":"owner","amount":20}"""))),
                ["triggers"] = Node("""[{"id":"end-gate","window":"afterTurnEnded","subject":"owner","turnOwnerScope":"own","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"gate","options":[{"id":"continue"}]}]}]""") };
            var lord = new JsonObject { ["id"] = RangeLord, ["revision"] = 1,
                ["attackRangeRelations"] = Node("""[{"id":"source-neighbours","ownerRole":"lord","while":["and",["not",["same","owner","eventSource"]],["isFaction","eventSource","wu"],["==",["distance","owner","eventTarget"],1]]}]""") };
            var quiet = new JsonObject { ["id"] = RangeQuiet, ["revision"] = 1,
                ["triggers"] = Node("""[{"id":"no-draw","window":"drawPhaseStarting","subject":"owner","optional":false,"effects":[{"op":"adjustNormalDraw","target":"owner","amount":-2}]}]""") };
            var rules = new JsonObject { ["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion,
                ["skills"] = new JsonArray(issuer, driver, lord, quiet) };
            var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                skills = new Dictionary<string, object>
                {
                    [RangeIssuer] = new { name = "Shared relation issuer", description = "Directed and mutual relations" },
                    [RangeDriver] = new { name = "Shared boundary driver", description = "Real source and native boundary changes", optionLabels = new { @continue = "Continue" } },
                    [RangeLord] = new { name = "Continuous Lord relation", description = "Current faction and physical distance" },
                    [RangeQuiet] = new { name = "Quiet", description = "No ordinary cards or seed searches" }
                } });
            var catalog = SkillProgramCatalog.Load(rules.ToJsonString(), presentation);
            foreach (var (id, program) in catalog.Programs)
                builder.AddSkill(new(id, id, id)
                    { Tags = id == RangeLord ? SkillTag.Lord : SkillTag.None, Program = program, ProgramPresentation = catalog.Presentations[id] });
            builder.AddGeneral(new(RangeOwner, "Shared relation owner", "", RangeIssuer, "wei", 8, [RangeDriver, RangeLord, RangeQuiet]));
            var peers = Enumerable.Range(1, 4).Select(i => "fixture:range-peer-" + i).ToArray();
            foreach (var peer in peers) builder.AddGeneral(new(peer, "Shared relation peer", "", RangeQuiet, "wu", 20));
            const string deck = "fixture:shared-range-deck";
            builder.AddDeck(new(deck, "Fixed empty opening", 0, 0, [])
                { PhysicalCards = [new("classic:fangtian-halberd", Suit.Spade, 12), new("standard:offensive_horse", Suit.Heart, 5),
                    .. Enumerable.Range(0, 32).Select(_ => new ContentDeckPhysicalCard("standard:slash", Suit.Club, 7))] });
            builder.AddMode(new(RangeMode, "Shared attack range relations", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                deck, GeneralCandidateCount: peers.Length + 1, GeneralPoolIds: [RangeOwner, .. peers]));
        }
    }
}
