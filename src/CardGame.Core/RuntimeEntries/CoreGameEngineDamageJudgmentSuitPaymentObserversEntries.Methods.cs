#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageJudgmentSuitPaymentObserversEntries
{
    internal bool DamageJudgmentSuitPaymentEdge(int index, ProgramSkillFrame root, ProgramDamageJudgmentSuitPayment draft) => _resolve().DamageJudgmentSuitPaymentEdge(index, root, draft);
    internal ProgramSkillFrame? DamageJudgmentSuitPaymentRoot(long damageWindow) => _resolve().DamageJudgmentSuitPaymentRoot(damageWindow);
    internal bool HasDamageJudgmentMatchingClaim(ProgramSkillFrame frame, ProgramDamageJudgmentSuitPayment draft, bool judged) => _resolve().HasDamageJudgmentMatchingClaim(frame, draft, judged);
    internal bool IsDamageJudgmentHpLossDyingRide(DyingFrame dying, ProgramSkillFrame program) => _resolve().IsDamageJudgmentHpLossDyingRide(dying, program);
    internal bool IsDamageJudgmentSuitPaymentDying() => _resolve().IsDamageJudgmentSuitPaymentDying();
    internal bool IsDamageJudgmentSuitPaymentMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) => _resolve().IsDamageJudgmentSuitPaymentMovement(frame, effect, movement);
    internal bool IsValidDamageJudgmentSuitPayment(ProgramSkillFrame frame) => _resolve().IsValidDamageJudgmentSuitPayment(frame);
    internal PromptChoice SelectAiDamageJudgmentSuitPayment(PendingDecision decision) => _resolve().SelectAiDamageJudgmentSuitPayment(decision);
}
