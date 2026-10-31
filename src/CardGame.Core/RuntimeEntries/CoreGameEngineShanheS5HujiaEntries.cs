#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5HujiaEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5HujiaRuntime> _resolve;
    private CoreGameEngineShanheS5HujiaEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5HujiaRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheS5HujiaEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheS5HujiaRuntime> resolve) => new(resolve);
}
