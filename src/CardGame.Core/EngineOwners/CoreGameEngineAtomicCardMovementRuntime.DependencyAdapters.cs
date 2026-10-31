#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(AtomicCardMovementRuntime))]
internal sealed class CoreAtomicCardMovementAtomicCardMovementAdapter(Func<AtomicCardMovementRuntime> targetFactory) : ICoreAtomicCardMovementAtomicCardMovementOperations
{
    private readonly Func<AtomicCardMovementRuntime> _targetFactory = targetFactory;
    public AtomicCardMovementRuntime Get() => this._targetFactory();
}
