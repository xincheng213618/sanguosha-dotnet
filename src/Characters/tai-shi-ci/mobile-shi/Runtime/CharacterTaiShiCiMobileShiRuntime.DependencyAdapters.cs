#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ShiTaiShiCiRuntime))]
internal sealed class CharacterTaiShiCiMobileShiShiTaiShiCiAdapter(Func<ShiTaiShiCiRuntime> targetFactory) : ICharacterTaiShiCiMobileShiShiTaiShiCiOperations
{
    private readonly Func<ShiTaiShiCiRuntime> _targetFactory = targetFactory;
    public ShiTaiShiCiRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterTaiShiCiMobileShiCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICharacterTaiShiCiMobileShiCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}
