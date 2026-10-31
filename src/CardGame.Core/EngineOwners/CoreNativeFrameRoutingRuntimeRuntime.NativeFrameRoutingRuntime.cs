#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFrameRoutingRuntimeRuntime
{
    bool global::CardGame.Core.INativeFrameRoutes.TryAdvance(ResolutionFrame frame) => _nativeCardGameCoreModeNativeFrameRuntimeOperations.TryAdvance(frame);
    bool global::CardGame.Core.INativeProgramExecution.TryResume(long frameId) => _characterHuaTuoAnniversaryGodOperations.TryBeginProgramSkillInvocation(frameId) || _kernelCoreGameEngineProgramContinuationsOperations.TryResume(global::CardGame.Core.EngineOwners.CoreGameEngineProgramContinuationsDefinitions.ProgramContinuations.Value, frameId);
    void global::CardGame.Core.INativeProgramExecution.Execute(long frameId)
    {
        if (_characterHuaTuoAnniversaryGodOperations.TryBeginProgramSkillInvocation(frameId))
            return;
        global::CardGame.Core.NativeOwners.ProgramSkillHost host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(frameId, host, host);
    }
}
