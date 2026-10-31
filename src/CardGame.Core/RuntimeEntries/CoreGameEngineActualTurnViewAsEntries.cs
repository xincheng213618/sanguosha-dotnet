#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualTurnViewAsRuntime))]
internal sealed partial class CoreGameEngineActualTurnViewAsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualTurnViewAsRuntime> _resolve;
    private CoreGameEngineActualTurnViewAsEntries(Func<EngineOwners.CoreGameEngineActualTurnViewAsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnViewAsEntries Create(Func<EngineOwners.CoreGameEngineActualTurnViewAsRuntime> resolve) => new(resolve);
}
