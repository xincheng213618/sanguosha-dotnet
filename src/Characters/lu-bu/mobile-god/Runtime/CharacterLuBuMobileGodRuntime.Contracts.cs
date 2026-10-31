#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICharacterLuBuMobileGodCoreProgramCardLegalityOperations : IProgramRuntimeOperations
{
    ProgramCardLegality CardLegality { get; }
}

internal interface ICharacterLuBuMobileGodCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    ProgramCardUsePayments CardUsePayments { get; }
}
