#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedUndamagedTargetRevealObserversEntries
{
    internal bool CompletedUndamagedTargetRevealFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().CompletedUndamagedTargetRevealFirstChild(f, child);
    internal bool CompletedUndamagedTargetRevealObserverEdge(int child) => _resolve().CompletedUndamagedTargetRevealObserverEdge(child);
    internal global::CardGame.Core.ProgramSkillFrame? CompletedUndamagedTargetRevealObserverRoot() => _resolve().CompletedUndamagedTargetRevealObserverRoot();
    internal bool CompletedUndamagedTargetRevealStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().CompletedUndamagedTargetRevealStructuralEdge(parent, child);
    internal bool ExactCompletedUndamagedParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.CardUseFrame use) => _resolve().ExactCompletedUndamagedParent(f, context, use);
    internal bool IsCompletedUndamagedTargetRevealDying() => _resolve().IsCompletedUndamagedTargetRevealDying();
    internal bool TryAdvanceCompletedUndamagedTargetRevealSubtree() => _resolve().TryAdvanceCompletedUndamagedTargetRevealSubtree();
    internal bool ValidCompletedUndamagedTargetRevealReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidCompletedUndamagedTargetRevealReceipt(f);
}
