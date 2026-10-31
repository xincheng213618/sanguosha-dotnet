#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries
{
    internal void AssertDyingSuits(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertDyingSuits(f);
    internal bool AssertDyingSuitsSubtree() => _resolve().AssertDyingSuitsSubtree();
    internal void AssertEndingHistoricalUses(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertEndingHistoricalUses(f);
    internal void AssertHistoricalEndingUse(global::CardGame.Core.CardUseFrame use) => _resolve().AssertHistoricalEndingUse(use);
    internal global::CardGame.Core.DyingSuitsOriginalCursor CaptureDyingSuitsOriginalCursor(global::CardGame.Core.DyingFrame dying, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry) => _resolve().CaptureDyingSuitsOriginalCursor(dying, entry);
    internal bool DyingSuitsFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().DyingSuitsFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? DyingSuitsObserverRoot() => _resolve().DyingSuitsObserverRoot();
    internal bool DyingSuitsPaidPrefix(global::CardGame.Core.ProgramSkillFrame root) => _resolve().DyingSuitsPaidPrefix(root);
    internal bool DyingSuitsStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().DyingSuitsStructuralEdge(parent, child);
    internal bool DyingSuitsStructuralPrefix(global::CardGame.Core.ProgramSkillFrame root) => _resolve().DyingSuitsStructuralPrefix(root);
    internal bool ExactDyingSuitsOriginalCursor(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.DyingFrame dying, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame entry) => _resolve().ExactDyingSuitsOriginalCursor(root, dying, entry);
    internal bool HasDyingSuitsSuspendedAttack(long attackId) => _resolve().HasDyingSuitsSuspendedAttack(attackId);
    internal bool HistoricalUseObserverEdge(int index) => _resolve().HistoricalUseObserverEdge(index);
    internal bool IsDyingSuitsProgramDying() => _resolve().IsDyingSuitsProgramDying();
    internal bool IsOriginalDyingSuspendedByDyingSuits(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByDyingSuits(dying);
    internal bool TryAdvanceDyingSuitsSubtree() => _resolve().TryAdvanceDyingSuitsSubtree();
    internal bool ValidDyingSuitsPeachUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.DyingSuitsPeachReturn ret) => _resolve().ValidDyingSuitsPeachUse(use, ret);
    internal bool ValidDyingSuitsReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidDyingSuitsReceipt(f);
}
