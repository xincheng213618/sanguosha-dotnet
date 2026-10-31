#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramSelectionModulesRuntime))]
internal sealed class CharacterHeQiOlCoreProgramSelectionModulesAdapter(Func<CoreProgramSelectionModulesRuntime> targetFactory) : ICharacterHeQiOlCoreProgramSelectionModulesOperations
{
    private readonly Func<CoreProgramSelectionModulesRuntime> _targetFactory = targetFactory;
    public IProgramSelectionQueries ProgramSelectionQueries { get => this._targetFactory().ProgramSelectionQueries; }
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterHeQiOlCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICharacterHeQiOlCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
    public int GetAttackRange(int sourceSeat) => this._targetFactory().GetAttackRange(sourceSeat);
}

[Core.ProgramCapability(typeof(CoreGameEngineEquipmentContentRuntime))]
internal sealed class CharacterHeQiOlCoreEquipmentContentAdapter(Func<CoreGameEngineEquipmentContentRuntime> targetFactory) : ICharacterHeQiOlCoreEquipmentContentOperations
{
    private readonly Func<CoreGameEngineEquipmentContentRuntime> _targetFactory = targetFactory;
    public EquipmentContentRegistry EquipmentRules { get => this._targetFactory().EquipmentRules; }
}
