#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramCardPaymentsProgramPaidCardOperations : IProgramRuntimeOperations
{
    ProgramPaidCardRuntime Get();
}
