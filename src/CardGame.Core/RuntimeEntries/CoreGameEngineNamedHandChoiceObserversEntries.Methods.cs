#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedHandChoiceObserversEntries
{
    internal bool IsNamedHandChoiceMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => _resolve().IsNamedHandChoiceMovement(f, effect, movement);
    internal bool NamedHandChoiceFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().NamedHandChoiceFirstChild(root, child);
    internal ProgramSkillFrame? NamedHandChoiceObserverRoot() => _resolve().NamedHandChoiceObserverRoot();
    internal bool TryAdvanceNamedHandChoiceSubtree() => _resolve().TryAdvanceNamedHandChoiceSubtree();
}
