#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDragonBoatCardsRuntime))]
internal sealed partial class CoreGameEngineDragonBoatCardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDragonBoatCardsRuntime> _resolve;
    private CoreGameEngineDragonBoatCardsEntries(Func<EngineOwners.CoreGameEngineDragonBoatCardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDragonBoatCardsEntries Create(Func<EngineOwners.CoreGameEngineDragonBoatCardsRuntime> resolve) => new(resolve);
}
