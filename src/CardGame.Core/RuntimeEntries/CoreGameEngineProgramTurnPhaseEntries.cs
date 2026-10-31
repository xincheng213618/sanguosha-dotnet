#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramTurnPhaseRuntime))]
internal sealed partial class CoreGameEngineProgramTurnPhaseEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramTurnPhaseRuntime> _resolve;
    private CoreGameEngineProgramTurnPhaseEntries(Func<EngineOwners.CoreGameEngineProgramTurnPhaseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramTurnPhaseEntries Create(Func<EngineOwners.CoreGameEngineProgramTurnPhaseRuntime> resolve) => new(resolve);
}
