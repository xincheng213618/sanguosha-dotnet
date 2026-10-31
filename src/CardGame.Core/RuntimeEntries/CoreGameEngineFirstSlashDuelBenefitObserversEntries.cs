#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime> _resolve;
    private CoreGameEngineFirstSlashDuelBenefitObserversEntries(Func<EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFirstSlashDuelBenefitObserversEntries Create(Func<EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime> resolve) => new(resolve);
}
