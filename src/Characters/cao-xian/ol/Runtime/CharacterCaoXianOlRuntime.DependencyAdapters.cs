#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterCaoXianOlCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterCaoXianOlCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}

[Core.ProgramCapability(typeof(CaoXianRuntime))]
internal sealed class CharacterCaoXianOlCaoXianAdapter(Func<CaoXianRuntime> targetFactory) : ICharacterCaoXianOlCaoXianOperations
{
    private readonly Func<CaoXianRuntime> _targetFactory = targetFactory;
    public CaoXianRuntime Get() => this._targetFactory();
}
