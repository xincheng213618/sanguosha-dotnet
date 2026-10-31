#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineForeignTurnContestsRuntime))]
internal sealed partial class CoreGameEngineForeignTurnContestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineForeignTurnContestsRuntime> _resolve;
    private CoreGameEngineForeignTurnContestsEntries(Func<EngineOwners.CoreGameEngineForeignTurnContestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForeignTurnContestsEntries Create(Func<EngineOwners.CoreGameEngineForeignTurnContestsRuntime> resolve) => new(resolve);
}
