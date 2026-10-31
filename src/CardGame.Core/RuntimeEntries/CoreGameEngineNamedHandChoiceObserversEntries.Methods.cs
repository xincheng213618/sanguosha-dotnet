#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedHandChoiceObserversEntries
{
    internal bool HasNamedHandChoiceDamageObserver(long id) => _resolve().HasNamedHandChoiceDamageObserver(id);
    internal bool IsNamedHandChoiceDying() => _resolve().IsNamedHandChoiceDying();
    internal bool IsNamedHandChoiceMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsNamedHandChoiceMovement(f, effect, movement);
    internal bool NamedHandChoiceFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().NamedHandChoiceFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? NamedHandChoiceObserverRoot() => _resolve().NamedHandChoiceObserverRoot();
    internal bool TryAdvanceNamedHandChoiceSubtree() => _resolve().TryAdvanceNamedHandChoiceSubtree();
}
