#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreActualDiscardPhaseEntries
{
    internal void AutoDiscard(CharacterState player) => _resolve().AutoDiscard(player);
    internal void BeginDiscardPhase(bool beforeEntryPolicyCompleted = false) => _resolve().BeginDiscardPhase(beforeEntryPolicyCompleted);
    internal void BeginSkipDiscardPolicyChoice(CharacterState current) => _resolve().BeginSkipDiscardPolicyChoice(current);
    internal void CompleteDiscardPhaseAfterProgramWindow(CharacterState current) => _resolve().CompleteDiscardPhaseAfterProgramWindow(current);
    internal void ResolveSkipDiscardPolicyChoice(bool useSkill) => _resolve().ResolveSkipDiscardPolicyChoice(useSkill);
}
