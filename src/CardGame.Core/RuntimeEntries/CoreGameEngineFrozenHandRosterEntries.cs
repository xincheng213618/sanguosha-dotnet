#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFrozenHandRosterEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterRuntime> _resolve;
    private CoreGameEngineFrozenHandRosterEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFrozenHandRosterEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFrozenHandRosterRuntime> resolve) => new(resolve);
}
