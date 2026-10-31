#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime))]
internal sealed partial class CoreGameEngineCappedConversionBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> _resolve;
    private CoreGameEngineCappedConversionBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCappedConversionBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCappedConversionBenefitsRuntime> resolve) => new(resolve);
}
