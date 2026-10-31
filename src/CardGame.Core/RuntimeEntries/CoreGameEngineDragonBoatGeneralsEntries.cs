#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime))]
internal sealed partial class CoreGameEngineDragonBoatGeneralsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> _resolve;
    private CoreGameEngineDragonBoatGeneralsEntries(Func<EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatGeneralsEntries Create(Func<EngineOwners.CoreGameEngineDragonBoatGeneralsRuntime> resolve) => new(resolve);
}
