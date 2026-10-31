#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoAnniversaryGodEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversaryDuoruiLeaseEvent> ActiveAnnZlLeases() => _resolve().ActiveAnnZlLeases();
    internal bool AdvanceAnnZlChildren() => _resolve().AdvanceAnnZlChildren();
    internal global::CardGame.Core.PromptChoice AiAnnZlCuixi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAnnZlCuixi(p, f);
    internal global::CardGame.Core.PromptChoice AiAnnZlDuorui(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAnnZlDuorui(p, f);
    internal global::CardGame.Core.PromptChoice AiAnnZlZhiti(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiAnnZlZhiti(p, f);
    internal int AnnZlAbolished(int seat) => _resolve().AnnZlAbolished(seat);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.EquipmentSlot> AnnZlAvailable(int seat) => _resolve().AnnZlAvailable(seat);
    internal bool AnnZlBeforeDamageObserver(long id) => _resolve().AnnZlBeforeDamageObserver(id);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnnZlChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnnZlChoices(f);
    internal bool AnnZlDamageObserver(long id) => _resolve().AnnZlDamageObserver(id);
    internal bool AnnZlDying() => _resolve().AnnZlDying();
    internal bool AnnZlEligible(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().AnnZlEligible(c, t, w);
    internal bool AnnZlFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().AnnZlFirstChild(f, child);
    internal bool AnnZlMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().AnnZlMovement(f, effect, pending);
    internal global::CardGame.Core.ProgramSkillFrame? AnnZlObserverRoot() => _resolve().AnnZlObserverRoot();
    internal void AnswerAnnZl(global::CardGame.Core.PromptChoice chosen) => _resolve().AnswerAnnZl(chosen);
    internal void ApplyAnniversaryCuixiDrawSkip(int currentSeat) => _resolve().ApplyAnniversaryCuixiDrawSkip(currentSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnnZl(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginAnnZl(supplied, op);
    internal bool DrainAnnZl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainAnnZl(f);
    internal void FinishAnnZl(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishAnnZl(f);
    internal void ObserveAnnZlLeases(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveAnnZlLeases(payload);
    internal bool ResumeAnnZl(long id) => _resolve().ResumeAnnZl(id);
    internal bool RetainAnnZlPaid(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainAnnZlPaid(f);
    internal void ReturnAnnZlDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnAnnZlDraw(f, outcome);
    internal bool ReturnAnnZlMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnnZlMovement(f);
    internal void SaveAnnZlPayment(long id, long before, int ordinal) => _resolve().SaveAnnZlPayment(id, before, ordinal);
}
