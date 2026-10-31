#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreGameEngineProgramLifecycleRuntime))]
internal sealed class CoreShenwuCardsCoreProgramLifecycleAdapter(Func<CoreGameEngineProgramLifecycleRuntime> targetFactory) : ICoreShenwuCardsCoreProgramLifecycleOperations
{
    private readonly Func<CoreGameEngineProgramLifecycleRuntime> _targetFactory = targetFactory;
    public ProgramInteractionRouter ProgramInteractions { get => this._targetFactory().ProgramInteractions; }
}
