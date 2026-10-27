using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Content.Standard;
using CardGame.Core;

internal static partial class SoleTargetProtectionChecks
{
    private const string Skill = "ol:zhenwei", Driver = "fixture:sole-driver", TargetPause = "fixture:sole-target-pause";
    private const string GainPause = "fixture:sole-return-pause", SourceLoss = "fixture:sole-cost-loss", Replacement = "fixture:sole-replacement";
    private const string ReturnHealth = "fixture:sole-return-health", AfterDianjun = "fixture:sole-after-dianjun";
    private const string Mode = "identity:classic-sole-protection";

    private static (GameEngine, ContentRegistry) Start(CardKind kind, bool substitute = false, bool sourceLoss = false, bool red = false, bool ownSkill = false, bool multi = false, bool chain = false, bool foreignActor = false, bool zero = false, bool liuli = false, bool fullBenxi = false, int benxiRedirectCase = 0, bool liuliLoop = false, bool dianjun = false)
    {
        var registry = ContentRegistry.Build(new StandardContentPackage(), new Fixture(kind, substitute, sourceLoss, red, ownSkill, multi, chain, foreignActor, zero, liuli, fullBenxi, benxiRedirectCase, liuliLoop, dianjun));
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, ModeId = Mode,
            UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 4 }, registry);
        Accept(g, new StartGameCommand()); Reach(g, p => p is { Kind: DecisionKind.SelectGeneral, PlayerSeat: 0 });
        Accept(g, new SelectGeneralCommand(0, "fixture:sole-owner", g.Revision, P(g)!.PromptId));
        Reach(g, zero ? p => p?.SkillPrompt?.SkillId == "fixture:sole-legacy" : IsPlay); return (g, registry);
    }
    private static int Protector(GameEngine g) => g.CreateSnapshot(0).Players.Single(p => p.GeneralId == "fixture:sole-protector").Seat;
    private static LegalAction FindAction(GameEngine g, CardKind kind, int protector) => g.GetHumanLegalActions().First(a =>
        a.Kind == Enum.Parse<LegalActionKind>(kind.ToString()) && (kind is CardKind.DrawTwo or CardKind.Lightning ||
            a.TargetSeats is [var target] && target != protector && target != 0));
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s, false).PendingDecision).FirstOrDefault(p => p is not null);
    private static bool IsPlay(PendingDecision? p) => p is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 };
    private static T[] Facts<T>(GameEngine g) => g.Events.Select(e => e.Payload).OfType<T>().ToArray();
    private static void Play(GameEngine g, LegalAction a) => Accept(g, new PlayCardCommand(0, a.CardId!.Value, a.TargetSeats, g.Revision, P(g)!.PromptId,
        a.PlayedCardKind, a.TargetCardId) { ConversionSource = a.ConversionSource, AdditionalConversionSources = a.AdditionalConversionSources });
    private static void End(GameEngine g) => Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
    private static void Answer(GameEngine g, Func<PromptChoice, bool> pick) { var p = P(g)!; Require(p.PlayerSeat == 0, "Only true human choices use Answer commands."); Accept(g, new AnswerPromptCommand(0, p.PromptId, p.Choices.First(pick).Id, g.Revision)); }
    // This AI fixture exposes a real responder-owned prompt. Choose its native
    // branch explicitly so AI utility ties cannot skip the behavior under test.
    private static void AnswerPublished(GameEngine g, Func<PromptChoice, bool> pick)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(pick).Id, g.Revision)); }
    private static void Reach(GameEngine g, Func<PendingDecision?, bool> stop)
    {
        for (var i = 0; i < 200; i++)
        {
            var p = P(g); if (stop(p)) return;
            if (g.State.Winner != Winner.None)
                throw new InvalidOperationException("The sole-protection fixture ended before its expected native prompt: " +
                    JsonSerializer.Serialize(new { stack = g.ResolutionStack, events = g.Events.Where(e =>
                        e.Payload is SoleTargetReplacedEvent or SoleTargetAfterProtectionRedirectedEvent or
                            SoleTargetRedirectWindowIssuedEvent or ProgramBindingStartedEvent)
                        .Select(e => new { Type = e.Payload.GetType().Name, Value = JsonSerializer.Serialize(e.Payload, e.Payload.GetType()) }) }));
            if (p is { PlayerSeat: 0 })
            {
                if (p.Kind == DecisionKind.DiscardCards) Accept(g, new DiscardCardsCommand(0, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, g.Revision));
                else if (IsPlay(p)) End(g);
                else
                {
                    var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ??
                        p.Choices.FirstOrDefault(c => c.Cards.Count == 0 && c.Targets.Count == 0) ?? p.Choices[0];
                    Accept(g, new AnswerPromptCommand(0, p.PromptId, choice.Id, g.Revision));
                }
            }
            else Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Sole protection exceeded bounded native prompts: " + JsonSerializer.Serialize(P(g)));
    }
    private static void Accept(GameEngine g, GameCommand command)
    { var r = g.Submit(CommandJson.Deserialize(CommandJson.Serialize([command])).Single()); Require(r.Accepted, r.Error?.Message ?? "Rejected real fixture command."); }
    private static GameEngine Cold(GameEngine g, ContentRegistry registry)
    {
        var copy = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Require(Enumerable.Range(0, 4).All(s => SnapshotJson.Serialize(g.CreateSnapshot(s, false)) == SnapshotJson.Serialize(copy.CreateSnapshot(s, false))) &&
            JsonSerializer.Serialize(g.ResolutionStack) == JsonSerializer.Serialize(copy.ResolutionStack) &&
            g.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType())).SequenceEqual(copy.Events.Select(e => JsonSerializer.Serialize(e.Payload, e.Payload.GetType()))) &&
            JsonSerializer.Serialize(g.CardMovements) == JsonSerializer.Serialize(copy.CardMovements), "Every view, owning receipt, immutable fact and real movement replays cold identically."); return copy;
    }
    private static void Frozen<T>(IReadOnlyList<T> values)
    { Require(values is IList<T> { IsReadOnly: true }, "Committed nested collections must be immutable."); if (values.Count == 0) return; try { ((IList<T>)values)[0] = values[0]; } catch (NotSupportedException) { return; } throw new InvalidOperationException("Observer changed a committed nested collection."); }
    private static void Require(bool yes, string message) { if (!yes) throw new InvalidOperationException(message); }
    private static string Resource(string bundle, string kind)
    { var asm = typeof(StandardContentPackage).Assembly; using var r = new StreamReader(asm.GetManifestResourceStream(asm.GetManifestResourceNames().Single(n => n.EndsWith(bundle + "." + kind + ".json", StringComparison.Ordinal)))!); return r.ReadToEnd(); }

    private sealed class Fixture(CardKind kind, bool substitute, bool sourceLoss, bool red, bool ownSkill, bool multi, bool chain, bool foreignActor, bool zero, bool liuli, bool fullBenxi, int benxiRedirectCase, bool liuliLoop, bool dianjun) : IGameContentPackage
    {
        private static readonly Lazy<SkillProgramCatalog> WenPin = new(() => LoadCatalog("ol-wen-pin"));
        private static readonly Lazy<SkillProgramCatalog> TrimmedBenxi = new(() => LoadBenxi(trim: true));
        private static readonly Lazy<SkillProgramCatalog> FullBenxi = new(() => LoadBenxi(trim: false));
        private static readonly Lazy<SkillProgramCatalog> Liuli = new(() => LoadCatalog("passive-response-rules"));
        private static readonly Lazy<SkillProgramCatalog> Lihuo = new(() => LoadCatalog("boundary-cheng-pu"));
        private static readonly Dictionary<(string Id, string Body, bool ContinueLabel), SkillProgramCatalog> FixturePrograms = [];

        private static SkillProgramCatalog LoadCatalog(string bundle) =>
            SkillProgramCatalog.Load(Resource(bundle, "rules"), Resource(bundle, "presentation"));

        private static SkillProgramCatalog LoadBenxi(bool trim)
        {
            var rules = Resource("classic-wu-yi", "rules"); var presentation = Resource("classic-wu-yi", "presentation");
            if (trim)
            {
                // The sole-target fixture keeps only the distance observer.
                // Complete enhancement fixtures load an independent catalog.
                var parsedRules = JsonNode.Parse(rules)!;
                var triggers = parsedRules["skills"]![0]!["triggers"]!.AsArray();
                for (var i = triggers.Count - 1; i >= 0; i--)
                    if (triggers[i]!["id"]!.GetValue<string>() != "use-shortens-turn-distance") triggers.RemoveAt(i);
                rules = parsedRules.ToJsonString();
                var parsedPresentation = JsonNode.Parse(presentation)!;
                parsedPresentation["skills"]!["classic:benxi"]!["triggerChoices"]!.AsObject().Remove("choose-current-card-enhancements");
                presentation = parsedPresentation.ToJsonString();
            }
            return SkillProgramCatalog.Load(rules, presentation);
        }

        public PackageManifest Manifest { get; } = new("fixture-sole-protection", new(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            if (dianjun) CharacterFixtureContent.Register(b, "ol:zhang-yi");
            foreach (var c in new[] { WenPin.Value, fullBenxi ? FullBenxi.Value : TrimmedBenxi.Value })
            {
                foreach (var (id, p) in c.Programs) b.AddSkill(new(id, c.Presentations[id].Name, c.Presentations[id].Description) { Program = p, ProgramPresentation = c.Presentations[id] });
            }
            if (liuli)
            {
                var catalog = Liuli.Value;
                var id = "classic:liuli";
                b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
                var fire = Lihuo.Value;
                var fireId = "boundary:lihuo";
                b.AddSkill(new(fireId, fire.Presentations[fireId].Name, fire.Presentations[fireId].Description)
                    { Program = fire.Programs[fireId], ProgramPresentation = fire.Presentations[fireId] });
            }
            if (benxiRedirectCase != 0 && !liuli)
            {
                var catalog = Liuli.Value;
                var id = "classic:liuli";
                b.AddSkill(new(id, catalog.Presentations[id].Name, catalog.Presentations[id].Description)
                    { Program = catalog.Programs[id], ProgramPresentation = catalog.Presentations[id] });
            }
            void Add(string id, string body)
            {
                lock (FixturePrograms)
                {
                    var key = (id, body, id == TargetPause || id == GainPause || id == ReturnHealth || id == AfterDianjun || id == "fixture:sole-legacy" && fullBenxi);
                    if (FixturePrograms.TryGetValue(key, out var existing))
                    {
                        b.AddSkill(new(id, id, id) { Program = existing.Programs[id], ProgramPresentation = existing.Presentations[id] });
                        return;
                    }
                    var c = SkillProgramCatalog.Load($$"""{"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"{{id}}","revision":1,{{body}}}]}""",
                        JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object>
                        { [id] = new { name = id, description = id, optionLabels = id != TargetPause && id != GainPause && id != ReturnHealth && id != AfterDianjun && (id != "fixture:sole-legacy" || !fullBenxi)
                            ? new Dictionary<string, string>()
                            : new Dictionary<string, string> { ["continue"] = "继续" } } } }));
                    b.AddSkill(new(id, id, id) { Program = c.Programs[id], ProgramPresentation = c.Presentations[id] });
                    FixturePrograms.Add(key, c);
                }
            }
            if (benxiRedirectCase != 0 || liuliLoop) Add("fixture:sole-redirect-range", """
            "modifiers":[{"id":"published-liuli-target-range","query":"attackRange","operation":"add","value":1,"priority":0}]
            """);
            Add(Driver, """
            "activations":[{"id":"hurt","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"loseHp","target":"selectedTarget","amount":1}]},
            {"id":"equip","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"useRandomDeckEquipment","target":"owner","resultBind":"gear"}]},
            {"id":"draw","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{"op":"draw","target":"owner","amount":3}]}]
            """);
            Add(TargetPause, """
            "triggers":[{"id":"actual-new-target","window":"cardUseTargetsFinalized","ownerRelation":"target","priority":-100,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]
            """);
            Add(GainPause, dianjun ? """
            "triggers":[{"id":"real-return-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.sole-target-protection.return"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]},{"op":"loseHp","target":"owner","amount":1}]}]
            """ : """
            "triggers":[{"id":"real-return-gain","window":"cardsGained","subject":"owner","destinationZones":["hand"],"movementOccurrence":"perBatch","movementReasons":["skill-program.sole-target-protection.return"],"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]
            """);
            if (dianjun)
            {
                Add(ReturnHealth, """
                "triggers":[{"id":"native-return-health-child","window":"afterHealthChanged","subject":"owner","optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]
                """);
                Add(AfterDianjun, """
                "triggers":[{"id":"original-after-end-sibling","window":"afterTurnEnded","subject":"owner","turnOwnerScope":"own","priority":-100,"optional":false,"effects":[{"op":"chooseOption","target":"owner","resultBind":"pause","options":[{"id":"continue"}]}]}]
                """);
            }
            if (multi) b.AddCard(StandardContentRegistry.CreateWithClassicGenerals().Cards["classic:zhangba-serpent-spear"]);
            if (chain) Add("fixture:sole-high", """
            "triggers":[{"id":"high-protection","window":"cardUseTargetsFinalized","ownerRelation":"observer","priority":-50,"optional":true,"effects":[{"op":"protectOtherSoleCardTarget","target":"owner"}]}]
            """);
            if (foreignActor) Add("fixture:sole-actor-swap", """
            "triggers":[{"id":"actual-user-replacement","window":"cardUseTargetsFinalized","ownerRelation":"actor","cardKinds":["duel"],"priority":1000,"optional":false,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLegalCurrentCardTarget"},{"op":"replaceCurrentCardUseActor","target":"selectedTarget"}]}]
            """);
            if (zero)
            {
                var returned = fullBenxi ? ",{\"op\":\"chooseOption\",\"target\":\"owner\",\"resultBind\":\"legacy-return\",\"options\":[{\"id\":\"continue\"}]}" : "";
                Add("fixture:sole-legacy", $$"""
                "triggers":[{"id":"legal-legacy-slash","window":"turnStartBeforeNormalFlow","subject":"owner","optional":true,"effects":[{"op":"selectTarget","target":"owner","targetKind":"otherLivingVirtualSlashTarget"},{"op":"useVirtualCard","target":"selectedTarget","outputKind":"slash","targetRestriction":"distanceUnlimitedAgainstTarget"}{{returned}}]}]
                """);
            }
            if (sourceLoss)
            {
                Add(SourceLoss, """
                "triggers":[{"id":"actual-cost-kills-source","window":"cardsMoved","subject":"owner","sourceZones":["hand"],"movementDiscardOnly":true,"movementOccurrence":"perBatch","movementReasons":["skill-program.sole-target-protection.cost"],"optional":false,"effects":[{"op":"loseHp","target":"owner","amount":20}]}]
                """);
                Add(Replacement, """
                "triggers":[{"id":"replace-lost-source","window":"dyingEntering","subject":"owner","usageScope":"game","usageLimit":1,"optional":false,"effects":[{"op":"recoverTo","target":"owner","numberExpression":"integerConstant","minimumValue":3,"clampToMaxHp":true},{"op":"loseOwnerSkillsAndGrant","target":"owner","skillIds":["ol:zhenwei"],"sourceBind":"fixture:sole-spent"}]}]
                """);
                b.AddSkill(new("fixture:sole-spent", "Spent", "Actual source removed after cost."));
            }
            b.AddSkill(new("fixture:sole-peer", "Peer", "Fixed remaining generals") { SelectionWeights = Enum.GetValues<Role>().ToDictionary(r => r, r => r == Role.Lord ? -10000d : 10000d) });
            b.AddGeneral(new("fixture:sole-owner", "Actual user", "supporter", Driver, "wei", dianjun ? 8 : 1, [GainPause, "classic:benxi",
                .. (liuli ? new[] { "boundary:lihuo" } : []), .. (liuliLoop ? new[] { "fixture:sole-redirect-range" } : []), .. (ownSkill ? new[] { Skill } : []),
                .. (foreignActor ? new[] { "fixture:sole-actor-swap" } : []), .. (zero ? new[] { "fixture:sole-legacy" } : []),
                .. (dianjun ? new[] { "ol:dianjun", ReturnHealth, AfterDianjun } : [])]));
            b.AddGeneral(new("fixture:sole-protector", "Actual protector", "supporter", "fixture:sole-peer", "wei", substitute ? 12 : 3,
                [.. (ownSkill ? Array.Empty<string>() : new[] { Skill }), TargetPause, GainPause,
                    .. (liuli ? new[] { "classic:liuli" } : []), .. (liuliLoop ? new[] { "fixture:sole-redirect-range" } : []), .. (sourceLoss ? new[] { SourceLoss, Replacement } : [])]));
            b.AddGeneral(new("fixture:sole-peer-1", "Original target", "supporter", "fixture:sole-peer", "wei", benxiRedirectCase == 0 ? 1 : 4,
                [TargetPause, .. (benxiRedirectCase is 1 or 3 || liuliLoop ? new[] { "classic:liuli", "fixture:sole-redirect-range" } : [])]));
            if (chain) b.AddGeneral(new("fixture:sole-high-protector", "Second protector", "supporter", "fixture:sole-peer", "wei", 20, ["fixture:sole-high", TargetPause]));
            else b.AddGeneral(new("fixture:sole-peer-2", "Original target", "supporter", "fixture:sole-peer", "wei", benxiRedirectCase == 0 ? 1 : 4,
                [TargetPause, .. (benxiRedirectCase is 2 or 3 ? new[] { "classic:liuli", "fixture:sole-redirect-range" } : [])]));
            var definition = kind switch { CardKind.Dodge => "standard:dodge", CardKind.Slash => "standard:slash", CardKind.Duel => "standard:duel", CardKind.Snatch => "standard:snatch",
                CardKind.DrawTwo => "standard:draw_two", CardKind.Indulgence => "standard:indulgence", CardKind.SupplyShortage => "standard:supply_shortage", CardKind.Lightning => "standard:lightning", _ => throw new InvalidOperationException() };
            b.AddDeck(new("fixture:sole-deck", "Small fixed real deck", 4, 0, []) { PhysicalCards = Enumerable.Range(0, 80).Select(i => new ContentDeckPhysicalCard(multi && i % 10 == 0 ? "classic:zhangba-serpent-spear" : definition, red ? Suit.Heart : Suit.Spade, 7)).ToArray() });
            b.AddMode(new(Mode, "Sole protection", 4, 4, new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 3 },
                "fixture:sole-deck", GeneralCandidateCount: 4, GeneralPoolIds: ["fixture:sole-owner", "fixture:sole-protector", "fixture:sole-peer-1", chain ? "fixture:sole-high-protector" : "fixture:sole-peer-2"]));
        }
    }
}
