using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;

internal static class GeneratedEquipmentChecks
{
    private const string Driver = "fixture:equipment-actions";
    private const string Observer = "fixture:equipment-observer";
    private const string Owner = "fixture:equipment-owner";
    private const string Mode = "identity:classic-generated-equipment-fixture";

    public static void TransfersDeckReturnsAndSimultaneousDestructionUseOriginalEntities()
    {
        var registry = Registry(); var g = Start(registry);
        var weapon = Generate(g, CardKind.BlackIronChain);
        Use(g, "move", [weapon]); Reach(g, p => Action(p, "equipment-recipient")); Answer(g, c => c.Targets.SequenceEqual([1])); Play(g);
        Need(Location(g, weapon) == CardLocation.Equipment(1), "A forged equipment transfer survives leaving its original equipment area.");
        Use(g, "take", targets: [1]); Reach(g, p => p.Choices.Any(c => c.Cards.Contains(weapon))); Answer(g, c => c.Cards.Contains(weapon)); Play(g);
        Need(Location(g, weapon) == CardLocation.Hand(0), "Obtaining a forged equipment into hand retains its physical identity.");
        Use(g, "top", [weapon]); Play(g); Need(Location(g, weapon) == CardLocation.DrawPile, "Returning a forged equipment to the deck does not destroy it.");
        Use(g, "draw-top"); Play(g); Need(Location(g, weapon) == CardLocation.Hand(0), "The same generated entity can be drawn and used again.");
        var armor = Generate(g, CardKind.NationalStyleRobe); Unwear(g, armor);
        Use(g, "discard-two", [weapon, armor]); Play(g);
        var paid = g.CardMovements.Where(m => new[] { weapon, armor }.Contains(m.CardId) && m.To == CardLocation.DiscardPile).ToArray();
        Need(paid.Length == 2 && paid.Select(m => m.BatchId).Distinct().Count() == 1 && paid[0].BatchId > 0 &&
            Location(g, weapon) == CardLocation.OutsideGame && Location(g, armor) == CardLocation.OutsideGame,
            "One simultaneous native discard retains both actual discard facts and destroys both physical entities in one original batch.");
        var treasure = Generate(g, CardKind.Sanlue); Use(g, "discard", [treasure]); Play(g);
        g = Cold(g, registry);
        var before = EndingDraws(g);
        ReachState(g, () => EndingDraws(g) == before + 2);
        Need(EndingDraws(g) == before + 2, "Two independent destruction batches earn two end-phase cards; a multi-card batch earns only one.");
        _ = Cold(g, registry);
    }

    public static void EquipmentDepartureAuthorizationRefillsAfterLossAndColdMovementChild()
    {
        var registry = Registry(); var g = Start(registry);
        var tianji = Generate(g, CardKind.TianjiDiagram);
        Need(g.CardMovements.Any(m => m.Reason.Value.Contains("equipment:tianji-diagram") && m.To == CardLocation.DiscardPile),
            "Tianji's own entry forces one other real card discard.");
        while (g.CreateSnapshot(0).Players[0].HandCount > 1)
        { Use(g, "discard", [g.CreateSnapshot(0).Players[0].Hand.First().Id]); Play(g); }
        Use(g, "unwear"); Reach(g, p => p.Choices.Any(c => c.Cards.Contains(tianji))); Answer(g, c => c.Cards.Contains(tianji));
        Reach(g, p => p.SkillPrompt?.SkillId == Observer && g.ResolutionStack.OfType<ProgramSkillFrame>()
            .Any(f => f.SkillId == "equipment:tianji-diagram" && f.TriggerId == "departure-refill" && f.InvocationAuthorization is not null));
        Need(!g.CreateSnapshot(0).Players[0].Skills!.Any(s => s.ContentId == "equipment:tianji-diagram") &&
            Location(g, tianji) == CardLocation.Hand(0), "Departure authorization continues one window after possession has actually ended.");
        g = Cold(g, registry); Play(g);
        Need(g.CreateSnapshot(0).Players[0].HandCount == 5 && g.Events.Count(e => e.Payload is ProgramBindingStartedEvent s &&
            s.SkillId == "equipment:tianji-diagram" && s.BindingId == "departure-refill") == 1,
            "The exact departed instance refills to five once and returns through its original movement child after cold restore.");

        var lion = Generate(g, CardKind.SilverLion);
        Use(g, "wound"); Play(g); var wounded = g.CreateSnapshot(0).Players[0].Hp;
        Use(g, "make-nationalstylerobe"); Reach(g, p => Action(p, "equipment-offer")); Answer(g, _ => true);
        Reach(g, p => Action(p, "equipment-recipient")); Answer(g, c => c.Targets.SequenceEqual([0]));
        Play(g); g = Cold(g, registry);
        Need(g.CreateSnapshot(0).Players[0].Hp == wounded + 1 && Location(g, lion) == CardLocation.OutsideGame &&
            g.CreateSnapshot(0).Players[0].Equipment.Any(c => c.Kind == CardKind.NationalStyleRobe),
            $"Silver Lion replacement: HP {wounded}->{g.CreateSnapshot(0).Players[0].Hp}; lion {Location(g, lion)}; equipment {string.Join(",", g.CreateSnapshot(0).Players[0].Equipment.Select(c => c.Kind))}.");

        var armor = g.CreateSnapshot(0).Players[0].Equipment.Single(c => c.Kind == CardKind.NationalStyleRobe).Id;
        Unwear(g, armor); Accept(g, new EndPlayPhaseCommand(0, g.Revision, P(g)!.PromptId));
        Reach(g, p => p.Kind == DecisionKind.DiscardCards && p.PlayerSeat == 0);
        var discard = P(g)!;
        var payment = new[] { armor }.Concat(discard.ValidCardIds.Where(id => id != armor)).Take(discard.RequiredCardCount).ToArray();
        Accept(g, new DiscardCardsCommand(0, payment, discard.PromptId, g.Revision));
        Need(Location(g, armor) == CardLocation.OutsideGame && g.CardMovements.Any(m => m.CardId == armor && m.To == CardLocation.DiscardPile),
            "A forged entity discarded in the actual discard phase keeps its original public discard fact after destruction for all movement observers.");
        _ = Cold(g, registry);
    }

    public static void FullCurrentTargetSelectionChainsEverySlashTarget()
    {
        var registry = Registry(); var g = Start(registry); Generate(g, CardKind.BlackIronChain);
        Use(g, "extra-target"); Play(g); Use(g, "get-slash"); Play(g);
        var action = g.GetHumanLegalActions().First(a => a.Kind == LegalActionKind.Slash && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1, 2]));
        Accept(g, new PlayCardCommand(0, action.CardId!.Value, action.TargetSeats, g.Revision, P(g)!.PromptId, action.PlayedCardKind));
        Reach(g, p => p.Kind == DecisionKind.RespondDodge);
        Need(g.CreateSnapshot(0).Players[1].IsChained && g.CreateSnapshot(0).Players[2].IsChained &&
            !g.CreateSnapshot(0).Players[3].IsChained,
            "Mandatory complete target selection chains both actual Slash targets before their effects, and no unrelated participant.");
        g = Cold(g, registry); Play(g);
        Need(g.Events.Count(e => e.Payload is ProgramCardTriggerResolvedEvent p && p.SkillId == "equipment:black-iron-chain" && p.Activated) == 1,
            "The complete target-set instruction runs once for the original use and survives cold native responses.");
    }

    public static void CancelledSlashClaimUsesSharedPhysicalUseAndHealthReturn()
    {
        var registry = Registry(); var g = Start(registry); Generate(g, CardKind.BintieDoubleHalberd);
        Use(g, "get-slash"); Play(g);
        var action = g.GetHumanLegalActions().First(a => a.CardId is not null && a.Kind == LegalActionKind.Slash && a.ConversionSource is null && a.TargetSeats.SequenceEqual([1]));
        var slash = action.CardId!.Value; var hp = g.CreateSnapshot(0).Players[0].Hp;
        Accept(g, new PlayCardCommand(0, slash, [1], g.Revision, P(g)!.PromptId, action.PlayedCardKind)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        Reach(g, p => p.SkillPrompt?.SkillId == "equipment:bintie-double-halberd" && Action(p, "activate") || p.Kind == DecisionKind.PlayCard && p.PlayerSeat == 0);
        Need(P(g)?.SkillPrompt?.SkillId == "equipment:bintie-double-halberd", "A natively dodged Slash offers the equipped Bintie skill.");
        Answer(g, c => c.Parameters.GetValueOrDefault("program-action") == "activate");
        Reach(g, p => p.SkillPrompt?.SkillId == Observer && Location(g, slash) == CardLocation.Hand(0));
        g = Cold(g, registry); Play(g);
        Need(g.CreateSnapshot(0).Players[0].Hp == hp - 1 && Location(g, slash) == CardLocation.Hand(0) &&
            g.Events.Count(e => e.Payload is ProgramCurrentUsePhysicalCardsClaimedEvent claim && claim.Claim.SkillId == "equipment:bintie-double-halberd") == 1 &&
            g.GetHumanLegalActions().Any(a => a.CardId == slash && a.Kind == LegalActionKind.Slash),
            "A native Dodge pays one health cost, claims the original Slash once, draws one and adds one real Slash allowance.");
    }

    public static void ElementalSlashConversionsComposeWithoutReusingTheSameInstance()
    {
        var registry = Registry(); var g = Start(registry); Generate(g, CardKind.FiveElementsFan);
        Use(g, "get-slash"); Play(g); Use(g, "get-fire"); Play(g);
        Need(g.GetHumanLegalActions().Any(a => a.PlayedCardKind == CardKind.ThunderSlash &&
            a.ConversionSource?.SkillId == "equipment:five-elements-fan" && (a.AdditionalConversionSources?.Count ?? 0) == 0),
            "A physical elemental Slash has a direct conversion through the same legal-action producer.");
        var action = g.GetHumanLegalActions().FirstOrDefault(a => a.PlayedCardKind == CardKind.ThunderSlash &&
            a.ConversionSource?.SkillId == Driver && a.AdditionalConversionSources?.SingleOrDefault()?.SkillId == "equipment:five-elements-fan" &&
            a.TargetSeats.SequenceEqual([1]));
        Need(action is not null, "A configured primary Slash conversion composes with an equipped elemental conversion.");
        Need(!g.GetHumanLegalActions().Any(a => a.ConversionSource?.SkillId == "equipment:five-elements-fan" &&
            a.AdditionalConversionSources?.Any(s => s.SkillId == "equipment:five-elements-fan") == true),
            "One equipment skill instance cannot convert the same use repeatedly through its different aliases.");
        var id = action!.CardId!.Value;
        Accept(g, new PlayCardCommand(0, id, [1], g.Revision, P(g)!.PromptId, action.PlayedCardKind)
            { ConversionSource = action.ConversionSource, AdditionalConversionSources = action.AdditionalConversionSources });
        Play(g);
        Need(g.Events.Select(e => e.Payload).OfType<CardActionAcceptedEvent>().Any(e => e.Action.EffectiveKind == CardKind.ThunderSlash &&
            e.Action.PhysicalCards.Any(c => c.CardId == id) && e.Action.ConversionChain.Any(s => s.SkillId == Driver) &&
            e.Action.ConversionChain.Any(s => s.SkillId == "equipment:five-elements-fan")),
            "A configured Slash-to-fire source and the fan produce one native thunder Slash using the original entity and both exact sources.");
        _ = Cold(g, registry);
    }

    private static ContentRegistry Registry() => ContentRegistry.Build(new StandardContentPackage(),
        new StandardActiveSkillExpansionPackage(includeJijiu: true), new StandardRescueSkillExpansionPackage(),
        new StandardClassicGeneralPackage(), new Fixture());
    private static GameEngine Start(ContentRegistry registry)
    {
        var g = GameEngine.CreateStandard(new GameOptions { Seed = 31, PlayerCount = 4, HumanSeat = 0, HumanSeats = [0, 1, 2, 3], HumanRole = Role.Lord,
            ModeId = Mode, UseInteractiveSetup = true, UseInteractiveDiscard = true, AdvanceAfterHumanCommands = false, MaxTurns = 5 }, registry);
        Accept(g, new StartGameCommand()); Reach(g, p => p.Kind == DecisionKind.SelectGeneral);
        Accept(g, new SelectGeneralCommand(0, Owner, g.Revision, P(g)!.PromptId)); Play(g); return g;
    }
    private static int Generate(GameEngine g, CardKind kind)
    {
        Use(g, "make-" + kind.ToString().ToLowerInvariant()); Reach(g, p => Action(p, "equipment-offer")); Answer(g, _ => true);
        Reach(g, p => Action(p, "equipment-recipient")); Answer(g, c => c.Targets.SequenceEqual([0])); Play(g);
        return g.CreateCardZoneDiagnostics().Single(c => c.CardKind == kind && c.Location == CardLocation.Equipment(0)).CardId;
    }
    private static void Unwear(GameEngine g, int id)
    { Use(g, "unwear"); Reach(g, p => p.Choices.Any(c => c.Cards.Contains(id))); Answer(g, c => c.Cards.Contains(id)); Play(g); }
    private static CardLocation Location(GameEngine g, int id) => g.CreateCardZoneDiagnostics().Single(c => c.CardId == id).Location;
    private static int EndingDraws(GameEngine g) => g.CardMovements.Count(m => m.To == CardLocation.Hand(0) && m.Reason.Value == "skill-program." + Driver + ".Draw");
    private static void Use(GameEngine g, string id, int[]? cards = null, int[]? targets = null) =>
        Accept(g, new UseProgramSkillCommand(0, Driver, id, cards ?? [], targets ?? [], g.Revision, P(g)!.PromptId));
    private static PendingDecision? P(GameEngine g) => Enumerable.Range(0, 4).Select(s => g.CreateSnapshot(s).PendingDecision).FirstOrDefault(p => p is not null);
    private static bool Action(PendingDecision p, string action) => p.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") == action);
    private static void Answer(GameEngine g, Func<PromptChoice, bool> choose)
    { var p = P(g)!; Accept(g, new AnswerPromptCommand(p.PlayerSeat, p.PromptId, p.Choices.First(choose).Id, g.Revision)); }
    private static void Play(GameEngine g) => Reach(g, p => p.Kind == DecisionKind.PlayCard && p.PlayerSeat == 0);
    private static void Reach(GameEngine g, Func<PendingDecision, bool> stop) => ReachState(g, () => P(g) is { } p && stop(p));
    private static void ReachState(GameEngine g, Func<bool> stop)
    {
        for (var i = 0; i < 180; i++)
        {
            if (stop()) return;
            var p = P(g);
            if (p is null) { Accept(g, new AdvanceOneStepCommand(g.Revision)); continue; }
            if (p.Kind == DecisionKind.SelectGeneral) { Accept(g, new SelectGeneralCommand(p.PlayerSeat, p.ValidContentIds[0], g.Revision, p.PromptId)); continue; }
            if (p.Kind == DecisionKind.PlayCard) { Accept(g, new EndPlayPhaseCommand(p.PlayerSeat, g.Revision, p.PromptId)); continue; }
            if (p.Kind == DecisionKind.RespondDodge && p.PlayerSeat != 0 && p.Choices.Any(c => c.Cards.Count > 0))
            { Answer(g, c => c.Cards.Count > 0); continue; }
            if (p.Kind == DecisionKind.DiscardCards)
            { Accept(g, new DiscardCardsCommand(p.PlayerSeat, p.ValidCardIds.Take(p.RequiredCardCount).ToArray(), p.PromptId, g.Revision)); continue; }
            if (p.Choices.Count > 0)
            {
                var choice = p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("program-action") == "skip") ??
                    p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("option-id") == "continue") ??
                    p.Choices.FirstOrDefault(c => c.Cards.Count == 0) ?? p.Choices[0];
                Answer(g, c => c.Id == choice.Id); continue;
            }
            Accept(g, new AdvanceOneStepCommand(g.Revision));
        }
        throw new InvalidOperationException("Missed generated-equipment boundary: " + JsonSerializer.Serialize(P(g)));
    }
    private static GameEngine Cold(GameEngine g, ContentRegistry registry)
    {
        var restored = GameReplay.Restore(GameCheckpointJson.Deserialize(GameCheckpointJson.Serialize(g.CreateCheckpoint())), registry);
        Need(State(restored) == State(g), "Cold equipment replay changed the original pending cursor, physical cards or private player views."); return restored;
    }
    private static string State(GameEngine g) => JsonSerializer.Serialize(new { Views = Enumerable.Range(0, 4).Select(s => SnapshotJson.Serialize(g.CreateSnapshot(s))),
        g.CardMovements, Frames = JsonSerializer.Serialize(g.ResolutionStack), Zones = g.CreateCardZoneDiagnostics() });
    private static void Accept(GameEngine g, GameCommand command) { var result = g.Submit(command); Need(result.Accepted, result.Error?.Message ?? "Equipment fixture command rejected."); }
    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }

    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:equipment-composition", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder b)
        {
            var actions = new List<object>();
            static object E(string op, object? extra = null)
            {
                var values = extra is null ? new Dictionary<string, object?>() : JsonSerializer.Deserialize<Dictionary<string, object?>>(JsonSerializer.Serialize(extra))!;
                values["op"] = op; values["target"] = "owner"; return values;
            }
            void Add(string id, int cards, object[] effects, int targets = 0) => actions.Add(new { id, minCards = cards, maxCards = cards,
                sourceZones = new[] { "hand", "equipment" }, minTargets = targets, maxTargets = targets,
                targetKind = targets == 0 ? "anyLiving" : "otherLiving", usesPerTurn = (int?)null, effects });
            foreach (var kind in new[] { CardKind.BlackIronChain, CardKind.NationalStyleRobe, CardKind.Sanlue, CardKind.TianjiDiagram, CardKind.BintieDoubleHalberd, CardKind.FiveElementsFan, CardKind.SilverLion })
                Add("make-" + kind.ToString().ToLowerInvariant(), 0, [E("offerGeneratedEquipment", new { amount = 1, source = BuiltInEquipmentContent.ForgedSource,
                    resultBind = "generated", templates = new[] { new { kind = char.ToLowerInvariant(kind.ToString()[0]) + kind.ToString()[1..], suit = "spade", rank = 5 } } }),
                    E("placeBoundEquipment", new { sourceBind = "generated", chooseRecipient = true })]);
            Add("move", 1, [E("captureSelectedCards", new { resultBind = "material" }), E("placeBoundEquipment", new { sourceBind = "material", chooseRecipient = true })]);
            Add("top", 1, [E("captureSelectedCards", new { resultBind = "material" }), E("moveBoundCards", new { sourceBind = "material", destination = "drawPileTop", awaitMovementTriggers = true })]);
            Add("draw-top", 0, [E("revealTopCards", new { amount = 1, resultBind = "top", visibility = "public" }), E("moveBoundCards", new { sourceBind = "top", destination = "ownerHand", awaitMovementTriggers = true })]);
            Add("take", 0, [E("selectAndMoveOwnedCard", new { chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "selectedTarget" }, zones = new[] { "equipment" }, count = 1, destination = "ownerHand", awaitMovementTriggers = true })], 1);
            Add("unwear", 0, [E("selectAndMoveOwnedCard", new { chooserRef = new { kind = "owner" }, cardOwnerRef = new { kind = "owner" }, zones = new[] { "equipment" }, count = 1, destination = "ownerHand", awaitMovementTriggers = true })]);
            Add("discard", 1, [E("discardSelected", new { amount = 1 })]);
            Add("discard-two", 2, [E("discardSelected", new { amount = 2 })]);
            Add("extra-target", 0, [E("grantTurnRuleModifier", new { ruleQuery = "cardTargetCount", ruleOperation = "add", amount = 1,
                cardKinds = new[] { "slash", "fireSlash", "thunderSlash", "coldSlash" } })]);
            Add("wound", 0, [E("loseHp", new { amount = 1 })]);
            Add("get-fire", 0, [E("obtainDeckCards", new { amount = 1, cardKinds = new[] { "fireSlash" } })]);
            Add("get-slash", 0, [E("obtainDeckCards", new { amount = 1, cardKinds = new[] { "slash" } })]);
            var driver = new { id = Driver, revision = 1, activations = actions,
                viewAs = new[] { new { id = "slash-to-fire", inputKinds = new[] { "slash" }, inputSuits = Array.Empty<string>(), outputKind = "fireSlash", forPlay = true, forResponse = false } },
                triggers = new[] { new { id = "ending", window = "turnEnding", subject = "owner", optional = false,
                effects = new[] { E("draw", new { amountExpr = new object[] { "number", GeneratedEquipmentRuntime.DestroyedThisTurn } }) } } } };
            var observer = new { id = Observer, revision = 1, triggers = new[] {
                new { id = "pause-gain", window = "cardsGained", subject = "owner", destinationZones = new[] { "hand" }, movementOccurrence = "perBatch", optional = false,
                    effects = new[] { E("chooseOption", new { resultBind = "continued", options = new[] { new { id = "continue" } } }) } } } };
            var defender = new { id = "fixture:equipment-defender", revision = 1, triggers = new[] { new { id = "initial-dodges", window = "gameStarting", subject = "owner", optional = false,
                effects = new[] { E("obtainDeckCards", new { amount = 4, cardKinds = new[] { "dodge" } }) } } } };
            var program = SkillProgramCatalog.Load(JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.RulesSchemaVersion, skills = new object[] { driver, observer, defender } }),
                JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion, skills = new Dictionary<string, object> {
                    [Driver] = new { name = "装备实体测试", description = "共享装备动作" }, [Observer] = new { name = "获得子窗", description = "共享移动续接", optionLabels = new { @continue = "继续" } },
                    ["fixture:equipment-defender"] = new { name = "原生闪", description = "真实响应材料" } } }));
            foreach (var id in new[] { Driver, Observer, "fixture:equipment-defender" }) b.AddSkill(new(id, id, "共享装备测试") { Program = program.Programs[id], ProgramPresentation = program.Presentations[id] });
            var generals = new List<string> { Owner }; b.AddGeneral(new(Owner, Owner, Owner, Driver, BaseHp: 4, AdditionalSkillIds: [Observer]));
            for (var i = 1; i < 4; i++) { var id = "fixture:equipment-defender-" + i; generals.Add(id); b.AddGeneral(new(id, id, id, "fixture:equipment-defender")); }
            var kinds = new[] { CardKind.Slash, CardKind.FireSlash, CardKind.Dodge, CardKind.Crossbow, CardKind.BaguaFormation, CardKind.OffensiveHorse, CardKind.DefensiveHorse, CardKind.WoodenOx };
            foreach (var kind in kinds) b.AddCard(new("fixture:equipment-" + kind, kind.ToString(), "牌", "真实测试实体", kind));
            b.AddDeck(new("fixture:equipment-deck", "共享装备牌堆", 4, 2, kinds.Select(k => new ContentDeckCardCount("fixture:equipment-" + k, 20)).ToArray()));
            b.AddMode(new(Mode, Mode, 4, 4, new Dictionary<string, int> { ["Lord"] = 1, ["Loyalist"] = 1, ["Rebel"] = 1, ["Renegade"] = 1 }, "fixture:equipment-deck", GeneralPoolIds: generals));
        }
    }
}
