#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime))]
internal sealed partial class CoreGameEngineOwnedDeathBenefitReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> _resolve;
    private CoreGameEngineOwnedDeathBenefitReturnsEntries(Func<EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDeathBenefitReturnsEntries Create(Func<EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> resolve) => new(resolve);
}
