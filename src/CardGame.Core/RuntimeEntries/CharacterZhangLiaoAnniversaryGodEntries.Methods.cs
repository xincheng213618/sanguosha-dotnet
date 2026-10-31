#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoAnniversaryGodEntries
{
    internal IEnumerable<AnniversaryDuoruiLeaseEvent> ActiveAnnZlLeases() => _resolve().ActiveAnnZlLeases();
    internal bool AdvanceAnnZlChildren() => _resolve().AdvanceAnnZlChildren();
    internal PromptChoice AiAnnZlCuixi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiAnnZlCuixi(p, f);
    internal PromptChoice AiAnnZlDuorui(PendingDecision p, ProgramSkillFrame f) => _resolve().AiAnnZlDuorui(p, f);
    internal PromptChoice AiAnnZlZhiti(PendingDecision p, ProgramSkillFrame f) => _resolve().AiAnnZlZhiti(p, f);
    internal int AnnZlAbolished(int seat) => _resolve().AnnZlAbolished(seat);
    internal IEnumerable<EquipmentSlot> AnnZlAvailable(int seat) => _resolve().AnnZlAvailable(seat);
    internal IReadOnlyList<PromptChoice> AnnZlChoices(ProgramSkillFrame f) => _resolve().AnnZlChoices(f);
    internal bool AnnZlDying() => _resolve().AnnZlDying();
    internal bool AnnZlEligible(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().AnnZlEligible(c, t, w);
    internal bool AnnZlFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().AnnZlFirstChild(f, child);
    internal bool AnnZlMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => _resolve().AnnZlMovement(f, effect, pending);
    internal ProgramSkillFrame? AnnZlObserverRoot() => _resolve().AnnZlObserverRoot();
    internal void AnswerAnnZl(PromptChoice chosen) => _resolve().AnswerAnnZl(chosen);
    internal void ApplyAnniversaryCuixiDrawSkip(int currentSeat) => _resolve().ApplyAnniversaryCuixiDrawSkip(currentSeat);
    internal SkillProgramStepOutcome BeginAnnZl(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginAnnZl(supplied, op);
    internal bool DrainAnnZl(ProgramSkillFrame f) => _resolve().DrainAnnZl(f);
    internal void FinishAnnZl(ProgramSkillFrame f) => _resolve().FinishAnnZl(f);
    internal void ObserveAnnZlLeases(IGameEvent payload) => _resolve().ObserveAnnZlLeases(payload);
    internal bool ResumeAnnZl(long id) => _resolve().ResumeAnnZl(id);
    internal bool RetainAnnZlPaid(ProgramSkillFrame f) => _resolve().RetainAnnZlPaid(f);
    internal void ReturnAnnZlDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnAnnZlDraw(f, outcome);
    internal bool ReturnAnnZlMovement(ProgramSkillFrame f) => _resolve().ReturnAnnZlMovement(f);
    internal void SaveAnnZlPayment(long id, long before, int ordinal) => _resolve().SaveAnnZlPayment(id, before, ordinal);
}
