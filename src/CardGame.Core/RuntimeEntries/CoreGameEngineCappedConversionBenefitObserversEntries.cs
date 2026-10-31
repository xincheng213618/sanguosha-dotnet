#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCappedConversionBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitObserversRuntime> _resolve;
    private CoreGameEngineCappedConversionBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedConversionBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitObserversRuntime> resolve) => new(resolve);
}
