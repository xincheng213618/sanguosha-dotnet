#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineProgramTargetResolversRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversAdapter(Func<CoreGameEngineProgramTargetResolversRuntime> targetFactory) : ICoreProgramTargetSelectionRuntimePortsCoreProgramTargetResolversOperations
{
    private readonly Func<CoreGameEngineProgramTargetResolversRuntime> _targetFactory = targetFactory;
    public ProgramTargetResolverRegistry ProgramTargetResolvers { get => this._targetFactory().ProgramTargetResolvers; }
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreProgramTargetSelectionRuntimePortsCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}
