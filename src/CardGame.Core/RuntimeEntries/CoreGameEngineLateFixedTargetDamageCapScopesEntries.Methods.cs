#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLateFixedTargetDamageCapScopesEntries
{
    internal void CaptureLateFixedTargetDamageCapScope(DamageFrame damage) => _resolve().CaptureLateFixedTargetDamageCapScope(damage);
    internal (IReadOnlyList<int> Targets, int Index, RecipientScopedDamageState? Scope) FixedDamageCapChain(GameEngine.IDamageAttempt attack) => _resolve().FixedDamageCapChain(attack);
    internal bool IsValidLateFixedTargetDamageCapProof(LateFixedTargetDamageCapScopeReceipt proof, long attackFrameId, IReadOnlyList<int> chainTargets) => _resolve().IsValidLateFixedTargetDamageCapProof(proof, attackFrameId, chainTargets);
    internal bool IsValidLateFixedTargetDamageCapScope(RecipientScopedDamageState scope, long attackFrameId, IReadOnlyList<int> targets) => _resolve().IsValidLateFixedTargetDamageCapScope(scope, attackFrameId, targets);
}
