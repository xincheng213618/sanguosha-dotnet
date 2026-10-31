#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeathProgramsRuntime))]
internal sealed partial class CoreGameEngineDeathProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeathProgramsRuntime> _resolve;
    private CoreGameEngineDeathProgramsEntries(Func<EngineOwners.CoreGameEngineDeathProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeathProgramsEntries Create(Func<EngineOwners.CoreGameEngineDeathProgramsRuntime> resolve) => new(resolve);
}
