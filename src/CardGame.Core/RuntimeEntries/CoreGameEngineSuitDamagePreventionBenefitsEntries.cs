#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime))]
internal sealed partial class CoreGameEngineSuitDamagePreventionBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> _resolve;
    private CoreGameEngineSuitDamagePreventionBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSuitDamagePreventionBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSuitDamagePreventionBenefitsRuntime> resolve) => new(resolve);
}
