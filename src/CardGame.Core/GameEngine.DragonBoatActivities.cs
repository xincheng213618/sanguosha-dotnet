namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsDragonBoatActivity => _modeDefinition.DragonBoatActivity is not null;
    private CharacterState DragonBoatTeammate(int seat) => _players.Single(player => player.Seat != seat && player.TeamId == _players[seat].TeamId);
    private int DragonBoatTeamScore(string teamId) => DragonBoatRules?.TeamScore(teamId) ?? 0;
    private string DragonBoatFamily(GeneralDefinition general) => _contentRegistry.Generals.GetValueOrDefault(general.Id)?.CharacterId ?? general.Id;
    private IReadOnlyList<DragonBoatTeamSnapshot>? GetDragonBoatStandings(bool finalizing = false) => DragonBoatRules?.Standings(finalizing);
    private DragonBoatDraftSnapshot? GetDragonBoatDraft(int viewer) => DragonBoatRules?.Draft(viewer);
    private bool TryCompleteDragonBoatRace() => DragonBoatRules is IMatchModeLifecycle lifecycle &&
        lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterTurnEnded);
    private IEnumerable<RuleQueryContribution> DragonBoatRuleContributions(CharacterState player, SkillRuleQuery query) =>
        DragonBoatRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
}
