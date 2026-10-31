#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime))]
internal sealed partial class CoreGameEngineGuanduGeneralBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime> _resolve;
    private CoreGameEngineGuanduGeneralBenefitsEntries(Func<EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGuanduGeneralBenefitsEntries Create(Func<EngineOwners.CoreGameEngineGuanduGeneralBenefitsRuntime> resolve) => new(resolve);
}
