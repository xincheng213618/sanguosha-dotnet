#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime))]
internal sealed partial class CoreGameEngineCappedConversionBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> _resolve;
    private CoreGameEngineCappedConversionBenefitsEntries(Func<EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedConversionBenefitsEntries Create(Func<EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> resolve) => new(resolve);
}
