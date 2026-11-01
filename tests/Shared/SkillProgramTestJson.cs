using System.Globalization;
using System.Text.Json.Nodes;
using CardGame.Core;

internal static class SkillProgramTestJson
{
    internal static string CurrentSchema(string template) => template.Replace("__RULES_SCHEMA__",
        SkillProgramCatalog.RulesSchemaVersion.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);

    internal static string VersionedEmbeddedRules(string rules)
    {
        var root = JsonNode.Parse(rules)!.AsObject();
        root.Add("schemaVersion", SkillProgramCatalog.RulesSchemaVersion);
        return root.ToJsonString();
    }
}
