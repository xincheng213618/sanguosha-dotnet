#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetTurnCountCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFinalTargetTurnCountsEntries> getCoreGameEngineFinalTargetTurnCountsRuntime) : IFinalTargetTurnCountHost
{
    public SkillProgramStepOutcome ExecuteFinalTargetTurnCount(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineFinalTargetTurnCountsRuntime().ExecuteFinalTargetTurnCount(effect, frame);
}
