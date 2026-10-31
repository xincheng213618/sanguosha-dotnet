#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ResponseCompletionRuntime))]
internal sealed class CoreResponseCompletionResponseCompletionAdapter(Func<ResponseCompletionRuntime> targetFactory) : ICoreResponseCompletionResponseCompletionOperations
{
    private readonly Func<ResponseCompletionRuntime> _targetFactory = targetFactory;
    public ResponseCompletionRuntime Get() => this._targetFactory();
}
