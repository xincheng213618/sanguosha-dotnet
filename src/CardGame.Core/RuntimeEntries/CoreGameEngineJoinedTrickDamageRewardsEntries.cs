#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime))]
internal sealed partial class CoreGameEngineJoinedTrickDamageRewardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> _resolve;
    private CoreGameEngineJoinedTrickDamageRewardsEntries(Func<EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJoinedTrickDamageRewardsEntries Create(Func<EngineOwners.CoreGameEngineJoinedTrickDamageRewardsRuntime> resolve) => new(resolve);
}
