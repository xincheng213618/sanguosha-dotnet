#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrepPoisonOutcomeCapability(Func<RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime) : IPrepPoisonOutcomeHost
{
    public SkillProgramStepOutcome ResolvePrepPoisonOutcomeByRound(ProgramSkillFrame frame) => getCoreGameEnginePrepPoisonOutcomesRuntime().ResolvePrepPoisonOutcomeByRound(frame);
}
