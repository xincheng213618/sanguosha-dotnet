#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ResponseCompletionRuntime))]
internal sealed class CoreResponseCompletionHealthResponseCompletionAdapter(Func<ResponseCompletionRuntime> targetFactory) : ICoreResponseCompletionHealthResponseCompletionOperations
{
    private readonly Func<ResponseCompletionRuntime> _targetFactory = targetFactory;
    public ResponseCompletionRuntime Get() => this._targetFactory();
}
