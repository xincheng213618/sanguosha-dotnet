#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime))]
internal sealed partial class CoreNativeDeathFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> _resolve;
    private CoreNativeDeathFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeDeathFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeDeathFramesRuntime> resolve) => new(resolve);
}
