#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MatchStepProtocols(Func<NativeFrameAutomaticRuntime> getNativeFrameAutomaticRuntime, Func<MatchModeRuntime> getMatchModeRuntime, Func<ProgramRuntimeHooks> getProgramRuntimeHooks, Func<RuntimeEntries.CoreGameEngineTypedRecoveryDamageCursorEntries> getCoreGameEngineTypedRecoveryDamageCursorRuntime, Func<RuntimeEntries.CoreGameEngineWenhePoisonEntries> getCoreGameEngineWenhePoisonRuntime, KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, Func<RuntimeEntries.CoreNativeDyingEntries> getCoreNativeDyingRuntime, Func<RuntimeEntries.CoreNativeDamageWindowsEntries> getCoreNativeDamageWindowsRuntime, Func<RuntimeEntries.CoreNativeGroupRecoveryEntries> getCoreNativeGroupRecoveryRuntime, Func<RuntimeEntries.CoreMatchParticipantSetupEntries> getCoreMatchParticipantSetupRuntime, Func<RuntimeEntries.CoreGameEngineParticipantAndReserveProgramsEntries> getCoreGameEngineParticipantAndReserveProgramsRuntime) : IMatchStepProtocols
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
