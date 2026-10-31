#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramTurnPhaseRuntime))]
internal sealed partial class CoreGameEngineProgramTurnPhaseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTurnPhaseRuntime> _resolve;
    private CoreGameEngineProgramTurnPhaseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTurnPhaseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTurnPhaseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramTurnPhaseRuntime> resolve) => new(resolve);
}
