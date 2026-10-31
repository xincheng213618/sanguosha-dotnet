#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterSimaShiMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterSimaShiMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouSimaShiRuntime))]
internal sealed class CharacterSimaShiMouMouSimaShiAdapter(Func<MouSimaShiRuntime> targetFactory) : ICharacterSimaShiMouMouSimaShiOperations
{
    private readonly Func<MouSimaShiRuntime> _targetFactory = targetFactory;
    public MouSimaShiRuntime Get() => this._targetFactory();
}
