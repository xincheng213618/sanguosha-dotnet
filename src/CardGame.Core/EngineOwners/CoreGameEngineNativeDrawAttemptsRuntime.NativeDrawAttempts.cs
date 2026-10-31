#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDrawAttemptsRuntime
{
    internal NativeDrawAttemptRuntime NativeDrawAttempts => _nativeDrawAttemptOperations.Get();
    internal bool HasNativeDrawAttemptCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.NativeDrawStarting);

    internal bool TryBeginNativeDrawAttempt(ResolutionFrame parent) => NativeDrawAttempts.TryBegin(parent);
    internal NativeDrawAttemptFrame? ExactNativeDrawAttempt(ProgramSkillWindowContext context) => NativeDrawAttempts.ExactNativeDrawAttempt(context);
    internal bool IsExactNativeDrawCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => NativeDrawAttempts.IsExactNativeDrawCandidate(candidate, context);
    internal ProgramSkillWindowContext AttachNativeDrawContext(ProgramLifecycleTriggerWindowFrame window, ProgramSkillWindowContext context) => NativeDrawAttempts.AttachNativeDrawContext(window, context);
    internal void CompleteNativeDrawReplacement(ProgramSkillFrame frame, int sourceSeat, IReadOnlyList<int> gainedIds, CardMoveReason reason, long sequenceBefore) => NativeDrawAttempts.CompleteNativeDrawReplacement(frame, sourceSeat, gainedIds, reason, sequenceBefore);
    internal void ReturnNativeDrawStartingProgram(ProgramSkillFrame frame, bool completed) => NativeDrawAttempts.ReturnNativeDrawStartingProgram(frame, completed);
    internal void ReturnNativeDrawAttemptWindow(ProgramLifecycleTriggerWindowFrame window) => NativeDrawAttempts.ReturnNativeDrawAttemptWindow(window);
    internal void ContinueNativeDrawAttempt(long id) => NativeDrawAttempts.ContinueNativeDrawAttempt(id);
    internal static bool NativeDrawAttemptRidesOn(ResolutionFrame child, ResolutionFrame parent) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_NativeDrawAttempts.NativeDrawAttemptRidesOn(child, parent);
    internal void AssertNativeDrawAttempts() => NativeDrawAttempts.AssertNativeDrawAttempts();
}
