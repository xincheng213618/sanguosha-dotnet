#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShownBoundGiftsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShownBoundGiftsRuntime> _resolve;
    private CoreGameEngineShownBoundGiftsEntries(Func<EngineOwners.CoreGameEngineShownBoundGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShownBoundGiftsEntries Create(Func<EngineOwners.CoreGameEngineShownBoundGiftsRuntime> resolve) => new(resolve);
}
