#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(OwnedDrawPhaseRuntime))]
internal sealed class CoreDrawPhaseAttemptsOwnedDrawPhaseAdapter(Func<OwnedDrawPhaseRuntime> targetFactory) : ICoreDrawPhaseAttemptsOwnedDrawPhaseOperations
{
    private readonly Func<OwnedDrawPhaseRuntime> _targetFactory = targetFactory;
    public OwnedDrawPhaseRuntime Get() => this._targetFactory();
}
