#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime))]
internal sealed partial class CoreNativeQilinBowEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> _resolve;
    private CoreNativeQilinBowEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeQilinBowEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeQilinBowRuntime> resolve) => new(resolve);
}
