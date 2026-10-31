#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatActivitiesEntries
{
    internal string DragonBoatFamily(GeneralDefinition general) => _resolve().DragonBoatFamily(general);
    internal IEnumerable<RuleQueryContribution> DragonBoatRuleContributions(CharacterState player, SkillRuleQuery query) => _resolve().DragonBoatRuleContributions(player, query);
    internal int DragonBoatTeamScore(string teamId) => _resolve().DragonBoatTeamScore(teamId);
    internal CharacterState DragonBoatTeammate(int seat) => _resolve().DragonBoatTeammate(seat);
    internal DragonBoatDraftSnapshot? GetDragonBoatDraft(int viewer) => _resolve().GetDragonBoatDraft(viewer);
    internal IReadOnlyList<DragonBoatTeamSnapshot>? GetDragonBoatStandings(bool finalizing = false) => _resolve().GetDragonBoatStandings(finalizing);
    internal bool TryCompleteDragonBoatRace() => _resolve().TryCompleteDragonBoatRace();
    internal bool IsDragonBoatActivity { get => _resolve().IsDragonBoatActivity; }
}
