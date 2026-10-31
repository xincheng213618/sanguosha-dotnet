#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathGrowthSkillObserversEntries
{
    internal void AssertRegisteredDeathGrowthSkill(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredDeathGrowthSkill(frame);
    internal bool DeathGrowthFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().DeathGrowthFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? DeathGrowthObserverRoot() => _resolve().DeathGrowthObserverRoot();
    internal bool ExactDeathGrowthDyingParent(int index, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.DeathGrowthSkillReceipt receipt) => _resolve().ExactDeathGrowthDyingParent(index, dying, receipt);
    internal bool ExactDeathGrowthOriginalCursor(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.DeathFrame death) => _resolve().ExactDeathGrowthOriginalCursor(f, death);
    internal bool HasDeathGrowthDamageObserver(long id) => _resolve().HasDeathGrowthDamageObserver(id);
    internal bool IsDeathGrowthDying() => _resolve().IsDeathGrowthDying();
    internal bool IsOriginalDyingSuspendedByDeathGrowth(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByDeathGrowth(dying);
    internal void RequireDeathGrowthSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireDeathGrowthSkill(f);
    internal bool TryAdvanceDeathGrowthSubtree() => _resolve().TryAdvanceDeathGrowthSubtree();
    internal bool ValidDeathGrowthSkill(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDeathGrowthSkill(f);
}
