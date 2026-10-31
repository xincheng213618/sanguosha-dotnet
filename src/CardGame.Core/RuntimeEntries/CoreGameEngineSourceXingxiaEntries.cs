#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceXingxiaEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime> _resolve;
    private CoreGameEngineSourceXingxiaEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceXingxiaEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceXingxiaRuntime> resolve) => new(resolve);
}
