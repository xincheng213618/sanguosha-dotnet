#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess : IProgramRuntimeOperations
{
    ref GameEngine.ProjectionDependency[] _lordProjectionDependencies { get; }

    ref long _lordProjectionDependencyRevision { get; }

    ref bool _synchronizingLordProjections { get; }
}
