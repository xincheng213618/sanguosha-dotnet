using System.Text.Json;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Core.Campaigns;

internal static class ShanheSpecialEquipmentChecks
{

    public static void OtherLivingTargetSetUsesCapturedCountAndRejectsWrongDefinitions()
    {
        const string id = "fixture:other-living-targets";
        var rules = $$$"""
        {"schemaVersion":{{{SkillProgramCatalog.RulesSchemaVersion}}},"skills":[{"id":"{{{id}}}","revision":1,"modifiers":[
         {"id":"all-others","query":"cardTargetCount","operation":"set","valueExpression":"otherLivingPlayerCount",
          "cardKinds":["slash","fireSlash","thunderSlash","coldSlash"],"priority":0}]}]}
        """;
        var presentation = JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "实际其他存活人数", description = "原生杀目标数" } } });
        var modifier = SkillProgramCatalog.Load(rules, presentation).Programs[id].Modifiers.Single();
        var context = new SkillProgramRuleContext(new(0, 4, 4, 0, TurnPhase.Play), 1);
        Need(new[] { 0, 2, 7 }.All(count => modifier.EvaluateValue(context with { OtherLivingPlayerCount = count }) == count),
            "The shared modifier uses its captured actual other-living count, including zero, rather than a local mode maximum.");
        foreach (var invalid in new[]
        {
            rules.Replace("\"operation\":\"set\"", "\"operation\":\"add\"", StringComparison.Ordinal),
            rules.Replace("\"query\":\"cardTargetCount\"", "\"query\":\"handLimit\"", StringComparison.Ordinal),
            rules.Replace("\"coldSlash\"", "\"duel\"", StringComparison.Ordinal),
            rules.Replace(",\"coldSlash\"", "", StringComparison.Ordinal)
        })
        {
            var rejected = false;
            try { _ = SkillProgramCatalog.Load(invalid, presentation); }
            catch (InvalidOperationException) { rejected = true; }
            Need(rejected, "The loader limits dynamic all-other targets to an owner CardTargetCount Set over the complete four-Slash family.");
        }
    }

    private static void Need(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
