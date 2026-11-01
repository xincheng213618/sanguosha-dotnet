using System.Globalization;
using CardGame.Core;

internal static class SkillProgramTestJson
{
    internal static string CurrentSchema(string template) => template.Replace("__RULES_SCHEMA__",
        SkillProgramCatalog.RulesSchemaVersion.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
}
