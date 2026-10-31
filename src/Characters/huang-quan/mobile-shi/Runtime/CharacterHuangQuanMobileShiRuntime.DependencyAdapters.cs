#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterHuangQuanMobileShiCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterHuangQuanMobileShiCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}
