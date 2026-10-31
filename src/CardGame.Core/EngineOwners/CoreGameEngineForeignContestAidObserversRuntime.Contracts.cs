#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreForeignContestAidObserversForeignTurnContestOperations : IProgramRuntimeOperations
{
    ForeignTurnContestRuntime Get();
}
