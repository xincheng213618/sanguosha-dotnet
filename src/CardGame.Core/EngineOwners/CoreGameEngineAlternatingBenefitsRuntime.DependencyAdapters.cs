#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineProgramEventHistoryRuntime))]
internal sealed class CoreAlternatingBenefitsCoreProgramEventHistoryAdapter(Func<CoreGameEngineProgramEventHistoryRuntime> targetFactory) : ICoreAlternatingBenefitsCoreProgramEventHistoryOperations
{
    private readonly Func<CoreGameEngineProgramEventHistoryRuntime> _targetFactory = targetFactory;
    public ProgramEventHistoryIndex ProgramHistory { get => this._targetFactory().ProgramHistory; }

    public IReadOnlyList<T> ProgramEventHistory<T>()
        where T : IGameEvent => this._targetFactory().ProgramEventHistory<T>();
}
