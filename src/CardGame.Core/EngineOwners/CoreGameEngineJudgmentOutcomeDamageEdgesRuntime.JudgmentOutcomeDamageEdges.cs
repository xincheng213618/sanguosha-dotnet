#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesRuntime
{
    internal ProgramSkillFrame? BeforeDamageJudgmentOutcomeRoot(long windowId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == windowId);
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index] is not BeforeDamageProgramWindowFrame window || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame root || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, window.Candidates[window.CandidateIndex].Candidate) || root.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || context.ParentFrameId != window.Id || (_coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(window) ? context.SourceSeat is not null : context.SourceSeat != window.SourceSeat) || context.TargetSeat != window.TargetSeat || context.Amount != window.Amount || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(root.SkillId)?.Program is not { } program || program.GameplayHash != root.GameplayHash || !ProgramInstructionResolver.Default.Resolve(root, program).Instructions.Any(effect => effect.OutcomeBind is not null))
            return null;
        for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            if (!JudgmentOutcomeDamageEdge(child, root))
                return null;
        return root;
    }

    internal bool JudgmentOutcomeDamageEdge(int index, ProgramSkillFrame root)
    {
        var child = _resolutionStackOperations._resolutionStack[index];
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        if (child is JudgmentFrame judgment && parent.Id == root.Id)
            return _coreProgramJudgmentOutcomesOperations.JudgmentOutcomeInstruction(root, root.InstructionIndex)is not null && judgment.ParentFrameId == root.Id && _coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(judgment, root) && _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentRequestedEvent>().Count(request => request.ResolutionId == judgment.Id && request.ParentResolutionId == root.Id && request.TargetSeat == judgment.TargetSeat && request.Reason == judgment.Reason) == 1;
        if (child is ProgramJudgmentTriggerWindowFrame window && parent is JudgmentFrame judged)
            return judged.ParentFrameId == root.Id && window.ParentFrameId == judged.Id && window.Judgment.JudgmentFrameId == judged.Id && window.Judgment.SubjectSeat == judged.TargetSeat && window.Judgment.SourceSeat == judged.SourceSeat && window.Judgment.Reason == judged.Reason && _coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(judged, root);
        if (child is ProgramSkillFrame final && parent is ProgramJudgmentTriggerWindowFrame finalized)
        {
            if (finalized.ZhenyiBeforeResult == true)
                return _coreZhenyiJudgmentWindowsOperations.IsExactZhenyiBeforeResultProgramChild(finalized, final);
            if (!finalized.Activated || finalized.CandidateIndex < 0 || finalized.CandidateIndex >= finalized.Candidates.Count || final.WindowContext is not { Window: SkillProgramTriggerWindow.JudgmentFinalized } context || context.ParentFrameId != finalized.Id || context.Judgment != finalized.Judgment)
                return false;
            var candidate = finalized.Candidates[finalized.CandidateIndex];
            return candidate.OwnerSeat == final.OwnerSeat && candidate.SkillId == final.SkillId && candidate.TriggerId == final.TriggerId && candidate.SkillInstanceId == final.SkillInstanceId && candidate.GameplayHash == final.GameplayHash;
        }

        if (child is ProgramSkillFrame replacement && parent is JudgmentFrame replace)
            return replace.ParentFrameId == root.Id && _coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(replace, root) && replacement.WindowContext is { Window: SkillProgramTriggerWindow.JudgmentReplacing, JudgmentReplacement: { } receipt } context && context.ParentFrameId == replace.Id && receipt.JudgmentFrameId == replace.Id && _coreNativeJudgmentOperations.CurrentJudgmentCandidate(replace)is { } candidate && candidate.OwnerSeat == replacement.OwnerSeat && candidate.ProgramId == replacement.SkillId && candidate.ProgramTriggerId == replacement.TriggerId && candidate.SkillInstanceId == replacement.SkillInstanceId && candidate.GameplayHash == replacement.GameplayHash;
        if (child is DyingFrame dying && parent is ProgramSkillFrame producer)
            return _coreDamageJudgmentSuitPaymentObserversOperations.IsDamageJudgmentHpLossDyingRide(dying, producer);
        return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    }

    internal bool HasExactBeforeDamageJudgmentSubtree(long windowId) => BeforeDamageJudgmentOutcomeRoot(windowId)is not null;
    internal bool HasExactJudgmentOutcomeSubtree(long judgmentId) => _resolutionStackOperations._resolutionStack.OfType<JudgmentFrame>().SingleOrDefault(candidate => candidate.Id == judgmentId)is { } judgment && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(before => BeforeDamageJudgmentOutcomeRoot(before.Id)?.Id == judgment.ParentFrameId);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 260)]
    internal bool IsJudgmentOutcomeProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } active && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(before => BeforeDamageJudgmentOutcomeRoot(before.Id)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == active.Id) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id) && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().SingleOrDefault(frame => frame.Id == active.Id)is { } dying && dying.ParentFrameId == active.ParentFrameId && dying.VictimSeat == active.VictimSeat);
    internal bool IsExactJudgmentOutcomeFinalizedSubtree(ProgramJudgmentTriggerWindowFrame window, JudgmentFrame? judgment, int index) => judgment is not null && index >= 1 && index + 1 < _resolutionStackOperations._resolutionStack.Count && window.ParentFrameId == judgment.Id && _resolutionStackOperations._resolutionStack[index - 1] == judgment && _resolutionStackOperations._resolutionStack[index + 1] is ProgramSkillFrame && _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(before => BeforeDamageJudgmentOutcomeRoot(before.Id)?.Id == judgment.ParentFrameId);
}
