#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreNativeFrameRoutingRuntimeRuntime : global::CardGame.Core.INativeFrameRoutes, global::CardGame.Core.INativeProgramExecution
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries _characterHuaTuoAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations _kernelCoreGameEngineAdvancedLifecycleHostOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTryResumeOperations _kernelCoreGameEngineProgramContinuationsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelTryAdvanceOperations _nativeCardGameCoreModeNativeFrameRuntimeOperations;
    internal CoreNativeFrameRoutingRuntimeRuntime(global::CardGame.Core.RuntimeEntries.CharacterHuaTuoAnniversaryGodEntries characterHuaTuoAnniversaryGodOperations,global::CardGame.Core.EngineOwners.IKernelCreateProgramSkillHostOperations kernelCoreGameEngineAdvancedLifecycleHostOperations,global::CardGame.Core.EngineOwners.IKernelTryResumeOperations kernelCoreGameEngineProgramContinuationsOperations,global::CardGame.Core.EngineOwners.IKernelTryAdvanceOperations nativeCardGameCoreModeNativeFrameRuntimeOperations)
    {
        this._characterHuaTuoAnniversaryGodOperations = characterHuaTuoAnniversaryGodOperations;
        this._kernelCoreGameEngineAdvancedLifecycleHostOperations = kernelCoreGameEngineAdvancedLifecycleHostOperations;
        this._kernelCoreGameEngineProgramContinuationsOperations = kernelCoreGameEngineProgramContinuationsOperations;
        this._nativeCardGameCoreModeNativeFrameRuntimeOperations = nativeCardGameCoreModeNativeFrameRuntimeOperations;
    }
}
