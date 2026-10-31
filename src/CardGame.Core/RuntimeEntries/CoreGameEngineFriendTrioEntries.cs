#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime))]
internal sealed partial class CoreGameEngineFriendTrioEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> _resolve;
    private CoreGameEngineFriendTrioEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFriendTrioEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFriendTrioRuntime> resolve) => new(resolve);
}
