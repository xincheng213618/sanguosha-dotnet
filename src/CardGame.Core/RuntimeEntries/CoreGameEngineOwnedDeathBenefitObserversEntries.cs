#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedDeathBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> _resolve;
    private CoreGameEngineOwnedDeathBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDeathBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> resolve) => new(resolve);
}
