#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(PhysicalCardDrawRuntime))]
internal sealed class CorePhysicalCardDrawPhysicalCardDrawAdapter(Func<PhysicalCardDrawRuntime> targetFactory) : ICorePhysicalCardDrawPhysicalCardDrawOperations
{
    private readonly Func<PhysicalCardDrawRuntime> _targetFactory = targetFactory;
    public PhysicalCardDrawRuntime Get() => this._targetFactory();
}
