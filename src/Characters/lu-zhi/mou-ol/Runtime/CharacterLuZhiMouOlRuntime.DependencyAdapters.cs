#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterLuZhiMouOlCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterLuZhiMouOlCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
