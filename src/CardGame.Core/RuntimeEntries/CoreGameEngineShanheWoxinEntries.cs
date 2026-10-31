#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheWoxinEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> _resolve;
    private CoreGameEngineShanheWoxinEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheWoxinEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheWoxinRuntime> resolve) => new(resolve);
}
