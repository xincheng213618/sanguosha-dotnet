namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_Core_NativeDrawAttempts
{
    internal static bool MatchesIssuedAttempt(ResolutionFrame parent, NativeDrawAttemptFrame attempt) =>
        parent.NativeDraw is { AttemptReturned: false, RecipientApplied: false } paid &&
        paid.ActiveAttemptId == attempt.Id && attempt.Context.AttemptFrameId == attempt.Id &&
        attempt.Context.ParentFrameId == parent.Id && paid.ReturnKind == attempt.Return.Kind &&
        paid.InstructionIndex == attempt.Return.InstructionIndex && paid.RecipientIndex == attempt.Return.RecipientIndex &&
        paid.RecipientIndex >= 0 && paid.RecipientIndex < paid.Recipients.Count &&
        paid.Recipients[paid.RecipientIndex] == attempt.Context.RecipientSeat &&
        paid.RequestedCount == attempt.Context.RequestedCount && paid.Reason == attempt.Context.Reason;

    internal static bool NativeDrawAttemptRidesOn(ResolutionFrame child, ResolutionFrame parent) =>
        child is NativeDrawAttemptFrame attempt &&
            attempt.Context.ParentFrameId == parent.Id && parent.NativeDraw is { } paid &&
            paid.ActiveAttemptId == attempt.Id && paid.InstructionIndex == attempt.Return.InstructionIndex &&
            paid.RecipientIndex == attempt.Return.RecipientIndex && paid.ReturnKind == attempt.Return.Kind ||
        parent is NativeDrawAttemptFrame native && child is ProgramLifecycleTriggerWindowFrame window &&
            window.Id == native.ActiveWindowFrameId && window.NativeDrawAttempt == native.Context &&
            window.Continuation == ProgramLifecycleContinuation.ResumeNativeDrawAttempt ||
        child is ProgramSkillFrame programChild && parent is ProgramLifecycleTriggerWindowFrame lifecycle &&
            lifecycle.NativeDrawAttempt is { } context && programChild.WindowContext?.NativeDraw == context &&
            programChild.WindowContext.ParentFrameId == lifecycle.Id &&
            lifecycle.CandidateIndex >= 0 && lifecycle.CandidateIndex < lifecycle.Candidates.Count &&
            RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(programChild, lifecycle.Candidates[lifecycle.CandidateIndex]);
}
