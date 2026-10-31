#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageObserversEntries
{
    internal bool HasSameNameHandDamageObserver(long windowId) => _resolve().HasSameNameHandDamageObserver(windowId);
    internal bool IsSameNameHandDiscardOrDamageChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision decision) => _resolve().IsSameNameHandDiscardOrDamageChoice(f, decision);
    internal bool IsSameNameHandDying() => _resolve().IsSameNameHandDying();
    internal bool IsSameNameHandObserverEdge(int child) => _resolve().IsSameNameHandObserverEdge(child);
    internal global::CardGame.Core.ProgramCardTriggerWindowFrame? ResponseCompletionObserverRoot() => _resolve().ResponseCompletionObserverRoot();
    internal bool SameNameHandFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().SameNameHandFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? SameNameHandObserverRoot() => _resolve().SameNameHandObserverRoot();
    internal bool SameNameHandObserverSuffix(int index) => _resolve().SameNameHandObserverSuffix(index);
    internal bool SameNameHandStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().SameNameHandStructuralEdge(parent, child);
    internal bool TryAdvanceSameNameHandSubtree() => _resolve().TryAdvanceSameNameHandSubtree();
    internal bool ValidSameNameHandReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidSameNameHandReceipt(f);
}
