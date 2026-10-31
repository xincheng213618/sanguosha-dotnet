#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed partial class CoreGameEngineDragonBoatCardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _resolve;
    private CoreGameEngineDragonBoatCardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatCardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatCardsRuntime> resolve) => new(resolve);
}
