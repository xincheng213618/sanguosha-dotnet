#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(NativeLogicalDiscardRuntime))]
internal sealed class CoreNativeLogicalDiscardsNativeLogicalDiscardAdapter(Func<NativeLogicalDiscardRuntime> targetFactory) : ICoreNativeLogicalDiscardsNativeLogicalDiscardOperations
{
    private readonly Func<NativeLogicalDiscardRuntime> _targetFactory = targetFactory;
    public NativeLogicalDiscardRuntime Get() => this._targetFactory();
}
