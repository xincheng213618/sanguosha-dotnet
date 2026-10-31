#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramNativeReturnsRuntime))]
internal sealed class CoreNativeAttackCompletionCoreProgramNativeReturnsAdapter(Func<CoreProgramNativeReturnsRuntime> targetFactory) : ICoreNativeAttackCompletionCoreProgramNativeReturnsOperations
{
    private readonly Func<CoreProgramNativeReturnsRuntime> _targetFactory = targetFactory;
    public ProgramNativeReturns NativeReturns { get => this._targetFactory().NativeReturns; }
}
