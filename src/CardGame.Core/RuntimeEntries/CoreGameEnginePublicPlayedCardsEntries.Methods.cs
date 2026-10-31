#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPlayedCardsEntries
{
    internal IGameEvent CapturePublicPlayedCard(IGameEvent payload) => _resolve().CapturePublicPlayedCard(payload);
}
