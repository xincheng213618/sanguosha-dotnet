#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetPublicPileCapability(Func<RuntimeEntries.CoreGameEngineFinalTargetPublicPilesEntries> getCoreGameEngineFinalTargetPublicPilesRuntime) : IFinalTargetPublicPileHost
{
    public SkillProgramStepOutcome ExecuteFinalTargetPublicPile(SkillProgramEffect e, ProgramSkillFrame f) => getCoreGameEngineFinalTargetPublicPilesRuntime().ExecuteFinalTargetPublicPile(e, f);
}
