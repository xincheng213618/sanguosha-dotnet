#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouLiuXieRuntime))]
internal sealed class CharacterLiuXieMouMouLiuXieAdapter(Func<MouLiuXieRuntime> targetFactory) : ICharacterLiuXieMouMouLiuXieOperations
{
    private readonly Func<MouLiuXieRuntime> _targetFactory = targetFactory;
    public MouLiuXieRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterLiuXieMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterLiuXieMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
