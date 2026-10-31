#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceDingxiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime> _resolve;
    private CoreGameEngineSourceDingxiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceDingxiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceDingxiRuntime> resolve) => new(resolve);
}
