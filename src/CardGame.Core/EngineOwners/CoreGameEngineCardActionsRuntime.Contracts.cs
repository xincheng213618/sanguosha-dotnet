#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreCardActionsProgramWindowCursorOperations : IProgramRuntimeOperations
{
    ProgramWindowCursorRuntime Get();
}

internal interface ICoreCardActionsCoreNativeRuleQueriesOperations : IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICoreCardActionsCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    ProgramCardLegality CardLegality { get; }
}

internal interface ICoreCardActionsCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    ProgramCardUsePayments CardUsePayments { get; }
}
