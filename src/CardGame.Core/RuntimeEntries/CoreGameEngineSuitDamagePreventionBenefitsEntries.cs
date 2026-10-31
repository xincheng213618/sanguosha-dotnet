#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime))]
internal sealed partial class CoreGameEngineSuitDamagePreventionBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> _resolve;
    private CoreGameEngineSuitDamagePreventionBenefitsEntries(Func<EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSuitDamagePreventionBenefitsEntries Create(Func<EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> resolve) => new(resolve);
}
