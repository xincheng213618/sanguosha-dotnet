#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime))]
internal sealed partial class CoreGameEngineOwnTrickDrawAlcoholObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> _resolve;
    private CoreGameEngineOwnTrickDrawAlcoholObserversEntries(Func<EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnTrickDrawAlcoholObserversEntries Create(Func<EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> resolve) => new(resolve);
}
