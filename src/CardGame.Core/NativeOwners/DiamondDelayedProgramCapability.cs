#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiamondDelayedProgramCapability(Func<RuntimeEntries.CoreGameEngineDiamondDelayedProgramsEntries> getCoreGameEngineDiamondDelayedProgramsRuntime) : IDiamondDelayedProgramHost
{
    public SkillProgramStepOutcome UseDiamondDelayedOrDiscard(ProgramSkillFrame frame) => getCoreGameEngineDiamondDelayedProgramsRuntime().BeginDiamondDelayed(frame);
}
