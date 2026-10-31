#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePhaseStartCardControlObserversEntries
{
    internal bool HasPhaseStartCardControlDamageObserver(long id) => _resolve().HasPhaseStartCardControlDamageObserver(id);
    internal bool IsPhaseStartCardControlDying() => _resolve().IsPhaseStartCardControlDying();
    internal bool IsPhaseStartCardControlMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPhaseStartCardControlMovement(f, effect, pending);
    internal bool PhaseStartCardControlFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().PhaseStartCardControlFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? PhaseStartCardControlObserverRoot() => _resolve().PhaseStartCardControlObserverRoot();
    internal bool TryAdvancePhaseStartCardControlSubtree() => _resolve().TryAdvancePhaseStartCardControlSubtree();
}
