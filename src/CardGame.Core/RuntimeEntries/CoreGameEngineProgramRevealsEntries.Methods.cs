#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRevealsEntries
{
    internal void CommitProgramHandReveal(long frameId, int holderSeat, string resultBind, global::System.Collections.Generic.IReadOnlyList<int> cardIds) => _resolve().CommitProgramHandReveal(frameId, holderSeat, resultBind, cardIds);
    internal void PublishProgramRevealCardSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishProgramRevealCardSelection(frame);
    internal void ResolveProgramRevealCardSelection(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramRevealCardSelection(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome RevealProgramTargetHandCard(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference chooserRef, global::CardGame.Core.ProgramParticipantReference cardOwnerRef, string resultBind, global::CardGame.Core.SkillProgramRevealMode mode, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> eligibleSuits, bool allowDecline) => _resolve().RevealProgramTargetHandCard(frame, chooserRef, cardOwnerRef, resultBind, mode, eligibleSuits, allowDecline);
}
