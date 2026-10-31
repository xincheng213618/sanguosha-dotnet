#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramHandLimitKindExemptionsEntries
{
    internal global::CardGame.Core.ShanheMarketDiscardQuote? CaptureShanheMarketDiscardQuote() => _resolve().CaptureShanheMarketDiscardQuote();
    internal global::System.Collections.Generic.IReadOnlySet<global::CardGame.Core.CardKind> GetCurrentHandLimitExemptKinds(global::CardGame.Core.CharacterState owner) => _resolve().GetCurrentHandLimitExemptKinds(owner);
    internal bool RefreshShanheMarketDiscardPrompt(global::CardGame.Core.ShanheMarketDiscardQuote? quote) => _resolve().RefreshShanheMarketDiscardPrompt(quote);
}
