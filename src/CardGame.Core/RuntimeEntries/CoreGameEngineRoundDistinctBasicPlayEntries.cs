#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundDistinctBasicPlayEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> _resolve;
    private CoreGameEngineRoundDistinctBasicPlayEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundDistinctBasicPlayEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundDistinctBasicPlayRuntime> resolve) => new(resolve);
}
