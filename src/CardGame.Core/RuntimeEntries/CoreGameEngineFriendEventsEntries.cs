#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime))]
internal sealed partial class CoreGameEngineFriendEventsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> _resolve;
    private CoreGameEngineFriendEventsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFriendEventsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendEventsRuntime> resolve) => new(resolve);
}
