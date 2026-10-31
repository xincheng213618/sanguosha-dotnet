#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardContinuationCursorsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> _resolve;
    private CoreNativeCardContinuationCursorsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardContinuationCursorsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardContinuationCursorsRuntime> resolve) => new(resolve);
}
