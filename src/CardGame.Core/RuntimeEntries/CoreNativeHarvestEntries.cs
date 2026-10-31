#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeHarvestRuntime))]
internal sealed partial class CoreNativeHarvestEntries
{
    private readonly Func<EngineOwners.CoreNativeHarvestRuntime> _resolve;
    private CoreNativeHarvestEntries(Func<EngineOwners.CoreNativeHarvestRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeHarvestEntries Create(Func<EngineOwners.CoreNativeHarvestRuntime> resolve) => new(resolve);
}
