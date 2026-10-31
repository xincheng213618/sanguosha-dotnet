#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePlayerViewProjectionQueriesEntries
{
    internal GameSnapshot ProjectPlayerView(int viewerSeat, bool revealAll) => _resolve().ProjectPlayerView(viewerSeat, revealAll);
}
