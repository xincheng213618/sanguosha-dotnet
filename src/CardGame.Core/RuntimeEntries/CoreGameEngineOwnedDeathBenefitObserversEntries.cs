#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineOwnedDeathBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> _resolve;
    private CoreGameEngineOwnedDeathBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDeathBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> resolve) => new(resolve);
}
