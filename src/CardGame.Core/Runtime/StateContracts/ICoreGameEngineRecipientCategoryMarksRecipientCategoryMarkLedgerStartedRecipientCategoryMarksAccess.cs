#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref bool _recipientCategoryMarkLedgerStarted { get; }

    ref readonly global::System.Collections.Generic.Dictionary<global::CardGame.Core.GameEngine.RecipientCategoryMarkKey, global::CardGame.Core.RecipientCategoryMarkToken> _recipientCategoryMarks { get; }
}
