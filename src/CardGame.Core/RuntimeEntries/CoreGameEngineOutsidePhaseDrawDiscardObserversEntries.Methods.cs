#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardObserversEntries
{
    internal bool IsOutsidePhaseDrawDiscardDying() => _resolve().IsOutsidePhaseDrawDiscardDying();
    internal bool OutsidePhaseDrawDiscardFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().OutsidePhaseDrawDiscardFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? OutsidePhaseDrawDiscardObserverRoot() => _resolve().OutsidePhaseDrawDiscardObserverRoot();
    internal bool OutsidePhaseDrawDiscardStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OutsidePhaseDrawDiscardStructuralEdge(parent, child);
    internal bool TryAdvanceOutsidePhaseDrawDiscardSubtree() => _resolve().TryAdvanceOutsidePhaseDrawDiscardSubtree();
    internal bool ValidOutsidePhaseDrawDiscardReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidOutsidePhaseDrawDiscardReceipt(f);
}
