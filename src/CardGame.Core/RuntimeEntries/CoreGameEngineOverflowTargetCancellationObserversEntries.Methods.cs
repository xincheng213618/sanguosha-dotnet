#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOverflowTargetCancellationObserversEntries
{
    internal bool HasOverflowAcceptedPrefix(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.OverflowUseTargetsCanceledEvent fact, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, int boundary) => _resolve().HasOverflowAcceptedPrefix(use, fact, history, boundary);
    internal bool IsOverflowTargetCancellationDying() => _resolve().IsOverflowTargetCancellationDying();
    internal bool IsOverflowTargetCancellationFact(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.OverflowUseTargetsCanceledEvent fact, global::System.Collections.Generic.IReadOnlyList<int> beforeTargets) => _resolve().IsOverflowTargetCancellationFact(use, fact, beforeTargets);
    internal bool OverflowTargetCancellationFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().OverflowTargetCancellationFirstChild(frame, child);
    internal bool OverflowTargetCancellationObserverEdge(int child) => _resolve().OverflowTargetCancellationObserverEdge(child);
    internal global::CardGame.Core.ProgramSkillFrame? OverflowTargetCancellationObserverRoot() => _resolve().OverflowTargetCancellationObserverRoot();
    internal bool OverflowTargetCancellationStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().OverflowTargetCancellationStructuralEdge(parent, child);
    internal bool TryAdvanceOverflowTargetCancellationSubtree() => _resolve().TryAdvanceOverflowTargetCancellationSubtree();
    internal bool ValidOverflowTargetCancellationReceipt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidOverflowTargetCancellationReceipt(frame);
}
