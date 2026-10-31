#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetTurnCountCapability(Func<RuntimeEntries.CoreGameEngineFinalTargetTurnCountsEntries> getCoreGameEngineFinalTargetTurnCountsRuntime) : IFinalTargetTurnCountHost
{
    public SkillProgramStepOutcome ExecuteFinalTargetTurnCount(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineFinalTargetTurnCountsRuntime().ExecuteFinalTargetTurnCount(effect, frame);
}
