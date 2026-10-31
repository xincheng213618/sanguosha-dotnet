#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class RandomAreaAcquisitionCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRandomAreaAcquisitionEntries> getCoreGameEngineRandomAreaAcquisitionRuntime) : IRandomAreaAcquisitionHost
{
    public SkillProgramStepOutcome ObtainRandomAreaFromEachOtherThenTurnOver(ProgramSkillFrame frame) => getCoreGameEngineRandomAreaAcquisitionRuntime().ObtainRandomAreaFromEachOtherThenTurnOver(frame);
}
