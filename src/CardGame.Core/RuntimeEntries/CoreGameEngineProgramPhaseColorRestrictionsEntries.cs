#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime))]
internal sealed partial class CoreGameEngineProgramPhaseColorRestrictionsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> _resolve;
    private CoreGameEngineProgramPhaseColorRestrictionsEntries(Func<EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPhaseColorRestrictionsEntries Create(Func<EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> resolve) => new(resolve);
}
