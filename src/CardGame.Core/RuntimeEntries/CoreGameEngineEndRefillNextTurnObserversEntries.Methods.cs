#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineEndRefillNextTurnObserversEntries
{
    internal bool EndRefillFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().EndRefillFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? EndRefillObserverRoot() => _resolve().EndRefillObserverRoot();
    internal bool IsEndRefillDying() => _resolve().IsEndRefillDying();
    internal bool TryAdvanceEndRefillSubtree() => _resolve().TryAdvanceEndRefillSubtree();
}
