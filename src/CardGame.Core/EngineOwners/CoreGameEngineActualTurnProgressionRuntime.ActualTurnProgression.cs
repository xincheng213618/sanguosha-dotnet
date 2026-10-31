#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualTurnProgressionRuntime
{
    internal void AdvanceActualTurnProgression(CharacterState previous)
    {
        var progress = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression;
        var resume = progress.NormalResumeSeat ?? _coreTableSeatQueriesOperations.FindNextAliveSeat(previous.Seat);
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
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat = _coreMatchParticipantsPlayersAccess._players[resume].IsAlive ? resume : _coreTableSeatQueriesOperations.FindNextAliveSeat(resume);
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = new(ActualTurnKind.Normal, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber);
        }
    }
}
