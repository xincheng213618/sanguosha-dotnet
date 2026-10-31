#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime))]
internal sealed partial class CoreGameEngineJoinedTrickDamageObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> _resolve;
    private CoreGameEngineJoinedTrickDamageObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJoinedTrickDamageObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> resolve) => new(resolve);
}
