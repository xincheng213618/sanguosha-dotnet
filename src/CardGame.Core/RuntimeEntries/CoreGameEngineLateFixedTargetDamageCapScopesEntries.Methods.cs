#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLateFixedTargetDamageCapScopesEntries
{
    internal void CaptureLateFixedTargetDamageCapScope(global::CardGame.Core.DamageFrame damage) => _resolve().CaptureLateFixedTargetDamageCapScope(damage);
    internal (global::System.Collections.Generic.IReadOnlyList<int> Targets, int Index, global::CardGame.Core.RecipientScopedDamageState? Scope) FixedDamageCapChain(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().FixedDamageCapChain(attack);
    internal bool IsValidLateFixedTargetDamageCapProof(global::CardGame.Core.LateFixedTargetDamageCapScopeReceipt proof, long attackFrameId, global::System.Collections.Generic.IReadOnlyList<int> chainTargets) => _resolve().IsValidLateFixedTargetDamageCapProof(proof, attackFrameId, chainTargets);
    internal bool IsValidLateFixedTargetDamageCapScope(global::CardGame.Core.RecipientScopedDamageState scope, long attackFrameId, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().IsValidLateFixedTargetDamageCapScope(scope, attackFrameId, targets);
}
