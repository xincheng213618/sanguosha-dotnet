#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeHarvestEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _resolve;
    private CoreNativeHarvestEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeHarvestEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> resolve) => new(resolve);
}
