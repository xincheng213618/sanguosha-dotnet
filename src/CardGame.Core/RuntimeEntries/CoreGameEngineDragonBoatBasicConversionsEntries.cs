#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDragonBoatBasicConversionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> _resolve;
    private CoreGameEngineDragonBoatBasicConversionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatBasicConversionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> resolve) => new(resolve);
}
