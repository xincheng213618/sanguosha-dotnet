#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndRefillNextTurnObserversEntries
{
    internal bool EndRefillFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().EndRefillFirstChild(root, child);
    internal ProgramSkillFrame? EndRefillObserverRoot() => _resolve().EndRefillObserverRoot();
    internal bool IsEndRefillDying() => _resolve().IsEndRefillDying();
    internal bool TryAdvanceEndRefillSubtree() => _resolve().TryAdvanceEndRefillSubtree();
}
