#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class FinalTargetTurnCountCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFinalTargetTurnCountsRuntime>> getCoreGameEngineFinalTargetTurnCountsRuntime) : IFinalTargetTurnCountHost
{
    public SkillProgramStepOutcome ExecuteFinalTargetTurnCount(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineFinalTargetTurnCountsRuntime().Value.ExecuteFinalTargetTurnCount(effect, frame);
}
