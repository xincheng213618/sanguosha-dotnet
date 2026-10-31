#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActorHandLimitPenaltiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime> _resolve;
    private CoreGameEngineActorHandLimitPenaltiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActorHandLimitPenaltiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineActorHandLimitPenaltiesRuntime> resolve) => new(resolve);
}
