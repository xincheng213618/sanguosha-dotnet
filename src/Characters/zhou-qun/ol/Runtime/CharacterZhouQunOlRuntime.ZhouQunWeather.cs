#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouQunOlRuntime
{
    internal static (string Id, string Name) ZhouQunWeather(Suit suit) => global::CardGame.Core.RuntimeSkills.RuntimeRules_zhou_qun_ol_ZhouQunWeather.ZhouQunWeather(suit);
    internal bool TryReturnZhouQunPaidPreparation(ProgramLifecycleTriggerWindowFrame frame)
    {
        if (!HasZhouQunCapability || frame.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || frame.Continuation != ProgramLifecycleContinuation.NormalTurnStart || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || frame.CandidateIndex != frame.Candidates.Count || _resolutionStackOperations._resolutionStack.Count != 0 || _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
            return false;
        if (!_coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunStartedEvent>().Any(e => e.WindowId == frame.Id && e.Source.OwnerSeat == frame.OwnerSeat && e.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == frame.OwnerSeat && e.Op == SkillProgramEffectOp.ResolveChenshuoRepeatedReveal && frame.Candidates.Any(c => c.OwnerSeat == e.Source.OwnerSeat && c.SkillId == e.Source.SkillId && c.BindingId == e.Source.BindingId && c.SkillInstanceId == e.Source.SkillInstanceId && c.GameplayHash == e.GameplayHash) && _coreProgramEventHistoryOperations.ProgramEventHistory<ZhouQunCompletedEvent>().Count(done => done.FrameId == e.FrameId && done.Applied) == 1))
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == frame.OwnerSeat))
            throw new InvalidOperationException("Chenshuo cannot finalize the same dead actual turn twice.");
        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        return true;
    }
}
