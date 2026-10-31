#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageFramesEntries
{
    internal long BeginDamage(long parentFrameId, int sourceSeat, int targetSeat, int amount, DamageNature nature = (DamageNature)0) => _resolve().BeginDamage(parentFrameId, sourceSeat, targetSeat, amount, nature);
    internal DamageTriggerWindowFrame SetDamageTriggerWindowCursor(long frameId, int candidateIndex) => _resolve().SetDamageTriggerWindowCursor(frameId, candidateIndex);
    internal void SetDamageTriggerWindowStep(long frameId, ResolutionFrameStep step) => _resolve().SetDamageTriggerWindowStep(frameId, step);
}
