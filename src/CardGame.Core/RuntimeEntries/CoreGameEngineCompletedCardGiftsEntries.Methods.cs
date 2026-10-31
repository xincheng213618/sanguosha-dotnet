#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCompletedCardGiftsEntries
{
    internal void CompleteProgramCardGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteProgramCardGift(frame);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> CompletedCardGiftChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompletedCardGiftChoices(frame);
    internal global::CardGame.Core.CardActionContext CompletedGiftAction(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompletedGiftAction(frame);
    internal int[] CompletedGiftSlashTargets(global::CardGame.Core.ProgramSkillFrame frame, int actorSeat) => _resolve().CompletedGiftSlashTargets(frame, actorSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardActionContext> CompletedGiftSources(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompletedGiftSources(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferProgramCompletedCardGift(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OfferProgramCompletedCardGift(frame);
    internal void PublishCompletedCardGiftPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishCompletedCardGiftPrompt(frame);
    internal void ResolveCompletedCardGiftChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveCompletedCardGiftChoice(selected);
}
