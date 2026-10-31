#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionObserversEntries
{
    internal bool AdvanceRandomAreaChildren() => _resolve().AdvanceRandomAreaChildren();
    internal bool IsRandomAreaRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsRandomAreaRecoveryParent(frame, recovery);
    internal bool RandomAreaFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().RandomAreaFirstChild(root, child);
    internal bool RandomAreaObserverEdge(int index) => _resolve().RandomAreaObserverEdge(index);
    internal ProgramSkillFrame? RandomAreaObserverRoot() => _resolve().RandomAreaObserverRoot();
}
