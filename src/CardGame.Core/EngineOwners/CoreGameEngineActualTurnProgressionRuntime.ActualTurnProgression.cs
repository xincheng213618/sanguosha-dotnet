#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnProgressionRuntime
{
    internal void AdvanceActualTurnProgression(CharacterState previous)
    {
        previous.ReturnedFromRestThisTurn = false;
        var progress = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression;
        var resume = progress.NormalResumeSeat ?? _coreTableSeatQueriesOperations.FindNextNormalTurnSeat(previous.Seat);
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && progress.PendingBeneficiarySeat is { } beneficiary && _coreMatchParticipantsPlayersAccess._players[beneficiary].IsAlive)
        {
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat = beneficiary;
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = new(ActualTurnKind.Extra, beneficiary, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, NormalResumeSeat: resume)
            {
                MouXyBizuoPending = progress.MouXyBizuoPending
            };
            _coreMatchLogOperations.AddLog("ExtraTurnStarted", $"{(_coreMatchParticipantsPlayersAccess._players[beneficiary].Name)} 的额外回合开始。", beneficiary);
        }
        else
        {
            var next = _coreMatchParticipantsPlayersAccess._players[resume];
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat = next.IsAlive || next.IsResting ? resume : _coreTableSeatQueriesOperations.FindNextNormalTurnSeat(resume);
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = new(ActualTurnKind.Normal, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber);
        }
    }
}
