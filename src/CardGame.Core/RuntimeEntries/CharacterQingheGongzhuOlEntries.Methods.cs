#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQingheGongzhuOlEntries
{
    internal double ActualDodgeZengouHpCostForAi(CharacterState owner) => _resolve().ActualDodgeZengouHpCostForAi(owner);
    internal SkillProgramStepOutcome ChangjiProgramEndingDamageChoice(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ChangjiProgramEndingDamageChoice(frame, effect);
    internal (ProgramAiEstimate Estimate, int? TargetSeat) EstimateActualDodgeZengouForAi(CharacterState owner, ProgramSkillWindowContext? context) => _resolve().EstimateActualDodgeZengouForAi(owner, context);
    internal Card? LatestSettledDodgeEntity() => _resolve().LatestSettledDodgeEntity();
    internal IReadOnlyList<Card> NonBasicOwnedCards(int ownerSeat) => _resolve().NonBasicOwnedCards(ownerSeat);
    internal void PresentChangjiDiscardPrompt(ProgramSkillFrame frame) => _resolve().PresentChangjiDiscardPrompt(frame);
    internal void ResolveChangjiDiscardChoice(PromptChoice selected) => _resolve().ResolveChangjiDiscardChoice(selected);
    internal void ResolveChangjiEndingChoice(PromptChoice selected) => _resolve().ResolveChangjiEndingChoice(selected);
    internal void ResolveZengouCostChoice(PromptChoice selected) => _resolve().ResolveZengouCostChoice(selected);
    internal SkillProgramStepOutcome ZengouProgramNullifyDodge(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ZengouProgramNullifyDodge(frame, effect);
}
