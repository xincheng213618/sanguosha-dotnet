#nullable enable
namespace CardGame.Core.EngineStates.Contracts;
internal interface ICoreGameEngineLordSkillProjectionsLordProjectionDependenciesLordProjectionDependencyRevisionSynchronizingLordProjectionsAccess : global::CardGame.Core.IProgramRuntimeOperations
{
    ref global::CardGame.Core.GameEngine.ProjectionDependency[] _lordProjectionDependencies { get; }

    ref long _lordProjectionDependencyRevision { get; }

    ref bool _synchronizingLordProjections { get; }
}
