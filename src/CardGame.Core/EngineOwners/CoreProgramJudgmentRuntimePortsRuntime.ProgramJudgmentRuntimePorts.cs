#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramJudgmentRuntimePortsRuntime
{
    bool IProgramJudgmentNativeProofs.IsLishiJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhongHuiAnniversaryGodOperations.IsLishiJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsShenZfJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhangFeiAnniversaryGodOperations.IsShenZfJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsShenGuoJiaJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterGuoJiaMobileGodOperations.IsShenGuoJiaJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsMouGjJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterGuoJiaMouOperations.IsMouGjJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsSourceHuojieJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _coreSourceHuojieOperations.IsSourceHuojieJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsNingwuJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhouQunOlOperations.IsNingwuJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsBinghuoJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterMaYuanYiOlOperations.IsBinghuoJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsWeifuJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterQianZhaoOlOperations.IsWeifuJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.IsZhangJiJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhangJiOlOperations.IsZhangJiJudgment(judgment, frame);
    bool IProgramJudgmentNativeProofs.CanBeginLishiJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhongHuiAnniversaryGodOperations.CanBeginLishiJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.CanBeginShenZfJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhangFeiAnniversaryGodOperations.CanBeginShenZfJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.CanBeginShenGuoJiaJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterGuoJiaMobileGodOperations.CanBeginShenGuoJiaJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.IsWushuangLightningJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _coreWushuangCombatProgramsOperations.IsWushuangLightningJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.MouGjJudgmentRequest(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterGuoJiaMouOperations.MouGjJudgmentRequest(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.CanBeginSourceHuojieJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _coreSourceHuojieOperations.CanBeginSourceHuojieJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.CanBeginNingwuJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhouQunOlOperations.CanBeginNingwuJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.CanBeginBinghuoJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterMaYuanYiOlOperations.CanBeginBinghuoJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.MatchesWeifuJudgmentInstruction(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterQianZhaoOlOperations.MatchesWeifuJudgmentInstruction(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.CanBeginZhangJiJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhangJiOlOperations.CanBeginZhangJiJudgment(frame, target, reason, bind, visibility);
    bool IProgramJudgmentNativeProofs.IsDamageJudgmentInstructionTarget(ProgramSkillFrame frame, int target) => _coreDamageJudgmentSuitPaymentsOperations.IsDamageJudgmentInstructionTarget(frame, target);
    int IProgramJudgmentNativeProofs.Participant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    void IProgramJudgmentProducer.Begin(long programFrameId, int targetSeat, string reason, CardKind? sourceCard, int sourceSeat, string resultBind, SkillProgramCardSetVisibility visibility) => _ = _coreNativeJudgmentOperations.BeginJudgment(attack: _coreActiveNativeAttackQueriesOperations.ActiveCardAttack, targetSeat, reason, programFrameId, sourceCard: sourceCard, JudgmentContinuationKind.ProgramSkill, sourceSeat: sourceSeat, programResultBind: resultBind, programResultVisibility: visibility);
}
