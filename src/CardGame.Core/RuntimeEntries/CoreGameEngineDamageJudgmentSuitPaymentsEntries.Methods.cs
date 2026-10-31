#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentsEntries
{
    internal CardUseFrame? ActualDamageJudgmentUse(ProgramSkillWindowContext context) => _resolve().ActualDamageJudgmentUse(context);
    internal void BeginDamageJudgmentCleanup(ProgramSkillFrame frame) => _resolve().BeginDamageJudgmentCleanup(frame);
    internal SkillProgramStepOutcome BeginDamageJudgmentSuitPayment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().BeginDamageJudgmentSuitPayment(frame, effect);
    internal bool CanRunDamageJudgmentSuitPayment(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int owner) => _resolve().CanRunDamageJudgmentSuitPayment(trigger, context, owner);
    internal void CaptureDamageJudgmentSuitResult(JudgmentFrame judgment, Card card, Suit suit) => _resolve().CaptureDamageJudgmentSuitResult(judgment, card, suit);
    internal void ClaimDamageJudgmentMatchingCard(ProgramSkillFrame frame, int id, bool judged) => _resolve().ClaimDamageJudgmentMatchingCard(frame, id, judged);
    internal IReadOnlyList<PromptChoice> DamageJudgmentPaymentChoices(ProgramSkillFrame frame, bool sourceDiscard) => _resolve().DamageJudgmentPaymentChoices(frame, sourceDiscard);
    internal bool DamageJudgmentUseMatches(ProgramDamageJudgmentSuitPayment draft, CardKind? kind) => _resolve().DamageJudgmentUseMatches(draft, kind);
    internal bool IsDamageJudgmentInstructionTarget(ProgramSkillFrame frame, int target) => _resolve().IsDamageJudgmentInstructionTarget(frame, target);
    internal long? LegacyDamageJudgmentVirtualProducer(CardUseFrame use, int target) => _resolve().LegacyDamageJudgmentVirtualProducer(use, target);
    internal void PublishDamageJudgmentPayment(ProgramSkillFrame frame, bool sourceDiscard) => _resolve().PublishDamageJudgmentPayment(frame, sourceDiscard);
    internal void ResolveDamageJudgmentSuitPaymentChoice(PromptChoice selected) => _resolve().ResolveDamageJudgmentSuitPaymentChoice(selected);
    internal bool ResumeDamageJudgmentSuitPayment(long frameId) => _resolve().ResumeDamageJudgmentSuitPayment(frameId);
    internal bool ReturnDamageJudgmentSuitPaymentMovement(ProgramSkillFrame frame) => _resolve().ReturnDamageJudgmentSuitPaymentMovement(frame);
    internal bool TryReturnDamageJudgmentSuitPayment(JudgmentFrame judgment) => _resolve().TryReturnDamageJudgmentSuitPayment(judgment);
}
