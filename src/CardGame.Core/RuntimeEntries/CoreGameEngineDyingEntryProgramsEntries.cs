#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime))]
internal sealed partial class CoreGameEngineDyingEntryProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> _resolve;
    private CoreGameEngineDyingEntryProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDyingEntryProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDyingEntryProgramsRuntime> resolve) => new(resolve);
}
