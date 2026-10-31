#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePrintedLordBenefitsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPrintedLordBenefit(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginPrintedLordBenefit(input);
    internal bool ExactPrintedLordStart(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactPrintedLordStart(f);
    internal int[] PrintedLordBeneficiaries(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrintedLordBeneficiaries(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> PrintedLordChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrintedLordChoices(f);
    internal bool PrintedLordUnissuedSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PrintedLordUnissuedSource(f);
    internal void PublishPrintedLordChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishPrintedLordChoice(f);
    internal void ResolvePrintedLordChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolvePrintedLordChoice(choice);
    internal bool ResumePrintedLordBenefit(long id) => _resolve().ResumePrintedLordBenefit(id);
    internal global::CardGame.Core.PromptChoice SelectAiRecipientConsequences(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiRecipientConsequences(p, f);
}
