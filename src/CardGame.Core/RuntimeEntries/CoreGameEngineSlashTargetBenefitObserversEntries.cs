#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> _resolve;
    private CoreGameEngineSlashTargetBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSlashTargetBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSlashTargetBenefitObserversRuntime> resolve) => new(resolve);
}
