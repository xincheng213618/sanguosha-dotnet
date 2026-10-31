#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineModeRulesRuntime
{
    internal IMatchModeSetupPresentation ModeSetupPresentation => _modeRuntimeOperations._modeRuntime.Find<IMatchModeSetupPresentation>() ?? throw new InvalidOperationException("The mode requires its installed setup presentation.");
    internal IMatchModeFactionPolicy ModeFactionPolicy => _modeRuntimeOperations._modeRuntime.Find<IMatchModeFactionPolicy>() ?? throw new InvalidOperationException("The mode requires its installed faction policy.");

    internal ModePlayerLife[] CaptureModeLife() => _coreMatchParticipantsPlayersAccess._players.Select(player => new ModePlayerLife(player.Seat, player.Role, player.IsAlive, player.TeamId, player.NationalFactionId)).ToArray();
    internal RuleQueryEvaluation ApplyModeNumericRules(CharacterState player, RuleQueryEvaluation ordinary, SkillRuleQuery query) => _modeRuntimeOperations._modeRuntime.AdjustNumericEvaluation(player.Seat, ordinary, query);
    [ProgramDistanceRule(ProgramDistanceStage.DistanceBaseTerms, 10)]
    internal void AddModeDistanceBaseTerms(ProgramDistanceQuery query, List<RuleQueryBaseTerm> terms) => terms.AddRange(_modeRuntimeOperations._modeRuntime.BaseDistanceTerms(query.Source.Seat, query.Target.Seat));
    internal void ApplyModeDeathOutcome(long deathFrameId, CharacterState victim, CharacterState? killer)
    {
        var outcome = _modeRulesOperations._modeRules.Death.Evaluate(CaptureModeLife(), _modeRulesOperations._modeRules.Death.EvaluatesIdentityVictory && _characterDongZhaoOlOperations.ZaowangRebelDeathWins(victim, killer));
        switch (outcome.Kind)
        {
            case ModeDeathOutcomeKind.Preserve:
                return;
            case ModeDeathOutcomeKind.SurvivalRank:
                _modeRuntimeOperations._modeRuntime.RecordDeathRank(deathFrameId, victim.Seat);
                return;
            case ModeDeathOutcomeKind.Identity:
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId = null;
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId = null;
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner = outcome.Winner;
                return;
            case ModeDeathOutcomeKind.Team:
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId = outcome.WinningGroupId;
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner = _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId is null ? Winner.None : _coreMatchOutcomesOperations.GetWinnerForTeam(_coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId);
                return;
            case ModeDeathOutcomeKind.Faction:
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId = outcome.WinningGroupId;
                _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winner = _coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId is null ? Winner.None : _coreMatchOutcomesOperations.GetWinnerForFaction(_coreMatchOutcomeWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId);
                return;
            default:
                throw new InvalidOperationException("The mode returned an unsupported death outcome.");
        }
    }
}
