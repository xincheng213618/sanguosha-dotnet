using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class MobileDaQiaoChecks
{
    private const string Skill = "mobile:liuli", Driver = "fixture:mobile-da-qiao-driver", Watch = "fixture:mobile-da-qiao-watch";
    private const string Distance = "fixture:mobile-da-qiao-source-distance", Owner = "fixture:mobile-da-qiao-owner";
    private const string Peer = "fixture:mobile-da-qiao-peer-", Mode = "identity:classic-mobile-da-qiao";
    private const string CostReason = "skill-program.mobile:liuli.SelectAndMoveOwnedCard";

    public static void EquipmentCostTargetPairsAndNativePaidCold()
    {
        foreach (var equipment in new[] { CardKind.QinggangSword, CardKind.OffensiveHorse })
        foreach (var handCost in new[] { false, true })
        {
            var s = Scene.Start(equipment, handCost);
            var equipped = s.EquipmentId;
            s.Incoming(); s.Activate(); s.Targets();
            var distant = s.P!.Choices.Any(c => c.Targets.SequenceEqual(new[] { 2 }));
            s.Need(distant == handCost && s.P.Choices.Any(c => c.Targets.SequenceEqual(new[] { 3 })) &&
                s.P.Choices.All(c => !c.Targets.Contains(1)),
                "A distant target requires at least one genuinely payable card that preserves the post-cost range; the original Slash user is excluded.");
            var target = handCost ? 2 : 3; var originalHp = s.V(0).Hp; var targetHp = s.V(target).Hp;
            s.Answer(c => c.Targets.SequenceEqual(new[] { target })); s.Costs();
            var cost = handCost ? s.V(0).Hand.Single().Id : equipped;
            s.Need(s.P!.Choices.Any(c => c.Cards.SequenceEqual(new[] { cost })) &&
                (!handCost || s.P.Choices.All(c => !c.Cards.Contains(equipped))),
                "The real native HE prompt filters out the equipped sword or offensive horse when discarding it would invalidate this selected target.");
            if (handCost)
            {
                var before = s.Dump();
                var rejected = s.G.Submit(new AnswerPromptCommand(0, s.P.PromptId,
                    new ChoiceId($"program-owned-card.frame-{s.Root.Id}.zone-Equipment.slot-0"), s.G.Revision));
                s.Need(!rejected.Accepted && before == s.Dump(),
                    "Submitting the unpublished equipment/target pair rejects before native payment or original Slash mutation.");
            }
            var parent = s.Root.Id; var use = s.IncomingUseId;
            s.Answer(c => c.Cards.SequenceEqual(new[] { cost })); s.Reach(() => s.Watching(handCost ? "paid-hand" : "paid-equipment"));
            s.Need(s.Root.Id == parent && s.Root.SelectedTargetSeats.SequenceEqual(new[] { target }) &&
                s.Root.PendingMovementContinuation is not null && s.Zone(cost) == CardLocation.DiscardPile &&
                s.V(0).Hp == originalHp && s.V(target).Hp == targetHp,
                "The exact original target and paid entity remain on the native Liuli frame while its movement child is open, before the Slash redirects.");
            if (!handCost && equipment == CardKind.QinggangSword) s.Cold();
            s.Continue(); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
            s.Need(s.V(0).Hp == originalHp && s.V(target).Hp == targetHp - 1 &&
                s.G.CardMovements.Count(m => m.CardId == cost && m.Reason.Value == CostReason &&
                    m.From == (handCost ? CardLocation.Hand(0) : CardLocation.Equipment(0)) && m.To == CardLocation.DiscardPile) == 1 &&
                s.Events<ProgramBindingResolvedEvent>().Count(e => e.FrameId == parent && e.Activated && e.Completed) == 1 &&
                s.Events<CardUseFinishedEvent>().Count(e => e.ResolutionId == use) == 1 &&
                (!handCost || s.Zone(equipped) == CardLocation.Equipment(0)),
                $"Native movement returns once to the same attack: its original user damages only the legal new target, retaining equipment when the cost was in hand. equipment={equipment}, handCost={handCost}, target={target}, parent={parent}, use={use}, originalHp={originalHp}, targetHp={targetHp}");
        }
    }

    public static void OriginalSlashDistanceExemptionDoesNotExpandOwnerRange()
    {
        var s = Scene.Start(null, handCost: true, sourceDistance: true);
        s.Need(s.G.GetCombatDistance(0, 2) == 2 && s.G.GetAttackRange(0) == 1 &&
            s.G.GetLegalActions(1).Any(a => a.CardId == s.SlashId && (a.TargetSeat == 3 || a.TargetSeats.Contains(3))),
            "The actual source can legally use this physical Slash at distance two, while Da Qiao still has her own ordinary range one.");
        s.Incoming(); s.Activate(); s.Targets();
        s.Need(s.P!.Choices.All(c => !c.Targets.Contains(2)) && s.P.Choices.Any(c => c.Targets.SequenceEqual(new[] { 3 })),
            "The source's actual card-distance exemption cannot widen mobile Liuli beyond Da Qiao's post-cost attack range.");
        var cost = s.V(0).Hand.Single().Id; var hp = s.V(3).Hp;
        s.Answer(c => c.Targets.SequenceEqual(new[] { 3 })); s.Costs();
        s.Answer(c => c.Cards.SequenceEqual(new[] { cost })); s.Reach(() => s.Watching("paid-hand"));
        s.Continue(); s.Reach(() => s.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
        s.Need(s.V(3).Hp == hp - 1 && s.Events<DamageAppliedEvent>().Any(e => e.SourceSeat == 1 && e.TargetSeat == 3),
            "The legal adjacent native redirection still retains the genuine original Slash source and damage.");

        var classic = Scene.Start(null, handCost: true, sourceDistance: true, classic: true);
        classic.Incoming(); classic.Activate(); classic.Targets();
        classic.Need(classic.P!.Choices.Any(c => c.Targets.SequenceEqual(new[] { 2 })),
            "Omitting the optional strategy retains classic Liuli's existing source-distance-exemption behavior.");
    }

    private sealed class Scene(ContentRegistry registry, GameEngine game, bool classic)
    {
        public GameEngine G { get; private set; } = game;
        public int EquipmentId { get; private set; }
        public int SlashId { get; private set; }
        public long IncomingUseId { get; private set; }
        private string ActiveSkill => classic ? "classic:liuli" : Skill;
        public PendingDecision? P => Enumerable.Range(0, 4).Select(s => G.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
        public PlayerSnapshot V(int seat) => G.CreateSnapshot(seat).Players[seat];
        public ProgramSkillFrame Root => G.ResolutionStack.OfType<ProgramSkillFrame>().Single(f => f.SkillId == ActiveSkill);
        public T[] Events<T>() where T : IGameEvent => G.Events.Select(e => e.Payload).OfType<T>().ToArray();
        public CardLocation Zone(int id) => G.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
        public bool Watching(string trigger) => P?.SkillPrompt?.SkillId == Watch &&
            G.ResolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()?.TriggerId == trigger;
        public void Submit(GameCommand command)
        {
            var result = G.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single());
            Need(result.Accepted, result.Error?.Message ?? "Mobile Da Qiao command rejected.");
        }
        public void Answer(Func<PromptChoice, bool> predicate)
        {
            var p = P!; var choice = p.Choices.FirstOrDefault(predicate); Need(choice is not null, "The native prompt lacks the requested choice.");
            Submit(new AnswerPromptCommand(p.PlayerSeat, p.PromptId, choice!.Id, G.Revision));
        }
        public void Continue() => Answer(c => c.Parameters.GetValueOrDefault("option-id") == "continue");
        public void Activate() => Answer(c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        public void Targets() => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-target") == true);
        public void Costs() => Reach(() => P?.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "select-and-move-owned-card") == true);
        private void Play0() => Reach(() => P is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
        public void Incoming()
        {
            Submit(new PlayCardCommand(1, SlashId, [0], G.Revision, P!.PromptId));
            Reach(() => P?.SkillPrompt?.SkillId == ActiveSkill && P.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == "activate"));
            var use = G.ResolutionStack.OfType<CardUseFrame>().Single(f => f.CardAttack?.TargetSeat == 0);
            IncomingUseId = use.Id;
            Need(use.SourceSeat == 1 && use.PhysicalCardIds.Contains(SlashId), "The incoming original attack is the other human's real physical Slash.");
        }
        public void Reach(Func<bool> done)
        {
            for (var step = 0; step < 180; step++)
            {
                if (done()) return;
                var p = P;
                if (p is { Kind: DecisionKind.SelectGeneral })
                    Submit(new SelectGeneralCommand(p.PlayerSeat, p.PlayerSeat == 0 ? Owner : Peer + p.PlayerSeat, G.Revision, p.PromptId));
                else if (p is { Kind: DecisionKind.DiscardCards })
                    Submit(new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, G.Revision));
                else if (p is { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash })
                    Answer(c => c.Parameters.GetValueOrDefault("response") == "take-damage");
                else Submit(new AdvanceOneStepCommand(G.Revision));
            }
            Need(false, "The bounded actual Da Qiao flow missed its named boundary.");
        }
        public string Dump() => JsonSerializer.Serialize(new
        {
            Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(G.CreateSnapshot(s))).ToArray(),
            Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(), G.CardMovements,
            Events = G.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).ToArray(), Commands = CommandJson.Serialize(G.AcceptedCommands)
        });
        public void Cold()
        {
            var before = Dump(); G = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(G.CreateCheckpoint())), registry);
            Need(before == Dump(), "Cold preserves four views, the original paid HE movement child, selected target and exact native Slash parent.");
        }
        public void Need(bool value, string message)
        {
            if (!value) throw new InvalidOperationException(message + " " + JsonSerializer.Serialize(new
            {
                G.State.TurnNumber, G.State.CurrentSeat, G.State.Phase,
                Hp = Enumerable.Range(0, 4).Select(seat => new { Seat = seat, V(seat).Hp, V(seat).IsAlive }).ToArray(),
                Damage = Events<DamageAppliedEvent>(), Resolved = Events<ProgramBindingResolvedEvent>(), Finished = Events<CardUseFinishedEvent>(),
                Pending = P is { } p ? new { p.Kind, p.PlayerSeat, Skill = p.SkillPrompt?.SkillId,
                    Choices = p.Choices.Select(c => new { c.Id, c.Cards, c.Targets, c.Parameters }).ToArray() } : null,
                Frames = G.ResolutionStack.Select(f => JsonSerializer.Serialize(f, f.GetType())).ToArray(),
                Movements = G.CardMovements.TakeLast(6).ToArray()
            }));
        }
        public static Scene Start(CardKind? equipment, bool handCost, bool sourceDistance = false, bool classic = false)
        {
            var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(sourceDistance));
            var scene = new Scene(registry, GameEngine.CreateStandard(new GameOptions
            {
                Seed = 17, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
                ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true,
                AdvanceAfterHumanCommands = false, MaxTurns = 5
            }, registry), classic);
            scene.Submit(new StartGameCommand()); scene.Play0();
            if (classic)
            {
                scene.Submit(new UseProgramSkillCommand(0, Driver, "classic-source", [], [], scene.G.Revision, scene.P!.PromptId)); scene.Play0();
            }
            if (equipment is { } kind)
            {
                scene.Need(scene.V(0).Hand.Any(c => c.Kind == kind), "The fixed physical deck must actually deal the needed equipment.");
                scene.EquipmentId = scene.V(0).Hand.First(c => c.Kind == kind).Id;
                scene.Submit(new PlayCardCommand(0, scene.EquipmentId, [], scene.G.Revision, scene.P!.PromptId)); scene.Play0();
            }
            scene.Submit(new UseProgramSkillCommand(0, Driver, "empty-hand", [], [], scene.G.Revision, scene.P!.PromptId)); scene.Play0();
            if (handCost)
            {
                scene.Submit(new UseProgramSkillCommand(0, Driver, "draw-one", [], [], scene.G.Revision, scene.P!.PromptId)); scene.Play0();
                scene.Need(scene.V(0).Hand.Count == 1, "The native draw leaves one actual hand cost.");
            }
            scene.Submit(new EndPlayPhaseCommand(0, scene.G.Revision, scene.P!.PromptId));
            scene.Reach(() => scene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
            if (sourceDistance)
            {
                scene.Submit(new UseProgramSkillCommand(1, Distance, "grant", [], [], scene.G.Revision, scene.P!.PromptId));
                scene.Reach(() => scene.P is { Kind: DecisionKind.PlayCard, PlayerSeat: 1 });
            }
            scene.Need(scene.V(1).Hand.Any(c => c.Kind == CardKind.Slash), "The fixed deck must actually supply the other human's Slash.");
            scene.SlashId = scene.V(1).Hand.First(c => c.Kind == CardKind.Slash).Id;
            return scene;
        }
    }

    private sealed class Fixture(bool sourceDistance) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:ordinary-mobile-da-qiao", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            CharacterFixtureContent.Register(builder, "mobile:da-qiao", "classic:da-qiao");
            var definitions = new[]
            {
                JsonNode.Parse("""{"id":"fixture:mobile-da-qiao-driver","revision":1,"activations":[{"id":"empty-hand","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"discardOwnedZoneCards","target":"owner","zones":["hand"]}]},{"id":"draw-one","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":1}]},{"id":"classic-source","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["mobile:liuli"],"sourceBind":"classic:liuli"}]}]}""")!,
                JsonNode.Parse("""{"id":"fixture:mobile-da-qiao-watch","revision":1,"triggers":[{"id":"paid-hand","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:liuli.SelectAndMoveOwnedCard"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"hand-paid","options":[{"id":"continue"}]}]},{"id":"paid-equipment","window":"cardsMoved","subject":"owner","sourceZones":["equipment"],"movementOccurrence":"perBatch","movementReasons":["skill-program.mobile:liuli.SelectAndMoveOwnedCard"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"equipment-paid","options":[{"id":"continue"}]}]}]}""")!,
                JsonNode.Parse("""{"id":"fixture:mobile-da-qiao-source-distance","revision":1,"activations":[{"id":"grant","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"grantTurnRuleModifier","target":"owner","ruleQuery":"cardUseDistanceLimit","ruleOperation":"unlimited","cardKinds":["slash","fireSlash","thunderSlash"]}]}]}""")!
            };
            var labels = definitions.ToDictionary(d => d["id"]!.GetValue<string>(), d => (object)new { name = d["id"]!.GetValue<string>(), description = "Native fixture" });
            labels[Watch] = new { name = "Native paid discard", description = "Native paid discard", optionLabels = new { @continue = "Continue" } };
            var catalog = SkillProgramCatalog.Load(new JsonObject { ["schemaVersion"] = SkillProgramCatalog.RulesSchemaVersion,
                ["skills"] = new JsonArray(definitions) }.ToJsonString(), JsonSerializer.Serialize(new { schemaVersion = 3, skills = labels }));
            foreach (var (id, program) in catalog.Programs) builder.AddSkill(new(id, id, id) { Program = program });
            builder.AddSkill(new("fixture:mobile-da-qiao-none", "none", "none"));
            builder.AddGeneral(new(Owner, "owner", "supporter", Skill, "wu", 3, [Driver, Watch], GeneralGender.Female));
            var peers = Enumerable.Range(1, 3).Select(i => Peer + i).ToArray();
            for (var i = 1; i < 4; i++) builder.AddGeneral(new(Peer + i, "other", "supporter", "fixture:mobile-da-qiao-none", "wei", 8,
                sourceDistance && i == 1 ? [Distance] : []));
            builder.AddDeck(new("fixture:mobile-da-qiao-deck", "fixed", 12, 0, [])
            {
                PhysicalCards = Enumerable.Range(0, 96).Select(i => new ContentDeckPhysicalCard(
                    (i % 3) switch { 0 => "standard:slash", 1 => "standard:qinggang_sword", _ => "standard:offensive_horse" }, Suit.Spade, 7)).ToArray()
            });
            builder.AddMode(new(Mode, "ordinary mobile Da Qiao", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Loyalist)] = 1, [nameof(Role.Rebel)] = 2 },
                "fixture:mobile-da-qiao-deck", GeneralCandidateCount: 4, GeneralPoolIds: [Owner, .. peers]));
        }
    }
}
