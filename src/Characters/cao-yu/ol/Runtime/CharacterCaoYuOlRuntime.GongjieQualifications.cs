#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoYuOlRuntime
{
    internal bool CaoYuRoundFirst(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:gongjie" || c.BindingId != "round-first" || context.Window != SkillProgramTriggerWindow.RoundStarting || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber < 1 || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Extra || _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Count != 1 || !_coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Contains(_coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<RoundStartedEvent>().LastOrDefault()is not { } round || round.RoundNumber != _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber || round.ActorSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.RoundStarting } w || w.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c || _coreProgramEventHistoryOperations.ProgramEventHistory<CaoYuStartedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId && e.Round == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber))
            return false;
        return CaoYuGiftCards(c.OwnerSeat, c.OwnerSeat).Count > 0 && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.Seat != c.OwnerSeat);
    }
}
