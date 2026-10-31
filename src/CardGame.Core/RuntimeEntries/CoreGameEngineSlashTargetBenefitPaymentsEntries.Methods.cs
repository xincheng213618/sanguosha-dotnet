#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSlashTargetBenefitPaymentsEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome CompleteSlashBenefitOperation(global::CardGame.Core.ProgramSkillFrame f, bool paid) => _resolve().CompleteSlashBenefitOperation(f, paid);
    internal void ContinueSlashBenefitPaymentChildren(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSlashBenefitPaymentChildren(f);
    internal void FinishResumedSlashBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishResumedSlashBenefit(f);
    internal global::CardGame.Core.SkillProgramStepOutcome OfferSlashTargetBenefit(global::CardGame.Core.ProgramSkillFrame supplied, string settlementBinding) => _resolve().OfferSlashTargetBenefit(supplied, settlementBinding);
    internal void PaySlashBenefitDiscard(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().PaySlashBenefitDiscard(f, card, from);
    internal void PaySlashBenefitDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PaySlashBenefitDraw(f);
    internal void PublishSlashBenefitPaid(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SlashTargetBenefitReceipt paid, int actual) => _resolve().PublishSlashBenefitPaid(f, paid, actual);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveSlashTargetBenefit(global::CardGame.Core.ProgramSkillFrame supplied, bool settlement, string? settlementBinding) => _resolve().ResolveSlashTargetBenefit(supplied, settlement, settlementBinding);
    internal void ResolveSlashTargetBenefitChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveSlashTargetBenefitChoice(selected);
    internal bool ResumeSlashTargetBenefit(long frameId) => _resolve().ResumeSlashTargetBenefit(frameId);
    internal bool ReturnSlashBenefitMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnSlashBenefitMovement(f);
    internal global::CardGame.Core.PromptChoice SelectAiSlashTargetBenefit(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SelectAiSlashTargetBenefit(decision, f);
    internal global::CardGame.Core.SkillProgramStepOutcome SettleDodgeCancelledSlashBenefit(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().SettleDodgeCancelledSlashBenefit(supplied);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SlashBenefitChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SlashBenefitChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> SlashBenefitOwnedChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SlashBenefitOwnedChoices(f);
    internal global::System.Collections.Generic.Dictionary<string, string> SlashBenefitParameters(global::CardGame.Core.ProgramSkillFrame f, string step) => _resolve().SlashBenefitParameters(f, step);
    internal bool SlashBenefitProgramParentMatches(global::CardGame.Core.ProgramSkillFrame f) => _resolve().SlashBenefitProgramParentMatches(f);
    internal long SlashBenefitSequence { get => _resolve().SlashBenefitSequence; }
}
