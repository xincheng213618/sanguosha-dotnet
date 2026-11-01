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
            ("""["alive","it"]""", "only available inside all, any or count"),
            ("""["hp","owner"]""", "expected a boolean"),
            ("""["and",true]""", "requires 2 to 8 arguments"),
            ("""["<",["hp","owner"],100]""", "integer from -99 to 99"),
            ("""[">",["publicPileCount","owner","invalid"],0]""", "requires a skill identifier"),
            ("""[">",["publicPileCount","owner",1],0]""", "requires a skill identifier"),
            ("""["ruleWounded","it"]""", "only available inside all, any or count"),
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
