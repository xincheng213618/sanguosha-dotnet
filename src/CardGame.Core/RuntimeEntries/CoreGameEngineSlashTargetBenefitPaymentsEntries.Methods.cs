#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitPaymentsEntries
{
    internal SkillProgramStepOutcome CompleteSlashBenefitOperation(ProgramSkillFrame f, bool paid) => _resolve().CompleteSlashBenefitOperation(f, paid);
    internal void ContinueSlashBenefitPaymentChildren(ProgramSkillFrame f) => _resolve().ContinueSlashBenefitPaymentChildren(f);
    internal void FinishResumedSlashBenefit(ProgramSkillFrame f) => _resolve().FinishResumedSlashBenefit(f);
    internal SkillProgramStepOutcome OfferSlashTargetBenefit(ProgramSkillFrame supplied, string settlementBinding) => _resolve().OfferSlashTargetBenefit(supplied, settlementBinding);
    internal void PaySlashBenefitDiscard(ProgramSkillFrame f, Card card, CardLocation from) => _resolve().PaySlashBenefitDiscard(f, card, from);
    internal void PaySlashBenefitDraw(ProgramSkillFrame f) => _resolve().PaySlashBenefitDraw(f);
    internal void PublishSlashBenefitPaid(ProgramSkillFrame f, SlashTargetBenefitReceipt paid, int actual) => _resolve().PublishSlashBenefitPaid(f, paid, actual);
    internal SkillProgramStepOutcome ResolveSlashTargetBenefit(ProgramSkillFrame supplied, bool settlement, string? settlementBinding) => _resolve().ResolveSlashTargetBenefit(supplied, settlement, settlementBinding);
    internal void ResolveSlashTargetBenefitChoice(PromptChoice selected) => _resolve().ResolveSlashTargetBenefitChoice(selected);
    internal bool ResumeSlashTargetBenefit(long frameId) => _resolve().ResumeSlashTargetBenefit(frameId);
    internal bool ReturnSlashBenefitMovement(ProgramSkillFrame f) => _resolve().ReturnSlashBenefitMovement(f);
    internal PromptChoice SelectAiSlashTargetBenefit(PendingDecision decision, ProgramSkillFrame f) => _resolve().SelectAiSlashTargetBenefit(decision, f);
    internal SkillProgramStepOutcome SettleDodgeCancelledSlashBenefit(ProgramSkillFrame supplied) => _resolve().SettleDodgeCancelledSlashBenefit(supplied);
    internal IReadOnlyList<PromptChoice> SlashBenefitChoices(ProgramSkillFrame f) => _resolve().SlashBenefitChoices(f);
    internal IReadOnlyList<PromptChoice> SlashBenefitOwnedChoices(ProgramSkillFrame f) => _resolve().SlashBenefitOwnedChoices(f);
    internal Dictionary<string, string> SlashBenefitParameters(ProgramSkillFrame f, string step) => _resolve().SlashBenefitParameters(f, step);
    internal bool SlashBenefitProgramParentMatches(ProgramSkillFrame f) => _resolve().SlashBenefitProgramParentMatches(f);
    internal long SlashBenefitSequence { get => _resolve().SlashBenefitSequence; }
}
