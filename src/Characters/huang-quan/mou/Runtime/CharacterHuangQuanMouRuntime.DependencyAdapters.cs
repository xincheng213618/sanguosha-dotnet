#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterHuangQuanMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterHuangQuanMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(BoundaryYiJiRuntime))]
internal sealed class CharacterHuangQuanMouBoundaryYiJiAdapter(Func<BoundaryYiJiRuntime> targetFactory) : ICharacterHuangQuanMouBoundaryYiJiOperations
{
    private readonly Func<BoundaryYiJiRuntime> _targetFactory = targetFactory;
    public BoundaryYiJiRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(MouHuangQuanRuntime))]
internal sealed class CharacterHuangQuanMouMouHuangQuanAdapter(Func<MouHuangQuanRuntime> targetFactory) : ICharacterHuangQuanMouMouHuangQuanOperations
{
    private readonly Func<MouHuangQuanRuntime> _targetFactory = targetFactory;
    public MouHuangQuanRuntime Get() => this._targetFactory();
}
