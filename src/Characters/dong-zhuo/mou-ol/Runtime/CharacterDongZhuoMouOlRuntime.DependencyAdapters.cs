#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouDongZhuoOlRuntime))]
internal sealed class CharacterDongZhuoMouOlMouDongZhuoOlAdapter(Func<MouDongZhuoOlRuntime> targetFactory) : ICharacterDongZhuoMouOlMouDongZhuoOlOperations
{
    private readonly Func<MouDongZhuoOlRuntime> _targetFactory = targetFactory;
    public MouDongZhuoOlRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CoreProgramTargetRulesRuntime))]
internal sealed class CharacterDongZhuoMouOlCoreProgramTargetRulesAdapter(Func<CoreProgramTargetRulesRuntime> targetFactory) : ICharacterDongZhuoMouOlCoreProgramTargetRulesOperations
{
    private readonly Func<CoreProgramTargetRulesRuntime> _targetFactory = targetFactory;
    public ProgramTargetRules TargetRules { get => this._targetFactory().TargetRules; }
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterDongZhuoMouOlCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterDongZhuoMouOlCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
