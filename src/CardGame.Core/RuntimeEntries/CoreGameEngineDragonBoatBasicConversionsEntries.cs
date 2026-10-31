#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime))]
internal sealed partial class CoreGameEngineDragonBoatBasicConversionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> _resolve;
    private CoreGameEngineDragonBoatBasicConversionsEntries(Func<EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatBasicConversionsEntries Create(Func<EngineOwners.CoreGameEngineDragonBoatBasicConversionsRuntime> resolve) => new(resolve);
}
