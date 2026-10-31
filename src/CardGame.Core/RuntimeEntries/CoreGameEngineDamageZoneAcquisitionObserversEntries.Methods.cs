#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageZoneAcquisitionObserversEntries
{
    internal bool DamageAcquisitionFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().DamageAcquisitionFirstChild(root, child);
    internal ProgramSkillFrame? DamageAcquisitionObserverRoot() => _resolve().DamageAcquisitionObserverRoot();
    internal bool IsDamageAcquisitionDying() => _resolve().IsDamageAcquisitionDying();
    internal bool TryAdvanceDamageAcquisitionSubtree() => _resolve().TryAdvanceDamageAcquisitionSubtree();
}
