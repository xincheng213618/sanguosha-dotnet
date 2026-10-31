#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnTrickDrawAlcoholObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> _resolve;
    private CoreGameEngineOwnTrickDrawAlcoholObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnTrickDrawAlcoholObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnTrickDrawAlcoholObserversRuntime> resolve) => new(resolve);
}
