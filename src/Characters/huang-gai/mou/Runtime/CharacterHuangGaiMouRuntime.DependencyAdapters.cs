#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangGaiMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterHuangGaiMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouHuangGaiRuntime))]
internal sealed class CharacterHuangGaiMouMouHuangGaiAdapter(Func<MouHuangGaiRuntime> targetFactory) : ICharacterHuangGaiMouMouHuangGaiOperations
{
    private readonly Func<MouHuangGaiRuntime> _targetFactory = targetFactory;
    public MouHuangGaiRuntime Get() => this._targetFactory();
}
