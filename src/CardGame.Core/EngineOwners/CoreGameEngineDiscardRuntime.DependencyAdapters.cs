#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ManualDiscardRuntime))]
internal sealed class CoreDiscardManualDiscardAdapter(Func<ManualDiscardRuntime> targetFactory) : ICoreDiscardManualDiscardOperations
{
    private readonly Func<ManualDiscardRuntime> _targetFactory = targetFactory;
    public ManualDiscardRuntime Get() => this._targetFactory();
}
