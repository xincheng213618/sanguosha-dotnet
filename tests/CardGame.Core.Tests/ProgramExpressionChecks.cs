using System.Text.Json;
using CardGame.Core;
using Scene = BoundaryLiaoHuaScenario;

internal static class ProgramExpressionChecks
{
    private const string Draw = """{"op":"draw","target":"owner","amount":1,"condition":{"kind":"expression","expr":EXPR}}""";

    public static void LoaderAndEvaluation()
    {
        var short1 = Load(Draw.Replace("EXPR", """["<",["handCount","owner"],["maxHp","owner"]]"""));
        var short2 = Load(Draw.Replace("EXPR", """["<", ["handCount", "owner"], ["maxHp", "owner"]]"""));
        var other = Load(Draw.Replace("EXPR", """["<=",["handCount","owner"],["maxHp","owner"]]"""));
        Need(short1.GameplayHash == short2.GameplayHash && short1.GameplayHash != other.GameplayHash,
            "Expression content joins the canonical gameplay hash without whitespace sensitivity.");
        Need(short1.Activations[0].Effects[0].Condition is { Kind: SkillProgramConditionKind.Expression, Expression.Op: "<" },
            "The loader freezes the typed expression on the instruction condition.");

        foreach (var (expr, fragment) in new[]
        {
            ("""["sum",1,2]""", "unknown operator"),
            ("""["<","owner",1]""", "expected an integer"),
            ("""["alive","it"]""", "only available inside a participant-set"),
            ("""["hp","owner"]""", "expected a boolean"),
            ("""["and",true]""", "requires 2 to 8 arguments"),
            ("""["<",["hp","owner"],100]""", "integer from -99 to 99"),
            ("""[">",["publicPileCount","owner","invalid"],0]""", "requires a skill identifier"),
            ("""[">",["publicPileCount","owner",1],0]""", "requires a skill identifier"),
            ("""["ruleWounded","it"]""", "only available inside a participant-set"),
        })
            Rejects(Draw.Replace("EXPR", expr), fragment);
        Rejects("""{"op":"draw","target":"owner","amount":1,"condition":{"kind":"expression","value":1,"expr":true}}""", "accepts only kind and expr");
        Rejects("""{"op":"draw","target":"owner","amount":1,"condition":{"kind":"expression"}}""", "expression requires expr");
        Rejects("""{"op":"draw","target":"owner","amount":1}""", "requires a composition instruction condition",
            activationCondition: """{"kind":"expression","expr":true}""");

        var world = new SkillContextExpressionWorld(
            new PlayerSkillContext(0, 4, 4, 2, TurnPhase.Play),
            new PlayerSkillContext(1, 0, 3, 1, TurnPhase.Play) { HpUndefined = true });
        Need(Eval("""["<",["handCount","selectedTarget"],["maxHp","selectedTarget"]]""", world) == true &&
             Eval("""[">",["hp","selectedTarget"],0]""", world) is null &&
             Eval("""["not",[">",["hp","selectedTarget"],0]]""", world) is null &&
             Eval("""["or",[">",["hp","selectedTarget"],0],true]""", world) == true &&
             Eval("""["and",[">",["hp","selectedTarget"],0],false]""", world) == false &&
             Eval("""["all","livingOthers",true]""", world) is null &&
             Eval("""["==",["-",["max",["hp","owner"],1],["lostHp","owner"]],4]""", world) == true,
            "Undefined HP and unknown sets stay unknown through not and short-circuit only on a decided and/or operand.");
        ConditionalGrantFactsUseSharedReadsAndOrderedSuitRewrites();
        CardUseFactsComposeThroughExistingExpressions();
    }

    private static void ConditionalGrantFactsUseSharedReadsAndOrderedSuitRewrites()
    {
        SkillProgramCardPolicy Rewrite(string id, Suit from, Suit to) => new(id,
            SkillProgramCardPolicyKind.RewriteSuit, [], [], 0, from, to, new(SkillProgramConditionKind.Always, 0, []));
        var policies = new[] { Rewrite("first", Suit.Spade, Suit.Heart), Rewrite("second", Suit.Heart, Suit.Diamond) };
        Card[] cards = [new(1, CardKind.Slash, Suit.Spade, 1), new(2, CardKind.Slash, Suit.Heart, 2),
            new(3, CardKind.Slash, Suit.Diamond, 3), new(4, CardKind.Slash, Suit.None, 4)];
        Need(CardSuitRewrites.Apply(Suit.Spade, policies) == Suit.Diamond &&
            CardSuitRewrites.CountEquipmentSuits(cards, policies) == 1 &&
            CardSuitRewrites.CountEquipmentSuits(cards, policies.Reverse().ToArray()) == 2,
            "Equipment suit facts apply the admitted root policies in order, collapse rewritten suits and ignore None.");
        foreach (var contextual in new[] { policies[0] with { OwnerRole = Role.Lord },
                     policies[0] with { Condition = new(SkillProgramConditionKind.Expression, 0, [], expression: Bool("true")) } })
        {
            var rejected = false;
            try { CardSuitRewrites.CountEquipmentSuits([], [contextual]); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "Context-dependent suit rewrites are rejected even when the equipment zone is empty.");
        }
        var owner = new CharacterState { Seat = 0, Name = "Facts", IsHuman = true, Role = Role.Rebel, RoleRevealed = true,
            General = new("fixture:facts", "Facts", "", []), GeneralSelected = true, GeneralRevealed = true, Hp = 4, MaxHp = 4 };
        var world = new SkillStateConditionWorld(new(0, 0), owner, wounded: new WoundedFacts(),
            equipmentSuitCount: _ => CardSuitRewrites.CountEquipmentSuits(cards, policies));
        const string facts = """["and",[">=",["count","living",["ruleWounded","it"]],1],["==",["equipmentSuitCount","owner"],1]]""";
        Load(Draw.Replace("EXPR", facts));
        Need(Eval(facts, world) == true && owner.Hp == owner.MaxHp && ConditionalSkillGrantParser.IsCondition(Bool(facts)) &&
            !SkillStateDescriptor.IsPersistentCondition(Bool(facts)),
            "Dependent-grant conditions use rule-wounded and nonrecursive suit facts; raw recipient restrictions cannot depend on skill effectiveness.");
        Rejects($$"""{"op":"skillState","target":"owner","action":"suppress","match":{},"while":{{facts}}}""", "persistent");
        Need(Eval("""["not",["ruleWounded","owner"]]""", new SkillStateConditionWorld(new(0, 0), owner)) is null &&
            Eval("""[">",["equipmentSuitCount","selectedTarget"],0]""", new SkillContextExpressionWorld(new(0, 4, 4, 0, TurnPhase.Play), null)) is null,
            "Unavailable wounded facts and absent participants remain unknown rather than false or zero.");
        var piles = new PileFacts();
        const string pileCondition = """["and",["==",["publicPileCount","owner","fixture:pile"],3],["==",["publicPileCount","owner","fixture:empty"],0]]""";
        Load(Draw.Replace("EXPR", pileCondition));
        Need(Eval(pileCondition, piles) == true && SkillStateDescriptor.IsPersistentCondition(Bool(pileCondition)) &&
            Eval("""["not",[">",["publicPileCount","selectedTarget","fixture:pile"],0]]""", piles) is null &&
            SkillStateMatcher.Matches(new(filter: Bool(pileCondition)), new("grant", "fixture:skill", "instance", "acquired:fixture"),
                owner, new("fixture:skill", "Skill", ""), bindings: piles),
            "Named pile reads preserve known empty versus unknown, and skill metadata matching forwards the same raw fact.");
    }

    private static void CardUseFactsComposeThroughExistingExpressions()
    {
        const long order = (long)int.MaxValue + 100;
        var current = new CardUseHistoryFact(10, order, 0, 0, CardActionType.Use, false, [0, 2], true, order);
        CardUseHistoryFact[] latest = [new(null, 1, 0, 0, CardActionType.Use, false, [2], false, order - 1),
            current, new(20, order + 1, 0, 0, CardActionType.Response, true, [3], false, order + 1),
            new(30, null, 2, 2, CardActionType.Use, false, [1], false, order + 2),
            new(40, order + 3, 1, 1, CardActionType.Response, false, [1], false, order + 3)];
        CardUseHistoryFact[] observations = [latest[0], current with { Targets = [1] }, .. latest.Skip(1)];
        var world = new CardUseFactsWorld(latest, observations, current);
        const string targets = """["filter",["targets",["filter",["cardUses","window","observations"],["and",["useEstablished","use"],["same",["useActor","use"],"owner"]]]],["not",["same","it","owner"]]]""";
        IReadOnlyList<int>? Seats(string expression)
        {
            using var document = JsonDocument.Parse(expression);
            return ProgramExpressions.EvaluateParticipantSet(ProgramExpressions.ParseParticipantSet(document.RootElement, "targets"), world);
        }
        Need(Seats(targets)!.SequenceEqual([1, 2]) &&
            Seats(targets.Replace("observations", "latest", StringComparison.Ordinal))!.SequenceEqual([2]) &&
            Seats("""["filter","livingOthers",["any",["cardUses","window","observations"],["and",["useEstablished","use"],["contains",["useTargets","use"],"it"]]]]""")!.SequenceEqual([1, 2]) &&
            Eval("""["==",["count",["cardUses","window","latest"],["any",["useTargets","use"],["same","it",["useActor","use"]]]],2]""", world) == true,
            "Both set types compose: observations retain prior targets, latest does not, and nested use/it find both actor 0 and actor 1 targeting themselves.");
        Need(Eval("""["and",["==",["count",["cardUses","window","latest"],["useDeclared","use"]],4],["==",["count",["cardUses","window","latest"],["useIsResponse","use"]],2],["==",["count",["cardUses","window","latest"],["useIsSelfNativeResponse","use"]],1]]""", world) == true &&
            Eval("""["==",["count",["cardUses","window","latest"],["atOrBefore","use","currentUse","accepted"]],2]""", world) == true &&
            Eval("""["same","turnOwner","owner"]""", world) == true,
            "Declaration, response roles and accepted ordering remain independent facts; ordering compares original long IDs or the legacy declaration cursor.");
        var unknown = new CardUseFactsWorld(null, null, null);
        const string missing = """["not",["any",["cardUses","window","latest"],true]]""";
        Need(Eval(missing, unknown) is null && Eval("""["not",["useDeclared","currentUse"]]""", unknown) is null &&
            Eval("""["any",["cardUses","now","latest"],true]""", world) is null,
            "Unknown histories, missing current use and unavailable moments remain unknown through not.");

        var selection = Load($$"""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":{{targets}}},{"op":"draw","target":"selectedTarget","amount":1}""");
        var otherSelection = Load($$"""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":{{targets.Replace("observations", "latest", StringComparison.Ordinal)}}},{"op":"draw","target":"selectedTarget","amount":1}""");
        var trigger = LoadTrigger("""{"kind":"expression","expr":["useDeclared","currentUse"]}""");
        var changedTrigger = LoadTrigger("""{"kind":"expression","expr":["useEstablished","currentUse"]}""");
        var unknownTrigger = LoadTrigger($$"""{"kind":"not","children":[{"kind":"expression","expr":{{missing}}}]}""").Triggers.Single();
        Need(!unknownTrigger.Condition.Evaluate(new(0, 4, true), evaluateExpression: expression => ProgramExpressions.EvaluateBool(expression, unknown)) &&
            !unknownTrigger.Condition.Evaluate(new(0, 4, true)), "Unknown trigger expressions cannot gain qualification through an outer condition not or an absent evaluator.");
        ContentSkillDefinition Definition(SkillProgram program) => new(program.Id, "Expression", "Expression") { Program = program };
        Need(selection.Activations[0].Effects[0].TargetSetExpression?.Type == ProgramExpressionType.ParticipantSet &&
            new SkillProgramDependencies([Definition(selection)]).RequiresFact(ProgramTrackedFact.ActualCardUseHistory) &&
            new SkillProgramDependencies([Definition(trigger)]).RequiresFact(ProgramTrackedFact.ActualCardUseHistory) &&
            !new SkillProgramDependencies([Definition(Load("""{"op":"draw","target":"owner","amount":1}"""))]).RequiresFact(ProgramTrackedFact.ActualCardUseHistory),
            "Only expression consumers request history; collection inputs are typed and do not require a character descriptor demand.");
        Need(ProgramInstructionResolver.Default.Features(LoadTrigger("""{"kind":"expression","expr":["<",["handCount","owner"],["handLimit","owner"]]}""").Triggers[0]).RequiresHandLimitContext &&
            ProgramInstructionResolver.Default.Features(Load("""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":["filter","livingOthers",["<",["handCount","it"],["handLimit","it"]]]},{"op":"draw","target":"selectedTarget","amount":1}""").Activations[0]).RequiresHandLimitContext,
            "Trigger and participant-set expressions request the same hand-limit context as instruction conditions.");
        Need(selection.GameplayHash != otherSelection.GameplayHash && trigger.GameplayHash != changedTrigger.GameplayHash &&
            JsonSerializer.Serialize(selection.Activations[0].Effects[0]).Contains("\"TargetSetExpression\"", StringComparison.Ordinal) &&
            JsonSerializer.Serialize(trigger.Triggers[0].Condition).Contains("\"Expression\"", StringComparison.Ordinal),
            "Target-set and trigger ASTs are serialized, and both raw definition fields join the canonical gameplay fingerprint.");
        foreach (var binding in new[] { "number", "choiceNumber", "boundCount" })
        {
            try { LoadTrigger($$"""{"kind":"expression","expr":["==",["{{binding}}","unissued"],0]}"""); }
            catch (InvalidOperationException error) when (error.Message.Contains("instruction-local bindings", StringComparison.Ordinal)) { continue; }
            throw new InvalidOperationException("Trigger expressions accepted an instruction-local binding.");
        }
        Rejects("""{"op":"selectTarget","target":"owner","targetKind":"otherLiving","targetSetExpr":["filter","livingOthers",[">",["boundCount","unissued"],0]]},{"op":"draw","target":"selectedTarget","amount":1}""", "unknown card binding");
    }

    private sealed class CardUseFactsWorld(IReadOnlyList<CardUseHistoryFact>? latest,
        IReadOnlyList<CardUseHistoryFact>? observations, CardUseHistoryFact? current) : IProgramExpressionWorld
    {
        public int? Participant(string name) => name is "owner" or "turnOwner" ? 0 : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => name == "livingOthers" ? [1, 2, 3] : null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public IReadOnlyList<CardUseHistoryFact>? CardUses(string moment, string view) =>
            moment != "window" ? null : view == "latest" ? latest : observations;
        public CardUseHistoryFact? CurrentUse() => current;
    }

    private sealed class WoundedFacts : ISelectionWoundedQueries
    {
        public bool IdentityConcealed(int seat) => false;
        public bool IsRuleWounded(int seat) => true;
    }

    private sealed class PileFacts : IProgramExpressionWorld
    {
        public int? Participant(string name) => name == "owner" ? 0 : null;
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public int? PublicPileCount(int seat, string skillId) => skillId == "fixture:pile" ? 3 : 0;
    }

    private static ProgramExpression Bool(string json)
    { using var document = JsonDocument.Parse(json); return ProgramExpressions.Parse(document.RootElement, "facts"); }

    public static void FuliFlipsOnlyAsUniqueHighestHp()
    {
        foreach (var (weiHp, flips) in new[] { (3, true), (4, false) })
        {
            var (_, game) = Scene.CreateScene(discardSlash: false, weiHp: weiHp);
            Need(!game.CreateSnapshot(0).Players[0].IsFaceDown, "Liao Hua starts face up.");
            Scene.FatalDamage(game);
            Scene.Reach(game, prompt => prompt is { Kind: DecisionKind.RescueDying, PlayerSeat: 0 });
            Scene.Answer(game, choice => choice.Parameters.GetValueOrDefault("response") == "program-trigger" &&
                choice.Parameters.GetValueOrDefault("skill-id") == Scene.Fuli);
            Scene.Reach(game, prompt => prompt is { Kind: DecisionKind.PlayCard, PlayerSeat: 0 });
            var hero = game.CreateSnapshot(0).Players[0];
            Need(hero.IsAlive && hero.Hp == 4, "Fuli first recovers to the four living factions.");
            Need(hero.IsFaceDown == flips, flips
                ? "A recovered owner with strictly the highest HP turns over through the generic instruction."
                : "A peer with equal HP blocks the expression-gated turnover.");
        }
    }

    private static SkillProgram Load(string effect, string? activationCondition = null)
    {
        var condition = activationCondition is null ? "" : $"\"condition\":{activationCondition},";
        var rules = $$"""
            {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:expression","revision":1,"activations":[
              {"id":"go","minCards":0,"maxCards":0,"minTargets":0,"maxTargets":0,"targetKind":"anyLiving","usesPerTurn":1,{{condition}}"effects":[{{effect}}]}]}]}
            """;
        return ReadProgram(rules);
    }

    private static SkillProgram LoadTrigger(string condition) => ReadProgram($$"""
        {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[{"id":"fixture:expression","revision":1,"triggers":[
          {"id":"go","window":"playPhaseStarting","subject":"owner","optional":true,"condition":{{condition}},"effects":[{"op":"draw","target":"owner","amount":1}]}]}]}
        """);

    private static SkillProgram ReadProgram(string rules)
    {
        var presentation = JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { ["fixture:expression"] = new { name = "表达式", description = "表达式条件" } }
        });
        return SkillProgramCatalog.Load(rules, presentation).Programs["fixture:expression"];
    }

    private static void Rejects(string effect, string fragment, string? activationCondition = null)
    {
        try { Load(effect, activationCondition); }
        catch (InvalidOperationException error) when (error.Message.Contains(fragment, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException($"Expected rejection containing '{fragment}'.");
    }

    private static bool? Eval(string json, IProgramExpressionWorld world)
    {
        using var document = JsonDocument.Parse(json);
        return ProgramExpressions.EvaluateBool(ProgramExpressions.Parse(document.RootElement, "expr"), world);
    }

    private static void Need(bool ok, string message) { if (!ok) throw new InvalidOperationException(message); }
}
