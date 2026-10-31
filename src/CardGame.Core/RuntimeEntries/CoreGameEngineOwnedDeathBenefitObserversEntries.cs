#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineOwnedDeathBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> _resolve;
    private CoreGameEngineOwnedDeathBenefitObserversEntries(Func<EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDeathBenefitObserversEntries Create(Func<EngineOwners.CoreGameEngineOwnedDeathBenefitObserversRuntime> resolve) => new(resolve);
}
