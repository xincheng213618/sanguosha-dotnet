using System.Text.Json;
using CardGame.Core;

internal static class AppliedDamageReflectionDefinitionChecks
{
    public static void ReflectionRequiresAnOwnerAppliedSegmentAndUnconditionalEffect()
    {
        Load();
        Reject(window: "beforeDamageApplied");
        Reject(window: "playPhaseStarting");
        Reject(subject: "damageSource");
        Reject(occurrence: "perDamagePoint");
        Reject(target: "selectedTarget");
        Reject(conditional: true);
        Reject(activation: true);
    }

    private static void Reject(string window = "afterDamageApplied", string subject = "owner",
        string occurrence = "perDamage", string target = "owner", bool conditional = false, bool activation = false)
    {
        try { Load(window, subject, occurrence, target, conditional, activation); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Applied-damage reflection accepted an unsupported entry or effect shape.");
    }

    private static void Load(string window = "afterDamageApplied", string subject = "owner",
        string occurrence = "perDamage", string target = "owner", bool conditional = false, bool activation = false)
    {
        const string id = "fixture:applied-damage-reflection-shape";
        var effect = new Dictionary<string, object> { ["op"] = "reflectAppliedDamage", ["target"] = target };
        if (conditional) effect["condition"] = new { kind = "wounded" };
        var entry = activation
            ? new Dictionary<string, object> { ["id"] = "reflect", ["minCards"] = 0, ["maxCards"] = 0,
                ["minTargets"] = 0, ["maxTargets"] = 0, ["targetKind"] = "anyLiving", ["usesPerTurn"] = null!,
                ["effects"] = new[] { effect } }
            : new Dictionary<string, object> { ["id"] = "reflect", ["window"] = window, ["subject"] = subject,
                ["damageOccurrence"] = occurrence, ["optional"] = false, ["effects"] = new[] { effect } };
        var skill = new Dictionary<string, object> { ["id"] = id, ["revision"] = 1,
            [activation ? "activations" : "triggers"] = new[] { entry } };
        _ = SkillProgramCatalog.Load(JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.RulesSchemaVersion,
            skills = new[] { skill }
        }), JsonSerializer.Serialize(new
        {
            schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
            skills = new Dictionary<string, object> { [id] = new { name = "反伤入口", description = "复制实际伤害段" } }
        }));
    }
}
