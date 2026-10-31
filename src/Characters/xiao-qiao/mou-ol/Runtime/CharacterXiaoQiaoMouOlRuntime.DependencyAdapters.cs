#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterXiaoQiaoMouOlCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterXiaoQiaoMouOlCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterXiaoQiaoMouOlCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterXiaoQiaoMouOlCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouXiaoQiaoOlRuntime))]
internal sealed class CharacterXiaoQiaoMouOlMouXiaoQiaoOlAdapter(Func<MouXiaoQiaoOlRuntime> targetFactory) : ICharacterXiaoQiaoMouOlMouXiaoQiaoOlOperations
{
    private readonly Func<MouXiaoQiaoOlRuntime> _targetFactory = targetFactory;
    public MouXiaoQiaoOlRuntime Get() => this._targetFactory();
}
