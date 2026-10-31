#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeActionDispatchEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> _resolve;
    private CoreNativeActionDispatchEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeActionDispatchEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeActionDispatchRuntime> resolve) => new(resolve);
}
