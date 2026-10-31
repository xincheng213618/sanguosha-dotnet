#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandLimitKindExemptionsEntries
{
    internal ShanheMarketDiscardQuote? CaptureShanheMarketDiscardQuote() => _resolve().CaptureShanheMarketDiscardQuote();
    internal IReadOnlySet<CardKind> GetCurrentHandLimitExemptKinds(CharacterState owner) => _resolve().GetCurrentHandLimitExemptKinds(owner);
    internal bool RefreshShanheMarketDiscardPrompt(ShanheMarketDiscardQuote? quote) => _resolve().RefreshShanheMarketDiscardPrompt(quote);
}
