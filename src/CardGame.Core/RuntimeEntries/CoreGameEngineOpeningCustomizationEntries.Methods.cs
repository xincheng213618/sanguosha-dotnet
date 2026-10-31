#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOpeningCustomizationEntries
{
    internal bool IsFixedShanheOpeningCard(Card card) => _resolve().IsFixedShanheOpeningCard(card);
    internal IEnumerable<Card> OpeningReplacementCards() => _resolve().OpeningReplacementCards();
    internal void ReplaceOpeningCard(PromptChoice selected) => _resolve().ReplaceOpeningCard(selected);
    internal void RequestOpeningHandConfirmation() => _resolve().RequestOpeningHandConfirmation();
    internal void ResolveOpeningHandChoice(PromptChoice selected) => _resolve().ResolveOpeningHandChoice(selected);
    internal CommandResult SubmitGeneralRoster(RequestGeneralRosterCommand command) => _resolve().SubmitGeneralRoster(command);
}
