#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineConvertingGiftsEntries
{
    internal void AppendGiftRetentionEndingItems(global::System.Collections.Generic.List<global::CardGame.Core.TurnEndingBoundaryItem> items) => _resolve().AppendGiftRetentionEndingItems(items);
    internal void AppendGiftRetentionToActiveEnding(global::CardGame.Core.GiftHandRetentionObligation due) => _resolve().AppendGiftRetentionToActiveEnding(due);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginConvertingGiftDamage(global::CardGame.Core.ProgramSkillFrame f, int limit) => _resolve().BeginConvertingGiftDamage(f, limit);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageSourceObservationGift(global::CardGame.Core.ProgramSkillFrame f, int limit) => _resolve().BeginDamageSourceObservationGift(f, limit);
    internal bool CanActivateConvertingGift(global::CardGame.Core.CharacterState owner, string skillId, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().CanActivateConvertingGift(owner, skillId, features);
    internal bool CanOfferConvertingGift(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().CanOfferConvertingGift(candidate, context, features);
    internal bool ConsumeGiftRetentionEndingItem(global::CardGame.Core.TurnEndingBoundaryFrame frame, global::CardGame.Core.TurnEndingBoundaryItem item) => _resolve().ConsumeGiftRetentionEndingItem(frame, item);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ConvertingGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ConvertingGiftChoices(f);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawProgramToHandCount(global::CardGame.Core.ProgramSkillFrame f, int seat, int limit, bool capAtTargetMaxHp = false) => _resolve().DrawProgramToHandCount(f, seat, limit, capAtTargetMaxHp);
    internal void ExpireGiftRetentionObligations() => _resolve().ExpireGiftRetentionObligations();
    internal void FinishConvertingGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishConvertingGift(f);
    internal global::CardGame.Core.CardSnapshot[] GetConvertingGiftPrivatelyViewedCards(int viewerSeat) => _resolve().GetConvertingGiftPrivatelyViewedCards(viewerSeat);
    internal void MarkConvertingGiftExactDeath(long deathFrameId, int victimSeat) => _resolve().MarkConvertingGiftExactDeath(deathFrameId, victimSeat);
    internal void MoveConvertingGiftCard(global::CardGame.Core.ProgramSkillFrame f, int cardId) => _resolve().MoveConvertingGiftCard(f, cardId);
    internal bool OwnedGiftCard(int owner, int cardId) => _resolve().OwnedGiftCard(owner, cardId);
    internal void PublishConvertingGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishConvertingGift(f);
    internal void ResolveConvertingGiftChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveConvertingGiftChoice(choice);
    internal void ReturnHandCountDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHandCountDraw(supplied, outcome);
    internal global::CardGame.Core.PromptChoice SelectAiConvertingGift(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiConvertingGift(decision, frame);
    internal bool TryResumeConvertingGift(long frameId) => _resolve().TryResumeConvertingGift(frameId);
}
