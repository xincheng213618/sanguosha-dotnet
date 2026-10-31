#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed partial class CoreGameEngineFriendEventsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFriendEventsRuntime> _resolve;
    private CoreGameEngineFriendEventsEntries(Func<EngineOwners.CoreGameEngineFriendEventsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFriendEventsEntries Create(Func<EngineOwners.CoreGameEngineFriendEventsRuntime> resolve) => new(resolve);
}
