#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime> _resolve;
    private CoreGameEngineGuanduGeneralBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime> resolve) => new(resolve);
}
