#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRecipientCategoryMarksState : Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess
{
    private bool _recipientCategoryMarkLedgerStarted = default !;
    internal ref bool ReferenceToRecipientCategoryMarkLedgerStarted => ref _recipientCategoryMarkLedgerStarted;

    // Durable gameplay tokens only. Every unfinished choice/payment belongs to its ProgramSkillFrame.
    private Dictionary<GameEngine.RecipientCategoryMarkKey, RecipientCategoryMarkToken> _recipientCategoryMarks = new();
    internal ref readonly Dictionary<GameEngine.RecipientCategoryMarkKey, RecipientCategoryMarkToken> ReferenceToRecipientCategoryMarks => ref _recipientCategoryMarks;

    ref bool Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarkLedgerStarted => ref _recipientCategoryMarkLedgerStarted;

    ref readonly Dictionary<GameEngine.RecipientCategoryMarkKey, RecipientCategoryMarkToken> Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks => ref _recipientCategoryMarks;
}
