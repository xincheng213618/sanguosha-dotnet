#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeDrawAttemptsEntries
{
    internal global::CardGame.Core.ProgramSkillWindowContext AttachNativeDrawContext(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().AttachNativeDrawContext(window, context);
    internal void CompleteNativeDrawReplacement(global::CardGame.Core.ProgramSkillFrame frame, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> gainedIds, global::CardGame.Core.CardMoveReason reason, long sequenceBefore) => _resolve().CompleteNativeDrawReplacement(frame, sourceSeat, gainedIds, reason, sequenceBefore);
    internal void ContinueNativeDrawAttempt(long id) => _resolve().ContinueNativeDrawAttempt(id);
    internal global::CardGame.Core.NativeDrawAttemptFrame? ExactNativeDrawAttempt(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ExactNativeDrawAttempt(context);
    internal bool IsExactNativeDrawCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsExactNativeDrawCandidate(candidate, context);
    internal void ReturnNativeDrawAttemptWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().ReturnNativeDrawAttemptWindow(window);
    internal void ReturnNativeDrawStartingProgram(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().ReturnNativeDrawStartingProgram(frame, completed);
    internal bool TryBeginNativeDrawAttempt(global::CardGame.Core.ResolutionFrame parent) => _resolve().TryBeginNativeDrawAttempt(parent);
    internal bool HasNativeDrawAttemptCapability { get => _resolve().HasNativeDrawAttemptCapability; }
    internal global::CardGame.Core.NativeDrawAttemptRuntime NativeDrawAttempts { get => _resolve().NativeDrawAttempts; }
}
