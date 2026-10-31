#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineRecipientCategoryMarksRecipientCategoryMarkLedgerStartedRecipientCategoryMarksAccess : IProgramRuntimeOperations
{
    ref bool _recipientCategoryMarkLedgerStarted { get; }

    ref readonly Dictionary<GameEngine.RecipientCategoryMarkKey, RecipientCategoryMarkToken> _recipientCategoryMarks { get; }
}
