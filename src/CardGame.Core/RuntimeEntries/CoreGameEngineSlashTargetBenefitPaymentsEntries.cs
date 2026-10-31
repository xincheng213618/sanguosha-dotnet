#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetBenefitPaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitPaymentsEntries(Func<EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitPaymentsEntries Create(Func<EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> resolve) => new(resolve);
}
