#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBlackGiftContestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> _resolve;
    private CoreGameEngineBlackGiftContestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBlackGiftContestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBlackGiftContestsRuntime> resolve) => new(resolve);
}
