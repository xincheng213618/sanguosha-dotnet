#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSimultaneousPindianObserversEntries
{
    internal bool IsSimultaneousPindianMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsSimultaneousPindianMovement(f, e, p);
    internal bool SimultaneousFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().SimultaneousFirstChild(root, child);
    internal global::System.Collections.Generic.IReadOnlyList<int> SimultaneousHeldProcessingCards() => _resolve().SimultaneousHeldProcessingCards();
    internal bool SimultaneousMovementMatches(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().SimultaneousMovementMatches(f, m);
    internal global::CardGame.Core.ProgramSkillFrame? SimultaneousObserverRoot() => _resolve().SimultaneousObserverRoot();
    internal bool TryAdvanceSimultaneousSubtree() => _resolve().TryAdvanceSimultaneousSubtree();
}
