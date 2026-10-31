#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterKeBiNengOlCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterKeBiNengOlCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
