#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime))]
internal sealed partial class CoreGameEngineSourceDingxiNativeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> _resolve;
    private CoreGameEngineSourceDingxiNativeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceDingxiNativeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> resolve) => new(resolve);
}
