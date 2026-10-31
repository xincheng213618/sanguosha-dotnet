#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaTuoMobileGodEntries
{
    internal void AnswerMobileHt(PromptChoice choice) => _resolve().AnswerMobileHt(choice);
    internal void ApplyMobileHtAnimal(ProgramSkillFrame f) => _resolve().ApplyMobileHtAnimal(f);
    internal SkillProgramStepOutcome BeginMobileHt(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileHt(supplied, op);
    internal void BeginMobileHtEntryBenefit(ProgramSkillFrame f) => _resolve().BeginMobileHtEntryBenefit(f);
    internal bool CanActivateMobileHt(ProgramActivationLegalityContext gate) => _resolve().CanActivateMobileHt(gate);
    internal bool CanTriggerMobileHt(ProgramTriggerLegalityContext gate) => _resolve().CanTriggerMobileHt(gate);
    internal PromptChoice ChooseAdvanceMobileHt(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAdvanceMobileHt(p, f);
    internal PromptChoice ChooseDepositMobileHt(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseDepositMobileHt(p, f);
    internal PromptChoice ChooseRecoverMobileHt(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseRecoverMobileHt(p, f);
    internal PromptChoice ChooseTeachMobileHt(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseTeachMobileHt(p, f);
    internal void ClearMobileHtDeerJudgments(ProgramSkillFrame f) => _resolve().ClearMobileHtDeerJudgments(f);
    internal bool ContinueIssuedMobileHt(ProgramSkillFrame f) => _resolve().ContinueIssuedMobileHt(f);
    internal void ContinueMobileHt(long id) => _resolve().ContinueMobileHt(id);
    internal bool DrainMobileHt(long id) => _resolve().DrainMobileHt(id);
    internal void EndMobileHtCourse(long id, long frameId) => _resolve().EndMobileHtCourse(id, frameId);
    internal void FinishMobileHt(ProgramSkillFrame f) => _resolve().FinishMobileHt(f);
    internal void GrantMobileHtCourseSkill(MobileWulingTaughtEvent course, string skill) => _resolve().GrantMobileHtCourseSkill(course, skill);
    internal PromptChoice MobileHtChoice(ProgramSkillFrame f, string option, string label, int chooser, int? target = default, int? card = default) => _resolve().MobileHtChoice(f, option, label, chooser, target, card);
    internal IReadOnlyList<PromptChoice> MobileHtChoices(ProgramSkillFrame f) => _resolve().MobileHtChoices(f);
    internal MobileWulingTaughtEvent? MobileHtCourse(int seat) => _resolve().MobileHtCourse(seat);
    internal int MobileHtCourseIndex(long id) => _resolve().MobileHtCourseIndex(id);
    internal MobileWulingAnimal[] MobileHtCourseOrder(long id) => _resolve().MobileHtCourseOrder(id);
    internal DiscardRecoveryEntity[] MobileHtDiscarded(int owner) => _resolve().MobileHtDiscarded(owner);
    internal void MobileHtRecoverOne(ProgramSkillFrame f, int seat) => _resolve().MobileHtRecoverOne(f, seat);
    internal bool MobileHtSpirit(int seat, MobileWulingAnimal animal) => _resolve().MobileHtSpirit(seat, animal);
    internal int[] MobileHtTeachingTargets() => _resolve().MobileHtTeachingTargets();
    internal int MobileHtTigerDamage(GameEngine.IDamageAttempt attack) => _resolve().MobileHtTigerDamage(attack);
    internal void ObserveMobileHtDeath(IGameEvent payload) => _resolve().ObserveMobileHtDeath(payload);
    internal void PublishMobileHt(ProgramSkillFrame f) => _resolve().PublishMobileHt(f);
    internal bool ResumeMobileHt(long id) => _resolve().ResumeMobileHt(id);
    internal void ReturnMobileHtAdvanceDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMobileHtAdvanceDraw(f, outcome);
    internal void ReturnMobileHtCrane(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnMobileHtCrane(supplied, outcome);
    internal bool ReturnMobileHtMovement(ProgramSkillFrame f) => _resolve().ReturnMobileHtMovement(f);
    internal void ReturnMobileHtTeachDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnMobileHtTeachDraw(f, outcome);
    internal void RevokeMobileHtCourseSkill(MobileWulingTaughtEvent course, string skill) => _resolve().RevokeMobileHtCourseSkill(course, skill);
    internal void SetMobileHtMarker(long frameId, int target, int teacher, PlayerMarkerKind marker, int count) => _resolve().SetMobileHtMarker(frameId, target, teacher, marker, count);
}
