#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionObserversEntries
{
    internal bool AdvanceRandomAreaChildren() => _resolve().AdvanceRandomAreaChildren();
    internal bool IsRandomAreaRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsRandomAreaRecoveryParent(frame, recovery);
    internal bool RandomAreaFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().RandomAreaFirstChild(root, child);
    internal bool RandomAreaObserverEdge(int index) => _resolve().RandomAreaObserverEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? RandomAreaObserverRoot() => _resolve().RandomAreaObserverRoot();
}
