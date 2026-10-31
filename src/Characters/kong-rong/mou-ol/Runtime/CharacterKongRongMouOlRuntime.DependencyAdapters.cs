#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(MouKongRongRuntime))]
internal sealed class CharacterKongRongMouOlMouKongRongAdapter(Func<MouKongRongRuntime> targetFactory) : ICharacterKongRongMouOlMouKongRongOperations
{
    private readonly Func<MouKongRongRuntime> _targetFactory = targetFactory;
    public MouKongRongRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterKongRongMouOlCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterKongRongMouOlCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
