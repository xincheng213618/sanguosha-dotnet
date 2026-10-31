#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConvertingGiftsEntries
{
    internal void AppendGiftRetentionEndingItems(List<TurnEndingBoundaryItem> items) => _resolve().AppendGiftRetentionEndingItems(items);
    internal void AppendGiftRetentionToActiveEnding(GiftHandRetentionObligation due) => _resolve().AppendGiftRetentionToActiveEnding(due);
    internal SkillProgramStepOutcome BeginConvertingGiftDamage(ProgramSkillFrame f, int limit) => _resolve().BeginConvertingGiftDamage(f, limit);
    internal SkillProgramStepOutcome BeginDamageSourceObservationGift(ProgramSkillFrame f, int limit) => _resolve().BeginDamageSourceObservationGift(f, limit);
    internal bool CanActivateConvertingGift(CharacterState owner, string skillId, ProgramInstructionFeatures features) => _resolve().CanActivateConvertingGift(owner, skillId, features);
    internal bool CanOfferConvertingGift(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context, ProgramInstructionFeatures features) => _resolve().CanOfferConvertingGift(candidate, context, features);
    internal bool ConsumeGiftRetentionEndingItem(TurnEndingBoundaryFrame frame, TurnEndingBoundaryItem item) => _resolve().ConsumeGiftRetentionEndingItem(frame, item);
    internal IReadOnlyList<PromptChoice> ConvertingGiftChoices(ProgramSkillFrame f) => _resolve().ConvertingGiftChoices(f);
    internal SkillProgramStepOutcome DrawProgramToHandCount(ProgramSkillFrame f, int seat, int limit, bool capAtTargetMaxHp = false) => _resolve().DrawProgramToHandCount(f, seat, limit, capAtTargetMaxHp);
    internal void ExpireGiftRetentionObligations() => _resolve().ExpireGiftRetentionObligations();
    internal void FinishConvertingGift(ProgramSkillFrame f) => _resolve().FinishConvertingGift(f);
    internal CardSnapshot[] GetConvertingGiftPrivatelyViewedCards(int viewerSeat) => _resolve().GetConvertingGiftPrivatelyViewedCards(viewerSeat);
    internal void MarkConvertingGiftExactDeath(long deathFrameId, int victimSeat) => _resolve().MarkConvertingGiftExactDeath(deathFrameId, victimSeat);
    internal void MoveConvertingGiftCard(ProgramSkillFrame f, int cardId) => _resolve().MoveConvertingGiftCard(f, cardId);
    internal bool OwnedGiftCard(int owner, int cardId) => _resolve().OwnedGiftCard(owner, cardId);
    internal void PublishConvertingGift(ProgramSkillFrame f) => _resolve().PublishConvertingGift(f);
    internal void ResolveConvertingGiftChoice(PromptChoice choice) => _resolve().ResolveConvertingGiftChoice(choice);
    internal void ReturnHandCountDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnHandCountDraw(supplied, outcome);
    internal PromptChoice SelectAiConvertingGift(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiConvertingGift(decision, frame);
    internal bool TryResumeConvertingGift(long frameId) => _resolve().TryResumeConvertingGift(frameId);
}
