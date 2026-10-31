#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class RecipientContestConsequencesCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBlackGiftContestsEntries> getCoreGameEngineBlackGiftContestsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePrintedLordBenefitsEntries> getCoreGameEnginePrintedLordBenefitsRuntime) : IRecipientContestConsequencesHost
{
    public SkillProgramStepOutcome BlackGiftContest(ProgramSkillFrame f, int recipient) => getCoreGameEngineBlackGiftContestsRuntime().BeginBlackGiftContest(f, recipient);
    public SkillProgramStepOutcome PrintedLordBenefit(ProgramSkillFrame f) => getCoreGameEnginePrintedLordBenefitsRuntime().BeginPrintedLordBenefit(f);
}
