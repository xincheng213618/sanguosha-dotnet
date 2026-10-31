#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime))]
internal sealed partial class CoreNativeWoodenOxEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> _resolve;
    private CoreNativeWoodenOxEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeWoodenOxEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeWoodenOxRuntime> resolve) => new(resolve);
}
