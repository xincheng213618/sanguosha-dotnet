#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime))]
internal sealed partial class CoreGameEngineForcedPhaseAlcoholEntries
{
    private readonly Func<EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> _resolve;
    private CoreGameEngineForcedPhaseAlcoholEntries(Func<EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineForcedPhaseAlcoholEntries Create(Func<EngineOwners.CoreGameEngineForcedPhaseAlcoholRuntime> resolve) => new(resolve);
}
