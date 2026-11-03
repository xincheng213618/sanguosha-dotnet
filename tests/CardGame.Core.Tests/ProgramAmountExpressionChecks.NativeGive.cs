using System.Text.Json.Nodes;
using CardGame.Core;

internal static partial class ProgramAmountExpressionChecks
{
    private static void NativeGiveOccurrenceIsScopedAndDeclaresSharedFactDemand()
    {
        const string trigger = """
            {"id":"give","window":"cardsMoved","subject":"owner","movementOccurrence":"perNativeGive","optional":false,
              "effects":[{"op":"selectTarget","target":"owner","targetKind":"eventTarget"},{"op":"draw","target":"selectedTarget","amount":1}]}
            """;
        SkillProgram Parse(JsonNode node) => SkillProgramCatalog.Load($$$"""
            {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{Skill}}}","revision":1,"triggers":[{{{node.ToJsonString()}}}]}]}
            """, Presentation(Skill)).Programs[Skill];
        var baseline = JsonNode.Parse(trigger)!;
        var program = Parse(baseline);
        var demand = new SkillProgramDependencies([new ContentSkillDefinition(Skill, Skill, Skill) { Program = program }]).RequiredFacts;
        Need(program.Triggers.Single().MovementOccurrence == SkillProgramMovementOccurrence.PerNativeGive &&
            demand.HasFlag(ProgramTrackedFact.NativeGive),
            "A custom logical Give trigger declares NativeGive demand without any character id or specialized operation.");
        void Refuse(string field, JsonNode value)
        {
            var invalid = baseline.DeepClone(); invalid[field] = value;
            try { _ = Parse(invalid); }
            catch (InvalidOperationException) { return; }
            throw new InvalidOperationException("Invalid logical Give trigger was accepted: " + field);
        }
        Refuse("window", JsonValue.Create("cardsGained")!);
        Refuse("subject", JsonValue.Create("any")!);
        Refuse("sourceZones", new JsonArray());
        Refuse("destinationZones", JsonNode.Parse("""["hand"]""")!);
        Refuse("movementReasons", new JsonArray());
        Refuse("cardKinds", JsonNode.Parse("""["slash"]""")!);
        Refuse("movementDiscardOnly", JsonValue.Create(false)!);
        var physical = baseline.DeepClone(); physical["movementOccurrence"] = "perCard"; physical["sourceZones"] = JsonNode.Parse("""["hand"]""");
        physical["effects"] = JsonNode.Parse("""[{"op":"draw","target":"owner","amount":1}]""");
        var physicalProgram = Parse(physical);
        Need(!new SkillProgramDependencies([new ContentSkillDefinition(Skill, Skill, Skill) { Program = physicalProgram }])
                .RequiredFacts.HasFlag(ProgramTrackedFact.NativeGive),
            "The physical card occurrence remains independent of logical Give history demand.");
    }
}
