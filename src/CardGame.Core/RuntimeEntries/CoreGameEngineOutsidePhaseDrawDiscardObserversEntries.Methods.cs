#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOutsidePhaseDrawDiscardObserversEntries
{
    internal bool IsOutsidePhaseDrawDiscardDying() => _resolve().IsOutsidePhaseDrawDiscardDying();
    internal bool OutsidePhaseDrawDiscardFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().OutsidePhaseDrawDiscardFirstChild(f, child);
    internal ProgramSkillFrame? OutsidePhaseDrawDiscardObserverRoot() => _resolve().OutsidePhaseDrawDiscardObserverRoot();
    internal bool OutsidePhaseDrawDiscardStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OutsidePhaseDrawDiscardStructuralEdge(parent, child);
    internal bool TryAdvanceOutsidePhaseDrawDiscardSubtree() => _resolve().TryAdvanceOutsidePhaseDrawDiscardSubtree();
    internal bool ValidOutsidePhaseDrawDiscardReceipt(ProgramSkillFrame f) => _resolve().ValidOutsidePhaseDrawDiscardReceipt(f);
}
