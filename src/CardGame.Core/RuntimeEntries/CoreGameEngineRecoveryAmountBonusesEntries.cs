#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryAmountBonusesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> _resolve;
    private CoreGameEngineRecoveryAmountBonusesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecoveryAmountBonusesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> resolve) => new(resolve);
}
