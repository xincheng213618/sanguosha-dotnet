#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouLidanRuntime))]
internal sealed class CharacterChengYuMouOlMouLidanAdapter(Func<MouLidanRuntime> targetFactory) : ICharacterChengYuMouOlMouLidanOperations
{
    private readonly Func<MouLidanRuntime> _targetFactory = targetFactory;
    public MouLidanRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(MouDanchiRuntime))]
internal sealed class CharacterChengYuMouOlMouDanchiAdapter(Func<MouDanchiRuntime> targetFactory) : ICharacterChengYuMouOlMouDanchiOperations
{
    private readonly Func<MouDanchiRuntime> _targetFactory = targetFactory;
    public MouDanchiRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreNativeRuleQueriesRuntime))]
internal sealed class CharacterChengYuMouOlCoreNativeRuleQueriesAdapter(Func<CoreNativeRuleQueriesRuntime> targetFactory) : ICharacterChengYuMouOlCoreNativeRuleQueriesOperations
{
    private readonly Func<CoreNativeRuleQueriesRuntime> _targetFactory = targetFactory;
    public int GetCombatDistance(int sourceSeat, int targetSeat) => this._targetFactory().GetCombatDistance(sourceSeat, targetSeat);
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterChengYuMouOlCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterChengYuMouOlCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
