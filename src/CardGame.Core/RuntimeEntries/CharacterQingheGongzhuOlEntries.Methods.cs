#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQingheGongzhuOlEntries
{
    internal double ActualDodgeZengouHpCostForAi(global::CardGame.Core.CharacterState owner) => _resolve().ActualDodgeZengouHpCostForAi(owner);
    internal global::CardGame.Core.SkillProgramStepOutcome ChangjiProgramEndingDamageChoice(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ChangjiProgramEndingDamageChoice(frame, effect);
    internal (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateActualDodgeZengouForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().EstimateActualDodgeZengouForAi(owner, context);
    internal global::CardGame.Core.Card? LatestSettledDodgeEntity() => _resolve().LatestSettledDodgeEntity();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NonBasicOwnedCards(int ownerSeat) => _resolve().NonBasicOwnedCards(ownerSeat);
    internal void PresentChangjiDiscardPrompt(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PresentChangjiDiscardPrompt(frame);
    internal void ResolveChangjiDiscardChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveChangjiDiscardChoice(selected);
    internal void ResolveChangjiEndingChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveChangjiEndingChoice(selected);
    internal void ResolveZengouCostChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveZengouCostChoice(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome ZengouProgramNullifyDodge(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ZengouProgramNullifyDodge(frame, effect);
}
