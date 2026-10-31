#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime))]
internal sealed partial class CoreNativeHarvestEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> _resolve;
    private CoreNativeHarvestEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeHarvestEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeHarvestRuntime> resolve) => new(resolve);
}
