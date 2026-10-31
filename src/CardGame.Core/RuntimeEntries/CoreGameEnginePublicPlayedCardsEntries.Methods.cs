#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPlayedCardsEntries
{
    internal global::CardGame.Core.IGameEvent CapturePublicPlayedCard(global::CardGame.Core.IGameEvent payload) => _resolve().CapturePublicPlayedCard(payload);
}
