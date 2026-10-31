#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRevealsEntries
{
    internal void CommitProgramHandReveal(long frameId, int holderSeat, string resultBind, IReadOnlyList<int> cardIds) => _resolve().CommitProgramHandReveal(frameId, holderSeat, resultBind, cardIds);
    internal void PublishProgramRevealCardSelection(ProgramSkillFrame frame) => _resolve().PublishProgramRevealCardSelection(frame);
    internal void ResolveProgramRevealCardSelection(PromptChoice selected) => _resolve().ResolveProgramRevealCardSelection(selected);
    internal SkillProgramStepOutcome RevealProgramTargetHandCard(ProgramSkillFrame frame, ProgramParticipantReference chooserRef, ProgramParticipantReference cardOwnerRef, string resultBind, SkillProgramRevealMode mode, IReadOnlyList<Suit> eligibleSuits, bool allowDecline) => _resolve().RevealProgramTargetHandCard(frame, chooserRef, cardOwnerRef, resultBind, mode, eligibleSuits, allowDecline);
}
