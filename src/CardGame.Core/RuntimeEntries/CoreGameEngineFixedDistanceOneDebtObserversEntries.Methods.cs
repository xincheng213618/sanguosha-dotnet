#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFixedDistanceOneDebtObserversEntries
{
    internal bool FixedDistanceDebtFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().FixedDistanceDebtFirstChild(root, child);
    internal ProgramSkillFrame? FixedDistanceDebtObserverRoot() => _resolve().FixedDistanceDebtObserverRoot();
    internal bool IsFixedDistanceDebtProgramDying() => _resolve().IsFixedDistanceDebtProgramDying();
    internal bool TryAdvanceFixedDistanceDebtSubtree() => _resolve().TryAdvanceFixedDistanceDebtSubtree();
}
