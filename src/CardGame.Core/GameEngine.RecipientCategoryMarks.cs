using System.Globalization;

namespace CardGame.Core;
public sealed partial class GameEngine
{
    internal sealed record RecipientCategoryMarkKey(int HolderSeat, string SourceSkillId, string StateId);
    // Durable gameplay tokens only. Every unfinished choice/payment belongs to its ProgramSkillFrame.
    private ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.RecipientCategoryMarkKey, global::CardGame.Core.RecipientCategoryMarkToken> _recipientCategoryMarks => ref _recipientCategoryMarks2.ReferenceToRecipientCategoryMarks;
    private ref bool _recipientCategoryMarkLedgerStarted => ref _recipientCategoryMarks2.ReferenceToRecipientCategoryMarkLedgerStarted;
}
