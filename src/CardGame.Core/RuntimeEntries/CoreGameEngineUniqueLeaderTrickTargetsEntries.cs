#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime))]
internal sealed partial class CoreGameEngineUniqueLeaderTrickTargetsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> _resolve;
    private CoreGameEngineUniqueLeaderTrickTargetsEntries(Func<EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineUniqueLeaderTrickTargetsEntries Create(Func<EngineOwners.CoreGameEngineUniqueLeaderTrickTargetsRuntime> resolve) => new(resolve);
}
