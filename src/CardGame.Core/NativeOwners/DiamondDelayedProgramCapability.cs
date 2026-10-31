#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class DiamondDelayedProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineDiamondDelayedProgramsRuntime>> getCoreGameEngineDiamondDelayedProgramsRuntime) : IDiamondDelayedProgramHost
{
    public SkillProgramStepOutcome UseDiamondDelayedOrDiscard(ProgramSkillFrame frame) => getCoreGameEngineDiamondDelayedProgramsRuntime().Value.BeginDiamondDelayed(frame);
}
