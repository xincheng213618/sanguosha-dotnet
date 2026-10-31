#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime))]
internal sealed partial class CoreGameEngineRoundDistinctBasicRequestsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> _resolve;
    private CoreGameEngineRoundDistinctBasicRequestsEntries(Func<EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundDistinctBasicRequestsEntries Create(Func<EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> resolve) => new(resolve);
}
