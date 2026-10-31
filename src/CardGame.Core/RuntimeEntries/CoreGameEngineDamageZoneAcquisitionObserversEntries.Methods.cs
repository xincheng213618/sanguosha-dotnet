#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionObserversEntries
{
    internal bool DamageAcquisitionFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().DamageAcquisitionFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? DamageAcquisitionObserverRoot() => _resolve().DamageAcquisitionObserverRoot();
    internal bool IsDamageAcquisitionDying() => _resolve().IsDamageAcquisitionDying();
    internal bool TryAdvanceDamageAcquisitionSubtree() => _resolve().TryAdvanceDamageAcquisitionSubtree();
}
