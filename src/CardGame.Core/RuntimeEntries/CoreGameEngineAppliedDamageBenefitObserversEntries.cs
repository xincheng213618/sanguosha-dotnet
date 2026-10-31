#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineAppliedDamageBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime> _resolve;
    private CoreGameEngineAppliedDamageBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAppliedDamageBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAppliedDamageBenefitObserversRuntime> resolve) => new(resolve);
}
