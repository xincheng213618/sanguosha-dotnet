#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class PrepPoisonOutcomeCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepPoisonOutcomesEntries> getCoreGameEnginePrepPoisonOutcomesRuntime) : IPrepPoisonOutcomeHost
{
    public SkillProgramStepOutcome ResolvePrepPoisonOutcomeByRound(ProgramSkillFrame frame) => getCoreGameEnginePrepPoisonOutcomesRuntime().ResolvePrepPoisonOutcomeByRound(frame);
}
