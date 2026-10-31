#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoHappyGodEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.HappyZlLeaseEvent> ActiveHappyZlLeases(int owner, int target) => _resolve().ActiveHappyZlLeases(owner, target);
    internal bool AdvanceHappyZlChildren() => _resolve().AdvanceHappyZlChildren();
    internal global::CardGame.Core.PromptChoice AiHappyZlDamage(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHappyZlDamage(p, f);
    internal global::CardGame.Core.PromptChoice AiHappyZlTake(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHappyZlTake(p, f);
    internal global::CardGame.Core.PromptChoice AiHappyZlZhiti(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiHappyZlZhiti(p, f);
    internal void AnswerHappyZl(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerHappyZl(chosen);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginHappyZl(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginHappyZl(supplied, op);
    internal void FinishHappyZl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHappyZl(f);
    internal bool HappyZlBeforeDamageObserver(long id) => _resolve().HappyZlBeforeDamageObserver(id);
    internal bool HappyZlCannotRespond(global::CardGame.Core.CardActionContext action, int responderSeat) => _resolve().HappyZlCannotRespond(action, responderSeat);
    internal bool HappyZlCannotRespond2(long useId, int responderSeat) => _resolve().HappyZlCannotRespond(useId, responderSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HappyZlChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HappyZlChoices(f);
    internal global::CardGame.Core.HappyZlColorEvent? HappyZlColor(int owner, int target, bool? color) => _resolve().HappyZlColor(owner, target, color);
    internal bool HappyZlDamageObserver(long id) => _resolve().HappyZlDamageObserver(id);
    internal global::CardGame.Core.CardUseFrame? HappyZlDamageUse(int owner, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HappyZlDamageUse(owner, w);
    internal bool HappyZlDying() => _resolve().HappyZlDying();
    internal bool HappyZlEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().HappyZlEligible(c, t, w);
    internal bool HappyZlFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().HappyZlFirstChild(f, child);
    internal bool HappyZlMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HappyZlMovement(f, e, p);
    internal bool HappyZlNullificationForbidden(global::CardGame.Core.NullificationWindowFrame window, int responderSeat) => _resolve().HappyZlNullificationForbidden(window, responderSeat);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> HappyZlNumericContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().HappyZlNumericContributions(player, query);
    internal global::CardGame.Core.ProgramSkillFrame? HappyZlObserverRoot() => _resolve().HappyZlObserverRoot();
    internal global::CardGame.Core.CardSnapshot[] HappyZlPrivateHand(int viewer) => _resolve().HappyZlPrivateHand(viewer);
    internal int HappyZlScore() => _resolve().HappyZlScore();
    internal bool HasHappyZlSourceRestoration(int source, int target) => _resolve().HasHappyZlSourceRestoration(source, target);
    internal void ObserveHappyZlCounterspell(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveHappyZlCounterspell(payload);
    internal void ObserveHappyZlSlots(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveHappyZlSlots(payload);
    internal void RequireHappyZl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireHappyZl(f);
    internal bool ResumeHappyZl(long id) => _resolve().ResumeHappyZl(id);
    internal bool RetainHappyZlPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainHappyZlPaid(f);
    internal void ReturnHappyZlDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHappyZlDraw(f, outcome);
    internal bool ReturnHappyZlMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHappyZlMovement(f);
    internal void SaveHappyZlPayment(long id, long before) => _resolve().SaveHappyZlPayment(id, before);
    internal bool HasHappyZl { get => _resolve().HasHappyZl; }
}
