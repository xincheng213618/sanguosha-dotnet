#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability]
internal sealed class CoreNativeFrameAutomaticRuntimeCoreModeNativeFrameRuntimeAdapter(GameEngine engine) : ICoreNativeFrameAutomaticRuntimeCoreModeNativeFrameRuntimeOperations
{
    public int StepCount(NativeFrameAutomaticPhase phase) => engine.GetRuntimeModule<CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase].Count;
    public NativeFrameAutomaticStepMetadata StepAt(NativeFrameAutomaticPhase phase, int index) => engine.GetRuntimeModule<CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Step;
    public bool Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame) => engine.GetRuntimeModule<CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.Automatic[phase][index].Matches(engine, frame);
    public bool TryAdvance(ResolutionFrame frame) => engine.GetRuntimeModule<CoreModeNativeFrameRuntimeRuntime>().ModeNativeFrames.TryAdvance(engine, frame);
}
