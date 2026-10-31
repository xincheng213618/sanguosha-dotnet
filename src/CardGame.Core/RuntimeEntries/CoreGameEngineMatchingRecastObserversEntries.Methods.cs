#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchingRecastObserversEntries
{
    internal void AssertMatchingRecast(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMatchingRecast(f);
    internal bool HasMatchingRecastBeforeDamageObserver(long windowId) => _resolve().HasMatchingRecastBeforeDamageObserver(windowId);
    internal bool HasMatchingRecastDamageObserver(long windowId) => _resolve().HasMatchingRecastDamageObserver(windowId);
    internal bool IsMatchingRecastChoice(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PendingDecision decision) => _resolve().IsMatchingRecastChoice(f, decision);
    internal bool IsMatchingRecastDying() => _resolve().IsMatchingRecastDying();
    internal bool MatchingRecastFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MatchingRecastFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? MatchingRecastObserverRoot() => _resolve().MatchingRecastObserverRoot();
    internal bool MatchingRecastStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().MatchingRecastStructuralEdge(parent, child);
    internal bool TryAdvanceMatchingRecastSubtree() => _resolve().TryAdvanceMatchingRecastSubtree();
    internal bool ValidMatchingRecastReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMatchingRecastReceipt(f);
}
