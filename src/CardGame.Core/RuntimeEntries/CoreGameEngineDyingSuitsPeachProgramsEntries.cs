#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime))]
internal sealed partial class CoreGameEngineDyingSuitsPeachProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> _resolve;
    private CoreGameEngineDyingSuitsPeachProgramsEntries(Func<EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingSuitsPeachProgramsEntries Create(Func<EngineOwners.CoreGameEngineDyingSuitsPeachProgramsRuntime> resolve) => new(resolve);
}
