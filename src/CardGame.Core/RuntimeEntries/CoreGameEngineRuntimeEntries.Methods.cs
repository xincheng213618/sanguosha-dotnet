#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRuntimeEntries
{
    internal void AdvanceOneStepCore() => _resolve().AdvanceOneStepCore();
    internal void AdvanceRuntimeFrame(long frameId) => _resolve().AdvanceRuntimeFrame(frameId);
    internal void AdvanceRuntimeProgram(long frameId) => _resolve().AdvanceRuntimeProgram(frameId);
    internal void AdvanceRuntimeTop<TFrame>()
        where TFrame : global::CardGame.Core.ResolutionFrame => _resolve().AdvanceRuntimeTop<TFrame>();
    internal void CompleteRuntimeProgramBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().CompleteRuntimeProgramBinding(frame, completed);
    internal void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind) => _resolve().PopResolutionFrame(frameId, expectedKind);
    internal void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame) => _resolve().PushRuntimeFrame(frame);
    internal void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next) => _resolve().ReplaceRuntimeFrame(expectedFrameId, next);
    internal void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next) => _resolve().ReplaceRuntimeTop(next);
    internal void ReturnRuntimeProgramMovement(long frameId) => _resolve().ReturnRuntimeProgramMovement(frameId);
    internal void RunOneEngineStep() => _resolve().RunOneEngineStep();
    internal global::CardGame.Core.ProgramCompletionRuntime ProgramCompletion { get => _resolve().ProgramCompletion; }
}
