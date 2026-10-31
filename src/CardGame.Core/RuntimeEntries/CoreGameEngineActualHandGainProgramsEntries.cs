#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualHandGainProgramsRuntime))]
internal sealed partial class CoreGameEngineActualHandGainProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> _resolve;
    private CoreGameEngineActualHandGainProgramsEntries(Func<EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualHandGainProgramsEntries Create(Func<EngineOwners.CoreGameEngineActualHandGainProgramsRuntime> resolve) => new(resolve);
}
