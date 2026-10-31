#nullable enable

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeFrameAutomaticRuntimeCoreModeNativeFrameRuntimeOperations : IProgramRuntimeOperations
{
    int StepCount(NativeFrameAutomaticPhase phase);
    NativeFrameAutomaticStepMetadata StepAt(NativeFrameAutomaticPhase phase, int index);
    bool Matches(NativeFrameAutomaticPhase phase, int index, ResolutionFrame frame);
    bool TryAdvance(ResolutionFrame frame);
}
