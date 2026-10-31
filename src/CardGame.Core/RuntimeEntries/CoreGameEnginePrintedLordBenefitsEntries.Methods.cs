#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrintedLordBenefitsEntries
{
    internal SkillProgramStepOutcome BeginPrintedLordBenefit(ProgramSkillFrame input) => _resolve().BeginPrintedLordBenefit(input);
    internal bool ExactPrintedLordStart(ProgramSkillFrame f) => _resolve().ExactPrintedLordStart(f);
    internal int[] PrintedLordBeneficiaries(ProgramSkillFrame f) => _resolve().PrintedLordBeneficiaries(f);
    internal IReadOnlyList<PromptChoice> PrintedLordChoices(ProgramSkillFrame f) => _resolve().PrintedLordChoices(f);
    internal bool PrintedLordUnissuedSource(ProgramSkillFrame f) => _resolve().PrintedLordUnissuedSource(f);
    internal void PublishPrintedLordChoice(ProgramSkillFrame f) => _resolve().PublishPrintedLordChoice(f);
    internal void ResolvePrintedLordChoice(PromptChoice choice) => _resolve().ResolvePrintedLordChoice(choice);
    internal bool ResumePrintedLordBenefit(long id) => _resolve().ResumePrintedLordBenefit(id);
    internal PromptChoice SelectAiRecipientConsequences(PendingDecision p, ProgramSkillFrame f) => _resolve().SelectAiRecipientConsequences(p, f);
}
