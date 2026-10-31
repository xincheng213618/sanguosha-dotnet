#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealObserversEntries
{
    internal bool CompletedUndamagedTargetRevealFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().CompletedUndamagedTargetRevealFirstChild(f, child);
    internal bool CompletedUndamagedTargetRevealObserverEdge(int child) => _resolve().CompletedUndamagedTargetRevealObserverEdge(child);
    internal ProgramSkillFrame? CompletedUndamagedTargetRevealObserverRoot() => _resolve().CompletedUndamagedTargetRevealObserverRoot();
    internal bool CompletedUndamagedTargetRevealStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().CompletedUndamagedTargetRevealStructuralEdge(parent, child);
    internal bool ExactCompletedUndamagedParent(ProgramSkillFrame f, ProgramSkillWindowContext context, CardUseFrame use) => _resolve().ExactCompletedUndamagedParent(f, context, use);
    internal bool IsCompletedUndamagedTargetRevealDying() => _resolve().IsCompletedUndamagedTargetRevealDying();
    internal bool TryAdvanceCompletedUndamagedTargetRevealSubtree() => _resolve().TryAdvanceCompletedUndamagedTargetRevealSubtree();
    internal bool ValidCompletedUndamagedTargetRevealReceipt(ProgramSkillFrame f) => _resolve().ValidCompletedUndamagedTargetRevealReceipt(f);
}
