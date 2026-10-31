#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceKeshouEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceKeshouRuntime> _resolve;
    private CoreGameEngineShanheSourceKeshouEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceKeshouRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceKeshouEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceKeshouRuntime> resolve) => new(resolve);
}
