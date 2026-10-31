#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActorHandLimitPenaltiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime> _resolve;
    private CoreGameEngineActorHandLimitPenaltiesEntries(Func<EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActorHandLimitPenaltiesEntries Create(Func<EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime> resolve) => new(resolve);
}
