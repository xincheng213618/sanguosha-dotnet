#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeathGrowthSkillObserversEntries
{
    internal bool DeathGrowthFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().DeathGrowthFirstChild(root, child);
    internal ProgramSkillFrame? DeathGrowthObserverRoot() => _resolve().DeathGrowthObserverRoot();
    internal bool ExactDeathGrowthDyingParent(int index, DyingFrame dying, DeathGrowthSkillReceipt receipt) => _resolve().ExactDeathGrowthDyingParent(index, dying, receipt);
    internal bool ExactDeathGrowthOriginalCursor(ProgramSkillFrame f, DeathFrame death) => _resolve().ExactDeathGrowthOriginalCursor(f, death);
    internal bool IsOriginalDyingSuspendedByDeathGrowth(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByDeathGrowth(dying);
    internal void RequireDeathGrowthSkill(ProgramSkillFrame f) => _resolve().RequireDeathGrowthSkill(f);
    internal bool TryAdvanceDeathGrowthSubtree() => _resolve().TryAdvanceDeathGrowthSubtree();
    internal bool ValidDeathGrowthSkill(ProgramSkillFrame f) => _resolve().ValidDeathGrowthSkill(f);
}
