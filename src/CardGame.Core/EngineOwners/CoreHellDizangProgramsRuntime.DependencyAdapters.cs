#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CoreHellDizangProgramsCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICoreHellDizangProgramsCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}
