#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class OverflowTargetCancellationCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationEntries> getCoreGameEngineOverflowTargetCancellationRuntime) : IOverflowTargetCancellationHost
{
    public SkillProgramStepOutcome BeginOverflowTargetCancellation(ProgramSkillFrame frame) => getCoreGameEngineOverflowTargetCancellationRuntime().BeginOverflowTargetCancellation(frame);
}
