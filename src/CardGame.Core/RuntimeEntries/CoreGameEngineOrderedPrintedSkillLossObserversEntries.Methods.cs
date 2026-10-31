#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossObserversEntries
{
    internal bool ExactOrderedPrintedSkillLossParent(global::CardGame.Core.ProgramSkillFrame f, scoped out global::CardGame.Core.DamageTriggerWindowFrame window, scoped out global::CardGame.Core.DamageFrame damage, scoped out global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ExactOrderedPrintedSkillLossParent(f, out window, out damage, out attack);
    internal bool IsOrderedPrintedSkillLossProgramDying() => _resolve().IsOrderedPrintedSkillLossProgramDying();
    internal bool OrderedPrintedSkillLossFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().OrderedPrintedSkillLossFirstChild(root, child);
    internal bool OrderedPrintedSkillLossObserverEdge(int child) => _resolve().OrderedPrintedSkillLossObserverEdge(child);
    internal global::CardGame.Core.ProgramSkillFrame? OrderedPrintedSkillLossObserverRoot(long? ownerRootId = default) => _resolve().OrderedPrintedSkillLossObserverRoot(ownerRootId);
    internal bool OrderedPrintedSkillLossSkillsChangedEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OrderedPrintedSkillLossSkillsChangedEdge(parent, child);
    internal bool OrderedPrintedSkillLossStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OrderedPrintedSkillLossStructuralEdge(parent, child);
    internal bool TryAdvanceOrderedPrintedSkillLossSubtree() => _resolve().TryAdvanceOrderedPrintedSkillLossSubtree();
    internal bool ValidOrderedPrintedSkillLossLease(global::CardGame.Core.OrderedPrintedSkillLossLease lease) => _resolve().ValidOrderedPrintedSkillLossLease(lease);
    internal bool ValidOrderedPrintedSkillLossReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidOrderedPrintedSkillLossReceipt(f);
}
