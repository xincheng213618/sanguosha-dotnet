#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRequiredGroupDodgesEntries
{
    internal void FreezeRequiredGroupDodgeResponses(NativeOwners.GroupCardHandle group, NativeOwners.CardAttackHandle attack) => _resolve().FreezeRequiredGroupDodgeResponses(group, attack);
    internal bool TryFinishRequiredGroupDodge(NativeOwners.CardAttackHandle attack) => _resolve().TryFinishRequiredGroupDodge(attack);
}
