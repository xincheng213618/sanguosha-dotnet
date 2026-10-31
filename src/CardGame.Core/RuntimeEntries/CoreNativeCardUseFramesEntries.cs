#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime))]
internal sealed partial class CoreNativeCardUseFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> _resolve;
    private CoreNativeCardUseFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeCardUseFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeCardUseFramesRuntime> resolve) => new(resolve);
}
