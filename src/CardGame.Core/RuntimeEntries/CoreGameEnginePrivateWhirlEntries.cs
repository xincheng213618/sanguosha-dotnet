#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrivateWhirlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateWhirlRuntime> _resolve;
    private CoreGameEnginePrivateWhirlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateWhirlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateWhirlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePrivateWhirlRuntime> resolve) => new(resolve);
}
