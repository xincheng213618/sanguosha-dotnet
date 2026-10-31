#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDynamicDiscardDamageObserversEntries
{
    internal bool DynamicDiscardDamageFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().DynamicDiscardDamageFirstChild(root, child);
    internal ProgramSkillFrame? DynamicDiscardDamageObserverRoot(long? exactWindow = default) => _resolve().DynamicDiscardDamageObserverRoot(exactWindow);
    internal bool IsDynamicDiscardDamageProgramDying() => _resolve().IsDynamicDiscardDamageProgramDying();
    internal bool TryAdvanceDynamicDiscardDamageSubtree() => _resolve().TryAdvanceDynamicDiscardDamageSubtree();
}
