#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitPaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitPaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitPaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitPaymentsRuntime> resolve) => new(resolve);
}
