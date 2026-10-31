#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterJiaXuMouCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterJiaXuMouCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(MouJiaXuRuntime))]
internal sealed class CharacterJiaXuMouMouJiaXuAdapter(Func<MouJiaXuRuntime> targetFactory) : ICharacterJiaXuMouMouJiaXuOperations
{
    private readonly Func<MouJiaXuRuntime> _targetFactory = targetFactory;
    public MouJiaXuRuntime Get() => this._targetFactory();
}
