#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFirstSlashDuelBenefitsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> _resolve;
    private CoreGameEngineFirstSlashDuelBenefitsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineFirstSlashDuelBenefitsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineFirstSlashDuelBenefitsRuntime> resolve) => new(resolve);
}
