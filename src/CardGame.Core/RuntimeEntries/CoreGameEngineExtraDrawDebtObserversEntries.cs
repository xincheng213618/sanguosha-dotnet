#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineExtraDrawDebtObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineExtraDrawDebtObserversRuntime> _resolve;
    private CoreGameEngineExtraDrawDebtObserversEntries(Func<EngineOwners.CoreGameEngineExtraDrawDebtObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineExtraDrawDebtObserversEntries Create(Func<EngineOwners.CoreGameEngineExtraDrawDebtObserversRuntime> resolve) => new(resolve);
}
