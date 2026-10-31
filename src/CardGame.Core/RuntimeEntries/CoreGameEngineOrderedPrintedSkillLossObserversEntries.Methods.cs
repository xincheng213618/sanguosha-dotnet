#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOrderedPrintedSkillLossObserversEntries
{
    internal bool ExactOrderedPrintedSkillLossParent(ProgramSkillFrame f, scoped out DamageTriggerWindowFrame window, scoped out DamageFrame damage, scoped out GameEngine.IDamageAttempt attack) => _resolve().ExactOrderedPrintedSkillLossParent(f, out window, out damage, out attack);
    internal bool IsOrderedPrintedSkillLossProgramDying() => _resolve().IsOrderedPrintedSkillLossProgramDying();
    internal bool OrderedPrintedSkillLossFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().OrderedPrintedSkillLossFirstChild(root, child);
    internal bool OrderedPrintedSkillLossObserverEdge(int child) => _resolve().OrderedPrintedSkillLossObserverEdge(child);
    internal ProgramSkillFrame? OrderedPrintedSkillLossObserverRoot(long? ownerRootId = default) => _resolve().OrderedPrintedSkillLossObserverRoot(ownerRootId);
    internal bool OrderedPrintedSkillLossSkillsChangedEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OrderedPrintedSkillLossSkillsChangedEdge(parent, child);
    internal bool OrderedPrintedSkillLossStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OrderedPrintedSkillLossStructuralEdge(parent, child);
    internal bool TryAdvanceOrderedPrintedSkillLossSubtree() => _resolve().TryAdvanceOrderedPrintedSkillLossSubtree();
    internal bool ValidOrderedPrintedSkillLossLease(OrderedPrintedSkillLossLease lease) => _resolve().ValidOrderedPrintedSkillLossLease(lease);
    internal bool ValidOrderedPrintedSkillLossReceipt(ProgramSkillFrame f) => _resolve().ValidOrderedPrintedSkillLossReceipt(f);
}
