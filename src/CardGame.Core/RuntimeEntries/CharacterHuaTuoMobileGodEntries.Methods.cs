#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoMobileGodEntries
{
    internal void AnswerMobileHt(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMobileHt(choice);
    internal void ApplyMobileHtAnimal(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ApplyMobileHtAnimal(f);
    internal void AssertMobileHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileHt(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileHt(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileHt(supplied, op);
    internal void BeginMobileHtEntryBenefit(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMobileHtEntryBenefit(f);
    internal bool CanActivateMobileHt(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().CanActivateMobileHt(gate);
    internal bool CanTriggerMobileHt(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanTriggerMobileHt(gate);
    internal global::CardGame.Core.PromptChoice ChooseAdvanceMobileHt(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAdvanceMobileHt(p, f);
    internal global::CardGame.Core.PromptChoice ChooseDepositMobileHt(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseDepositMobileHt(p, f);
    internal global::CardGame.Core.PromptChoice ChooseRecoverMobileHt(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseRecoverMobileHt(p, f);
    internal global::CardGame.Core.PromptChoice ChooseTeachMobileHt(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseTeachMobileHt(p, f);
    internal void ClearMobileHtDeerJudgments(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ClearMobileHtDeerJudgments(f);
    internal bool ContinueIssuedMobileHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedMobileHt(f);
    internal void ContinueMobileHt(long id) => _resolve().ContinueMobileHt(id);
    internal bool DrainMobileHt(long id) => _resolve().DrainMobileHt(id);
    internal void EndMobileHtCourse(long id, long frameId) => _resolve().EndMobileHtCourse(id, frameId);
    internal void FinishMobileHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileHt(f);
    internal void GrantMobileHtCourseSkill(global::CardGame.Core.MobileWulingTaughtEvent course, string skill) => _resolve().GrantMobileHtCourseSkill(course, skill);
    internal global::CardGame.Core.PromptChoice MobileHtChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string label, int chooser, int? target = default, int? card = default) => _resolve().MobileHtChoice(f, option, label, chooser, target, card);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileHtChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileHtChoices(f);
    internal global::CardGame.Core.MobileWulingTaughtEvent? MobileHtCourse(int seat) => _resolve().MobileHtCourse(seat);
    internal int MobileHtCourseIndex(long id) => _resolve().MobileHtCourseIndex(id);
    internal global::CardGame.Core.MobileWulingAnimal[] MobileHtCourseOrder(long id) => _resolve().MobileHtCourseOrder(id);
    internal global::CardGame.Core.DiscardRecoveryEntity[] MobileHtDiscarded(int owner) => _resolve().MobileHtDiscarded(owner);
    internal bool MobileHtMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MobileHtMovementProof(f, e, p);
    internal void MobileHtRecoverOne(global::CardGame.Core.ProgramSkillFrame f, int seat) => _resolve().MobileHtRecoverOne(f, seat);
    internal bool MobileHtSpirit(int seat, global::CardGame.Core.MobileWulingAnimal animal) => _resolve().MobileHtSpirit(seat, animal);
    internal int[] MobileHtTeachingTargets() => _resolve().MobileHtTeachingTargets();
    internal int MobileHtTigerDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MobileHtTigerDamage(attack);
    internal void ObserveMobileHtDeath(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMobileHtDeath(payload);
    internal void PublishMobileHt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileHt(f);
    internal bool ResumeMobileHt(long id) => _resolve().ResumeMobileHt(id);
    internal void ReturnMobileHtAdvanceDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileHtAdvanceDraw(f, outcome);
    internal void ReturnMobileHtCrane(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileHtCrane(supplied, outcome);
    internal bool ReturnMobileHtMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileHtMovement(f);
    internal void ReturnMobileHtTeachDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnMobileHtTeachDraw(f, outcome);
    internal void RevokeMobileHtCourseSkill(global::CardGame.Core.MobileWulingTaughtEvent course, string skill) => _resolve().RevokeMobileHtCourseSkill(course, skill);
    internal void SetMobileHtMarker(long frameId, int target, int teacher, global::CardGame.Core.PlayerMarkerKind marker, int count) => _resolve().SetMobileHtMarker(frameId, target, teacher, marker, count);
}
