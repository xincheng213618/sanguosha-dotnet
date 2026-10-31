#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CorePlayerViewProjectionRuntime))]
internal sealed class CoreSameNameHandDiscardOrDamageCorePlayerViewProjectionAdapter(Func<CorePlayerViewProjectionRuntime> targetFactory) : ICoreSameNameHandDiscardOrDamageCorePlayerViewProjectionOperations
{
    private readonly Func<CorePlayerViewProjectionRuntime> _targetFactory = targetFactory;
    public GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false) => this._targetFactory().CreateSnapshot(viewerSeat, revealAll);
}
