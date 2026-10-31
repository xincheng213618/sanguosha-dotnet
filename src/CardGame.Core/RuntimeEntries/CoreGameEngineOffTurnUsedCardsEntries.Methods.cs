#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOffTurnUsedCardsEntries
{
    internal SkillProgramStepOutcome BeginOffTurnUsedCardGift(ProgramSkillFrame f) => _resolve().BeginOffTurnUsedCardGift(f);
    internal bool CanOfferOffTurnUsedCards(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanOfferOffTurnUsedCards(c, context);
    internal void CaptureMovementBatchFactsCaptureOffTurnUsedCards(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureOffTurnUsedCards(batch, movements);
    internal void CaptureOffTurnResponseUseOrigin(long owningUseId, CardActionContext action) => _resolve().CaptureOffTurnResponseUseOrigin(owningUseId, action);
    internal void CaptureOffTurnResponseUsedCards(CardActionContext action) => _resolve().CaptureOffTurnResponseUsedCards(action);
    internal void CaptureOffTurnUsedAction(CardActionContext action, long useId, OffTurnUsedCardKind kind, long anchorBatchId, IReadOnlyList<CardMovementRecord> anchorMovements) => _resolve().CaptureOffTurnUsedAction(action, useId, kind, anchorBatchId, anchorMovements);
    internal void CaptureOffTurnUsedCards(long batchId, int turnNumber, IReadOnlyList<CardMovementRecord> movements) => _resolve().CaptureOffTurnUsedCards(batchId, turnNumber, movements);
    internal bool HasOffTurnUsedCardGiftSource(CharacterState owner) => _resolve().HasOffTurnUsedCardGiftSource(owner);
    internal int[] MatchingOffTurnUsedCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate c) => _resolve().MatchingOffTurnUsedCardIndexes(batch, c);
    internal int? OffTurnGiftActualTurnOwner() => _resolve().OffTurnGiftActualTurnOwner();
    internal int[] OffTurnGiftRecipients(int owner) => _resolve().OffTurnGiftRecipients(owner);
    internal IReadOnlyList<PromptChoice> OffTurnUsedCardGiftChoices(ProgramSkillFrame f) => _resolve().OffTurnUsedCardGiftChoices(f);
    internal bool OffTurnUsedEntityStillAvailable(OffTurnUsedDiscardEntity e) => _resolve().OffTurnUsedEntityStillAvailable(e);
    internal OffTurnUsedMaterialDiscardedEvent[] OffTurnUsedFacts(long batchId, CardConversionSource source, string hash) => _resolve().OffTurnUsedFacts(batchId, source, hash);
    internal void ResolveOffTurnUsedCardGift(PromptChoice selected) => _resolve().ResolveOffTurnUsedCardGift(selected);
    internal bool ResumeOffTurnUsedCardGift(long id) => _resolve().ResumeOffTurnUsedCardGift(id);
    internal bool ReturnOffTurnUsedCardGiftMovement(ProgramSkillFrame f) => _resolve().ReturnOffTurnUsedCardGiftMovement(f);
    internal int[]? SelectWindowIndexes_MatchingOffTurnUsedCardIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOffTurnUsedCardIndexes(batch, candidate, trigger);
    internal bool TryGetOffTurnResponseUse(CardActionContext action, scoped out long useId, scoped out OffTurnUsedCardKind kind) => _resolve().TryGetOffTurnResponseUse(action, out useId, out kind);
    internal bool TracksOffTurnUsedCards { get => _resolve().TracksOffTurnUsedCards; }
}
