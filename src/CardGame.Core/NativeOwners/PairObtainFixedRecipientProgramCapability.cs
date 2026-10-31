#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class PairObtainFixedRecipientProgramCapability(Func<RuntimeEntries.CoreGameEnginePairObtainShownGiftsEntries> getCoreGameEnginePairObtainShownGiftsRuntime, Func<RuntimeEntries.CoreGameEngineFixedRecipientBenefitsEntries> getCoreGameEngineFixedRecipientBenefitsRuntime) : IPairObtainFixedRecipientProgramHost
{
    public SkillProgramStepOutcome ObtainOneFromEachSelectedTarget(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEnginePairObtainShownGiftsRuntime().ObtainOneFromEachSelectedTarget(f, e);
    public SkillProgramStepOutcome GiveShownCardToLeastOriginalTarget(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEnginePairObtainShownGiftsRuntime().GiveShownCardToLeastOriginalTarget(f, e);
    public SkillProgramStepOutcome IssueFixedRecipientBenefit(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineFixedRecipientBenefitsRuntime().IssueFixedRecipientBenefit(f, e);
    public SkillProgramStepOutcome SelectIssuedFixedRecipient(ProgramSkillFrame f, SkillProgramEffect e) => getCoreGameEngineFixedRecipientBenefitsRuntime().SelectIssuedFixedRecipient(f, e);
}
