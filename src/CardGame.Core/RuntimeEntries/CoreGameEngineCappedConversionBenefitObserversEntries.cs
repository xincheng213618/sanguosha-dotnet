#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedConversionBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCappedConversionBenefitObserversRuntime> _resolve;
    private CoreGameEngineCappedConversionBenefitObserversEntries(Func<EngineOwners.CoreGameEngineCappedConversionBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedConversionBenefitObserversEntries Create(Func<EngineOwners.CoreGameEngineCappedConversionBenefitObserversRuntime> resolve) => new(resolve);
}
