#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime))]
internal sealed partial class CoreNativeDyingFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime> _resolve;
    private CoreNativeDyingFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDyingFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDyingFramesRuntime> resolve) => new(resolve);
}
