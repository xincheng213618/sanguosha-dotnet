#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime))]
internal sealed partial class CoreNativeGroupResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> _resolve;
    private CoreNativeGroupResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupResponsesRuntime> resolve) => new(resolve);
}
