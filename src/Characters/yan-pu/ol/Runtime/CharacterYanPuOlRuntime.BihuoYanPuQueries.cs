#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanPuOlRuntime
{
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 80, SkillProgramTriggerWindow.DyingExited)]
    internal bool IsYanPuDyingExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.DrawDyingExitRoundDistance }] && context.Window == SkillProgramTriggerWindow.DyingExited && context.OwnerSeat == c.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } parent && ret.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && ret.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ret.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.OwnerSeat == ret.VictimSeat && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && parent.Candidates[parent.CandidateIndex] == c && context.SourceSeat == ret.VictimSeat && context.TargetSeat == ret.VictimSeat && _coreMatchParticipantsPlayersAccess._players[ret.VictimSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[ret.VictimSeat].Hp > 0 && _resolutionStackOperations._resolutionStack.FindIndex(w => w.Id == parent.Id)is var index && index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == ret.DyingParentFrameId && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Count(e => e.ResolutionId == ret.DyingFrameId && e.VictimSeat == ret.VictimSeat && e.Survived) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<YanPuStartedEvent>().Any(e => e.Stage == YanPuStage.ExitDrawChildren && e.Source.OwnerSeat == c.OwnerSeat && e.Source.SkillId == c.SkillId);
}
