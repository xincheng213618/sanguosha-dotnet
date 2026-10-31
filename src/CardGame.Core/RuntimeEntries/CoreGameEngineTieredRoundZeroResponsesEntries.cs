#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime))]
internal sealed partial class CoreGameEngineTieredRoundZeroResponsesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> _resolve;
    private CoreGameEngineTieredRoundZeroResponsesEntries(Func<EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTieredRoundZeroResponsesEntries Create(Func<EngineOwners.CoreGameEngineTieredRoundZeroResponsesRuntime> resolve) => new(resolve);
}
