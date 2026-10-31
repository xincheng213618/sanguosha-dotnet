#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime))]
internal sealed partial class CoreGameEngineOwnedDeathBenefitReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> _resolve;
    private CoreGameEngineOwnedDeathBenefitReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineOwnedDeathBenefitReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOwnedDeathBenefitReturnsRuntime> resolve) => new(resolve);
}
