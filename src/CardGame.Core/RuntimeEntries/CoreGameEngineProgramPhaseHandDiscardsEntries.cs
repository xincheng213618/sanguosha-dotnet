#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime))]
internal sealed partial class CoreGameEngineProgramPhaseHandDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> _resolve;
    private CoreGameEngineProgramPhaseHandDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPhaseHandDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseHandDiscardsRuntime> resolve) => new(resolve);
}
