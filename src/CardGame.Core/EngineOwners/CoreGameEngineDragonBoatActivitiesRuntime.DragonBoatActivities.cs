#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDragonBoatActivitiesRuntime
{
    internal bool IsDragonBoatActivity => _coreMatchDefinitionModeDefinitionAccess._modeDefinition.DragonBoatActivity is not null;

    internal CharacterState DragonBoatTeammate(int seat) => _coreMatchParticipantsPlayersAccess._players.Single(player => player.Seat != seat && player.TeamId == _coreMatchParticipantsPlayersAccess._players[seat].TeamId);
    internal int DragonBoatTeamScore(string teamId) => _coreActivityBattleRulesOperations.Value.DragonBoatRules?.TeamScore(teamId) ?? 0;
    internal string DragonBoatFamily(GeneralDefinition general) => _contentRegistryOperations._contentRegistry.Generals.GetValueOrDefault(general.Id)?.CharacterId ?? general.Id;
    internal IReadOnlyList<DragonBoatTeamSnapshot>? GetDragonBoatStandings(bool finalizing = false) => _coreActivityBattleRulesOperations.Value.DragonBoatRules?.Standings(finalizing);
    internal DragonBoatDraftSnapshot? GetDragonBoatDraft(int viewer) => _coreActivityBattleRulesOperations.Value.DragonBoatRules?.Draft(viewer);
    internal bool TryCompleteDragonBoatRace() => _coreActivityBattleRulesOperations.Value.DragonBoatRules is IMatchModeLifecycle lifecycle && lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterTurnEnded);
    internal IEnumerable<RuleQueryContribution> DragonBoatRuleContributions(CharacterState player, SkillRuleQuery query) => _coreActivityBattleRulesOperations.Value.DragonBoatRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
}
