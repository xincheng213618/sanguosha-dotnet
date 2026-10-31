#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheChilianEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChilianRuntime> _resolve;
    private CoreGameEngineShanheChilianEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChilianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheChilianEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheChilianRuntime> resolve) => new(resolve);
}
