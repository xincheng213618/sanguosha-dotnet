#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualHandGainObserversEntries
{
    internal bool ActualHandGainFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().ActualHandGainFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? ActualHandGainObserverRoot() => _resolve().ActualHandGainObserverRoot();
    internal bool IsActualHandGainProgramDying() => _resolve().IsActualHandGainProgramDying();
    internal bool TryAdvanceActualHandGainSubtree() => _resolve().TryAdvanceActualHandGainSubtree();
    internal bool ValidActualHandGain(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidActualHandGain(f);
}
