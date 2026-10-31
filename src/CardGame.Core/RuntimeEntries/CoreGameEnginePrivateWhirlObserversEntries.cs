#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime))]
internal sealed partial class CoreGameEnginePrivateWhirlObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime> _resolve;
    private CoreGameEnginePrivateWhirlObserversEntries(Func<EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePrivateWhirlObserversEntries Create(Func<EngineOwners.CoreGameEnginePrivateWhirlObserversRuntime> resolve) => new(resolve);
}
