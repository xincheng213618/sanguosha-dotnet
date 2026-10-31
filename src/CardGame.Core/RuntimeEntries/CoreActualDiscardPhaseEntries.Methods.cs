#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualDiscardPhaseEntries
{
    internal void AutoDiscard(global::CardGame.Core.CharacterState player) => _resolve().AutoDiscard(player);
    internal void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false) => _resolve().BeginDiscardPhase(beforeEntryPolicyCompleted);
    internal void BeginSkipDiscardPolicyChoice(global::CardGame.Core.CharacterState current) => _resolve().BeginSkipDiscardPolicyChoice(current);
    internal void CompleteDiscardPhaseAfterProgramWindow(global::CardGame.Core.CharacterState current) => _resolve().CompleteDiscardPhaseAfterProgramWindow(current);
    internal void ResolveSkipDiscardPolicyChoice(bool useSkill) => _resolve().ResolveSkipDiscardPolicyChoice(useSkill);
}
