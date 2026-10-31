#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class RecipientContestConsequencesCapability(Func<RuntimeEntries.CoreGameEngineBlackGiftContestsEntries> getCoreGameEngineBlackGiftContestsRuntime, Func<RuntimeEntries.CoreGameEnginePrintedLordBenefitsEntries> getCoreGameEnginePrintedLordBenefitsRuntime) : IRecipientContestConsequencesHost
{
    public SkillProgramStepOutcome BlackGiftContest(ProgramSkillFrame f, int recipient) => getCoreGameEngineBlackGiftContestsRuntime().BeginBlackGiftContest(f, recipient);
    public SkillProgramStepOutcome PrintedLordBenefit(ProgramSkillFrame f) => getCoreGameEnginePrintedLordBenefitsRuntime().BeginPrintedLordBenefit(f);
}
