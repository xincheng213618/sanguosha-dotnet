#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CorePlayCardHistoryCoreProgramEventHistoryAdapter(Func<CoreGameEngineProgramEventHistoryRuntime> targetFactory) : ICorePlayCardHistoryCoreProgramEventHistoryOperations
{
    private readonly Func<CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public ProgramEventHistoryIndex ProgramHistory { get => this._targetFactory().ProgramHistory; }
}
