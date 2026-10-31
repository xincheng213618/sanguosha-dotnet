#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CorePlayerViewNativeCardSelectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewNativeCardSelectionsRuntime> _resolve;
    private CorePlayerViewNativeCardSelectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewNativeCardSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CorePlayerViewNativeCardSelectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewNativeCardSelectionsRuntime> resolve) => new(resolve);
}
