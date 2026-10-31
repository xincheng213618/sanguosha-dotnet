#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramExpressionBindingsCoreProgramExpressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramExpressionNumber(long frameId, string name, int value);
}

internal interface ICoreProgramExpressionBindingsKernelCoreGameEngineProgramExpressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramExpressionWorld CreateProgramExpressionWorld(global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICoreProgramExpressionBindingsCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void CancelProgramBindingAndCleanup(global::CardGame.Core.ProgramSkillFrame frame, string reason);
}
