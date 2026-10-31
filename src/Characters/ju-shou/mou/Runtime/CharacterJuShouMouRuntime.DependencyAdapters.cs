#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterJuShouMouCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterJuShouMouCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(MouJuShouRuntime))]
internal sealed class CharacterJuShouMouMouJuShouAdapter(Func<MouJuShouRuntime> targetFactory) : ICharacterJuShouMouMouJuShouOperations
{
    private readonly Func<MouJuShouRuntime> _targetFactory = targetFactory;
    public MouJuShouRuntime Get() => this._targetFactory();
}
