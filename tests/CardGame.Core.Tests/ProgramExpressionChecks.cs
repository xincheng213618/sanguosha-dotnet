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
    }

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
