#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequiredGroupDodgesEntries
{
    internal void FreezeRequiredGroupDodgeResponses(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().FreezeRequiredGroupDodgeResponses(group, attack);
    internal bool TryFinishRequiredGroupDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryFinishRequiredGroupDodge(attack);
}
