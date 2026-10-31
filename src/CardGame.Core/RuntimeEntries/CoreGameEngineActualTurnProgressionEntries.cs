#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualTurnProgressionRuntime))]
internal sealed partial class CoreGameEngineActualTurnProgressionEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualTurnProgressionRuntime> _resolve;
    private CoreGameEngineActualTurnProgressionEntries(Func<EngineOwners.CoreGameEngineActualTurnProgressionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualTurnProgressionEntries Create(Func<EngineOwners.CoreGameEngineActualTurnProgressionRuntime> resolve) => new(resolve);
}
