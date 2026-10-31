#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterHuangfuSongWumiaoCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterHuangfuSongWumiaoCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangfuSongWumiaoCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterHuangfuSongWumiaoCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
