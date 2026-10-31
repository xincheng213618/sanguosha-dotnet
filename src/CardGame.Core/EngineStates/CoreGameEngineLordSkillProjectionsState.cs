#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineLordSkillProjectionsState : global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess
{
    private global::CardGame.Core.GameEngine.ProjectionDependency[] _lordProjectionDependencies = [];
    internal ref global::CardGame.Core.GameEngine.ProjectionDependency[] ReferenceToLordProjectionDependencies => ref _lordProjectionDependencies;

    private long _lordProjectionDependencyRevision = default !;
    internal ref long ReferenceToLordProjectionDependencyRevision => ref _lordProjectionDependencyRevision;

    private bool _synchronizingLordProjections = default !;
    internal ref bool ReferenceToSynchronizingLordProjections => ref _synchronizingLordProjections;

    ref global::CardGame.Core.GameEngine.ProjectionDependency[] global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencies => ref _lordProjectionDependencies;

    ref long global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencyRevision => ref _lordProjectionDependencyRevision;

    ref bool global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._synchronizingLordProjections => ref _synchronizingLordProjections;
}
