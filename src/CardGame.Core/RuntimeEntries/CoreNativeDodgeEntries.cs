#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDodgeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> _resolve;
    private CoreNativeDodgeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDodgeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDodgeRuntime> resolve) => new(resolve);
}
