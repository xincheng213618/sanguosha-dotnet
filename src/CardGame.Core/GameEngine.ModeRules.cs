namespace CardGame.Core;

public sealed partial class GameEngine
{
    private readonly ModeRuleSet _modeRules;
    private readonly MatchModeRuntime _modeRuntime;

    private IMatchModeSetupPresentation ModeSetupPresentation => _modeRuntime.Find<IMatchModeSetupPresentation>() ??
        throw new InvalidOperationException("The mode requires its installed setup presentation.");
    private IMatchModeFactionPolicy ModeFactionPolicy => _modeRuntime.Find<IMatchModeFactionPolicy>() ??
        throw new InvalidOperationException("The mode requires its installed faction policy.");

    private ModePlayerLife[] CaptureModeLife() => _players.Select(player => new ModePlayerLife(
        player.Seat, player.Role, player.IsAlive, player.TeamId, player.NationalFactionId)).ToArray();

    private RuleQueryEvaluation ApplyModeNumericRules(CharacterState player, RuleQueryEvaluation ordinary, SkillRuleQuery query) =>
        _modeRuntime.AdjustNumericEvaluation(player.Seat, ordinary, query);

    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 10)]
    private void AddModeDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms) =>
        terms.AddRange(_modeRuntime.BaseDistanceTerms(query.Source.Seat, query.Target.Seat));

    private void ApplyModeDeathOutcome(long deathFrameId, CharacterState victim, CharacterState? killer)
    {
        var outcome = _modeRules.Death.Evaluate(CaptureModeLife(),
            _modeRules.Death.EvaluatesIdentityVictory && ZaowangRebelDeathWins(victim, killer));
        switch (outcome.Kind)
        {
            case ModeDeathOutcomeKind.Preserve:
                return;
            case ModeDeathOutcomeKind.SurvivalRank:
                _modeRuntime.RecordDeathRank(deathFrameId, victim.Seat);
                return;
            case ModeDeathOutcomeKind.Identity:
                _winnerTeamId = null;
                _winnerFactionId = null;
                _winner = outcome.Winner;
                return;
            case ModeDeathOutcomeKind.Team:
                _winnerTeamId = outcome.WinningGroupId;
                _winner = _winnerTeamId is null ? Winner.None : GetWinnerForTeam(_winnerTeamId);
                return;
            case ModeDeathOutcomeKind.Faction:
                _winnerFactionId = outcome.WinningGroupId;
                _winner = _winnerFactionId is null ? Winner.None : GetWinnerForFaction(_winnerFactionId);
                return;
            default:
                throw new InvalidOperationException("The mode returned an unsupported death outcome.");
        }
    }
}
