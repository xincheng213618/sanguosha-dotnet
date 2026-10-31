#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatActivitiesEntries
{
    internal string DragonBoatFamily(global::CardGame.Core.GeneralDefinition general) => _resolve().DragonBoatFamily(general);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> DragonBoatRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().DragonBoatRuleContributions(player, query);
    internal int DragonBoatTeamScore(string teamId) => _resolve().DragonBoatTeamScore(teamId);
    internal global::CardGame.Core.CharacterState DragonBoatTeammate(int seat) => _resolve().DragonBoatTeammate(seat);
    internal global::CardGame.Core.DragonBoatDraftSnapshot? GetDragonBoatDraft(int viewer) => _resolve().GetDragonBoatDraft(viewer);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DragonBoatTeamSnapshot>? GetDragonBoatStandings(bool finalizing = false) => _resolve().GetDragonBoatStandings(finalizing);
    internal bool TryCompleteDragonBoatRace() => _resolve().TryCompleteDragonBoatRace();
    internal bool IsDragonBoatActivity { get => _resolve().IsDragonBoatActivity; }
}
