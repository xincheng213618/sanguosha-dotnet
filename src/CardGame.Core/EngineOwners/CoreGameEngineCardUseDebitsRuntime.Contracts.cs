#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreCardUseDebitsCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    ProgramCardUsePayments CardUsePayments { get; }
}
