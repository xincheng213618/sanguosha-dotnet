#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeQilinBowEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> _resolve;
    private CoreNativeQilinBowEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeQilinBowEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> resolve) => new(resolve);
}
