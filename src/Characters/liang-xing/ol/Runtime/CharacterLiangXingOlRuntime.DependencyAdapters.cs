#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterLiangXingOlCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICharacterLiangXingOlCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetSeatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetSeatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}
