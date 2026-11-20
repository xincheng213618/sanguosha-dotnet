#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFrameRoutingRuntimeRuntime
{
    bool INativeFrameRoutes.TryAdvance(ResolutionFrame frame) => _nativeCardGameCoreModeNativeFrameRuntimeOperations.TryAdvance(frame);
    bool INativeProgramExecution.TryResume(long frameId) =>
        pipelineHooks._programPipelineHooks.TryBeginProgramInvocation(frameId) ||
        _kernelCoreGameEngineProgramContinuationsOperations.TryResume(CoreGameEngineProgramContinuationsDefinitions.ProgramContinuations.Value, frameId);
    void INativeProgramExecution.Execute(long frameId)
    {
        if (pipelineHooks._programPipelineHooks.TryBeginProgramInvocation(frameId))
            return;
        NativeOwners.ProgramSkillHost host = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost();
        new SkillProgramExecutor().Run(frameId, host, host);
    }
}
