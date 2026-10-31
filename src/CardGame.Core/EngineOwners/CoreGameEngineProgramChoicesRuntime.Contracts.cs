#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreProgramChoicesProgramPaidCardOperations : IProgramRuntimeOperations
{
    ProgramPaidCardRuntime Get();
}
