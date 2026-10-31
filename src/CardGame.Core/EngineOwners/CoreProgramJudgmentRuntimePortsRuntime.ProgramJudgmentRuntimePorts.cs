#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramJudgmentRuntimePortsRuntime
{
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsLishiJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhongHuiAnniversaryGodOperations.IsLishiJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsShenZfJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhangFeiAnniversaryGodOperations.IsShenZfJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsShenGuoJiaJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterGuoJiaMobileGodOperations.IsShenGuoJiaJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsMouGjJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterGuoJiaMouOperations.IsMouGjJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsSourceHuojieJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _coreSourceHuojieOperations.IsSourceHuojieJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsNingwuJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhouQunOlOperations.IsNingwuJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsBinghuoJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterMaYuanYiOlOperations.IsBinghuoJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsWeifuJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterQianZhaoOlOperations.IsWeifuJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsZhangJiJudgment(JudgmentFrame judgment, ProgramSkillFrame frame) => _characterZhangJiOlOperations.IsZhangJiJudgment(judgment, frame);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginLishiJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhongHuiAnniversaryGodOperations.CanBeginLishiJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginShenZfJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhangFeiAnniversaryGodOperations.CanBeginShenZfJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginShenGuoJiaJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterGuoJiaMobileGodOperations.CanBeginShenGuoJiaJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsWushuangLightningJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _coreWushuangCombatProgramsOperations.IsWushuangLightningJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.MouGjJudgmentRequest(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterGuoJiaMouOperations.MouGjJudgmentRequest(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginSourceHuojieJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _coreSourceHuojieOperations.CanBeginSourceHuojieJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginNingwuJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhouQunOlOperations.CanBeginNingwuJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginBinghuoJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterMaYuanYiOlOperations.CanBeginBinghuoJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.MatchesWeifuJudgmentInstruction(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterQianZhaoOlOperations.MatchesWeifuJudgmentInstruction(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.CanBeginZhangJiJudgment(ProgramSkillFrame frame, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _characterZhangJiOlOperations.CanBeginZhangJiJudgment(frame, target, reason, bind, visibility);
    bool global::CardGame.Core.IProgramJudgmentNativeProofs.IsDamageJudgmentInstructionTarget(ProgramSkillFrame frame, int target) => _coreDamageJudgmentSuitPaymentsOperations.IsDamageJudgmentInstructionTarget(frame, target);
    int global::CardGame.Core.IProgramJudgmentNativeProofs.Participant(ProgramSkillFrame frame, ProgramParticipantReference reference) => _coreProgramCardPaymentsOperations.ResolveProgramParticipant(frame, reference);
    void global::CardGame.Core.IProgramJudgmentProducer.Begin(long programFrameId, int targetSeat, string reason, CardKind? sourceCard, int sourceSeat, string resultBind, SkillProgramCardSetVisibility visibility) => _ = _coreNativeJudgmentOperations.BeginJudgment(attack: _coreActiveNativeAttackQueriesOperations.ActiveCardAttack, targetSeat, reason, programFrameId, sourceCard: sourceCard, JudgmentContinuationKind.ProgramSkill, sourceSeat: sourceSeat, programResultBind: resultBind, programResultVisibility: visibility);
}
