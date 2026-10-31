#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterCaoHongMouCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICharacterCaoHongMouCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[Core.ProgramCapability(typeof(MouCaoHongRuntime))]
internal sealed class CharacterCaoHongMouMouCaoHongAdapter(Func<MouCaoHongRuntime> targetFactory) : ICharacterCaoHongMouMouCaoHongOperations
{
    private readonly Func<MouCaoHongRuntime> _targetFactory = targetFactory;
    public MouCaoHongRuntime Get() => this._targetFactory();
}
