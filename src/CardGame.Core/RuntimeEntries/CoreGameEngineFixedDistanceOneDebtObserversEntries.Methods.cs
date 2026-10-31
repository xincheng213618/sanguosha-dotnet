#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtObserversEntries
{
    internal bool FixedDistanceDebtFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().FixedDistanceDebtFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? FixedDistanceDebtObserverRoot() => _resolve().FixedDistanceDebtObserverRoot();
    internal bool HasFixedDistanceDebtDamageObserver(long window) => _resolve().HasFixedDistanceDebtDamageObserver(window);
    internal bool IsFixedDistanceDebtProgramDying() => _resolve().IsFixedDistanceDebtProgramDying();
    internal bool TryAdvanceFixedDistanceDebtSubtree() => _resolve().TryAdvanceFixedDistanceDebtSubtree();
}
