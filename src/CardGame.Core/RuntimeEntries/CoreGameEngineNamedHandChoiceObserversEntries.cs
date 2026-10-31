#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNamedHandChoiceObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime> _resolve;
    private CoreGameEngineNamedHandChoiceObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNamedHandChoiceObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNamedHandChoiceObserversRuntime> resolve) => new(resolve);
}
