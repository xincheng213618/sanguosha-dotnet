#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime))]
internal sealed partial class CoreGameEngineAppliedDamageBenefitsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> _resolve;
    private CoreGameEngineAppliedDamageBenefitsEntries(Func<EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAppliedDamageBenefitsEntries Create(Func<EngineOwners.CoreGameEngineAppliedDamageBenefitsRuntime> resolve) => new(resolve);
}
