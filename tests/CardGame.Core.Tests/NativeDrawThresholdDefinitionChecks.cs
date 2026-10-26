using System.Text.Json;
using CardGame.Core;

internal static class NativeDrawThresholdDefinitionChecks
{
    public static void ThresholdRequiresMandatoryOwnerHandGainAndPositiveCount()
    {
        Load();
        Reject(threshold: 0);
        Reject(window: "cardsMoved");
        Reject(subject: "any");
        Reject(occurrence: "perCard");
        Reject(destination: "equipment");
        Reject(optional: true);
        Reject(conditional: true);
    }

    private static void Reject(int threshold = 9, string window = "cardsGained", string subject = "owner",
        string occurrence = "perBatch", string destination = "hand", bool optional = false, bool conditional = false)
    {
        try { Load(threshold, window, subject, occurrence, destination, optional, conditional); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Native draw threshold accepted an unsupported entry or nonpositive threshold.");
    }

    private static void Load(int threshold = 9, string window = "cardsGained", string subject = "owner",
        string occurrence = "perBatch", string destination = "hand", bool optional = false, bool conditional = false)
    {
        const string id = "fixture:native-draw-threshold-shape";
        var effect = new Dictionary<string, object> { ["op"] = "damageRandomEnemyAtDrawThreshold", ["target"] = "owner",
            ["threshold"] = threshold, ["amount"] = 1 };
        if (conditional) effect["condition"] = new { kind = "wounded" };
        _ = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { new { id, revision = 1, triggers = new[] { new
            {
                id = "threshold", window, subject, movementOccurrence = occurrence,
                destinationZones = new[] { destination }, optional, effects = new[] { effect }
            } } } }
        }), JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "摸牌阈值", description = "实际材料累计" } }
        }));
    }
}
