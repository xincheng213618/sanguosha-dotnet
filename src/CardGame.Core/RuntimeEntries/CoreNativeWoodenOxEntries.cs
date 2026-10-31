#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeWoodenOxRuntime))]
internal sealed partial class CoreNativeWoodenOxEntries
{
    private readonly Func<EngineOwners.CoreNativeWoodenOxRuntime> _resolve;
    private CoreNativeWoodenOxEntries(Func<EngineOwners.CoreNativeWoodenOxRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeWoodenOxEntries Create(Func<EngineOwners.CoreNativeWoodenOxRuntime> resolve) => new(resolve);
}
