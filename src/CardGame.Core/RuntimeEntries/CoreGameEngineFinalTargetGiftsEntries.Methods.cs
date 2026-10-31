#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFinalTargetGiftsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginFinalTargetGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginFinalTargetGift(frame);
    internal bool CanOfferFinalTargetGift(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().CanOfferFinalTargetGift(candidate, context, features);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> FinalTargetGiftChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinalTargetGiftChoices(frame);
    internal global::System.Collections.Generic.IReadOnlyList<int> FinalTargetGiftSeats(global::CardGame.Core.CardUseFrame use) => _resolve().FinalTargetGiftSeats(use);
    internal global::CardGame.Core.CardUseFrame FinalTargetGiftUse(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().FinalTargetGiftUse(frame);
    internal void ResolveFinalTargetGiftChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveFinalTargetGiftChoice(selected);
    internal bool ResumeFinalTargetGift(long frameId) => _resolve().ResumeFinalTargetGift(frameId);
}
