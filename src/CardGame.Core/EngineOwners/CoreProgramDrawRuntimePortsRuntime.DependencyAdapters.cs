#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreProgramDrawRuntimePortsCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreProgramDrawRuntimePortsCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}
