#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime))]
internal sealed partial class CoreGameEngineAppliedDamageBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> _resolve;
    private CoreGameEngineAppliedDamageBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAppliedDamageBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> resolve) => new(resolve);
}
