#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramJudgmentRuntimePortsCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsNingwuJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool CanBeginNingwuJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int ResolveProgramParticipant(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramParticipantReference reference);
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterGuoJiaMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenGuoJiaJudgment(global::CardGame.Core.JudgmentFrame judgment, global::CardGame.Core.ProgramSkillFrame frame);
    bool CanBeginShenGuoJiaJudgment(global::CardGame.Core.ProgramSkillFrame frame, int targetSeat, string reason, string resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterQianZhaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWeifuJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool MatchesWeifuJudgmentInstruction(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCoreDamageJudgmentSuitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDamageJudgmentInstructionTarget(global::CardGame.Core.ProgramSkillFrame frame, int target);
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShenZfJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool CanBeginShenZfJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLishiJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool CanBeginLishiJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCoreNativeJudgmentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool? BeginJudgment(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, int targetSeat, string reason, long parentFrameId, global::CardGame.Core.CardKind? sourceCard, global::CardGame.Core.JudgmentContinuationKind continuation, global::CardGame.Core.Card? delayedCard = default, int? sourceSeat = default, string? programResultBind = default, global::CardGame.Core.SkillProgramCardSetVisibility? programResultVisibility = default, global::CardGame.Core.OriginalUseLightningAttachment? originalUseLightning = default, int? originalUseLightningExcludedCardId = default);
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterGuoJiaMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGjJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool MouGjJudgmentRequest(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCoreWushuangCombatProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWushuangLightningJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCoreSourceHuojieOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSourceHuojieJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool CanBeginSourceHuojieJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterMaYuanYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsBinghuoJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool CanBeginBinghuoJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}

internal interface ICoreProgramJudgmentRuntimePortsCharacterZhangJiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsZhangJiJudgment(global::CardGame.Core.JudgmentFrame j, global::CardGame.Core.ProgramSkillFrame f);
    bool CanBeginZhangJiJudgment(global::CardGame.Core.ProgramSkillFrame f, int targetSeat, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility);
}
