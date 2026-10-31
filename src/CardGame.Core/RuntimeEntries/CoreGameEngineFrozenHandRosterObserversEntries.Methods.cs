#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenHandRosterObserversEntries
{
    internal bool CanContinueFrozenHandRoster(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueFrozenHandRoster(f);
    internal bool FrozenHandRosterFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().FrozenHandRosterFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? FrozenHandRosterObserverRoot() => _resolve().FrozenHandRosterObserverRoot();
    internal bool TryAdvanceFrozenHandRosterSubtree() => _resolve().TryAdvanceFrozenHandRosterSubtree();
}
