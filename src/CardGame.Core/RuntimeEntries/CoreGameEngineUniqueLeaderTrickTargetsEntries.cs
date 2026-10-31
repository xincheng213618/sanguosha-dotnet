#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineUniqueLeaderTrickTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> _resolve;
    private CoreGameEngineUniqueLeaderTrickTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueLeaderTrickTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> resolve) => new(resolve);
}
