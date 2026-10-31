#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesRuntime
{

    internal bool JudgmentOutcomeDamageEdge(int index, ProgramSkillFrame root)
    {
        var child = _resolutionStackOperations._resolutionStack[index];
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        if (child is JudgmentFrame judgment && parent.Id == root.Id)
            return _coreProgramJudgmentOutcomesOperations.JudgmentOutcomeInstruction(root, root.InstructionIndex)is not null && judgment.ParentFrameId == root.Id && _coreProgramLifecycleOperations.IsValidProgramJudgmentContinuation(judgment, root) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<JudgmentRequestedEvent>().Count(request => request.ResolutionId == judgment.Id && request.ParentResolutionId == root.Id && request.TargetSeat == judgment.TargetSeat && request.Reason == judgment.Reason) == 1;
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
            return _coreDamageJudgmentSuitPaymentObserversOperations.Value.IsDamageJudgmentHpLossDyingRide(dying, producer);
        return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    }
}
