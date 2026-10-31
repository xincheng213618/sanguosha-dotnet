#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawAttemptsEntries
{
    internal ProgramSkillWindowContext AttachNativeDrawContext(ProgramLifecycleTriggerWindowFrame window, ProgramSkillWindowContext context) => _resolve().AttachNativeDrawContext(window, context);
    internal void CompleteNativeDrawReplacement(ProgramSkillFrame frame, int sourceSeat, IReadOnlyList<int> gainedIds, CardMoveReason reason, long sequenceBefore) => _resolve().CompleteNativeDrawReplacement(frame, sourceSeat, gainedIds, reason, sequenceBefore);
    internal void ContinueNativeDrawAttempt(long id) => _resolve().ContinueNativeDrawAttempt(id);
    internal NativeDrawAttemptFrame? ExactNativeDrawAttempt(ProgramSkillWindowContext context) => _resolve().ExactNativeDrawAttempt(context);
    internal bool IsExactNativeDrawCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsExactNativeDrawCandidate(candidate, context);
    internal void ReturnNativeDrawAttemptWindow(ProgramLifecycleTriggerWindowFrame window) => _resolve().ReturnNativeDrawAttemptWindow(window);
    internal void ReturnNativeDrawStartingProgram(ProgramSkillFrame frame, bool completed) => _resolve().ReturnNativeDrawStartingProgram(frame, completed);
    internal bool TryBeginNativeDrawAttempt(ResolutionFrame parent) => _resolve().TryBeginNativeDrawAttempt(parent);
    internal bool HasNativeDrawAttemptCapability { get => _resolve().HasNativeDrawAttemptCapability; }
    internal NativeDrawAttemptRuntime NativeDrawAttempts { get => _resolve().NativeDrawAttempts; }
}
