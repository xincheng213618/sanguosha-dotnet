#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundRankBonusObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusObserversRuntime> _resolve;
    private CoreGameEngineBoundRankBonusObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineBoundRankBonusObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineBoundRankBonusObserversRuntime> resolve) => new(resolve);
}
