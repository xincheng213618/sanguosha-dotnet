#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchingRecastObserversEntries
{
    internal bool IsMatchingRecastChoice(ProgramSkillFrame f, PendingDecision decision) => _resolve().IsMatchingRecastChoice(f, decision);
    internal bool IsMatchingRecastDying() => _resolve().IsMatchingRecastDying();
    internal bool MatchingRecastFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MatchingRecastFirstChild(f, child);
    internal ProgramSkillFrame? MatchingRecastObserverRoot() => _resolve().MatchingRecastObserverRoot();
    internal bool MatchingRecastStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().MatchingRecastStructuralEdge(parent, child);
    internal bool TryAdvanceMatchingRecastSubtree() => _resolve().TryAdvanceMatchingRecastSubtree();
    internal bool ValidMatchingRecastReceipt(ProgramSkillFrame f) => _resolve().ValidMatchingRecastReceipt(f);
}
