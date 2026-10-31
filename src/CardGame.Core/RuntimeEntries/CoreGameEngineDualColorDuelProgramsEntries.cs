#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime))]
internal sealed partial class CoreGameEngineDualColorDuelProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> _resolve;
    private CoreGameEngineDualColorDuelProgramsEntries(Func<EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDualColorDuelProgramsEntries Create(Func<EngineOwners.CoreGameEngineDualColorDuelProgramsRuntime> resolve) => new(resolve);
}
