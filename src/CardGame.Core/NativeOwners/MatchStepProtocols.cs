#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepProtocols(global::System.Func<global::CardGame.Core.NativeFrameAutomaticRuntime> getNativeFrameAutomaticRuntime, global::System.Func<global::CardGame.Core.MatchModeRuntime> getMatchModeRuntime, global::System.Func<global::CardGame.Core.ProgramRuntimeHooks> getProgramRuntimeHooks, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries> getCoreGameEngineTypedRecoveryDamageCursorRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries> getCoreGameEngineWenhePoisonRuntime, global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDyingEntries> getCoreNativeDyingRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageWindowsEntries> getCoreNativeDamageWindowsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeGroupRecoveryEntries> getCoreNativeGroupRecoveryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchParticipantSetupEntries> getCoreMatchParticipantSetupRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineParticipantAndReserveProgramsEntries> getCoreGameEngineParticipantAndReserveProgramsRuntime) : IMatchStepProtocols
{
    public bool TryAdvanceAutomatic(NativeFrameAutomaticPhase phase) => getNativeFrameAutomaticRuntime().TryAdvance(phase);
    public bool TryBeginOpening() => getMatchModeRuntime().TryBeginOpening();
    public bool TryAdvanceHook(ProgramRuntimePhase phase) => getProgramRuntimeHooks().TryAdvance(phase);
    public bool HasTypedResponseCompletionHealthCursor() => getCoreGameEngineTypedRecoveryDamageCursorRuntime().ProjectTypedResponseCompletionHealthCursor()is { Owner: not null, IsMalformed: false };
    public bool HasTypedCommittedSlashPaymentCursor() => getCoreGameEngineTypedRecoveryDamageCursorRuntime().ProjectTypedCommittedSlashPaymentCursor()is { Owner: not null, IsMalformed: false };
    public bool IsCurrentWenhePoisonDying() => getCoreGameEngineWenhePoisonRuntime().IsCurrentWenhePoisonDying();
    public bool HasTypedRecoveryProgramDying() => getCoreGameEngineTypedRecoveryDamageCursorRuntime().HasTypedRecoveryProgramDying();
    public bool HasRuntimeDyingProof() => getProgramRuntimeHooks().IsDying(ProgramDyingProofPhase.Runtime);
    public void PublishState() => advanceRulesAndPublishState();
    public void RunDyingStep() => getCoreNativeDyingRuntime().RunOneDyingStep();
    public void RunDamageTriggerStep() => getCoreNativeDamageWindowsRuntime().RunOneDamageTriggerStep();
    public void RunGroupRecoveryStep() => getCoreNativeGroupRecoveryRuntime().RunOneGroupRecoveryStep();
    public void RunSetupStep() => getCoreMatchParticipantSetupRuntime().RunOneSetupStep();
    public bool BeginGameStartingPrograms() => getCoreGameEngineParticipantAndReserveProgramsRuntime().BeginGameStartingPrograms();
}
