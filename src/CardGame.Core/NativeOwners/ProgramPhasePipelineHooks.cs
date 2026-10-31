#nullable enable

namespace CardGame.Core.NativeOwners;

internal sealed class ProgramPhasePipelineHooks(global::System.Func<global::CardGame.Core.ProgramPipelineHooks> getProgramPipelineHooks) : IProgramPhasePipelineHooks
    {
        public bool ResumeScheduledPhase() => getProgramPipelineHooks().ResumeScheduledPhase();
        public bool CancelScheduledPhase() => getProgramPipelineHooks().CancelScheduledPhase();
        public bool ContinueDetachedProgramBinding(ProgramSkillFrame frame) => getProgramPipelineHooks().ContinueDetachedProgramBinding(frame);
    }
