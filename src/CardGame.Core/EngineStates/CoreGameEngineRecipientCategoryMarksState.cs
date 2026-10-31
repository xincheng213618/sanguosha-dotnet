#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineRecipientCategoryMarksState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess
{
    private bool _recipientCategoryMarkLedgerStarted = default !;
    internal ref bool ReferenceToRecipientCategoryMarkLedgerStarted => ref _recipientCategoryMarkLedgerStarted;

    // Durable gameplay tokens only. Every unfinished choice/payment belongs to its ProgramSkillFrame.
    private global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.RecipientCategoryMarkKey, global::CardGame.Core.RecipientCategoryMarkToken> _recipientCategoryMarks = new();
    internal ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.RecipientCategoryMarkKey, global::CardGame.Core.RecipientCategoryMarkToken> ReferenceToRecipientCategoryMarks => ref _recipientCategoryMarks;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarkLedgerStarted => ref _recipientCategoryMarkLedgerStarted;

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.RecipientCategoryMarkKey, global::CardGame.Core.RecipientCategoryMarkToken> global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess._recipientCategoryMarks => ref _recipientCategoryMarks;
}
