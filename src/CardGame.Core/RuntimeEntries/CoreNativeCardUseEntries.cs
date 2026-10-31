#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime))]
internal sealed partial class CoreNativeCardUseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> _resolve;
    private CoreNativeCardUseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardUseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseRuntime> resolve) => new(resolve);
}
