#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime))]
internal sealed partial class CoreNativeGroupCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime> _resolve;
    private CoreNativeGroupCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeGroupCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeGroupCompletionRuntime> resolve) => new(resolve);
}
