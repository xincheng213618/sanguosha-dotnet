#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraDrawDebtObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraDrawDebtObserversRuntime> _resolve;
    private CoreGameEngineExtraDrawDebtObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraDrawDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraDrawDebtObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineExtraDrawDebtObserversRuntime> resolve) => new(resolve);
}
