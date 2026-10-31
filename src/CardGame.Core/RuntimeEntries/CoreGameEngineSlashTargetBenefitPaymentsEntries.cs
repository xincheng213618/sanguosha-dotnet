#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime))]
internal sealed partial class CoreGameEngineSlashTargetBenefitPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> resolve) => new(resolve);
}
