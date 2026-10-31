#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CoreGameEngineLordSkillProjectionsState : Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess
{
    private GameEngine.ProjectionDependency[] _lordProjectionDependencies = [];
    internal ref GameEngine.ProjectionDependency[] ReferenceToLordProjectionDependencies => ref _lordProjectionDependencies;

    private long _lordProjectionDependencyRevision = default !;
    internal ref long ReferenceToLordProjectionDependencyRevision => ref _lordProjectionDependencyRevision;

    private bool _synchronizingLordProjections = default !;
    internal ref bool ReferenceToSynchronizingLordProjections => ref _synchronizingLordProjections;

    ref GameEngine.ProjectionDependency[] Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencies => ref _lordProjectionDependencies;

    ref long Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._lordProjectionDependencyRevision => ref _lordProjectionDependencyRevision;

    ref bool Contracts.ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess._synchronizingLordProjections => ref _synchronizingLordProjections;
}
