#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDeckProgramsRuntime))]
internal sealed partial class CoreGameEngineDeckProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDeckProgramsRuntime> _resolve;
    private CoreGameEngineDeckProgramsEntries(Func<EngineOwners.CoreGameEngineDeckProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDeckProgramsEntries Create(Func<EngineOwners.CoreGameEngineDeckProgramsRuntime> resolve) => new(resolve);
}
