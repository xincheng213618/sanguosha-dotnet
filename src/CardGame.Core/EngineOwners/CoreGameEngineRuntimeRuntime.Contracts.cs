#nullable enable

namespace CardGame.Core.EngineOwners;

internal interface ICoreRuntimeNativeFrameAutomaticOperations : IProgramRuntimeOperations
{
    NativeFrameAutomaticRuntime Get();
}

internal interface ICoreRuntimeMatchStepDispatcherOperations : IProgramRuntimeOperations
{
    MatchStepDispatcher Get();
}

internal interface ICoreRuntimeNativeFrameRoutingOperations : IProgramRuntimeOperations
{
    NativeFrameRoutingRuntime Get();
}

internal interface ICoreRuntimeProgramCompletionOperations : IProgramRuntimeOperations
{
    ProgramCompletionRuntime Get();
}

internal interface ICoreRuntimeCoreProgramRuntimeHooksOperations : IProgramRuntimeOperations
{
    ProgramRuntimeHooks RuntimeHooks { get; }
}
