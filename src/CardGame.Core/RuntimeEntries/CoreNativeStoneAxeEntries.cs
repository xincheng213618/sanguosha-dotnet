#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeStoneAxeRuntime))]
internal sealed partial class CoreNativeStoneAxeEntries
{
    private readonly Func<EngineOwners.CoreNativeStoneAxeRuntime> _resolve;
    private CoreNativeStoneAxeEntries(Func<EngineOwners.CoreNativeStoneAxeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeStoneAxeEntries Create(Func<EngineOwners.CoreNativeStoneAxeRuntime> resolve) => new(resolve);
}
