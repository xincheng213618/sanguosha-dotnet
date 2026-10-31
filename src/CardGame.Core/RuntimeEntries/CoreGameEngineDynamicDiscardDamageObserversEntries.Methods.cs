#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDynamicDiscardDamageObserversEntries
{
    internal bool DynamicDiscardDamageFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().DynamicDiscardDamageFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? DynamicDiscardDamageObserverRoot(long? exactWindow = default) => _resolve().DynamicDiscardDamageObserverRoot(exactWindow);
    internal bool IsDynamicDiscardDamageProgramDying() => _resolve().IsDynamicDiscardDamageProgramDying();
    internal bool TryAdvanceDynamicDiscardDamageSubtree() => _resolve().TryAdvanceDynamicDiscardDamageSubtree();
}
