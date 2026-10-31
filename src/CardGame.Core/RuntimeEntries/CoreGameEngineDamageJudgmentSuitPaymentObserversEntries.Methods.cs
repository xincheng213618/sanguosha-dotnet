#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentObserversEntries
{
    internal void AssertDamageJudgmentSuitPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertDamageJudgmentSuitPayment(frame);
    internal bool DamageJudgmentSuitPaymentEdge(int index, global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ProgramDamageJudgmentSuitPayment draft) => _resolve().DamageJudgmentSuitPaymentEdge(index, root, draft);
    internal global::CardGame.Core.ProgramSkillFrame? DamageJudgmentSuitPaymentRoot(long damageWindow) => _resolve().DamageJudgmentSuitPaymentRoot(damageWindow);
    internal bool HasDamageJudgmentMatchingClaim(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramDamageJudgmentSuitPayment draft, bool judged) => _resolve().HasDamageJudgmentMatchingClaim(frame, draft, judged);
    internal bool HasDamageJudgmentSuitPaymentObserver(long id) => _resolve().HasDamageJudgmentSuitPaymentObserver(id);
    internal bool HasDamageJudgmentSuitPaymentRide(long judgment) => _resolve().HasDamageJudgmentSuitPaymentRide(judgment);
    internal bool IsDamageJudgmentHpLossDyingRide(global::CardGame.Core.DyingFrame dying, global::CardGame.Core.ProgramSkillFrame program) => _resolve().IsDamageJudgmentHpLossDyingRide(dying, program);
    internal bool IsDamageJudgmentSuitPaymentDying() => _resolve().IsDamageJudgmentSuitPaymentDying();
    internal bool IsDamageJudgmentSuitPaymentMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsDamageJudgmentSuitPaymentMovement(frame, effect, movement);
    internal bool IsExactDamageJudgmentSuitPaymentFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => _resolve().IsExactDamageJudgmentSuitPaymentFinalizedSubtree(window, judgment, index);
    internal bool IsValidDamageJudgmentSuitPayment(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().IsValidDamageJudgmentSuitPayment(frame);
    internal global::CardGame.Core.PromptChoice SelectAiDamageJudgmentSuitPayment(global::CardGame.Core.PendingDecision decision) => _resolve().SelectAiDamageJudgmentSuitPayment(decision);
}
