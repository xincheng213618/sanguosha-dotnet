#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterZhangXiuMouCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterZhangXiuMouCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterZhangXiuMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterZhangXiuMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouZhangXiuAnniversaryRuntime))]
internal sealed class CharacterZhangXiuMouMouZhangXiuAnniversaryAdapter(Func<MouZhangXiuAnniversaryRuntime> targetFactory) : ICharacterZhangXiuMouMouZhangXiuAnniversaryOperations
{
    private readonly Func<MouZhangXiuAnniversaryRuntime> _targetFactory = targetFactory;
    public MouZhangXiuAnniversaryRuntime Get() => this._targetFactory();
}
