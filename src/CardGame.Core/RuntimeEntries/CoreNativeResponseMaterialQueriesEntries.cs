#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeResponseMaterialQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _resolve;
    private CoreNativeResponseMaterialQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeResponseMaterialQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeResponseMaterialQueriesRuntime> resolve) => new(resolve);
}
