#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheNullifiedClaimObserversEntries
{
    internal bool IsShanheNullifiedClaimProgramDying() => _resolve().IsShanheNullifiedClaimProgramDying();
    internal bool ShanheClaimProgramHpLossDying(ProgramSkillFrame producer, DyingFrame dying) => _resolve().ShanheClaimProgramHpLossDying(producer, dying);
    internal int[] ShanheClaimProvenChildMaterials(int rootIndex) => _resolve().ShanheClaimProvenChildMaterials(rootIndex);
    internal bool TryGetShanheNullifiedClaimRide(NullificationWindowFrame window, scoped out int[] childProcessingCosts) => _resolve().TryGetShanheNullifiedClaimRide(window, out childProcessingCosts);
}
