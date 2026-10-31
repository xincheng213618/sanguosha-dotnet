#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeFrameRoutingRuntimeKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICoreNativeFrameRoutingRuntimeCharacterHuaTuoAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramSkillInvocation(long id);
}

internal interface ICoreNativeFrameRoutingRuntimeKernelCoreGameEngineProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryResume(global::CardGame.Core.GameEngine.ProgramContinuationCatalog __nativeReceiver, long frameId);
}

internal interface ICoreNativeFrameRoutingRuntimeCoreModeNativeFrameRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int StepCount(global::CardGame.Core.NativeFrameAutomaticPhase phase);
    global::CardGame.Core.NativeFrameAutomaticStepMetadata StepAt(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index);
    bool Matches(global::CardGame.Core.NativeFrameAutomaticPhase phase, int index, global::CardGame.Core.ResolutionFrame frame);
    bool TryAdvance(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreNativeFrameRoutingRuntimeNativeCardGameCoreModeNativeFrameRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryAdvance(global::CardGame.Core.ResolutionFrame frame);
}
