#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOldHandDispositionObserversEntries
{
    internal bool OldHandDispositionFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().OldHandDispositionFirstChild(root, child);
    internal ProgramSkillFrame? OldHandDispositionObserverRoot() => _resolve().OldHandDispositionObserverRoot();
    internal bool TryAdvanceOldHandDispositionSubtree() => _resolve().TryAdvanceOldHandDispositionSubtree();
}
