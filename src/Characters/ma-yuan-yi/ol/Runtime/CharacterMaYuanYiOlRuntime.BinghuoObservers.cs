#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaYuanYiOlRuntime
{
    internal bool IsBinghuoHandoffMovement(MaYuanYiReceipt r, CardMovementRecord m) => r.JudgmentHandoffSequence == m.Sequence && m.Sequence <= r.Before && r.JudgmentCardId == m.CardId && r.TargetSeat is { } target && m.From == CardLocation.Judgment(target) && m.To == CardLocation.Processing && m.Reason.Value == "skill-program.judgment.result";
    internal bool IsBinghuoCleanupMovement(MaYuanYiReceipt r, CardMovementRecord m) => r.JudgmentCardId == m.CardId && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.JudgmentFinish && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(p => p.Sequence <= r.Before && p.CardId == m.CardId)is { } entry && IsBinghuoHandoffMovement(r, entry);
}
