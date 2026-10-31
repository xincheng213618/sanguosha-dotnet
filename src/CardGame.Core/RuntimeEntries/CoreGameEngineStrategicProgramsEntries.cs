#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineStrategicProgramsRuntime))]
internal sealed partial class CoreGameEngineStrategicProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineStrategicProgramsRuntime> _resolve;
    private CoreGameEngineStrategicProgramsEntries(Func<EngineOwners.CoreGameEngineStrategicProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineStrategicProgramsEntries Create(Func<EngineOwners.CoreGameEngineStrategicProgramsRuntime> resolve) => new(resolve);
}
