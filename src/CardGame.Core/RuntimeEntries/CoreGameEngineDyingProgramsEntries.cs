#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingProgramsRuntime))]
internal sealed partial class CoreGameEngineDyingProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingProgramsRuntime> _resolve;
    private CoreGameEngineDyingProgramsEntries(Func<EngineOwners.CoreGameEngineDyingProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingProgramsEntries Create(Func<EngineOwners.CoreGameEngineDyingProgramsRuntime> resolve) => new(resolve);
}
