#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterXuShengMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterXuShengMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouXuShengRuntime))]
internal sealed class CharacterXuShengMouMouXuShengAdapter(Func<MouXuShengRuntime> targetFactory) : ICharacterXuShengMouMouXuShengOperations
{
    private readonly Func<MouXuShengRuntime> _targetFactory = targetFactory;
    public MouXuShengRuntime Get() => this._targetFactory();
}
