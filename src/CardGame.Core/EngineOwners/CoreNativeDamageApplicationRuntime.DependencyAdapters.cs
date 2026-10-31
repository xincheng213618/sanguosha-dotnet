#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreNativeDamageApplicationCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreNativeDamageApplicationCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[Core.ProgramCapability(typeof(CoreProgramDamageHooksRuntime))]
internal sealed class CoreNativeDamageApplicationCoreProgramDamageHooksAdapter(Func<CoreProgramDamageHooksRuntime> targetFactory) : ICoreNativeDamageApplicationCoreProgramDamageHooksOperations
{
    private readonly Func<CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}
