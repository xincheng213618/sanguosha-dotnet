#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime))]
internal sealed partial class CoreNativeNullificationFramesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> _resolve;
    private CoreNativeNullificationFramesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeNullificationFramesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime> resolve) => new(resolve);
}
