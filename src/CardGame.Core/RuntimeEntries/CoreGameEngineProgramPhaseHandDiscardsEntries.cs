#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime))]
internal sealed partial class CoreGameEngineProgramPhaseHandDiscardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> _resolve;
    private CoreGameEngineProgramPhaseHandDiscardsEntries(Func<EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPhaseHandDiscardsEntries Create(Func<EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> resolve) => new(resolve);
}
