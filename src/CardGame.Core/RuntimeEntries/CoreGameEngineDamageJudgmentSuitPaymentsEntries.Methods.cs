#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentsEntries
{
    internal global::CardGame.Core.CardUseFrame? ActualDamageJudgmentUse(global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ActualDamageJudgmentUse(context);
    internal void BeginDamageJudgmentCleanup(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().BeginDamageJudgmentCleanup(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDamageJudgmentSuitPayment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().BeginDamageJudgmentSuitPayment(frame, effect);
    internal bool CanRunDamageJudgmentSuitPayment(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int owner) => _resolve().CanRunDamageJudgmentSuitPayment(trigger, context, owner);
    internal void CaptureDamageJudgmentSuitResult(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.Card card, global::CardGame.Core.Suit suit) => _resolve().CaptureDamageJudgmentSuitResult(judgment, card, suit);
    internal void ClaimDamageJudgmentMatchingCard(global::CardGame.Core.ProgramSkillFrame frame, int id, bool judged) => _resolve().ClaimDamageJudgmentMatchingCard(frame, id, judged);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DamageJudgmentPaymentChoices(global::CardGame.Core.ProgramSkillFrame frame, bool sourceDiscard) => _resolve().DamageJudgmentPaymentChoices(frame, sourceDiscard);
    internal bool DamageJudgmentUseMatches(global::CardGame.Core.ProgramDamageJudgmentSuitPayment draft, global::CardGame.Core.CardKind? kind) => _resolve().DamageJudgmentUseMatches(draft, kind);
    internal bool IsDamageJudgmentInstructionTarget(global::CardGame.Core.ProgramSkillFrame frame, int target) => _resolve().IsDamageJudgmentInstructionTarget(frame, target);
    internal long? LegacyDamageJudgmentVirtualProducer(global::CardGame.Core.CardUseFrame use, int target) => _resolve().LegacyDamageJudgmentVirtualProducer(use, target);
    internal void PublishDamageJudgmentPayment(global::CardGame.Core.ProgramSkillFrame frame, bool sourceDiscard) => _resolve().PublishDamageJudgmentPayment(frame, sourceDiscard);
    internal void ResolveDamageJudgmentSuitPaymentChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveDamageJudgmentSuitPaymentChoice(selected);
    internal bool ResumeDamageJudgmentSuitPayment(long frameId) => _resolve().ResumeDamageJudgmentSuitPayment(frameId);
    internal bool ReturnDamageJudgmentSuitPaymentMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnDamageJudgmentSuitPaymentMovement(frame);
    internal bool TryReturnDamageJudgmentSuitPayment(global::CardGame.Core.JudgmentFrame judgment) => _resolve().TryReturnDamageJudgmentSuitPayment(judgment);
}
