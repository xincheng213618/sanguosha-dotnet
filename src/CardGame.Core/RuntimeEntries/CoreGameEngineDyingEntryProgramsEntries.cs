#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDyingEntryProgramsRuntime))]
internal sealed partial class CoreGameEngineDyingEntryProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> _resolve;
    private CoreGameEngineDyingEntryProgramsEntries(Func<EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingEntryProgramsEntries Create(Func<EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> resolve) => new(resolve);
}
