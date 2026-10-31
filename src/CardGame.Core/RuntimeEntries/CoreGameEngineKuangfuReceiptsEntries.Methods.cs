#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineKuangfuReceiptsEntries
{
    internal void AssertKuangfuPrograms() => _resolve().AssertKuangfuPrograms();
    internal bool DistanceCardUseExemption110HasIssuedKuangfuDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption110HasIssuedKuangfuDistance(q);
    internal bool DistancePhysicalSlash130HasIssuedKuangfuDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().DistancePhysicalSlash130HasIssuedKuangfuDistance(q);
    internal bool HasExactKuangfuProgramSelection(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().HasExactKuangfuProgramSelection(f, plan);
    internal bool HasIssuedKuangfuDistance(long? id, int actor) => _resolve().HasIssuedKuangfuDistance(id, actor);
    internal bool HasKuangfuDamageObserver(long window) => _resolve().HasKuangfuDamageObserver(window);
    internal bool IsKuangfuMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsKuangfuMovement(f, effect, pending);
    internal bool IsKuangfuProgramDying() => _resolve().IsKuangfuProgramDying();
    internal bool IsKuangfuSlashUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsKuangfuSlashUse(use);
    internal bool KuangfuFirstPaidChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().KuangfuFirstPaidChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? KuangfuPaidObserverRoot(long? damageWindow = default) => _resolve().KuangfuPaidObserverRoot(damageWindow);
    internal void ObserveKuangfuAppliedDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveKuangfuAppliedDamage(payload);
    internal bool TryAdvanceKuangfuSubtree() => _resolve().TryAdvanceKuangfuSubtree();
    internal bool ValidKuangfu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidKuangfu(f);
}
