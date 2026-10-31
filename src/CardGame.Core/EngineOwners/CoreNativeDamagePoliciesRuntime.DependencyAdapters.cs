#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramDamageHooksRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreProgramDamageHooksAdapter(Func<CoreProgramDamageHooksRuntime> targetFactory) : ICoreNativeDamagePoliciesCoreProgramDamageHooksOperations
{
    private readonly Func<CoreProgramDamageHooksRuntime> _targetFactory = targetFactory;
    public ProgramDamageHooks DamageHooks { get => this._targetFactory().DamageHooks; }
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreNativeDamagePoliciesCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreNativeDamagePoliciesCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}
