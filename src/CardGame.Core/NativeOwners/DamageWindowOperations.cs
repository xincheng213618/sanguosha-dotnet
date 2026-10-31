#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DamageWindowOperations(Func<RuntimeEntries.CoreNativeDamageFramesEntries> getCoreNativeDamageFramesRuntime, Func<RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, DamageTriggerCapturedAttackFactory createDamageTriggerCapturedAttack) : IDamageWindowOperations
{
    public DamageTriggerWindowFrame SetWindowCursor(long windowId, int candidateIndex) => getCoreNativeDamageFramesRuntime().SetDamageTriggerWindowCursor(windowId, candidateIndex);
    public void SetWindowStep(long windowId, ResolutionFrameStep step) => getCoreNativeDamageFramesRuntime().SetDamageTriggerWindowStep(windowId, step);
    public IDamageTriggerCapturedAttack CaptureAttack(DamageTriggerWindowFrame window) => createDamageTriggerCapturedAttack(getCoreNativeDamageHandlesRuntime().GetDamageTriggerAttack(window));
}
