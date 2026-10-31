#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(OwnedDrawPhaseRuntime))]
internal sealed class CoreDrawPhaseObligationsOwnedDrawPhaseAdapter(Func<OwnedDrawPhaseRuntime> targetFactory) : ICoreDrawPhaseObligationsOwnedDrawPhaseOperations
{
    private readonly Func<OwnedDrawPhaseRuntime> _targetFactory = targetFactory;
    public OwnedDrawPhaseRuntime Get() => this._targetFactory();
}
