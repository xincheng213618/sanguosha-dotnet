#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOverflowTargetCancellationObserversEntries
{
    internal bool HasOverflowAcceptedPrefix(CardUseFrame use, OverflowUseTargetsCanceledEvent fact, IReadOnlyList<IGameEvent> history, int boundary) => _resolve().HasOverflowAcceptedPrefix(use, fact, history, boundary);
    internal bool IsOverflowTargetCancellationDying() => _resolve().IsOverflowTargetCancellationDying();
    internal bool IsOverflowTargetCancellationFact(CardUseFrame use, OverflowUseTargetsCanceledEvent fact, IReadOnlyList<int> beforeTargets) => _resolve().IsOverflowTargetCancellationFact(use, fact, beforeTargets);
    internal bool OverflowTargetCancellationFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().OverflowTargetCancellationFirstChild(frame, child);
    internal bool OverflowTargetCancellationObserverEdge(int child) => _resolve().OverflowTargetCancellationObserverEdge(child);
    internal ProgramSkillFrame? OverflowTargetCancellationObserverRoot() => _resolve().OverflowTargetCancellationObserverRoot();
    internal bool OverflowTargetCancellationStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().OverflowTargetCancellationStructuralEdge(parent, child);
    internal bool TryAdvanceOverflowTargetCancellationSubtree() => _resolve().TryAdvanceOverflowTargetCancellationSubtree();
    internal bool ValidOverflowTargetCancellationReceipt(ProgramSkillFrame frame) => _resolve().ValidOverflowTargetCancellationReceipt(frame);
}
