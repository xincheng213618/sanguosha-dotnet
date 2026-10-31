#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreHandGuidanceCoreNativeLegalActionsOperations : IProgramRuntimeOperations
{
    IReadOnlyList<LegalAction> GetHumanLegalActions();
}

internal interface ICoreHandGuidanceCoreCommandSessionQueriesOperations : IProgramRuntimeOperations
{
    long Revision { get; }
}
