#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceDingxiNativeEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> _resolve;
    private CoreGameEngineSourceDingxiNativeEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceDingxiNativeEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiNativeRuntime> resolve) => new(resolve);
}
