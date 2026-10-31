#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianObserversEntries
{
    internal bool IsSimultaneousPindianMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().IsSimultaneousPindianMovement(f, e, p);
    internal bool SimultaneousFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().SimultaneousFirstChild(root, child);
    internal IReadOnlyList<int> SimultaneousHeldProcessingCards() => _resolve().SimultaneousHeldProcessingCards();
    internal bool SimultaneousMovementMatches(ProgramSkillFrame f, CardMovementRecord m) => _resolve().SimultaneousMovementMatches(f, m);
    internal ProgramSkillFrame? SimultaneousObserverRoot() => _resolve().SimultaneousObserverRoot();
    internal bool TryAdvanceSimultaneousSubtree() => _resolve().TryAdvanceSimultaneousSubtree();
}
