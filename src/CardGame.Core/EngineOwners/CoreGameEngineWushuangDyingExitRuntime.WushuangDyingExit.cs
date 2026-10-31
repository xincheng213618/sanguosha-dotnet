#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangDyingExitRuntime
{
    internal bool IsWushuangDyingExitCandidate(ProgramTriggerCandidate c, int victim) => _coreWushuangModeProjectionOperations.IsWushuangMode && c.OwnerSeat == victim && _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.ResolveWushuangCombatTactic, Amount: 2174 or 2175 } e] && c.SkillId == $"wushuang:tactic-{(e.Amount)}";
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 100, SkillProgramTriggerWindow.DyingExited)]
    internal bool IsWushuangDyingExitContext(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.DyingExited || context.SourceSeat != c.OwnerSeat || context.TargetSeat != c.OwnerSeat || !IsWushuangDyingExitCandidate(c, c.OwnerSeat) || !_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Hp <= 0 || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } parent || parent.OwnerSeat != c.OwnerSeat || ret.VictimSeat != c.OwnerSeat || ret.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || ret.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || ret.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != c)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(w => w.Id == parent.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == ret.DyingParentFrameId && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Count(e => e.ResolutionId == ret.DyingFrameId && e.VictimSeat == ret.VictimSeat && e.Survived) == 1;
    }

    internal bool IsExactWushuangDyingExitProgram(ProgramSkillFrame f) => f.WushuangCombat is { TacticId: 2174 or 2175 } && IsWushuangDyingExitReturn(f);
    internal bool IsWushuangDyingExitReturn(ProgramSkillFrame f) => f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) && IsWushuangDyingExitContext(parent.Candidates[parent.CandidateIndex], context);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 61400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 61400)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 61400)]
    internal bool HasWushuangDyingExitDamageObserver(long id)
    {
        var root = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(IsExactWushuangDyingExitProgram);
        return root is not null && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    }
}
