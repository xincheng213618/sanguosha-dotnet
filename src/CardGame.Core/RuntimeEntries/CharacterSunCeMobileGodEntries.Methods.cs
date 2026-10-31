#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunCeMobileGodEntries
{
    internal PromptChoice AiShenSunCeGift(PendingDecision d, ProgramSkillFrame f) => _resolve().AiShenSunCeGift(d, f);
    internal void AnswerShenSunCeGift(PromptChoice choice) => _resolve().AnswerShenSunCeGift(choice);
    internal SkillProgramStepOutcome BeginShenSunCe(ProgramSkillFrame supplied, SkillProgramEffectOp operation) => _resolve().BeginShenSunCe(supplied, operation);
    internal void ContinueShenSunCe(long id) => _resolve().ContinueShenSunCe(id);
    internal bool DrainShenSunCe(long id) => _resolve().DrainShenSunCe(id);
    internal void FinishShenSunCe(ProgramSkillFrame f) => _resolve().FinishShenSunCe(f);
    internal void IssueShenSunCeDraw(ProgramSkillFrame f, int count) => _resolve().IssueShenSunCeDraw(f, count);
    internal bool KeepPaidShenSunCe(ProgramSkillFrame f) => _resolve().KeepPaidShenSunCe(f);
    internal int Pingding(int seat) => _resolve().Pingding(seat);
    internal void PublishShenSunCeGift(ProgramSkillFrame f) => _resolve().PublishShenSunCeGift(f);
    internal bool ResumeShenSunCe(long id) => _resolve().ResumeShenSunCe(id);
    internal void ReturnShenSunCeDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShenSunCeDraw(supplied, outcome);
    internal bool ReturnShenSunCeMovement(ProgramSkillFrame f) => _resolve().ReturnShenSunCeMovement(f);
    internal bool ShenSunCeCannotRespond(int source, int responder, CardKind kind, IReadOnlyList<int> targets) => _resolve().ShenSunCeCannotRespond(source, responder, kind, targets);
    internal bool ShenSunCeCardDistance(ProgramCardDistanceQuery q) => _resolve().ShenSunCeCardDistance(q);
    internal void ShenSunCeChangeMaximum(ProgramSkillFrame f, int seat, int delta) => _resolve().ShenSunCeChangeMaximum(f, seat, delta);
    internal bool ShenSunCeDistance(CharacterState actor, CharacterState target) => _resolve().ShenSunCeDistance(actor, target);
    internal IReadOnlyList<PromptChoice> ShenSunCeGiftChoices(ProgramSkillFrame f) => _resolve().ShenSunCeGiftChoices(f);
    internal bool ShenSunCeSlashDistance(ProgramSlashDistanceQuery q) => _resolve().ShenSunCeSlashDistance(q);
    internal bool ShenSunCeTriggerLegal(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => _resolve().ShenSunCeTriggerLegal(c, t, w);
}
