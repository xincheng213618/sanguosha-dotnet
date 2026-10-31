#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouZhugeJinRuntime))]
internal sealed class CharacterZhugeJinMouMouZhugeJinAdapter(Func<MouZhugeJinRuntime> targetFactory) : ICharacterZhugeJinMouMouZhugeJinOperations
{
    private readonly Func<MouZhugeJinRuntime> _targetFactory = targetFactory;
    public MouZhugeJinRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterZhugeJinMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterZhugeJinMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
