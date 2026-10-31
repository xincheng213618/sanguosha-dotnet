#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime> _resolve;
    private CoreGameEngineFirstSlashDuelBenefitObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFirstSlashDuelBenefitObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitObserversRuntime> resolve) => new(resolve);
}
