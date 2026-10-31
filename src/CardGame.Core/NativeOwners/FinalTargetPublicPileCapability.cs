#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetPublicPileCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFinalTargetPublicPilesRuntime>> getCoreGameEngineFinalTargetPublicPilesRuntime) : IFinalTargetPublicPileHost
{
    public SkillProgramStepOutcome ExecuteFinalTargetPublicPile(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineFinalTargetPublicPilesRuntime().Value.ExecuteFinalTargetPublicPile(e, f);
}
