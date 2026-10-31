#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRuntimeEntries
{
    internal void AdvanceOneStepCore() => _resolve().AdvanceOneStepCore();
    internal void AdvanceRuntimeFrame(long frameId) => _resolve().AdvanceRuntimeFrame(frameId);
    internal void AdvanceRuntimeProgram(long frameId) => _resolve().AdvanceRuntimeProgram(frameId);
    internal void AdvanceRuntimeTop<TFrame>()
        where TFrame : ResolutionFrame => _resolve().AdvanceRuntimeTop<TFrame>();
    internal void CompleteRuntimeProgramBinding(ProgramSkillFrame frame, bool completed) => _resolve().CompleteRuntimeProgramBinding(frame, completed);
    internal void PopResolutionFrame(long frameId, ResolutionFrameKind expectedKind) => _resolve().PopResolutionFrame(frameId, expectedKind);
    internal void PushRuntimeFrame(ResolutionFrame frame) => _resolve().PushRuntimeFrame(frame);
    internal void ReplaceRuntimeFrame(long expectedFrameId, ResolutionFrame next) => _resolve().ReplaceRuntimeFrame(expectedFrameId, next);
    internal void ReplaceRuntimeTop(ResolutionFrame next) => _resolve().ReplaceRuntimeTop(next);
    internal void ReturnRuntimeProgramMovement(long frameId) => _resolve().ReturnRuntimeProgramMovement(frameId);
    internal void RunOneEngineStep() => _resolve().RunOneEngineStep();
    internal ProgramCompletionRuntime ProgramCompletion { get => _resolve().ProgramCompletion; }
}
