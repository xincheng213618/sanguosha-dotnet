#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouZhouYuRuntime))]
internal sealed class CharacterZhouYuMouMouZhouYuAdapter(Func<MouZhouYuRuntime> targetFactory) : ICharacterZhouYuMouMouZhouYuOperations
{
    private readonly Func<MouZhouYuRuntime> _targetFactory = targetFactory;
    public MouZhouYuRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterZhouYuMouCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterZhouYuMouCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
