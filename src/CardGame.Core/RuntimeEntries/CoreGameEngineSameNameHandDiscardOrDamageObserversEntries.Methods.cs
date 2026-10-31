#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSameNameHandDiscardOrDamageObserversEntries
{
    internal bool IsSameNameHandDiscardOrDamageChoice(ProgramSkillFrame f, PendingDecision decision) => _resolve().IsSameNameHandDiscardOrDamageChoice(f, decision);
    internal bool IsSameNameHandDying() => _resolve().IsSameNameHandDying();
    internal bool IsSameNameHandObserverEdge(int child) => _resolve().IsSameNameHandObserverEdge(child);
    internal ProgramCardTriggerWindowFrame? ResponseCompletionObserverRoot() => _resolve().ResponseCompletionObserverRoot();
    internal bool SameNameHandFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().SameNameHandFirstChild(f, child);
    internal ProgramSkillFrame? SameNameHandObserverRoot() => _resolve().SameNameHandObserverRoot();
    internal bool SameNameHandObserverSuffix(int index) => _resolve().SameNameHandObserverSuffix(index);
    internal bool SameNameHandStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().SameNameHandStructuralEdge(parent, child);
    internal bool TryAdvanceSameNameHandSubtree() => _resolve().TryAdvanceSameNameHandSubtree();
    internal bool ValidSameNameHandReceipt(ProgramSkillFrame f) => _resolve().ValidSameNameHandReceipt(f);
}
