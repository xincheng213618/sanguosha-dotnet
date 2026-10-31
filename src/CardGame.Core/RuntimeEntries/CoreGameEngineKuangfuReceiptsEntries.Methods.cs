#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKuangfuReceiptsEntries
{
    internal bool DistanceCardUseExemption110HasIssuedKuangfuDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption110HasIssuedKuangfuDistance(q);
    internal bool DistancePhysicalSlash130HasIssuedKuangfuDistance(ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash130HasIssuedKuangfuDistance(q);
    internal bool HasIssuedKuangfuDistance(long? id, int actor) => _resolve().HasIssuedKuangfuDistance(id, actor);
    internal bool IsKuangfuProgramDying() => _resolve().IsKuangfuProgramDying();
    internal bool IsKuangfuSlashUse(CardUseFrame use) => _resolve().IsKuangfuSlashUse(use);
    internal bool KuangfuFirstPaidChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().KuangfuFirstPaidChild(root, child);
    internal ProgramSkillFrame? KuangfuPaidObserverRoot(long? damageWindow = default) => _resolve().KuangfuPaidObserverRoot(damageWindow);
    internal void ObserveKuangfuAppliedDamage(IGameEvent payload) => _resolve().ObserveKuangfuAppliedDamage(payload);
    internal bool TryAdvanceKuangfuSubtree() => _resolve().TryAdvanceKuangfuSubtree();
    internal bool ValidKuangfu(ProgramSkillFrame f) => _resolve().ValidKuangfu(f);
}
