#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOldHandDispositionObserversEntries
{
    internal bool OldHandDispositionFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().OldHandDispositionFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? OldHandDispositionObserverRoot() => _resolve().OldHandDispositionObserverRoot();
    internal bool TryAdvanceOldHandDispositionSubtree() => _resolve().TryAdvanceOldHandDispositionSubtree();
}
