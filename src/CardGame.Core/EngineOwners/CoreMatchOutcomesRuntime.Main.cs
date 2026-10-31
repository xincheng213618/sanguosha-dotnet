#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchOutcomesRuntime
{
    internal void EndAsDraw(string reason)
    {
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is not null || _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null || _cardZonesOperations._cardZones.Count(CardLocation.Processing) != 0)
        {
            throw new InvalidOperationException("A game cannot end as a draw during an active card resolution.");
        }

        _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner = Winner.Draw;
        _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Clear();
        _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.Clear();
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
        {
            player.RoleRevealed = true;
            player.TeamRevealed = _coreNativeDamageHandlesOperations.Value.IsTeamMode;
            if (_coreNativeDamageHandlesOperations.Value.IsNationalWarMode)
            {
                _coreNationalGeneralRevealOperations.RevealNationalInformation(player);
            }
        }

        _coreActualTurnProgressPhaseAccess._phase = TurnPhase.Finished;
        _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._status = EngineStatus.Completed;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        _coreMatchLogOperations.AddLog("GameEnded", $"{(reason)}，本局记为平局。");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WinnerDeterminedEvent(_coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner, _coreNativeDamageHandlesOperations.Value.IsTeamMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId : null, _coreNativeDamageHandlesOperations.Value.IsNationalWarMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId : null));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameEndedEvent(_coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winner, _coreNativeDamageHandlesOperations.Value.IsTeamMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerTeamId : null, _coreNativeDamageHandlesOperations.Value.IsNationalWarMode ? _coreMatchOutcomeStatusWinnerWinnerFactionIdWinnerTeamIdAccess._winnerFactionId : null));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal Winner GetWinnerForTeam(string teamId) => _coreMatchSetupDefinitionsOperations.GetModeTeamIds().Select((candidate, index) => (candidate, index)).Where(item => item.candidate == teamId).Select(item => item.index).FirstOrDefault(-1) switch
    {
        0 => Winner.TeamA,
        1 => Winner.TeamB,
        _ => throw new InvalidOperationException($"Mode '{(_coreMatchDefinitionModeDefinitionAccess._modeDefinition.Id)}' does not define a supported winning team '{(teamId)}'.")};
    internal Winner GetWinnerForFaction(string factionId) => _coreMatchSetupDefinitionsOperations.GetModeFactionIds().Select((candidate, index) => (candidate, index)).Where(item => item.candidate == factionId).Select(item => item.index).FirstOrDefault(-1) switch
    {
        0 => Winner.NationalFactionA,
        1 => Winner.NationalFactionB,
        _ when _coreMatchSetupDefinitionsOperations.GetModeFactionIds().Count > 2 => Winner.NationalFactionC,
        _ => throw new InvalidOperationException($"Mode '{(_coreMatchDefinitionModeDefinitionAccess._modeDefinition.Id)}' does not define a supported winning faction '{(factionId)}'.")};
}
