#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJoinedTrickDamageRewardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> _resolve;
    private CoreGameEngineJoinedTrickDamageRewardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJoinedTrickDamageRewardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> resolve) => new(resolve);
}
