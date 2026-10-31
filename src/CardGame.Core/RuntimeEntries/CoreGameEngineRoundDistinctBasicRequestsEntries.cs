#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicRequestsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> _resolve;
    private CoreGameEngineRoundDistinctBasicRequestsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundDistinctBasicRequestsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicRequestsRuntime> resolve) => new(resolve);
}
