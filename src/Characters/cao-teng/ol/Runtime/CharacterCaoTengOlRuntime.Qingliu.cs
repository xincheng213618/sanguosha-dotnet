#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoTengOlRuntime
{
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 90, SkillProgramTriggerWindow.DyingExited)]
    internal bool IsCaoTengQingliuDyingExitContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (_coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is not [{ Op: SkillProgramEffectOp.ResolveQingliuFactionChoice }] || context.Window != SkillProgramTriggerWindow.DyingExited || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].Hp <= 0 || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } parent || ret.VictimSeat != candidate.OwnerSeat || ret.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || ret.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || ret.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || parent.OwnerSeat != candidate.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(w => w.Id == parent.Id);
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1].Id == ret.DyingParentFrameId && _coreProgramEventHistoryOperations.ProgramEventHistory<PlayerDyingEvent>().FirstOrDefault(e => e.VictimSeat == candidate.OwnerSeat)?.ResolutionId == ret.DyingFrameId && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Count(e => e.ResolutionId == ret.DyingFrameId && e.VictimSeat == candidate.OwnerSeat && e.Survived) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<CaoTengFactionCompletedEvent>().Any(e => e.Source.OwnerSeat == candidate.OwnerSeat && !e.Setup);
    }

    internal void ApplyCaoTengFaction(ProgramSkillFrame f, string faction, bool setup)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { CaoTeng = f.CaoTeng!with { Faction = faction, Stage = CaoTengStage.FactionApplying } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengFactionChosenEvent(f.Id, faction, setup));
        _coreNativeFactionChangesOperations.ApplyCaoTengFactionChange(f, faction);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CaoTengFactionCompletedEvent(f.Id, CaoTengSource(f), faction, setup));
        FinishCaoTeng(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal bool IsCaoTengFactionChange(ProgramSkillFrame f, string faction) => f.CaoTeng is { Op: SkillProgramEffectOp.ResolveQingliuFactionChoice, Stage: CaoTengStage.FactionApplying } r && r.Faction == faction && faction is ("qun" or "wei") && r.Source == CaoTengSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<CaoTengFactionCompletedEvent>().Any(e => e.FrameId == f.Id) && CaoTengParentMatches(f) && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CaoTengFactionChosenEvent>().Count(e => e.FrameId == f.Id && e.Faction == faction && e.Setup == (f.WindowContext!.Window == SkillProgramTriggerWindow.GameStarting)) == 1 && (f.WindowContext!.Window == SkillProgramTriggerWindow.GameStarting || faction == (_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]) == "wei" ? "qun" : "wei"));
}
