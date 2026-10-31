#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    internal bool IsMoLuBuOpeningPhase => _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && IsMoLuBuOpeningPlay(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat);

    internal bool TryBeginMoLuBuOpening()
    {
        if (!HasMoLuBu || !_coreMatchSetupSetupCompleteAccess._setupComplete || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Normal || _coreActualRoundProgressRoundNumberAccess._roundNumber != 0 || _resolutionStackOperations._resolutionStack.Count != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.MoLuBuOpening is not null)
            return false;
        var request = _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuInitializedEvent>().FirstOrDefault(e => _coreMatchParticipantsPlayersAccess._players[e.Source.OwnerSeat].IsAlive && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuOpeningStartedEvent>().Any(s => s.IssuerFrameId == e.FrameId));
        if (request is null)
            return false;
        var normal = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat;
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat = request.Source.OwnerSeat;
        var receipt = new MoLuBuOpeningReceipt(request.FrameId, request.Source, request.GameplayHash, normal, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber + 1);
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = new(ActualTurnKind.Extra, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, NormalResumeSeat: normal)
        {
            MoLuBuOpening = receipt
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuOpeningStartedEvent(request.FrameId, request.Source, receipt.ActualTurn, normal));
        _coreActualTurnsOperations.BeginTurnAfterBeforeTurnPrelude(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat]);
        return true;
    }

    internal bool TryStartMoLuBuOpeningPlay(CharacterState owner)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.MoLuBuOpening is not { PlayStarted: false } r)
            return false;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.NormalResumeSeat != r.NormalSeat || owner.Seat != r.Source.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != owner.Seat || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("Duoqi lost its exact opening actual-turn owner and first-seat return.");
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression with
        {
            MoLuBuOpening = r with
            {
                PlayStarted = true
            }
        };
        _coreMatchLogOperations.AddLog("SkillActivated", $"{(owner.Name)} 发动【夺炁】，执行开局额外出牌阶段（不能使用延时锦囊牌）。", owner.Seat);
        _coreActualTurnsOperations.EnterPlayPhase(owner);
        return true;
    }

    internal bool IsMoLuBuOpeningPlay(int owner) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.MoLuBuOpening is { PlayStarted: true } r && r.Source.OwnerSeat == owner && r.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Extra && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.NormalResumeSeat == r.NormalSeat;
    internal bool TryFinishMoLuBuOpeningPlay()
    {
        if (!IsMoLuBuOpeningPlay(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat))
            return false;
        var r = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.MoLuBuOpening!;
        _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression with
        {
            MoLuBuOpening = null
        };
        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MoLuBuOpeningCompletedEvent(r.IssuerFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, r.NormalSeat));
        _coreMatchLogOperations.AddLog("SkillResolved", $"【夺炁】额外出牌阶段结束，接下来由先手 {(_coreMatchParticipantsPlayersAccess._players[r.NormalSeat].Name)} 开始首个正常回合。", _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat);
        _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat]);
        return true;
    }

    internal bool TryEndMoLuBuOpeningEarly()
    {
        if (!IsMoLuBuOpeningPlay(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat))
            return false;
        _coreProgramLifecycleOperations.CompletePlayPhaseAfterProgramWindow();
        return true;
    }

    internal bool IsMoLuBuOpeningDelayedForbidden(int actor, CardKind kind) => IsMoLuBuOpeningPlay(actor) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(kind);
    [ProgramCardLegality(ProgramCardLegalityStage.BeforeTurnUseBan, 10)]
    internal bool MoLuBuOpeningCardUseForbidden(ProgramCardUseQuery query) => query.ActionType == CardActionType.Use && IsMoLuBuOpeningDelayedForbidden(query.ActorSeat, query.Kind);
}
