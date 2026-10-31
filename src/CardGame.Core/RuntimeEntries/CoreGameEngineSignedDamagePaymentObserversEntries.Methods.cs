#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSignedDamagePaymentObserversEntries
{
    internal bool IsSignedDamagePaymentProgramDying() => _resolve().IsSignedDamagePaymentProgramDying();
    internal bool SignedDamagePaymentFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SignedDamagePaymentFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? SignedDamagePaymentObserverRoot(long? exactWindowId = default) => _resolve().SignedDamagePaymentObserverRoot(exactWindowId);
    internal bool TryAdvanceSignedDamagePaymentSubtree() => _resolve().TryAdvanceSignedDamagePaymentSubtree();
}
