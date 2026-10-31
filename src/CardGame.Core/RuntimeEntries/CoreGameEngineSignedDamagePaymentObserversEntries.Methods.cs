#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSignedDamagePaymentObserversEntries
{
    internal bool IsSignedDamagePaymentProgramDying() => _resolve().IsSignedDamagePaymentProgramDying();
    internal bool SignedDamagePaymentFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SignedDamagePaymentFirstChild(root, child);
    internal ProgramSkillFrame? SignedDamagePaymentObserverRoot(long? exactWindowId = default) => _resolve().SignedDamagePaymentObserverRoot(exactWindowId);
    internal bool TryAdvanceSignedDamagePaymentSubtree() => _resolve().TryAdvanceSignedDamagePaymentSubtree();
}
