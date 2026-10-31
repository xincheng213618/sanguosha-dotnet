#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchPresentationRuntime
{
    internal static string FormatRoleSummary(ContentModeDefinition mode) => RuntimeSkills.RuntimeRules_Core_CommonRules.FormatRoleSummary(mode);
    internal static string FormatTeamSummary(ContentModeDefinition mode) => RuntimeSkills.RuntimeRules_Core_CommonRules.FormatTeamSummary(mode);
    internal static string FormatFactionSummary(ContentModeDefinition mode) => RuntimeSkills.RuntimeRules_Core_CommonRules.FormatFactionSummary(mode);
    internal static string GetTeamName(string teamId, ContentModeDefinition? mode = null) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetTeamName(teamId, mode);
    internal static string GetFactionName(string factionId) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetFactionName(factionId);
    internal string GetRoleName(Role role) => role switch
    {
        Role.Lord => "主公",
        Role.Loyalist => "忠臣",
        Role.Rebel => "反贼",
        Role.Renegade => "内奸",
        Role.Contestant => "独立参赛者",
        Role.TeamA => GetTeamName(_coreMatchSetupDefinitionsOperations.GetModeTeamIds().FirstOrDefault() ?? "team:blue", _coreMatchDefinitionModeDefinitionAccess._modeDefinition),
        Role.TeamB => GetTeamName(_coreMatchSetupDefinitionsOperations.GetModeTeamIds().Skip(1).FirstOrDefault() ?? "team:red", _coreMatchDefinitionModeDefinitionAccess._modeDefinition),
        _ => role.ToString()};
    internal string GetPublicGeneralName(CharacterState player) => player.IdentityConcealed ? "隐匿" : _coreNativeDamageHandlesOperations.Value.IsNationalWarMode && !player.GeneralRevealed ? "暗将" : player.General.Name;
    internal static string GetWinnerName(Winner winner) => RuntimeSkills.RuntimeRules_Core_CommonRules.GetWinnerName(winner);
}
