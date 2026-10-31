#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDamageFramesEntries
{
    internal long BeginDamage(long parentFrameId, int sourceSeat, int targetSeat, int amount, global::CardGame.Core.DamageNature nature = (global::CardGame.Core.DamageNature)0) => _resolve().BeginDamage(parentFrameId, sourceSeat, targetSeat, amount, nature);
    internal global::CardGame.Core.DamageTriggerWindowFrame SetDamageTriggerWindowCursor(long frameId, int candidateIndex) => _resolve().SetDamageTriggerWindowCursor(frameId, candidateIndex);
    internal void SetDamageTriggerWindowStep(long frameId, global::CardGame.Core.ResolutionFrameStep step) => _resolve().SetDamageTriggerWindowStep(frameId, step);
}
