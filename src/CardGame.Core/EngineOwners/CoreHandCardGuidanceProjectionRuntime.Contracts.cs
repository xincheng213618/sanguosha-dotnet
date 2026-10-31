#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHandCardGuidanceProjectionCoreNativeLegalActionsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<LegalAction> GetHumanLegalActions();
}
