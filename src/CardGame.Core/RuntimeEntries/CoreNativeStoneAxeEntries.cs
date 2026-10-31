#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime))]
internal sealed partial class CoreNativeStoneAxeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> _resolve;
    private CoreNativeStoneAxeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeStoneAxeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeStoneAxeRuntime> resolve) => new(resolve);
}
