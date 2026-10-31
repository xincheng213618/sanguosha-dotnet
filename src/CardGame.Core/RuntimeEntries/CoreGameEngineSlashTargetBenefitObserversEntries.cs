#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime))]
internal sealed partial class CoreGameEngineSlashTargetBenefitObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitObserversEntries(Func<EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitObserversEntries Create(Func<EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> resolve) => new(resolve);
}
