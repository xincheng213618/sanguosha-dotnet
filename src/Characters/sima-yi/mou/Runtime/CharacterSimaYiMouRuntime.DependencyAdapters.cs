#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterSimaYiMouCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterSimaYiMouCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(MouSimaYiRuntime))]
internal sealed class CharacterSimaYiMouMouSimaYiAdapter(Func<MouSimaYiRuntime> targetFactory) : ICharacterSimaYiMouMouSimaYiOperations
{
    private readonly Func<MouSimaYiRuntime> _targetFactory = targetFactory;
    public MouSimaYiRuntime Get() => this._targetFactory();
}
