#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiamondDelayedProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDiamondDelayedProgramsEntries> getCoreGameEngineDiamondDelayedProgramsRuntime) : IDiamondDelayedProgramHost
{
    public SkillProgramStepOutcome UseDiamondDelayedOrDiscard(ProgramSkillFrame frame) => getCoreGameEngineDiamondDelayedProgramsRuntime().BeginDiamondDelayed(frame);
}
