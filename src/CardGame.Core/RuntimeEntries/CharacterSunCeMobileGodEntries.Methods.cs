#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunCeMobileGodEntries
{
    internal global::CardGame.Core.PromptChoice AiShenSunCeGift(global::CardGame.Core.PendingDecision d, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiShenSunCeGift(d, f);
    internal void AnswerShenSunCeGift(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerShenSunCeGift(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginShenSunCe(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginShenSunCe(supplied, operation);
    internal void ContinueShenSunCe(long id) => _resolve().ContinueShenSunCe(id);
    internal bool DrainShenSunCe(long id) => _resolve().DrainShenSunCe(id);
    internal void FinishShenSunCe(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishShenSunCe(f);
    internal bool IsShenSunCePaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsShenSunCePaidMovement(f, effect, pending);
    internal void IssueShenSunCeDraw(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().IssueShenSunCeDraw(f, count);
    internal bool KeepPaidShenSunCe(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepPaidShenSunCe(f);
    internal int Pingding(int seat) => _resolve().Pingding(seat);
    internal void PublishShenSunCeGift(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishShenSunCeGift(f);
    internal bool ResumeShenSunCe(long id) => _resolve().ResumeShenSunCe(id);
    internal void ReturnShenSunCeDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShenSunCeDraw(supplied, outcome);
    internal bool ReturnShenSunCeMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnShenSunCeMovement(f);
    internal bool ShenSunCeCannotRespond(int source, int responder, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets) => _resolve().ShenSunCeCannotRespond(source, responder, kind, targets);
    internal bool ShenSunCeCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().ShenSunCeCardDistance(q);
    internal void ShenSunCeChangeMaximum(global::CardGame.Core.ProgramSkillFrame f, int seat, int delta) => _resolve().ShenSunCeChangeMaximum(f, seat, delta);
    internal bool ShenSunCeDistance(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target) => _resolve().ShenSunCeDistance(actor, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> ShenSunCeGiftChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ShenSunCeGiftChoices(f);
    internal bool ShenSunCeSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().ShenSunCeSlashDistance(q);
    internal bool ShenSunCeTriggerLegal(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().ShenSunCeTriggerLegal(c, t, w);
}
