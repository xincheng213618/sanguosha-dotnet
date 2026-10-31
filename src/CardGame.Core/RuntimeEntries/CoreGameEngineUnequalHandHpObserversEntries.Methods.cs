#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUnequalHandHpObserversEntries
{
    internal bool IsUnequalHandHpDying() => _resolve().IsUnequalHandHpDying();
    internal bool TryAdvanceUnequalHandHpSubtree() => _resolve().TryAdvanceUnequalHandHpSubtree();
    internal bool UnequalHandHpFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().UnequalHandHpFirstChild(root, child);
    internal ProgramSkillFrame? UnequalHandHpObserverRoot() => _resolve().UnequalHandHpObserverRoot();
}
