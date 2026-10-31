#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries
{
    internal void AssertHistoricalEndingUse(CardUseFrame use) => _resolve().AssertHistoricalEndingUse(use);
    internal DyingSuitsOriginalCursor CaptureDyingSuitsOriginalCursor(DyingFrame dying, ProgramLifecycleTriggerWindowFrame entry) => _resolve().CaptureDyingSuitsOriginalCursor(dying, entry);
    internal bool DyingSuitsFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().DyingSuitsFirstChild(f, child);
    internal ProgramSkillFrame? DyingSuitsObserverRoot() => _resolve().DyingSuitsObserverRoot();
    internal bool DyingSuitsPaidPrefix(ProgramSkillFrame root) => _resolve().DyingSuitsPaidPrefix(root);
    internal bool DyingSuitsStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().DyingSuitsStructuralEdge(parent, child);
    internal bool DyingSuitsStructuralPrefix(ProgramSkillFrame root) => _resolve().DyingSuitsStructuralPrefix(root);
    internal bool ExactDyingSuitsOriginalCursor(ProgramSkillFrame root, DyingFrame dying, ProgramLifecycleTriggerWindowFrame entry) => _resolve().ExactDyingSuitsOriginalCursor(root, dying, entry);
    internal bool HistoricalUseObserverEdge(int index) => _resolve().HistoricalUseObserverEdge(index);
    internal bool IsDyingSuitsProgramDying() => _resolve().IsDyingSuitsProgramDying();
    internal bool IsOriginalDyingSuspendedByDyingSuits(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByDyingSuits(dying);
    internal bool TryAdvanceDyingSuitsSubtree() => _resolve().TryAdvanceDyingSuitsSubtree();
    internal bool ValidDyingSuitsPeachUse(CardUseFrame use, DyingSuitsPeachReturn ret) => _resolve().ValidDyingSuitsPeachUse(use, ret);
    internal bool ValidDyingSuitsReceipt(ProgramSkillFrame f) => _resolve().ValidDyingSuitsReceipt(f);
}
