#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOpeningCustomizationEntries
{
    internal bool IsFixedShanheOpeningCard(global::CardGame.Core.Card card) => _resolve().IsFixedShanheOpeningCard(card);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.Card> OpeningReplacementCards() => _resolve().OpeningReplacementCards();
    internal void ReplaceOpeningCard(global::CardGame.Core.PromptChoice selected) => _resolve().ReplaceOpeningCard(selected);
    internal void RequestOpeningHandConfirmation() => _resolve().RequestOpeningHandConfirmation();
    internal void ResolveOpeningHandChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveOpeningHandChoice(selected);
    internal global::CardGame.Core.CommandResult SubmitGeneralRoster(global::CardGame.Core.RequestGeneralRosterCommand command) => _resolve().SubmitGeneralRoster(command);
}
