#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouJiangJiRuntime))]
internal sealed class CharacterJiangJiMouMouJiangJiAdapter(Func<MouJiangJiRuntime> targetFactory) : ICharacterJiangJiMouMouJiangJiOperations
{
    private readonly Func<MouJiangJiRuntime> _targetFactory = targetFactory;
    public MouJiangJiRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterJiangJiMouCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterJiangJiMouCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
