#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime))]
internal sealed partial class CoreGameEngineDragonBoatGeneralsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> _resolve;
    private CoreGameEngineDragonBoatGeneralsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatGeneralsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> resolve) => new(resolve);
}
