#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramDistanceRulesRuntime))]
internal sealed class CoreFinalTargetSlashCoreProgramDistanceRulesAdapter(Func<CoreProgramDistanceRulesRuntime> targetFactory) : ICoreFinalTargetSlashCoreProgramDistanceRulesOperations
{
    private readonly Func<CoreProgramDistanceRulesRuntime> _targetFactory = targetFactory;
    public ProgramDistanceRules DistanceRules { get => this._targetFactory().DistanceRules; }
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreFinalTargetSlashCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreFinalTargetSlashCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}
