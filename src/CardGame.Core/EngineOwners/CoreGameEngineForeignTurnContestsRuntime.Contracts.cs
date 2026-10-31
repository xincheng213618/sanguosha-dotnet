#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreForeignTurnContestsForeignActualTurnWindowOperations : IProgramRuntimeOperations
{
    ForeignActualTurnWindowRuntime Get();
}

internal interface ICoreForeignTurnContestsForeignTurnContestOperations : IProgramRuntimeOperations
{
    ForeignTurnContestRuntime Get();
}
