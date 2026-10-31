#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenHandRosterObserversEntries
{
    internal bool CanContinueFrozenHandRoster(ProgramSkillFrame f) => _resolve().CanContinueFrozenHandRoster(f);
    internal bool FrozenHandRosterFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().FrozenHandRosterFirstChild(f, child);
    internal ProgramSkillFrame? FrozenHandRosterObserverRoot() => _resolve().FrozenHandRosterObserverRoot();
    internal bool TryAdvanceFrozenHandRosterSubtree() => _resolve().TryAdvanceFrozenHandRosterSubtree();
}
