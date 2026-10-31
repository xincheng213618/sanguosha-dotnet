#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoHappyGodEntries
{
    internal IEnumerable<HappyZlLeaseEvent> ActiveHappyZlLeases(int owner, int target) => _resolve().ActiveHappyZlLeases(owner, target);
    internal bool AdvanceHappyZlChildren() => _resolve().AdvanceHappyZlChildren();
    internal PromptChoice AiHappyZlDamage(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHappyZlDamage(p, f);
    internal PromptChoice AiHappyZlTake(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHappyZlTake(p, f);
    internal PromptChoice AiHappyZlZhiti(PendingDecision p, ProgramSkillFrame f) => _resolve().AiHappyZlZhiti(p, f);
    internal void AnswerHappyZl(PromptChoice chosen) => _resolve().AnswerHappyZl(chosen);
    internal SkillProgramStepOutcome BeginHappyZl(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginHappyZl(supplied, op);
    internal void FinishHappyZl(ProgramSkillFrame f) => _resolve().FinishHappyZl(f);
    internal bool HappyZlCannotRespond(CardActionContext action, int responderSeat) => _resolve().HappyZlCannotRespond(action, responderSeat);
    internal bool HappyZlCannotRespond2(long useId, int responderSeat) => _resolve().HappyZlCannotRespond(useId, responderSeat);
    internal IReadOnlyList<PromptChoice> HappyZlChoices(ProgramSkillFrame f) => _resolve().HappyZlChoices(f);
    internal HappyZlColorEvent? HappyZlColor(int owner, int target, bool? color) => _resolve().HappyZlColor(owner, target, color);
    internal CardUseFrame? HappyZlDamageUse(int owner, ProgramSkillWindowContext w) => _resolve().HappyZlDamageUse(owner, w);
    internal bool HappyZlDying() => _resolve().HappyZlDying();
    internal bool HappyZlEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().HappyZlEligible(c, t, w);
    internal bool HappyZlFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().HappyZlFirstChild(f, child);
    internal bool HappyZlMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => _resolve().HappyZlMovement(f, e, p);
    internal bool HappyZlNullificationForbidden(NullificationWindowFrame window, int responderSeat) => _resolve().HappyZlNullificationForbidden(window, responderSeat);
    internal IEnumerable<RuleQueryContribution> HappyZlNumericContributions(CharacterState player, SkillRuleQuery query) => _resolve().HappyZlNumericContributions(player, query);
    internal ProgramSkillFrame? HappyZlObserverRoot() => _resolve().HappyZlObserverRoot();
    internal CardSnapshot[] HappyZlPrivateHand(int viewer) => _resolve().HappyZlPrivateHand(viewer);
    internal int HappyZlScore() => _resolve().HappyZlScore();
    internal bool HasHappyZlSourceRestoration(int source, int target) => _resolve().HasHappyZlSourceRestoration(source, target);
    internal void ObserveHappyZlCounterspell(IGameEvent payload) => _resolve().ObserveHappyZlCounterspell(payload);
    internal void ObserveHappyZlSlots(IGameEvent payload) => _resolve().ObserveHappyZlSlots(payload);
    internal bool ResumeHappyZl(long id) => _resolve().ResumeHappyZl(id);
    internal bool RetainHappyZlPaid(ProgramSkillFrame f) => _resolve().RetainHappyZlPaid(f);
    internal void ReturnHappyZlDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnHappyZlDraw(f, outcome);
    internal bool ReturnHappyZlMovement(ProgramSkillFrame f) => _resolve().ReturnHappyZlMovement(f);
    internal void SaveHappyZlPayment(long id, long before) => _resolve().SaveHappyZlPayment(id, before);
    internal bool HasHappyZl { get => _resolve().HasHappyZl; }
}
