#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShownBoundGiftsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownBoundGiftsRuntime> _resolve;
    private CoreGameEngineShownBoundGiftsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownBoundGiftsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShownBoundGiftsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShownBoundGiftsRuntime> resolve) => new(resolve);
}
