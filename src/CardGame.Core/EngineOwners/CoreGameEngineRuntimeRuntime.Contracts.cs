#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;

internal interface ICoreRuntimeNativeFrameAutomaticOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeFrameAutomaticRuntime Get();
}

internal interface ICoreRuntimeMatchStepDispatcherOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MatchStepDispatcher Get();
}

internal interface ICoreRuntimeNativeFrameRoutingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeFrameRoutingRuntime Get();
}

internal interface ICoreRuntimeProgramCompletionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCompletionRuntime Get();
}

internal interface ICoreRuntimeCoreProgramRuntimeHooksOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramRuntimeHooks RuntimeHooks { get; }
}
