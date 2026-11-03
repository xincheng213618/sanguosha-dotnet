using System.Text.Json;
using System.Text.Json.Nodes;
using CardGame.Core;

internal static partial class ProgramAmountExpressionChecks
{
    private static void AttackRangeDeclarationsAndFactsAreTypedAndFingerprinted()
    {
        const string declaration = """
            {"id":"near-source","ownerRole":"lord","while":["and",["isFaction","eventSource","wu"],["==",["distance","owner","eventTarget"],1]]}
            """;
        SkillProgram Parse(JsonNode node) => SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Skill}}}","revision":1,"attackRangeRelations":[{{{node.ToJsonString()}}}]}]}
            """, Presentation(Skill)).Programs[Skill];
        var node = JsonNode.Parse(declaration)!;
        var parsed = Parse(node);
        var same = Parse(JsonNode.Parse(node.ToJsonString(new JsonSerializerOptions { WriteIndented = true }))!);
        var changed = node.DeepClone(); changed["while"]![1]![2] = "shu";
        var unqualified = node.DeepClone(); unqualified.AsObject().Remove("ownerRole");
        Need(parsed.AttackRangeRelations.Single() is { Id: "near-source", OwnerRole: Role.Lord, While.Type: ProgramExpressionType.Bool } &&
                parsed.AttackRangeRelations is IList<AttackRangeRelationDeclaration> { IsReadOnly: true } &&
                parsed.GameplayHash == same.GameplayHash && parsed.GameplayHash != Parse(changed).GameplayHash &&
                parsed.GameplayHash != Parse(unqualified).GameplayHash,
            "Continuous range declarations retain typed conditions, freeze their list and fingerprint their condition and source-role requirement.");

        void Refuse(string field, JsonNode? value)
        {
            var invalid = node.DeepClone(); invalid[field] = value;
            try { _ = Parse(invalid); }
            catch (InvalidOperationException) { return; }
            throw new InvalidOperationException("Invalid continuous range declaration was accepted: " + invalid);
        }
        Refuse("ownerRole", JsonValue.Create("unknown-role"));
        Refuse("while", JsonValue.Create(1));
        Refuse("while", JsonNode.Parse("""["isFaction","owner",1]"""));
        Refuse("while", JsonNode.Parse("""["==",["distance","owner",1],1]"""));
        foreach (var expression in new[]
        {
            """[">",["handLimit","owner"],0]""", """[">",["windowAmount"],0]""",
            """[">",["number","saved"],0]""", """[">",["boundCount","saved"],0]""",
            """["isFaction","selectedTarget","wu"]"""
        }) Refuse("while", JsonNode.Parse(expression));

        const string policy = """
            {"op":"grantDirectedTurnCardPolicy","target":"owner","actorRef":{"kind":"owner"},"targetRef":{"kind":"selectedTarget"},
              "effects":["inAttackRange"],"requiresEffectiveSource":false,"captureOnUse":false,"until":[],"while":["isFaction","eventTarget","wu"]}
            """;
        SkillProgram Policy(string effect) => SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Skill}}}","revision":1,"activations":[
              {"id":"grant","minCards":0,"maxCards":0,"minTargets":1,"maxTargets":1,"targetKind":"anyLiving","usesPerTurn":null,"effects":[{{{effect}}}]}]}]}
            """, Presentation(Skill)).Programs[Skill];
        var applied = Policy(policy);
        Need(applied.Activations.Single().Effects.Single() is
                { DirectedPolicyEffects: DirectedTurnCardPolicyEffect.InAttackRange, DirectedPolicyOptions.While.Type: ProgramExpressionType.Bool } &&
                applied.GameplayHash != Policy(policy.Replace("\"wu\"", "\"shu\"", StringComparison.Ordinal)).GameplayHash,
            "Issued relations use the existing directed-policy instruction, with the continuing condition included in its gameplay fingerprint.");

        var world = new RangeFactExpressionWorld();
        bool? Bool(string expression)
        {
            using var document = JsonDocument.Parse(expression);
            return ProgramExpressions.EvaluateBool(ProgramExpressions.Parse(document.RootElement, "range-fact"), world);
        }
        Need(Bool("""["isFaction","eventSource","wu"]""") == true &&
                Bool("""["isFaction","eventTarget","wu"]""") == false &&
                Bool("""["==",["distance","owner","eventTarget"],2]""") == true &&
                Bool("""["==",["distance","eventTarget","owner"],3]""") == true,
            "Faction and directed combat distance are shared participant facts, with source and target order preserved.");
        world.Known = false;
        Need(Bool("""["isFaction","eventSource","wu"]""") is null &&
                Bool("""["not",["isFaction","eventSource","wu"]]""") is null &&
                Bool("""["==",["distance","owner","eventTarget"],2]""") is null,
            "Unavailable faction and distance facts remain unknown rather than inventing a relation.");
        IProgramExpressionWorld ai = new SkillContextExpressionWorld(new PlayerSkillContext(0, 4, 4, 0, TurnPhase.Play), null);
        Need(ai.IsFaction(0, "wu") is null && ai.CombatDistance(0, 1) is null,
            "A local AI context without authoritative participant facts does not invent faction or table geometry.");
    }

    private sealed class RangeFactExpressionWorld : IProgramExpressionWorld
    {
        public bool Known { get; set; } = true;
        public int? Participant(string name) => name switch { "owner" => 0, "eventSource" => 1, "eventTarget" => 2, _ => null };
        public IReadOnlyList<int>? ParticipantSet(string name) => null;
        public int? Stat(string stat, int seat) => null;
        public bool? Flag(string flag, int seat) => null;
        public bool? IsFaction(int seat, string factionId) => Known ? (seat == 1 ? "wu" : "wei") == factionId : null;
        public int? CombatDistance(int source, int target) => Known ? (source == 0 ? 2 : 3) : null;
    }
}
