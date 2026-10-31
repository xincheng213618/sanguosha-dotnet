#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime))]
internal sealed partial class CoreGameEngineJoinedTrickDamageObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> _resolve;
    private CoreGameEngineJoinedTrickDamageObserversEntries(Func<EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineJoinedTrickDamageObserversEntries Create(Func<EngineOwners.CoreGameEngineJoinedTrickDamageObserversRuntime> resolve) => new(resolve);
}
