#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetGiftsEntries
{
    internal SkillProgramStepOutcome BeginFinalTargetGift(ProgramSkillFrame frame) => _resolve().BeginFinalTargetGift(frame);
    internal bool CanOfferFinalTargetGift(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramInstructionFeatures features) => _resolve().CanOfferFinalTargetGift(candidate, context, features);
    internal IReadOnlyList<PromptChoice> FinalTargetGiftChoices(ProgramSkillFrame frame) => _resolve().FinalTargetGiftChoices(frame);
    internal IReadOnlyList<int> FinalTargetGiftSeats(CardUseFrame use) => _resolve().FinalTargetGiftSeats(use);
    internal CardUseFrame FinalTargetGiftUse(ProgramSkillFrame frame) => _resolve().FinalTargetGiftUse(frame);
    internal void ResolveFinalTargetGiftChoice(PromptChoice selected) => _resolve().ResolveFinalTargetGiftChoice(selected);
    internal bool ResumeFinalTargetGift(long frameId) => _resolve().ResumeFinalTargetGift(frameId);
}
