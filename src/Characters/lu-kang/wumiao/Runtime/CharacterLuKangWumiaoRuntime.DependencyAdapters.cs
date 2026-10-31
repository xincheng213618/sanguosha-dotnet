#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramTargetSelectionRuntime))]
internal sealed class CharacterLuKangWumiaoProgramTargetSelectionAdapter(Func<ProgramTargetSelectionRuntime> targetFactory) : ICharacterLuKangWumiaoProgramTargetSelectionOperations
{
    private readonly Func<ProgramTargetSelectionRuntime> _targetFactory = targetFactory;
    public ProgramTargetSelectionRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CharacterLuKangWumiaoCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICharacterLuKangWumiaoCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
