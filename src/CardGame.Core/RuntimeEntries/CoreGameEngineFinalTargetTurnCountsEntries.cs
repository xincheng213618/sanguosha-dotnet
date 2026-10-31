#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime))]
internal sealed partial class CoreGameEngineFinalTargetTurnCountsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> _resolve;
    private CoreGameEngineFinalTargetTurnCountsEntries(Func<EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFinalTargetTurnCountsEntries Create(Func<EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime> resolve) => new(resolve);
}
