#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedCardGiftsEntries
{
    internal void CompleteProgramCardGift(ProgramSkillFrame frame) => _resolve().CompleteProgramCardGift(frame);
    internal IReadOnlyList<PromptChoice> CompletedCardGiftChoices(ProgramSkillFrame frame) => _resolve().CompletedCardGiftChoices(frame);
    internal CardActionContext CompletedGiftAction(ProgramSkillFrame frame) => _resolve().CompletedGiftAction(frame);
    internal int[] CompletedGiftSlashTargets(ProgramSkillFrame frame, int actorSeat) => _resolve().CompletedGiftSlashTargets(frame, actorSeat);
    internal IReadOnlyList<CardActionContext> CompletedGiftSources(ProgramSkillFrame frame) => _resolve().CompletedGiftSources(frame);
    internal SkillProgramStepOutcome OfferProgramCompletedCardGift(ProgramSkillFrame frame) => _resolve().OfferProgramCompletedCardGift(frame);
    internal void PublishCompletedCardGiftPrompt(ProgramSkillFrame frame) => _resolve().PublishCompletedCardGiftPrompt(frame);
    internal void ResolveCompletedCardGiftChoice(PromptChoice selected) => _resolve().ResolveCompletedCardGiftChoice(selected);
}
