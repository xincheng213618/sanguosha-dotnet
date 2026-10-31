#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualHandGainObserversEntries
{
    internal bool ActualHandGainFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().ActualHandGainFirstChild(root, child);
    internal ProgramSkillFrame? ActualHandGainObserverRoot() => _resolve().ActualHandGainObserverRoot();
    internal bool IsActualHandGainProgramDying() => _resolve().IsActualHandGainProgramDying();
    internal bool TryAdvanceActualHandGainSubtree() => _resolve().TryAdvanceActualHandGainSubtree();
    internal bool ValidActualHandGain(ProgramSkillFrame f) => _resolve().ValidActualHandGain(f);
}
