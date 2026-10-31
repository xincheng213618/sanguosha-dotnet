#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSignedDamagePaymentObserversEntries
{
    internal bool HasSignedDamagePaymentAttackObserver(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().HasSignedDamagePaymentAttackObserver(attack);
    internal bool HasSignedDamagePaymentObserver(long exactWindowId) => _resolve().HasSignedDamagePaymentObserver(exactWindowId);
    internal bool IsSignedDamagePaymentProgramDying() => _resolve().IsSignedDamagePaymentProgramDying();
    internal bool SignedDamagePaymentFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SignedDamagePaymentFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? SignedDamagePaymentObserverRoot(long? exactWindowId = default) => _resolve().SignedDamagePaymentObserverRoot(exactWindowId);
    internal bool TryAdvanceSignedDamagePaymentSubtree() => _resolve().TryAdvanceSignedDamagePaymentSubtree();
}
