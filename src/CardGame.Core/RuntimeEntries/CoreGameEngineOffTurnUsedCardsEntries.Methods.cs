#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOffTurnUsedCardsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOffTurnUsedCardGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginOffTurnUsedCardGift(f);
    internal bool CanOfferOffTurnUsedCards(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferOffTurnUsedCards(c, context);
    internal void CaptureMovementBatchFactsCaptureOffTurnUsedCards(global::CardGame.Core.CardMovementBatchBuilder batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureMovementBatchFactsCaptureOffTurnUsedCards(batch, movements);
    internal void CaptureOffTurnResponseUseOrigin(long owningUseId, global::CardGame.Core.CardActionContext action) => _resolve().CaptureOffTurnResponseUseOrigin(owningUseId, action);
    internal void CaptureOffTurnResponseUsedCards(global::CardGame.Core.CardActionContext action) => _resolve().CaptureOffTurnResponseUsedCards(action);
    internal void CaptureOffTurnUsedAction(global::CardGame.Core.CardActionContext action, long useId, global::CardGame.Core.OffTurnUsedCardKind kind, long anchorBatchId, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> anchorMovements) => _resolve().CaptureOffTurnUsedAction(action, useId, kind, anchorBatchId, anchorMovements);
    internal void CaptureOffTurnUsedCards(long batchId, int turnNumber, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord> movements) => _resolve().CaptureOffTurnUsedCards(batchId, turnNumber, movements);
    internal bool HasOffTurnUsedCardGiftSource(global::CardGame.Core.CharacterState owner) => _resolve().HasOffTurnUsedCardGiftSource(owner);
    internal int[] MatchingOffTurnUsedCardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().MatchingOffTurnUsedCardIndexes(batch, c);
    internal int? OffTurnGiftActualTurnOwner() => _resolve().OffTurnGiftActualTurnOwner();
    internal int[] OffTurnGiftRecipients(int owner) => _resolve().OffTurnGiftRecipients(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OffTurnUsedCardGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().OffTurnUsedCardGiftChoices(f);
    internal bool OffTurnUsedEntityStillAvailable(global::CardGame.Core.OffTurnUsedDiscardEntity e) => _resolve().OffTurnUsedEntityStillAvailable(e);
    internal global::CardGame.Core.OffTurnUsedMaterialDiscardedEvent[] OffTurnUsedFacts(long batchId, global::CardGame.Core.CardConversionSource source, string hash) => _resolve().OffTurnUsedFacts(batchId, source, hash);
    internal void ResolveOffTurnUsedCardGift(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveOffTurnUsedCardGift(selected);
    internal bool ResumeOffTurnUsedCardGift(long id) => _resolve().ResumeOffTurnUsedCardGift(id);
    internal bool ReturnOffTurnUsedCardGiftMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnOffTurnUsedCardGiftMovement(f);
    internal int[]? SelectWindowIndexes_MatchingOffTurnUsedCardIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().SelectWindowIndexes_MatchingOffTurnUsedCardIndexes(batch, candidate, trigger);
    internal bool TryGetOffTurnResponseUse(global::CardGame.Core.CardActionContext action, scoped out long useId, scoped out global::CardGame.Core.OffTurnUsedCardKind kind) => _resolve().TryGetOffTurnResponseUse(action, out useId, out kind);
    internal bool TracksOffTurnUsedCards { get => _resolve().TracksOffTurnUsedCards; }
}
