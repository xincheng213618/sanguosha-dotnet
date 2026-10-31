#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardUseCompletionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> _resolve;
    private CoreNativeCardUseCompletionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardUseCompletionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseCompletionRuntime> resolve) => new(resolve);
}
