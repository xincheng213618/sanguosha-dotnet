#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreNativeSlashUseCoreProgramCardUsePaymentsOperations : IProgramRuntimeOperations
{
    ProgramCardUsePayments CardUsePayments { get; }
}
