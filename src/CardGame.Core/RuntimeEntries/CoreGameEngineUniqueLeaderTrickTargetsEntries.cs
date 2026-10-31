#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime))]
internal sealed partial class CoreGameEngineUniqueLeaderTrickTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> _resolve;
    private CoreGameEngineUniqueLeaderTrickTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueLeaderTrickTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> resolve) => new(resolve);
}
