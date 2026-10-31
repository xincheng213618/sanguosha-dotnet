#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeResponseMaterialQueriesRuntime))]
internal sealed partial class CoreNativeResponseMaterialQueriesEntries
{
    private readonly Func<EngineOwners.CoreNativeResponseMaterialQueriesRuntime> _resolve;
    private CoreNativeResponseMaterialQueriesEntries(Func<EngineOwners.CoreNativeResponseMaterialQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeResponseMaterialQueriesEntries Create(Func<EngineOwners.CoreNativeResponseMaterialQueriesRuntime> resolve) => new(resolve);
}
