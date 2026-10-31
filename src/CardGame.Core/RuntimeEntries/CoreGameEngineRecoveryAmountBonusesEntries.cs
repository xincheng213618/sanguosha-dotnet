#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime))]
internal sealed partial class CoreGameEngineRecoveryAmountBonusesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> _resolve;
    private CoreGameEngineRecoveryAmountBonusesEntries(Func<EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRecoveryAmountBonusesEntries Create(Func<EngineOwners.CoreGameEngineRecoveryAmountBonusesRuntime> resolve) => new(resolve);
}
